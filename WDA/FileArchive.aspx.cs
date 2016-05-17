using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WDA.Class;

namespace WDA
{
    public partial class FileArchive : GridViewUtility
    {
        #region Page_Load()
        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadPage(true);
            try
            {
                if (!IsPostBack)
                {
                    this.HiddenShowPanel.Value = "false";

                    this.GridView1.PageSize = Convert.ToInt32(this.GetSystem("PageSize"));
                }
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex);
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
                string wpinno = string.Empty;

                if (!string.IsNullOrEmpty(this.txtBarcodeValue.Text.Trim()))
                {
                    wpinno = this.txtBarcodeValue.Text.Trim().Replace(StringFormatException.Mode.Sql);
                }

                string userIP = this.Request.ServerVariables["REMOTE_ADDR"].ToString();

                //MODIFY BY RICHARD 20160407
                this.MonitorLog.LogMonitor(wpinno, this.UserInfo.UserName, this.UserInfo.RealName, userIP, Monitor.MSGID.WDA04, "文號查詢");
                #endregion
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex);
            }
        }
        #endregion

        protected void BtnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                this.InitSQL();
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message);
            }
        }

        #region ImageBtnEdit_Click()
        protected void ImageBtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow gridViewRow = (GridViewRow)((ImageButton)sender).NamingContainer;
            GridView gv = (GridView)gridViewRow.NamingContainer;

            gv.EditIndex = gridViewRow.RowIndex;
            gv.DataSource = ViewState[this.GridView1.ClientID];
            gv.DataBind();
        }
        #endregion

        #region ITImageListBtnChang_Click()
        protected void ITImageListBtnChang_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow gridViewRow = (GridViewRow)((ImageButton)sender).NamingContainer;

            int DataIndex = this.GridView1.PageSize * this.GridView1.PageIndex + gridViewRow.RowIndex;

            int result = 0;

            Hashtable ht = new Hashtable();

            string strSql = string.Empty, strWhere = string.Empty;
            try
            {
                    #region Update

                    //ht.Add("WpoutNo", gridViewRow.Cells[3].Text.Trim());
                    ht.Add("FileNo", ((TextBox)gridViewRow.Cells[0].FindControl("txtFileNo")).Text.Trim());
                    ht.Add("FileDate", ((TextBox)gridViewRow.Cells[0].FindControl("txtFileDate")).Text.Trim());
                    ht.Add("KeepYr", ((TextBox)gridViewRow.Cells[0].FindControl("txtKeepYr")).Text.Trim());
                    ht.Add("BoxNo", ((TextBox)gridViewRow.Cells[0].FindControl("txtBoxNo")).Text.Trim());
                    ht.Add("LastModifyUserID", UserInfo.UserID);
                    ht.Add("LastModifyTime", "SYSDATE");

                    this.DBConnTransac.GeneralSqlCmd.Command.CommandTimeout = 90;

                    //Wprec
                    strWhere = string.Format("And WpinNo = '{0}'\n", gridViewRow.Cells[2].Text.Trim());
                    strSql = this.Update.WprecEdit(ht, strWhere);
                    this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);
                    result = this.DBConnTransac.GeneralSqlCmd.ExecuteNonQuery(strSql);
                    if (result < 1)
                    {
                        this.ShowMessage("修改失敗"); return;
                    }

                    //Wptrans
                    strWhere = string.Format("And WpinNo = '{0}'\n", gridViewRow.Cells[2].Text.Trim());
                    strSql = this.Update.WptransEdit(this.UserInfo.RealName, strWhere);
                    this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);
                    result = this.DBConnTransac.GeneralSqlCmd.ExecuteNonQuery(strSql);
                    //if (result < 1)
                    //{
                    //    this.ShowMessage("修改失敗"); return;
                    //}

                    //BarcodeTable
                    strWhere = string.Format("And BarcodeValue = '{0}'", gridViewRow.Cells[2].Text.Trim());

                    strSql = this.Select.BarcodeTable(strWhere);

                    this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                    this.DBConn.GeneralSqlCmd.Command.CommandTimeout = 90;

                    string caseID = this.DBConn.GeneralSqlCmd.ExecuteByColumnName(strSql, "CaseID");

                    if (caseID.Length > 0)
                    {
                        strSql = this.Update.BarcodeTable(ht, strWhere);
                        this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);
                        result = this.DBConnTransac.GeneralSqlCmd.ExecuteNonQuery(strSql);

                        if (result <= 0)
                        {
                            this.ShowMessage("修改失敗"); return;
                        }
                    }
                    this.DBConnTransac.GeneralSqlCmd.Transaction.Commit();
                    this.ShowMessage("修改成功", MessageMode.INFO);

                    //ADD BY RICHARD 20160407 for Monitor
                    #region Monitor

                    string userIP = this.Request.ServerVariables["REMOTE_ADDR"].ToString();

                    this.MonitorLog.LogMonitor(gridViewRow.Cells[2].Text.Trim(), this.UserInfo.UserName, this.UserInfo.RealName, userIP, Monitor.MSGID.WDA04, "修改文號");
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

        #region Private Method

        #region DataBind()
        /// <summary>
        /// 資料繫結
        /// </summary>
        /// <param name="Anew">是否重新撈資料</param>
        /// <param name="LockPageNum">是否保留目前頁數</param>
        private void DataBind(bool Anew, bool LockPageNum)
        {
            string strSql = string.Empty, barWhere = string.Empty, wprecWhere = string.Empty, wptransWhere = string.Empty;

            DataTable dt = null; bool isWpnno = false;
            try
            {
                if (Anew)
                {
                    string barcodeValue = this.txtBarcodeValue.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim();
                    string wpoutNo = this.txtWpoutNo.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim();
                    string fileNo = this.txtFileNo.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim();
                    string fileDate = this.txtFileDate.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim();
                    string keepYr = this.txtKeepYr.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim();
                    string boxNoS = this.txtBoxNoS.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim();
                    string boxNoE = this.txtBoxNoE.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim();
                    string onFile = this.txtOnFile.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim();

                    barWhere += string.Format("And bt.FileNo is not null\n");
                    wprecWhere += string.Format("And wp.FileNo is not null\n");

                    //strWhere += string.Format("And bt.FileNo != ' '\n");

                    if (barcodeValue.Length > 0)
                    {
                        isWpnno = true;
                        barWhere += string.Format("And bt.BarcodeValue = '{0}'\n", barcodeValue);
                        wprecWhere += string.Format("And wp.wpinno = '{0}'\n", barcodeValue);
                        wptransWhere += string.Format("And wp.wpinno = '{0}'\n", barcodeValue);
                    }

                    if (wpoutNo.Length > 0)
                    {
                        barWhere += string.Format("And bt.WpoutNo = '{0}'\n", wpoutNo);
                        wprecWhere += string.Format("And wp.WpoutNo = '{0}'\n", wpoutNo);
                        wptransWhere += string.Format("And wp.WpoutNo = '{0}'\n", wpoutNo);
                    }

                    if (fileNo.Length > 0)
                    {
                        barWhere += string.Format("And bt.FileNo Like '{0}%'\n", fileNo);
                        wprecWhere += string.Format("And wp.FileNo Like '{0}%'\n", fileNo);
                        wptransWhere += string.Format("And wp.FileNo Like '{0}%'\n", fileNo);
                    }

                    if (fileDate.Length > 0) 
                    {
                        barWhere += string.Format("And bt.FileDate = '{0}'\n", fileDate.Replace("/", string.Empty));
                        wprecWhere += string.Format("And wp.FileDate = '{0}'\n", fileDate.Replace("/", string.Empty));
                        wptransWhere += string.Format("And wp.FileDate = '{0}'\n", fileDate.Replace("/", string.Empty)); 
                    }

                    if (keepYr.Length > 0)
                    {
                        barWhere += string.Format("And bt.KeepYr = '{0}'\n", keepYr);
                        wprecWhere += string.Format("And wp.KeepYr = '{0}'\n", keepYr);
                        wptransWhere += string.Format("And wp.KeepYr = '{0}'\n", keepYr);
                    }

                    if (boxNoS.Length > 0 && boxNoE.Length > 0)
                    {
                        barWhere += string.Format("And bt.BoxNo >= '{0}' And bt.BoxNo <='{1}'\n", boxNoS,boxNoE);
                        wprecWhere += string.Format("And wp.BoxNo >= '{0}' And wp.BoxNo <='{1}'\n", boxNoS, boxNoE);
                        wptransWhere += string.Format("And wp.BoxNo >= '{0}' And wp.BoxNo <='{1}'\n", boxNoS, boxNoE);
                    }

                    if (onFile.Length > 0)
                    {
                        barWhere += string.Format("And bt.OnFile = N'{0}'\n", onFile);
                        wprecWhere += string.Format("And OnFile = N'{0}'\n", onFile);
                        wptransWhere += string.Format("And RECEIVER = N'{0}'\n", onFile);
                    }

                    if (isWpnno)
                    {
                        strSql = this.Select.FileArchiveCheck(wprecWhere, wptransWhere);

                        this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                        this.DBConn.GeneralSqlCmd.Command.CommandTimeout = 90;

                        dt = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                        if (dt.Rows.Count == 2)
                        {
                            strSql = this.Select.FileArchive2(wptransWhere);
                        }
                        else
                        {
                            wptransWhere += "And ROWNUM <=300";

                            strSql = this.Select.FileArchive(barWhere, wprecWhere, wptransWhere);
                        }
                    }
                    else
                    {
                        wptransWhere += "And ROWNUM <=300";

                        strSql = this.Select.FileArchive(barWhere, wprecWhere, wptransWhere);
                    }

                    this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                    this.DBConn.GeneralSqlCmd.Command.CommandTimeout = 90;

                    dt = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                    if (dt.Rows.Count == 0)
                    {
                        this.ShowMessage("目前查詢沒有任何資料", MessageMode.INFO);
                        ViewState[this.GridView1.ClientID] = dt;
                        this.HiddenShowPanel.Value = "false";
                        return;
                    }
                    else
                    {
                        ViewState[this.GridView1.ClientID] = dt;
                        this.HiddenShowPanel.Value = "true";
                    }
                }
                this.GridView1.DataBind((DataTable)ViewState[this.GridView1.ClientID], Anew, LockPageNum, this.lblTotalPage_GridView1, this.lblPage_GridView1, null);
            }
            catch (System.Exception ex) { this.ShowMessage(ex); }
            finally
            {
                if (dt != null) { dt.Dispose(); dt = null; }

                if (this.DBConn != null) { this.DBConn.Dispose(); this.DBConn = null; }
            }
        }
        #endregion

        #region InitSQL()
        private void InitSQL()
        {
            string strSql = string.Empty;
            string where = string.Empty, barWhere = string.Empty, wprecWhere = string.Empty, wptransWhere = string.Empty;
            DataTable dt = null; bool isWpnno = false;

            try
            {
                string barcodeValue = this.txtBarcodeValue.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim();
                string wpoutNo = this.txtWpoutNo.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim();
                string fileNo = this.txtFileNo.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim();
                string fileDate = this.txtFileDate.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim();
                string keepYr = this.txtKeepYr.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim();
                string boxNoS = this.txtBoxNoS.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim();
                string boxNoE = this.txtBoxNoE.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim();
                string onFile = this.txtOnFile.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim();

                barWhere += string.Format("And bt.FileNo is not null\n");
                wprecWhere += string.Format("And wp.FileNo is not null\n");

                //strWhere += string.Format("And bt.FileNo != ' '\n");

                if (barcodeValue.Length > 0)
                {
                    isWpnno = true;
                    barWhere += string.Format("And bt.BarcodeValue = '{0}'\n", barcodeValue);
                    wprecWhere += string.Format("And wp.wpinno = '{0}'\n", barcodeValue);
                    wptransWhere += string.Format("And wp.wpinno = '{0}'\n", barcodeValue);

                }

                if (wpoutNo.Length > 0)
                {
                    barWhere += string.Format("And bt.WpoutNo = '{0}'\n", wpoutNo);
                    wprecWhere += string.Format("And wp.WpoutNo = '{0}'\n", wpoutNo);
                    wptransWhere += string.Format("And wp.WpoutNo = '{0}'\n", wpoutNo);
                }

                if (fileNo.Length > 0)
                {
                    barWhere += string.Format("And bt.FileNo Like '{0}%'\n", fileNo);
                    wprecWhere += string.Format("And wp.FileNo Like '{0}%'\n", fileNo);
                    wptransWhere += string.Format("And wp.FileNo Like '{0}%'\n", fileNo);
                }

                if (fileDate.Length > 0)
                {
                    barWhere += string.Format("And bt.FileDate = '{0}'\n", fileDate.Replace("/", string.Empty));
                    wprecWhere += string.Format("And wp.FileDate = '{0}'\n", fileDate.Replace("/", string.Empty));
                    wptransWhere += string.Format("And wp.FileDate = '{0}'\n", fileDate.Replace("/", string.Empty)); 
                }

                if (keepYr.Length > 0)
                {
                    barWhere += string.Format("And bt.KeepYr = '{0}'\n", keepYr);
                    wprecWhere += string.Format("And wp.KeepYr = '{0}'\n", keepYr);
                    wptransWhere += string.Format("And wp.KeepYr = '{0}'\n", keepYr);
                }

                if (boxNoS.Length > 0 && boxNoE.Length > 0)
                {
                    barWhere += string.Format("And bt.BoxNo >= '{0}' And bt.BoxNo <='{1}'\n", boxNoS, boxNoE);
                    wprecWhere += string.Format("And wp.BoxNo >= '{0}' And wp.BoxNo <='{1}'\n", boxNoS, boxNoE);
                    wptransWhere += string.Format("And wp.BoxNo >= '{0}' And wp.BoxNo <='{1}'\n", boxNoS, boxNoE);

                }

                if (onFile.Length > 0)
                {
                    barWhere += string.Format("And bt.OnFile = N'{0}'\n", onFile);
                    wprecWhere += string.Format("And OnFile = N'{0}'\n", onFile);
                    wptransWhere += string.Format("And RECEIVER = N'{0}'\n", onFile);

                }

                if (isWpnno)
                {
                    strSql = this.Select.FileArchiveCheck(wprecWhere, wptransWhere);

                    this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                    this.DBConn.GeneralSqlCmd.Command.CommandTimeout = 90;

                    dt = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                    if (dt.Rows.Count == 2)
                    {
                        strSql = this.Select.FileArchive2(wptransWhere);
                    }
                    else
                    {
                        strSql = this.Select.FileArchive(barWhere, wprecWhere, wptransWhere);
                    }
                }
                else
                {
                    strSql = this.Select.FileArchive(barWhere, wprecWhere, wptransWhere);
                }

                this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                this.WriteLog(global::Log.Mode.LogMode.DEBUG, "FileArchive：Session");

                Session["FileArchive"] = strSql;
            }
            catch (System.Exception ex) { this.ShowMessage(ex.Message); }
            finally
            {
                this.DBConn.Dispose(); this.DBConn = null;
            }
        }
        #endregion

        #endregion

        #region GridView Events

        #region GridView1_RowCommand()
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView gv = (GridView)sender;
            try
            {
                if (e.CommandName != "Stop") return;

                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;

                int result = 0;

                string strSql = string.Empty, strWhere = string.Empty;

                string barcodeValue = gv.Rows[rowIndex].Cells[2].Text.Trim();

                if (e.CommandName == "Stop")
                {
                        #region Delete

                        this.DBConn.GeneralSqlCmd.Command.CommandTimeout = 90;

                        //Wprec
                        strWhere = string.Format("And WpinNo = '{0}'\n", barcodeValue);
                        strSql = this.Update.WprecDelete(strWhere);
                        this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);
                        result = this.DBConn.GeneralSqlCmd.ExecuteNonQuery(strSql);
                        if (result < 1)
                        {
                            this.ShowMessage("修改失敗"); return;
                        }

                        //Wptrans
                        strWhere = string.Format("And WpinNo = '{0}'\n", barcodeValue);
                        strSql = this.Delete.WptransDelete(barcodeValue);
                        this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);
                        result = this.DBConn.GeneralSqlCmd.ExecuteNonQuery(strSql);

                        //BarcodeTable
                        strWhere = string.Format("And BarcodeValue = '{0}'", barcodeValue);

                        strSql = this.Select.BarcodeTable(strWhere);

                        this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                        this.DBConn.GeneralSqlCmd.Command.CommandTimeout = 90;

                        string caseID = this.DBConn.GeneralSqlCmd.ExecuteByColumnName(strSql, "CaseID");

                        if (caseID.Length > 0)
                        {
                            strSql = this.Update.BarcodeTable(strWhere);
                            this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);
                            result = this.DBConn.GeneralSqlCmd.ExecuteNonQuery(strSql);
                            if (result < 1)
                            {
                                this.ShowMessage("刪除失敗");
                            }
                        }

                        this.ShowMessage("刪除成功", MessageMode.INFO);
                        this.DataBind(true, true);

                        //ADD BY RICHARD 20160407 for Monitor
                        #region Monitor

                        string userIP = this.Request.ServerVariables["REMOTE_ADDR"].ToString();

                        this.MonitorLog.LogMonitor(barcodeValue, this.UserInfo.UserName, this.UserInfo.RealName, userIP, Monitor.MSGID.WDA04, "刪除文號");
                        #endregion

                        #endregion
                }
            }
            catch (System.Exception ex)
            {
                this.ShowMessage(ex);
            }
            finally
            {
                if (this.DBConn != null)
                {
                    this.DBConn.Dispose(); this.DBConn = null;
                }
            }
        }
        #endregion

        #region GridView1_RowDataBound()
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.RowDataBound();

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton btnDelete = (ImageButton)e.Row.Cells[1].FindControl("ImageBtnDelete");

                this.Confirm(btnDelete, ConfirmMode.Delete);

                switch (e.Row.Cells[9].Text)
                {
                    case "1": e.Row.Cells[9].Text = "紙本"; 
                        break;
                    case "2": e.Row.Cells[9].Text = "電子"; 
                        break;
                    case "99": e.Row.Cells[9].Text = "其它";
                        break;
                }

                //ADD BY RICHARD 20160407 for 歸檔資料（檔號、日期、卷宗號及文件類型）編輯僅能由原始歸檔者進行操作
                if (!UserInfo.RealName.Equals(e.Row.Cells[8].Text.Trim()))
                {
                    e.Row.Cells[0].Text = "";
                    e.Row.Cells[1].Text = "";
                }


                e.Row.Cells[10].Text = string.IsNullOrEmpty(e.Row.Cells[10].Text.Replace(StringFormatException.Mode.Nbsp)) ? "無" : "有";
            }
        }
        #endregion

        #region GridView1_PageIndexChanging()
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            e.PageIndexChanging(sender);

            this.DataBind(false, false);
        }
        #endregion

        #region GridView1_Sorting()
        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            e.Sorting(sender);

            this.DataBind(false, true);
        }
        #endregion

        #region GridView1_RowCreated()
        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            //e.RowVisible(sender, new int[] { 1 });
        } 
        #endregion

        #endregion
    }
}