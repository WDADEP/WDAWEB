<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WDA._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
        <h1>勞發署-影像系統</h1>
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
                    <td>日期：</td>
                    <td>說明：</td>
                </tr>
                <tr class="success">
                    <td>2015/10/20</td>
                    <td>第一階段交付</td>
                </tr>
                <tr>
                    <td>2015/10/21</td>
                    <td>掃描測試</td>
                </tr>
                  <tr class="success">
                    <td>2015/11/06</td>
                    <td>整合測試</td>
                </tr>
            </table>
        </div>
        <%-- </a>--%>
    </div>
</asp:Content>
