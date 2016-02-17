using Log;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Services;
using WDA.Class;

namespace WDA
{
    /// <summary>
    ///AspNetAjaxInAction 的摘要描述
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下列一行。
    [System.Web.Script.Services.ScriptService]
    public class AspNetAjaxInAction : System.Web.Services.WebService
    {
        #region DulyAdjusted
        public class DulyAdjusted
        {
            public DulyAdjusted() { }

            #region DulyAdjustedInfos
            /// <summary>
            /// 
            /// </summary>
            public class DulyAdjustedInfos
            {
                public string WpinDate = string.Empty;
                public string WpouTno = string.Empty;
                public string WpoutDate = string.Empty;

                public string CaseNo = string.Empty;
                public string ApplKind = string.Empty;
                public string CommName = string.Empty;

                public string Receiver = string.Empty;
                public string RName = string.Empty;

                public string Transt = string.Empty;
                public string Getime = string.Empty;
                //public string Workerid = string.Empty;
                //public string WName = string.Empty;
            }
            #endregion

        } 
        #endregion

        #region AlsoFile
        public class AlsoFile
        {
            public AlsoFile() { }

            #region AlsoFileInfos
            /// <summary>
            /// 
            /// </summary>
            public class AlsoFileInfos
            {
                public string Wpinno = string.Empty;
                public string Wpoutno = string.Empty;

                public string Transt = string.Empty;
                public string Receiver = string.Empty;
                public string Rname = string.Empty;
                public string Kind = string.Empty;
                public string Redate = string.Empty;
                public string Exten = string.Empty;
                public string ExtensionRedate = string.Empty;
                public string ReturnMessage = string.Empty;
            }
            #endregion

        }
        #endregion

        #region PaperAlsoFile
        public class PaperAlsoFile
        {
            public PaperAlsoFile() { }

            #region PaperAlsoFileInfos
            /// <summary>
            /// 
            /// </summary>
            public class PaperAlsoFileInfos
            {
                public string Wpinno = string.Empty;
                public string Wpoutno = string.Empty;

