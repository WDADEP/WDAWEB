<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReservationBorrowCancel.aspx.cs" Inherits="WDA.ReservationBorrowCancel" %>

<%@ Register Assembly="WDA" Namespace="WDA" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">

        function pageLoad() {
            if ($("#MainContent_HiddenShowPanel").val() == "true") {
                $('#divPanel').show();
            }
            else {
                $('#divPanel').hide();
            }

            showMessage();
        }

        function ImageBtnQueryClick() {
            try {
                var txtWpinNo = $get('MainContent_txtWpinNo').value;

                if (txtWpinNo.length == 0) {
                    alert('請輸入「局收文號」。');
                    return false;
                }
                return true;
            }
            catch (e) {
                alert(e.message);
                Sys.Debug.traceDump(e);
            };
        }
    </script>
    <div class="alert alert-info">
        <button type="button" class="close" data-dismiss="alert">&times;</button>
        <h4>說明：</h4>
        <ol>
            <li>請先輸入「局收文號」，完成後請按【查詢】鍵執行查詢動作。</li>
            <li>輸入相關資訊。</li>
            <li>按下「取消預約」按鈕完成取消作業。</li>
        </ol>
    </div>
    <div class="well well-lg">
        <div id="divQuery" class="panel panel-primary" runat="server">
            <div class="panel-heading">
                <h3 class="panel-title">預約借檔(取消)─查詢</h3>
            </div>
            <div class="panel-body">
                <table class="ItemTD_green" style="width: 98%; float: right; border-collapse: separate; border-spacing: 1px;" border="1">
                    <tr>
                        <td class="HeadTD_green" style="padding: 5px;"><span class="t15_red">＊</span>局收文號</td>
                        <td style="padding: 5px; text-align: left;">
                            <asp:TextBox ID="txtWpinNo" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>
            <table border="0" style="width: 100%; border-collapse: collapse">
                <tr style="text-align: center">
                    <td style="text-align: center">
                        <asp:Button ID="BtnQuery" runat="server" Text="查 詢" CssClass="btn btn-large btn-success" OnClientClick="JavaScript:if(!ImageBtnQueryClick()) {return false};" OnClick="BtnQuery_Click" />
                    </td>
                </tr>
            </table>
        </div>
        <div id="divPanel">
            <br>
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <h3 class="panel-title">預約借檔─取消：</h3>
                </div>
                <div class="panel-body">
                    <table class="ItemTD_green" style="width: 98%; float: right; border-collapse: separate; border-spacing: 1px;" border="1">
                        <tr>
                            <td class="HeadTD_green" style="padding: 5px;">會發文號：</td>
                            <td style="padding: 5px; text-align: left;">
                                <asp:TextBox ID="txtWpoutNo" runat="server" TextMode="Number"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="HeadTD_green" style="padding: 5px;">使用者分機：</td>
                            <td style="padding: 5px; text-align: left;">
                                <asp:TextBox ID="txtTel" runat="server" TextMode="Number"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="HeadTD_green" style="padding: 5px;">借檔類別：</td>
                            <td style="padding: 5px; text-align: left;">
                                <asp:DropDownList ID="ddlKind" Style="width: 150px" runat="server">
                                    <asp:ListItem Value="0">請選擇</asp:ListItem>
                                    <asp:ListItem Value="1">1. 一般</asp:ListItem>
                                    <asp:ListItem Value="2">2. 法制</asp:ListItem>
                                    <asp:ListItem Value="3">3. 行政救濟</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="HeadTD_green" style="padding: 5px;">調卷者：</td>
                            <td style="padding: 5px; text-align: left;">
                                <asp:TextBox ID="txtReceiver" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="HeadTD_green" style="padding: 5px;">收文日期：</td>
                            <td style="padding: 5px; text-align: left;">
                                <asp:TextBox ID="txtWpindate" runat="server" pattern="\d{4}/\d{1,2}/\d{1,2}" title="日期格式"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="HeadTD_green" style="padding: 5px;">圈名：</td>
                            <td style="padding: 5px; text-align: left;">
                                <asp:TextBox ID="txtCirlName" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="HeadTD_green" style="padding: 5px;">雇主編號：</td>
                            <td style="padding: 5px; text-align: left;">
                                <asp:TextBox ID="txtCaseNo" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="HeadTD_green" style="padding: 5px;">聯絡人姓名：</td>
                            <td style="padding: 5px; text-align: left;">
                                <asp:TextBox ID="txtCommName" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="HeadTD_green" style="padding: 5px;">備註：</td>
                            <td style="padding: 5px; text-align: left;">
                                <asp:TextBox ID="txtNote" Style="width: 300px" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="HeadTD_green" style="padding: 5px;">調閱類型：</td>
                            <td style="padding: 5px; text-align: left;">
                                <asp:DropDownList ID="ddlViewType" Style="width: 150px" runat="server">
                                    <asp:ListItem Value="0">請選擇</asp:ListItem>
                                    <asp:ListItem Value="1">1. 紙本調閱</asp:ListItem>
                                    <asp:ListItem Value="2">2. 電子調閱</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </div>
                <table border="0" style="width: 100%; border-collapse: collapse">
                    <tr style="text-align: center">
                        <td style="text-align: center">
                            <asp:Button ID="BtnOK" runat="server" Text="取 消 預 約" CssClass="btn btn-large btn-success" OnClick="BtnOK_Click" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <cc1:LiteralMessageBox ID="LiteralMessageBox1" runat="server"></cc1:LiteralMessageBox>
        <input id="HiddenMessage" type="Hidden" runat="server" />
        <input id="HiddenShowPanel" type="Hidden" runat="server" />
    </div>
</asp:Content>
