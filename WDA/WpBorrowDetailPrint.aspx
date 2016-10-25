<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WpBorrowDetailPrint.aspx.cs" Inherits="WDA.WpBorrowDetailPrint" %>

<%@ Register Assembly="WDA" Namespace="WDA" TagPrefix="cc1" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>【勞發署】影像暨檔管系統</title>
    <script src="Scripts/jquery-1.10.2.min.js"></script>
    <script src="Scripts/JQueryUI/jquery-ui-1.10.4.custom.js"></script>
    <link href="Content/jquery-ui-1.10.4.custom.css" rel="stylesheet" />
    <script src="Scripts/Utility/initPageLoad.js"></script>
    <script type="text/javascript">
        function pageLoad() {
            LoginShowMessage();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt">
                        <LocalReport ReportPath="RptWpBorrowDetail.rdlc">
                        </LocalReport>
                    </rsweb:ReportViewer>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <cc1:LiteralMessageBox ID="LiteralMessageBox1" runat="server"></cc1:LiteralMessageBox>
        <input id="HiddenMessage" type="Hidden" runat="server" />
    </form>
</body>
</html>
