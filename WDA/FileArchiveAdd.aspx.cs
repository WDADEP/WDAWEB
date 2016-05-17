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
    public partial class FileArchiveAdd : PageUtility
    {
        #region Property

        #region ViewType
        /// <summary>
        /// CaseID
        /// </summary>
        protected string ViewType
        {
            get
            {
                if (ViewState["ViewType"] == null) { return string.Empty; }
                else { return ViewState["ViewType"].ToString(); }
            }
            set { ViewState["ViewType"] = value; }
        }
        #endregion

        #region CaseID
        /// <summary>
        /// CaseID
        /// </summary>
        protected string CaseID
        {
            get
            {
                if (ViewState["CaseID"] == null) { return string.Empty; }
                else { return ViewState["CaseID"].ToString(); }
            }
            set { ViewState["CaseID"] = value; }
        }
        #endregion

        #region BarcodeValue
        /// <summary>
        /// 收文文號
        /// </summary>
        protected string BarcodeValue
        {
            get
            {
                if (ViewState["BarcodeValue"] == null) { return string.Empty; }
                else { return ViewState["BarcodeValue"].ToString(); }
            }
            set { ViewState["BarcodeValue"] = value; }
        }
        #endregion

        #region FileDate
        /// <summary>
        /// 歸檔日期
        /// </summary>
        protected string FileDate
        {
            get
            {
                if (ViewState["FileDate"] == null) { return string.Empty; }
                else { return ViewState["FileDate"].ToString(); }
            }
            set { ViewState["FileDate"] = value; }
        }
        #endregion

        #region FileNo
        /// <summary>
        /// 歸檔檔號
        /// </summary>
        protected string FileNo
        {
            get
            {
                if (ViewState["FileNo"] == null) { return string.Empty; }
                else { return ViewState["FileNo"].ToString(); }
            }
            set { ViewState["FileNo"] = value; }
        }
        #endregion

        #region KeepYr
        /// <summary>
        /// 保存年限
        /// </summary>
        protected string KeepYr
        {
            get
            {
                if (ViewState["KeepYr"] == null) { return string.Empty; }
                else { return ViewState["KeepYr"].ToString(); }
            }
            set { ViewState["KeepYr"] = value; }
        }
        #endregion

        #region BoxNo
        /// <summary>
        /// 卷宗號
        /// </summary>
        protected string BoxNo
        {
            get
            {
                if (ViewState["BoxNo"] == null) { return string.Empty; }
                else { return ViewState["BoxNo"].ToString(); }
            }
            set { ViewState["BoxNo"] = value; }
        }
        #endregion

        #endregion

        #region Page_Load()
        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadPage(true);

            this.Form.DefaultButton = this.Button1.UniqueID;
            try
            {
                if (!IsPostBack)
                {
                    this.InitControl();

                    this.HiddenShowPanel.Value = "false";
                }
            }
            catch (Exception ex) { this.ShowMessage(ex); }
        }
        #endregion

        #region BtnOK_Click()
        protected void BtnOK_Click(object sender, EventArgs e)
        {
            if (!this.CheckWprec())
            {
                this.HiddenShowPanel.Value = "false";

                this.ShowMessage("查詢不到收文號，或此文號已歸檔", MessageMode.INFO); return;
            }


            if (!this.GetCaseID())
            {
                this.ViewType = "1";//紙本

                //this.HiddenShowPanel.Value = "false";

                //this.ShowMessage("查詢不到對應的案件編號", MessageMode.INFO);

                this.txtBarcodeValue.Text = this.txtQueryBarcodeValue.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim();
                this.txtFileDate.Text = DateTime.Now.ToString("yyyyMMdd");

                this.txtOnFile.Text = UserInfo.RealName;

                this.HiddenShowPanel.Value = "true";
            }
            else
            {
                this.ViewType = "2";//電子

                this.DataBind(true, true);
            }
        }
        #endregion

        #region BtnAdd_Click()
        protected void BtnAdd_Click(object sender, EventArgs e)
        {
            if (this.ViewType == "1")
            {
                if (this.UpdateDataByWprec())
                {
                    this.txtBarcodeValue.Text = string.Empty;
                    this.txtBarcodeValue.Enabled = true;
                    if (this.RadioButtonList1.SelectedValue == "0")
                    {
                        this.txtBoxNo.Text = (this.BoxNo.Length == 0) ? "" : (Convert.ToInt64(this.BoxNo) + 1).ToString().PadLeft(this.BoxNo.Length, '0');
                    }
                    else if (this.RadioButtonList1.SelectedValue == "1")
                    {
                        this.txtBoxNo.Text = this.BoxNo;
                    }

                    this.txtFileNo.Text = this.FileNo;
                    this.txtKeepYr.Text = this.KeepYr;
                    //ADD BY RICHARD 20160414 set focus
                    this.txtBarcodeValue.Focus();
                    //this.HiddenShowPanel.Value = "false";
                }
            }
            else if (this.ViewType == "2") //電子
            {
                if (this.UpdateData())
                {
                    this.DataBind(true, false);
                }
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
            this.txtBarcodeValue.Enabled =
            this.txtFileDate.Enabled =
            this.txtOnFile.Enabled = false;
        }
        #endregion

        #region CheckWprec()
        /// <summary>
        /// 檢查Wprec
        /// </summary>
        /// <returns></returns>
        private bool CheckWprec()
        {
            string strSql = string.Empty, where = string.Empty;

            bool result = false;
            DataTable dt = null;
            try
            {
                //MODIFY BY RICHARD 20160427 檢查該文號是否有發文(發文日期或發文人員有值)
                where = string.Format("And WPINNO = '{0}'\n And FILENO is null And FILEDATE is null And KEEPYR is null And BOXNO is null AND  (SENDMAN is not null or WPOUTDATE is not null) ", this.txtQueryBarcodeValue.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim());

                strSql = this.Select.WprecCheck(where);

                this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                this.DBConn.GeneralSqlCmd.Command.CommandTimeout = 90;

                dt = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                if (dt.Rows.Count > 0) { result = true; }
            }
            catch (System.Exception ex) { this.ShowMessage(ex); }
            finally
            {
                if (this.DBConn != null) { this.DBConn.Dispose(); this.DBConn = null; }
            }
            return result;
        }
        #endregion

        #region DataBind()
        /// <summary>
        /// 資料繫結
        /// </summary>
        /// <param name="Anew">是否重新撈資料</param>
        private void DataBind(bool Anew, bool IsQuery)
        {
            string strSql = string.Empty, strWhere = string.Empty;

            DataTable dt = null;
            try
            {
                this.txtBarcodeValue.Text =
                this.txtFileNo.Text =
                this.txtBoxNo.Text =
                this.txtKeepYr.Text =
                this.txtFileDate.Text =
                this.txtOnFile.Text = string.Empty;

                if (Anew)
                {
                    strWhere += string.Format("And bt.CaseID = '{0}'\n", this.CaseID);
                    strWhere += string.Format("And bt.FileNo IS NULL\n");
                    strWhere += string.Format("And bt.FileDate IS NULL\n");
                    strWhere += string.Format("And bt.KeepYr IS NULL\n");
                    strWhere += string.Format("And bt.BoxNo IS NULL\n");
                    //strWhere += string.Format("And bt.OnFile IS NULL\n");

                    strSql = this.Select.BarcodeTable(strWhere);

                    this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                    this.DBConn.GeneralSqlCmd.Command.CommandTimeout = 90;

                    dt = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                    if (dt.Rows.Count == 0)
                    {
                        this.txtQueryBarcodeValue.Text = string.Empty;

                        this.HiddenShowPanel.Value = "false";

                        //Remark by Richard 20160407 for 建議於歸檔未成功時，再行跳出提示畫面
                        //this.ShowMessage("已完成歸檔", MessageMode.INFO);
                    }
                    else
                    {
                        int index = 0;
                        bool isAdd = true;
                        if (IsQuery)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                if (dt.Rows[i]["BarcodeValue"].ToString() == this.txtQueryBarcodeValue.Text.Trim())
                                {
                                    index = i; isAdd = false; break;
                                }
                            }
                            if (isAdd)
                            {
                                this.ShowMessage(string.Format("{0} 已歸檔", this.txtQueryBarcodeValue.Text.Trim()), MessageMode.INFO);
                            }
                        }

                        //顯示資料
                        this.txtBarcodeValue.Text = dt.Rows[index]["BarcodeValue"].ToString();

                        this.txtFileNo.Text = this.FileNo;

                        if (this.RadioButtonList1.SelectedValue == "0")
                        {
                            this.txtBoxNo.Text = (this.BoxNo.Length == 0) ? "" : (Convert.ToInt64(this.BoxNo) + 1).ToString().PadLeft(this.BoxNo.Length, '0');
                        }
                        else if (this.RadioButtonList1.SelectedValue == "1")
                        {
                            this.txtBoxNo.Text = this.BoxNo;
                        }
                        this.txtKeepYr.Text = this.KeepYr;

                        this.txtFileDate.Text = DateTime.Now.ToString("yyyyMMdd");

                        this.txtOnFile.Text = UserInfo.RealName;

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

        #region GetCaseID()
        /// <summary>
        /// 取得 CaseID
        /// </summary>
        /// <returns></returns>
        private bool GetCaseID()
        {
            string strSql = string.Empty, strWhere = string.Empty;

            bool result = false;
            try
            {
                strWhere += string.Format("And bt.BarcodeValue = '{0}'\n", this.txtQueryBarcodeValue.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim());

                strSql = this.Select.BarcodeTable(strWhere);

                this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                this.DBConn.GeneralSqlCmd.Command.CommandTimeout = 90;

                this.CaseID = this.DBConn.GeneralSqlCmd.ExecuteByColumnName(strSql, "CaseID");

                if (this.CaseID.Length > 0) { result = true; }
            }
            catch (System.Exception ex) { this.ShowMessage(ex); }
            finally
            {
                if (this.DBConn != null) { this.DBConn.Dispose(); this.DBConn = null; }
            }
            return result;
        }
        #endregion

        #region UpdateData
        /// <summary>
        /// 更新資料
        /// </summary>
        /// <returns></returns>
        private bool UpdateData()
        {
            string strSql = string.Empty, strWhere = string.Empty;

            int result = 0;

            Hashtable ht = new Hashtable();
            try
            {
                this.BarcodeValue = this.txtBarcodeValue.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim();
                this.FileDate = this.txtFileDate.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim();
                this.FileNo = this.txtFileNo.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim();
                this.KeepYr = this.txtKeepYr.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim();
                this.BoxNo = this.txtBoxNo.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim();

                ht.Add("BarcodeValue", this.BarcodeValue);
                ht.Add("FileDate", this.FileDate);
                ht.Add("FileNo", this.FileNo);
                ht.Add("KeepYr", this.KeepYr);
                ht.Add("BoxNo", this.BoxNo);
                ht.Add("OnFile", UserInfo.RealName);
                ht.Add("LastModifyUserID", UserInfo.UserID);
                ht.Add("LastModifyTime", "SYSDATE");

                this.DBConnTransac.GeneralSqlCmd.Command.CommandTimeout = 90;

                //Wprec
                strWhere = string.Format("And WPINNO = '{0}'\n", this.BarcodeValue);
                strSql = this.Update.Wprec(ht, strWhere);
                this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);
                result = this.DBConnTransac.GeneralSqlCmd.ExecuteNonQuery(strSql);
                if (result < 1)
                {
                    this.ShowMessage("WPREC Table 找不到對應的文號");
                    this.WriteLog(global::Log.Mode.LogMode.ERROR, "Update WPREC Fail");
                    return false;
                }

                //Wptrans
                strSql = this.Insert.Wptrans(ht);
                this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);
                result = this.DBConnTransac.GeneralSqlCmd.ExecuteNonQuery(strSql);
                if (result < 1)
                {
                    this.WriteLog(global::Log.Mode.LogMode.ERROR, "Insert WPTRANS Fail");
                    return false;
                }

                //BarcodeTable
                strWhere = string.Format("And wp.WpinNo = '{0}'\n", this.BarcodeValue);
                strSql = this.Select.Wprec(strWhere);
                this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);
                string wpoutNo = this.DBConnTransac.GeneralSqlCmd.ExecuteByColumnName(strSql, "WpoutNo");  //找出發文文號
                ht.Add("WpoutNo", wpoutNo);

                strWhere = string.Format("And BarcodeValue = '{0}'\n", this.BarcodeValue);
                strSql = this.Insert.BarcodeTable(ht, strWhere);
                this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);
                result = this.DBConnTransac.GeneralSqlCmd.ExecuteNonQuery(strSql);

                if (result > 0)
                {
                    #region Monitor

                    string userIP = this.Request.ServerVariables["REMOTE_ADDR"].ToString();

                    this.MonitorLog.LogMonitor(this.BarcodeValue, this.UserInfo.UserName, this.UserInfo.RealName, userIP, Monitor.MSGID.WDA05, "電子檔歸檔");
                    #endregion

                    this.DBConnTransac.GeneralSqlCmd.Transaction.Commit();
                }
                else
                {
                    this.WriteLog(global::Log.Mode.LogMode.ERROR, "Update BARCODETABLE Fail");
                }
            }
            catch (System.Exception ex)
            {
                try
                {
                    if (this.DBConnTransac.GeneralSqlCmd.Transaction != null) this.DBConnTransac.GeneralSqlCmd.Transaction.Rollback();
                }
                catch { }

                this.ShowMessage(ex);
            }
            finally
            {
                if (ht != null) { ht.Clear(); ht = null; }

                if (this.DBConnTransac != null) { this.DBConnTransac.Dispose(); this.DBConnTransac = null; }
            }
            return Convert.ToBoolean(result);
        }
        #endregion

        #region UpdateDataByWprec
        /// <summary>
        /// 更新資料
        /// </summary>
        /// <returns></returns>
        private bool UpdateDataByWprec()
        {
            string strSql = string.Empty, strWhere = string.Empty;

            int result = 0;

            Hashtable ht = new Hashtable();
            try
            {
                this.BarcodeValue = this.txtBarcodeValue.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim();
                this.FileDate = this.txtFileDate.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim();
                this.FileNo = this.txtFileNo.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim();
                this.KeepYr = this.txtKeepYr.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim();
                this.BoxNo = this.txtBoxNo.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim();

                ht.Add("BarcodeValue", this.BarcodeValue);
                ht.Add("FileDate", this.FileDate);
                ht.Add("FileNo", this.FileNo);
                ht.Add("KeepYr", this.KeepYr);
                ht.Add("BoxNo", this.BoxNo);
                ht.Add("OnFile", UserInfo.RealName);
                ht.Add("LastModifyUserID", UserInfo.UserID);
                ht.Add("LastModifyTime", "SYSDATE");

                this.DBConnTransac.GeneralSqlCmd.Command.CommandTimeout = 90;

                //Wprec
                strWhere = string.Format("And WpinNo = '{0}'\n", this.BarcodeValue);
                strSql = this.Update.Wprec(ht, strWhere);
                this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);
                result = this.DBConnTransac.GeneralSqlCmd.ExecuteNonQuery(strSql);
                if (result < 1)
                {
                    this.ShowMessage("WPREC Table 找不到對應的收文號或是此文號已經歸檔");
                    this.WriteLog(global::Log.Mode.LogMode.ERROR, "Update WPREC Fail");
                    return false;
                }

                //Wptrans
                strSql = this.Insert.Wptrans(ht);
                this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);
                result = this.DBConnTransac.GeneralSqlCmd.ExecuteNonQuery(strSql);
                if (result < 1)
                {
                    this.WriteLog(global::Log.Mode.LogMode.ERROR, "Insert WPTRANS Fail");
                    return false;
                }

                if (result > 0)
                {
                    #region Monitor

                    string userIP = this.Request.ServerVariables["REMOTE_ADDR"].ToString();

                    this.MonitorLog.LogMonitor(this.BarcodeValue, this.UserInfo.UserName, this.UserInfo.RealName, userIP, Monitor.MSGID.WDA05, "紙本歸檔");
                    #endregion

                    this.DBConnTransac.GeneralSqlCmd.Transaction.Commit();
                }
            }
            catch (System.Exception ex)
            {
                try
                {
                    if (this.DBConnTransac.GeneralSqlCmd.Transaction != null) this.DBConnTransac.GeneralSqlCmd.Transaction.Rollback();
                }
                catch { }

                this.ShowMessage(ex);
            }
            finally
            {
                if (ht != null) { ht.Clear(); ht = null; }

                if (this.DBConnTransac != null) { this.DBConnTransac.Dispose(); this.DBConnTransac = null; }
            }
            return Convert.ToBoolean(result);
        }
        #endregion

        #endregion
    }
}