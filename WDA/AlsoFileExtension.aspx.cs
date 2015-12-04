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
            string Where = string.Empty;

            Hashtable ht = new Hashtable();
            try
            {
                if (!string.IsNullOrEmpty(this.txtWpinno.Text)) { Where = string.Format("WPINNO ='{0}'", this.txtWpinno.Text); }
                if (!string.IsNullOrEmpty(this.txtWpoutNo.Text)) { Where = string.Format("WPOUTNO ='{0}'", this.txtWpinno.Text); }

                ht.Clear();
                ht.Add("EXTEN", "Y");
                ht.Add("APPROVEUSERID", this.ddlApproveuserID.SelectedValue);

                strSql = this.Update.wpborrow(ht, Where);

                this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                int result = this.DBConn.GeneralSqlCmd.ExecuteNonQuery(strSql);

                if (result < 1)
                {
                    this.ShowMessage("展期成功失敗"); return;
                }

                this.ShowMessage("展期成功", MessageMode.INFO);

                this.txtWpinno.Text = string.Empty; this.txtWpoutNo.Text = string.Empty; 
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