                public string Receiver = string.Empty;
                public string Tel = string.Empty;
                public string Wpstatus = string.Empty;
                public string Borrdate = string.Empty;
                public string Redate = string.Empty;
            }
            #endregion

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
        protected ILog Log
        {
            get { if (this._Log == null) this._Log = new WebLog("WDA"); return this._Log; }
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

        #region CheckUserName()
        [WebMethod]
        public bool CheckUserName(string UserName)
        {
            string strSql = string.Empty;
            string where = string.Empty;
            try
            {
                where = string.Format("And UserName ='{0}'", UserName);

                strSql = this.Select.UsersCheck(where);

                this.WriteLog(Mode.LogMode.DEBUG, strSql);

                bool check = this.DBConn.GeneralSqlCmd.ExecuteScalar(strSql);

                this.WriteLog(string.Format("CheckUserName={0}", check.ToString()));

                return check;
            }
            catch (System.Exception ex)
            {
                this.WriteLog(Mode.LogMode.ERROR, string.Format("CheckUserName.Exception : {0}", ex.Message));

                return false;
            }
            finally
            {
                this._Log = null;

                this.DBConn.Dispose();
            }
        }
        #endregion

        #region GetDulyAdjusted()
        [WebMethod]
        public List<AspNetAjaxInAction.DulyAdjusted.DulyAdjustedInfos> GetDulyAdjusted(string Wpinno)
        {
            string strSql = string.Empty;
            string where = string.Empty;
            DataTable dt = null;

            AspNetAjaxInAction.DulyAdjusted.DulyAdjustedInfos DulyAdjustedInfos = null;

            List<AspNetAjaxInAction.DulyAdjusted.DulyAdjustedInfos> ListDulyAdjustedInfos = new List<DulyAdjusted.DulyAdjustedInfos>();

            try
            {
                DulyAdjustedInfos = new DulyAdjusted.DulyAdjustedInfos();

                where = string.Format("And wpb.wpinno ='{0}'", Wpinno);

                strSql = this.Select.DulyAdjustedInfo(where);

                this.WriteLog(Mode.LogMode.DEBUG, strSql);

                dt = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                if (dt.Rows.Count > 0)
                {
                    DulyAdjustedInfos.WpinDate = dt.Rows[0]["wpindate"].ToString();
                    DulyAdjustedInfos.WpouTno = dt.Rows[0]["wpoutno"].ToString();
                    DulyAdjustedInfos.WpoutDate = dt.Rows[0]["wpoutdate"].ToString();
                    DulyAdjustedInfos.CaseNo = dt.Rows[0]["caseno"].ToString();
                    DulyAdjustedInfos.ApplKind = dt.Rows[0]["kindName"].ToString();
                    DulyAdjustedInfos.CommName = dt.Rows[0]["commname"].ToString();
                    DulyAdjustedInfos.Receiver = dt.Rows[0]["receiver"].ToString();
                    DulyAdjustedInfos.RName = dt.Rows[0]["rname"].ToString();
                    DulyAdjustedInfos.Transt = dt.Rows[0]["transt"].ToString();
                    DulyAdjustedInfos.Getime = dt.Rows[0]["getime"].ToString();
                }

                ListDulyAdjustedInfos.Add(DulyAdjustedInfos);

                return ListDulyAdjustedInfos;
            }
            catch (System.Exception ex)
            {
                this.WriteLog(Mode.LogMode.ERROR, string.Format("GetDulyAdjusted.Exception : {0}", ex.Message));

                return ListDulyAdjustedInfos;
            }
            finally
            {
                this._Log = null;

                this.DBConn.Dispose();

                if (dt != null) { dt.Dispose(); dt = null; }
            }
        }
        #endregion

        #region GetAlsoFile()
        [WebMethod]
        public List<AspNetAjaxInAction.AlsoFile.AlsoFileInfos> GetAlsoFile(string No, string ViewType, string UserName)
        {
            string strSql = string.Empty;
            string where = string.Empty;
            string Redate = string.Empty;
            string ExtensionRedate = string.Empty;
            DataTable dt = null;

            AspNetAjaxInAction.AlsoFile.AlsoFileInfos AlsoFileInfos = null;

            List<AspNetAjaxInAction.AlsoFile.AlsoFileInfos> ListAlsoFileInfos = new List<AlsoFile.AlsoFileInfos>();

            try
            {
                AlsoFileInfos = new AlsoFile.AlsoFileInfos();

                where = string.Format("And wpb.wpinno ='{0}' And wpb.ViewType ='{1}' And wpb.receiver ='{2}'", No, ViewType, UserName);

                #region ReturnMessage

                strSql = this.Select.AlsoFileInfosExten(where);

                this.WriteLog(Mode.LogMode.DEBUG, strSql);

                dt = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                if (dt.Rows.Count > 0)
                {
                    AlsoFileInfos.ReturnMessage = "該筆資料申請展期中";

                    ListAlsoFileInfos.Add(AlsoFileInfos);

                    return ListAlsoFileInfos;
                }
                #endregion

                strSql = this.Select.AlsoFileInfos(where);

                this.WriteLog(Mode.LogMode.DEBUG, strSql);

                dt = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Viewtype"].ToString() == "1")
                    {
                        if (string.IsNullOrEmpty(dt.Rows[0]["EXTENSIONDATE"].ToString()))
                        {
                            switch (dt.Rows[0]["kind"].ToString())
                            {
                                case "1":
                                    Redate = DateTime.Parse(dt.Rows[0]["Transt"].ToString()).AddDays(8).ToString("yyyy/MM/dd HH:mm:ss");
                                    ExtensionRedate = DateTime.Parse(dt.Rows[0]["Transt"].ToString()).AddDays(15).ToString("yyyy/MM/dd HH:mm:ss");
                                    break;
                                case "2":
                                    Redate = DateTime.Parse(dt.Rows[0]["Transt"].ToString()).AddDays(8).ToString("yyyy/MM/dd HH:mm:ss");
                                    ExtensionRedate = DateTime.Parse(dt.Rows[0]["Transt"].ToString()).AddDays(15).ToString("yyyy/MM/dd HH:mm:ss");
                                    break;
                                case "3":
                                    Redate = DateTime.Parse(dt.Rows[0]["Transt"].ToString()).AddDays(366).ToString("yyyy/MM/dd HH:mm:ss");
                                    ExtensionRedate = DateTime.Parse(dt.Rows[0]["Transt"].ToString()).AddDays(456).ToString("yyyy/MM/dd HH:mm:ss");
                                    break;
                                default: break;
                            }
                        }
                        else
                        {
                            switch (dt.Rows[0]["kind"].ToString())
                            {
                                case "1":
                                    Redate = DateTime.Parse(dt.Rows[0]["EXTENSIONDATE"].ToString()).AddDays(8).ToString("yyyy/MM/dd HH:mm:ss");
                                    ExtensionRedate = DateTime.Parse(dt.Rows[0]["EXTENSIONDATE"].ToString()).AddDays(15).ToString("yyyy/MM/dd HH:mm:ss");
                                    break;
                                case "2":
                                    Redate = DateTime.Parse(dt.Rows[0]["EXTENSIONDATE"].ToString()).AddDays(8).ToString("yyyy/MM/dd HH:mm:ss");
                                    ExtensionRedate = DateTime.Parse(dt.Rows[0]["EXTENSIONDATE"].ToString()).AddDays(15).ToString("yyyy/MM/dd HH:mm:ss");
                                    break;
                                case "3":
                                    Redate = DateTime.Parse(dt.Rows[0]["EXTENSIONDATE"].ToString()).AddDays(366).ToString("yyyy/MM/dd HH:mm:ss");
                                    ExtensionRedate = DateTime.Parse(dt.Rows[0]["EXTENSIONDATE"].ToString()).AddDays(456).ToString("yyyy/MM/dd HH:mm:ss");
                                    break;
                                default: break;
                            }
                        }
                    }
                    else if (dt.Rows[0]["Viewtype"].ToString() == "2")
                    {
                        if (dt.Rows[0]["kind"].ToString() == "3" && !string.IsNullOrEmpty(dt.Rows[0]["EXTENSIONDATE"].ToString()))
                        {
                            DateTime STime = DateTime.Parse(DateTime.Parse(dt.Rows[0]["Transt"].ToString()).AddDays(15).ToString("yyyy/MM/dd HH:mm:ss")); //起始日
                            DateTime ETime = DateTime.Parse(dt.Rows[0]["EXTENSIONDATE"].ToString());
                            TimeSpan Total = ETime.Subtract(STime); //日期相減

                            if (Total.Days > 90)
                            {
                                AlsoFileInfos.ReturnMessage = "行政救濟案件展期天數則不得超過3個月";
                                ListAlsoFileInfos.Add(AlsoFileInfos);
                                return ListAlsoFileInfos;
                            }
                        }

                        if (string.IsNullOrEmpty(dt.Rows[0]["EXTENSIONDATE"].ToString()))
                        {
                            Redate = DateTime.Parse(dt.Rows[0]["Transt"].ToString()).AddDays(8).ToString("yyyy/MM/dd HH:mm:ss");
                            ExtensionRedate = DateTime.Parse(dt.Rows[0]["Transt"].ToString()).AddDays(15).ToString("yyyy/MM/dd HH:mm:ss");
                        }
                        else
                        {
                            Redate = DateTime.Parse(dt.Rows[0]["EXTENSIONDATE"].ToString()).AddDays(8).ToString("yyyy/MM/dd HH:mm:ss");
                            ExtensionRedate = DateTime.Parse(dt.Rows[0]["EXTENSIONDATE"].ToString()).AddDays(15).ToString("yyyy/MM/dd HH:mm:ss");
                        }
                    }
                     
                    AlsoFileInfos.Wpinno = dt.Rows[0]["Wpinno"].ToString();
                    AlsoFileInfos.Wpoutno = dt.Rows[0]["Wpoutno"].ToString();
                    AlsoFileInfos.Transt = DateTime.Parse(dt.Rows[0]["Transt"].ToString()).ToString("yyyy/MM/dd HH:mm:ss");
                    AlsoFileInfos.Receiver = dt.Rows[0]["Receiver"].ToString();
                    AlsoFileInfos.Rname = dt.Rows[0]["RealName"].ToString();
                    AlsoFileInfos.Kind = dt.Rows[0]["kindName"].ToString();
                    AlsoFileInfos.Redate = Redate;
                    AlsoFileInfos.Exten = dt.Rows[0]["Exten"].ToString();
                    AlsoFileInfos.ExtensionRedate = ExtensionRedate;
                }

                ListAlsoFileInfos.Add(AlsoFileInfos);

                return ListAlsoFileInfos;
            }
            catch (System.Exception ex)
            {
                this.WriteLog(Mode.LogMode.ERROR, string.Format("GetAlsoFile.Exception : {0}", ex.Message));

                return ListAlsoFileInfos;
            }
            finally
            {
                this._Log = null;

                this.DBConn.Dispose();

                if (dt != null) { dt.Dispose(); dt = null; }
            }
        }
        #endregion

