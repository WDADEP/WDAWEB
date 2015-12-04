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
    public partial class UserEdit : PageUtility
    {
        #region ModifyUserInfo
        /// <summary>
        /// Modify User Info's
        /// </summary>
        protected class ModifyUserInfoLib
        {
            public string UserID = string.Empty;
            public string UserName = string.Empty;
            public string RealName = string.Empty;
            public string PassWord = string.Empty;
            public string TEL = string.Empty;
        }
        /// <summary>
        /// Modify User Info's
        /// </summary>
        private ModifyUserInfoLib _ModifyUserInfo = null;
        /// <summary>
        /// Modify User Info's
        /// </summary>
        protected ModifyUserInfoLib ModifyUserInfo
        {
            get
            {
                if (this._ModifyUserInfo == null)
                {
                    this._ModifyUserInfo = new ModifyUserInfoLib();
                    try
                    {
                        string userId = this.RequestQueryString("UserId");

                        string where = string.Format("And UserId={0}", userId);

                        if (userId == string.Empty && this.Session[SessionName.UserID] != null)
                            userId = this.Session[SessionName.UserID].ToString();

                        string strSql = this.Select.UserTable(where);

                        this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                        DataTable dt = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                        if (dt == null || dt.Rows.Count == 0)
                        {
                            this._ModifyUserInfo = null;

                            this.ShowMessage(string.Format("UserID 錯誤:{0}", userId));
                        }
                        else
                        {
                            this._ModifyUserInfo.UserID = dt.Rows[0]["UserID"].ToString();
                            this._ModifyUserInfo.UserName = dt.Rows[0]["UserName"].ToString();
                            this._ModifyUserInfo.RealName = dt.Rows[0]["RealName"].ToString();
                            this._ModifyUserInfo.PassWord = dt.Rows[0]["PassWord"].ToString();
                            this._ModifyUserInfo.TEL = dt.Rows[0]["TEL"].ToString();

                        }
                    }
                    catch (System.Exception ex)
                    {
                        this.ShowMessage(ex.Message);

                        this.WriteLog(global::Log.Mode.LogMode.ERROR, string.Format("UserEdit.ModifyUserInfo.Exception:{0}", ex));
                    }
                    finally
                    {
                        this.DBConn.Dispose(); this.DBConn = null;
                    }
                }
                return this._ModifyUserInfo;
            }
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
            this._ModifyUserInfo = null;

            this.LblRealName.Text = this.ModifyUserInfo.RealName;
            this.LblUserName.Text = this.ModifyUserInfo.UserName;
            this.TxtTel.Text = this.ModifyUserInfo.TEL;
            //this.TxtPassWord.Text = this.ModifyUserInfo.PassWord;
        }
        #endregion

        #region BtnOK_Click()
        protected void BtnOK_Click(object sender, EventArgs e)
        {
            string strSql = string.Empty;

            Hashtable ht = new Hashtable();
            try
            {
                if (string.IsNullOrEmpty(this.TxtPassWord.Text.Trim())) { return; }
                if (string.IsNullOrEmpty(this.TxtTel.Text.Trim())) { return; }

                ht.Add("UserID", this.ModifyUserInfo.UserID);
                ht.Add("Password", this.TxtPassWord.Text.Replace(StringFormatException.Mode.Sql).EncryptDES());
                ht.Add("TEL", this.TxtTel.Text.Replace(StringFormatException.Mode.Sql));

                strSql = this.Update.UserTablePassWord(ht);

                this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                int result = this.DBConn.GeneralSqlCmd.ExecuteNonQuery(strSql);

                if (result < 1)
                {
                    this.ShowMessage("更新使用者資料失敗"); return;
                }

                //監控
                this.ShowMessage("更新成功", MessageMode.INFO);

                this.InitInfo();
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
            this.ReloadPage();
        }
        #endregion
    }
}