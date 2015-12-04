<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RoleAdd.aspx.cs" Inherits="WDA.RoleAdd" %>

<%@ Register Assembly="WDA" Namespace="WDA" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
        <script type="text/javascript">
        function pageLoad() {
            showMessage(); 
        }

        //判斷瀏覽器
        function getTargetElement(evt) {
            var elem;
            if (evt.target) {
                elem = (evt.target.nodeType == 3) ? evt.target.parentNode : evt.target
            }
            else {
                elem = evt.srcElement
            }
            return elem
        }
        //單選
        function OnClientTreeNodeChecked(evt, treeId) {
            evt = (evt) ? evt : ((window.event) ? window.event : " ");
            if (evt == " ") {
                return;
            }
            var ele = getTargetElement(evt);
            if (ele.type == "checkbox" && ele.checked) {
                tv = document.getElementById(treeId.id);
                es = tv.getElementsByTagName("INPUT");
                for (var i = 0; i < es.length; i++) {
                    if (es[i].id != ele.id) {
                        es[i].checked = false;
                    }
                }
            }
        }

        function AutoOnTreeNodeChecked() {
            var ele = event.srcElement;
            if (ele.type == "checkbox") {
                var childrenDivID = ele.id.replace("CheckBox", "Nodes");
                var div = document.getElementById(childrenDivID);
                if (div != null) {
                    var checkBoxs = div.getElementsByTagName("INPUT");
                    for (var i = 0; i < checkBoxs.length; i++) {
                        if (checkBoxs[i].type == "checkbox") {
                            checkBoxs[i].checked = ele.checked;
                        }
                    }
                }
                AutoOnTreeNodeChildChecked(ele);
            }
        }
        function AutoOnTreeNodeChildChecked(ele) {
            //自動處理上層
            var parentDiv = ele.parentElement.parentElement.parentElement.parentElement.parentElement;
            var parentChkBox = document.getElementById(parentDiv.id.replace("Nodes", "CheckBox"));
            if (parentChkBox != null) {
                var ChildsChkAll = true;
                var Boxs = parentDiv.getElementsByTagName("INPUT");
                for (var i = 0; i < Boxs.length; i++) {
                    if (Boxs[i].type == "checkbox" && Boxs[i].checked == false) {
                        ChildsChkAll = false;
                    }
                }
                parentChkBox.checked = ChildsChkAll;
                OnTreeNodeChildChecked(parentChkBox);
            }
        }

        function OnTreeNodeChecked() {
            var ele = event.srcElement;
            if (ele.type == "checkbox") {
                //父不選時，子節點都不選
                if (!ele.checked) {
                    var childrenDivID = ele.id.replace("CheckBox", "Nodes");
                    var div = document.getElementById(childrenDivID);
                    if (div != null) {
                        var checkBoxs = div.getElementsByTagName("INPUT");
                        for (var i = 0; i < checkBoxs.length; i++) {
                            if (checkBoxs[i].type == "checkbox") {
                                checkBoxs[i].checked = ele.checked;
                            }
                        }
                    }
                }
                OnTreeNodeChildChecked(ele);

                CheckBoxCheckAll(ele.id);
            }
        }
        function OnTreeNodeChildChecked(ele) {
            var parentDiv = ele.parentElement.parentElement.parentElement.parentElement.parentElement;
            var parentChkBox = document.getElementById(parentDiv.id.replace("Nodes", "CheckBox"));
            if (parentChkBox != null) {
                var ChildsChkAll = false;
                var Boxs = parentDiv.getElementsByTagName("INPUT");
                for (var i = 0; i < Boxs.length; i++) {
                    if (Boxs[i].type == "checkbox" && Boxs[i].checked == true) {
                        ChildsChkAll = true;
                    }
                }
                parentChkBox.checked = ChildsChkAll;
                OnTreeNodeChildChecked(parentChkBox);
            }
        }
        function CheckBoxCheckAll(treeId) {
            var id;
            var checkControl;
            if (treeId.indexOf('MainContent_treeViewRolePrivilege') > -1) {
                id = 'MainContent_treeViewRolePrivilege';
                checkControl = document.getElementById('MainContent_ChkRolePrivilege');
            }
            else if (treeId.indexOf('MainContent_treeViewViewerPrivilege') > -1) {
                id = 'MainContent_treeViewViewerPrivilege';
                checkControl = document.getElementById('MainContent_ChkViewerPrivilege');
            }
            if (id != null) {
                var div = document.getElementById(id);
                if (div != null) {
                    var check = false;
                    var checkBoxs = div.getElementsByTagName("INPUT");
                    for (var i = 0; i < checkBoxs.length; i++) {
                        if (checkBoxs[i].type == "checkbox") {
                            check = checkBoxs[i].checked;
                        }
                        if (!check) {
                            break;
                        }
                    }
                    checkControl.checked = check;
                }
            }
        }
        function TreeViewCheckAll(treeId) {
            var div = document.getElementById(treeId.id);
            if (div != null) {
                var check = false;
                var checkBoxs = div.getElementsByTagName("INPUT");
                if (treeId.id == 'MainContent_treeViewRolePrivilege') {
                    check = $get('MainContent_ChkRolePrivilege').checked;
                }
                else if (treeId.id == 'MainContent_treeViewViewerPrivilege') {
                    check = $get('MainContent_ChkViewerPrivilege').checked;
                }

                for (var i = 0; i < checkBoxs.length; i++) {
                    if (checkBoxs[i].type == "checkbox") {
                        checkBoxs[i].checked = check;
                    }
                }
            }
        }
    </script>
      <div class="alert alert-info">
        <button type="button" class="close" data-dismiss="alert">&times;</button>
        <h4>說明：</h4>
        <ol>
            <li>輸入「新增項目」，完成後請按【新增】鍵執行新增動作。</li>
             <li><kbd>*</kbd>號為必填項目</li>
            <li><a href="RoleQueryMenu.aspx" class="alert-link">返回角色管理</a></li>
        </ol>
    </div>
    <div class="well well-lg">
           <div class="panel panel-primary">
               <div class="panel-heading">
                   <h3 class="panel-title">角色管理─新增角色：</h3>
               </div>
               <div class="panel-body">
                   <table class="ItemTD_green" style="width: 98%; float: right; border-collapse: separate; border-spacing: 1px" border="1">
                        <tr style="width: 100px">
                            <td class="HeadTD_green" style="padding: 5px">
                                <span class="t15_red">＊</span>
                                <asp:Label ID="Label1" runat="server">角色名稱</asp:Label>
                            </td>
                            <td style="text-align: left; padding: 5px">
                                <asp:TextBox ID="txtRoleName" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="HeadTD_green" style="padding: 5px">備註說明</td>
                            <td style="text-align: left; padding: 5px">
                                <asp:TextBox ID="txtComments" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="HeadTD_green" style="padding: 5px">
                                <span class="t15_red">＊</span>
                                <asp:Label ID="Label2" runat="server">網頁權限</asp:Label>
                                <asp:CheckBox ID="ChkRolePrivilege" runat="server" />
                            </td>
                            <td style="text-align: left; padding: 5px">
                                <div id="divRolePrivilege">
                                    <asp:TreeView ID="treeViewRolePrivilege" runat="server" ShowCheckBoxes="All" ShowExpandCollapse="False" ForeColor="Blue"></asp:TreeView>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="HeadTD_green" style="padding: 5px">
                                <span class="t15_red">＊</span>
                                <asp:Label ID="Label3" runat="server">影像預覽權限</asp:Label>
                                <asp:CheckBox ID="ChkViewerPrivilege" runat="server" />
                            </td>
                            <td style="text-align: left; padding: 5px">
                                <div id="divViewerPrivilege">
                                    <asp:TreeView ID="treeViewViewerPrivilege" runat="server" ShowCheckBoxes="All" ShowExpandCollapse="False" ForeColor="Blue"></asp:TreeView>
                                </div>
                            </td>
                        </tr>
                    </table>
                   <table style="width: 100%; text-align: center; padding: 3px; border: 1px; border-spacing: 1px; border-top: 1px solid #FFFFFF; border-left: 1px solid #FFFFFF; border-right: 1px solid #FFFFFF; border-bottom: 1px solid #FFFFFF">
                    <tr style="text-align: center">
                        <td>
                            <asp:Button ID="Button1" runat="server" Text="新 增" class="btn btn-large btn-success" OnClick="BtnOK_Click"/>
                        </td>
                        <td>
                            <asp:Button ID="Button2" runat="server" Text="清 除" class="btn btn-large btn-success" OnClick="BtnClear_Click" />
                        </td>
                    </tr>
                </table>
               </div>
           </div>
     </div>
    <cc1:LiteralMessageBox ID="LiteralMessageBox1" runat="server"></cc1:LiteralMessageBox>
    <input id="HiddenMessage" type="Hidden" runat="server" />
</asp:Content>
