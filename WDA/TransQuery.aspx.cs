using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.OleDb;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WDA.Class;

namespace WDA
{
    public partial class TransQuery : GridViewUtility
    {
        #region Page_load
        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadPage(true);

            try
            {
                if (!IsPostBack)
                {
                    this.HiddenShowPanel.Value = "false";
                    this.GridView1.PageSize = Convert.ToInt32(this.GetSystem("PageSize"));
                    this.TxtWpinno.Focus();
                    this.TxtReceiver.Text = this.UserInfo.RealName;
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
            DataTable dt = null;
            try
            {
                string strSql = string.Empty;
                string where = string.Empty;

                if (Anew)
                {

                    if (!string.IsNullOrEmpty(this.TxtWpinno.Text.Trim()))
                    {
                        string wpinno = this.TxtWpinno.Text.Trim();


                        where += string.Format(" And WPINNO = '{0}'", wpinno);

                    }

                    if (!string.IsNullOrEmpty(this.TxtReceiver.Text.Trim()))
                    {
                        string realName = this.TxtReceiver.Text.Trim();

                        where += string.Format(" AND RECEIVER = '{0}'", realName);

                    }

                    if (!string.IsNullOrEmpty(this.txtScanCreateTime.Text.Trim()))
                    {
                        string startTime = this.txtScanCreateTime.Text.Trim().Replace(StringFormatException.Mode.Sql);//開始日期
                        string endTime = this.txtScanEndTime.Text.Trim().Replace(StringFormatException.Mode.Sql);//結束日期

                        where += string.Format(" AND TRANSTIME Between TO_DATE('{0}','YYYY/MM/DD HH24:MI:SS') And TO_DATE('{1}','YYYY/MM/DD HH24:MI:SS') ", startTime, endTime);
                    }

                    strSql = this.Select.Transtable(where);

                    this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                    Session["Transtable"] = strSql; // used for printing report

                    this.DBConn.GeneralSqlCmd.Command.CommandTimeout = 90;


                    //ADD BY RICHARD 20160621 limit 300
                    strSql += " AND ROWNUM <=300 ORDER BY TRANSTIME DESC  ";


                    dt = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                    if (dt.Rows.Count == 0)
                    {
                        this.ShowMessage("目前查詢沒有任何資料", MessageMode.INFO);

                        ViewState[this.GridView1.ClientID] = dt;
                        this.HiddenShowPanel.Value = "false";
                    }
                    else
                    {
                        ViewState[this.GridView1.ClientID] = dt;
                        this.HiddenShowPanel.Value = "true";
                    }
                }

                //this.GridView1.DataBind((DataTable)ViewState[this.GridView1.ClientID], Anew, LockPageNum);
                this.GridView1.DataBind((DataTable)ViewState[this.GridView1.ClientID], Anew, LockPageNum, this.lblTotalPage_GridView1, this.lblPage_GridView1, null);

            }
            catch (System.Exception ex) { this.ShowMessage(ex); }
            finally
            {
                if (dt != null) { dt.Dispose(); dt = null; }

                this.DBConn.Dispose(); this.DBConn = null;
            }
        }
        #endregion

        #region GridView Events

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

        #region GridView1_RowCommand
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView gv = (GridView)sender;
            try
            {
                if (e.CommandName != "Stop") return;

                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;

                int result = 0;

                string strSql = string.Empty, strWhere = string.Empty;

                string strWPINNO = gv.Rows[rowIndex].Cells[2].Text.Trim();

                if (e.CommandName == "Stop")
                {
                    #region Delete

                    this.DBConn.GeneralSqlCmd.Command.CommandTimeout = 90;

                    //TRANSTABLE
                    strWhere = string.Format(" And WPINNO = '{0}' and RECEIVER=N'{1}' \n", strWPINNO, this.UserInfo.RealName);
                    strSql = this.Delete.TRANSTable(strWhere);
                    this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);
                    result = this.DBConn.GeneralSqlCmd.ExecuteNonQuery(strSql);
                    if (result < 1)
                    {
                        this.ShowMessage("刪除失敗"); return;
                    }
                    else
                    {
                        this.ShowMessage("刪除成功", MessageMode.INFO);
                        this.DataBind(true, true);

                        //ADD BY RICHARD 20160614 for Monitor
                        #region Monitor
                        string userIP = this.Request.ServerVariables["REMOTE_ADDR"].ToString();
                        this.MonitorLog.LogMonitor(strWPINNO, this.UserInfo.UserName, this.UserInfo.RealName, userIP, Monitor.MSGID.WDA16, "105簽收刪除文號");
                        #endregion
                    }
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

        #region
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.RowDataBound();

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton btnDelete = (ImageButton)e.Row.Cells[0].FindControl("ImageBtnDelete");

                this.Confirm(btnDelete, ConfirmMode.Delete);

                //ADD BY RICHARD 20160705 for 簽收資料編輯僅能由原始作業者進行操作
                if (!UserInfo.RealName.Equals(e.Row.Cells[5].Text.Trim()))
                {
                    e.Row.Cells[0].Text = "";
                }


            }

        }
        #endregion



        #endregion

        #region BtnOK_Click()
        protected void BtnOK_Click(object sender, EventArgs e)
        {
            try
            {
                this.DataBind(true, false);

            }
            catch (Exception ex)
            {
                this.ShowMessage(ex);
            }

        }
        #endregion


    }
}