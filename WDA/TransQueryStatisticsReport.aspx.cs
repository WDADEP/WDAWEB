using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WDA.Class;


namespace WDA
{
    public partial class WebForm1 : GridViewUtility
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

                    //ADD BY RICHARD 20160816
                    this.GetFileUserList();
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
                //ADD 增加部門別判斷
                // Modified by Luke 2016/09/12
                strSql = this.Select.UserTable(" AND DEPTID IN (10,12) AND USERSTATUS=0 ");

                this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                dt = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                DataRow defaultRow = dt.NewRow();
                defaultRow["RealName"] = "選擇人員";
                dt.Rows.Add(defaultRow);

                dt.DefaultView.Sort = "UserID";
                this.ddlReceiver.DataSource = dt;

                this.ddlReceiver.DataTextField = "RealName";
                this.ddlReceiver.DataValueField = "RealName";

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

                    if (!string.IsNullOrEmpty(this.TxtWpinno.Text.Trim()))
                    {
                        string wpinno = this.TxtWpinno.Text.Trim();

                        where += string.Format(" And tt.WPINNO = '{0}'", wpinno);
                    }

                    string realName = this.ddlReceiver.SelectedValue.Trim();
                    if (realName != "選擇人員")
                    {
                        where += string.Format(" AND tt.RECEIVER = N'{0}'", realName.Trim());
                    }

                    if (!string.IsNullOrEmpty(this.txtScanCreateTime.Text.Trim()))
                    {
                        string startTime = this.txtScanCreateTime.Text.Trim().Replace(StringFormatException.Mode.Sql);//開始日期
                        string endTime = this.txtScanEndTime.Text.Trim().Replace(StringFormatException.Mode.Sql);//結束日期

                        where += string.Format(" AND tt.TRANSTIME Between TO_DATE('{0}','YYYY/MM/DD HH24:MI:SS') And TO_DATE('{1}','YYYY/MM/DD HH24:MI:SS') ", startTime, endTime);
                    }

                    if (!string.IsNullOrEmpty(this.TxtFileNo.Text.Trim()))
                    {
                        string strFileNo = this.TxtFileNo.Text.Trim();

                        where += string.Format(" And wp.FileNo like '{0}%'", strFileNo);
                    }

                    if (this.radFile.SelectedValue.Trim().Equals("1"))
                        where += string.Format(" AND wp.FileNo is not null");
                    else if (this.radFile.SelectedValue.Trim().Equals("0"))
                        where += string.Format(" AND wp.FileNo is null");

                    // Added by Luke 2016/09/12
                    whereDetail = where;        // the same filter

                    //ADD BY RICHARD 20160908 GROUP BY 
                    where += "  GROUP BY substr(tt.TRANSTIME,1,10),tt.RECEIVER,wp.FILENO \n  ORDER BY substr(tt.TRANSTIME,1,10) \n ";

                    // Added by Luke 2016/09/12
                    whereDetail += "  ORDER BY tt.TRANSTIME, tt.WPINNO ";

                    strSql = this.Select.TranstableGroup(where);

                    this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                    // Added by Luke 2016/09/12
                    strDetailSql = this.Select.TranstableDetail(whereDetail);
                    this.WriteLog(global::Log.Mode.LogMode.DEBUG, strDetailSql);

                    // Modified by Luke 2016/09/12
                    Session["TranstableGroup"] = strSql; // used for printing report

                    // Added by Luke 2016/09/12
                    Session["TranstableDetail"] = strDetailSql; // used for printing report

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

        #region GridView1_RowDataBound
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.RowDataBound();

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                switch (e.Row.Cells[4].Text)
                {
                    case "0": e.Row.Cells[4].Text = "無";
                        break;
                    default: e.Row.Cells[4].Text = "有";
                        break;
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