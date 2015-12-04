<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ScanListQuery.aspx.cs" Inherits="WDA.ScanListQuery" %>

<%@ Register Assembly="WDA" Namespace="WDA" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
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
                var txtScanCreateTime = $get('MainContent_txtScanCreateTime').value;
                var txtScanEndTime = $get('MainContent_txtScanEndTime').value;

                if (txtScanCreateTime.length != 0 && txtScanEndTime.length == 0) {
                    $('#MainContent_txtScanEndTime').val($('#MainContent_txtScanCreateTime').val()); 
                }
                if (txtScanCreateTime.length == 0 && txtScanEndTime.length != 0) {
                    $('#MainContent_txtScanCreateTime').val($('#MainContent_txtScanEndTime').val()); 
                }

                if (txtScanCreateTime.length == 0)
                {
                    alert('請輸入掃描起訖日');
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
                   <h3 class="panel-title">掃描清單作業─查詢</h3>
               </div>
               <div class="panel-body">
                   <table class="ItemTD_green" style="width: 98%; float: right; border-collapse: separate; border-spacing: 1px;" border="1">
                       <tr>
                           <td class="HeadTD_green" style="padding: 5px;">掃瞄作業者：
                           </td>
                           <td style="padding: 5px; text-align: left;">
                               <asp:TextBox ID="TxtUserName" runat="server"></asp:TextBox>
                           </td>
                       </tr>
                       <tr style="padding: 5px;">
                           <td class="HeadTD_green" style="padding: 5px;">
                               <span class="t15_red">＊</span>掃描起訖日：
                           </td>
                           <td style="padding: 5px; text-align: left;">起：
                                              <asp:TextBox ID="txtScanCreateTime" runat="server" pattern="\d{4}/\d{1,2}/\d{1,2}" title="日期格式"></asp:TextBox>
                               ～迄：
                                            <asp:TextBox ID="txtScanEndTime" runat="server" pattern="\d{4}/\d{1,2}/\d{1,2}" title="日期格式"></asp:TextBox>
                           </td>
                       </tr>
                   </table>
               </div>
               <table border="0" style="width: 100%; border-collapse: collapse">
                   <tr style="text-align: center">
                       <td style="text-align: right">
                           <asp:Button ID="BtnOK" runat="server" Text="確 定" class="btn btn-large btn-success" OnClientClick="JavaScript:if(!ImageBtnOKClick()) {return false} ;" OnClick="BtnOK_Click"/>
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
                       <h3 class="panel-title">掃描清單查詢─詳細列表如下：</h3>
                   </div>
                   <div class="panel-body">
                       <div style="width: 98%; text-align: right">
                           <asp:Button ID="BtnAdd" runat="server" Text="列印" class="btn btn-large btn-success" OnClientClick="var scriptName=document.forms[0].action;window.document.forms[0].target='_blank';setTimeout(function(){window.document.forms[0].target='';document.forms[0].action=scriptName;}, 500);" PostBackUrl="~/ScanListReport.aspx" />
                       </div>
                   </div>
                    <asp:GridView ID="GridView1" runat="server" AllowSorting="True" AutoGenerateColumns="False" CssClass="GridViewStyle" Width="98%" OnSorting="GridView1_Sorting" AllowPaging="True" OnPageIndexChanging="GridView1_PageIndexChanging" OnRowDataBound="GridView1_RowDataBound">
                       <AlternatingRowStyle CssClass="AlternatingRowStyle" />
                       <Columns>
                           <asp:TemplateField HeaderText="案件編號" SortExpression="CASEID">
                               <ItemTemplate>
                                   <asp:Label ID="LblCaseID" runat="server" Text='<%# Eval("CASEID") %>'></asp:Label>
                               </ItemTemplate>
                           </asp:TemplateField>
                           <asp:BoundField DataField="BARCODEVALUE" HeaderText="收文文號" SortExpression="BARCODEVALUE" />
                           <asp:BoundField HeaderText="掃描日期" DataField="CREATETIME" SortExpression="CREATETIME" />
                           <asp:BoundField HeaderText="掃瞄作業者" DataField="RealName" SortExpression="RealName" />
                           <asp:BoundField HeaderText="掃描頁數" DataField="FileCount" SortExpression="FileCount" />
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
           </div>
</asp:Content>
