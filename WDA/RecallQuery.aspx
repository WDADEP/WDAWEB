<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RecallQuery.aspx.cs" Inherits="WDA.RecallQuery" %>

<%@ Register Assembly="WDA" Namespace="WDA" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function pageLoad() {
            $("#MainContent_txtJicuiTime").datepicker("option", $.datepicker.regional["zh-TW"]);

            $("#MainContent_txtJicuiTime").datepicker({
                defaultDate: new Date(),
                changeMonth: true,
                numberOfMonths: 1
            });
            $("#MainContent_txtJicuiTime").datepicker('setDate', new Date());
            showMessage();
        }

        function ImageBtnOKClick() {
            try {
                var txtJicuiTime = $get('MainContent_txtJicuiTime').value;
                var txtUserName = $get('MainContent_txtUserName').value;

                if (txtJicuiTime.length == 0) { alert('請選擇稽催日期'); return false; }
                if (txtUserName.length == 0) { $('#MainContent_txtUserName').val('*'); }

                var scriptName = document.forms[0].action;
                window.document.forms[0].target = '_blank';
                setTimeout(function () {
                    window.document.forms[0].target = '';
                    document.forms[0].action = scriptName;
                }, 500);

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
            <li>稽催日期預設為當日日期</li>
            <li>借閱人代號可使用＊列印全部，也可輸入帳號個別列印 </li>
        </ol>
    </div>
    <div class="well well-lg">
        <div class="panel panel-primary">
            <div class="panel-heading">
                <h3 class="panel-title">借檔催還─列印</h3>
            </div>
            <div class="panel-body">
                <table class="ItemTD_green" style="width: 98%; float: right; border-collapse: separate; border-spacing: 1px;" border="1">
                    <tr style="padding: 5px;">
                        <td class="HeadTD_green" style="padding: 5px;"><span class="t15_red">＊</span>
                            稽催日期：
                        </td>
                        <td style="padding: 5px; text-align: left;">
                            <asp:TextBox ID="txtJicuiTime" runat="server" pattern="\d{4}/\d{1,2}/\d{1,2}" title="日期格式"></asp:TextBox>
                            <span class="t15_red">(列印稽催日期以前借檔尚未歸還檔案清冊)</span>
                        </td>
                    </tr>
                    <tr>
                        <td class="HeadTD_green" style="padding: 5px;">借閱人代號：
                        </td>
                        <td style="padding: 5px; text-align: left;">
                            <asp:TextBox ID="txtUserName" runat="server" Text="*"></asp:TextBox>
                            <span class="t15_red">(＊表示全部列印)</span>
                        </td>
                    </tr>
                    <tr>
                        <td class="HeadTD_green" style="padding: 5px;">列印種類：
                        </td>
                        <td style="padding: 5px; text-align: left;">
                            <asp:DropDownList ID="ddlKind" runat="server">
                                <asp:ListItem Value="0">請選擇</asp:ListItem>
                                <asp:ListItem Value="1">一般案件</asp:ListItem>
                                <asp:ListItem Value="2">法制案件</asp:ListItem>
                                <asp:ListItem Value="3">行政救濟案件</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </div>
            <table border="0" style="width: 100%; border-collapse: collapse">
                <tr style="text-align: center">
                    <td style="text-align: right">
                        <asp:Button ID="BtnPrint" runat="server" Text="列 印" class="btn btn-large btn-success" OnClick="BtnPrint_Click" OnClientClick="JavaScript:if(!ImageBtnOKClick()) {return false} ;" PostBackUrl="~/RecallQueryPrint.aspx" />
                    </td>
                    <td style="text-align: center"></td>
                    <td style="text-align: left">
                        <asp:Button ID="BtnClear" runat="server" Text="取 消" class="btn btn-large btn-success" OnClick="BtnClear_Click" />
                    </td>
                </tr>
            </table>
        </div>
        <cc1:LiteralMessageBox ID="LiteralMessageBox1" runat="server"></cc1:LiteralMessageBox>
        <input id="HiddenMessage" type="Hidden" runat="server" />
    </div>
</asp:Content>
