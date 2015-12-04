using System;
using System.Collections;
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
    public partial class ScanListQuery : PageUtility
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
                    string sScript = "$('#divPanel').hide();";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ScanListQuery", sScript, true);

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
            try
            {
                string strSql = string.Empty;
                string where = string.Empty;

                if (Anew)
                {
                    DataTable dt = null;

                    OleDbCommand command = (OleDbCommand)this.DBConn.GeneralSqlCmd.Command;

                    command.Parameters.Clear();
                  
                    if (!string.IsNullOrEmpty(this.TxtUserName.Text.Trim()))
                    {
                        string userName = this.TxtUserName.Text.Trim();

                        command.Parameters.Add(new OleDbParameter("UserName", OleDbType.VarChar)).Value = userName;

                        where += string.Format("And ut.UserName =:UserName");
                    }
                    if (!string.IsNullOrEmpty(txtScanCreateTime.Text))
                    {
                        string startTime = this.txtScanCreateTime.Text.Trim().Replace(StringFormatException.Mode.Sql);//開始日期
                        string endTime = this.txtScanEndTime.Text.Trim().Replace(StringFormatException.Mode.Sql);//結束日期

                        endTime = DateTime.Parse(endTime).AddDays(1).AddSeconds(-1).ToString("yyyy/MM/dd HH:mm:ss");

                        where += string.Format(" And ct.CreateTime Between '{0}' And '{1}'", startTime, endTime);
                    }

                    strSql = this.Select.ScanListQuery(where);

                    this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                    Session["ScanListQuery"] = strSql;

                    this.DBConn.GeneralSqlCmd.Command.CommandTimeout = 90;

                    dt = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                    if (dt.Rows.Count == 0)
                    {
                        this.ShowMessage("目前查詢沒有任何資料", MessageMode.INFO);

                        return;
                    }
                    else
                    {
                        ViewState[this.GridView1.ClientID] = dt;

                        dt.Dispose(); dt = null;
                    }
                }

                this.GridView1.DataBind((DataTable)ViewState[this.GridView1.ClientID], Anew, LockPageNum);
            }
            catch (System.Exception ex) { this.ShowMessage(ex); }
            finally
            {
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

        #region GridView1_RowDataBound
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.RowDataBound();

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label  lblCaseID = (Label)e.Row.Cells[0].FindControl("LblCaseID");

                if (string.IsNullOrEmpty(CaseID))
                {
                    CaseID = lblCaseID.Text;
                }

                if (CaseID != lblCaseID.Text)
                {
                    CaseID = lblCaseID.Text;
                    ColorInt += 1;
                }

                if (ColorInt % 2 != 0)
                {
                    lblCaseID.Attributes.Add("style", "color:#AA0000 ");
                    e.Row.Cells[1].Text = " <span style=color:#AA0000>" + e.Row.Cells[1].Text + "</span>";
                    e.Row.Cells[2].Text = " <span style=color:#AA0000>" + e.Row.Cells[2].Text + "</span>";
                    e.Row.Cells[3].Text = " <span style=color:#AA0000>" + e.Row.Cells[3].Text + "</span>";
                    e.Row.Cells[4].Text = " <span style=color:#AA0000>" + e.Row.Cells[4].Text + "</span>";
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

        #region GridView1_PageIndexChanging()
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            e.PageIndexChanging(sender);

            this.DataBind(false, false);
        }
        #endregion
    }
}