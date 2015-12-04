<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DulyAdjustedQuery.aspx.cs" Inherits="WDA.DulyAdjustedQuery" %>

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
            $("#MainContent_txtWpindate").datepicker("option", $.datepicker.regional["zh-TW"]);
            $("#MainContent_txtWpoutdate").datepicker("option", $.datepicker.regional["zh-TW"]);
            $("#MainContent_txtTranst").datepicker("option", $.datepicker.regional["zh-TW"]);
            $("#MainContent_txtGetime").datepicker("option", $.datepicker.regional["zh-TW"]);

            $("#MainContent_txtWpindate").datepicker({
                defaultDate: new Date(),
                changeMonth: true,
                numberOfMonths: 1
            });
            $("#MainContent_txtWpoutdate").datepicker({
                defaultDate: new Date(),
                changeMonth: true,
                numberOfMonths: 1
            });
            $("#MainContent_txtTranst").datepicker({
                defaultDate: new Date(),
                changeMonth: true,
                numberOfMonths: 1
            });
            $("#MainContent_txtGetime").datepicker({
                defaultDate: new Date(),
                changeMonth: true,
                numberOfMonths: 1
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
                   <h3 class="panel-title">調妥作業─查詢</h3>
               </div>
               <div class="panel-body">
                   <table class="ItemTD_green" style="width: 98%; float: right; border-collapse: separate; border-spacing: 1px;" border="1">
                       <tr>
                           <td class="HeadTD_green" style="padding: 5px;">收文文號：</td>
                           <td style="padding: 5px; text-align: left;">
                               <asp:TextBox ID="txtWpinno" runat="server"></asp:TextBox>
                           </td>
                       </tr>
                       <tr>
                           <td class="HeadTD_green" style="padding: 5px;">收文日期：</td>
                           <td style="padding: 5px; text-align: left;">
                               <asp:TextBox ID="txtWpindate" runat="server"></asp:TextBox>
                           </td>
                       </tr>
                       <tr>
                           <td class="HeadTD_green" style="padding: 5px;">發文文號：</td>
                           <td style="padding: 5px; text-align: left;">
                               <asp:TextBox ID="txtWpoutNo" runat="server" TextMode="Number"></asp:TextBox>
                           </td>
                       </tr>
                       <tr>
                           <td class="HeadTD_green" style="padding: 5px;">發文日期：</td>
                           <td style="padding: 5px; text-align: left;">
                               <asp:TextBox ID="txtWpoutdate" runat="server"></asp:TextBox>
                           </td>
                       </tr>
                    <tr>
                        <td class="HeadTD_green" style="padding: 5px;">案號：</td>
                        <td style="padding: 5px; text-align: left;">
                            <asp:TextBox ID="txtCaseno" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="HeadTD_green" style="padding: 5px;">案 件 別：</td>
                        <td style="padding: 5px; text-align: left;">
                            <asp:DropDownList ID="ddlApplkind" runat="server">
                                <asp:ListItem Value="0">請選擇</asp:ListItem>
                                <asp:ListItem Value="1">一般</asp:ListItem>
                                <asp:ListItem Value="2">法制</asp:ListItem>
                                <asp:ListItem Value="3">行政</asp:ListItem>
                            </asp:DropDownList>
                          <%--  <asp:TextBox ID="txtApplkind" runat="server"></asp:TextBox>--%>
                        </td>
                    </tr>
                    <tr>
                        <td class="HeadTD_green" style="padding: 5px;">雇主名稱：</td>
                        <td style="padding: 5px; text-align: left;">
                            <asp:TextBox ID="txtCommname" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="HeadTD_green" style="padding: 5px;">預 約 人：</td>
                        <td style="padding: 5px; text-align: left;">
                            <%--<asp:TextBox ID="txtReceiver" runat="server"></asp:TextBox>--%>
                            <asp:Label ID="lblReceiver" runat="server" Text="Label"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="HeadTD_green" style="padding: 5px;">預約時間：</td>
                        <td style="padding: 5px; text-align: left;">
                            <asp:TextBox ID="txtTranst" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                       <tr>
                        <td class="HeadTD_green" style="padding: 5px;">登錄時間：</td>
                        <td style="padding: 5px; text-align: left;">
                            <asp:TextBox ID="txtGetime" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                       <tr>
                        <td class="HeadTD_green" style="padding: 5px;">登 錄 人：</td>
                        <td style="padding: 5px; text-align: left;">
                            <asp:TextBox ID="txtWorkerid" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                   </table>
               </div>
               <table border="0" style="width: 100%; border-collapse: collapse">
                   <tr style="text-align: center">
                       <td style="text-align: right">
                           <asp:Button ID="BtnOK" runat="server" Text="確 定" class="btn btn-large btn-success" OnClick="BtnOK_Click"/>
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
                       <h3 class="panel-title">調妥查詢─詳細列表如下：</h3>
                   </div>
                   <div class="panel-body">
                    <asp:GridView ID="GridView1" runat="server" AllowSorting="True" AutoGenerateColumns="False" CssClass="GridViewStyle" Width="98%" OnSorting="GridView1_Sorting" AllowPaging="True" OnPageIndexChanging="GridView1_PageIndexChanging" OnRowDataBound="GridView1_RowDataBound" OnRowCreated="GridView1_RowCreated" style="width:inherit">
                       <AlternatingRowStyle CssClass="AlternatingRowStyle" />
                       <Columns>
                            <asp:TemplateField HeaderText="調閱">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgBtnProduction" runat="server" ImageUrl="~/Images/view.gif"  OnClick="ImgBtnProduction_Click" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CASEID" HeaderText="案件編號" SortExpression="CASEID" />
                           <asp:BoundField DataField="wpinno" HeaderText="收文文號" SortExpression="wpinno" />
                           <asp:BoundField HeaderText="收文日期" DataField="wpindate" SortExpression="wpindate" />
                           <asp:BoundField HeaderText="發文文號" DataField="wpoutno" SortExpression="wpoutno" />
                           <asp:BoundField HeaderText="發文日期" DataField="wpoutdate" SortExpression="wpoutdate" />
                            <asp:BoundField DataField="caseno" HeaderText="案號" SortExpression="caseno" />
                            <asp:BoundField DataField="kindName" HeaderText="案件別" SortExpression="kindName" />
                            <asp:BoundField DataField="commname" HeaderText="雇主名稱" SortExpression="commname" />
                            <asp:BoundField DataField="receiver" HeaderText="預約人" SortExpression="receiver" />
                            <asp:BoundField DataField="transt" HeaderText="預約時間" SortExpression="transt" />
                            <asp:BoundField DataField="getime" HeaderText="登錄時間" SortExpression="getime" />
                            <asp:BoundField DataField="workerid" HeaderText="登錄人" SortExpression="workerid" />
                            <asp:BoundField DataField="ViewType" HeaderText="調閱類型" SortExpression="ViewType" />
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
           </div>
           <cc1:LiteralMessageBox ID="LiteralMessageBox1" runat="server"></cc1:LiteralMessageBox>
    <input id="HiddenMessage" type="Hidden" runat="server" />
           <input id="HiddenShowPanel" type="Hidden" runat="server" />
           </div>
</asp:Content>
