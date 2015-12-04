using Log;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using WDA.Class;

namespace WDA
{
    public partial class SiteMaster : MasterPage
    {
        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;

        protected void Page_Init(object sender, EventArgs e)
        {
            // 下面的程式碼有助於防禦 XSRF 攻擊
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;
            if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                // 使用 Cookie 中的 Anti-XSRF 權杖
                _antiXsrfTokenValue = requestCookie.Value;
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            else
            {
                // 產生新的 Anti-XSRF 權杖並儲存到 cookie
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    HttpOnly = true,
                    Value = _antiXsrfTokenValue
                };
                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                {
                    responseCookie.Secure = true;
                }
                Response.Cookies.Set(responseCookie);
            }

            Page.PreLoad += master_Page_PreLoad;
        }

        protected void master_Page_PreLoad(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // 設定 Anti-XSRF 權杖
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
                ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
            }
            else
            {
                // 驗證 Anti-XSRF 權杖
                if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                    || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
                {
                    throw new InvalidOperationException("Anti-XSRF 權杖驗證失敗。");
                }
            }
        }

        #region Page_Load()
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.LoadRolePrivilegeList();
                this.LoadUserInfo();
            }
        } 
        #endregion

        #region LoadRolePrivilegeList()
        private void LoadRolePrivilegeList()
        {
            try
            {
                PageUtility pageUtility = new PageUtility();

                this.LiteralMenu.Text = "<nav class=\"navbar navbar-default\" role=\"navigation\">";
                this.LiteralMenu.Text += "<div>";
                this.LiteralMenu.Text += "<div class=\"accordion\" id=\"leftMenu\">";

                #region 紙本掃描
                if (pageUtility.UserInfo.Privilege.Tables[0].Select(string.Format("PrivID={0}", 1)).Length > 0)
                {
                    this.LiteralMenu.Text += "<div class=\"accordion-group\">";
                    this.LiteralMenu.Text += "<div class=\"accordion-heading\">";
                    this.LiteralMenu.Text += "<a class=\"accordion-toggle\" data-toggle=\"collapse\" data-parent=\"#leftMenu\" href=\"#collapseOne\">";
                    this.LiteralMenu.Text += "<span class=\"glyphicon glyphicon-file\" aria-hidden=\"true\">紙本掃描(105)</span></a>";
                    this.LiteralMenu.Text += "</div>";
                    this.LiteralMenu.Text += "<div id=\"collapseOne\" class=\"accordion-body collapse\" style=\"height:0px;\">";
                    this.LiteralMenu.Text += "<div class=\"accordion-inner\">";
                    this.LiteralMenu.Text += "<ul class=\"nav\">";

                    if (pageUtility.UserInfo.Privilege.Tables[0].Select(string.Format("PrivID={0}", 9)).Length > 0)
                    { this.LiteralMenu.Text += "<li><a runat=\"server\" href=\"ActiveXScan.aspx?CaseSet=-1\" target=\"_new\">開啟掃描</a></li>"; }

                    if (pageUtility.UserInfo.Privilege.Tables[0].Select(string.Format("PrivID={0}", 10)).Length > 0)
                    { this.LiteralMenu.Text += "<li><a runat=\"server\" href=\"ScanListQuery.aspx\">掃描清單</a></li>"; }

                    if (pageUtility.UserInfo.Privilege.Tables[0].Select(string.Format("PrivID={0}", 28)).Length > 0)
                    { this.LiteralMenu.Text += "<li><a runat=\"server\" href=\"CaseQueryMenu.aspx\">上傳影像查詢</a></li>"; }

                    this.LiteralMenu.Text += "</ul>";
                    this.LiteralMenu.Text += "</div>";
                    this.LiteralMenu.Text += "</div>";
                    this.LiteralMenu.Text += "</div>";
                }
                #endregion

                #region 歸檔登入(701)
                if (pageUtility.UserInfo.Privilege.Tables[0].Select(string.Format("PrivID={0}", 2)).Length > 0)
                {
                    this.LiteralMenu.Text += "<div class=\"accordion-group\">";
                    this.LiteralMenu.Text += "<div class=\"accordion-heading\">";
                    this.LiteralMenu.Text += "<a class=\"accordion-toggle\" data-toggle=\"collapse\" data-parent=\"#leftMenu\" href=\"#collapseTwo\">";
                    this.LiteralMenu.Text += "<span class=\"glyphicon glyphicon-folder-open\" aria-hidden=\"true\">歸檔登入(701)</span></a> ";
                    this.LiteralMenu.Text += "</div>";
                    this.LiteralMenu.Text += "<div id=\"collapseTwo\" class=\"accordion-body collapse\" style=\"height: 0px;\">";
                    this.LiteralMenu.Text += "<div class=\"accordion-inner\">";
                    this.LiteralMenu.Text += "<ol class=\"nav\">";

                    if (pageUtility.UserInfo.Privilege.Tables[0].Select(string.Format("PrivID={0}", 11)).Length > 0)
                    { this.LiteralMenu.Text += "<li><a runat=\"server\" href=\"FileArchive.aspx\">歸檔登入(管理)</a></li>"; }

                    if (pageUtility.UserInfo.Privilege.Tables[0].Select(string.Format("PrivID={0}", 12)).Length > 0)
                    { this.LiteralMenu.Text += "<li><a runat=\"server\" href=\"FileArchiveAdd.aspx\">歸檔登入(新增)</a></li>"; }

                    this.LiteralMenu.Text += "</ol>";
                    this.LiteralMenu.Text += "</div>";
                    this.LiteralMenu.Text += "</div>";
                    this.LiteralMenu.Text += "</div>";
                }
                #endregion

                #region 預約借檔(703)
                if (pageUtility.UserInfo.Privilege.Tables[0].Select(string.Format("PrivID={0}", 3)).Length > 0)
                {
                    this.LiteralMenu.Text += "<div class=\"accordion-group\">";
                    this.LiteralMenu.Text += "<div class=\"accordion-heading\">";
                    this.LiteralMenu.Text += "<a class=\"accordion-toggle\" data-toggle=\"collapse\" data-parent=\"#leftMenu\" href=\"#collapseThree\">";
                    this.LiteralMenu.Text += "<span class=\"glyphicon glyphicon-sort\" aria-hidden=\"true\">預約借檔(703)</span></a>";
                    this.LiteralMenu.Text += "</div>";
                    this.LiteralMenu.Text += "<div id=\"collapseThree\" class=\"accordion-body collapse\" style=\"height: 0px;\">";
                    this.LiteralMenu.Text += "<div class=\"accordion-inner\">";
                    this.LiteralMenu.Text += "<ol class=\"nav\">";

                    if (pageUtility.UserInfo.Privilege.Tables[0].Select(string.Format("PrivID={0}", 13)).Length > 0)
                    { this.LiteralMenu.Text += "<li><a runat=\"server\" href=\"ReservationBorrowAdd.aspx\">預約借檔(新增)</a></li>"; }

                    if (pageUtility.UserInfo.Privilege.Tables[0].Select(string.Format("PrivID={0}", 14)).Length > 0)
                    { this.LiteralMenu.Text += "<li><a runat=\"server\" href=\"ReservationBorrowCancel.aspx\">預約借檔(取消)</a></li>"; }

                    if (pageUtility.UserInfo.Privilege.Tables[0].Select(string.Format("PrivID={0}", 15)).Length > 0)
                    { this.LiteralMenu.Text += "<li><a runat=\"server\" href=\"ReservationBorrowApprove.aspx\">預約借檔(簽核)</a></li>"; }

                    if (pageUtility.UserInfo.Privilege.Tables[0].Select(string.Format("PrivID={0}", 16)).Length > 0)
                    { this.LiteralMenu.Text += "<li><a runat=\"server\" href=\"ReservationBorrowPrint.aspx\">預約借檔(列印)</a></li>"; }

                    this.LiteralMenu.Text += "</ol>";
                    this.LiteralMenu.Text += "</div>";
                    this.LiteralMenu.Text += "</div>";
                    this.LiteralMenu.Text += "</div>";
                }
                #endregion

                #region 調妥作業(707)
                if (pageUtility.UserInfo.Privilege.Tables[0].Select(string.Format("PrivID={0}", 6)).Length > 0)
                {
                    this.LiteralMenu.Text += "<div class=\"accordion-group\">";
                    this.LiteralMenu.Text += "<div class=\"accordion-heading\">";
                    this.LiteralMenu.Text += "<a class=\"accordion-toggle\" data-toggle=\"collapse\" data-parent=\"#leftMenu\" href=\"#collapseSix\">";
                    this.LiteralMenu.Text += "<span class=\"glyphicon glyphicon-picture\" aria-hidden=\"true\">調妥作業(707)</span></a>";
                    this.LiteralMenu.Text += "</div>";
                    this.LiteralMenu.Text += "<div id=\"collapseSix\" class=\"accordion-body collapse\" style=\"height:0px;\">";
                    this.LiteralMenu.Text += "<div class=\"accordion-inner\">";
                    this.LiteralMenu.Text += "<ol class=\"nav\">";

                    if (pageUtility.UserInfo.Privilege.Tables[0].Select(string.Format("PrivID={0}", 20)).Length > 0)
                    { this.LiteralMenu.Text += "<li><a runat=\"server\" href=\"DulyAdjustedAdd.aspx\">調妥新增</a></li>"; }

                    if (pageUtility.UserInfo.Privilege.Tables[0].Select(string.Format("PrivID={0}", 21)).Length > 0)
                    { this.LiteralMenu.Text += "<li><a runat=\"server\" href=\"DulyAdjustedQuery.aspx\">調妥查詢</a></li>"; }

                    this.LiteralMenu.Text += "</ol>";
                    this.LiteralMenu.Text += "</div>";
                    this.LiteralMenu.Text += "</div>";
                    this.LiteralMenu.Text += "</div>";
                }
                #endregion

                #region 借檔催還(704)
                if (pageUtility.UserInfo.Privilege.Tables[0].Select(string.Format("PrivID={0}", 4)).Length > 0)
                {
                    this.LiteralMenu.Text += "<div class=\"accordion-group\">";
                    this.LiteralMenu.Text += "<div class=\"accordion-heading\">";
                    this.LiteralMenu.Text += "<a class=\"accordion-toggle\" data-toggle=\"collapse\" data-parent=\"#leftMenu\" href=\"#collapseFour\">";
                    this.LiteralMenu.Text += "<span class=\"glyphicon glyphicon-flash\" aria-hidden=\"true\">借檔催還(704)</span></a>";
                    this.LiteralMenu.Text += "</div>";
                    this.LiteralMenu.Text += "<div id=\"collapseFour\" class=\"accordion-body collapse\" style=\"height:0px;\">";
                    this.LiteralMenu.Text += "<div class=\"accordion-inner\">";
                    this.LiteralMenu.Text += "<ol class=\"nav\">";

                    if (pageUtility.UserInfo.Privilege.Tables[0].Select(string.Format("PrivID={0}", 17)).Length > 0)
                    { this.LiteralMenu.Text += "<li><a runat=\"server\" href=\"RecallQuery.aspx\">借檔催還</a></li>"; }

                    this.LiteralMenu.Text += "</ol>";
                    this.LiteralMenu.Text += "</div>";
                    this.LiteralMenu.Text += "</div>";
                    this.LiteralMenu.Text += "</div>";
                }
                #endregion

                #region 還檔作業(705)
                if (pageUtility.UserInfo.Privilege.Tables[0].Select(string.Format("PrivID={0}", 5)).Length > 0)
                {
                    this.LiteralMenu.Text += "<div class=\"accordion-group\">";
                    this.LiteralMenu.Text += "<div class=\"accordion-heading\">";
                    this.LiteralMenu.Text += "<a class=\"accordion-toggle\" data-toggle=\"collapse\" data-parent=\"#leftMenu\" href=\"#collapseFive\">";
                    this.LiteralMenu.Text += "<span class=\"glyphicon glyphicon-floppy-save\" aria-hidden=\"true\">還檔作業(703&705)</span></a>";
                    this.LiteralMenu.Text += "</div>";
                    this.LiteralMenu.Text += "<div id=\"collapseFive\" class=\"accordion-body collapse\" style=\"height:0px;\">";
                    this.LiteralMenu.Text += "<div class=\"accordion-inner\">";
                    this.LiteralMenu.Text += "<ol class=\"nav\">";

                    if (pageUtility.UserInfo.Privilege.Tables[0].Select(string.Format("PrivID={0}", 25)).Length > 0)
                    { this.LiteralMenu.Text += "<li><a runat=\"server\" href=\"PaperAlsoFile.aspx\">紙本還檔</a></li>"; }

                    if (pageUtility.UserInfo.Privilege.Tables[0].Select(string.Format("PrivID={0}", 18)).Length > 0)
                    { this.LiteralMenu.Text += "<li><a runat=\"server\" href=\"AlsoFileExtension.aspx\">還檔展期</a></li>"; }

                    if (pageUtility.UserInfo.Privilege.Tables[0].Select(string.Format("PrivID={0}", 19)).Length > 0)
                    { this.LiteralMenu.Text += "<li><a runat=\"server\" href=\"VisaExtension.aspx\">簽准展期</a></li>"; }

                    this.LiteralMenu.Text += "</ol>";
                    this.LiteralMenu.Text += "</div>";
                    this.LiteralMenu.Text += "</div>";
                    this.LiteralMenu.Text += "</div>";
                }
                #endregion

                #region 帳號管理
                if (pageUtility.UserInfo.Privilege.Tables[0].Select(string.Format("PrivID={0}", 7)).Length > 0)
                {
                    this.LiteralMenu.Text += "<div class=\"accordion-group\">";
                    this.LiteralMenu.Text += "<div class=\"accordion-heading\">";
                    this.LiteralMenu.Text += "<a class=\"accordion-toggle\" data-toggle=\"collapse\" data-parent=\"#leftMenu\" href=\"#collapseSeven\">";
                    this.LiteralMenu.Text += "<span class=\"glyphicon glyphicon-user\" aria-hidden=\"true\">帳號管理</span></a>";
                    this.LiteralMenu.Text += "</div>";
                    this.LiteralMenu.Text += "<div id=\"collapseSeven\" class=\"accordion-body collapse\" style=\"height:0px;\">";
                    this.LiteralMenu.Text += "<div class=\"accordion-inner\">";
                    this.LiteralMenu.Text += "<ol class=\"nav\">";

                    if (pageUtility.UserInfo.Privilege.Tables[0].Select(string.Format("PrivID={0}", 22)).Length > 0)
                    { this.LiteralMenu.Text += "<li><a runat=\"server\" href=\"UserQueryMenu.aspx\">人員管理</a></li>"; }

                    if (pageUtility.UserInfo.Privilege.Tables[0].Select(string.Format("PrivID={0}", 23)).Length > 0)
                    { this.LiteralMenu.Text += "<li><a runat=\"server\" href=\"RoleQueryMenu.aspx\">角色管理</a></li>"; }

                    this.LiteralMenu.Text += "</ol>";
                    this.LiteralMenu.Text += "</div>";
                    this.LiteralMenu.Text += "</div>";
                    this.LiteralMenu.Text += "</div>";
                }
                #endregion

                #region 系統管理
                if (pageUtility.UserInfo.Privilege.Tables[0].Select(string.Format("PrivID={0}", 8)).Length > 0)
                {
                    this.LiteralMenu.Text += "<div class=\"accordion-group\">";
                    this.LiteralMenu.Text += "<div class=\"accordion-heading\">";
                    this.LiteralMenu.Text += "<a class=\"accordion-toggle\" data-toggle=\"collapse\" data-parent=\"#leftMenu\" href=\"#collapseEight\">";
                    this.LiteralMenu.Text += "<span class=\"glyphicon glyphicon-wrench\" aria-hidden=\"true\">系統管理</span></a>";
                    this.LiteralMenu.Text += "</div>";
                    this.LiteralMenu.Text += "<div id=\"collapseEight\" class=\"accordion-body collapse\" style=\"height:0px;\">";
                    this.LiteralMenu.Text += "<div class=\"accordion-inner\">";
                    this.LiteralMenu.Text += "<ol class=\"nav\">";

                    if (pageUtility.UserInfo.Privilege.Tables[0].Select(string.Format("PrivID={0}", 24)).Length > 0)
                    { this.LiteralMenu.Text += "<li><a runat=\"server\" href=\"SystemSet.aspx\">環境設定</a></li>"; }

                    this.LiteralMenu.Text += "</ol>";
                    this.LiteralMenu.Text += "</div>";
                    this.LiteralMenu.Text += "</div>";
                    this.LiteralMenu.Text += "</div>";
                }
                #endregion

                #region 登出系統
                this.LiteralMenu.Text += "<div class=\"accordion-group\">";
                this.LiteralMenu.Text += "<div class=\"accordion-heading\">";
                this.LiteralMenu.Text += "<a class=\"accordion-toggle\" data-toggle=\"collapse\" data-parent=\"#leftMenu\" href=\"#collapseNight\">";
                this.LiteralMenu.Text += "<span class=\"glyphicon glyphicon-off\" aria-hidden=\"true\">登出系統</span></a>";
                this.LiteralMenu.Text += "</div>";
                this.LiteralMenu.Text += "<div id=\"collapseNight\" class=\"accordion-body collapse\" style=\"height:0px;\">";
                this.LiteralMenu.Text += "<div class=\"accordion-inner\">";
                this.LiteralMenu.Text += "<ol class=\"nav\">";

                { this.LiteralMenu.Text += "<li><a runat=\"server\" href=\"Login.aspx?RePage=1\">回登入頁面</a></li>"; }

                this.LiteralMenu.Text += "</ol>";
                this.LiteralMenu.Text += "</div>";
                this.LiteralMenu.Text += "</div>";
                this.LiteralMenu.Text += "</div>";
                #endregion

                this.LiteralMenu.Text += "</div>";
                this.LiteralMenu.Text += "</div>";
                this.LiteralMenu.Text += "</nav>";
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        #region LoadUserInfo()
        private void LoadUserInfo()
        {
            PageUtility pageUtility = new PageUtility();

            try
            {
                this.LiteralUserInfo.Text = "<ul class=\"nav navbar-nav navbar-right\">";

                this.LiteralUserInfo.Text += "<li ><a runat=\"server\" href=\"UserEdit.aspx?UserID=" + pageUtility.UserInfo.UserID + "\">" + string.Format("使用者帳號：{0}", pageUtility.UserInfo.UserName) + "</a></li>";

                this.LiteralUserInfo.Text += "<li><a runat=\"server\">" + string.Format("版本號：{0}", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()) + "</a></li>";

                this.LiteralUserInfo.Text += "</ul>";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        } 
        #endregion
    }

}