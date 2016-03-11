using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WDA.Class;

namespace WDA
{
    public partial class FileArchivePrint : PageUtility
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadPage(true);

            if (!IsPostBack)
            {
                if (PreviousPage != null && PreviousPage.IsCrossPagePostBack)
                {
                    this.ReportViewer1.LocalReport.DataSources.Clear();
                }

                ReportViewer1.SizeToReportContent = true;

                DataTable dt = getReportDataTable();

                if (dt.Rows.Count > 0)
                {
                    try
                    {
                        ReportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("FileArchiveTable", dt));

                        ReportViewer1.SizeToReportContent = true;
                    }
                    catch (Exception ex) { this.LoginShowMessage(ex.Message); }
                    finally
                    {
                        if (this.DBConn != null)
                        {
                            this.DBConn.Dispose(); this.DBConn = null;
                        }
                    }
                }
            }
        }

        #region getReportDataTable()
        private DataTable getReportDataTable()
        {
            DataTable dt = new DataTable();

            string strSql = string.Empty;
            try
            {
                strSql = Session["FileArchive"].ToString();

                this.WriteLog(global::Log.Mode.LogMode.DEBUG, "FileArchive：Session：GetReportData");

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
                if (this.DBConn != null)
                {
                    this.DBConn.Dispose(); this.DBConn = null;
                }
            }
            return dt;
        }
        #endregion
    }
}