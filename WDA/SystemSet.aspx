<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SystemSet.aspx.cs" Inherits="WDA.SystemSet" %>

<%@ Register Assembly="WDA" Namespace="WDA" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
      <script type="text/javascript">
        var IsUpdate = false;
        function pageLoad() {
            showMessage(); 
        }

        function doUpdate(name) {
            try {
                IsUpdate = true;
                var HiddenSystemNames = $get('MainContent_HiddenSystemNames');
                if (HiddenSystemNames.value.indexOf(" " + name.id + " ") < 0) {
                    HiddenSystemNames.value += " " + name.id + " ";
                }
            } catch (ex) {
                alert(ex.description);
            }
        }

        function doSave() {
            try {
                if (!IsUpdate) {
                    alert("沒有異動任何資料。");
                    return false;
                }
                if (confirm("確定要修改?"))
                { return true; }
                else return false;

            } catch (ex) {
                alert(ex.description);
            }
        }
    </script>
      <div class="alert alert-info">
        <button type="button" class="close" data-dismiss="alert">&times;</button>
        <h4>說明：</h4>
        <ol>
            <li>請輸入設定值，完成後請按【確定】鍵執行設定動作。</li>
        </ol>
    </div>
    <div class="well well-lg">
        <div class="panel panel-primary">
            <div class="panel-heading">
                <h3 class="panel-title">系統管理─環境設定</h3>
            </div>
            <div class="panel-body">
                <div class="accordion" id="accordion">
                    <div class="accordion-group">
                        <div class="accordion-heading">
                            <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion" href="#collapseSystem">影像環境設定：
                            </a>
                        </div>
                        <div id="collapseSystem" class="accordion-body collapse in" style="height: 0px;">
                            <div class="accordion-inner">
                                <table style="width: 100%; float: right; border-collapse: separate; border-spacing: 1px;" border="1">
                                    <tr style="padding: 5px;">
                                        <td class="t15_darkblue_B" style="padding: 5px;"><span class="t15_red">＊</span>匯出檔案格式：
                                        </td>
                                        <td style="padding: 5px; text-align: left;">

                                            <asp:TextBox ID="txtExportFileFormat" runat="server" onchange="javascript:doUpdate(this);"></asp:TextBox>

                                        </td>
                                    </tr>
                                    <tr style="padding: 5px;">
                                        <td class="t15_darkblue_B" style="padding: 5px;"><span class="t15_red">＊</span>匯入檔案格式：
                                        </td>
                                        <td style="padding: 5px; text-align: left;">
                                            <asp:TextBox ID="txtImportFileFormat" runat="server" onchange="javascript:doUpdate(this);"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="t15_darkblue_B" style="padding: 5px;">
                                            <span class="t15_red">＊</span>圖檔最高寬度：
                                        </td>
                                        <td style="padding: 5px; text-align: left;">
                                            <asp:TextBox ID="txtImageIType" runat="server" onchange="javascript:doUpdate(this);"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="t15_darkblue_B" style="padding: 5px;">
                                            <span class="t15_red">＊</span>全彩壓縮比：
                                        </td>
                                        <td style="padding: 5px; text-align: left;">
                                            <asp:TextBox ID="txtImageIJZip" runat="server" onchange="javascript:doUpdate(this);"></asp:TextBox>
                                            <span class="t15_red">(1-100)</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="t15_darkblue_B" style="padding: 5px;">
                                            <span class="t15_red">＊</span>二值化參數：
                                        </td>
                                        <td style="padding: 5px; text-align: left;">
                                            <asp:TextBox ID="txtImageIThreshold" runat="server" onchange="javascript:doUpdate(this);"></asp:TextBox>
                                            <span class="t15_red">(0-255),自動=-1</span>
                                        </td>
                                    </tr>
                                    <tr style="padding: 5px;">
                                        <td class="t15_darkblue_B" style="padding: 5px;"><span class="t15_red">＊</span>圖檔最大限制(Byte)：
                                        </td>
                                        <td style="padding: 5px; text-align: left;">
                                            <asp:TextBox ID="txtISize" runat="server" onchange="javascript:doUpdate(this);"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                    <div class="accordion-group">
                        <div class="accordion-heading">
                            <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion" href="#collapseOthers">其他環境設定：
                            </a>
                        </div>
                        <div id="collapseOthers" class="accordion-body collapse in" style="height: 0px;">
                            <div class="accordion-inner">
                                <table style="width: 100%;float: right; border-collapse: separate; border-spacing: 1px;" border="1">
                                    <tr style="padding: 5px;">
                                        <td class="t15_darkblue_B" style="padding: 5px;"><span class="t15_red">＊</span>ServiceURL：
                                        </td>
                                        <td style="padding: 5px; text-align: left;">
                                            <asp:TextBox ID="txtServiceURL" runat="server" Width="700px" onchange="javascript:doUpdate(this);"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="padding: 5px;">
                                        <td class="t15_darkblue_B" style="padding: 5px;"><span class="t15_red">＊</span>每頁顯示多少筆數：
                                        </td>
                                        <td style="padding: 5px; text-align: left;">
                                            <asp:DropDownList ID="ddlPageSize" runat="server" onchange="javascript:doUpdate(this);">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                            </div>
                        </div>
                    </div>
                    <div class="accordion-group">
                        <div class="accordion-heading">
                            <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion" href="#collapseAutoUpload">自動版更設定：
                            </a>
                        </div>
                        <div id="collapseAutoUpload" class="accordion-body collapse in" style="height: 0px;">
                            <div class="accordion-inner">
                                <table style="width: 100%; float: right; border-collapse: separate; border-spacing: 1px;" border="1">
                                    <tr style="padding: 5px;">
                                        <td class="t15_darkblue_B" style="padding: 5px;"><span class="t15_red">＊</span>ViewerClassID：
                                        </td>
                                        <td style="padding: 5px; text-align: left;">
                                            <asp:TextBox ID="txtViewerClassID" runat="server" onchange="javascript:doUpdate(this);" Width="700px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="padding: 5px;">
                                        <td class="t15_darkblue_B" style="padding: 5px;"><span class="t15_red">＊</span>ViewerVersion：
                                        </td>
                                        <td style="padding: 5px; text-align: left;">
                                            <asp:TextBox ID="txtViewerVersion" runat="server" onchange="javascript:doUpdate(this);"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="padding: 5px;">
                                        <td class="t15_darkblue_B" style="padding: 5px;"><span class="t15_red">＊</span>ViewerUploadServerURL：
                                        </td>
                                        <td style="padding: 5px; text-align: left;">
                                            <asp:TextBox ID="txtViewerUploadServerURL" runat="server" onchange="javascript:doUpdate(this);" Width="700px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <table border="0" style="width: 100%; border-collapse: collapse">
            <tr style="text-align: center">
                <td>
                    <asp:Button ID="BtnOK" runat="server" Text="確 定" class="btn btn-large btn-success" OnClientClick="JavaScript:if(!doSave()) {return false} ;" OnClick="BtnOK_Click" />
                </td>
                <td>
                    <asp:Button ID="BtnClear" runat="server" Text="復 原" class="btn btn-large btn-success" OnClick="BtnClear_Click" />
                </td>
            </tr>
        </table>
    </div>
    <cc1:LiteralMessageBox ID="LiteralMessageBox1" runat="server"></cc1:LiteralMessageBox>
    <input id="HiddenMessage" type="Hidden" runat="server" />
    <input id="HiddenSystemNames" runat="server" type="hidden" />
</asp:Content>
