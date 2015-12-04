<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ActiveXViewer.aspx.cs" Inherits="WDA.ActiveXViewer" %>

<%@ Register Assembly="WDA" Namespace="WDA" TagPrefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
      <script src="Scripts/jquery-1.10.2.min.js"></script>
    <title>【勞發署】影像暨檔管系統</title>
     <script type="text/javascript">
        function pageLoad() {
            try {
                var scanViewer = document.getElementById('scanViewer1');

                if (!(typeof (scanViewer) == "object" && scanViewer.object != null)) {
                    $('#obj_TiMac').show();
                }
                else {
                    $('#obj_TiMac').hide();
                }

                var url = '<%= Session["URL"].ToString() %>';
                scanViewer.debug = true; scanViewer.setViewer(url, 'GET');
            }
            catch (e) { alert(e.message); }
        }

        function closeWindow() {
            window.close();
        }
    </script>
      <noscript>
         您的瀏覽器，不支援script語法，請砍掉重練。
    </noscript>
</head>
<body>
     <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div style="position: absolute; top: 0px; left: 0px; width: 100%; height: 100%">
            <div id="obj_TiMac">
                <span class="t15_red">掃描元件未成功安裝，請檢查是否已將入信任網站</span>
            </div>
            <object id="scanViewer1" classid="clsid:D8FC0E2B-8EAA-4169-AC2D-B872D01A619E" codebase="FileDownload/ScanViewer.cab" style="width: 100%; height: 100%" />
        </div>
         <cc1:LiteralMessageBox ID="LiteralMessageBox1" runat="server"></cc1:LiteralMessageBox>
        <input id="HiddenMessage" type="Hidden" runat="server" />
    </form>
</body>
</html>
