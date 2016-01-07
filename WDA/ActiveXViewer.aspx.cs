using Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WDA.Class;

namespace WDA
{
    public partial class ActiveXViewer : PageUtility
    {
        #region Page_Load()
        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadPage(true);

            if (!IsPostBack)
            {
                try
                {
                    string url = string.Empty; string script = string.Empty; string isEncrypt = string.Empty; string requestText = string.Empty;

                    //取用戶真實IP1，預留看是否要用IP控管掃描工作站
                    string userIP = this.Request.ServerVariables["REMOTE_ADDR"].ToString();

                    string httpType = this.Request.ServerVariables["HTTPS"].ToString();

                    string httpLink = httpType.ToLower() == "on" ? " https" : "http";

                    string serverName = this.Request.ServerVariables["SERVER_NAME"].ToString();
                    //ex:80
                    string serverPort = this.Request.ServerVariables["SERVER_PORT"].ToString();
                    //ex:/DMG/ViewerXmlMaker.aspx
                    string serverMainPath = this.Request.ServerVariables["SCRIPT_NAME"].ToString();
                    //ex:DMG
                    string mainPath = serverMainPath.Substring(serverMainPath.IndexOf('/') + 1, serverMainPath.LastIndexOf('/') - 1);

                    string mode = this.RequestQueryString("Mode");

                    if (string.IsNullOrEmpty(mode))
                    {
                        string caseSet = this.RequestQueryString("CaseSet");

                        string userSet = this.UserInfo.UserID;

                        requestText = string.Format("?CaseSet={0}&UserSet={1}", caseSet, userSet);
                    }
                    else if (mode == "D")
                    {
                        string caseSet = this.RequestQueryString("CaseSet");

                        string priID = this.RequestQueryString("PriID");

                        string userSet = this.UserInfo.UserID;

                        requestText = string.Format("?Mode={0}&CaseSet={1}&UserSet={2}&PriID={3}", mode, caseSet, userSet,priID);
 
                    }
                    else 
                    {
                        string caseSet = this.RequestQueryString("CaseSet");

                        string priID = this.RequestQueryString("PriID");

                        string userSet = this.RequestQueryString("UserSet");

                        requestText = string.Format("?Mode={0}&CaseSet={1}&UserSet={2}&PriID={3}", mode, caseSet, userSet, priID);
                    }

                    url = string.Format("{0}://{1}:{2}/{3}/ViewerXmlMaker.aspx{4}", httpLink, serverName, serverPort, mainPath, requestText);

                    Session["URL"] = Session["InstallURL"] = null;
                    Session["URL"] = url;
                    Session["InstallURL"] = "ViewerClientInstall.aspx";
                }
                catch (Exception ex)
                {
                    this.WriteLog(Mode.LogMode.ERROR, ex.Message);
                }
            }
        }
        #endregion
    }
}