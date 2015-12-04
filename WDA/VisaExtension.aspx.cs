using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WDA.Class;

namespace WDA
{
    public partial class VisaExtension : PageUtility
    {
        #region Page_Load()
        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadPage(true);
            try
            {
                if (!IsPostBack)
                {
                    this.HiddenUserID.Value = this.UserInfo.UserID;
                }
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message);
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
                ht.Add("EXTEN", "D");

                strSql = this.Update.wpborrowByVisa(ht, Where);

                this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                int result = this.DBConn.GeneralSqlCmd.ExecuteNonQuery(strSql);

                if (result < 1)
                {
                    this.ShowMessage("簽准展期成功失敗"); return;
                }

                this.ShowMessage("簽准展期成功", MessageMode.INFO);
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