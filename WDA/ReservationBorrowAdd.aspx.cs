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
    public partial class ReservationBorrowAdd : PageUtility
    {
        #region ViewType()
        /// <summary>
        /// ViewType：1、紙本；2、電子
        /// </summary>
        protected int ViewType
        {
            get
            {
                if (ViewState["ViewType"] == null)
                    return 0;
                else
                    return (int)(ViewState["ViewType"]);
            }

            set { ViewState["ViewType"] = value; }
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
                    this.InitControl();

                    this.GetApproveUser();

                    this.HiddenShowPanel.Value = "false";
                }
            }
            catch (Exception ex) { this.ShowMessage(ex); }
        }
        #endregion

        #region BtnQuery_Click()
        protected void BtnQuery_Click(object sender, EventArgs e)
        {
            this.UIDataBind();
        }
        #endregion

        #region BtnOK_Click()
        protected void BtnOK_Click(object sender, EventArgs e)
        {
            if (this.ddlViewType.SelectedValue == "1" && this.ViewType == 2)
            {
                if (string.IsNullOrEmpty(this.txtReason.Text.Trim())) { this.ShowMessage("有電子檔，選擇紙本調閱，需填寫理由"); return; }
            }
            if (this.ToBorrow())
            {
                this.ShowMessage("預約借檔(新增) 成功", MessageMode.INFO);

                this.HiddenShowPanel.Value = "false";

                #region Monitor
                string wpinno = string.Empty;

                if (!string.IsNullOrEmpty(this.txtWpoutNo.Text.Trim()))
                {
                    wpinno = this.txtWpoutNo.Text.Trim().Replace(StringFormatException.Mode.Sql);
                }

                string userIP = this.Request.ServerVariables["REMOTE_ADDR"].ToString();

                this.MonitorLog.LogMonitor(wpinno, this.UserInfo.UserName, this.UserInfo.RealName, userIP, Monitor.MSGID.WDA06, string.Empty);
                #endregion
            }
            else
            {
                this.ShowMessage("預約借檔(新增) 失敗");
            }
        }
        #endregion

        #region Private Method

        #region InitControl()
        /// <summary>
        /// 初始化控制項
        /// </summary>
        private void InitControl()
        {
            this.txtWpoutNo.Enabled =
            //this.txtTel.Enabled =
            this.txtReceiver.Enabled =
            this.txtWpindate.Enabled =
            this.txtCirlName.Enabled =
            this.txtCaseNo.Enabled =
            this.txtCommName.Enabled =
            this.txtNote.Enabled = false;
        }
        #endregion

        #region GetViewType
        /// <summary>
        /// 
        /// </summary>
        private void GetViewType(bool Doc,bool Elec)
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
            try
            {
                dt.Columns.Add("Text");
                dt.Columns.Add("Value");

                if (Doc)
                {
                    dr = null; dr = dt.NewRow();
                    dr["Text"] = "1. 紙本調閱";
                    dr["Value"] = "1";
                    dt.Rows.Add(dr);
                }

                if (Elec)
                {
                    dr = null; dr = dt.NewRow();
                    dr["Text"] = "2. 電子調閱";
                    dr["Value"] = "2";
                    dt.Rows.Add(dr);
                }

                dr = null; dr = dt.NewRow();
                dr["Text"] = "請選擇";
                dr["Value"] = "0";
                dt.Rows.InsertAt(dr, 0);

                this.ddlViewType.DataSource = dt;

                this.ddlViewType.DataTextField = "Text";
                this.ddlViewType.DataValueField = "Value";

                this.ddlViewType.DataBind();
            }
            catch (Exception ex) { this.ShowMessage(ex.Message); }
            finally
            {
                if (dt != null) { dt.Dispose(); dt = null; }
            }
        }
        #endregion

        #region GetApproveUser
        /// <summary>
        /// 
        /// </summary>
        private void GetApproveUser()
        {
            string strSql = string.Empty;

            DataTable dt = null;
            try
            {
                strSql = this.Select.GetApproveuserID("27");

                this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                this.DBConn.GeneralSqlCmd.Command.CommandTimeout = 90;

                dt = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                dt.DefaultView.Sort = "UserName";
                this.ddlApproveUser.DataSource = dt;

                if (dt.Rows.Count > 0)
                {
                    for (int i = dt.Rows.Count - 1; i >= 0; i--)
                    {
                        if (dt.Rows[i]["UserID"].ToString() == UserInfo.UserID)
                        {
                            dt.Rows.RemoveAt(i);
                        }
                    }
                }

                DataRow defaultRow = dt.NewRow();
                defaultRow["RealName"] = "請選擇";
                defaultRow["UserID"] = "0";
                dt.Rows.InsertAt(defaultRow, 0);

                this.ddlApproveUser.DataTextField = "RealName";
                this.ddlApproveUser.DataValueField = "UserID";

                this.ddlApproveUser.DataBind();
            }
            catch (Exception ex) { this.ShowMessage(ex.Message); }
            finally
            {
                if (this.DBConn != null) { this.DBConn.Dispose(); this.DBConn = null; }

                if (dt != null) { dt.Dispose(); dt = null; }
            }
        }
        #endregion

        #region UIDataBind()
        /// <summary>
        /// 資料繫結
        /// </summary>
        private void UIDataBind()
        {
            string strSql = string.Empty, where = string.Empty; string note = string.Empty; string wpinNo = string.Empty; string receiver = string.Empty;
            DataTable dt = null;
            bool isDocBorrow = false; bool isElscBorrow = false; bool isBoxNoBorrow = false; bool check = true;

            try
            {
                #region BKWPFILE
                where = string.Format("And A.WpinNo = '{0}'", this.txtWpinNo.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim());

                strSql = this.Select.BoxNOCheck(where);

                this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                this.DBConn.GeneralSqlCmd.Command.CommandTimeout = 90;

                dt = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                if (dt.Rows.Count > 0)
                {
                    isBoxNoBorrow = true;

                    wpinNo = dt.Rows[0]["WpinNo"].ToString();
                    receiver = dt.Rows[0]["RECEIVER"].ToString();
                }
                #endregion

                #region WprecCheck
                where = string.Format("And wr.WpinNo = '{0}'", this.txtWpinNo.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim());

                strSql = this.Select.WprecCheck(where);

                this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                this.DBConn.GeneralSqlCmd.Command.CommandTimeout = 90;

                dt = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                if (dt.Rows.Count == 0)
                {
                    this.ShowMessage("此收文號不存在(WPREC)", MessageMode.INFO); return;
                }
                #endregion

                #region WpborrowCheckByDOC
                where = string.Format("And wp.WpinNo = '{0}'", this.txtWpinNo.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim());

                strSql = this.Select.WpborrowCheckByDOC(where);

                this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                this.DBConn.GeneralSqlCmd.Command.CommandTimeout = 90;

                dt = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["VIEWTYPE"].ToString() == "1")//紙本檔已被借閱
                        {
                            isDocBorrow = true;
                        }
                    }
                }
                #endregion

                #region WpborrowCheckByELSC
                where = string.Format("And wp.WpinNo = '{0}' And wp.receiver ='{1}'", this.txtWpinNo.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim(), this.UserInfo.UserName);

                strSql = this.Select.WpborrowCheckByELSC(where);

                this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                this.DBConn.GeneralSqlCmd.Command.CommandTimeout = 90;

                dt = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["VIEWTYPE"].ToString() == "2")//電子檔已被同一人借閱
                        {
                            isElscBorrow = true;
                        }
                    }
                }
                #endregion

                #region BarcodeCheck
                where = string.Format("And bt. BARCODEVALUE = '{0}'", this.txtWpinNo.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim());

                strSql = this.Select.BarcodeCheck(where);

                this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                this.DBConn.GeneralSqlCmd.Command.CommandTimeout = 90;

                dt = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                if (dt.Rows.Count > 0)//有電子檔
                {
                    if (string.IsNullOrEmpty(dt.Rows[0]["OnFile"].ToString()) && !isDocBorrow && !isBoxNoBorrow)
                    {
                        this.GetViewType(true, false);
                        note = "電子檔尚未歸檔，僅可借閱紙本檔";
                        this.ViewType = 1;

                        this.ddlViewType.SelectedIndex = 1;

                        this.ddlViewType_SelectedIndexChanged(null, null);
                    }
                    else if (isDocBorrow)
                    {
                        if (isElscBorrow)
                        {
                            check = false;
                            note = "紙本檔、電子檔已被借閱";
                        }
                        else
                        {
                            this.GetViewType(false, true); note = "紙本檔已被借閱，僅可借閱電子檔";
                            this.ddlViewType.SelectedIndex = 1;
                            this.ddlViewType_SelectedIndexChanged(null, null);
                        }
                    }
                    else if (isBoxNoBorrow)
                    {
                        if (isElscBorrow)
                        {
                            check = false;
                            note = "紙本檔、電子檔已被借閱";
                        }
                        else
                        {
                            this.GetViewType(false, true);
                            note = string.Format("紙本同卷宗號已被借閱，借閱人：{0}、借閱文號：{1}", receiver, wpinNo);
                            this.ddlViewType.SelectedIndex = 1;
                            this.ddlViewType_SelectedIndexChanged(null, null);
                        }
                    }
                    else
                    {
                        if (isElscBorrow)
                        {
                            this.GetViewType(true, false); note = "電子檔已被同一人借閱，僅可借閱紙本檔";
                        }
                        else
                        {
                            this.GetViewType(true, true); note = "電子紙本皆可預約";

                            this.ViewType = 2;

                            this.ddlViewType.SelectedIndex = 2;
                            this.ddlViewType_SelectedIndexChanged(null, null);
                        }
                    }
                }
                else if (isDocBorrow) //紙本檔已被借閱且沒有電子檔
                {
                    check = false;
                    note = "紙本檔已被借閱且沒有電子檔";
                }
                else if (isBoxNoBorrow)
                {
                    check = false;
                    note = string.Format("紙本同卷宗號已被借閱，借閱人：{0}、借閱文號：{1}",receiver,wpinNo);
                }
                else { this.ViewType = 1; this.GetViewType(true, false); note = "只有紙本檔"; }
                #endregion

                where = string.Format("And wr.WpinNo = '{0}'", this.txtWpinNo.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim());

                strSql = this.Select.WprecQuery(where);

                this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                this.DBConn.GeneralSqlCmd.Command.CommandTimeout = 90;

                dt = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                if (string.IsNullOrEmpty(dt.Rows[0]["fileno"].ToString()))
                {
                    this.HiddenShowPanel.Value = "false";
                    this.ShowMessage("歸檔檔案號為空", MessageMode.INFO); return;
                }
                if (string.IsNullOrEmpty(dt.Rows[0]["filedate"].ToString()))
                {
                    this.HiddenShowPanel.Value = "false";
                    this.ShowMessage("歸檔日期為空", MessageMode.INFO); return;
                }

                this.txtWpoutNo.Text = String.IsNullOrEmpty(dt.Rows[0]["WpoutNo"].ToString()) ? "無對應資料" : dt.Rows[0]["WpoutNo"].ToString();

                this.txtTel.Text = UserInfo.Tel;

                this.txtReceiver.Text = UserInfo.UserName;

                this.txtWpindate.Text = String.IsNullOrEmpty(dt.Rows[0]["WpinDate"].ToString()) ? "無對應資料" : dt.Rows[0]["WpinDate"].ToString();

                this.txtCirlName.Text = String.IsNullOrEmpty(dt.Rows[0]["CirlName"].ToString()) ? "無對應資料" : dt.Rows[0]["CirlName"].ToString();

                this.txtCaseNo.Text = String.IsNullOrEmpty(dt.Rows[0]["CaseNo"].ToString()) ? "無對應資料" : dt.Rows[0]["CaseNo"].ToString();

                this.txtCommName.Text = String.IsNullOrEmpty(dt.Rows[0]["CommName"].ToString()) ? "無對應資料" : dt.Rows[0]["CommName"].ToString();

                this.txtNote.Text = note;

                this.BtnOK.Enabled = check;

                this.HiddenShowPanel.Value = "true";
            }
            catch (System.Exception ex) { this.ShowMessage(ex); }
            finally
            {
                if (dt != null) { dt.Dispose(); dt = null; }

                if (this.DBConn != null) { this.DBConn.Dispose(); this.DBConn = null; }
            }
        }
        #endregion

        #region ToBorrow()
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool ToBorrow()
        {
            string strSql = string.Empty, strWhere = string.Empty;

            int result = 0;

            Hashtable ht = new Hashtable();
            try
            {
                string wpinNo = this.txtWpinNo.Text.Trim().Replace(StringFormatException.Mode.Sql);
                string wpoutNo = this.txtWpoutNo.Text.Trim().Replace(StringFormatException.Mode.Sql);
                string kind = this.ddlKind.SelectedValue;
                //string kind = string.Empty;
                string viewType = this.ddlViewType.SelectedValue;
                string approveUserID = this.ddlApproveUser.SelectedValue;
                string imagePriv = this.ddlImagePriv.SelectedValue;
                string reason = this.txtReason.Text.Trim().Replace(StringFormatException.Mode.Sql);

                //switch (this.lblkindName.Text)
                //{
                //    case "一般":
                //        kind ="1"; break;
                //    case "法制":
                //        kind ="2"; break;
                //    case "行政":
                //        kind ="3"; break;
                //}

                ht.Add("WpinNo", wpinNo);
                ht.Add("Transt", "SYSDATE");
                ht.Add("Receiver", UserInfo.UserName);
                ht.Add("WpoutNo", wpoutNo);
                ht.Add("Kind", kind);
                ht.Add("ViewType", viewType);
                ht.Add("ApproveUserID", approveUserID);
                ht.Add("IMAGEPRIV", imagePriv);
                ht.Add("REASON", reason);

                this.DBConn.GeneralSqlCmd.Command.CommandTimeout = 90;

                strSql = this.Insert.Wpborrow(ht);

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

        #region ddlViewType_SelectedIndexChanged()
        protected void ddlViewType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ddlViewType.SelectedValue == "1")
            {
                if (this.ViewType == 2)
                { this.txtReason.Enabled = true; }
                else
                {
                    this.txtReason.Enabled = false;
                    this.txtReason.Text = string.Empty;
                }
                this.ddlImagePriv.Enabled = false;
            }
            if (this.ddlViewType.SelectedValue == "2")
            {
                this.ddlImagePriv.Enabled = true;
                this.txtReason.Enabled = false;
                this.txtReason.Text = string.Empty;
            }

            //else { this.ddlImagePriv.Enabled = false; this.txtReason.Enabled = false; this.txtReason.Text = string.Empty; }
        } 
        #endregion
    }
}