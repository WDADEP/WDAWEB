<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TransReceiver.aspx.cs" Inherits="WDA.TransReceiver" %>

<%@ Register Assembly="WDA" Namespace="WDA" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function pageLoad() {
            showMessage();
            if ($("#MainContent_HiddenShowPanel").val() == "true") {
                $('#divPanel').show();
            }
            else {
                $('#divPanel').hide();
            }
        }

        function ImageBtnOKClick() {
            try {
                var txtWPINNO = $get('MainContent_TxtWpinno').value;
                var txtReceiver = $get('MainContent_TxtReceiver').value;

                if (txtWPINNO.length == 0) {
                    alert('請輸入收文文號');
                    return false;
                }
                if (txtReceiver.length == 0) {
                    alert('請輸入收文者姓名');
                    return false;
                }

                $('#divPanel').show();
                return true;
            }
            catch (e) {
                alert(e.message);
                Sys.Debug.traceDump(e);
            };
        }
    </script>
    <div class="well well-lg">
        <div class="panel panel-primary">
            <div class="panel-heading">
                <h3 class="panel-title">簽收功能─新增</h3>
            </div>
            <div class="panel-body">
                <table class="ItemTD_green" style="width: 98%; float: right; border-collapse: separate; border-spacing: 1px;" border="1">
                    <tr>
                        <td class="HeadTD_green" style="padding: 5px;">收文文號：</td>
                        <td style="padding: 5px; text-align: left;">
                            <asp:TextBox ID="TxtWpinno" runat="server" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="HeadTD_green" style="padding: 5px;">接 收 文 件 者：
                        </td>
                        <td style="padding: 5px; text-align: left;">
                            <asp:TextBox ID="TxtReceiver" runat="server" ReadOnly="true" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>
            <table border="0" style="width: 100%; border-collapse: collapse">
                <tr style="text-align: center">
                    <td style="text-align: right">
                        <asp:Button ID="BtnOK" runat="server" Text="確 定" class="btn btn-large btn-success" OnClientClick="JavaScript:if(!ImageBtnOKClick()) {return false} ;" OnClick="BtnOK_Click" />
                    </td>
                    <td style="text-align: center"></td>
                    <td style="text-align: left">
                        <asp:Button ID="BtnClear" runat="server" Text="取 消" class="btn btn-large btn-success" OnClick="BtnClear_Click" />
                    </td>
                </tr>
            </table>
        </div>
        <div id="divPanel">
            <br>
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <h3 class="panel-title">簽收功能─今日新增結果列表如下：</h3>
                </div>
                <div class="panel-body">
                    <div style="width: 98%; text-align: right">
                    </div>
                </div>
                <asp:GridView ID="GridView1" runat="server" AllowSorting="True" AutoGenerateColumns="False" CssClass="GridViewStyle" Width="98%" OnSorting="GridView1_Sorting" AllowPaging="True" OnPageIndexChanging="GridView1_PageIndexChanging">
                    <AlternatingRowStyle CssClass="AlternatingRowStyle" />
                    <Columns>
                        <asp:BoundField HeaderText="收文文號" DataField="WPINNO" SortExpression="WPINNO" ReadOnly="True" />
                        <asp:BoundField HeaderText="接收文件者" DataField="RECEIVER" SortExpression="RECEIVER" ReadOnly="True" />
                        <asp:BoundField HeaderText="收文日期" DataField="TRANSTIME" SortExpression="TRANSTIME" ReadOnly="True" />
                        <asp:BoundField HeaderText="聯絡人姓名" DataField="COMMNAME" SortExpression="COMMNAME" ReadOnly="True" />
                    </Columns>
                    <FooterStyle CssClass="FooterStyle" />
                    <HeaderStyle CssClass="HeaderStyle" />
                    <PagerStyle CssClass="PagerStyle" HorizontalAlign="Center" />
                    <RowStyle CssClass="RowStyle" />
                    <PagerSettings Mode="NumericFirstLast" FirstPageText="[第一頁]" LastPageText="[最末頁]" />
                </asp:GridView>
                <table id="tbPageLayer_GridView1" style="border-right: #ffffff 1px solid; border-top: #ffffff 1px solid; border-left: #e6e6e6 1px solid; border-bottom: #e6e6e6 1px solid; width: 100%;" border="1">
                    <tr>
                        <td style="padding: 0; float: none; background-color: #ffffff; text-align: right;" class="t12_blue">
                            <asp:Label ID="lblTotalPage_GridView1" runat="server"></asp:Label>
                            <asp:Label ID="lblPage_GridView1" runat="server"></asp:Label>
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
