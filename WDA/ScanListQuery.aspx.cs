using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WDA.Class;

namespace WDA
{
    public partial class ScanListQuery : PageUtility
    {
        #region CaseID
        protected String CaseID
        {
            get
            {
                if (ViewState["CaseID"] == null)
                    return string.Empty;
                else
                    return (String)(ViewState["CaseID"]);
            }

            set { ViewState["CaseID"] = value; }
        } 
        #endregion

        #region BARCODEVALUE
        protected String BARCODEVALUE
        {
            get
            {
                if (ViewState["BARCODEVALUE"] == null)
                    return string.Empty;
                else
                    return (String)(ViewState["BARCODEVALUE"]);
            }
            set
            {
                ViewState["BARCODEVALUE"] = value;
            }
        }
        #endregion

        #region ColorInt
        protected int ColorInt
        {
            get
            {
                if (ViewState["ColorInt"] == null)
                    return 0;
                else
                    return (int)(ViewState["ColorInt"]);
            }

            set { ViewState["ColorInt"] = value; }
        }
        #endregion

        #region Page_Load()
        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadPage(true);
            //ADD BY RICHARD 20160511
            this.Form.DefaultButton = this.Button1.UniqueID;

            try
            {
                if (!IsPostBack)
                {
                    string sScript = "$('#divPanel').hide();";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ScanListQuery", sScript, true);

                    this.GridView1.PageSize = Convert.ToInt32(this.GetSystem("PageSize"));
                }
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex);
            }
        }
        #endregion

        #region DataBind()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="MyDataGrid">DataGrid Object</param>
        /// <param name="Anew">是否重新撈資料</param>
        /// <param name="LockPageNum">是否保留目前頁數</param>
        /// <param name="SelectIndex">索引類型</param>
        public void DataBind(bool Anew, bool LockPageNum)
        {
            try
            {
                string strSql = string.Empty;
                string where = string.Empty;

                if (Anew)
                {
                    DataTable dt = null;

                    OleDbCommand command = (OleDbCommand)this.DBConn.GeneralSqlCmd.Command;

                    command.Parameters.Clear();

                    //ADD BY RICHARD 20160330 for ADD WPINNO QUERY
                    if (!string.IsNullOrEmpty(this.TxtWpinno.Text.Trim()))
                    {
                        string strWpinno = this.TxtWpinno.Text.Trim();
                        command.Parameters.Add(new OleDbParameter("BARCODEVALUE", OleDbType.VarChar)).Value = strWpinno;
                        where += string.Format(" And bt.BARCODEVALUE =:BARCODEVALUE");
                    }
                    if (!string.IsNullOrEmpty(this.TxtRealName.Text.Trim()))
                    {
                        string realName = this.TxtRealName.Text.Trim();

                        command.Parameters.Add(new OleDbParameter("RealName", OleDbType.VarChar)).Value = realName;

                        where += string.Format(" And ut.RealName =:RealName");
                    }
                    if (!string.IsNullOrEmpty(txtScanCreateTime.Text))
                    {
                        string startTime = this.txtScanCreateTime.Text.Trim().Replace(StringFormatException.Mode.Sql);//開始日期
                        string endTime = this.txtScanEndTime.Text.Trim().Replace(StringFormatException.Mode.Sql);//結束日期

                        //endTime = DateTime.Parse(endTime).AddDays(1).AddSeconds(-1).ToString("yyyy/MM/dd HH:mm:ss");

                        where += string.Format(" And bt.CreateTime Between  TO_DATE('{0}', 'YYYY/MM/DD HH24:MI:SS') And TO_DATE('{1}', 'YYYY/MM/DD HH24:MI:SS')", startTime, endTime);
                    }

                    strSql = this.Select.ScanListQuery(where);

                    this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                    Session["ScanListQuery"] = strSql;

                    this.DBConn.GeneralSqlCmd.Command.CommandTimeout = 90;

                    dt = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                    if (dt.Rows.Count == 0)
                    {
                        this.ShowMessage("目前查詢沒有任何資料", MessageMode.INFO);

                        return;
                    }
                    else
                    {
                        ViewState[this.GridView1.ClientID] = dt;

                        dt.Dispose(); dt = null;
                    }
                }

                this.GridView1.DataBind((DataTable)ViewState[this.GridView1.ClientID], Anew, LockPageNum);
            }
            catch (System.Exception ex) { this.ShowMessage(ex); }
            finally
            {
                this.DBConn.Dispose(); this.DBConn = null;
            }
        }
        #endregion

        #region BtnOK_Click()
        protected void BtnOK_Click(object sender, EventArgs e)
        {
            try
            {
                this.DataBind(true, false);

                #region Monitor
                string userIP = this.Request.ServerVariables["REMOTE_ADDR"].ToString();

                this.MonitorLog.LogMonitor(string.Empty, this.UserInfo.UserName, this.UserInfo.RealName, userIP, Monitor.MSGID.WDA02, string.Empty); 
                #endregion
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex);
            }
           
        }
        #endregion

        #region BtnClear_Click()
        protected void BtnClear_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(this.Request.Url.AbsoluteUri);
        }
        #endregion

        #region ITImageListBtnChang_Click()
        // Modified by Luke (add Select region)
        protected void ITImageListBtnChang_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow gridViewRow = (GridViewRow)((ImageButton)sender).NamingContainer;

            int DataIndex = this.GridView1.PageSize * this.GridView1.PageIndex + gridViewRow.RowIndex;

            string sNewBarcodeValue = ((TextBox)gridViewRow.Cells[0].FindControl("txtBARCODEVALUE")).Text.Trim();

            int result = 0;

            Hashtable ht = new Hashtable();

            string strSql = string.Empty, strWhere = string.Empty;
            DataTable dt = null;
            try
            {
                #region Select

                if (string.IsNullOrEmpty(sNewBarcodeValue))
                {
                    this.ShowMessage("新收文文號欄位空白"); return;
                }

                this.DBConn.GeneralSqlCmd.Command.CommandTimeout = 90;

                //BarcodeTable
                strWhere = string.Format("And BarcodeValue = '{0}'", sNewBarcodeValue);
                strSql = this.Select.BarcodeTableBarcodeValues(strWhere);
                this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);
                dt = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                if ((dt != null) && (dt.Rows.Count > 0))
                {
                    this.ShowMessage("欲更新之收文文號已經存在"); return;
                }

                #endregion

                #region Update

                ht.Add("BARCODEVALUE", sNewBarcodeValue);

                this.DBConnTransac.GeneralSqlCmd.Command.CommandTimeout = 90;
                
                //BarcodeTable
                strWhere = string.Format("And BarcodeValue = '{0}' And CaseID = '{1}' ", gridViewRow.Cells[7].Text.Trim(), gridViewRow.Cells[2].Text.Trim());
                strSql = this.Update.BarcodeTableByBarCodeValue(ht, strWhere);
                this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);
                result = this.DBConnTransac.GeneralSqlCmd.ExecuteNonQuery(strSql);

                if (result <= 0)
                {
                    this.ShowMessage("修改失敗"); return;
                }

                this.DBConnTransac.GeneralSqlCmd.Transaction.Commit();
                this.ShowMessage("修改成功", MessageMode.INFO);

                //ADD BY RICHARD 20160407 
                #region Monitor
                string userIP = this.Request.ServerVariables["REMOTE_ADDR"].ToString();
                this.MonitorLog.LogMonitor(sNewBarcodeValue, this.UserInfo.UserName, this.UserInfo.RealName, userIP, Monitor.MSGID.WDA02, "修改收文文號");
                #endregion


                #endregion
            }
            catch (Exception ex)
            {
                try
                {
                    if (this.DBConnTransac.GeneralSqlCmd.Transaction != null) this.DBConnTransac.GeneralSqlCmd.Transaction.Rollback();
                }
                catch { }

                this.ShowMessage(ex);
            }
            finally
            {
                if (ht != null) { ht.Clear(); ht = null; }

                if (dt != null) { dt.Dispose(); dt = null; }

                if (this.DBConn != null) { this.DBConn.Dispose(); this.DBConn = null; }

                if (this.DBConnTransac != null) { this.DBConnTransac.Dispose(); this.DBConnTransac = null; }

                this.GridView1.EditIndex = -1;

                this.DataBind(true, true);
            }
        }
        #endregion

        #region ITImageListBtnAdd_Click()
        // Added by Luke
        protected void ITImageListBtnAdd_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow gridViewRow = (GridViewRow)((ImageButton)sender).NamingContainer;

            int DataIndex = this.GridView1.PageSize * this.GridView1.PageIndex + gridViewRow.RowIndex;

            string sNewBarcodeValue = ((TextBox)gridViewRow.Cells[0].FindControl("txtBARCODEVALUE")).Text.Trim();

            int result = 0;

            Hashtable ht = new Hashtable();

            string strSql = string.Empty, strWhere = string.Empty;
            DataTable dt = null;
            try
            {
                #region Select

                if (string.IsNullOrEmpty(sNewBarcodeValue))
                {
                    this.ShowMessage("新收文文號欄位空白"); return;
                }

                this.DBConn.GeneralSqlCmd.Command.CommandTimeout = 90;

                //BarcodeTable
                strWhere = string.Format("And BarcodeValue = '{0}'", sNewBarcodeValue);
                strSql = this.Select.BarcodeTableBarcodeValues(strWhere);
                this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);
                dt = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                if ((dt != null) && (dt.Rows.Count > 0))
                {
                    this.ShowMessage("欲新增之收文文號已經存在"); return;
                }

                #endregion

                #region Insert

                DateTime dTime = DateTime.Parse(gridViewRow.Cells[4].Text.Trim());

                ht.Add("CaseID", gridViewRow.Cells[2].Text.Trim());
                ht.Add("BarcodeValue", sNewBarcodeValue);
                ht.Add("CreateTime", dTime.ToString("yyyy/MM/dd HH:mm:ss"));
                ht.Add("CreateUserID", UserInfo.UserID);
                ht.Add("LastModifyTime", dTime.ToString("yyyy/MM/dd HH:mm:ss"));
                ht.Add("LastModifyUserID", UserInfo.UserID);
                ht.Add("WpoutNo", null);
                ht.Add("FileNo", null);
                ht.Add("FileDate", null);
                ht.Add("KeepYr", null);
                ht.Add("BoxNo", null);
                ht.Add("OnFile", null);

                this.DBConnTransac.GeneralSqlCmd.Command.CommandTimeout = 90;

                //BarcodeTable
                strSql = this.Insert.BarcodeTable(ht);
                this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);
                result = this.DBConnTransac.GeneralSqlCmd.ExecuteNonQuery(strSql);

                if (result > 0)
                {
                    this.DBConnTransac.GeneralSqlCmd.Transaction.Commit();
                    this.ShowMessage("新增成功", MessageMode.INFO);

                    //ADD BY RICHARD 20160407 
                    #region Monitor
                    string userIP = this.Request.ServerVariables["REMOTE_ADDR"].ToString();
                    this.MonitorLog.LogMonitor(sNewBarcodeValue, this.UserInfo.UserName, this.UserInfo.RealName, userIP, Monitor.MSGID.WDA02, "新增收文文號");
                    #endregion

                }
                else
                {
                    this.WriteLog(global::Log.Mode.LogMode.ERROR, "Insert BARCODETABLE Fail");
                }

                #endregion
            }
            catch (Exception ex)
            {
                try
                {
                    if (this.DBConnTransac.GeneralSqlCmd.Transaction != null) this.DBConnTransac.GeneralSqlCmd.Transaction.Rollback();
                }
                catch { }

                this.ShowMessage(ex);
            }
            finally
            {
                if (ht != null) { ht.Clear(); ht = null; }

                if (dt != null) { dt.Dispose(); dt = null; }

                if (this.DBConn != null) { this.DBConn.Dispose(); this.DBConn = null; }

                if (this.DBConnTransac != null) { this.DBConnTransac.Dispose(); this.DBConnTransac = null; }

                this.GridView1.EditIndex = -1;

                this.DataBind(true, true);
            }
        }
        #endregion

        #region ITImageBtnCancel_Click()
        protected void ITImageBtnCancel_Click(object sender, ImageClickEventArgs e)
        {
            this.GridView1.EditIndex = -1;

            this.DataBind(true, true);
        }
        #endregion

        #region ImageBtnEdit_Click
        // Modified by Luke
        protected void ImageBtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow gridViewRow = (GridViewRow)((ImageButton)sender).NamingContainer;
            GridView gv = (GridView)gridViewRow.NamingContainer;

            gv.EditIndex = gridViewRow.RowIndex;
            //this.DataBind(true, false);
            gv.DataSource = ViewState[this.GridView1.ClientID];
            gv.DataBind();
        }
        #endregion

        #region GridView Events

        #region GridView1_RowDataBound
        // Modified by Luke
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.RowDataBound();

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton btnDelete = (ImageButton)e.Row.Cells[1].FindControl("ImageBtnDelete");

                if (btnDelete != null)
                {
                    btnDelete.Attributes.Add("onclick", string.Format(@"javascript:return confirm('{0}')", "是否確定刪除？"));
                }

                //ADD BY RICHARD 20161129 for 掃描資料編輯與刪除僅能由原始掃描者進行操作
                if (!UserInfo.RealName.Equals(e.Row.Cells[5].Text.Trim()) )
                {
                    e.Row.Cells[0].Text = "";
                    e.Row.Cells[1].Text = "";
                }

            }
        }
        #endregion

        #region GridView1_RowCreated
        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            e.RowVisible(sender, new int[] { 7 });
        }
        #endregion

        #region GridView1_Sorting()
        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            e.Sorting(sender);

            this.DataBind(false, true);
        }
        #endregion

        #region GridView1_PageIndexChanging()
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            e.PageIndexChanging(sender);

            this.DataBind(false, false);
        }
        #endregion

        #region GridView1_RowCommand()
        // Added by Luke
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView gv = (GridView)sender;

            DataTable dt = null;
            DataTable dtWprec = null;
            try
            {
                if (e.CommandName != "Stop") return;

                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;

                string barcodeValue = gv.Rows[rowIndex].Cells[7].Text.Trim();
                string caseid = gv.Rows[rowIndex].Cells[2].Text.Trim();

                int result = 0;

                string strSql = string.Empty, strWhere = string.Empty;
                string strCond = string.Empty;
                string userIP = string.Empty;

                if (e.CommandName == "Stop")
                {
                    #region Select & delete

                    this.DBConn.GeneralSqlCmd.Command.CommandTimeout = 90;

                    //BarcodeTable
                    strWhere = string.Format("And CaseID = '{0}' ", caseid);
                    strSql = this.Select.BarcodeTableBarcodeValues(strWhere);
                    this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);
                    dt = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                    if ((dt != null) && (dt.Rows.Count > 0))
                    {
                        if (dt.Rows.Count == 1)
                        {
                            //this.ShowMessage("本收文文號僅有 1 筆，無法刪除"); return;
                            //ADD BY RICHARD 20161129 需於歸檔前才可以刪除
                            strWhere = string.Format(" WPINNO = '{0}'\n And FILENO is null And FILEDATE is null And KEEPYR is null And BOXNO is null  ", barcodeValue.Trim());

                            strSql = this.Select.WprecCheck(strWhere);

                            this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                            this.DBConn.GeneralSqlCmd.Command.CommandTimeout = 90;

                            dtWprec = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                            if (dtWprec != null && (dtWprec.Rows.Count > 0))
                            {
                                //delete barcodeTable
                                strSql = this.Delete.BarcodeTable(barcodeValue, caseid);
                                this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);
                                result = this.DBConnTransac.GeneralSqlCmd.ExecuteNonQuery(strSql);

                                strCond = string.Format(" AND CASEID='{0}' ", caseid);
                                //update casetable
                                strSql = this.Update.CaseStatus("99", "SYSDATE", strCond);
                                this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);
                                result = this.DBConnTransac.GeneralSqlCmd.ExecuteNonQuery(strSql);

                                if (result <= 0)
                                {
                                    this.ShowMessage("刪除失敗"); return;
                                }

                                this.DBConnTransac.GeneralSqlCmd.Transaction.Commit();
                                this.ShowMessage("刪除成功", MessageMode.INFO);

                                //ADD BY RICHARD 20161129
                                #region Monitor
                                userIP = this.Request.ServerVariables["REMOTE_ADDR"].ToString();
                                this.MonitorLog.LogMonitor(barcodeValue, this.UserInfo.UserName, this.UserInfo.RealName, userIP, Monitor.MSGID.WDA02, "刪除最後一筆收文文號");
                                #endregion

                            }
                            else
                            {
                                this.ShowMessage("此收文號已歸檔，無法刪除"); return;
                            }
                        }
                        else //比數大於1
                        {
                            this.DBConnTransac.GeneralSqlCmd.Command.CommandTimeout = 90;

                            //BarcodeTable
                            strSql = this.Delete.BarcodeTable(barcodeValue, caseid);
                            this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);
                            result = this.DBConnTransac.GeneralSqlCmd.ExecuteNonQuery(strSql);

                            if (result <= 0)
                            {
                                this.ShowMessage("刪除失敗"); return;
                            }

                            this.DBConnTransac.GeneralSqlCmd.Transaction.Commit();
                            this.ShowMessage("刪除成功", MessageMode.INFO);

                        }
                    }
                    else
                    {
                        this.ShowMessage("無此收文文號"); return;
                    }

                    #endregion


                    this.DataBind(true, true);

                    //ADD BY RICHARD 20160407 
                    #region Monitor
                    userIP = this.Request.ServerVariables["REMOTE_ADDR"].ToString();
                    this.MonitorLog.LogMonitor(barcodeValue, this.UserInfo.UserName, this.UserInfo.RealName, userIP, Monitor.MSGID.WDA02, "刪除收文文號");
                    #endregion

                }
            }
            catch (System.Exception ex)
            {
                try
                {
                    if (this.DBConnTransac.GeneralSqlCmd.Transaction != null) this.DBConnTransac.GeneralSqlCmd.Transaction.Rollback();
                }
                catch { }

                this.ShowMessage(ex);
            }
            finally
            {
                if (dt != null) { dt.Dispose(); dt = null; }

                if (dtWprec != null) { dtWprec.Dispose(); dtWprec = null; }

                if (this.DBConn != null) { this.DBConn.Dispose(); this.DBConn = null; }

                if (this.DBConnTransac != null) { this.DBConnTransac.Dispose(); this.DBConnTransac = null; }
            }
        }
        #endregion

        #endregion

    }
}