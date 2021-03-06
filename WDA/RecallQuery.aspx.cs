﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WDA.Class;

namespace WDA
{
    public partial class RecallQuery : PageUtility
    {
        #region Page_Load()
        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadPage(true);

            if (!this.IsPostBack)
            {
                try
                {

                }
                catch (System.Exception ex) { this.ShowMessage(ex); }
            }
        } 
        #endregion

        #region BtnClear_Click()
        protected void BtnClear_Click(object sender, EventArgs e)
        {
            this.ReloadPage();
        } 
        #endregion

        #region BtnPrint_Click()
        protected void BtnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                this.InitSQL();

                #region Monitor
                string jicuiTime = string.Empty;

                if (!string.IsNullOrEmpty(txtJicuiTime.Text))
                {
                    jicuiTime = this.txtJicuiTime.Text.Trim().Replace(StringFormatException.Mode.Sql);
                }

                string userIP = this.Request.ServerVariables["REMOTE_ADDR"].ToString();

                this.MonitorLog.LogMonitor(string.Empty, this.UserInfo.UserName, this.UserInfo.RealName, userIP, Monitor.MSGID.WDA12, string.Format("稽催日期：{0}", jicuiTime));
                #endregion
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message);
            }
        } 
        #endregion

        #region InitSQL()
        private void InitSQL()
        {
            string strSql = string.Empty;
            string where = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(txtJicuiTime.Text))
                {
                    string jicuiTime = this.txtJicuiTime.Text.Trim().Replace(StringFormatException.Mode.Sql);

                    jicuiTime = DateTime.Parse(jicuiTime).AddDays(1).AddSeconds(-1).ToString("yyyy/MM/dd HH:mm:ss");

                    where += string.Format("And B.transtExtra <= TO_DATE('{0}', 'YYYY/MM/DD HH24:MI:SS')", jicuiTime);
                }

                if (this.txtUserName.Text.Trim() != "*")
                {
                    string userName = this.txtUserName.Text.Trim();

                    //MODIFY BY RICHARD 20160411
                    //where += string.Format("And wb.receiver ='{0}'", userName);
                    where += string.Format("And ut.REALNAME ='{0}'", userName.Trim());
                }

                if (this.ddlKind.SelectedIndex > 0)
                {
                    string kind = this.ddlKind.SelectedValue;

                    where += string.Format("And wb.Kind = {0} ", kind);
                }

                strSql = this.Select.Wpborrow(where);

                this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                Session["RecallQuery"] = strSql;

                //DataTable dt = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);
            }
            catch (System.Exception ex) { this.ShowMessage(ex.Message); }
        }
        #endregion
    }
}