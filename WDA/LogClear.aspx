<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LogClear.aspx.cs" Inherits="WDA.LogClear" %>

<%@ Register Assembly="WDA" Namespace="WDA" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
      <script type="text/javascript">
        function pageLoad() {
            $("#MainContent_txtCreateTime").datepicker("option", $.datepicker.regional["zh-TW"]);
            $("#MainContent_txtEndTime").datepicker("option", $.datepicker.regional["zh-TW"]);

            $("#MainContent_txtCreateTime").datepicker({
                defaultDate: new Date(),
                changeMonth: true,
                numberOfMonths: 1,
                onClose: function (selectedDate) {
                    $("#MainContent_txtEndTime").datepicker("option", "minDate", selectedDate);
                }
            });
            $("#MainContent_txtEndTime").datepicker({
                defaultDate: new Date(),
                changeMonth: true,
                numberOfMonths: 1,
                onClose: function (selectedDate) {
                    $("#MainContent_txtCreateTime").datepicker("option", "maxDate", selectedDate);
                }
            });
            showMessage();
        }
        function ImageBtnOKClick() {
            try {
                var txtCreateTime = $get('MainContent_txtCreateTime').value;

                if (txtCreateTime.length == 0) { alert('開始時間為空'); $get('MainContent_txtCreateTime').focus(); $get('MainContent_txtCreateTime').style.backgroundColor = "#FF60AF"; return false; }

                var txtEndTime = $get('MainContent_txtEndTime').value;

                if (txtEndTime.length == 0) { alert('結束時間為空'); $get('MainContent_txtEndTime').focus(); $get('MainContent_txtEndTime').style.backgroundColor = "#FF60AF"; return false; }

                if (confirm("確定刪除")) {
                    return true;
                }
                else { return false; }
            }
            catch (e) {
                alert(e.message);
                Sys.Debug.traceDump(e);
            };
        }
    </script>
       <div class="well well-lg">
           <div class="panel panel-primary">
               <div class="panel-heading">
                   <h3 class="panel-title">交易紀錄管理─清除記錄：</h3>
               </div>
               <div class="panel-body">
                   <table class="ItemTD_green" style="width: 98%; float: right; border-collapse: separate; border-spacing: 1px;" border="1">
                       <tr>
                           <td class="HeadTD_green" style="padding: 5px;">
                                 請設定交易記錄的日期：
                           </td>
                       </tr>
                       <tr style="padding: 5px;">
                           <td class="HeadTD_green" style="padding: 5px;">
                               <span class="t15_red">＊</span>開始日期：
                           </td>
                           <td style="padding: 5px; text-align: left;">起：
                                              <asp:TextBox ID="txtCreateTime" runat="server" pattern="\d{4}/\d{1,2}/\d{1,2}" title="日期格式"></asp:TextBox>
                               ～訖：
                                            <asp:TextBox ID="txtEndTime" runat="server" pattern="\d{4}/\d{1,2}/\d{1,2}" title="日期格式"></asp:TextBox>
                                23:59 以前
                           </td>
                       </tr>
                   </table>
               </div>
               <table border="0" style="width: 100%; border-collapse: collapse">
                   <tr style="text-align: center">
                       <td style="text-align: right">
                           <asp:Button ID="BtnOK" runat="server" Text="確 定" class="btn btn-large btn-success" OnClientClick="JavaScript:if(!ImageBtnOKClick()) {return false} ;" OnClick="BtnOK_Click"/>
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
