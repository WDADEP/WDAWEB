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

            $("#MainContent_txtFileScanStartDate").datetimepicker({
                defaultDate: new Date(),
                changeMonth: true,
                numberOfMonths: 1,
                onClose: function (selectedDate) {
                    $("#MainContent_txtFileScanEndDate").datetimepicker("option", "minDate", selectedDate);
                }
            });
            $("#MainContent_txtFileScanEndDate").datetimepicker({
                defaultDate: new Date(),
                changeMonth: true,
                numberOfMonths: 1,
                onClose: function (selectedDate) {
                    $("#MainContent_txtFileScanStartDate").datetimepicker("option", "maxDate", selectedDate);
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
                var txtBarcodeValue = $get('MainContent_txtBarcodeValue').value;
                var txtWpoutNo = $get('MainContent_txtWpoutNo').value;
                var txtFileNo = $get('MainContent_txtFileNo').value;
                var txtFileScanStartDate = $get('MainContent_txtFileScanStartDate').value;
                var txtFileScanEndDate = $get('MainContent_txtFileScanEndDate').value;
                var txtKeepYr = $get('MainContent_txtKeepYr').value;
                var txtBoxNoS = $get('MainContent_txtBoxNoS').value;
                var txtBoxNoE = $get('MainContent_txtBoxNoE').value;
                var txtOnFile = $get('MainContent_ddlOnFile').value;

                if (txtBarcodeValue.length == 0 &&
                    txtWpoutNo.length == 0 &&
                    txtFileNo.length == 0 &&
                    txtFileScanStartDate.length == 0 &&
                    txtFileScanEndDate.length == 0 &&
                    txtKeepYr.length == 0 &&
                    txtBoxNoS.length == 0 &&
                    txtBoxNoE.length == 0 &&
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
    <div class="alert alert-info" id="success-alert">
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
                <h3 class="panel-title">歸檔登入─統計</h3>
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
                        <td class="HeadTD_green" style="padding: 5px;">歸檔起訖日：</td>
                        <td style="padding: 5px; text-align: left;">
                            起：<asp:TextBox ID="txtFileScanStartDate" pattern="(\d{4})/(\d{1,2})/(\d{1,2}) " runat="server"></asp:TextBox>
                            ～訖：<asp:TextBox ID="txtFileScanEndDate" pattern="(\d{4})/(\d{1,2})/(\d{1,2}) " runat="server"></asp:TextBox>
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
                            <strong>起：
                                <asp:TextBox ID="txtBoxNoS" runat="server" TextMode="Number"></asp:TextBox>～訖：
										<asp:TextBox ID="txtBoxNoE" runat="server" TextMode="Number"></asp:TextBox></strong>
                        </td>
                    </tr>
                    <tr>
                        <td class="HeadTD_green" style="padding: 5px;">歸檔作業者：</td>
                        <td style="padding: 5px; text-align: left;">
                                <asp:DropDownList ID="ddlOnFile" runat="server">
                                </asp:DropDownList>
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
                    <h3 class="panel-title">歸檔登入統計─詳細列表如下：</h3>
                </div>
                <div class="panel-body">
                    <div style="width: 98%; text-align: right">
                        <asp:Button ID="BtnDetailPrint" runat="server" Text="匯出細項資料" class="btn btn-large btn-success" OnClientClick="JavaScript:if(!ImageBtnPringClick()) {return false} ;" PostBackUrl="~/FileArchiveDetailPrint.aspx" />
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
