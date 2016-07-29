<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TransQuery.aspx.cs" Inherits="WDA.TransQuery" %>
<%@ Register Assembly="WDA" Namespace="WDA" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function pageLoad() {
            $("#MainContent_txtScanCreateTime").datepicker("option", $.datepicker.regional["zh-TW"]);
            $("#MainContent_txtScanEndTime").datepicker("option", $.datepicker.regional["zh-TW"]);

            $("#MainContent_txtScanCreateTime").datetimepicker({
                defaultDate: new Date(),
                changeMonth: true,
                numberOfMonths: 1,
                onClose: function (selectedDate) {
                    $("#MainContent_txtScanEndTime").datetimepicker("option", "minDate", selectedDate);
                },
                hourText: "時",
                minuteText: "分",
                currentText: "現在時間",
                closeText: "確定"
            });
            $("#MainContent_txtScanEndTime").datetimepicker({
                defaultDate: new Date(),
                changeMonth: true,
                numberOfMonths: 1,
                onClose: function (selectedDate) {
                    $("#MainContent_txtScanCreateTime").datetimepicker("option", "maxDate", selectedDate);
                },
                hourText: "時",
                minuteText: "分",
                currentText: "現在時間",
                closeText: "確定"
            });

            if ($("#MainContent_HiddenShowPanel").val() == "true") {
                $('#divPanel').show();
            }
            else {
                $('#divPanel').hide();
            }
            showMessage();
        }

        function ImageBtnOKClick() {
            try {
                var txtWPINNO = $get('MainContent_TxtWpinno').value;
                var txtReceiver = $get('MainContent_TxtReceiver').value;
                var txtScanCreateTime = $get('MainContent_txtScanCreateTime').value;

                if (txtWPINNO.length == 0 && txtReceiver.length == 0 && txtScanCreateTime.length== 0) {
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
                <h3 class="panel-title">簽收功能(查詢)</h3>
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
                            <asp:TextBox ID="TxtReceiver" runat="server" ></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="HeadTD_green" style="padding: 5px;">收文日期起訖：
                        </td>
                           <td style="padding: 5px; text-align: left;">起：
     <asp:TextBox ID="txtScanCreateTime" runat="server" pattern="(\d{4})/(\d{1,2})/(\d{1,2}) (\d{2}):(\d{2})" title="日期格式"></asp:TextBox>
                                ～訖：
                                            <asp:TextBox ID="txtScanEndTime" runat="server" pattern="(\d{4})/(\d{1,2})/(\d{1,2}) (\d{2}):(\d{2})" title="日期格式"></asp:TextBox>
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
                        <asp:Button ID="BtnClear" runat="server" Text="取 消" class="btn btn-large btn-success"  />
                    </td>
                </tr>
            </table>
        </div>
        <div id="divPanel">
            <br>
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <h3 class="panel-title">簽收功能─查詢列表如下：</h3>
                </div>
                <div class="panel-body">
                    <div style="width: 98%; text-align: right">
                        <asp:Button ID="BtnPrint" runat="server" Text="列 印" class="btn btn-large btn-success" OnClientClick="var scriptName=document.forms[0].action;window.document.forms[0].target='_blank';setTimeout(function(){window.document.forms[0].target='';document.forms[0].action=scriptName;}, 500);" PostBackUrl="~/TransReceiverPrint.aspx" />
                    </div>
                </div>
                <asp:GridView ID="GridView1" runat="server" AllowSorting="True"  AutoGenerateColumns="False" CssClass="GridViewStyle" Width="98%" OnSorting="GridView1_Sorting" AllowPaging="True" OnPageIndexChanging="GridView1_PageIndexChanging" OnRowCommand="GridView1_RowCommand" OnRowDataBound="GridView1_RowDataBound">
                    <AlternatingRowStyle CssClass="AlternatingRowStyle" />
                    <Columns>
                        <asp:TemplateField HeaderText="刪除">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImageBtnDelete" runat="server" ImageUrl="Images/delete.gif" CommandName="Stop" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="序號" DataField="RID" SortExpression="RID" ReadOnly="True" />
                        <asp:BoundField HeaderText="收文文號" DataField="WPINNO" SortExpression="WPINNO" ReadOnly="True" />
                        <asp:BoundField HeaderText="雇主名稱" DataField="COMMNAME" SortExpression="COMMNAME" ReadOnly="True" />
                        <asp:BoundField HeaderText="收文日期" DataField="TRANSTIME" SortExpression="TRANSTIME" ReadOnly="True" />
                        <asp:BoundField HeaderText="接收文件者" DataField="RECEIVER" SortExpression="RECEIVER" ReadOnly="True" />
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
