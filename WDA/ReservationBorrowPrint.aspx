<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReservationBorrowPrint.aspx.cs" Inherits="WDA.ReservationBorrowPrint" %>

<%@ Register Assembly="WDA" Namespace="WDA" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        var isPrint = false;

        function pageLoad() {
            $("#MainContent_txtScanCreateTime").datepicker("option", $.datepicker.regional["zh-TW"]);
            $("#MainContent_txtScanEndTime").datepicker("option", $.datepicker.regional["zh-TW"]);

            $("#MainContent_txtScanCreateTime").datepicker({
                defaultDate: new Date(),
                changeMonth: true,
                numberOfMonths: 1,
                onClose: function (selectedDate) {
                    $("#MainContent_txtScanEndTime").datepicker("option", "minDate", selectedDate);
                }
            });
            $("#MainContent_txtScanEndTime").datepicker({
                defaultDate: new Date(),
                changeMonth: true,
                numberOfMonths: 1,
                onClose: function (selectedDate) {
                    $("#MainContent_txtScanCreateTime").datepicker("option", "maxDate", selectedDate);
                }
            });
            showMessage();
        }

        function ImageBtnOKClick() {

            try {
                var scriptName = document.forms[0].action;
                window.document.forms[0].target = '_blank';
                setTimeout(function () {
                    window.document.forms[0].target = '';
                    document.forms[0].action = scriptName;
                }, 500);
                return true;
            }
            catch (e) {
                alert(e.message);
                Sys.Debug.traceDump(e);
            };
        }
    </script>
    <div class="well well-lg">
        <div id="divPanel" runat="server">
            <br>
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <h3 class="panel-title">預約借檔─列印：</h3>
                </div>
                <div class="panel-body">
                    <table class="ItemTD_green" style="width: 98%; float: right; border-collapse: separate; border-spacing: 1px;" border="1">
                        <tr>
                            <td class="HeadTD_green" style="padding: 5px;">
                                <span class="t15_red">＊</span>請選擇列印時間：
                            </td>
                            <td style="padding: 5px; text-align: left;">起：
                                              <asp:TextBox ID="txtScanCreateTime" runat="server" pattern="\d{4}/\d{1,2}/\d{1,2}" title="日期格式"></asp:TextBox>
                                <asp:DropDownList ID="ddlCreateTime" Style="width: 150px" runat="server">
                                    <asp:ListItem Value="1">1. AM 09:30</asp:ListItem>
                                    <asp:ListItem Value="2">2. AM 11:00</asp:ListItem>
                                    <asp:ListItem Value="3">3. PM 02:30</asp:ListItem>
                                    <asp:ListItem Value="4">4. PM 04:00</asp:ListItem>
                                </asp:DropDownList>
                                ～迄：
                                            <asp:TextBox ID="txtScanEndTime" runat="server" pattern="\d{4}/\d{1,2}/\d{1,2}" title="日期格式"></asp:TextBox>
                                <asp:DropDownList ID="ddlEndTime" Style="width: 150px" runat="server">
                                    <asp:ListItem Value="1">1. AM 09:30</asp:ListItem>
                                    <asp:ListItem Value="2">2. AM 11:00</asp:ListItem>
                                    <asp:ListItem Value="3">3. PM 02:30</asp:ListItem>
                                    <asp:ListItem Value="4">4. PM 04:00</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <%--     <td style="padding: 5px; text-align: left;">
                                <asp:DropDownList ID="ddlBorrowType" Style="width: 150px" runat="server">
                                    <asp:ListItem Value="1">1. AM 09:30</asp:ListItem>
                                    <asp:ListItem Value="2">2. AM 11:00</asp:ListItem>
                                    <asp:ListItem Value="3">3. PM 02:30</asp:ListItem>
                                    <asp:ListItem Value="4">4. PM 04:00</asp:ListItem>
                                </asp:DropDownList>
                            </td>--%>
                        </tr>
                    </table>
                </div>
                <table border="0" style="width: 100%; border-collapse: collapse">
                    <tr style="text-align: center">
                        <td style="text-align: center">
                            <asp:Button ID="BtnPrint" runat="server" Text="列 印" class="btn btn-large btn-success" OnClick="BtnPrint_Click" OnClientClick="JavaScript:if(!ImageBtnOKClick()) {return false} ;" PostBackUrl="~/ReservationBorrowReport.aspx" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <cc1:LiteralMessageBox ID="LiteralMessageBox1" runat="server"></cc1:LiteralMessageBox>
        <input id="HiddenMessage" type="Hidden" runat="server" />
    </div>
</asp:Content>