        #region GetAlsoFileByVisa()
        [WebMethod]
        public List<AspNetAjaxInAction.AlsoFile.AlsoFileInfos> GetAlsoFileByVisa(string No, int Type ,string UserName)
        {
            string strSql = string.Empty;
            string where = string.Empty;
            string Transt = string.Empty;
            string Redate = string.Empty;
            string ExtensionRedate = string.Empty;
            DataTable dt = null;

            AspNetAjaxInAction.AlsoFile.AlsoFileInfos AlsoFileInfos = null;

            List<AspNetAjaxInAction.AlsoFile.AlsoFileInfos> ListAlsoFileInfos = new List<AlsoFile.AlsoFileInfos>();

            try
            {
                AlsoFileInfos = new AlsoFile.AlsoFileInfos();

                if (Type == 0)
                {
                    where = string.Format("And wpb.wpinno ='{0}' And APPROVEUSERID='{1}'", No,UserName);
                }
                else if (Type == 1)
                {
                    where = string.Format("And wpb.Wpoutno ='{0}'And APPROVEUSERID='{1}'", No, UserName);
                }

                strSql = this.Select.GetAlsoFileByVisa(where);

                this.WriteLog(Mode.LogMode.DEBUG, strSql);

                dt = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                if (dt.Rows.Count > 0)
                {
                    switch (dt.Rows[0]["kind"].ToString())
                    {
                        case "1":
                            Redate = DateTime.Parse(dt.Rows[0]["Transt"].ToString()).AddDays(8).ToString("yyyy/MM/dd HH:mm:ss");
                            ExtensionRedate = DateTime.Parse(dt.Rows[0]["Transt"].ToString()).AddDays(15).ToString("yyyy/MM/dd HH:mm:ss");
                            break;
                        case "2":
                            Redate = DateTime.Parse(dt.Rows[0]["Transt"].ToString()).AddDays(8).ToString("yyyy/MM/dd HH:mm:ss");
                            ExtensionRedate = DateTime.Parse(dt.Rows[0]["Transt"].ToString()).AddDays(15).ToString("yyyy/MM/dd HH:mm:ss");
                            break;
                        case "3":
                            Redate = DateTime.Parse(dt.Rows[0]["Transt"].ToString()).AddDays(366).ToString("yyyy/MM/dd HH:mm:ss");
                            ExtensionRedate = DateTime.Parse(dt.Rows[0]["Transt"].ToString()).AddDays(456).ToString("yyyy/MM/dd HH:mm:ss");
                            break;
                        default: break;
                    }

                    AlsoFileInfos.Wpinno = dt.Rows[0]["Wpinno"].ToString();
                    AlsoFileInfos.Wpoutno = dt.Rows[0]["Wpoutno"].ToString();
                    AlsoFileInfos.Transt = DateTime.Parse(dt.Rows[0]["Transt"].ToString()).ToString("yyyy/MM/dd HH:mm:ss");
                    AlsoFileInfos.Receiver = dt.Rows[0]["Receiver"].ToString();
                    AlsoFileInfos.Rname = dt.Rows[0]["RealName"].ToString();
                    AlsoFileInfos.Kind = dt.Rows[0]["kindName"].ToString();
                    AlsoFileInfos.Redate = Redate;
                    AlsoFileInfos.Exten = dt.Rows[0]["Exten"].ToString();
                    AlsoFileInfos.ExtensionRedate = ExtensionRedate;
                }

                ListAlsoFileInfos.Add(AlsoFileInfos);

                return ListAlsoFileInfos;
            }
            catch (System.Exception ex)
            {
                this.WriteLog(Mode.LogMode.ERROR, string.Format("GetAlsoFileByVisa.Exception : {0}", ex.Message));

                return ListAlsoFileInfos;
            }
            finally
            {
                this._Log = null;

                this.DBConn.Dispose();

                if (dt != null) { dt.Dispose(); dt = null; }
            }
        }
        #endregion

