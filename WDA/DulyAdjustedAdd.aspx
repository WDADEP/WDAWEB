<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DulyAdjustedAdd.aspx.cs" Inherits="WDA.DulyAdjustedAdd" %>

<%@ Register Assembly="WDA" Namespace="WDA" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
        <script type="text/javascript">
            var IsCheckWpinno = false;
            function pageLoad() {
                //if (IsShowInfo)
                //{
                //    $('#ShowInfo').show();
                //}
                //else {
                //    $('#ShowInfo').hide();
                //}
                $('#ShowInfo').hide();
                showMessage();
            }

            function ImageBtnOKClick() {
                try {
                    if (!IsCheckWpinno) { alert('請先確認發文文號!!'); return false; }
                    return true;
                }
                catch (e) {
                    alert(e.message);
                    Sys.Debug.traceDump(e);
                };
            }

            function GetDulyAdjusted() {
                //if (event.keyCode == 13) {
                    var wpinno = $get("MainContent_txtWpinno").value;

                    if (wpinno.length != 0) {
                        WDA.AspNetAjaxInAction.GetDulyAdjusted(wpinno, onGetDulyAdjustedSuccess, onGetDulyAdjustedFailure, "context", 1000);
                    }
                    else { IsCheckWpinno = false; }
                //}
            }

            function onGetDulyAdjustedSuccess(result, context, methodName) {
                    for (var i = 0; i < result.length; i++) {
                        var bev = result[i];

                        if (bev.WpinDate.length > 0) {
                            IsCheckWpinno = true;
                            $('#ShowInfo').show();
                        }
                        else { alert("發文號不存在或已經調妥新增完成尚未還檔、請重新輸入"); $('#ShowInfo').hide(); IsCheckWpinno = false; return; }

                        $get("MainContent_lblWpindate").innerText = bev.WpinDate;
                        $get("MainContent_lblWpoutNo").innerText = bev.WpouTno;
                        $get("MainContent_lblWpoutdate").innerText = bev.WpoutDate;
                        $get("MainContent_lblCaseno").innerText = bev.CaseNo;
                        $get("MainContent_lblApplkind").innerText = bev.ApplKind;
                        $get("MainContent_lblCommname").innerText = bev.CommName;
                        $get("MainContent_lblReceiver").innerText = bev.Receiver;
                        $get("MainContent_lblRname").innerText = bev.RName;
                        $get("MainContent_lblTranst").innerText = bev.Transt;
                        $get("MainContent_lblGetime").innerText = bev.Getime;
                    }
                }
            function onGetDulyAdjustedFailure(error, context, methodName) {
             var errorMessage = error.get_message();
         }
         </script>
    <div class="alert alert-info">
        <button type="button" class="close" data-dismiss="alert">&times;</button>
        <h4>說明：</h4>
        <ol>
            <li>輸入「收文文號」，完成後請按下<kbd>「確認收文號」</kbd></li>
            <li>按下「新增」按鈕完成新增作業</li>
        </ol>
    </div>
    <div class="well well-lg">
        <div class="panel panel-primary">
            <div class="panel-heading">
                <h3 class="panel-title">調妥作業─新增</h3>
            </div>
            <p  class="alert alert-success">
                收文文號： <asp:TextBox ID="txtWpinno" runat="server"></asp:TextBox>
                <input id="btnCheck" type="button" value="確認收文號" class="btn btn-large btn-danger" onclick ="javascript:GetDulyAdjusted()"/>
                  <%--<asp:Button ID="btnCheck" runat="server" Text="確認收文號" class="btn btn-large btn-danger" OnClientClick="javascript:GetDulyAdjusted()"/>--%>
            </p>
            <div class="panel-body">
                <div id="ShowInfo" >
                     <div class="alert alert-success" role="alert">調妥新增詳細資料如下：</div>
                    <table class="ItemTD_green" style="width: 98%; float: right; border-collapse: separate; border-spacing: 1px;" border="1">
                        <tr>
                        <td class="HeadTD_green" style="padding: 5px;">收文日期：</td>
                        <td style="padding: 5px; text-align: left;">
                            <asp:Label ID="lblWpindate" runat="server" Text="lblWpindate"></asp:Label>
                        </td>
                    </tr>
                        <tr>
                            <td class="HeadTD_green" style="padding: 5px;">發文文號：</td>
                            <td style="padding: 5px; text-align: left;">

                                <asp:Label ID="lblWpoutNo" runat="server" Text="lblWpoutNo"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="HeadTD_green" style="padding: 5px;">發文日期：</td>
                            <td style="padding: 5px; text-align: left;">
                                <asp:Label ID="lblWpoutdate" runat="server" Text="lblWpoutdate"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="HeadTD_green" style="padding: 5px;">案號：</td>
                            <td style="padding: 5px; text-align: left;">
                                <asp:Label ID="lblCaseno" runat="server" Text="lblCaseno"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="HeadTD_green" style="padding: 5px;">案 件 別：</td>
                            <td style="padding: 5px; text-align: left;">
                                <asp:Label ID="lblApplkind" runat="server" Text="lblApplkind"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="HeadTD_green" style="padding: 5px;">雇主名稱：</td>
                            <td style="padding: 5px; text-align: left;">
                                <asp:Label ID="lblCommname" runat="server" Text="lblCommname"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="HeadTD_green" style="padding: 5px;">預 約 人：</td>
                            <td style="padding: 5px; text-align: left;">
                                <asp:Label ID="lblReceiver" runat="server" Text="lblReceiver"></asp:Label>
                                <asp:Label ID="lblRname" runat="server" Text="lblRname"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="HeadTD_green" style="padding: 5px;">預約時間：</td>
                            <td style="padding: 5px; text-align: left;">
                                <asp:Label ID="lblTranst" runat="server" Text="lblTranst"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="HeadTD_green" style="padding: 5px;">登錄時間：</td>
                            <td style="padding: 5px; text-align: left;">
                                <asp:Label ID="lblGetime" runat="server" Text="lblGetime"></asp:Label>
                            </td>
                        </tr>
                        <%-- <tr>
                        <td class="HeadTD_green" style="padding: 5px;">登 錄 人：</td>
                        <td style="padding: 5px; text-align: left;">
                            <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                        </td>
                    </tr>--%>
                    </table>
                </div>
        </div>
         
                <table border="0" style="width: 100%; border-collapse: collapse">
                    <tr style="text-align: center">
                        <td style="text-align: right">
                            <asp:Button ID="BtnOK" runat="server" Text="新 增" class="btn btn-large btn-success" OnClientClick="JavaScript:if(!ImageBtnOKClick()) {return false} ;" OnClick="BtnOK_Click" />
                        </td>
                        <td style="text-align: center">
                            <asp:Button ID="BtnScan" runat="server" Text="掃 描" class="btn btn-large btn-success" OnClick="BtnScan_Click" OnClientClick="JavaScript:if(!ImageBtnOKClick()) {return false} ;"/>
                        </td>
                        <td style="text-align: left">
                            <asp:Button ID="BtnClear" runat="server" Text="取 消" class="btn btn-large btn-success" OnClick="BtnClear_Click" />
                        </td>
                    </tr>
                </table>
          <br/>
            </div>
        <cc1:LiteralMessageBox ID="LiteralMessageBox1" runat="server"></cc1:LiteralMessageBox>
        <input id="HiddenMessage" type="Hidden" runat="server" />
    </div>
</asp:Content>
