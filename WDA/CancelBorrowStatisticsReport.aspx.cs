using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WDA.Class;

namespace WDA
{
    public partial class CancelBorrowStatisticsReport : GridViewUtility
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

                // Added by Luke 2016/09/12
                string strDetailSql = string.Empty;
                string whereDetail = string.Empty;

                if (Anew)
                {

                    if (!string.IsNullOrEmpty(this.txtBorrowCreateTime.Text.Trim()))
                    {
                        string startTime = this.txtBorrowCreateTime.Text.Trim().Replace(StringFormatException.Mode.Sql);//開始日期
                        string endTime = this.txtBorrowEndTime.Text.Trim().Replace(StringFormatException.Mode.Sql);//結束日期

                        where += string.Format(" AND wb.TRANST Between TO_DATE('{0} 00:00:00','YYYY/MM/DD HH24:MI:SS') And TO_DATE('{1} 23:59:59','YYYY/MM/DD HH24:MI:SS') ", startTime, endTime);
                    }

                    where += string.Format(" AND wb.REDATE = TO_DATE('{0}','YYYY/MM/DD HH24:MI:SS') ", "2999/12/31 12:00:00");


                    strSql = this.Select.CancelBorrowStatistics(where);

                    this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);


                    // ADD by Richard 2016/10/01
                    Session["CancelBorrowGroup"] = strSql; // used for printing report


                    this.DBConn.GeneralSqlCmd.Command.CommandTimeout = 90;

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
        
        #endregion


    }
}