        #region GetPaperAlsoFile()
        [WebMethod]
        public List<AspNetAjaxInAction.PaperAlsoFile.PaperAlsoFileInfos> GetPaperAlsoFile(string No, int Type)
        {
            string strSql = string.Empty;
            string where = string.Empty;
            string Redate = string.Empty;
            string Wpstatus = string.Empty;
            DataTable dt = null;

            AspNetAjaxInAction.PaperAlsoFile.PaperAlsoFileInfos PaperAlsoFileInfos = null;

            List<AspNetAjaxInAction.PaperAlsoFile.PaperAlsoFileInfos> ListPaperAlsoFileInfos = new List<PaperAlsoFile.PaperAlsoFileInfos>();

            try
            {
                PaperAlsoFileInfos = new PaperAlsoFile.PaperAlsoFileInfos();

                if (Type == 0)
                {
                    where = string.Format("And wpb.wpinno ='{0}'", No);
                }
                else if (Type == 1)
                {
                    where = string.Format("And wpb.Wpoutno ='{0}'", No);
                }

                strSql = this.Select.PaperAlsoFileInfos(where);

                this.WriteLog(Mode.LogMode.DEBUG, strSql);

                dt = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                if (dt.Rows.Count > 0)
                {
                    switch (dt.Rows[0]["kind"].ToString())
                    {
                        case "1":
                            Redate = DateTime.Parse(dt.Rows[0]["Transt"].ToString()).AddDays(8).ToString("yyyy/MM/dd HH:mm:ss");
                            break;
                        case "2":
                            Redate = DateTime.Parse(dt.Rows[0]["Transt"].ToString()).AddDays(8).ToString("yyyy/MM/dd HH:mm:ss");
                            break;
                        case "3":
                            Redate = DateTime.Parse(dt.Rows[0]["Transt"].ToString()).AddDays(366).ToString("yyyy/MM/dd HH:mm:ss");
                            break;
                        default: break;
                    }

                    Wpstatus = string.IsNullOrEmpty(dt.Rows[0]["Redate"].ToString()) ? "未還檔" : "已還檔";

                    PaperAlsoFileInfos.Wpinno = dt.Rows[0]["Wpinno"].ToString();
                    PaperAlsoFileInfos.Wpoutno = dt.Rows[0]["Wpoutno"].ToString();
                    PaperAlsoFileInfos.Receiver = dt.Rows[0]["Receiver"].ToString();
                    PaperAlsoFileInfos.Tel = dt.Rows[0]["Tel"].ToString();

                    PaperAlsoFileInfos.Wpstatus = Wpstatus;
                    PaperAlsoFileInfos.Borrdate = DateTime.Parse(dt.Rows[0]["Transt"].ToString()).ToString("yyyy/MM/dd HH:mm:ss");
                    //PaperAlsoFileInfos.Redate = Redate;
                    PaperAlsoFileInfos.Redate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                }

                ListPaperAlsoFileInfos.Add(PaperAlsoFileInfos);

                return ListPaperAlsoFileInfos;
            }
            catch (System.Exception ex)
            {
                this.WriteLog(Mode.LogMode.ERROR, string.Format("GetPaperAlsoFile.Exception : {0}", ex.Message));

                return ListPaperAlsoFileInfos;
            }
            finally
            {
                this._Log = null;

                this.DBConn.Dispose();

                if (dt != null) { dt.Dispose(); dt = null; }
            }
        }
        #endregion
    }
}
