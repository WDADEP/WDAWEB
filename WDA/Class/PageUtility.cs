using Log;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WDA.Class
{
    public class PageUtility:System.Web.UI.Page
    {
        public enum SelectMode { Text = 0, Value = 1 }

        #region XmlFormat
        /// <summary>
        /// XML 回應格式
        /// </summary>
        public class XmlFormat
        {
            #region Error
            /// <summary>
            /// 錯誤訊息回傳格式
            /// </summary>
            public static string Error
            {
                get { return "<?xml version=\"1.0\" encoding=\"utf-8\" ?><Return ReCode=\"{0}\" ErrMsg=\"{1}\"/>"; }
            }
            #endregion

            #region Pass
            /// <summary>
            /// 成功訊息回傳格式
            /// </summary>
            public static string Pass
            {
                get { return "<?xml version=\"1.0\" encoding=\"utf-8\" ?><Return ReCode=\"0000\"/>"; }
            }
            #endregion
        }
        #endregion

        #region SystemTable
        /// <summary>
        /// SystemTable
        /// </summary>
        public static DataView dv_Sys
        {
            get
            {
                if (_dv_Sys == null || _dv_Sys.Table.Rows.Count == 0) SystemTable();

                return _dv_Sys;
            }
            set { _dv_Sys = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        private static DataTable dt_Sys = null;
        /// <summary>
        /// 
        /// </summary>
        private static DataView _dv_Sys;
        /// <summary>
        /// 更新 SystemTable
        /// </summary>
        public static void SystemTable()
        {
            DBLib.DBConn dbconn = null;
            try
            {
                dbconn = new DBLib.DBConn(new DBLibUtility.Mode.FreeMode()
                    {
                        APMode = DBLibUtility.Mode.APMode.Web,
                        DBMode = DBLibUtility.Mode.DBMode.OleDb,
                        ConnectionString = WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString.ToString().DecryptDES()
                    });

                lock (dt_Sys = dbconn.GeneralSqlCmd.ExecuteToDataTable("Select SystemName, SystemComment From SystemTable"))
                {
                    _dv_Sys = new DataView(dt_Sys);

                    _dv_Sys.Sort = "SystemName";
                }
            }
            catch (System.Exception ex) { throw new Exception("SystemTable Error" + ex.ToString()); }
            finally
            {
                if (dbconn != null)
                {
                    dbconn.Dispose(); dbconn = null;
                }
            }
        }
        /// <summary>
        /// 取得 SystemTable 設定值
        /// </summary>
        /// <param name="SystemName"></param>
        /// <returns></returns>
        public string GetSystem(string SystemName)
        {
            try
            {
                string systemComment = string.Empty;

                if (PageUtility.dv_Sys == null || PageUtility.dv_Sys.Table.Rows.Count == 0) return string.Empty;

                var query = from systemSetting in PageUtility.dv_Sys.Table.AsEnumerable()
                            where systemSetting.Field<string>("SystemName") == SystemName
                            select new
                            {
                                SystemName = systemSetting.Field<string>("SystemName"),
                                SystemComment = systemSetting.Field<string>("SystemComment")
                            };

                foreach (var data in query)
                {
                    systemComment = data.SystemComment;
                }
                return systemComment;
            }

            catch (System.Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        #endregion

        #region DBParamList
        /// <summary>
        /// SQL語法參數陣列
        /// </summary>
        private List<IDataParameter> _DBSqlParamList = null;
        /// <summary>
        /// SQL語法參數陣列
        /// </summary>
        protected List<IDataParameter> DBSqlParamList
        {
            get
            {
                if (this._DBSqlParamList == null) this._DBSqlParamList = new List<IDataParameter>(); return this._DBSqlParamList;
            }
            set
            {
                this._DBSqlParamList = value;
            }
        }
        #endregion

        #region DBConn
        /// <summary>
        /// 資料庫物件介面
        /// </summary>
        private DBLib.IDBConn _DBConn = null;
        /// <summary>
        /// 資料庫物件介面
        /// </summary>
        public DBLib.IDBConn DBConn
        {
            get
            {
                if (this._DBConn == null)
                {
                    this._DBConn = new DBLib.DBConn(new DBLibUtility.Mode.FreeMode()
                    {
                        APMode = DBLibUtility.Mode.APMode.Web,
                        DBMode = DBLibUtility.Mode.DBMode.OleDb,
                        ConnectionString = WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString.ToString().DecryptDES()
                    });
                }
                return this._DBConn;
            }
            set { this._DBConn = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        private void CloseConn()
        {
            if (this._DBConn != null)
            {
                this.DBConn.Dispose(); this.DBConn = null;
            }
        }
        #endregion

        #region DBConnTransac
        /// <summary>
        /// 資料庫物件介面
        /// </summary>
        private DBLib.IDBTransacConn _DBConnTransac = null;
        /// <summary>
        /// 資料庫物件介面
        /// </summary>
        public DBLib.IDBTransacConn DBConnTransac
        {
            get
            {
                if (this._DBConnTransac == null)
                {
                    this._DBConnTransac = new DBLib.DBConnTransac(new DBLibUtility.Mode.FreeMode()
                    {
                        APMode = DBLibUtility.Mode.APMode.Web,
                        DBMode = DBLibUtility.Mode.DBMode.OleDb,
                        ConnectionString = WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString.ToString().DecryptDES()
                    });
                }
                return this._DBConnTransac;
            }
            set { this._DBConnTransac = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        private void CloseConnTransac()
        {
            if (this._DBConnTransac != null)
            {
                this.DBConnTransac.Dispose(); this.DBConnTransac = null;
            }
        }
        #endregion

        #region DBConnLog
        /// <summary>
        /// 資料庫物件
        /// </summary>
        private DBLib.IDBConn _DBConnLog = null;
        /// <summary>
        /// 資料庫物件
        /// </summary>
        protected DBLib.IDBConn DBConnLog
        {
            get
            {
                if (this._DBConnLog == null)
                {
                    this._DBConnLog = new DBLib.DBConn(new DBLibUtility.Mode.FreeMode()
                    {
                        APMode = DBLibUtility.Mode.APMode.Web,
                        DBMode = DBLibUtility.Mode.DBMode.OleDb,
                        ConnectionString = WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString.ToString().DecryptDES()
                    });
                }
                return this._DBConnLog;
            }
            set
            {
                this._DBConnLog = value;
            }
        }
        #endregion

        #region UserInfo
        /// <summary>
        /// User Info's
        /// </summary>
        public class UserInfoLib
        {
            public string UserID = string.Empty;
            public string SN = string.Empty;
            public string UserName = string.Empty;
            public string RealName = string.Empty;
            public int UserStatus = -1;
            public string Tel = string.Empty;

            public string UnitID = string.Empty;
            public string UnitName = string.Empty;
            public string UnitCode = string.Empty;

            public int RoleID = -1;
            public string RoleName = string.Empty;
            public string CreateDateTime = string.Empty;
            public DataSet Privilege = null;
        }
        /// <summary>
        /// User Info's
        /// </summary>
        private UserInfoLib _UserInfo = null;
        /// <summary>
        /// User Info's
        /// </summary>
        public UserInfoLib UserInfo
        {
            get
            {
                if (this._UserInfo == null || this._UserInfo.UserID != Session[SessionName.UserID].ToString())
                {
                    this._UserInfo = new UserInfoLib();
                    bool result = false;
                    string strSql = string.Empty;

                    try
                    {
                        strSql = this.Select.UserInfo(Session[SessionName.UserID].ToString());

                        this.WriteLog(Mode.LogMode.DEBUG, strSql);

                        DataTable dt = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                        if (dt == null || dt.Rows.Count == 0)
                        {
                            this._UserInfo = null;

                            this.ShowMessage(string.Format("UserID 錯誤:{0}", Session[SessionName.UserID].ToString()));
                        }
                        else
                        {
                            int number = 0;

                            this._UserInfo.UserID = dt.Rows[0]["UserID"].ToString();
                            this._UserInfo.UserName = dt.Rows[0]["UserName"].ToString();
                            this._UserInfo.RealName = dt.Rows[0]["RealName"].ToString();
                            this._UserInfo.Tel = dt.Rows[0]["Tel"].ToString();

                            result = Int32.TryParse(dt.Rows[0]["UserStatus"].ToString(), out number);

                            if (result)
                            {
                                this._UserInfo.UserStatus = number;
                            }

                            result = Int32.TryParse(dt.Rows[0]["RoleID"].ToString(), out number);

                            if (result)
                            {
                                this._UserInfo.RoleID = number;
                            }

                            this._UserInfo.RoleName = dt.Rows[0]["RoleName"].ToString();
                            this._UserInfo.Privilege = GetPrivilege(this._UserInfo.RoleID, this.DBConn);
                        }
                    }
                    catch (System.Exception ex)
                    {
                        this.ShowMessage(ex);

                        this.WriteLog(Mode.LogMode.ERROR, string.Format("PageUtility.UserInfo.Exception:{0}", ex));
                    }
                    finally
                    {
                        this.DBConn.Dispose(); this.DBConn = null;
                    }
                }
                return this._UserInfo;
            }
        }
        /// <summary>
        /// 取得 Privilege 設定值
        /// </summary>
        /// <returns></returns>
        protected DataSet GetPrivilege(int RoleID, DBLib.IDBConn DBConn)
        {
            DataSet ds = null;
            try
            {
                string strSql = string.Format("Select rp.PrivID\n"
                    + "From RoleTable rt\n"
                    + "Inner Join RolePrivilege rp On (rt.RoleID= rp.RoleID)\n"
                    + "Where rt.RoleID = {0}", RoleID);

                this.WriteLog(Mode.LogMode.INFO, string.Format("strSql:{0}", strSql));

                ds = DBConn.GeneralSqlCmd.ExecuteToDataSet(strSql);

                ds.Tables[0].TableName = "RoleList";

                return ds;
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        #endregion

        #region SqlCommand
        /// <summary>
        /// Sql Command (Select)
        /// </summary>
        private SqlCommand.Select _Select = null;
        /// <summary>
        /// Sql Command (Select)
        /// </summary>
        protected SqlCommand.Select Select
        {
            get
            {
                if (this._Select == null) this._Select = new SqlCommand.Select(); return this._Select;
            }
        }
        /// <summary>
        /// Sql Command (Update)
        /// </summary>
        private SqlCommand.Update _Update = null;
        /// <summary>
        /// Sql Command (Update)
        /// </summary>
        protected SqlCommand.Update Update
        {
            get
            {
                if (this._Update == null) this._Update = new SqlCommand.Update(); return this._Update;
            }
        }
        /// <summary>
        /// Sql Command (Delete)
        /// </summary>
        private SqlCommand.Delete _Delete = null;
        /// <summary>
        /// Sql Command (Delete)
        /// </summary>
        protected SqlCommand.Delete Delete
        {
            get
            {
                if (this._Delete == null) this._Delete = new SqlCommand.Delete(); return this._Delete;
            }
        }
        /// <summary>
        /// Sql Command (Insert)
        /// </summary>
        private SqlCommand.Insert _Insert = null;
        /// <summary>
        /// Sql Command (Insert)
        /// </summary>
        protected SqlCommand.Insert Insert
        {
            get
            {
                if (this._Insert == null) this._Insert = new SqlCommand.Insert(); return this._Insert;
            }
        }
        #endregion

        #region SessionName
        /// <summary>
        /// Session Name 管理
        /// </summary>
        public static class SessionName
        {
            /// <summary>
            /// UserID
            /// </summary>
            public static string UserID = "UserID";
            /// <summary>
            /// ClientIP
            /// </summary>
            public static string ClientIP = "ClientIP";
            /// <summary>
            /// UserMode(內部、外部)
            /// </summary>
            public static string UserMode = "UserMode";
        }
        #endregion

        #region Log
        /// <summary>
        /// Log 介面
        /// </summary>
        private ILog _Log = null;
        /// <summary>
        /// 
        /// </summary>
        public ILog Log
        {
            get
            {
                if (this._Log == null)
                    this._Log = new WebLog("WDA");
                this._Log.Delete(14);
                this._Log.Backup(5);
                this._Log.DeleteBackup(14);
                return this._Log;
            }
        }
        /// <summary>
        /// Write Log
        /// </summary>
        /// <param name="Message">Log 訊息</param>
        protected void WriteLog(string Message)
        {
            WriteLog(null, Message);
        }
        /// <summary>
        /// Write Log
        /// </summary>
        /// <param name="FileName">Log 檔案名稱</param>
        /// <param name="Message">Log 訊息</param>
        protected void WriteLog(string FileName, string Message)
        {
            if (FileName != null && FileName.Length > 0) this.Log.FileName = FileName;

            this.Log.WriteLog(Mode.LogMode.INFO, Message);
        }
        /// <summary>
        /// Write Log
        /// </summary>
        /// <param name="Mode"></param>
        /// <param name="Message">Log 訊息</param>
        protected void WriteLog(Mode.LogMode Mode, string Message)
        {
            WriteLog(Mode, null, Message);
        }
        /// <summary>
        /// Write Log
        /// </summary>
        /// <param name="Mode"></param>
        /// <param name="FileName">Log 檔案名稱</param>
        /// <param name="Message">Log 訊息</param>
        protected void WriteLog(Mode.LogMode Mode, string FileName, string Message)
        {
            if (FileName != null && FileName.Length > 0) this.Log.FileName = FileName;

            this.Log.WriteLog(Mode, Message);
        }
        /// <summary>
        /// Write Log
        /// </summary>
        /// <param name="Mode">Log 檔案名稱</param>
        /// <param name="Message">Log 訊息</param>
        /// <param name="SqlParamList">Sql 參數值</param>
        protected void WriteLog(Mode.LogMode Mode, string Message, List<IDataParameter> SqlParamList)
        {
            for (int i = 0; i < SqlParamList.Count; i++)
            {
                if (Message.Contains(SqlParamList[i].ParameterName))
                {
                    Message = Message.Replace(SqlParamList[i].ParameterName, string.Format("'{0}'", SqlParamList[i].Value.ToString()));
                }
            }

            WriteLog(Mode, null, Message);
        }
        #endregion

        #region MonitorLog
        /// <summary>
        /// 
        /// </summary>
        private Monitor _MonitorLog = null;
        /// <summary>
        /// 
        /// </summary>
        public Monitor MonitorLog
        {
            get { if (this._MonitorLog == null) this._MonitorLog = new Monitor(); return this._MonitorLog; }
            set { this._MonitorLog = value; }
        }
        #endregion

        #region IsNumber()
        /// <summary>
        /// 是否為數字
        /// </summary>
        /// <param name="CheckStr"></param>
        /// <returns></returns>
        protected bool IsNumber(string CheckStr)
        {
            Regex reg = new Regex("\\d", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            return reg.IsMatch(CheckStr, 0);
        }
        #endregion

        #region MessageBox()
        /// <summary>
        /// 訊息框
        /// </summary>
        public static string MessageBox()
        {
            return "<div id=\"divMessageBoxBase\" style=\"display: none;\" title=\"系統訊息提示\">"
               + "   <div id=\"divMessageBoxSub\">"
               + "       <table  id=\"tbMessageBoxBase\">"
               + "           <tr>"
               + "               <td id=\"tdMessageBoxBody\"></td>"
               + "           </tr>"
               + "       </table>"
               + "   </div>"
               + "</div>";
        }
        #endregion

        #region LoadPage()
        /// <summary>
        /// 初始化設定與初始判斷
        /// </summary>
        /// <param name="MyPage">Page Object</param>
        /// <param name="CheckSession">是否檢查 Session</param>
        protected void LoadPage(bool CheckSession)
        {
            if (Session[SessionName.ClientIP] == null || Session[SessionName.ClientIP].ToString().Length == 0) Session[SessionName.ClientIP] = this.Request.ServerVariables["REMOTE_ADDR"].ToString();

            if (CheckSession == true && Session[SessionName.UserID] == null) this.Response.Redirect("Login.aspx?RePage=1");

            if (this.ViewState[SessionName.UserID] == null && this.Session[SessionName.UserID] != null) this.ViewState[SessionName.UserID] = Session[SessionName.UserID].ToString();

            //this.Log.Delete(60);//清除大於60天的記錄

            this.Response.AddHeader("pragma", "no-cache");
            this.Response.AddHeader("Cache-Control", "no-cache, must-revalidate");
            this.Response.Expires = 0;

            if (this.ViewState[SessionName.UserID] != null) Session[SessionName.UserID] = this.ViewState[SessionName.UserID].ToString();
        }
        #endregion

        #region GenerateRandomCode()
        /// <summary> 
        /// 製造出幾個驗證碼
        /// </summary> 
        /// <returns></returns> 
        protected static string GenerateRandomCode()
        {
            string returnCode = "";

            Object thisLock = new Object();

            lock (thisLock)
            {
                Random random = null;
                try
                {
                    random = new Random();

                    int num = 0;

                    char code;

                    for (int i = 0; i < 6; i++)
                    {
                        num = random.Next();

                        if (num % 2 == 0) code = (char)('0' + (char)(num % 10));
                        else code = (char)('A' + (char)(num % 26));

                        returnCode += code.ToString();
                    }
                }
                catch (System.Exception ex)
                {
                    throw ex;
                }
            }
            thisLock = null;

            return returnCode;
        }
        #endregion

        #region ShowMessage()
        /// <summary>
        /// 
        /// </summary>
        public enum MessageMode { INFO = 1, ERROR = 2 };
        /// <summary>
        /// 顯示訊息
        /// </summary>
        /// <param name="Ex">欲顯示訊息</param>
        protected void ShowMessage(System.Exception Ex)
        {
            this.ShowMessage("系統發生錯誤，請洽系統管理員", MessageMode.ERROR);

            this.WriteLog(global::Log.Mode.LogMode.ERROR, string.Format("Exception : {0}", Ex.Message));
        }
        /// <summary>
        /// 顯示訊息
        /// </summary>
        /// <param name="Message">欲顯示訊息</param>
        protected void ShowMessage(string Message)
        {
            this.ShowMessage(Message, MessageMode.ERROR);
        }
        /// <summary>
        /// 顯示訊息
        /// </summary>
        /// <param name="Message">欲顯示訊息</param>
        protected void ShowMessage(string Message, MessageMode Mode)
        {
            string controlName = "HiddenMessage";
            ContentPlaceHolder mainContent = (ContentPlaceHolder)Master.FindControl("MainContent");
            System.Web.UI.HtmlControls.HtmlInputHidden hiddenMessage = (System.Web.UI.HtmlControls.HtmlInputHidden)mainContent.FindControl(controlName);
            //System.Web.UI.HtmlControls.HtmlInputHidden hiddenMessage = (System.Web.UI.HtmlControls.HtmlInputHidden)this.Master.FindControl(controlName);

            if (hiddenMessage != null) hiddenMessage.Value = Message;
        }
        protected void LoginShowMessage(string Message)
        {
            string controlName = "HiddenMessage";
            System.Web.UI.HtmlControls.HtmlInputHidden hiddenMessage = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl(controlName);

            if (hiddenMessage != null) hiddenMessage.Value = Message;
        }
        #endregion

        #region LocationReplace()
        /// <summary>
        /// 轉址進主要網頁
        /// </summary>
        protected void LocationReplace()
        {
            string script = "top.window.location.replace('Default.aspx');";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "LocationReplace", script, true);
        }
        #endregion

        #region PageReplace()
        /// <summary>
        /// 轉址指定網頁
        /// </summary>
        /// <param name="PageURL">轉址指定網頁位置</param>
        protected void PageReplace(string PageURL)
        {
            string script = string.Format("window.location.replace('{0}');", PageURL);

            ScriptManager.RegisterStartupScript(this, this.GetType(), "PageReplace", script, true);
        }
        #endregion

        #region ReloadPage()
        /// <summary>
        /// 重新載入本頁
        /// </summary>
        protected void ReloadPage()
        {
            this.Response.Redirect(this.Request.Url.AbsoluteUri);
        }
        #endregion

        #region RequestQueryString()
        /// <summary>
        /// 取得參數
        /// </summary>
        protected string RequestQueryString(string AttName)
        {
            string result = this.Request.QueryString[AttName] != null ? this.Request.QueryString[AttName].Trim().Replace(StringFormatException.Mode.CrossSiteScripting) : string.Empty;

            return !String.IsNullOrEmpty(result) ? result : this.Request.Form[AttName] != null ? this.Request.Form[AttName].Trim().Replace(StringFormatException.Mode.CrossSiteScripting) : string.Empty;
        }
        #endregion

        #region LoadDropDownList()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DDL"></param>
        /// <param name="FieldID"></param>
        /// <returns></returns>
        protected DropDownList LoadDropDownList(DropDownList DDL, string FieldID, string sort)
        {
            return this.LoadDropDownList(DDL, FieldID, null, SelectMode.Text, sort);
        }
        /// <summary>
        /// 載入資料至DropDownList
        /// </summary>
        /// <param name="DDL"></param>
        /// <param name="FieldID"></param>
        /// <param name="Select"></param>
        /// <param name="Mode"></param>
        /// <returns></returns>
        protected DropDownList LoadDropDownList(DropDownList DDL, string FieldID, string Select, SelectMode Mode, string sort)
        {

            DDL.Items.Clear();
            DDL.Items.Add(new ListItem("請選擇", ""));

            string strSql = string.Empty;

            switch (FieldID)
            {
                case "13": strSql = "Select it.ItemID, it.ItemName\n"
                                   + "From ItemTable it\n"
                                   + "Inner Join (\n"
                                   + "	Select A.ItemID\n"
                                   + "	From VaccinePrivilege A\n"
                                   + "	Where A.RoleID = {0}) vp\n"
                                   + "	On (vp.ItemID = it.ItemID)\n"
                                   + "Where FieldID = 13\n"
                                   + "Order By " + sort;

                    strSql = string.Format(strSql, UserInfo.RoleID);
                    break;

                case "22": strSql = "Select it.ItemID, it.ItemName\n"
                                     + "From ItemTable it\n"
                                     + "Inner Join (\n"
                                     + "	Select A.ItemID\n"
                                     + "	From CountyPrivilege A\n"
                                     + "	Where A.RoleID = {0}) cp\n"
                                     + "	On (cp.ItemID = it.ItemID)\n"
                                     + "Where FieldID = 22\n"
                                     + "Order By " + sort;

                    strSql = string.Format(strSql, UserInfo.RoleID);
                    break;

                default: strSql = "SELECT ItemID, ItemName FROM ItemTable where FieldID='" + FieldID + "' order by " + sort;
                    break;
            }

            //if (FieldID != "13") strSql = "SELECT ItemID, ItemName FROM ItemTable where FieldID='" + FieldID + "' order by " + sort;
            //else 
            //{
            //    strSql = "Select it.ItemID, it.ItemName\n"
            //        + "From ItemTable it\n"
            //        + "Inner Join (\n"
            //        + "	Select A.ItemID\n"
            //        + "	From VaccinePrivilege A\n"
            //        + "	Where A.RoleID = {0}) vp\n"
            //        + "	On (vp.ItemID = it.ItemID)\n"
            //        + "Where FieldID = 13\n"
            //        + "Order By " + sort;

            //    strSql = string.Format(strSql, UserInfo.RoleID);
            //}

            DataTable dt = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DDL.Items.Add(new ListItem(dt.Rows[i]["ItemName"].ToString(), dt.Rows[i]["ItemID"].ToString()));

                if (Select != null)
                {
                    if (Mode == SelectMode.Text && dt.Rows[i]["ItemName"].ToString().Trim().ToUpper() == Select.Trim().ToUpper()) DDL.Items[DDL.Items.Count - 1].Selected = true;
                    else if (Mode == SelectMode.Value && dt.Rows[i]["ItemID"].ToString().Trim().ToUpper() == Select.Trim().ToUpper()) DDL.Items[DDL.Items.Count - 1].Selected = true;
                }
            }
            DDL.Dispose();

            this.DBConn.Dispose(); this.DBConn = null;

            return DDL;
        }
        protected DropDownList LoadddlVaccinationKind(DropDownList DDL)
        {

            DDL.Items.Clear();
            DDL.Items.Add(new ListItem("請選擇", ""));

            string strSql = "SELECT distinct VaccinationKind FROM ResultsTable order by VaccinationKind";

            DataTable dt = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

            for (int i = 0; i < dt.Rows.Count; i++)
                DDL.Items.Add(new ListItem(dt.Rows[i]["VaccinationKind"].ToString(), dt.Rows[i]["VaccinationKind"].ToString()));

            DDL.Dispose();

            this.DBConn.Dispose(); this.DBConn = null;

            return DDL;
        }

        #endregion

        #region LoadRadioButtonList
        /// <summary>
        /// 載入資料至DropDownList
        /// </summary>
        /// <param name="ddl"></param>
        /// <param name="sSQL"></param>
        /// <param name="sCode"></param>
        /// <param name="sName"></param>
        protected RadioButtonList LoadRadioButtonList(RadioButtonList RbtnList, string FieldID)
        {

            RbtnList.Items.Clear();
            string sSQL = "SELECT ItemID, ItemName FROM ItemTable where FieldID='" + FieldID + "'";
            DataTable dt = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(sSQL);
            //Conn.Dispose();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                RbtnList.Items.Add(new ListItem(dt.Rows[i]["ItemName"].ToString(), dt.Rows[i]["ItemID"].ToString()));
            }
            RbtnList.Dispose();

            this.DBConn.Dispose(); this.DBConn = null;

            return RbtnList;
        }
        #endregion

        #region LoadHidden()
        /// <summary>
        /// 載入資料至DropDownList
        /// </summary>
        /// <param name="ddl"></param>
        /// <param name="sSQL"></param>
        /// <param name="sCode"></param>
        /// <param name="sName"></param>
        protected void LoadHidden(ref System.Web.UI.HtmlControls.HtmlInputHidden InputHidden, string FieldID)
        {
            InputHidden.Value = "";

            string strSql = string.Format("Select ItemID, ItemName From ItemTable Where FieldID='{0}'", FieldID);

            DataTable dt = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string result = string.Format("{0}={1}", dt.Rows[i]["ItemName"].ToString(), dt.Rows[i]["ItemID"].ToString());

                if (InputHidden.Value.Length == 0) InputHidden.Value = result;
                else InputHidden.Value += ";" + result;
            }
            this.DBConn.Dispose(); this.DBConn = null;
        }
        #endregion

        #region ResponseImage()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ImageByte"></param>
        protected void ResponseImage(byte[] ImageByte)
        {
            this.Response.ClearContent();
            this.Response.ContentType = "image/Png";
            this.Response.BinaryWrite(ImageByte);
        }
        #endregion

        #region SpaceLength()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Index"></param>
        /// <returns></returns>
        protected string SpaceLength(int Index, string PrivName)
        {
            string space = "";

            for (int j = 0; j < Index; j++)
            {
                space += "--";
            }
            return space;
        }
        #endregion

        #region BindPrivGroup()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="RoleID"></param>
        /// <returns></returns>
        protected DataTable BindPrivGroup(int RoleID)
        {
            string strSql = string.Empty;

            if (RoleID == -1)
            {
                strSql = this.Select.AllPrivilege();
            }
            else
            {
                strSql = this.Select.RolePrivilegeTable(RoleID);
            }
            DataTable dt = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

            DataTable dt_Copy = dt.Clone();
            try
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row["ParentID"].ToString().Trim().Length == 0)
                        {
                            dt_Copy.ImportRow(row);

                            this.BindPrivChildItem(ref dt_Copy, dt, row["PrivID"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex);
            }
            finally
            {
                this.DBConn.Dispose(); this.DBConn = null;
            }
            return dt_Copy;
        }
        #endregion

        #region BindPrivChildItem()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DTCopy"></param>
        /// <param name="DT"></param>
        /// <param name="ID"></param>
        private void BindPrivChildItem(ref DataTable DTCopy, DataTable DT, string ID)
        {
            DataRow[] rows = DT.Select(string.Format("ParentID='{0}'", ID), "Seq ASC");

            for (int i = 0; i < rows.Length; i++)
            {
                int level = Convert.ToInt32(rows[i]["PrivLevel"].ToString());

                string parentID = rows[i]["ParentID"].ToString();

                if (level > 1)
                {
                    DataRow[] row = DT.Select(string.Format("PrivID='{0}'", parentID));
                    //TODO...James 是否修改
                    rows[i]["PrivName"] = row[0]["PrivName"].ToString() + "--" + rows[i]["PrivName"].ToString();
                    //rows[i]["PrivName"] = rows[i]["PrivName"].ToString();
                }

                DTCopy.ImportRow(rows[i]);

                this.BindPrivChildItem(ref DTCopy, DT, rows[i]["PrivID"].ToString());
            }
        }
        #endregion

        #region bkwpfileSchema
        /// <summary>
        ///  Cirlm Table Schema
        /// </summary>
        public static string bkwpfileSchema
        {
            get
            {
                string bkwpfileSchema = "bkwpfile";

                if (ConfigurationManager.AppSettings["Location"] == null) return bkwpfileSchema;

                switch (ConfigurationManager.AppSettings["Location"].Trim())
                {
                    case "1":
                        bkwpfileSchema = "bkwpfile"; break;
                    case "2":
                        bkwpfileSchema = "fpv.bkwpfile@FILESCANUSER_TESTFPV"; break;
                    case "3":
                        bkwpfileSchema = "fpv.bkwpfile@FILESCANUSER_FPV"; break;
                }

                return bkwpfileSchema;
            }
        }
        #endregion

        #region CirlmSchema
        /// <summary>
        ///  Cirlm Table Schema
        /// </summary>
        public static string CirlmSchema
        {
            get
            {
                string cirlmSchema = "cirlm";

                if (ConfigurationManager.AppSettings["Location"] == null) return cirlmSchema;

                switch (ConfigurationManager.AppSettings["Location"].Trim())
                {
                    case "1":
                        cirlmSchema = "cirlm"; break;
                    case "2":
                        cirlmSchema = "fpv.cirlm@FILESCANUSER_TESTFPV"; break;
                    case "3":
                        cirlmSchema = "fpv.cirlm@FILESCANUSER_FPV"; break;
                }

                return cirlmSchema;
            }
        }
        #endregion

        #region WprecSchema
        /// <summary>
        ///  Wprec Table Schema
        /// </summary>
        public static string WprecSchema
        {
            get
            {
                string wprecSchema = "wprec";

                if (ConfigurationManager.AppSettings["Location"] == null) return wprecSchema;

                switch (ConfigurationManager.AppSettings["Location"].Trim())
                {
                    case "1":
                        wprecSchema = "wprec"; break;
                    case "2":
                        wprecSchema = "fpv.wprec@FILESCANUSER_TESTFPV"; break;
                    case "3":
                        wprecSchema = "fpv.wprec@FILESCANUSER_FPV"; break;
                }

                return wprecSchema;
            }
        }
        #endregion

        #region UsermSchema
        /// <summary>
        ///  userm  Table Schema
        /// </summary>
        public static string UsermSchema 
        {
            get
            {
                string usermSchema = "userm";

                if (ConfigurationManager.AppSettings["Location"] == null) return usermSchema;

                switch (ConfigurationManager.AppSettings["Location"].Trim())
                {
                    case "1":
                        usermSchema = "userm"; break;
                    case "2":
                        usermSchema = "fpv.userm@FILESCANUSER_TESTFPV"; break;
                    case "3":
                        usermSchema = "fpv.userm@FILESCANUSER_FPV"; break;
                }

                return usermSchema;
            }
        }
        #endregion
    }
}