﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CancelBorrowStatisticsReport.aspx.cs" Inherits="WDA.CancelBorrowStatisticsReport" %>
<%@ Register Assembly="WDA" Namespace="WDA" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function pageLoad() {
            $("#MainContent_txtBorrowCreateTime").datepicker("option", $.datepicker.regional["zh-TW"]);
            $("#MainContent_txtBorrowEndTime").datepicker("option", $.datepicker.regional["zh-TW"]);

            $("#MainContent_txtBorrowCreateTime").datepicker({
                defaultDate: new Date(),
                changeMonth: true,
                numberOfMonths: 1,
                onClose: function (selectedDate) {
                    $("#MainContent_txtBorrowEndTime").datepicker("option", "minDate", selectedDate);
                }
            });
            $("#MainContent_txtBorrowEndTime").datepicker({
                defaultDate: new Date(),
                changeMonth: true,
                numberOfMonths: 1,
                onClose: function (selectedDate) {
                    $("#MainContent_txtBorrowCreateTime").datepicker("option", "maxDate", selectedDate);
                }
            });

            if ($("#MainContent_HiddenShowPanel").val() == "true") {
                $('#divPanel').show();
            }
            else {
                $('#divPanel').hide();
            }
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
                var txtScanCreateTime = $get('MainContent_txtBorrowCreateTime').value;

                var dReceiver = $get('MainContent_ddlReceiver').value;

                if (txtScanCreateTime.length == 0 && dReceiver == 0) {
                    alert('請輸入任一個查詢條件');
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
                <h3 class="panel-title">取消借調作業(統計)</h3>
            </div>
            <div class="panel-body">
                <table class="ItemTD_green" style="width: 98%; float: right; border-collapse: separate; border-spacing: 1px;" border="1">
                    <tr>
                        <td class="HeadTD_green" style="padding: 5px;">取消調檔作業者：
                        </td>
                        <td style="padding: 5px; text-align: left;">
                                <asp:DropDownList ID="ddlReceiver" runat="server">
                                </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="HeadTD_green" style="padding: 5px;">借檔日期起訖：
                        </td>
                           <td style="padding: 5px; text-align: left;">起：
     <asp:TextBox ID="txtBorrowCreateTime" runat="server"  title="日期格式"></asp:TextBox>
                                ～訖：
                                            <asp:TextBox ID="txtBorrowEndTime" runat="server"  title="日期格式"></asp:TextBox>
                           </td>
                    </tr>
                </table>
            </div>
            <table border="0" style="width: 100%; border-collapse: collapse">
                <tr style="text-align: center">
                    <td style="text-align: right">
                        <asp:Button ID="BtnOK" runat="server" Text="確 定" class="btn btn-large btn-success" OnClientClick="JavaScript:if(!ImageBtnOKClick()) {return false} ;" OnClick="BtnOK_Click" />
                    </td>
                    <td style="text-align: left">
                        <asp:Button ID="BtnClear" runat="server" Text="取 消" class="btn btn-large btn-success" OnClick="BtnClear_Click"  />
                    </td>
                </tr>
            </table>
        </div>
        <div id="divPanel">
            <br>
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <h3 class="panel-title">取消借調作業─統計列表如下：</h3>
                </div>
                <div class="panel-body">
                    <div style="width: 98%; text-align: right">
                        <asp:Button ID="BtnPrint" runat="server" Text="列 印" class="btn btn-large btn-success" OnClientClick="var scriptName=document.forms[0].action;window.document.forms[0].target='_blank';setTimeout(function(){window.document.forms[0].target='';document.forms[0].action=scriptName;}, 500);" PostBackUrl="~/CancelBorrowGroupPrint.aspx" />
                    </div>
                </div>
                <asp:GridView ID="GridView1" runat="server" AllowSorting="True"  AutoGenerateColumns="False" CssClass="GridViewStyle" Width="98%" OnSorting="GridView1_Sorting" AllowPaging="True" OnPageIndexChanging="GridView1_PageIndexChanging"  >
                    <AlternatingRowStyle CssClass="AlternatingRowStyle" />
                    <Columns>
                        <asp:BoundField HeaderText="序號" DataField="RID" SortExpression="RID" ReadOnly="True" />
                        <asp:BoundField HeaderText="借檔日期" DataField="TRANST" SortExpression="TRANST" ReadOnly="True" />
                        <asp:BoundField HeaderText="借檔作業者" DataField="WORKERID" SortExpression="WORKERID" ReadOnly="True" />
                        <asp:BoundField HeaderText="檔號" DataField="FILENO" SortExpression="FILENO" ReadOnly="True" />
                        <asp:BoundField HeaderText="申請科室"　DataField="DeptName" SortExpression="DeptName" ReadOnly="true" />
                        <asp:BoundField HeaderText="數量" DataField="FILECOUNT" SortExpression="FILECOUNT" ReadOnly="True" />
                        <asp:TemplateField HeaderText="匯出細項資料">
                            <ItemTemplate>
                                <asp:Button ID="BtnDetailPrint" runat="server" Text="匯出細項資料" class="btn btn-default btn-xs" OnClick="BtnDetailPrint_Click" PostBackUrl="~/CancelBorrowDetailPrint.aspx" OnClientClick="JavaScript:if(!ImageBtnPringClick()) {return false} ;"/>
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

    </div>
</asp:Content>
