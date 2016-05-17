<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LogQueryMenu.aspx.cs" Inherits="WDA.LogQueryMenu" %>

<%@ Register Assembly="WDA" Namespace="WDA" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
        <script type="text/javascript">
            function pageLoad() {
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
            showMessage();
            if ($("#MainContent_HiddenShowPanel").val() == "true") {
                $('#divPanel').show();
            }
            else {
                $('#divPanel').hide();
            }
        }
    </script>
    <div class="alert alert-info">
        <button type="button" class="close" data-dismiss="alert">&times;</button>
        <h4>說明：</h4>
        <ol>
            <li>設定「查詢條件」，完成後請按【確定】鍵執行查詢動作。</li>
            <li>您可設定一個以上的查詢條件，以取得更精確的查詢結果。</li>
        </ol>
    </div>
    <div class="well well-lg">
        <div class="panel panel-primary">
            <div class="panel-heading">
                <h3 class="panel-title">交易紀錄管理─查詢條件設定：</h3>
            </div>
            <div class="panel-body">
                <div style="width: 98%; text-align: right">
                    <asp:Button ID="BtnClearLog" runat="server" Text="清除紀錄" Class="btn btn-success"/>
                </div>
                <table class="ItemTD_green" style="width: 98%; float: right; border-collapse: separate; border-spacing: 1px;" border="1">
                        <tr>
                            <td class="HeadTD_green" style="padding: 5px;">
                                <span class="t15_red">＊</span> 選擇交易時間：
                            </td>
                            <td style="padding: 5px; text-align: left;">開始日期：
                                        <asp:TextBox ID="txtCreateTime" runat="server"></asp:TextBox>
                                ～結束日期：
                                        <asp:TextBox ID="txtEndTime" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="HeadTD_green" style="padding: 5px;">發文文號：
                            </td>
                            <td style="padding: 5px; text-align: left;">
                                <asp:TextBox ID="txtWPINNO" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="HeadTD_green" style="padding: 5px;">帳號：
                            </td>
                            <td style="padding: 5px; text-align: left;">
                                <asp:TextBox ID="txtUserName" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="display:none">
                            <td class="HeadTD_green" style="padding: 5px;">系統行為
                            </td>
                            <td style="padding: 5px; text-align: left; background-color: #EEEEEE;">
                                <div id="divSystemOperatingPrivilege">
                                    <asp:Literal ID="LiteralSystemOperatingMenu" runat="server"></asp:Literal>
                                    <script src="Scripts/TreeView/js/jquery.tree.js"></script>
                                    <link rel="stylesheet" href="Scripts/TreeView/css/jquery.tree.css" />
                                    <script>
                                        $('#TreeSystemOperating').tree({
                                        });
                                    </script>
                                </div>
                                <input id="HiddenSystemOperatingPrivilegeList" runat="server" type="hidden" />
                            </td>
                        </tr>
                    </table>
            </div>
            <table border="0" style="width: 100%; border-collapse: collapse">
                <tr style="text-align: center">
                    <td style="text-align: right">
                        <asp:Button ID="BtnOK" runat="server" Text="確 定" class="btn btn-large btn-success" OnClick="BtnOK_Click" />
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
                    <h3 class="panel-title">交易紀錄管理─詳細列表如下：</h3>
                </div>
                <div class="panel-body">
                    <asp:GridView ID="GridView1" runat="server" AllowSorting="True" AutoGenerateColumns="False" CssClass="GridViewStyle" Width="98%" OnPageIndexChanging="GridView1_PageIndexChanging" OnRowDataBound="GridView1_RowDataBound" OnSorting="GridView1_Sorting" AllowPaging="True">
                                    <AlternatingRowStyle CssClass="AlternatingRowStyle" />
                                    <Columns>
                                        <asp:BoundField HeaderText="收文文號" DataField="WPINNO" SortExpression="WPINNO" />
                                        <asp:BoundField HeaderText="使用者帳號" DataField="USERNAME" SortExpression="USERNAME" />
                                        <asp:BoundField HeaderText="使用者姓名" DataField="RealName" SortExpression="RealName" />
                                        <asp:BoundField DataField="TransDateTime" HeaderText="交易日期" SortExpression="TransDateTime" />
                                        <asp:BoundField DataField="TransIP" HeaderText="交易IP" SortExpression="TransIP" />
                                        <asp:BoundField HeaderText="系統行為" DataField="MsgText" SortExpression="MsgText" />
                                        <asp:BoundField DataField="Comments" HeaderText="備註" SortExpression="Comments" />
                                    </Columns>
                                    <FooterStyle CssClass="FooterStyle" />
                                    <HeaderStyle CssClass="HeaderStyle" />
                                    <PagerStyle CssClass="PagerStyle" HorizontalAlign="Center" />
                                    <RowStyle CssClass="RowStyle" />
                                </asp:GridView>
                                <table id="tbPageLayer_GridView1" style="border-right: #ffffff 1px solid;border-top: #ffffff 1px solid;border-left: #e6e6e6 1px solid;border-bottom: #e6e6e6 1px solid;width: 100%;" border="1">
                                    <tr>
                                          <td style="padding: 0; float: none; background-color: #ffffff;text-align:right ;" class="t12_blue">
                                            <asp:Label ID="lblTotalPage_GridView1" runat="server"></asp:Label>
                                            <asp:Label ID="lblPage_GridView1" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                </div>
            </div>
        </div>
        <cc1:LiteralMessageBox ID="LiteralMessageBox1" runat="server"></cc1:LiteralMessageBox>
        <input id="HiddenMessage" type="Hidden" runat="server" />
        <input id="HiddenShowPanel" type="Hidden" runat="server" />
    </div>
</asp:Content>
