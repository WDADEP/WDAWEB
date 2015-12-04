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
            public static string Tm051 = "51";
            /// <summary>
            /// 開啟影像調閱
            /// </summary>
            public static string Tm052 = "52";
            /// <summary>
            /// 調閱所有案件影像
            /// </summary>
            public static string Tm053 = "53";
            /// <summary>
            /// 統計報表查詢-匯出業務CSV報表
            /// </summary>
            public static string Tm054 = "54";
            /// <summary>
            /// 統計報表查詢-匯出單位CSV報表
            /// </summary>
            public static string Tm055 = "55";
            /// <summary>
            /// 統計報表查詢-匯出使用者CSV報表
            /// </summary>
            public static string Tm056 = "56";
            /// <summary>
            /// 人員管理-使用者新增成功
            /// </summary>
            public static string Tm057 = "57";
            /// <summary>
            /// 人員管理-使用者新增失敗
            /// </summary>
            public static string Tm058 = "58";
            /// <summary>
            /// 人員管理-使用者修改成功
            /// </summary>
            public static string Tm059 = "59";
            /// <summary>
            /// 人員管理-使用者修改失敗
            /// </summary>
            public static string Tm060 = "60";
            /// <summary>
            /// 人員管理-使用者停用成功
            /// </summary>
            public static string Tm061 = "61";
            /// <summary>
            /// 人員管理-使用者停用失敗
            /// </summary>
            public static string Tm062 = "62";
            /// <summary>
            /// 角色管理-角色名稱新增成功
            /// </summary>
            public static string Tm063 = "63";
            /// <summary>
            /// 角色管理-角色名稱新增失敗
            /// </summary>
            public static string Tm064 = "64";
            /// <summary>
            /// 角色管理-角色名稱修改成功
            /// </summary>
            public static string Tm065 = "65";
            /// <summary>
            /// 角色管理-角色名稱修改失敗
            /// </summary>
            public static string Tm066 = "66";
            /// <summary>
            /// 角色管理-角色名稱刪除成功
            /// </summary>
            public static string Tm067 = "67";
            /// <summary>
            /// 角色管理-角色名稱刪除失敗
            /// </summary>
            public static string Tm068 = "68";
            /// <summary>
            /// 交易記錄查詢-匯出CSV報表
            /// </summary>
            public static string Tm069 = "69";
            /// <summary>
            /// 交易記錄查詢-清除記錄成功
            /// </summary>
            public static string Tm070 = "70";
            /// <summary>
            /// 交易記錄查詢-清除記錄失敗
            /// </summary>
            public static string Tm071 = "71";
            /// <summary>
            /// 環境設定-修改成功
            /// </summary>
            public static string Tm072 = "72";
            /// <summary>
            /// 環境設定-修改失敗
            /// </summary>
            public static string Tm073 = "73";
            /// <summary>
            /// 上傳時間管理-新增成功
            /// </summary>
            public static string Tm074 = "74";
            /// <summary>
            /// 上傳時間管理-新增失敗
            /// </summary>
            public static string Tm075 = "75";
            /// <summary>
            /// 上傳時間管理-修改成功
            /// </summary>
            public static string Tm076 = "76";
            /// <summary>
            /// 上傳時間管理-修改失敗
            /// </summary>
            public static string Tm077 = "77";
            /// <summary>
            /// 上傳時間管理-刪除成功
            /// </summary>
            public static string Tm078 = "78";
            /// <summary>
            /// 上傳時間管理-刪除失敗
            /// </summary>
            public static string Tm079 = "79";
            /// <summary>
            /// 單位批次更新-新增成功
            /// </summary>
            public static string Tm080 = "80";
            /// <summary>
            /// 單位批次更新-新增成功
            /// </summary>
            public static string Tm081 = "81";
            /// <summary>
            /// 單位批次更新-修改成功
            /// </summary>
            public static string Tm082 = "82";
            /// <summary>
            /// 單位批次更新-修改失敗
            /// </summary>
            public static string Tm083 = "83";
            /// <summary>
            ///  單位批次更新-刪除成功
            /// </summary>
            public static string Tm084 = "84";
            /// <summary>
            /// 單位批次更新-刪除失敗
            /// </summary>
            public static string Tm085 = "85";
            /// <summary>
            /// 註記管理-新增成功
            /// </summary>
            public static string Tm086 = "86";
            /// <summary>
            /// 註記管理-新增失敗
            /// </summary>
            public static string Tm087 = "87";
            /// <summary>
            /// 註記管理-修改成功
            /// </summary>
            public static string Tm088 = "88";
            /// <summary>
            /// 註記管理-修改失敗
            /// </summary>
            public static string Tm089 = "89";
            /// <summary>
            /// 註記管理-刪除成功
            /// </summary>
            public static string Tm090 = "90";
            /// <summary>
            /// 註記管理-刪除失敗
            /// </summary>
            public static string Tm091 = "91";
            /// <summary>
            ///索引管理-新增成功
            /// </summary>
            public static string Tm092 = "92";
            /// <summary>
            /// 索引管理-新增失敗
            /// </summary>
            public static string Tm093 = "93";
            /// <summary>
            /// 索引管理-修改成功
            /// </summary>
            public static string Tm094 = "94";
            /// <summary>
            /// 索引管理-修改失敗
            /// </summary>
            public static string Tm095 = "95";
            /// <summary>
            /// 索引管理-刪除成功
            /// </summary>
            public static string Tm096 = "96";
            /// <summary>
            ///  索引管理-刪除失敗
            /// </summary>
            public static string Tm097 = "97";
            /// <summary>
            /// 磁碟目錄管理-新增成功
            /// </summary>
            public static string Tm098 = "98";
            /// <summary>
            /// 磁碟目錄管理-新增失敗
            /// </summary>
            public static string Tm099 = "99";
            /// <summary>
            /// 磁碟目錄管理-修改成功
            /// </summary>
            public static string Tm100 = "100";
            /// <summary>
            /// 磁碟目錄管理-修改失敗
            /// </summary>
            public static string Tm101 = "101";
            /// <summary>
            /// 文件管理-新增成功
            /// </summary>
            public static string Tm102 = "102";
            /// <summary>
            /// 文件管理-新增失敗
            /// </summary>
            public static string Tm103 = "103";
            /// <summary>
            /// 文件管理-修改成功
            /// </summary>
            public static string Tm104 = "104";
            /// <summary>
            /// 文件管理-修改失敗
            /// </summary>
            public static string Tm105 = "105";
            /// <summary>
            /// 文件管理-刪除成功
            /// </summary>
            public static string Tm106 = "106";
            /// <summary>
            /// 文件管理-刪除失敗
            /// </summary>
            public static string Tm107 = "107";
            /// <summary>
            /// 文件管理-變更層級成功
            /// </summary>
            public static string Tm108 = "108";
            /// <summary>
            /// 文件管理-變更層級失敗
            /// </summary>
            public static string Tm109 = "109";
            /// <summary>
            /// 文件管理-變更順序成功
            /// </summary>
            public static string Tm110 = "110";
            /// <summary>
            /// 文件管理-變更順序失敗
            /// </summary>
            public static string Tm111 = "111";
            /// <summary>
            /// 檔案類型管理-新增成功
            /// </summary>
            public static string Tm112 = "112";
            /// <summary>
            /// 檔案類型管理-新增失敗
            /// </summary>
            public static string Tm113 = "113";
            /// <summary>
            /// 檔案類型管理-修改成功
            /// </summary>
            public static string Tm114 = "114";
            /// <summary>
            /// 檔案類型管理-修改失敗
            /// </summary>
            public static string Tm115 = "115";
            /// <summary>
            /// 檔案類型管理-刪除成功
            /// </summary>
            public static string Tm116 = "116";
            /// <summary>
            /// 檔案類型管理-刪除失敗
            /// </summary>
            public static string Tm117 = "117";
            /// <summary>
            /// 檔案刪除管理-刪除垃圾桶影像成功
            /// </summary>
            public static string Tm118 = "118";
            /// <summary>
            /// 檔案刪除管理-刪除垃圾桶影像失敗
            /// </summary>
            public static string Tm119 = "119";
            /// <summary>
            /// 檔案刪除管理-刪除整個案件成功
            /// </summary>
            public static string Tm120 = "120";
            /// <summary>
            /// 檔案刪除管理-刪除整個案件失敗
            /// </summary>
            public static string Tm121 = "121";
            /// <summary>
            /// 單位群組管理-新增成功
            /// </summary>
            public static string Tm122 = "122";
            /// <summary>
            /// 單位群組管理-新增失敗
            /// </summary>
            public static string Tm123 = "123";
            /// <summary>
            /// 單位群組管理-修改成功
            /// </summary>
            public static string Tm124 = "124";
            /// <summary>
            /// 單位群組管理-修改失敗
            /// </summary>
            public static string Tm125 = "125";
            /// <summary>
            /// 單位群組管理-刪除成功
            /// </summary>
            public static string Tm126 = "126";
            /// <summary>
            /// 單位群組管理-刪除失敗
            /// </summary>
            public static string Tm127 = "127";
            /// <summary>
            /// 綜合查詢-匯出CSV報表
            /// </summary>
            public static string Tm128 = "128";
            /// <summary>
            /// 光碟燒錄管理-修改成功
            /// </summary>
            public static string Tm129 = "129";
            /// <summary>
            /// 光碟燒錄管理-修改失敗
            /// </summary>
            public static string Tm130 = "130";
            /// <summary>
            /// API操作-透過案件編號更新案件狀態
            /// </summary>
            public static string Tm131 = "131";
            /// <summary>
            /// API操作-透過案件編號更新索引值
            /// </summary>
            public static string Tm132 = "132";
            /// <summary>
            /// API操作-透過案件編號新增案件
            /// </summary>
            public static string Tm133 = "133";
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
            + "	[CaseID]\n"
            + "	,[JobItemID]\n"
            + "	,[RepNo]\n"
            + "	,[DetailNo]\n"
            + "	,[UserID]\n"
            + "	,[UserName]\n"
            + "	,[UserUnit]\n"
            + "	,[TransDateTime]\n"
            + "	,[TransIP]\n"
            + "	,[TransResult]\n"
            + "	,[Comments]\n"
            + ")\n"
            + "Values (\n"
            + "	{0}\n"
            + "	,{1}\n"
            + "	,N'{2}'\n"
            + "	,N'{3}'\n"
            + "	,{4}\n"
            + "	,N'{5}'\n"
            + "	,{6}\n"
            + "	,GETDATE()\n"
            + "	,N'{7}'\n"
            + "	,{8}\n"
            + "	,N'{9}'\n"
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
        public void LogMonitor(string CaseID,string JobItemID,string RepNo ,string DetailNo ,string UserID, string UserName,string UserUnit,string TransIP, string TransResult, string Comments)
        {
            try
            {
                if (UserID.Length == 0) throw new Exception("UserID Is Null");

                if (Comments.GetStringLength() >= 500) Comments = Comments.GetString(500);

                LogSQL = string.Format(LogSQL,
                    CaseID,
                    JobItemID,
                    RepNo,
                    DetailNo,
                    UserID,
                    UserName,
                    UserUnit,
                    TransIP,
                    TransResult,
                    Comments//Comments
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