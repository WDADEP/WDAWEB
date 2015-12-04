<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RoleQueryMenu.aspx.cs" Inherits="WDA.RoleQueryMenu" %>

<%@ Register Assembly="WDA" Namespace="WDA" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <script type="text/javascript">
         function pageLoad() {
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
        </ol>
    </div>
       <div class="well well-lg">
           <div class="panel panel-primary">
               <div class="panel-heading">
                   <h3 class="panel-title">掃描清單作業─查詢</h3>
               </div>
               <div class="panel-body">
                    <div style="width: 98%; text-align: right">
                        <asp:Button ID="BtnAdd" runat="server" Text="新增角色" Class="btn btn-success" OnClick="BtnAdd_Click" />
                    </div>
                    <table class="ItemTD_green" style="width: 98%; float: right; border-collapse: separate; border-spacing: 1px;" border="1">
                        <tr>
                            <td class="HeadTD_green" style="width: 20px">
                                <span class="t15_red">＊</span>請輸入角色名稱：</td>
                            <td style="padding: 5px; text-align: left;">
                                <asp:DropDownList ID="DDLRoleName" runat="server"></asp:DropDownList>
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
                       <h3 class="panel-title">角色管理─詳細列表如下：</h3>
                   </div>
                   <div class="panel-body">
                       <div style="width: 98%; text-align: right">
                       </div>
                   </div>
                      <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="GridViewStyle" Width="98%" AllowPaging="True" AllowSorting="True" OnPageIndexChanging="GridView1_PageIndexChanging" OnRowCommand="GridView1_RowCommand" OnRowCreated="GridView1_RowCreated" OnRowDataBound="GridView1_RowDataBound" OnSorting="GridView1_Sorting">
                                <AlternatingRowStyle CssClass="AlternatingRowStyle" />
                                <Columns>
                                    <asp:TemplateField HeaderText="編輯">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImageBtnEdit" runat="server" ImageUrl="Images/Edit.gif" Style="width: 21px" CommandName="Modify" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="刪除">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImageBtnDelete" runat="server" ImageUrl="Images/delete.gif" CommandName="Delete" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="RoleID" HeaderText="編號" SortExpression="RoleID" />
                                    <asp:BoundField DataField="RoleName" HeaderText="角色名稱" SortExpression="RoleName" />
                                    <asp:BoundField DataField="Comments" HeaderText="備註說明" SortExpression="Comments" />
                                </Columns>
                                <FooterStyle CssClass="FooterStyle" />
                                <HeaderStyle CssClass="HeaderStyle" />
                                <PagerStyle CssClass="PagerStyle" HorizontalAlign="Center" />
                                <RowStyle CssClass="RowStyle" />
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
