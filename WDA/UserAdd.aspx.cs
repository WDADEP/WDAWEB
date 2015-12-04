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
    public partial class UserAdd : PageUtility
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
            this.GetDDLRole();
        }
        #endregion

        #region GetDDLRole()
        /// <summary>
        /// 取得角色列表
        /// </summary>
        private void GetDDLRole()
        {
            string strSql = string.Empty;

            DataTable dt = null;
            try
            {
                strSql = this.Select.RoleTable();

                this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                dt = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                DataRow defaultRow = dt.NewRow();
                defaultRow["RoleName"] = "請選擇";
                defaultRow["RoleID"] = "0";
                dt.Rows.Add(defaultRow);

                dt.DefaultView.Sort = "RoleID";
                this.DDLRole.DataSource = dt;

                this.DDLRole.DataTextField = "RoleName";
                this.DDLRole.DataValueField = "RoleID";

                this.DDLRole.DataBind();

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
            Hashtable ht = new Hashtable();
            string userID = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(this.TxtUserName.Text.Trim())) { return; }
                if (string.IsNullOrEmpty(this.TxtPassWord.Text.Trim())) { return; }
                if (string.IsNullOrEmpty(this.TxtRealName.Text.Trim())) { return; }
                if (string.IsNullOrEmpty(this.TxtTel.Text.Trim())) { return; }
                if (this.DDLRole.SelectedValue == "0") { return; }
            
                #region 新增使用者
                string strSqlUserID = this.Select.GetMaxUserID();

                userID =this.DBConnTransac.GeneralSqlCmd.ExecuteByColumnName(strSqlUserID, "UserID");


                ht.Clear();
                ht.Add("UserID", userID);
                ht.Add("UserName", this.TxtUserName.Text.Trim().Replace(StringFormatException.Mode.Sql));
                ht.Add("Password", this.TxtPassWord.Text.Trim().Replace(StringFormatException.Mode.Sql).EncryptDES());
                ht.Add("RealName", this.TxtRealName.Text.Trim().Replace(StringFormatException.Mode.Sql));
                ht.Add("TEL", this.TxtTel.Text.Trim().Replace(StringFormatException.Mode.Sql));
                ht.Add("CreateUserID", UserInfo.UserID);
                ht.Add("RoleID", this.DDLRole.SelectedValue);

                strSql = this.Insert.UserTable(ht);

                this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                int result = this.DBConnTransac.GeneralSqlCmd.ExecuteNonQuery(strSql);

                if (result < 1) throw new Exception("新增使用者資料失敗");

                #endregion

                this.DBConnTransac.GeneralSqlCmd.Transaction.Commit();

                this.ShowMessage("新增成功", MessageMode.INFO);

                this.TxtUserName.Text = string.Empty;
                this.TxtRealName.Text = string.Empty;
                this.DDLRole.SelectedIndex = -1;

            }
            catch (System.Exception ex)
            {
                try
                {
                    this.DBConnTransac.GeneralSqlCmd.Transaction.Rollback();
                }
                catch { }

                this.ShowMessage(ex);
            }
            finally
            {
                this.DBConnTransac.Dispose(); this.DBConnTransac = null;
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