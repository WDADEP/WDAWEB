<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserQueryMenu.aspx.cs" Inherits="WDA.UserQueryMenu" %>

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
            <li>您可設定一個以上的查詢條件，以取得更精確的查詢結果。</li>
        </ol>
    </div>
       <div class="well well-lg">
           <div class="panel panel-primary">
               <div class="panel-heading">
                   <h3 class="panel-title">掃描清單作業─查詢</h3>
               </div>
               <div class="panel-body">
                   <div style="width: 98%; text-align: right">
                        <asp:Button ID="BtnAdd" runat="server" Text="新增人員"  Class="btn btn-success" OnClick="BtnAdd_Click"/>
                    </div>
                   <table class="ItemTD_green" style="width: 98%; float: right; border-collapse: separate; border-spacing: 1px;" border="1">
                        <tr style="padding: 5px;">
                            <td class="HeadTD_green" style="padding: 5px;">
                                <asp:Label ID="lblUserName" runat="server" Text="帳號：" Width="90px"></asp:Label>
                            </td>
                            <td style="padding: 5px; text-align: left;">
                                <asp:TextBox ID="TxtUserName" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                         <tr>
                            <td class="HeadTD_green" style="padding: 5px;">
                                <asp:Label ID="lblRealName" runat="server" Text="姓名：" Width="90px"></asp:Label>
                            </td>
                            <td style="padding: 5px; text-align: left;">
                                <asp:TextBox ID="TxtRealName" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="HeadTD_green" style="padding: 5px;">
                                <asp:Label ID="lblRole" runat="server" Text="角色：" Width="90px"></asp:Label>
                            </td>
                            <td style="padding: 5px; text-align: left;">
                                <asp:DropDownList ID="DDLRole" runat="server"></asp:DropDownList>
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
                       <h3 class="panel-title">人員管理─詳細列表如下：</h3>
                   </div>
                   <div class="panel-body">
                   </div>
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="GridViewStyle" Width="98%" OnRowCreated="GridView1_RowCreated" OnRowDataBound="GridView1_RowDataBound" OnPageIndexChanging="GridView1_PageIndexChanging" OnRowCommand="GridView1_RowCommand" OnSorting="GridView1_Sorting" AllowPaging="True" AllowSorting="True">
                                <AlternatingRowStyle CssClass="AlternatingRowStyle" />
                                <Columns>
                                       <asp:TemplateField HeaderText="編輯">
                                        <EditItemTemplate>
                                            <asp:ImageButton ID="ITImageListBtnChang" runat="server" ImageUrl="~/Images/Chang.gif" OnClick="ITImageListBtnChang_Click" />
                                            <asp:ImageButton ID="ITImageBtnCancel" runat="server" ImageUrl="~/Images/Cancel.gif" OnClick="ITImageBtnCancel_Click" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImageBtnEdit" runat="server" ImageUrl="Images/Edit.gif" Style="width: 21px" OnClick="ImageBtnEdit_Click"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="停用">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImageBtnDelete" runat="server" ImageUrl="Images/delete.gif" CommandName="Stop" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="UserName" HeaderText="員工帳號" SortExpression="UserName" ReadOnly="True"  />
                                       <asp:TemplateField HeaderText="員工姓名" SortExpression="RealName">
                                           <EditItemTemplate>
                                               <asp:TextBox ID="TxtRealName" Text='<%# Bind("RealName") %>' runat="server"></asp:TextBox>
                                           </EditItemTemplate>
                                           <ItemTemplate>
                                               <asp:Label ID="Label2" runat="server" Text='<%# Bind("RealName") %>'></asp:Label>
                                           </ItemTemplate>
                                       </asp:TemplateField>
                                    <asp:TemplateField HeaderText="角色">
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ITDDLRole" runat="server">
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("RoleName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="UserStatus" HeaderText="UserStatus" SortExpression="UserStatus" ReadOnly="True"  />
                                      <asp:BoundField DataField="UserID" HeaderText="人員編號" SortExpression="UserID"   ReadOnly="True"/>
                                           <asp:BoundField DataField="RoleID" HeaderText="角色編號" SortExpression="RoleID"   ReadOnly="True"/>
                                       <asp:TemplateField HeaderText="分機號碼" SortExpression="TEL">
                                           <EditItemTemplate>
                                               <asp:TextBox ID="TxtTEL" runat="server" Text='<%# Bind("TEL") %>'></asp:TextBox>
                                           </EditItemTemplate>
                                           <ItemTemplate>
                                               <asp:Label ID="Label3" runat="server" Text='<%# Bind("TEL") %>'></asp:Label>
                                           </ItemTemplate>
                                       </asp:TemplateField>
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
