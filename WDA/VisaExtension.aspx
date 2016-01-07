<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="VisaExtension.aspx.cs" Inherits="WDA.VisaExtension" %>

<%@ Register Assembly="WDA" Namespace="WDA" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <script type="text/javascript">
         function pageLoad() {
             showMessage();
         }

         function SelectAllCheckboxes(oCheckbox, a) {
             var rBtnListApproveAll = $("input[id^='MainContent_GridView1_rBtnListApproveAll']:checked").val();
             var grid = $get("<%= GridView1.ClientID %>");
             for (i = 1; i < grid.rows.length; i++) {
                 if (rBtnListApproveAll == 'D') {
                     grid.rows[i].cells[a].getElementsByTagName("INPUT")[0].checked = true;
                 }
                 else if (rBtnListApproveAll == 'Z') {
                     grid.rows[i].cells[a].getElementsByTagName("INPUT")[1].checked = true;
                 }
             }
         }
         </script>
    <div class="well well-lg">
        <div id="divPanel">
            <br>
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <h3 class="panel-title">還檔作業─簽准展期：</h3>
                </div>
                <asp:GridView ID="GridView1" runat="server" AllowSorting="True" AutoGenerateColumns="False" CssClass="GridViewStyle" Width="98%" OnPageIndexChanging="GridView1_PageIndexChanging" OnRowDataBound="GridView1_RowDataBound" OnSorting="GridView1_Sorting" OnRowCreated="GridView1_RowCreated">
                    <AlternatingRowStyle CssClass="AlternatingRowStyle" />
                    <Columns>
                        <asp:TemplateField HeaderText="簽核">
                            <HeaderTemplate>
                                <asp:RadioButtonList ID="rBtnListApproveAll" ForeColor="#CC3300" runat="server" RepeatDirection="Horizontal" onclick="JavaScript:SelectAllCheckboxes(this,0);">
                                    <asp:ListItem Value="D">同意</asp:ListItem>
                                    <asp:ListItem Value="Z">不同意</asp:ListItem>
                                </asp:RadioButtonList>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:RadioButtonList ID="rBtnListApprove" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="D">同意</asp:ListItem>
                                    <asp:ListItem Value="Z">不同意</asp:ListItem>
                                </asp:RadioButtonList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="收文號" DataField="WpinNo" SortExpression="WpinNo"/>
                        <asp:BoundField HeaderText="發文號" DataField="WpoutNo" SortExpression="WpoutNo"/>
                        <asp:TemplateField HeaderText="預約時間">
                            <ItemTemplate>
                                <asp:Label ID="LblTranst" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="預約人" DataField="RECEIVER" SortExpression="RECEIVER" />
                        <asp:BoundField HeaderText="案件別" DataField="kindName" SortExpression="kindName"/>
                        <asp:TemplateField HeaderText="原定還檔日">
                            <ItemTemplate>
                                <asp:Label ID="LblReDate" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="是否已展期">
                            <ItemTemplate>
                                <asp:Label ID="LblExten" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="展期還檔日">
                            <ItemTemplate>
                                <asp:Label ID="LblExtenReDate" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="展期次數" DataField="EXTENSIONCOUNT" SortExpression="EXTENSIONCOUNT"/>
                        <asp:BoundField DataField="TRANST" HeaderText="TRANST" SortExpression="TRANST" />
                        <asp:BoundField DataField="RECEIVER" HeaderText="RECEIVER" SortExpression="RECEIVER" />
                        <asp:BoundField DataField="RealName" HeaderText="RealName" SortExpression="RealName" />
                        <asp:BoundField DataField="EXTEN" HeaderText="EXTEN" SortExpression="EXTEN" />
                        <asp:BoundField DataField="EXTENSIONDATE" HeaderText="EXTENSIONDATE" SortExpression="EXTENSIONDATE" />
                        <asp:BoundField DataField="EXTENSIONCOUNT" HeaderText="EXTENSIONCOUNT" SortExpression="EXTENSIONCOUNT" />
                        <asp:BoundField DataField="KIND" HeaderText="KIND" SortExpression="KIND" />
                        <asp:BoundField DataField="VIEWTYPE" HeaderText="VIEWTYPE" SortExpression="VIEWTYPE" />
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
                <table border="0" style="width: 100%; border-collapse: collapse">
                    <tr style="text-align: center">
                        <td style="text-align: center">
                            <asp:Button ID="BtnOK" runat="server" Text="確 定" CssClass="btn btn-large btn-success" OnClick="BtnOK_Click" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
       <cc1:LiteralMessageBox ID="LiteralMessageBox1" runat="server"></cc1:LiteralMessageBox>
        <input id="HiddenMessage" type="Hidden" runat="server" />
    </div>
</asp:Content>
