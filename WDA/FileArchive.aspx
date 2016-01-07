<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FileArchive.aspx.cs" Inherits="WDA.FileArchive" %>

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

            $("#MainContent_txtFileDate").datepicker("option", $.datepicker.regional["zh-TW"]);

            $("#MainContent_txtFileDate").datepicker({
                defaultDate: new Date(),
                changeMonth: true,
                numberOfMonths: 1
            });

            showMessage();
        }

        function ImageBtnPringClick() {
            try {
                if ($("#MainContent_HiddenShowPanel").val() == "true") {
                    var scriptName = document.forms[0].action;
                    window.document.forms[0].target = '_blank';
                    setTimeout(function () {
                        window.document.forms[0].target = '';
                        document.forms[0].action = scriptName;
                    }, 500);
                    return true;
                }
                else {
                    alert('請先進行查詢。');
                    return false;
                }
            }
            catch (e) {
                alert(e.message);
                Sys.Debug.traceDump(e);
            };
        }

        function ImageBtnOKClick() {
            try {
                var txtBarcodeValue = $get('MainContent_txtBarcodeValue').value;
                var txtWpoutNo = $get('MainContent_txtWpoutNo').value;
                var txtFileNo = $get('MainContent_txtFileNo').value;
                var txtFileDate = $get('MainContent_txtFileDate').value;
                var txtKeepYr = $get('MainContent_txtKeepYr').value;
                var txtBoxNo = $get('MainContent_txtBoxNo').value;
                var txtOnFile = $get('MainContent_txtOnFile').value;

                if (txtBarcodeValue.length == 0 &&
                    txtWpoutNo.length == 0 &&
                    txtFileNo.length == 0 &&
                    txtFileDate.length == 0 &&
                    txtKeepYr.length == 0 &&
                    txtBoxNo.length == 0 &&
                    txtOnFile.length == 0) {

                    alert('請輸入至少填入一個搜尋條件。');
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
            <li>您可設定一個以上的查詢條件，以取得更精確的查詢結果。</li>
            <li>設定「查詢條件」，完成後請按【確定】鍵執行查詢動作。</li>
        </ol>
    </div>
    <div class="well well-lg">
        <div class="panel panel-primary">
            <div class="panel-heading">
                <h3 class="panel-title">歸檔登入─管理</h3>
            </div>
            <div class="panel-body">
                <table class="ItemTD_green" style="width: 98%; float: right; border-collapse: separate; border-spacing: 1px;" border="1">
                    <tr>
                        <td class="HeadTD_green" style="padding: 5px;">收文文號：</td>
                        <td style="padding: 5px; text-align: left;">
                            <asp:TextBox ID="txtBarcodeValue" runat="server" TextMode="Number"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="HeadTD_green" style="padding: 5px;">發文文號：</td>
                        <td style="padding: 5px; text-align: left;">
                            <asp:TextBox ID="txtWpoutNo" runat="server" TextMode="Number"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="HeadTD_green" style="padding: 5px;">歸檔檔號：</td>
                        <td style="padding: 5px; text-align: left;">
                            <asp:TextBox ID="txtFileNo" runat="server" TextMode="Number"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="HeadTD_green" style="padding: 5px;">歸檔日期：</td>
                        <td style="padding: 5px; text-align: left;">
                            <asp:TextBox ID="txtFileDate" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="HeadTD_green" style="padding: 5px;">保存年限：</td>
                        <td style="padding: 5px; text-align: left;">
                            <asp:TextBox ID="txtKeepYr" runat="server" TextMode="Number"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="HeadTD_green" style="padding: 5px;">卷宗號：</td>
                        <td style="padding: 5px; text-align: left;">
                            <asp:TextBox ID="txtBoxNo" runat="server" TextMode="Number"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="HeadTD_green" style="padding: 5px;">歸檔作業者：</td>
                        <td style="padding: 5px; text-align: left;">
                            <asp:TextBox ID="txtOnFile" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>
            <table border="0" style="width: 100%; border-collapse: collapse">
                <tr style="text-align: center">
                    <td style="text-align: center">
                        <asp:Button ID="BtnOK" runat="server" Text="確 定" class="btn btn-large btn-success" OnClientClick="JavaScript:if(!ImageBtnOKClick()) {return false} ;" OnClick="BtnOK_Click" />
                    </td>
                </tr>
            </table>
        </div>
        <div id="divPanel">
            <br>
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <h3 class="panel-title">歸檔登入查詢─詳細列表如下：</h3>
                </div>
                <div class="panel-body">
                    <div style="width: 98%; text-align: right">
                        <asp:Button ID="BtnPrint" runat="server" Text="列 印" class="btn btn-large btn-success" OnClick="BtnPrint_Click" OnClientClick="JavaScript:if(!ImageBtnPringClick()) {return false} ;" PostBackUrl="~/FileArchivePrint.aspx" />
                    </div>
                </div>
                <asp:GridView ID="GridView1" runat="server" AllowSorting="True" AutoGenerateColumns="False" CssClass="GridViewStyle" Width="98%" OnPageIndexChanging="GridView1_PageIndexChanging" OnRowCommand="GridView1_RowCommand" OnRowDataBound="GridView1_RowDataBound" OnSorting="GridView1_Sorting">
                    <AlternatingRowStyle CssClass="AlternatingRowStyle" />
                    <Columns>
                        <asp:TemplateField HeaderText="編輯">
                            <EditItemTemplate>
                                <asp:ImageButton ID="ITImageListBtnChang" runat="server" ImageUrl="~/Images/Chang.gif" OnClick="ITImageListBtnChang_Click" />
                                <asp:ImageButton ID="ITImageBtnCancel" runat="server" ImageUrl="~/Images/Cancel.gif" OnClick="ITImageBtnCancel_Click" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:ImageButton ID="ImageBtnEdit" runat="server" ImageUrl="Images/Edit.gif" Style="width: 21px" OnClick="ImageBtnEdit_Click" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="刪除">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImageBtnDelete" runat="server" ImageUrl="Images/delete.gif" CommandName="Stop" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="收文文號" DataField="BarcodeValue" SortExpression="BarcodeValue" ReadOnly="True" />
                        <asp:BoundField HeaderText="發文文號" DataField="WpoutNo" SortExpression="WpoutNo" ReadOnly="True" />
                        <asp:TemplateField HeaderText="歸檔檔號" SortExpression="FileNo">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtFileNo" runat="server" Text='<%# Bind("FileNo") %>' Width="100%" TextMode="Number" MaxLength="10"></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblFileNo" runat="server" Text='<%# Eval("FileNo") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="歸檔日期" SortExpression="FileDate">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtFileDate" runat="server" Text='<%# Bind("FileDate", "{0:yyyyMMdd}") %>' Width="100%" MaxLength="8" TextMode="Number"></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblFileDate" runat="server" Text='<%# Eval("FileDate", "{0:yyyyMMdd}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="保存年限" SortExpression="KeepYr">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtKeepYr" runat="server" Text='<%# Bind("KeepYr") %>' Width="100%" TextMode="Number"></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblKeepYr" runat="server" Text='<%# Eval("KeepYr") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="卷宗號" SortExpression="BoxNo">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtBoxNo" runat="server" Text='<%# Bind("BoxNo") %>' Width="100%" TextMode="Number"></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblBoxNo" runat="server" Text='<%# Eval("BoxNo") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="歸檔作業者" DataField="onfile" SortExpression="onfile" ReadOnly="True" />
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
