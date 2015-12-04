<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CaseQueryMenu.aspx.cs" Inherits="WDA.CaseQueryMenu" %>

<%@ Register Assembly="WDA" Namespace="WDA" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function pageLoad() {
            showMessage();
            $("#MainContent_txtCreateTime").datepicker("option", $.datepicker.regional["zh-TW"]);
            $("#MainContent_txtEndTime").datepicker("option", $.datepicker.regional["zh-TW"]);
            $("#MainContent_txtCreateTime").datepicker({
                defaultDate: new Date(),
                changeMonth: true,
                numberOfMonths: 1,
                onClose: function (selectedDate) {
                    $("#MainContent_txtEndTime").datepicker("option", "minDate", selectedDate);
                }
            });
            $("#MainContent_txtEndTime").datepicker({
                defaultDate: new Date(),
                changeMonth: true,
                numberOfMonths: 1,
                onClose: function (selectedDate) {
                    $("#MainContent_txtCreateTime").datepicker("option", "maxDate", selectedDate);
                }
            });
                 if ($("#MainContent_HiddenShowPanel").val() == "true") {
                     $('#divPanel').show();
                 }
                 else {
                     $('#divPanel').hide();
                 }
        }

        function ImageBtnOKClick() {
            try {
                var txtCreateTime = $get('MainContent_txtCreateTime').value;
                var txtEndTime = $get('MainContent_txtEndTime').value;

                if (txtCreateTime.length != 0 && txtEndTime.length == 0) {
                    $('#MainContent_txtEndTime').val($('#MainContent_txtCreateTime').val()); IsPass = true;
                }
                if (txtCreateTime.length == 0 && txtEndTime.length != 0) {
                    $('#MainContent_txtCreateTime').val($('#MainContent_txtEndTime').val()); IsPass = true;
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
            <li>設定「查詢條件」，完成後請按【確定】鍵執行查詢動作。</li>
        </ol>
    </div>
    <div class="well well-lg">
        <div class="panel panel-primary">
            <div class="panel-heading">
                <h3 class="panel-title">上傳影像─查詢</h3>
            </div>
            <div class="panel-body">
                <table class="ItemTD_green" style="width: 98%; float: right; border-collapse: separate; border-spacing: 1px;" border="1">
                    <tr>
                        <td class="HeadTD_green" style="width: 20px">發文號：</td>
                        <td style="padding: 5px; text-align: left;">
                            <asp:TextBox ID="txtWPINNO" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="HeadTD_green" style="width: 20px">建立者帳號：</td>
                        <td style="padding: 5px; text-align: left;">
                            <asp:TextBox ID="txtUserName" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="HeadTD_green" style="width: 20px">掃描時間：</td>
                        <td style="padding: 5px; text-align: left;">
                            <strong>開始日期：
                                <asp:TextBox ID="txtCreateTime" runat="server" MaxLength="10"></asp:TextBox>00:00～結束日期：
										<asp:TextBox ID="txtEndTime" runat="server" MaxLength="10"></asp:TextBox>23:59</strong>
                        </td>
                    </tr>
                </table>
            </div>
            <table border="0" style="width: 100%; border-collapse: collapse">
                <tr style="text-align: center">
                    <td style="text-align: right">
                        <asp:Button ID="BtnOK" runat="server" Text="確 定" class="btn btn-large btn-success" OnClick="BtnOK_Click" OnClientClick="JavaScript:if(!ImageBtnOKClick()) {return false} ;" />
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
                    <h3 class="panel-title">上傳影像查詢─詳細列表如下：</h3>
                </div>
                <div class="panel-body">
                    <div style="width: 98%; text-align: right">
                    </div>
                </div>
                <asp:GridView ID="GridView1" runat="server" AllowSorting="True" AutoGenerateColumns="False" CssClass="GridViewStyle" Width="98%" OnSorting="GridView1_Sorting" AllowPaging="True" OnPageIndexChanging="GridView1_PageIndexChanging" OnRowDataBound="GridView1_RowDataBound" OnRowCreated="GridView1_RowCreated">
                        <AlternatingRowStyle CssClass="AlternatingRowStyle" />
                        <Columns>
                            <asp:TemplateField HeaderText="全選">
                                <HeaderTemplate>
                                    <asp:CheckBox ID="ChkBoxAll1" runat="server" ForeColor="#CC3300" Text="調閱"
                                        ToolTip="按一次全選，再按一次取消全選" onclick="JavaScript:SelectAllCheckboxes(this,0);" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkViewer" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="調閱">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgBtnProduction" runat="server" ImageUrl="~/Images/view.gif"  OnClick="ImgBtnProduction_Click" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="所有版本">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgBtnAllVersion" runat="server" ImageUrl="~/Images/system.gif" OnClick="ImgBtnAllVersion_Click"/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="案件編號" DataField="CaseID" SortExpression="CaseID" />
                            <asp:BoundField HeaderText="發文號" DataField="Barcodevalue" SortExpression="Barcodevalue" />
                            <asp:BoundField HeaderText="建立者編號" DataField="UserName" SortExpression="UserName" />
                            <asp:BoundField HeaderText="建立日期" DataField="CreateTime" SortExpression="CreateTime" />
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
