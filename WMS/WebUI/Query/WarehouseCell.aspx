<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WarehouseCell.aspx.cs" Inherits="WMS.WebUI.Query.WarehouseCell" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>货位信息显示</title>
    <script type="text/javascript" src="../../JQuery/jquery-1.8.3.min.js"></script>
    <script type="text/javascript" src= "../../JScript/Common.js"></script>
     <script type="text/javascript">
         function content_resize() {

             //編輯頁面 div高度
            
             var h = 300;
             if ($(window).height() <= 0) {
                 h = document.body.clientHeight - 50;
             }
             else {
                 h = $(window).height() - 50;
             }
             $("#pnlCell").css("height", h);
         }
     </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:Panel ID="pnlCell" runat="server"  Width="100%" Height="450px"  style=" overflow:auto; padding:10 10 10 5;" >
    </asp:Panel>
    </form>
</body>
</html>
