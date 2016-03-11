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
    public partial class ReservationBorrowReport : PageUtility
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
                    this.ReportViewer1.LocalReport.DataSources.Clear();

                    ReportViewer1.LocalReport.SetParameters(new ReportParameter("UserName", UserInfo.RealName));
                }

                DataTable dt = getReportDataTable();

                if (dt.Rows.Count > 0)
                {
                    try
                    {
                        string strSql = string.Empty;

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            strSql = this.Update.WpborrowApprove("P", string.Format("And wpinno = '{0}' And PrtFlag ='F'", dt.Rows[i]["wpinno"].ToString()), false);

                            this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                            this.DBConn.GeneralSqlCmd.Command.CommandTimeout = 90;

                            this.DBConn.GeneralSqlCmd.ExecuteNonQuery(strSql);
                        }

                        ReportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("ReservationBorrowTable", dt));

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
        #endregion

        #region getReportDataTable()
        private DataTable getReportDataTable()
        {
            DataTable dt = new DataTable();

            string strSql = string.Empty;
            string where = string.Empty;
            try
            {
                strSql = Session["ReservationBorrow"].ToString();

                this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                this.DBConn.GeneralSqlCmd.Command.CommandTimeout = 90;

                dt = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                if (dt.Rows.Count == 0)
                {
                    this.LoginShowMessage("目前查詢沒有任何資料");
                }
                else
                {
                    dt.Columns.Add("wpoutno", System.Type.GetType("System.String"));
                    dt.Columns.Add("commname", System.Type.GetType("System.String"));
                    dt.Columns.Add("boxno", System.Type.GetType("System.String"));
                    dt.Columns.Add("fileno", System.Type.GetType("System.String"));
                    dt.Columns.Add("onfile", System.Type.GetType("System.String"));

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        where = string.Format("And wp.WPINNO='{0}'", dt.Rows[i]["WPINNO"].ToString());

                        strSql = this.Select.WprecBkwfile(where);

                        this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                        DataTable dtWpr = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                        DataRow[] row = dt.Select(string.Format("WPINNO ='{0}'", dt.Rows[i]["WPINNO"].ToString()));

                        if (dtWpr.Rows.Count > 0)
                        {
                            //DataRow[] row = dt.Select(string.Format("wp.WPINNO ='{0}'", dt.Rows[i]["WPINNO"].ToString()));
                            row[0]["wpoutno"] = dtWpr.Rows[0]["wpoutno"].ToString();
                            row[0]["commname"] = dtWpr.Rows[0]["commname"].ToString();
                            row[0]["boxno"] = dtWpr.Rows[0]["boxno"].ToString();
                            row[0]["fileno"] = dtWpr.Rows[0]["fileno"].ToString();
                            row[0]["onfile"] = dtWpr.Rows[0]["onfile"].ToString();
                        }
                        else
                        {
                            strSql = this.Select.WprecWptrans(where);

                            this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                            DataTable dtWps = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                            if (dtWps.Rows.Count > 0)
                            {
                                row[0]["wpoutno"] = dtWps.Rows[0]["wpoutno"].ToString();
                                row[0]["commname"] = dtWps.Rows[0]["commname"].ToString();
                                row[0]["boxno"] = dtWps.Rows[0]["boxno"].ToString();
                                row[0]["fileno"] = dtWps.Rows[0]["fileno"].ToString();
                                row[0]["onfile"] = dtWps.Rows[0]["receiver"].ToString();
                            }
                        }

                        dtWpr.Dispose(); dtWpr = null;
                    }

                    DataView dv = dt.DefaultView;
                    dv.Sort = "fileno ASC,boxno ASC";
                    dt = dv.ToTable();
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