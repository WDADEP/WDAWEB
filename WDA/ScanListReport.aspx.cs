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
    public partial class ScanListReport : PageUtility
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

                    if (!string.IsNullOrEmpty(txtUserName.Text))
                    {
                        UserName = txtUserName.Text;
                    }
                }

                this.ReportViewer1.LocalReport.DataSources.Clear();
                ReportParameter RptPara1 = new ReportParameter();
                RptPara1.Name = "UserName";
                RptPara1.Values.Add(UserInfo.RealName);
                ReportViewer1.LocalReport.SetParameters(RptPara1);
                ReportViewer1.SizeToReportContent = true;

                DataTable dt = getReportDataTable();

                if (dt.Rows.Count > 0)
                {
                    ReportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("ScanListTable", dt));
                    ReportViewer1.SizeToReportContent = true;
                }
            }
        }
        #endregion

        #region getReportDataTable()
        private DataTable getReportDataTable()
        {
            DataTable dt = new DataTable();

            string strSql = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(UserName))
                {
                    OleDbCommand command = (OleDbCommand)this.DBConn.GeneralSqlCmd.Command;

                    command.Parameters.Clear();

                    command.Parameters.Add(new OleDbParameter("CREATEUSERID", OleDbType.VarChar)).Value = UserName;
                }

                strSql = Session["ScanListQuery"].ToString();

                this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                this.DBConn.GeneralSqlCmd.Command.CommandTimeout = 90;

                dt = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                if (dt.Rows.Count == 0)
                {
                    this.LoginShowMessage("目前查詢沒有任何資料");
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