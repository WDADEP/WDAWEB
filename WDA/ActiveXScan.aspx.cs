using Log;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WDA.Class;

namespace WDA
{
    public partial class ActiveXScan : PageUtility
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadPage(true);

            if (!IsPostBack)
            {
                string url = string.Empty; string pama = string.Empty; string script = string.Empty; string isEncrypt = string.Empty;
                string jobItemID = string.Empty; string viewerPriv = string.Empty;
                try
                {
                    string caseSet = this.RequestQueryString("CaseSet");
                    string wpinno = this.RequestQueryString("wpinno");
                    string sIsEncrypt = this.GetSystem("IsEncrypt");

                    //取用戶真實IP1，預留看是否要用IP控管掃描工作站
                    string userIP = this.Request.ServerVariables["REMOTE_ADDR"].ToString();

                    string httpType = this.Request.ServerVariables["HTTPS"].ToString();

                    string httpLink = httpType.ToLower() == "on" ? " https" : "http";

                    string serverName = this.Request.ServerVariables["SERVER_NAME"].ToString();
                    //ex:80
                    string serverPort = this.Request.ServerVariables["SERVER_PORT"].ToString();
                    //ex:/DMS/ViewerXmlMaker.aspx
                    string serverMainPath = this.Request.ServerVariables["SCRIPT_NAME"].ToString();
                    //ex:DMS
                    string mainPath = serverMainPath.Substring(serverMainPath.IndexOf('/') + 1, serverMainPath.LastIndexOf('/') - 1);

                    string requestText = string.Empty;

                    if (!string.IsNullOrEmpty(wpinno))
                    {
                        requestText = string.Format("?CaseSet={0}&UserSet={1}&wpinno={2}", caseSet, this.UserInfo.UserID, wpinno);
                    }
                    else
                    {
                        requestText = string.Format("?CaseSet={0}&UserSet={1}", caseSet, this.UserInfo.UserID);
                    }

                    pama = string.Format("GET;{0}://{1}:{2}/{3}/ViewerXmlMaker.aspx{4};6", httpLink, serverName, serverPort, mainPath, requestText);

                    this.WriteLog(Mode.LogMode.INFO, string.Format("pama：{0}", pama));

                    //if (sIsEncrypt == "1")
                    {
                        pama = pama.EncryptBase64();
                        url = string.Format("ScanViewer.URL.WDA://{0}", pama);

                        this.WriteLog(Mode.LogMode.INFO, string.Format("url：{0}", url));
                        //url = url.EncryptDES();
                    }
                    //else
                    //{
                    //    url = "GET;" + url + ";" + "0";
                    //}

                    //#region 交易紀錄
                    //this.MonitorLog.LogMonitor(caseSet, "0", string.Empty, string.Empty, this.UserInfo.UserID, string.Empty, "0", userIP, Monitor.MSGID.Tm051, string.Empty);
                    //#endregion

                    string sScript = string.Format("winid = window.open('{0}');winid.close();", url);

                    System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(System.Web.UI.Page), "ActiveXScan", sScript, true);

                    this.MonitorLog.LogMonitor(string.Empty, this.UserInfo.UserName, this.UserInfo.RealName, userIP, Monitor.MSGID.WDA01, string.Empty);
                }
                catch (Exception ex)
                {
                    this.WriteLog(Mode.LogMode.ERROR, ex.Message);
                }
                finally
                {
                    this.DBConn.Dispose(); this.DBConn = null;
                }
            }
        }
    }
}