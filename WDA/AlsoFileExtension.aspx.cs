using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WDA.Class;

namespace WDA
{
    public partial class AlsoFileExtension : PageUtility
    {
        #region Page_Load()
        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadPage(true);
            try
            {
                if (!IsPostBack)
                {
                    this.InitInfo();
                }
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message);
            }
        }

        #endregion

        #region InitInfo()
        /// <summary>
        /// 
        /// </summary>
        private void InitInfo()
        {
            this.GetApproveuserID();
        }
        #endregion

        #region GetApproveuserID()
        /// <summary>
        /// 取得角色列表
        /// </summary>
        private void GetApproveuserID()
        {
            string strSql = string.Empty;

            DataTable dt = null;
            try
            {
                strSql = this.Select.GetApproveuserID();

                this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                dt = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                dt.Columns.Add("ShowData", System.Type.GetType("System.String"), "UserName+'('+RealName+')'");

                DataRow defaultRow = dt.NewRow();
                defaultRow["UserName"] = "請選擇";
                defaultRow["RealName"] = "選擇審核人員";
                defaultRow["UserID"] = "0";
                dt.Rows.Add(defaultRow);

                dt.DefaultView.Sort = "UserID";
                this.ddlApproveuserID.DataSource = dt;

                this.ddlApproveuserID.DataTextField = "ShowData";
                this.ddlApproveuserID.DataValueField = "UserID";

                this.ddlApproveuserID.DataBind();

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

        #region BtnOK_Click()
        protected void BtnOK_Click(object sender, EventArgs e)
        {
            string strSql = string.Empty;
            string where = string.Empty;

            Hashtable ht = new Hashtable();
            try
            {
                if (!string.IsNullOrEmpty(this.txtWpinno.Text)) { where = string.Format("And wpinno ='{0}' And ViewType ='{1}' And receiver ='{2}'", this.txtWpinno.Text.Trim(), this.ddlViewType.SelectedValue, this.UserInfo.UserName); }

                ht.Clear();
                ht.Add("EXTEN", "Y");
                ht.Add("APPROVEUSERID", this.ddlApproveuserID.SelectedValue);

                strSql = this.Update.wpborrow(ht,where);

                this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                int result = this.DBConn.GeneralSqlCmd.ExecuteNonQuery(strSql);

                if (result < 1)
                {
                    this.ShowMessage("申請展期成功失敗"); return;
                }

                this.ShowMessage("申請展期成功", MessageMode.INFO);

                this.txtWpinno.Text = string.Empty; this.ddlViewType.SelectedIndex = 0;

                #region Monitor
                string wpinno = string.Empty;

                if (!string.IsNullOrEmpty(this.txtWpinno.Text.Trim()))
                {
                    wpinno = this.txtWpinno.Text.Trim().Replace(StringFormatException.Mode.Sql);
                }

                string userIP = this.Request.ServerVariables["REMOTE_ADDR"].ToString();

                this.MonitorLog.LogMonitor(wpinno, this.UserInfo.UserName, this.UserInfo.RealName, userIP, Monitor.MSGID.WDA14, string.Empty);
                #endregion

            }
            catch (System.Exception ex) { this.ShowMessage(ex.Message); }
            finally
            {
                ht = null;

                this.DBConn.Dispose(); this.DBConn = null;
            }
        } 
        #endregion

        #region BtnClear_Click()
        protected void BtnClear_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(this.Request.Url.AbsoluteUri);
        }
        #endregion
    }
}