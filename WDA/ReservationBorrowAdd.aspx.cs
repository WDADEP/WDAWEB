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
            this.DataBind(true);
        }
        #endregion

        #region BtnOK_Click()
        protected void BtnOK_Click(object sender, EventArgs e)
        {
            if (this.ToBorrow())
            {
                this.ShowMessage("預約借檔(新增) 成功", MessageMode.INFO);

                this.HiddenShowPanel.Value = "false";
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
            this.txtTel.Enabled =
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
        private void GetViewType(bool haveElecFile)
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
            try
            {
                dt.Columns.Add("Text");
                dt.Columns.Add("Value");

                dr = null; dr = dt.NewRow();
                dr["Text"] = "1. 紙本調閱";
                dr["Value"] = "1";
                dt.Rows.Add(dr);

                if (haveElecFile)
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

        #region DataBind()
        /// <summary>
        /// 資料繫結
        /// </summary>
        /// <param name="Anew">是否重新撈資料</param>
        private void DataBind(bool Anew)
        {
            string strSql = string.Empty, strWhere = string.Empty;

            DataTable dt = null;
            try
            {
                if (Anew)
                {
                    strWhere += string.Format("And wr.WpinNo = '{0}'\n",
                        this.txtWpinNo.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim());

                    strSql = this.Select.WpborrowQuery(strWhere);

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
                        this.txtWpoutNo.Text = String.IsNullOrEmpty(dt.Rows[0]["WpoutNo"].ToString()) ? "無對應資料" : dt.Rows[0]["WpoutNo"].ToString();

                        this.txtTel.Text = UserInfo.Tel;

                        this.txtReceiver.Text = UserInfo.UserName;

                        this.txtWpindate.Text = String.IsNullOrEmpty(dt.Rows[0]["WpinDate"].ToString()) ? "無對應資料" : dt.Rows[0]["WpinDate"].ToString();

                        this.txtCirlName.Text = String.IsNullOrEmpty(dt.Rows[0]["CirlName"].ToString()) ? "無對應資料" : dt.Rows[0]["CirlName"].ToString();

                        this.txtCaseNo.Text = String.IsNullOrEmpty(dt.Rows[0]["CaseNo"].ToString()) ? "無對應資料" : dt.Rows[0]["CaseNo"].ToString();

                        this.txtCommName.Text = String.IsNullOrEmpty(dt.Rows[0]["CommName"].ToString()) ? "無對應資料" : dt.Rows[0]["CommName"].ToString();

                        #region 調閱類型

                        if (String.IsNullOrEmpty(dt.Rows[0]["OnFile"].ToString()))
                            this.GetViewType(false);
                        else
                            this.GetViewType(true);

                        #endregion

                        bool check = false;

                        #region 備註

                        string note = string.Empty;
                        string redate = dt.Rows[0]["redate"].ToString();
                        string prtflag = dt.Rows[0]["Prtflag"].ToString();

                        switch (dt.Rows[0]["Marker"].ToString())
                        {
                            case "":
                                note += "本件尚未掃描"; break;
                            case "N":
                                note += "本件尚未歸檔"; break;
                            case "R":
                                note += "本件已歸檔"; break;
                        }

                        if (dt.Rows.Count >= 1 && redate.Length == 0)
                        {
                            switch (prtflag)
                            {
                                case "":
                                    check = true;
                                    note += "，可以預約"; break;
                                case "F":
                                case "P":
                                case "T":
                                    check = false;
                                    note += "，目前被借出，無法預約"; break;
                                case "N":
                                    check = false;
                                    note += "，目前預約借檔簽核中，無法預約"; break;
                            }
                        }
                        else
                        {
                            check = true;
                            note += "，可以預約";
                        }

                        this.txtNote.Text = note;

                        #endregion

                        this.BtnOK.Enabled = check;

                        this.HiddenShowPanel.Value = "true";
                    }
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
                string wpinNo = this.txtWpinNo.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim();
                string wpoutNo = this.txtWpoutNo.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim();
                string kind = this.ddlKind.SelectedValue;
                string viewType = this.ddlViewType.SelectedValue;
                string approveUserID = this.ddlApproveUser.SelectedValue;

                ht.Add("WpinNo", wpinNo);
                ht.Add("Transt", "SYSDATE");
                ht.Add("Receiver", UserInfo.UserName);
                ht.Add("WpoutNo", wpoutNo);
                ht.Add("Kind", kind);
                ht.Add("ViewType", viewType);
                ht.Add("ApproveUserID", approveUserID);

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
    }
}