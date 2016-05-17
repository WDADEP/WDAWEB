<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FileQuery.aspx.cs" Inherits="WDA.FileQuery" %>

<%@ Register Assembly="WDA" Namespace="WDA" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function pageLoad() {
            showMessage();
        }

        function ImageBtnOKClick() {
            try {
                var txtWPINNO = $get('MainContent_TxtWPINNO').value;

                if (txtWPINNO.length == 0) {
                    alert('收文號');
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
                   <h3 class="panel-title">檔案借閱─查詢</h3>
               </div>
               <div class="panel-body">
                   <table class="ItemTD_green" style="width: 98%; float: right; border-collapse: separate; border-spacing: 1px;" border="1">
                       <tr>
                           <td class="HeadTD_green" style="padding: 5px;">收文號：
                           </td>
                           <td style="padding: 5px; text-align: left;">
                               <asp:TextBox ID="TxtWPINNO" runat="server"></asp:TextBox>
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
                       <h3 class="panel-title">檔案借閱查詢─詳細列表如下：</h3>
                   </div>
                   <div class="panel-body">
                               <h5 class="panel-title">基本資料：</h5>
                       <div style="width: 98%">
                           <asp:GridView ID="GridView1" runat="server" CssClass="GridViewStyle" Width="98%" AutoGenerateColumns="False">
                               <AlternatingRowStyle CssClass="AlternatingRowStyle" />
                               <Columns>
                                   <asp:BoundField HeaderText="收文號" DataField="WPINNO"></asp:BoundField>
                                   <asp:BoundField HeaderText="聯絡人姓名" DataField="COMMNAME"></asp:BoundField>
                                   <asp:BoundField HeaderText="聯絡人地址" DataField="COMMADD" />
                                   <asp:BoundField HeaderText="發文日期" DataField="WPOUTDATE" />
                                   <asp:BoundField HeaderText="發文人員" DataField="USERNAME" />
                                   <asp:BoundField HeaderText="卷宗號" DataField="BOXNO" />
                                   <asp:BoundField HeaderText="歸檔檔號" DataField="FILENO" />
                                   <asp:BoundField HeaderText="歸檔作業者" DataField="OnFile" />
                               </Columns>
                               <FooterStyle CssClass="FooterStyle" />
                               <HeaderStyle CssClass="HeaderStyle" />
                               <PagerStyle CssClass="PagerStyle" HorizontalAlign="Center" />
                               <RowStyle CssClass="RowStyle" />
                           </asp:GridView>
                       </div>
                   </div>
                   <div class="panel-body">

                        <h5 class="panel-title">檔案狀況：</h5>
                       <div style="width: 98%">

                    <asp:GridView ID="GridView2" runat="server" AllowSorting="True" AutoGenerateColumns="False" CssClass="GridViewStyle" Width="98%" OnSorting="GridView2_Sorting" AllowPaging="True" OnPageIndexChanging="GridView2_PageIndexChanging">
                       <AlternatingRowStyle CssClass="AlternatingRowStyle" />
                       <Columns>
                           <asp:BoundField HeaderText="序號" DataField="RID" SortExpression="RID"></asp:BoundField>
                           <asp:BoundField HeaderText="借檔人" DataField="REALNAME1" SortExpression="REALNAME1"></asp:BoundField>
                           <asp:BoundField HeaderText="借檔分機" DataField="TEL" SortExpression="TEL"></asp:BoundField>
                           <asp:BoundField HeaderText="借檔日期" DataField="TRANST" SortExpression="TRANST" />
                           <asp:BoundField HeaderText="簽核日期" DataField="APPROVEDATE" SortExpression="APPROVEDATE" />
                           <asp:BoundField HeaderText="審核長官" DataField="REALNAME3" SortExpression="REALNAME3" />
                           <asp:BoundField HeaderText="調妥日期" DataField="GETIME" SortExpression="GETIME" />
                           <asp:BoundField HeaderText="調妥人員" DataField="WORKERID" SortExpression="WORKERID" />
                           <asp:BoundField HeaderText="還檔日期" DataField="REDATE" SortExpression="REDATE" />
                           <asp:BoundField HeaderText="還檔人員" DataField="REALNAME2" SortExpression="REALNAME2" />
                       </Columns>
                       <FooterStyle CssClass="FooterStyle" />
                       <HeaderStyle CssClass="HeaderStyle" />
                       <PagerStyle CssClass="PagerStyle" HorizontalAlign="Center" />
                       <RowStyle CssClass="RowStyle" />
                       <PagerSettings Mode="NumericFirstLast" FirstPageText="[第一頁]" LastPageText="[最末頁]" />
                   </asp:GridView>
			</div>
                   </div>
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
