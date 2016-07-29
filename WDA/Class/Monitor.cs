using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WDA.Class
{
    public class Monitor : PageUtility
    {
        #region MSGID
        /// <summary>
        /// Message ID List
        /// </summary>
        public class MSGID
        {
            /// <summary>
            /// 開啟影像掃描
            /// </summary>
            public static string WDA01 = "1";
            /// <summary>
            /// 開啟掃描清單
            /// </summary>
            public static string WDA02 = "2";
            /// <summary>
            /// 上傳影像查詢
            /// </summary>
            public static string WDA03 = "3";
            /// <summary>
            /// 歸檔登入(管理)
            /// </summary>
            public static string WDA04 = "4";
            /// <summary>
            /// 歸檔登入(新增)
            /// </summary>
            public static string WDA05 = "5";
            /// <summary>
            /// 預約借檔(新增)
            /// </summary>
            public static string WDA06 = "6";
            /// <summary>
            /// 預約借檔(取消)
            /// </summary>
            public static string WDA07 = "7";
            /// <summary>
            /// 預約借檔(簽核)
            /// </summary>
            public static string WDA08 = "8";
            /// <summary>
            /// 預約借檔(列印)
            /// </summary>
            public static string WDA09 = "9";
            /// <summary>
            /// 調妥新增
            /// </summary>
            public static string WDA10 = "10";
            /// <summary>
            /// 調妥查詢
            /// </summary>
            public static string WDA11 = "11";
            /// <summary>
            /// 借檔催還
            /// </summary>
            public static string WDA12 = "12";
            /// <summary>
            /// 紙本還檔
            /// </summary>
            public static string WDA13 = "13";
            /// <summary>
            /// 還檔展期
            /// </summary>
            public static string WDA14 = "14";
            /// <summary>
            /// 簽准展期
            /// </summary>
            public static string WDA15 = "15";
            /// <summary>
            /// 簽收功能
            /// </summary>
            public static string WDA16 = "16";

        }
        #endregion

        #region LogSQL
        /// <summary>
        /// Log SQL Format
        /// </summary>
        /// 
        private string _LogSQL = string.Empty;

        private string LogSQL
        {
            get
            {
                if (string.IsNullOrEmpty(this._LogSQL))
                {
                    this._LogSQL = "Insert Into LogTable (\n"
            + "	WPINNO\n"
            + "	,USERNAME\n"
            + "	,REALNAME\n"
            + "	,TRANSDATETIME\n"
            + "	,TRANSIP\n"
            + "	,TRANSRESULT\n"
            + "	,COMMENTS\n"
            + ")\n"
            + "Values (\n"
            + "	N'{0}'\n"
            + "	,N'{1}'\n"
            + "	,N'{2}'\n"
            + "	,sysdate\n"
            + "	,N'{3}'\n"
            + "	,N'{4}'\n"
            + "	,N'{5}'\n"
            + ")\n";
                }
                return this._LogSQL;
            }
            set { this._LogSQL = value; }
        }
        #endregion

        #region LogMonitor()
        /// <summary>
        /// 
        /// </summary>
        public void LogMonitor(string WPINNO, string USERNAME, string REALNAME, string TRANSIP, string TRANSRESULT, string COMMENTS)
        {
            try
            {
                if (COMMENTS.GetStringLength() >= 500) COMMENTS = COMMENTS.GetString(500);

                LogSQL = string.Format(LogSQL,
                    WPINNO,
                    USERNAME,
                    REALNAME,
                    TRANSIP,
                    TRANSRESULT,
                    COMMENTS
                );
                this.CommitLog(LogSQL);
            }
            catch (System.Exception ex)
            {
                this.WriteLog(global::Log.Mode.LogMode.ERROR, string.Format("Monitor.LogMonitor.Exception : {0}", ex));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void LogMonitor(string MsgID, string RepNo, string Comments)
        {
            try
            {
                string userId = Session[PageUtility.SessionName.UserID] != null ? Session[PageUtility.SessionName.UserID].ToString() : string.Empty;

                if (userId.Length == 0) throw new Exception("UserID Is Null");

                if (Comments.GetStringLength() >= 500) Comments = Comments.GetString(500);

                LogSQL = string.Format(LogSQL,
                    RepNo,
                    string.Empty,
                    this.UserInfo.UserName,
                    this.UserInfo.RealName,
                    Session[SessionName.ClientIP].ToString(),//IP
                     MsgID,//MsgID
                    Comments//Comments
                );
                this.CommitLog(LogSQL);
            }
            catch (System.Exception ex)
            {
                this.WriteLog(global::Log.Mode.LogMode.ERROR, string.Format("Monitor.LogMonitor.Exception : {0}", ex));
            }
        }
        #endregion

        #region CommitLog()
        /// <summary>
        /// 
        /// </summary>
        private void CommitLog(string SQL)
        {
            try
            {
                this.WriteLog(global::Log.Mode.LogMode.DEBUG, string.Format("Log.SQL : {0}", SQL));

                this.DBConnLog.GeneralSqlCmd.ExecuteNonQuery(SQL);
            }
            catch (System.Exception ex)
            {
                this.WriteLog(global::Log.Mode.LogMode.ERROR, string.Format("Monitor.Commit.Exception : {0}", ex));
            }
            finally
            {
                if (this.DBConnLog != null)
                {
                    this.DBConnLog.Dispose(); DBConnLog = null;
                }
            }
        }
        #endregion
    }
}