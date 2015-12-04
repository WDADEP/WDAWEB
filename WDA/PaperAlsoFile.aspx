<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PaperAlsoFile.aspx.cs" Inherits="WDA.PaperAlsoFile" %>

<%@ Register Assembly="WDA" Namespace="WDA" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
       <script type="text/javascript">
           var IsCheckNo = false;
           var IsCheckWpstatus = false;
           function pageLoad() {
               $('#ShowInfo').hide();
               showMessage();
           }

           function ImageBtnOKClick() {
               try {
                   if (!IsCheckNo) { alert('請先確認收文文號或發文文號!!'); return false; }
                   if (!IsCheckWpstatus) { alert('已歸檔、請勿重覆歸檔!'); return false; }
                   return true;
               }
               catch (e) {
                   alert(e.message);
                   Sys.Debug.traceDump(e);
               };
           }

           function GetPaperAlsoFile(Type) {
               var No = "";
               if (Type == 0) { No = $get("MainContent_txtWpinno").value; $get("MainContent_txtWpoutNo").innerText =""; }
               else if (Type == 1) { No = $get("MainContent_txtWpoutNo").value; $get("MainContent_txtWpinno").innerText = ""; }

               if (No.length != 0) {
                   WDA.AspNetAjaxInAction.GetPaperAlsoFile(No, Type, onGetPaperAlsoFileSuccess, onGetPaperAlsoFileFailure, "context", 1000);
               }
               else { IsCheckNo = false; IsCheckWpstatus = false;}
           }

           function onGetPaperAlsoFileSuccess(result, context, methodName) {
               for (var i = 0; i < result.length; i++) {
                   var bev = result[i];

                   if (bev.Wpinno.length > 0) {
                       IsCheckNo = true;
                       $('#ShowInfo').show();
                   }
                   else { alert("文號錯誤請重新輸入"); $('#ShowInfo').hide(); IsCheckNo = false; return; }

                   $get("MainContent_lblWpinno").innerText = bev.Wpinno;
                   $get("MainContent_lblWpoutNo").innerText = bev.Wpoutno;
                   $get("MainContent_lblReceiver").innerText = bev.Receiver;
                   $get("MainContent_lblTel").innerText = bev.Tel;
                   $get("MainContent_lblWpstatus").innerText = bev.Wpstatus;

                   if (bev.Wpstatus == '未還檔') { IsCheckWpstatus = true; }

                   $get("MainContent_lblBorrdate").innerText = bev.Borrdate;
                   $get("MainContent_lblRedate").innerText = bev.Redate;
               }
           }
           function onGetPaperAlsoFileFailure(error, context, methodName) {
               var errorMessage = error.get_message();
           }
         </script>
    <div class="alert alert-info">
        <button type="button" class="close" data-dismiss="alert">&times;</button>
        <h4>說明：</h4>
        <ol>
            <li>輸入「收文文號」或「發文文號」，完成後請按下「確認收文號」或 「確認發文號」</li>
            <li>系統將自動帶出訊息</li>
            <li>按下「還檔」按鈕完成還檔作業</li>
        </ol>
    </div>
    <div class="well well-lg">
        <div class="panel panel-primary">
            <div class="panel-heading">
                <h3 class="panel-title">還檔作業─借閱還檔</h3>
            </div>
            <div class="alert alert-success">
                <div class="row">
                    <div class="col-md-6">
                        收文文號：
                        <asp:TextBox ID="txtWpinno" runat="server"></asp:TextBox>
                        <input id="btnCheckWpinno" type="button" value="確認收文號" class="btn btn-large btn-danger" onclick="javascript: GetPaperAlsoFile(0)" />
                    </div>
                    <div class="col-md-6">
                        發文文號：
                        <asp:TextBox ID="txtWpoutNo" runat="server"></asp:TextBox>
                        <input id="btnCheckWpoutNo" type="button" value="確認發文號" class="btn btn-large btn-danger" onclick="javascript: GetPaperAlsoFile(1)" />
                    </div>
                </div>
            </div>
            <div class="panel-body">
                <div id="ShowInfo">
                    <div class="alert alert-success" role="alert">紙本還檔詳細資料如下：</div>
                    <table class="ItemTD_green" style="width: 98%; float: right; border-collapse: separate; border-spacing: 1px;" border="1">
                          <tr>
                            <td class="HeadTD_green" style="padding: 5px;">收文文號：</td>
                            <td style="padding: 5px; text-align: left;">
                                <asp:Label ID="lblWpinno" runat="server" Text="lblWpinno"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="HeadTD_green" style="padding: 5px;">發文文號：</td>
                            <td style="padding: 5px; text-align: left;">
                                <asp:Label ID="lblWpoutNo" runat="server" Text="lblWpoutNo"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="HeadTD_green" style="padding: 5px;">調卷者：</td>
                            <td style="padding: 5px; text-align: left;">
                                <asp:Label ID="lblReceiver" runat="server" Text="lblReceiver"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="HeadTD_green" style="padding: 5px;">分    機：</td>
                            <td style="padding: 5px; text-align: left;">
                                <asp:Label ID="lblTel" runat="server" Text="lblTel"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="HeadTD_green" style="padding: 5px;">檔案狀況：</td>
                            <td style="padding: 5px; text-align: left;">
                                <asp:Label ID="lblWpstatus" runat="server" Text="lblWpstatus"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="HeadTD_green" style="padding: 5px;">借檔日期：</td>
                            <td style="padding: 5px; text-align: left;">
                                <asp:Label ID="lblBorrdate" runat="server" Text="lblBorrdate"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="HeadTD_green" style="padding: 5px;">(原訂)還檔日期：</td>
                            <td style="padding: 5px; text-align: left;">
                                <asp:Label ID="lblRedate" runat="server" Text="lblRedate"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <table border="0" style="width: 100%; border-collapse: collapse">
                <tr style="text-align: center">
                    <td style="text-align: right">
                        <asp:Button ID="BtnOK" runat="server" Text="還 檔" class="btn btn-large btn-success" OnClick="BtnOK_Click" OnClientClick="JavaScript:if(!ImageBtnOKClick()) {return false} ;" />
                    </td>
                    <td style="text-align: center"></td>
                    <td style="text-align: left">
                        <asp:Button ID="BtnClear" runat="server" Text="取 消" class="btn btn-large btn-success" OnClick="BtnClear_Click"/>
                    </td>
                </tr>
            </table>
            <br />
        </div>
        <div id="btn1">
        </div>
        <cc1:LiteralMessageBox ID="LiteralMessageBox1" runat="server"></cc1:LiteralMessageBox>
        <input id="HiddenMessage" type="Hidden" runat="server" />
    </div>
</asp:Content>
