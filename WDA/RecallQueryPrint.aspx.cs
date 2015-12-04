using Microsoft.Reporting.WebForms;
using System;
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
    public partial class RecallQueryPrint : PageUtility
    {
        #region UserName
        protected string UserName
        {
            get
            {
                if (ViewState["UserName"] == null)
                    return null;
                else
                    return (string)(ViewState["UserName"]);
            }

            set { ViewState["UserName"] = value; }
        }
        #endregion

        #region Page_Load()
        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadPage(true);

            if (!IsPostBack)
            {
                if (PreviousPage != null && PreviousPage.IsCrossPagePostBack)
                {
                    var preContent = (ContentPlaceHolder)Page.PreviousPage.Master.FindControl("MainContent");

                    TextBox txtUserName = (TextBox)preContent.FindControl("TxtUserName");
                    TextBox txtJicuiTime = (TextBox)preContent.FindControl("txtJicuiTime");

                    if (!string.IsNullOrEmpty(txtUserName.Text))
                    {
                        UserName = txtUserName.Text;
                    }


                    this.ReportViewer1.LocalReport.DataSources.Clear();

                    ReportViewer1.LocalReport.SetParameters(new ReportParameter("UserName", UserInfo.RealName));
                    ReportViewer1.LocalReport.SetParameters(new ReportParameter("JicuiTime", txtJicuiTime.Text));
                }

                //ReportParameter RptPara1 = new ReportParameter();
                //RptPara1.Name = "UserName";
                //RptPara1.Values.Add(UserInfo.RealName);
                //ReportViewer1.LocalReport.SetParameters(RptPara1);

                ReportViewer1.SizeToReportContent = true;

                DataTable dt = getReportDataTable();

                if (dt.Rows.Count > 0)
                {
                    ReportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("RecallQueryTable", dt));
                    ReportViewer1.SizeToReportContent = true;
                }
                else { }
            }
        }
        #endregion

        #region getReportDataTable()
        private DataTable getReportDataTable()
        {
            DataTable dt = new DataTable();

            string strSql = string.Empty;
            string where = string.Empty;

            try
            {
                strSql = Session["RecallQuery"].ToString();

                this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                this.DBConn.GeneralSqlCmd.Command.CommandTimeout = 90;

                dt = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                if (dt.Rows.Count == 0)
                {
                    this.LoginShowMessage("目前查詢沒有任何資料");
                }
                else
                {
                    dt.Columns.Add("boxno", System.Type.GetType("System.String"));

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        where = string.Format("And WPINNO='{0}'", dt.Rows[i]["WPINNO"].ToString());

                        strSql = this.Select.WprecInfos(where);

                        DataTable dtBoxno = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                        if (dtBoxno.Rows.Count > 0)
                        {
                            DataRow[] boxnoRow = dt.Select(string.Format("WPINNO ={0}",dt.Rows[i]["WPINNO"].ToString()));
                            boxnoRow[0]["boxno"] = dtBoxno.Rows[0]["WPINNO"].ToString();
                        }

                        dtBoxno.Dispose(); dtBoxno = null;
                    }
                }
            }
            catch (Exception ex)
            {
                this.LoginShowMessage(ex.Message);
            }
            finally
            {
                this.DBConn.Dispose(); this.DBConn = null;
            }

            return dt;
        }
        #endregion
    }
}