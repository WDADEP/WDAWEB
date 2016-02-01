<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WDA._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
        <h1>勞動力發展署-影像系統</h1>
        <%--  <a href="https://www.tmnewa.com.tw/B2C_V2/FrontStage/WelCome.aspx" class="thumbnail" target="_blank">--%>
         <p>隨著藍領、白領及特殊系統發文歸檔後的紙本資料日益龐大，因此需要掃描成影像檔以便保存資料及減少儲藏檔案的空間，統合以上三個系統共用資料，進行歸檔、查詢及調閱文件資料。</p>
        <div class="panel panel-primary">
            <div class="panel-heading">
                <h2 class="panel-title">系統公告：</h2>
            </div>
            <div class="panel-body">
            </div>
            <table class="table table-hover">
                <tr>
                    <td>文件名稱：</td>
                    <td>連接：</td>
                </tr>
                <tr class="success">
                    <td>問題提報單</td>
                    <td><a href="FileDownload/問題提報單.docx">問題提報單</a></td>
                </tr>
                <tr>
                    <td>操作手冊</td>
                    <td><a href="FileDownload/操作手冊.pdf">操作手冊</a></td>
            </table>
        </div>
        <%-- </a>--%>
    </div>
</asp:Content>
