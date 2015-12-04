<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserEdit.aspx.cs" Inherits="WDA.UserEdit" %>

<%@ Register Assembly="WDA" Namespace="WDA" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <script type="text/javascript">
         function pageLoad() {
             showMessage();
         }
         </script>
    <div class="well well-lg">
        <div class="panel panel-primary">
            <div class="panel-heading">
                <h3 class="panel-title">人員管理─修改密碼</h3>
            </div>
            <div class="panel-body">
                <table class="ItemTD_green" style="width: 98%; float: right; border-collapse: separate; border-spacing: 1px;" border="1">
                    <tr>
                            <td class="HeadTD_green" style="padding: 5px;">
                          帳號：
                            </td>
                            <td style="padding: 5px; text-align: left;">
                                <asp:Label ID="LblUserName" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                         <tr>
                           
                            <td class="HeadTD_green" style="padding: 5px;">
                                姓名：
                            </td>
                            <td style="padding: 5px; text-align: left;">
                                <asp:Label ID="LblRealName" runat="server" Text="Label"></asp:Label>
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
                            <asp:Button ID="BtnOK" runat="server" Text="修 改" class="btn btn-large btn-success" OnClick="BtnOK_Click" />
                        </td>
                        <td>
                            <asp:Button ID="BtnClear" runat="server" Text="復 原" class="btn btn-large btn-success" OnClick="BtnClear_Click" />
                        </td>
                    </tr>
                </table>
        </div>
        <cc1:LiteralMessageBox ID="LiteralMessageBox1" runat="server"></cc1:LiteralMessageBox>
        <input id="HiddenMessage" type="Hidden" runat="server" />
    </div>
</asp:Content>
