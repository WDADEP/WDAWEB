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
    public partial class ReservationBorrowCancel : PageUtility
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadPage(true);
            try
            {
                if (!IsPostBack)
                {
                    this.InitControl();
                }
            }
            catch (Exception ex) { this.ShowMessage(ex); }
        }

        protected void BtnQuery_Click(object sender, EventArgs e)
        {
            this.UIDataBind();
        }

        protected void BtnOK_Click(object sender, EventArgs e)
        {
            if (this.ToCancel())
            {
                this.ShowMessage("預約借檔(取消) 成功", MessageMode.INFO);

                this.HiddenShowPanel.Value = "false";

                #region Monitor
                string wpinno = string.Empty;

                if (!string.IsNullOrEmpty(this.txtWpoutNo.Text.Trim()))
                {
                    wpinno = this.txtWpoutNo.Text.Trim().Replace(StringFormatException.Mode.Sql);
                }

                string userIP = this.Request.ServerVariables["REMOTE_ADDR"].ToString();

                this.MonitorLog.LogMonitor(wpinno, this.UserInfo.UserName, this.UserInfo.RealName, userIP, Monitor.MSGID.WDA07, string.Empty);
                #endregion
            }
            else
            {
                this.ShowMessage("預約借檔(取消) 失敗");
            }
        }

        #region Private Method

        #region InitControl()
        /// <summary>
        /// 初始化控制項
        /// </summary>
        private void InitControl()
        {
            this.txtWpoutNo.Enabled =
            this.txtTel.Enabled =
            this.ddlKind.Enabled =
            this.txtReceiver.Enabled =
            this.txtWpindate.Enabled =
            this.txtCirlName.Enabled =
            this.txtCaseNo.Enabled =
            this.txtCommName.Enabled =
            this.txtNote.Enabled = false;
            //this.ddlViewType.Enabled = false;
        }
        #endregion

        #region UIDataBind()
        /// <summary>
        /// 資料繫結
        /// </summary>
        /// <param name="Anew">是否重新撈資料</param>
        private void UIDataBind()
        {
            string strSql = string.Empty, where = string.Empty;

            DataTable dt = null;
            try
            {
                where = string.Format("And wb.WpinNo = '{0}' And wb.Receiver = '{1}' And wb.Prtflag != '{2}' And wb.Prtflag != '{3}' And wb.Viewtype ='{4}'",
                    this.txtWpinNo.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim(),
                    UserInfo.UserName,
                    "P",
                    "T",
                    this.ddlViewType.SelectedValue);

                strSql = this.Select.WpborrowQuery(where);

                this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                this.DBConn.GeneralSqlCmd.Command.CommandTimeout = 90;

                dt = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                if (dt.Rows.Count == 0)
                {
                    this.HiddenShowPanel.Value = "false";

                    this.ShowMessage("目前查詢沒有任何資料", MessageMode.INFO);
                }
                else
                {
                    this.txtWpoutNo.Text = dt.Rows[0]["WpoutNo"].ToString();

                    this.txtTel.Text = UserInfo.Tel;

                    this.ddlKind.SelectedValue = dt.Rows[0]["Kind"].ToString();

                    this.txtReceiver.Text = UserInfo.UserName;

                    this.txtWpindate.Text = dt.Rows[0]["Wpindate"].ToString();

                    this.txtCirlName.Text = dt.Rows[0]["CirlName"].ToString();

                    this.txtCaseNo.Text = dt.Rows[0]["CaseNo"].ToString();

                    this.txtCommName.Text = dt.Rows[0]["CommName"].ToString();

                    bool check = false;

                    #region 備註

                    string note = string.Empty;
                    switch (dt.Rows[0]["Prtflag"].ToString())
                    {
                        case "N":
                            check = true;
                            note += "預約借檔簽核中"; break;
                        case "F":
                            check = false;
                            note += "預約借檔簽核通過"; break;
                        case "Z":
                            check = false;
                            note += "預約借檔簽核不通過"; break;
                    }

                    this.txtNote.Text = note;

                    #endregion

                    this.BtnOK.Enabled = check;

                    this.HiddenShowPanel.Value = "true";
                }
            }
            catch (System.Exception ex) { this.ShowMessage(ex); }
            finally
            {
                if (dt != null) { dt.Dispose(); dt = null; }

                if (this.DBConn != null) { this.DBConn.Dispose(); this.DBConn = null; }
            }
        }
        #endregion

        #region ToCancel()
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool ToCancel()
        {
            string strSql = string.Empty, strWhere = string.Empty;

            int result = 0;

            Hashtable ht = new Hashtable();
            try
            {
                string wpinNo = this.txtWpinNo.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim();
                //string wpindate = this.txtWpindate.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim();
                //string wpoutNo = this.txtWpoutNo.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim();
                //string kind = this.ddlKind.SelectedValue;
                //string viewType = this.ddlViewType.SelectedValue;
                //string approveUserID = this.ddlApproveUser.SelectedValue;

                ht.Add("WpinNo", wpinNo);
                //ht.Add("Transt", wpindate);
                ht.Add("Receiver", UserInfo.UserName);
                //ht.Add("WpoutNo", wpoutNo);
                //ht.Add("Kind", kind);
                //ht.Add("ViewType", viewType);
                //ht.Add("ApproveUserID", approveUserID);

                this.DBConn.GeneralSqlCmd.Command.CommandTimeout = 90;

                strSql = this.Delete.Wpborrow(ht);

                this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                result = this.DBConn.GeneralSqlCmd.ExecuteNonQuery(strSql);
            }
            catch (System.Exception ex) { this.ShowMessage(ex); }
            finally
            {
                if (ht != null) { ht.Clear(); ht = null; }

                if (this.DBConn != null) { this.DBConn.Dispose(); this.DBConn = null; }
            }
            return Convert.ToBoolean(result);
        }
        #endregion
        #endregion
    }
}