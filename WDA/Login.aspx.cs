using Log;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using WDA.Class;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.DirectoryServices;
using System.Configuration;

namespace WDA
{
    public partial class Login : PageUtility
    {
        #region Page_Load()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // Modified by Luke 2016/08/12
            //int rePage = this.Request.QueryString["RePage"] != null ? Convert.ToInt16(this.Request.QueryString["RePage"].Trim()) : 0;
            int rePage = this.Request.Form["RePage"] != null ? Convert.ToInt16(this.Request.Form["RePage"].Trim()) : 0;

            this.LoadPage(false);

            this.LoginShowMessage("請使用電腦開機帳號密碼登入系統");

            try
            {
               
                if (rePage == 1)
                {
                    FormsAuthentication.SignOut();
                    this.Session.RemoveAll();

                    PageUtility.dv_Sys = null;

                    this.ClientScript.RegisterStartupScript(this.GetType(), "", "<script language=\"javascript\">top.window.location.replace('Login.aspx')</script>");
                }
                else
                {

                    if (!IsPostBack)
                    {
                        if (Request.QueryString.Count == 0)
                        {
                            this.Panel_Main.Visible = true;
                        }
                        else
                        {
                            this.Panel_Main.Visible = false;
                            this.WDAViewer();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.LoginShowMessage(ex.Message);
            }
            finally
            {
                this.DBConn.Dispose(); this.DBConn = null;
            }
        }
        #endregion

        #region BtnLoginClick()
        private void BtnLoginClick()
        {
            string userID = string.Empty;

            string userName = this.txtUserName.Text.Trim();
            string possword = this.txtPd.Text.Trim();

            string strSql = string.Empty;
            string where = string.Empty;
            try
            {
                //string userLogin = this.GetSystem("UserLogin");

                //userLogin = userName == "Admin" ? "0" : userLogin;

                //this.WriteLog(Mode.LogMode.INFO, string.Format("userLogin：{0}", userLogin));

                //if (userLogin == "0")
                {
                    // Modified by Luke 2016/11/23
                    /*#region 資料庫驗證
                    try
                    {
                        possword = possword.EncryptDES();

                        where = " And UserName = :username And ut.Password = :password";

                        strSql = this.Select.UserTable(where);

                        OleDbCommand command = (OleDbCommand)this.DBConn.GeneralSqlCmd.Command;

                        command.Parameters.Clear();
                        command.Parameters.Add(new OleDbParameter("username", OleDbType.VarChar)).Value = userName;
                        command.Parameters.Add(new OleDbParameter("password", OleDbType.VarChar)).Value = possword;

                        DataTable dt = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            string status = dt.Rows[0]["UserStatus"] != null ? dt.Rows[0]["UserStatus"].ToString().Trim() : "0";

                            userID = dt.Rows[0]["UserID"] != null ? dt.Rows[0]["UserID"].ToString().Trim() : "0";

                            if (status == "99") this.LoginShowMessage("帳號已被停用");
                            else
                            {
                                Session[SessionName.UserID] = userID;

                                this.LocationReplace();
                            }
                        }
                        else
                        {
                            this.LoginShowMessage("請輸入正確帳號密碼");
                        }
                    }
                    catch (Exception ex)
                    {
                        this.LoginShowMessage(ex.Message);
                    }
                    finally
                    {
                        this.DBConn.Dispose(); this.DBConn = null;
                    }

                    #endregion
                     * */

                    bool bEnableAD = (Properties.Settings.Default.EnableAD == "1");
                    bool bAuthenticated = false;
                    if (bEnableAD)
                    {
                        #region AD(LDAP)驗證

                        string host = Properties.Settings.Default.ADHost;
                        if (string.IsNullOrWhiteSpace(host))
                            this.LoginShowMessage("找不到AD主機!");

                        ushort port = Properties.Settings.Default.ADPort;
                        if (port == 0)
                            port = 389;

                        //string domain = Properties.Settings.Default.ADDefaultDomain;

                        //string baseDn = DomainToLDAPString(domain);
                        //if (!string.IsNullOrWhiteSpace(baseDn))
                        //{
                        //    if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.ADBaseDN))
                        //        baseDn = Properties.Settings.Default.ADBaseDN + "," + baseDn;
                        //}
                        //else
                        //{
                        //    if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.ADBaseDN))
                        //        baseDn = Properties.Settings.Default.ADBaseDN;
                        //}

                        string baseDn = Properties.Settings.Default.ADBaseDN;

                        string path = string.Empty;
                        if (!string.IsNullOrEmpty(baseDn))
                            path = string.Format("LDAP://{0}:{1}/{2}", host, port, baseDn);
                        else
                            path = string.Format("LDAP://{0}:{1}", host, port);

                        LdapAuthentication auth = new LdapAuthentication(path);

                        // userName若為"domain\account"格式,發現即使domain錯誤但只要account及密碼對,也能驗證成功,
                        // 因此要改成"account@domain"格式
                        if (userName.Contains(@"\"))
                        {
                            int index = userName.IndexOf(@"\");
                            string _account = userName.Substring(index + 1);
                            string _domain = userName.Substring(0, index);
                            userName = string.Format("{0}@{1}", _account, _domain);
                        }
                        //// userName若為單純的"account"格式,且ADDefaultDomain設定中有指定預設的domain,
                        //// 則userName要改成"account@ADDefaultDomain"格式
                        //else if (!userName.Contains("@") && !string.IsNullOrEmpty(domain))
                        //{
                        //    userName = string.Format("{0}@{1}", userName, domain);
                        //}

                        bAuthenticated = auth.IsAuthenticated(userName, possword);

                        if(!bAuthenticated)
                            this.LoginShowMessage("AD驗證失敗!");

                        #endregion
                    }

                    #region 資料庫驗證; 而AD(LDAP)驗證成功, 不需驗證密碼但也要自資料庫讀取帳號狀態
                    if (bEnableAD && bAuthenticated)
                    {
                        //where = " And UserName = :username";
                        where = " And UPPER(UserName) = UPPER(:username)";

                    }
                    else
                    {
                        possword = possword.EncryptDES();
                        //where = " And UserName = :username And ut.Password = :password";
                        where = " And UPPER(UserName) = UPPER(:username) And ut.Password = :password";

                    }

                    if (userName.Contains(@"\"))
                    {
                        userName = userName.Split(@"\".ToCharArray())[1];
                    }
                    else if (userName.Contains("@"))
                    {
                        userName = userName.Split("@".ToCharArray())[0];
                    }

                    strSql = this.Select.UserTable(where);

                    OleDbCommand command = (OleDbCommand)this.DBConn.GeneralSqlCmd.Command;

                    command.Parameters.Clear();
                    command.Parameters.Add(new OleDbParameter("username", OleDbType.VarChar)).Value = userName;
                    if (!bEnableAD || !bAuthenticated)
                        command.Parameters.Add(new OleDbParameter("password", OleDbType.VarChar)).Value = possword;

                    DataTable dt = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        string status = dt.Rows[0]["UserStatus"] != null ? dt.Rows[0]["UserStatus"].ToString().Trim() : "0";

                        userID = dt.Rows[0]["UserID"] != null ? dt.Rows[0]["UserID"].ToString().Trim() : "0";

                        if (status == "99") this.LoginShowMessage("帳號已被停用");
                        else
                        {
                            Session[SessionName.UserID] = userID;

                            this.LocationReplace();
                        }
                    }
                    else
                    {
                        this.LoginShowMessage("請輸入正確帳號密碼");
                    }
                    #endregion

                }
            }
            catch (Exception ex)
            {
                this.WriteLog(Mode.LogMode.ERROR, ex.Message);

                this.LoginShowMessage(ex.Message);
            }
            finally
            {
                this.DBConn.Dispose(); this.DBConn = null;
            }
        }
        #endregion

        #region ImgBtnLogin_Click()
        protected void ImgBtnLogin_Click(object sender, ImageClickEventArgs e)
        {
            this.BtnLoginClick();
        }
        #endregion

        #region WDAViewer()
        /// <summary>
        /// 外部系統調閱Viewer
        /// </summary>
        private void WDAViewer()
        {
            string url = string.Empty;
            string caseSet = string.Empty;
            string userSet = string.Empty;

            try
            {
                string wpinno = this.RequestQueryString("Wpinno");

                string priID = this.RequestQueryString("PriID");

                string userName = this.RequestQueryString("UserName");

                string realName = Server.UrlDecode(this.RequestQueryString("RealName"));

                if (wpinno.Length == 0)
                {
                    this.DrawMessage(this,"未取得資料，請檢查參數(Wpinno)", 800, 300); return;
                }

                if (priID.Length == 0)
                {
                    this.DrawMessage(this, "未取得資料，請檢查參數(PriID)", 800, 300); return;
                }

                if (userName.Length == 0)
                {
                    this.DrawMessage(this, "未取得資料，請檢查參數(UserName)", 800, 300); return;
                }

                //if (realName.Length == 0)
                //{
                //    this.DrawMessage(this, "未取得資料，請檢查參數(RealName)", 800, 300); return;
                //}

                userSet = this.GetUserID(userName);

                if (userSet.Length == 0)
                {
                    this.DrawMessage(this, "UserName有問題，請檢查參數(UserName)", 800, 300); return;
                }

                string[] wpinnoArray = wpinno.Trim().Split(';');

                for (int a = 0; a < wpinnoArray.Length; a++)
                {
                    string caseID = this.GetCaseID(wpinnoArray[a], userName);

                    if (!string.IsNullOrEmpty(caseSet))
                    {
                        caseSet = string.Format("{0};{1}(*)", caseSet, caseID);
                    }
                    else { caseSet = string.Format("{0}(*)", caseID); }
 
                }

                Session[SessionName.UserID] = userSet;

                url = string.Format("ActiveXViewer.aspx?Mode=E&CaseSet={0}&UserSet={1}&PriID={2}", caseSet, userSet, priID);

                //string sScript = string.Format("window.open('{0}'); window.opener = null;window.open('', '_parent');window.close();", url);
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "WDAViewer", sScript, true);

                this.PageReplace(url);
            }
            catch (Exception ex)
            {
                this.WriteLog(global::Log.Mode.LogMode.ERROR, ex.Message);
            }
            finally
            {
                this.DBConn.Dispose(); this.DBConn = null;
            }
        }
        #endregion

        #region DrawMessage()
        public  void DrawMessage( Page MyPage, string Message, int Width, int Height)
        {
            using (Bitmap bmp = new Bitmap(Width, Height))
            {
                Graphics g = Graphics.FromImage(bmp);

                Point point = new Point(0, 0);

                using (LinearGradientBrush brush = new LinearGradientBrush(
                    new Rectangle(point, new Size(bmp.Width, bmp.Height)),
                    Color.FromArgb(255, 255, 200),
                    Color.White,
                    80f,
                    true))
                {
                    Point p1 = new Point(0, point.Y);
                    Point p2 = new Point(0, point.Y + Height);
                    Point p3 = new Point(Width, point.Y + Height);
                    Point p4 = new Point(Width, point.Y);

                    Point[] curvePoint = new Point[] { p1, p2, p3, p4 };

                    g.FillPolygon(brush, curvePoint);
                }
                using (Font noImageFont = new Font("微軟正黑體", 22f, FontStyle.Regular))
                {
                    using (SolidBrush noImageBrush = new SolidBrush(Color.Red))
                    {
                        g.DrawString(Message, noImageFont, noImageBrush, 10, 10);
                    }
                }
                using (MemoryStream ms = new MemoryStream())
                {
                    bmp.Save(ms, ImageFormat.Png);
                    MyPage.Response.ClearContent();
                    MyPage.Response.ContentType = "image/Png";
                    MyPage.Response.BinaryWrite(ms.ToArray());
                    ms.Close();
                }
                bmp.Dispose();
            }
            GC.Collect(); GC.WaitForPendingFinalizers();
        }
        #endregion

        #region GetCaseID()
        private string GetCaseID(string Wpinno , string UserName)
        {
            string strSql = string.Empty;

            try
            {
                strSql = this.Select.GetCaseID();

                this.WriteLog(Mode.LogMode.DEBUG, strSql);

                OleDbCommand command = (OleDbCommand)this.DBConn.GeneralSqlCmd.Command;

                command.Parameters.Clear();
                command.Parameters.Add(new OleDbParameter("Wpinno", OleDbType.VarChar)).Value = Wpinno;
                command.Parameters.Add(new OleDbParameter("UserName", OleDbType.VarChar)).Value = UserName;

                DataTable dt = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                if (dt != null && dt.Rows.Count > 0)
                {
                    return dt.Rows[0]["CASEID"].ToString();
                }
                else { return string.Empty; }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                this.DBConn.Dispose(); this.DBConn = null;
            }
        }
        #endregion

        #region GetUserID()
        private string GetUserID(string UserName)
        {
            string strSql = string.Empty;

            try
            {
                strSql = this.Select.GetUserID();

                this.WriteLog(Mode.LogMode.DEBUG, strSql);

                OleDbCommand command = (OleDbCommand)this.DBConn.GeneralSqlCmd.Command;

                command.Parameters.Clear();
                command.Parameters.Add(new OleDbParameter("UserName", OleDbType.VarChar)).Value = UserName;

                DataTable dt = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                if (dt != null && dt.Rows.Count > 0)
                {
                    return dt.Rows[0]["UserID"].ToString();
                }
                else { return string.Empty; }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                this.DBConn.Dispose(); this.DBConn = null;
            }
        }
        #endregion

        // Added by Luke 2016/11/23
        #region DomainToLDAPString()
        private string DomainToLDAPString(string domain)
        {
            string ret = "";

            if (!string.IsNullOrWhiteSpace(domain))
            {
                string[] dcs = domain.Split('.');

                List<string> dcList = new List<string>(dcs.Length);
                foreach (string dc in dcs)
                {
                    if (!string.IsNullOrWhiteSpace(dc))
                        dcList.Add("DC=" + dc);
                }

                ret = string.Join(",", dcList.ToArray());
            }

            return ret;
        }
        #endregion

        // Added by Luke 2016/11/23
        #region class LdapAuthentication
        public class LdapAuthentication
        {
            private string _path;
            private string _filterAttribute;

            private List<string> _groups = new List<string>();

            public List<string> Groups
            {
                get
                {
                    return _groups;
                }
            }

            public LdapAuthentication(string path)
            {
                _path = path;
            }

            public bool IsAuthenticated(string username, string pwd)
            {
                //log.Debug(string.Format("====== {0} ======", MethodBase.GetCurrentMethod().Name));

                _groups.Clear();

                DirectoryEntry entry = new DirectoryEntry(_path, username, pwd, AuthenticationTypes.Secure | AuthenticationTypes.ServerBind);

                try
                {
                    //Bind to the native AdsObject to force authentication.
                    object obj = entry.NativeObject;

                    DirectorySearcher search = new DirectorySearcher(entry);

                    if (username.Contains(@"\"))
                        username = username.Split(@"\".ToCharArray())[1];
                    else if (username.Contains("@"))
                        username = username.Split("@".ToCharArray())[0];

                    search.Filter = "(sAMAccountName=" + username + ")";
                    search.PropertiesToLoad.Add("cn");
                    search.PropertiesToLoad.Add("memberOf");
                    SearchResult result = search.FindOne();

                    if (null == result)
                    {
                        return false;
                    }

                    //Update the new path to the user in the directory.
                    _path = result.Path;
                    //log.Debug("_path=" + _path);
                    _filterAttribute = (string)result.Properties["cn"][0];
                    //log.Debug("_filterAttribute=" + _filterAttribute);

                    //Get groups which user belongs to
                    int propertyCount = result.Properties["memberOf"].Count;
                    string dn;
                    int equalsIndex, commaIndex;
                    for (int propertyCounter = 0; propertyCounter < propertyCount; propertyCounter++)
                    {
                        dn = (string)result.Properties["memberOf"][propertyCounter];
                        equalsIndex = dn.IndexOf("=", 1);
                        commaIndex = dn.IndexOf(",", 1);
                        if (-1 == equalsIndex)
                        {
                            continue;
                        }
                        _groups.Add(dn.Substring((equalsIndex + 1), (commaIndex - equalsIndex) - 1));
                    }
                    //log.Debug("_groups=" + string.Join("|", _groups.ToArray()));

                }
                catch (Exception ex)
                {
                    throw new Exception("使用者帳號AD驗證錯誤: " + ex.Message);
                }

                return true;
            }
        }
        #endregion
    }
}