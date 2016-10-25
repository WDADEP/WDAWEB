<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FileArchiveStatisticsReport.aspx.cs" Inherits="WDA.FileArchiveStatisticsReport" %>

<%@ Register Assembly="WDA" Namespace="WDA" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function pageLoad() {
            $('#success-alert').on('closed.bs.alert', function () {
                $get('MainContent_HiddenShowAlert').value = true;
            })

            if ($("#MainContent_HiddenShowAlert").val() == "true") { $("#success-alert").alert('close'); }
            else { $("#success-alert").alert(); }

            if ($("#MainContent_HiddenShowPanel").val() == "true") {
                $('#divPanel').show();
            }
            else {
                $('#divPanel').hide();
            }

            $("#MainContent_txtFileScanStartDate").datepicker("option", $.datepicker.regional["zh-TW"]);
            $("#MainContent_txtFileScanEndDate").datepicker("option", $.datepicker.regional["zh-TW"]);

            $("#MainContent_txtFileScanStartDate").datepicker({
                defaultDate: new Date(),
                changeMonth: true,
                numberOfMonths: 1,
                onClose: function (selectedDate) {
                    $("#MainContent_txtFileScanEndDate").datepicker("option", "minDate", selectedDate);
                }
            });
            $("#MainContent_txtFileScanEndDate").datepicker({
                defaultDate: new Date(),
                changeMonth: true,
                numberOfMonths: 1,
                onClose: function (selectedDate) {
                    $("#MainContent_txtFileScanStartDate").datepicker("option", "maxDate", selectedDate);
                }
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
                var txtFileNo = $get('MainContent_txtFileNo').value;
                var txtFileScanStartDate = $get('MainContent_txtFileScanStartDate').value;
                var txtFileScanEndDate = $get('MainContent_txtFileScanEndDate').value;
                var txtOnFile = $get('MainContent_ddlOnFile').value;

                if (txtFileNo.length == 0 &&
                    txtFileScanStartDate.length == 0 &&
                    txtFileScanEndDate.length == 0 &&
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
    <div class="well well-lg">
        <div class="panel panel-primary">
            <div class="panel-heading">
                <h3 class="panel-title">歸檔作業─統計</h3>
            </div>
            <div class="panel-body">
                <table class="ItemTD_green" style="width: 98%; float: right; border-collapse: separate; border-spacing: 1px;" border="1">
                    <tr>
                        <td class="HeadTD_green" style="padding: 5px;">歸檔作業者：</td>
                        <td style="padding: 5px; text-align: left;">
                                <asp:DropDownList ID="ddlOnFile" runat="server">
                                </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="HeadTD_green" style="padding: 5px;">歸檔起訖日：</td>
                        <td style="padding: 5px; text-align: left;">
                            起：<asp:TextBox ID="txtFileScanStartDate"  runat="server"></asp:TextBox>
                            ～訖：<asp:TextBox ID="txtFileScanEndDate"  runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="HeadTD_green" style="padding: 5px;">歸檔檔號：</td>
                        <td style="padding: 5px; text-align: left;">
                            <asp:TextBox ID="txtFileNo" runat="server" TextMode="Number"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="HeadTD_green" style="padding: 5px">有無掃描：</td>
                        <td style="padding: 5px; text-align: left;">
                            <asp:RadioButtonList ID="radScanFile" runat="server">
                                <asp:ListItem Value="0">無</asp:ListItem>
                                <asp:ListItem Value="1">有</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                </table>
            </div>
            <table border="0" style="width: 100%; border-collapse: collapse">
                <tr style="text-align: center">
                    <td style="text-align: center">
                        <asp:Button ID="BtnOK" runat="server" Text="確 定" class="btn btn-large btn-success" OnClientClick="JavaScript:if(!ImageBtnOKClick()) {return false} ;" OnClick="BtnOK_Click"/>
                    </td>
                </tr>
            </table>
        </div>
        <div id="divPanel">
            <br>
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <h3 class="panel-title">歸檔作業統計─詳細列表如下：</h3>
                </div>
                <div class="panel-body">
                    <div style="width: 98%; text-align: right">
                        <%--<asp:Button ID="BtnDetailPrint" runat="server" Text="匯出細項資料" class="btn btn-large btn-success" OnClientClick="JavaScript:if(!ImageBtnPringClick()) {return false} ;" PostBackUrl="~/FileArchiveDetailPrint.aspx" />--%>
                        <asp:Button ID="BtnPrint" runat="server" Text="列 印" class="btn btn-large btn-success" OnClick="BtnPrint_Click" OnClientClick="JavaScript:if(!ImageBtnPringClick()) {return false} ;" PostBackUrl="~/FileArchiveGroupPrint.aspx" />
                    </div>
                </div>
                <asp:GridView ID="GridView1" runat="server" AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False" CssClass="GridViewStyle" Width="98%" OnPageIndexChanging="GridView1_PageIndexChanging" OnSorting="GridView1_Sorting" OnRowDataBound="GridView1_RowDataBound" >
                    <AlternatingRowStyle CssClass="AlternatingRowStyle" />
                    <Columns>
                        <asp:BoundField HeaderText="編號" DataField="RID" SortExpression="RID" ReadOnly="True" />
                        <asp:BoundField HeaderText="歸檔日期" DataField="TRANST" SortExpression="TRANST" ReadOnly="True" />
                        <asp:BoundField HeaderText="歸檔作業者" DataField="RECEIVER" SortExpression="RECEIVER" ReadOnly="True" />
                        <asp:BoundField HeaderText="檔號" DataField="FILENO" SortExpression="FILENO" ReadOnly="True" />
                        <asp:BoundField HeaderText="有無掃描" DataField="ISFILE" SortExpression="ISFILE" ReadOnly="True" />
                        <asp:BoundField HeaderText="數量" DataField="FILECOUNT" SortExpression="FILECOUNT" ReadOnly="True" />
                        <asp:TemplateField HeaderText="匯出細項資料">
                            <ItemTemplate>
                                <asp:Button ID="BtnDetailPrint" runat="server" Text="匯出細項資料" class="btn btn-default btn-xs" OnClick="BtnDetailPrint_Click" PostBackUrl="~/FileArchiveDetailPrint.aspx" OnClientClick="JavaScript:if(!ImageBtnPringClick()) {return false} ;"/>
                            </ItemTemplate>
                        </asp:TemplateField>
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
        <input id="HiddenShowAlert" type="Hidden" runat="server" />
    </div>
</asp:Content>
