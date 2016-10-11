<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginPost.aspx.cs" Inherits="WDA.LoginPost" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body onload='document.forms[0].submit()'>
    <form id="form1" runat="server" action="Login.aspx" method="post">
        <asp:HiddenField ID="RePage" runat="server" />
    </form>
</body>
</html>
