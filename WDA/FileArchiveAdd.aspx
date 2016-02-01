<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FileArchiveAdd.aspx.cs" Inherits="WDA.FileArchiveAdd" %>

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

        function ImageBtnOKClick() {
            try {
                var txtQueryBarcodeValue = $get('MainContent_txtQueryBarcodeValue').value;

                if (txtQueryBarcodeValue.length == 0) {
                    alert('請輸入收文文號。');
                    return false;
                }
                return true;
            }
            catch (e) {
                alert(e.message);
                Sys.Debug.traceDump(e);
            };
        }

        function ImageBtnAddClick() {
            try {
                var txtFileNo = $get('MainContent_txtFileNo').value;
                var txtFileDate = $get('MainContent_txtFileDate').value;
                var txtBoxNo = $get('MainContent_txtBoxNo').value;
                var txtKeepYr = $get('MainContent_txtKeepYr').value;

                if (txtFileNo.length == 0 ||
                    txtFileDate.length ==0 ||
                    txtBoxNo.length == 0 ||
                    txtKeepYr.length == 0) {
                    alert('請輸入必填欄位。');
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
            <li>請先輸入任一收文文號查詢「案件編號」。</li>
            <li>完成後請按【確定】鍵執行查詢動作。</li>
            <li>輸入【歸檔登入─新增】資訊。</li>
            <li>完成後請按【新增】鍵執行新增動作。</li>
        </ol>
    </div>
    <div class="well well-lg">
        <div id="divQuery" class="panel panel-primary">
            <div class="panel-heading">
                <h3 class="panel-title">歸檔登入(新增)─查詢</h3>
            </div>
            <div class="alert alert-success">
                <div class="row">
                    <div class="col-md-6">
                        收文文號：
                        <asp:TextBox ID="txtQueryBarcodeValue" runat="server" MaxLength="10"></asp:TextBox>
                         <asp:Button ID="BtnOK" runat="server" Text="確 定" CssClass="btn btn-large btn-success" OnClientClick="JavaScript:if(!ImageBtnOKClick()) {return false};" OnClick="BtnOK_Click" />
                    </div>
                    <div class="col-md-6">
                       卷宗號是否自動加號：
                        <asp:RadioButtonList ID="RadioButtonList1" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Value="0">是</asp:ListItem>
                            <asp:ListItem Value="1">否</asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </div>
            </div>
        </div>
        <div id="divPanel">
            <br>
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <h3 class="panel-title">歸檔登入─新增：</h3>
                </div>
                <div class="panel-body">
                    <table class="ItemTD_green" style="width: 98%; float: right; border-collapse: separate; border-spacing: 1px;" border="1">
                        <tr>
                            <td class="HeadTD_green" style="padding: 5px;"><span class="t15_red">＊</span>收文文號：</td>
                            <td style="padding: 5px; text-align: left;">
                                <asp:TextBox ID="txtBarcodeValue" runat="server" TextMode="Number" MaxLength="10"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="HeadTD_green" style="padding: 5px;"><span class="t15_red">＊</span>歸檔檔號：</td>
                            <td style="padding: 5px; text-align: left;">
                                <asp:TextBox ID="txtFileNo" runat="server" TextMode="Number" MaxLength="10"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="HeadTD_green" style="padding: 5px;"><span class="t15_red">＊</span>卷宗號：</td>
                            <td style="padding: 5px; text-align: left;">
                                <asp:TextBox ID="txtBoxNo" runat="server" TextMode="Number" MaxLength="11"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="HeadTD_green" style="padding: 5px;"><span class="t15_red">＊</span>保存年限：</td>
                            <td style="padding: 5px; text-align: left;">
                                <asp:TextBox ID="txtKeepYr" runat="server" TextMode="Number" MaxLength="2"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="HeadTD_green" style="padding: 5px;"><span class="t15_red">＊</span>歸檔日期：</td>
                            <td style="padding: 5px; text-align: left;">
                                <asp:TextBox ID="txtFileDate" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="HeadTD_green" style="padding: 5px;"><span class="t15_red">＊</span>歸檔作業者：</td>
                            <td style="padding: 5px; text-align: left;">
                                <asp:TextBox ID="txtOnFile" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <table border="0" style="width: 100%; border-collapse: collapse">
                    <tr style="text-align: center">
                        <td style="text-align: center">
                            <asp:Button ID="BtnAdd" runat="server" Text="新 增" CssClass="btn btn-large btn-success" OnClientClick="JavaScript:if(!ImageBtnAddClick()) {return false};" OnClick="BtnAdd_Click" />
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
