﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WDA.Class;

namespace WDA
{
    public partial class PaperAlsoFile : PageUtility
    {
        #region Page_Load()
        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadPage(true);

            int rePage = this.Request.QueryString["RePage"] != null ? Convert.ToInt16(this.Request.QueryString["RePage"].Trim()) : 0;
            try
            {
                if (rePage == 1)
                {

                    this.ShowMessage("還檔成功");
                    rePage = 0;
                }

                if (!IsPostBack)
                {

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
            int result = -1;
            string userID = this.UserInfo.UserID;

            Hashtable ht = new Hashtable();
            try
            {
                if (!string.IsNullOrEmpty(this.txtWpinno.Text)) { Where = string.Format("WPINNO ='{0}'", this.txtWpinno.Text); }
                if (!string.IsNullOrEmpty(this.txtWpoutNo.Text)) { Where = string.Format("WPOUTNO ='{0}'", this.txtWpoutNo.Text); }

                ht.Clear();
                ht.Add("CHK", "Y");

                strSql = this.Update.FILEBORO(ht, Where);

                this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                result = this.DBConnTransac.GeneralSqlCmd.ExecuteNonQuery(strSql);

                if (result < 1)
                {
                    this.ShowMessage("更新Table：FILEBORO失敗"); return;
                }

                string reDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

                ht.Clear();
                ht.Add("REDATE", reDate);
                ht.Add("USERID", userID);

                strSql = this.Update.wpborrowPaperAlsoFile(ht, Where);

                this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                result = this.DBConnTransac.GeneralSqlCmd.ExecuteNonQuery(strSql);

                if (result < 1)
                {
                    this.ShowMessage("更新Table：WPBORROW失敗"); return;
                }

                #region 電子還檔
                strSql = this.Update.wpborrowByViewType(userID);

                this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                result = this.DBConnTransac.GeneralSqlCmd.ExecuteNonQuery(strSql);

                strSql = this.Update.wpborrowByViewTypeExten(userID);

                this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                result = this.DBConnTransac.GeneralSqlCmd.ExecuteNonQuery(strSql);
                #endregion

                this.DBConnTransac.GeneralSqlCmd.Transaction.Commit();

                string strUrl = string.Format("PaperAlsoFile.aspx?RePage=1");

                Response.Redirect(strUrl, true);
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

            #region Monitor
            string wpinno = string.Empty;

            if (!string.IsNullOrEmpty(this.txtWpinno.Text.Trim()))
            {
                wpinno = this.txtWpinno.Text.Trim().Replace(StringFormatException.Mode.Sql);
            }

            string userIP = this.Request.ServerVariables["REMOTE_ADDR"].ToString();

            this.MonitorLog.LogMonitor(wpinno, this.UserInfo.UserName, this.UserInfo.RealName, userIP, Monitor.MSGID.WDA13, string.Empty);
            #endregion
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