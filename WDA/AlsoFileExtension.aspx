<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AlsoFileExtension.aspx.cs" Inherits="WDA.AlsoFileExtension" %>

<%@ Register Assembly="WDA" Namespace="WDA" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        var IsCheckNo = false;
        function pageLoad() {
            $('#ShowInfo').hide();
            showMessage();
        }
        function ImageBtnOKClick() {
            try {
                if (!IsCheckNo) { alert('請先確認收文文號或發文文號!!'); return false; }

                var caseStatus = $get('MainContent_ddlApproveuserID').value;
                if (caseStatus == 0) {
                    alert('請選擇審核人員');
                    return false;
                }
                return true;
            }
            catch (e) {
                alert(e.message);
                Sys.Debug.traceDump(e);
            };
        }

        function GetAlsoFile() {
            var No = $get("MainContent_txtWpinno").value;
            var ViewType = $get("MainContent_ddlViewType").value;
            var UserName = "<%= UserInfo.UserName %>";
          
            if (No.length != 0 && ViewType.length !=0) {
                WDA.AspNetAjaxInAction.GetAlsoFile(No, ViewType, UserName, onGetAlsoFileSuccess, onGetAlsoFileFailure, "context", 1000);
            }
            else { IsCheckNo = false; }
        }

        function onGetAlsoFileSuccess(result, context, methodName) {
            for (var i = 0; i < result.length; i++) {
                var bev = result[i];

                if (bev.ReturnMessage.length > 0) { alert(bev.ReturnMessage); $('#ShowInfo').hide(); IsCheckNo = false; return; }
                else if (bev.Wpinno.length > 0) {
                    IsCheckNo = true;
                    $('#ShowInfo').show();
                }
                else { alert("文號錯誤請重新輸入"); $('#ShowInfo').hide(); IsCheckNo = false; return; }

                $get("MainContent_lblWpinno").innerText = bev.Wpinno;
                $get("MainContent_lblWpoutNo").innerText = bev.Wpoutno;
                $get("MainContent_lblTranst").innerText = bev.Transt;
                $get("MainContent_lblReceiver").innerText = bev.Receiver;
                $get("MainContent_lblRname").innerText = bev.Rname;
                $get("MainContent_lblApplkind").innerText = bev.Kind;
                $get("MainContent_lblredate").innerText = bev.Redate;
                $get("MainContent_lblexten").innerText = bev.Exten;
                $get("MainContent_lblredate1").innerText = bev.ExtensionRedate;
                $("#BtnOK").show();

            }
        }
        function onGetAlsoFileFailure(error, context, methodName) {
            var errorMessage = error.get_message();
        }
    </script>
    <div class="alert alert-info">
        <button type="button" class="close" data-dismiss="alert">&times;</button>
        <h4>說明：</h4>
        <ol>
            <li>輸入「收文文號」及選擇「調閱類型」，完成後請按下「確認收文號」</li>
            <li>系統將自動帶出訊息</li>
            <li>按下「展期」按鈕完成展期作業</li>
        </ol>
    </div>
    <div class="well well-lg">
        <div class="panel panel-primary">
            <div class="panel-heading">
                <h3 class="panel-title">還檔作業─還檔展期</h3>
            </div>
            <div class="alert alert-success">
                <div class="row">
                    <div class="col-md-6">
                        收文文號：
                        <asp:TextBox ID="txtWpinno" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-md-6">
                        調閱類型：
                         <asp:DropDownList ID="ddlViewType" Style="width: 150px" runat="server">
                                    <asp:ListItem Value="0">請選擇</asp:ListItem>
                                    <asp:ListItem Value="1" Selected="True">1. 紙本調閱</asp:ListItem>
                                    <asp:ListItem Value="2">2. 電子調閱</asp:ListItem>
                                </asp:DropDownList>
                        <input id="btnCheckWpinno" type="button" value="確認收文號" class="btn btn-large btn-danger" onclick="javascript: GetAlsoFile()" />
                    </div>
                </div>
            </div>
            <div class="panel-body">
                <div id="ShowInfo">
                    <div class="alert alert-success" role="alert">還檔展期詳細資料如下：</div>
                    <table class="ItemTD_green" style="width: 98%; float: right; border-collapse: separate; border-spacing: 1px;" border="1">
                        <tr>
                            <td class="HeadTD_green" style="padding: 5px;">收文文號：</td>
                            <td style="padding: 5px; text-align: left;">
                                <asp:Label ID="lblWpinno" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="HeadTD_green" style="padding: 5px;">發文文號：</td>
                            <td style="padding: 5px; text-align: left;">
                                <asp:Label ID="lblWpoutNo" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="HeadTD_green" style="padding: 5px;">預約時間：</td>
                            <td style="padding: 5px; text-align: left;">
                                <asp:Label ID="lblTranst" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="HeadTD_green" style="padding: 5px;">預 約 人：</td>
                            <td style="padding: 5px; text-align: left;">
                                <asp:Label ID="lblReceiver" runat="server" Text=""></asp:Label>
                                <asp:Label ID="lblRname" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="HeadTD_green" style="padding: 5px;">借檔類別：</td>
                            <td style="padding: 5px; text-align: left;">
                                <asp:Label ID="lblApplkind" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="HeadTD_green" style="padding: 5px;">展期還檔日：</td>
                            <td style="padding: 5px; text-align: left;">
                                <asp:Label ID="lblredate1" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="HeadTD_green" style="padding: 5px;">是否已展期：</td>
                            <td style="padding: 5px; text-align: left;">

                                <asp:Label ID="lblexten" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="HeadTD_green" style="padding: 5px;">原定還檔日：</td>
                            <td style="padding: 5px; text-align: left;">
                                <asp:Label ID="lblredate" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="HeadTD_green" style="padding: 5px;">審核人員：</td>
                            <td style="padding: 5px; text-align: left;">
                                <asp:DropDownList ID="ddlApproveuserID" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <table border="0" style="width: 100%; border-collapse: collapse">
                        <tr style="text-align: center">
                            <td style="text-align: right">
                                <asp:Button ID="BtnOK" runat="server" Text="展 期" class="btn btn-large btn-success" OnClick="BtnOK_Click" OnClientClick="JavaScript:if(!ImageBtnOKClick()) {return false} ;"  />
                            </td>
                            <td style="text-align: center"></td>
                            <td style="text-align: left">
                                <asp:Button ID="BtnClear" runat="server" Text="取 消" class="btn btn-large btn-success" OnClick="BtnClear_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <br />
        </div>
        <div id="btn1">
        </div>
        <cc1:LiteralMessageBox ID="LiteralMessageBox1" runat="server"></cc1:LiteralMessageBox>
        <input id="HiddenMessage" type="Hidden" runat="server" />
    </div>
</asp:Content>
