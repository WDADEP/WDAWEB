<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserAdd.aspx.cs" Inherits="WDA.UserAdd" %>

<%@ Register Assembly="WDA" Namespace="WDA" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <script type="text/javascript">
         var IsErrorUserName = false;
         function pageLoad() {
             $('#UserName').hide();
             showMessage();
         }

         function ImageBtnOKClick() {
             try {
                 if (IsErrorUserName) { alert('帳號已經被使用'); return false; }
                 return true;
             }
             catch (e) {
                 alert(e.message);
                 Sys.Debug.traceDump(e);
             };
         }

         function CheckUserName() {
             var txtUserName = $get("MainContent_TxtUserName").value;

             WDA.AspNetAjaxInAction.CheckUserName(txtUserName, onCheckUserNameSuccess, onCheckUserNameSuccessFailure, "<%= DateTime.Now %>");
         }

         function onCheckUserNameSuccess(result, context, methodName) {
             if (result) {
                 $('#UserName').show('Drop');
                 IsErrorUserName = true;
             }
             else { $('#UserName').hide(); IsErrorUserName = false; }
         }
         function onCheckUserNameSuccessFailure(error, context, methodName) {
             var errorMessage = error.get_message();
             $get("UserName").innerHTML = errorMessage;
         }
         </script>
    <div class="alert alert-info">
        <button type="button" class="close" data-dismiss="alert">&times;</button>
        <h4>說明：</h4>
        <ol>
            <li>輸入「新增項目」，完成後請按【新增】鍵執行新增動作。</li>
             <li><kbd>*</kbd>號為必填項目</li>
            <li><a href="UserQueryMenu.aspx" class="alert-link">返回人員管理</a></li>
        </ol>
    </div>
    <div class="well well-lg">
        <div class="panel panel-primary">
            <div class="panel-heading">
                <h3 class="panel-title">人員管理─新增帳號</h3>
            </div>
            <div class="panel-body">
                <table class="ItemTD_green" style="width: 98%; float: right; border-collapse: separate; border-spacing: 1px;" border="1">
                    <tr>
                        <td class="HeadTD_green" style="padding: 5px;">
                            <span class="t15_red">＊</span>帳號：
                        </td>
                        <td style="padding: 5px; text-align: left;">
                            <asp:TextBox ID="TxtUserName" runat="server" onblur="javascript:CheckUserName()"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtUserName" CssClass="text-danger" ErrorMessage="必須填寫帳號。" SetFocusOnError="True" />
                            <div id="UserName"> <span class="t15_red">此帳號已經被使用</span></div>
                        </td>
                    </tr>
                    <tr>
                        <td class="HeadTD_green" style="padding: 5px;">
                            <span class="t15_red">＊</span> 姓名：
                        </td>
                        <td style="padding: 5px; text-align: left;">
                            <asp:TextBox ID="TxtRealName" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtRealName" CssClass="text-danger" ErrorMessage="必須填寫姓名。" SetFocusOnError="True" />
                            <div id="RealName"></div>
                    </tr>
                    <tr>
                        <td class="HeadTD_green" style="padding: 5px;">
                            <span class="t15_red">＊</span>角色：
                        </td>
                        <td style="padding: 5px; text-align: left;">
                            <asp:DropDownList ID="DDLRole" runat="server"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator_DDLRole" runat="server" ControlToValidate="DDLRole" ErrorMessage="請選擇角色" InitialValue="0" CssClass="text-danger" SetFocusOnError="True"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="HeadTD_green" style="padding: 5px;">
                            <span class="t15_red">＊</span>分機號碼：
                        </td>
                        <td style="padding: 5px; text-align: left;">
                            <asp:TextBox ID="TxtTel" runat="server"></asp:TextBox>
                              <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtTel" CssClass="text-danger" ErrorMessage="必須填寫分機號碼。" SetFocusOnError="True" />
                    </tr>
                    <tr>
                        <td class="HeadTD_green" style="padding: 5px;">
                            <span class="t15_red">＊</span>登入密碼：
                        </td>
                        <td style="padding: 5px; text-align: left;">
                            <asp:TextBox ID="TxtPassWord" runat="server" TextMode="Password"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtPassword" CssClass="text-danger" ErrorMessage="必須填寫登入密碼。" SetFocusOnError="True" />
                        </td>
                    </tr>
                    <tr>
                        <td class="HeadTD_green" style="padding: 5px;">
                            <span class="t15_red">＊</span>確認登入密碼：
                        </td>
                        <td style="padding: 5px; text-align: left;">
                            <asp:TextBox ID="TxtPasswordConfirm" runat="server" TextMode="Password"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtPasswordConfirm" CssClass="text-danger" Display="Dynamic" ErrorMessage="必須填寫確認密碼欄位。" SetFocusOnError="True" />
                            <asp:CompareValidator ID="CompareValidatorPW" runat="server" ErrorMessage="密碼確認錯誤" ControlToCompare="TxtPassword" ControlToValidate="TxtPasswordConfirm" ForeColor="Red"></asp:CompareValidator>
                        </td>
                    </tr>
                </table>
            </div>
            <table border="0" style="width: 100%; border-collapse: collapse">
                <tr style="text-align: center">
                    <td>
                        <asp:Button ID="BtnOK" runat="server" Text="新 增" class="btn btn-large btn-success" OnClick="BtnOK_Click" OnClientClick="JavaScript:if(!ImageBtnOKClick()) {return false} ;" />
                    </td>
                    <td>
                        <asp:Button ID="BtnClear" runat="server" Text="清 除" class="btn btn-large btn-success" OnClick="BtnClear_Click" />
                    </td>
                </tr>
            </table>
        </div>
        <cc1:LiteralMessageBox ID="LiteralMessageBox1" runat="server"></cc1:LiteralMessageBox>
        <input id="HiddenMessage" type="Hidden" runat="server" />
    </div>
</asp:Content>
