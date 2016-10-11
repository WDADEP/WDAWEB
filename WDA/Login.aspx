<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WDA.Login" %>

<%@ Register Assembly="WDA" Namespace="WDA" TagPrefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>【勞發署】影像暨檔管系統</title>
    <script src="Scripts/jquery-1.10.2.min.js"></script>
    <script src="Scripts/JQueryUI/jquery-ui-1.10.4.custom.js"></script>
    <link href="Content/jquery-ui-1.10.4.custom.css" rel="stylesheet" />
     <link href="Content/StyleLogin.css" rel="stylesheet" />
    <script src="Scripts/Utility/initPageLoad.js"></script>
    <script type="text/javascript">
        function pageLoad() {
            LoginShowMessage();    
        }
        function Login_Check() {
            if ($('#txtUserName').val().length == 0) {
                alert('請輸入帳號'); return false;
            }
            if ($('#txtPd').val().length == 0) {
                alert('請輸入密碼'); return false;
            }

            return true;
        }
    </script>
</head>
<body class="LoginStyle" style="background-image: url('../Images/Login_Style/bg_login_back.jpg');
    background-repeat: repeat-x; background-position: left top; background-color: #707070"
    onload="this.moveTo(0, 0);this.resizeTo(screen.availWidth, screen.availHeight);">
        <form id="form1" runat="server">
                    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
              <asp:Panel ID="Panel_Main" Visible="true" runat="server">
            <table style="margin-top: 40px; text-align: center">
                <tr>
                    <td style="vertical-align: top; width: 50px"></td>
                    <td>
                        <table class="LoginBackground" border="0" style="border-collapse: collapse; border-spacing: 0;">
                            <tr>
                                <td style="text-align: left">
                                    <asp:Panel ID="Panel1" runat="server">
                                        <table class="LoginBoxPosition" border="0" style="border-collapse: collapse; border-spacing: 0;">
                                            <tr>
                                                <td class="LoinBoxLeftTop" style="padding:0"></td>
                                                <td class="LoinBoxTop" style="padding:0"></td>
                                                <td class="LoinBoxRightTop" style="width: 15px;padding:0 "></td>
                                            </tr>
                                            <tr>
                                                <td class="LoinBoxLeft" style="padding:0"></td>
                                                <td class="LoinBoxCenter" style="padding:0">
                                                    <table style="text-align: left; font-size: 12pt; border-collapse: separate; border-spacing: 5px;" border="0">
                                                        <tr>
                                                            <td style="padding-top: 15px; white-space: nowrap" colspan="3">
                                                                <span style="white-space: nowrap">請輸入使用者帳號及密碼：</span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span style="white-space: nowrap">使用者帳號：</span>
                                                            </td>
                                                            <td style="padding-top: 10px">
                                                                <asp:TextBox ID="txtUserName" Style="ime-mode: disabled" runat="server" ClientIDMode="Static" AutoCompleteType="Disabled"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span id="Txt_Password" style="white-space: nowrap">使用者密碼：</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtPd" runat="server" TextMode="Password" ClientIDMode="Static"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td style="text-align: right">
                                                                <asp:ImageButton ID="ImgBtnLogin" runat="server" ImageUrl="~/Images/Login_Style/login_button.gif" OnClientClick="JavaScript:if(!Login_Check()) {return false} ;" Height="38px" Width="100px" onmouseover="this.src='Images/Login_Style/login_button2.gif'"  onmouseout="this.src='Images/Login_Style/login_button.gif'" OnClick="ImgBtnLogin_Click" />
                                                                <%--<input type="button" id="Btn_Login" class="LoginButton1" onmouseover="javascript:this.className='LoginButton2'"
                                                                    onmouseout="javascript:this.className='LoginButton1'" onclick="javascript: Login_Check();" runat="server" />--%>
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td class="LoinBoxRight" style="width: 15px ;padding:0"></td>
                                            </tr>
                                            <tr>
                                                <td class="LoinBoxLeftBottom" style="padding:0"></td>
                                                <td class="LoinBoxBottom" style="padding:0"></td>
                                                <td class="LoinBoxRightBottom" style="width: 15px;padding:0"></td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <cc1:LiteralMessageBox ID="LiteralMessageBox1" runat="server"></cc1:LiteralMessageBox>
    <input id="HiddenMessage" type="Hidden" runat="server" />
                   </asp:Panel>
        </form>
</body>
</html>
