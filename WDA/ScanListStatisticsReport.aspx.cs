using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WDA.Class;


namespace WDA
{
    public partial class ScanListStatisticsReport : PageUtility
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

            try
            {
                if (!IsPostBack)
                {
                    //string sScript = "$('#divPanel').hide();";
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "ScanListQuery", sScript, true);
                    this.HiddenShowPanel.Value = "false";
                    this.GridView1.PageSize = Convert.ToInt32(this.GetSystem("PageSize"));

                    //ADD BY RICHARD 20160818
                    this.GetScanUserList();
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

                    //ADD BY RICHARD 20160330 for ADD WPINNO QUERY
                    if (!string.IsNullOrEmpty(this.TxtWpinno.Text.Trim()))
                    {
                        string strWpinno = this.TxtWpinno.Text.Trim();
                        where += string.Format(" And bt.BARCODEVALUE = '{0}' \n", strWpinno);
                    }

                    // Modified by Luke 2016/09/12
                    string realName = this.ddlScanner.SelectedValue.Trim();
                    if (realName != "選擇人員")
                    {
                        where += string.Format("And ut.RealName =N'{0}' \n", realName.Trim());
                    }

                    if (!string.IsNullOrEmpty(txtScanCreateTime.Text))
                    {
                        string startTime = this.txtScanCreateTime.Text.Trim().Replace(StringFormatException.Mode.Sql);//開始日期
                        string endTime = this.txtScanEndTime.Text.Trim().Replace(StringFormatException.Mode.Sql);//結束日期

                        where += string.Format(" And bt.CreateTime Between  TO_DATE('{0}', 'YYYY/MM/DD HH24:MI:SS') And TO_DATE('{1}', 'YYYY/MM/DD HH24:MI:SS')", startTime, endTime);
                    }

                    if (!string.IsNullOrEmpty(this.TxtFileNo.Text.Trim()))
                    {
                        string strFileNo = this.TxtFileNo.Text.Trim();

                        where += string.Format(" And bt.FileNo = '{0}' \n", strFileNo);
                    }

                    // Added by Luke 2016/09/12
                    whereDetail = where;        // the same filter

                    // Added by Luke 2016/09/12
                    whereDetail += "  ORDER BY bt.CREATETIME, bt.BARCODEVALUE ";

                    strSql = this.Select.ScanListStatists(where);

                    this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                    // Added by Luke 2016/09/12
                    strDetailSql = this.Select.ScanListStatistsDetail(whereDetail);
                    this.WriteLog(global::Log.Mode.LogMode.DEBUG, strDetailSql);

                    // Modified by Luke 2016/09/12
                    Session["ScanListStatists"] = strSql;

                    // Added by Luke 2016/09/12
                    Session["ScanListStatistsDetail"] = strDetailSql; // used for printing report

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

        #region GetScanUserList
        /// <summary>
        /// 取得掃描室人員列表
        /// </summary>
        private void GetScanUserList()
        {
            string strSql = string.Empty;

            DataTable dt = null;
            try
            {
                //ADD 增加部門別判斷
                strSql = this.Select.UserTable(" AND DEPTID=12 AND USERSTATUS=0 ");

                this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                dt = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                DataRow defaultRow = dt.NewRow();
                defaultRow["RealName"] = "選擇人員";
                dt.Rows.Add(defaultRow);

                dt.DefaultView.Sort = "UserID";
                this.ddlScanner.DataSource = dt;

                this.ddlScanner.DataTextField = "RealName";
                this.ddlScanner.DataValueField = "RealName";

                this.ddlScanner.DataBind();

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
    }
}