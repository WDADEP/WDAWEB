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
    public partial class VisaExtension : PageUtility
    {
        #region Page_Load()
        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadPage(true);
            try
            {
                if (!IsPostBack)
                {
                    this.DataBind(true, false);

                    this.GridView1.PageSize = Convert.ToInt32(this.GetSystem("PageSize"));
                }
            }
            catch (Exception ex) { this.ShowMessage(ex); }
        }
        #endregion

        #region BtnOK_Click()
        protected void BtnOK_Click(object sender, EventArgs e)
        {
            string strSql = string.Empty, where = string.Empty; string extensiondate = string.Empty;

            int result = 0;
            int extensioncount = 0;
            try
            {
                for (int i = 0; i < this.GridView1.Rows.Count; i++)
                {
                    string WpinNo = this.GridView1.Rows[i].Cells[1].Text.Trim();
                    //MODIFY BY RICHARD 20160418 4->11
                    string RECEIVER = this.GridView1.Rows[i].Cells[11].Text.Trim();
                    string TRANST = DateTime.Parse(this.GridView1.Rows[i].Cells[10].Text.Trim()).ToString("yyyy/MM/dd tt hh:mm:ss");
                    //REMARK BY RICHARD
                    //Label lblTranst = (Label)this.GridView1.Rows[i].Cells[3].FindControl("LblTranst");
                    //ADD BY RICHARD 20160418
                    Label lblRedate = (Label)this.GridView1.Rows[i].Cells[6].FindControl("LblReDate");
                    string count = this.GridView1.Rows[i].Cells[15].Text.Trim();
                    string kind = this.GridView1.Rows[i].Cells[16].Text.Trim();
                    string viewtype = this.GridView1.Rows[i].Cells[17].Text.Trim();
                    string prtflag = ((RadioButtonList)this.GridView1.Rows[i].Cells[0].FindControl("rBtnListApprove")).SelectedValue;

                    Hashtable ht = new Hashtable();

                    where = string.Format("And  WpinNo ='{0}' And RECEIVER='{1}' And to_char(TRANST,'YYYY/MM/DD AM HH:MI:SS') ='{2}'", WpinNo, RECEIVER, TRANST);

                    if (!string.IsNullOrEmpty(prtflag))
                    {
                        if (prtflag == "D")
                        {
                            //if (viewtype == "1")
                            //{
                            //    switch (kind)
                            //    {
                            //        case "1":
                            //            extensiondate = DateTime.Parse(lblTranst.Text).AddDays(9).ToString("yyyy/MM/dd HH:mm:ss");
                            //            break;
                            //        case "2":
                            //            extensiondate = DateTime.Parse(lblTranst.Text).AddDays(9).ToString("yyyy/MM/dd HH:mm:ss");
                            //            break;
                            //        case "3":
                            //            extensiondate = DateTime.Parse(lblTranst.Text).AddDays(126).ToString("yyyy/MM/dd HH:mm:ss");
                            //            break;
                            //        default: break;
                            //    }
                            //}
                            //else if (viewtype == "2")
                            //{
                            //    extensiondate = DateTime.Parse(lblTranst.Text).AddDays(9).ToString("yyyy/MM/dd HH:mm:ss");
                            //}

                            extensioncount = Convert.ToInt32(count) + 1;
                            extensiondate = DateTime.Parse(lblRedate.Text).ToString("yyyy/MM/dd HH:mm:ss"); 

                            ht.Clear();
                            ht.Add("EXTEN", "D");
                            ht.Add("EXTENSIONDATE", extensiondate);
                            ht.Add("EXTENSIONCOUNT", extensioncount);
                            ht.Add("VISAEXTENSIONDATE", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                            strSql = this.Update.wpborrowByVisa(ht, where);

                            this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                            result = this.DBConnTransac.GeneralSqlCmd.ExecuteNonQuery(strSql);

                            if (result < 1)
                            {
                                this.ShowMessage("簽准展期失敗"); return;
                            }
                        }
                        else if (prtflag == "Z")
                        {
                            ht.Clear();
                            ht.Add("EXTEN", "Z");

                            strSql = this.Update.wpborrowByVisaNo(ht, where);

                            this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                            result = this.DBConnTransac.GeneralSqlCmd.ExecuteNonQuery(strSql);

                            if (result < 1)
                            {
                                this.ShowMessage("取消展期失敗"); return;
                            }
                        }
                    }

                    #region Monitor

                    string userIP = this.Request.ServerVariables["REMOTE_ADDR"].ToString();

                    this.MonitorLog.LogMonitor(WpinNo, this.UserInfo.UserName, this.UserInfo.RealName, userIP, Monitor.MSGID.WDA15, string.Empty);
                    #endregion
                }

                if (result >= 1)
                {
                    this.DBConnTransac.GeneralSqlCmd.Transaction.Commit();

                    this.ShowMessage("簽核完成", MessageMode.INFO);
                }
                this.DataBind(true, true);
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
                if (this.DBConnTransac != null)
                {
                    this.DBConnTransac.Dispose(); this.DBConnTransac = null;
                }
            }
        }
        #endregion

        #region Private Method

        #region DataBind()
        /// <summary>
        /// 資料繫結
        /// </summary>
        /// <param name="Anew">是否重新撈資料</param>
        /// <param name="LockPageNum">是否保留目前頁數</param>
        public void DataBind(bool Anew, bool LockPageNum)
        {
            string strSql = string.Empty, where = string.Empty;

            DataTable dt = null;
            try
            {
                if (Anew)
                {
                    where = string.Format("And APPROVEUSERID='{0}'", this.UserInfo.UserID);

                    strSql = this.Select.GetAlsoFileByVisa(where);

                    this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                    this.DBConn.GeneralSqlCmd.Command.CommandTimeout = 90;

                    dt = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                    if (dt.Rows.Count == 0)
                    {
                        this.GridView1.DataBind();
                        this.BtnOK.Visible = false;
                        return;
                    }
                    else
                    {
                        this.BtnOK.Visible = true;
                    }

                    ViewState[this.GridView1.ClientID] = dt;
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

        #endregion

        #region GridView1_PageIndexChanging()
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            e.PageIndexChanging(sender);

            this.DataBind(false, false);
        }
        #endregion

        #region GridView1_RowCreated()
        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            e.RowVisible(sender, new int[] { 10, 11, 12, 13, 14, 15, 16, 17, 18 });
        }
        #endregion

        #region GridView1_RowDataBound()
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.RowDataBound();

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblexten = (Label)e.Row.Cells[7].FindControl("LblExten");
                Label lblTranst = (Label)e.Row.Cells[3].FindControl("LblTranst");
                Label lblredate = (Label)e.Row.Cells[6].FindControl("LblReDate");
                Label lblextenredate = (Label)e.Row.Cells[8].FindControl("LblExtenReDate");

                if (e.Row.Cells[15].Text == "0")
                {
                    lblexten.Text = "否";

                    lblTranst.Text = e.Row.Cells[10].Text;

                    if (e.Row.Cells[17].Text == "1")
                    {
                        switch (e.Row.Cells[16].Text)
                        {
                            //#計算一般案件預約歸檔新增起 7日,展期再加7日
                            //#計算法制案件預約歸檔新增起14日,展期再加7日
                            //#計算行政案件預約歸檔新增起365日,展期再加90日
                            //MODIFY BY RICHARD 20160418
                            case "1":
                                lblredate.Text = DateTime.Parse(e.Row.Cells[18].Text).AddDays(9).ToString("yyyy/MM/dd HH:mm:ss");
                                lblextenredate.Text = DateTime.Parse(e.Row.Cells[18].Text).AddDays(18).ToString("yyyy/MM/dd HH:mm:ss");
                                break;
                            case "2":
                                lblredate.Text = DateTime.Parse(e.Row.Cells[18].Text).AddDays(18).ToString("yyyy/MM/dd HH:mm:ss");
                                lblextenredate.Text = DateTime.Parse(e.Row.Cells[18].Text).AddDays(27).ToString("yyyy/MM/dd HH:mm:ss");
                                break;
                            case "3":
                                lblredate.Text = DateTime.Parse(e.Row.Cells[18].Text).AddDays(485).ToString("yyyy/MM/dd HH:mm:ss");
                                lblextenredate.Text = DateTime.Parse(e.Row.Cells[18].Text).AddDays(611).ToString("yyyy/MM/dd HH:mm:ss");
                                break;
                            default: break;
                        }
                    }
                    else if (e.Row.Cells[17].Text == "2")
                    {
                        lblredate.Text = DateTime.Parse(e.Row.Cells[10].Text).AddDays(9).ToString("yyyy/MM/dd HH:mm:ss");
                        lblextenredate.Text = DateTime.Parse(e.Row.Cells[10].Text).AddDays(18).ToString("yyyy/MM/dd HH:mm:ss");
                    }
                }
                else
                {
                    lblexten.Text = "是";

                    lblTranst.Text = e.Row.Cells[14].Text;

                    if (e.Row.Cells[17].Text == "1")
                    {
                        switch (e.Row.Cells[16].Text)
                        {
                            case "1":
                                lblredate.Text = DateTime.Parse(e.Row.Cells[14].Text).AddDays(9).ToString("yyyy/MM/dd HH:mm:ss");
                                lblextenredate.Text = DateTime.Parse(e.Row.Cells[14].Text).AddDays(18).ToString("yyyy/MM/dd HH:mm:ss");
                                break;
                            case "2":
                                lblredate.Text = DateTime.Parse(e.Row.Cells[14].Text).AddDays(9).ToString("yyyy/MM/dd HH:mm:ss");
                                lblextenredate.Text = DateTime.Parse(e.Row.Cells[14].Text).AddDays(18).ToString("yyyy/MM/dd HH:mm:ss");
                                break;
                            case "3":
                                lblredate.Text = DateTime.Parse(e.Row.Cells[14].Text).AddDays(126).ToString("yyyy/MM/dd HH:mm:ss");
                                lblextenredate.Text = DateTime.Parse(e.Row.Cells[14].Text).AddDays(252).ToString("yyyy/MM/dd HH:mm:ss");
                                break;
                            default: break;
                        }
                    }
                    else if (e.Row.Cells[17].Text == "2")
                    {
                        lblredate.Text = DateTime.Parse(e.Row.Cells[14].Text).AddDays(9).ToString("yyyy/MM/dd HH:mm:ss");
                        lblextenredate.Text = DateTime.Parse(e.Row.Cells[14].Text).AddDays(18).ToString("yyyy/MM/dd HH:mm:ss");
                    }

                }

            }
        }
        #endregion

        #region GridView1_Sorting()
        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            e.Sorting(sender);
            this.DataBind(false, true);
        }
        #endregion
    }
}