using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using WDA.Class;

namespace WDA
{
    public partial class WpBorrowStatisticsReport : GridViewUtility
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

                    //ADD BY RICHARD 20161001
                    this.GetFileUserList();
                    this.GetDeptList();
                }
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex);
            }

        }
        #endregion

        #region GetFileUserList
        /// <summary>
        /// 取得檔案室人員列表
        /// </summary>
        private void GetFileUserList()
        {
            string strSql = string.Empty;

            DataTable dt = null;
            try
            {
                // Modified by Luke 2016/09/12
                strSql = this.Select.UserTable(" AND DEPTID = 10 AND USERSTATUS=0 ");

                this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                dt = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                DataRow defaultRow = dt.NewRow();
                defaultRow["RealName"] = "選擇人員";
                dt.Rows.Add(defaultRow);

                dt.DefaultView.Sort = "UserID";
                this.ddlReceiver.DataSource = dt;

                this.ddlReceiver.DataTextField = "RealName";
                this.ddlReceiver.DataValueField = "USERID";

                this.ddlReceiver.DataBind();

                dt.Dispose(); dt = null;
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message);
            }
            finally
            {
                this.DBConn.Dispose(); this.DBConn = null;
            }
        }
        #endregion

        #region GetDeptList
        /// <summary>
        /// 取得科室列表
        /// </summary>
        private void GetDeptList()
        {
            string strSql = string.Empty;

            DataTable dt = null;
            try
            {
                //ADD 增加部門別判斷
                strSql = this.Select.DEPT("");

                this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                dt = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                DataRow defaultRow = dt.NewRow();
                defaultRow["DEPTNAME"] = "選擇科室";
                dt.Rows.Add(defaultRow);

                dt.DefaultView.Sort = "DEPTID";
                this.ddlDept.DataSource = dt;

                this.ddlDept.DataTextField = "DEPTNAME";
                this.ddlDept.DataValueField = "DEPTID";

                this.ddlDept.DataBind();

                dt.Dispose(); dt = null;

            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message);
            }
            finally
            {
                this.DBConn.Dispose(); this.DBConn = null;
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

                    string realName = this.ddlReceiver.SelectedValue.Trim();
                    if (realName != "選擇人員")
                    {
                        where += string.Format(" AND wb.USERID = '{0}'", realName.Trim());
                    }

                    string strDept = this.ddlDept.SelectedValue.Trim();
                    if (strDept != string.Empty)
                    {
                        where += string.Format(" AND ut2.DEPTID = '{0}'", strDept.Trim());
                    }


                    if (!string.IsNullOrEmpty(this.txtBorrowCreateTime.Text.Trim()))
                    {
                        string startTime = this.txtBorrowCreateTime.Text.Trim().Replace(StringFormatException.Mode.Sql);//開始日期
                        string endTime = this.txtBorrowEndTime.Text.Trim().Replace(StringFormatException.Mode.Sql);//結束日期

                        where += string.Format(" AND wb.REDATE Between TO_DATE('{0} 00:00:00','YYYY/MM/DD HH24:MI:SS') And TO_DATE('{1} 23:59:59','YYYY/MM/DD HH24:MI:SS') ", startTime, endTime);
                    }

                    if (!string.IsNullOrEmpty(this.TxtFileNo.Text.Trim()))
                    {
                        string strFileNo = this.TxtFileNo.Text.Trim();

                        where += string.Format(" And wp.FileNo like '{0}%'", strFileNo);
                    }

                    if (this.radRe.SelectedValue.Trim().Equals("1"))
                        where += string.Format(" AND wb.REDATE is not null");
                    else if (this.radRe.SelectedValue.Trim().Equals("0"))
                        where += string.Format(" AND wb.REDATE is null");

                    if (this.radViewType.SelectedValue.Trim().Equals("1"))
                        where += string.Format(" AND wb.VIEWTYPE =2 ");
                    else if (this.radRe.SelectedValue.Trim().Equals("0"))
                        where += string.Format(" AND wb.VIEWTYPE = 1 ");

                    strSql = this.Select.WpBorrowStatistics(where);

                    this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);


                    // ADD by Richard 2016/10/01
                    Session["WpBorrowGroup"] = strSql; // used for printing report


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

        #region BtnClear_Click()
        protected void BtnClear_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(this.Request.Url.AbsoluteUri);
        }
        #endregion

        #region BtnDetailPrint_Click()
        // Added by Luke 2016/10/17
        protected void BtnDetailPrint_Click(object sender, EventArgs e)
        {
            GridViewRow gridViewRow = (GridViewRow)((Button)sender).NamingContainer;
            int DataIndex = this.GridView1.PageSize * this.GridView1.PageIndex + gridViewRow.RowIndex;

            string strSql = string.Empty;
            string where = string.Empty;

            // 統計項目1
            string transTime = gridViewRow.Cells[1].Text.Trim().Replace('-', '/');
            string startTime = string.Format("{0} 00:00:00", transTime);//開始日期
            string endTime = string.Format("{0} 23:59:59", transTime);//結束日期
            where += string.Format(" AND wb.REDATE Between TO_DATE('{0}','YYYY/MM/DD HH24:MI:SS') And TO_DATE('{1}','YYYY/MM/DD HH24:MI:SS') ", startTime, endTime);

            // 統計項目2
            string realName = gridViewRow.Cells[2].Text.Trim();
            where += string.Format(" AND UT.REALNAME = N'{0}'", realName);

            // 統計項目3
            string strFileNo = gridViewRow.Cells[3].Text.Trim();
            if (strFileNo == "&nbsp;" || strFileNo.Length == 0)
                where += string.Format(" And NVL(wp.FileNo,'WDA_RPT') = 'WDA_RPT'", strFileNo);
            else
                where += string.Format(" And wp.FileNo = '{0}'", strFileNo);

            // 統計項目4
            string strDept = gridViewRow.Cells[4].Text.Trim();
            where += string.Format(" AND dt.DEPTNAME = N'{0}'", strDept);

            // 統計項目5
            string chk = gridViewRow.Cells[5].Text.Trim();
            if (chk == "無")
                where += string.Format(" AND fb.chk = 'N'");
            else if (chk == "有")
                where += string.Format(" AND fb.chk = 'Y'");

            // 統計項目6
            string viewType = gridViewRow.Cells[6].Text.Trim();
            if (viewType == "否")
                where += string.Format(" AND wb.VIEWTYPE = 1");
            else if (viewType == "是")
                where += string.Format(" AND wb.VIEWTYPE = 2");

            // 產生SQL
            where += "  ORDER BY wt.RECEIVER, bt.BARCODEVALUE, wp.WPINNO ";

            strSql = this.Select.WpBorrowStatisticsDetail(where);

            this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

            Session["WpBorrowStatistsDetail"] = strSql; // used for printing report


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

        #region GridView1_RowDataBound
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.RowDataBound();

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                switch (e.Row.Cells[5].Text)
                {
                    case "N": e.Row.Cells[5].Text = "無";
                        break;
                    default: e.Row.Cells[5].Text = "有";
                        break;
                }

                switch (e.Row.Cells[6].Text)
                {
                    case "1": e.Row.Cells[6].Text = "否";
                        break;
                    case "2": e.Row.Cells[6].Text = "是";
                        break;
                }

            }
        }
        #endregion



        #endregion
    }
}