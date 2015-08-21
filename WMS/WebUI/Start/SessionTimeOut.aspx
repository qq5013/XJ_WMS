<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SessionTimeOut.aspx.cs" Inherits="WMS.WebUI.Start.SessionTimeOut" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script type="text/javascript" >
        function opennew() {
            alert("对不起，操作已经超时，请重新新登录！");
            window.parent.opener = null;
            window.parent.close();
            window.open('Start.aspx', '_Parent', 'toolbar=no,status=yes,resizable=no');
           
//            var udswin = window.open("Start.aspx", "", "toolbar=no,status=yes,resizable=no");
//            udswin.moveTo(0, 0);
//            udswin.resizeTo(window.screen.availWidth, window.screen.availHeight);
//            window.opener = null;
//            closeWindow();
        }
        function closeWindow() {
            window.open('', '_self', '');
            window.close();
        }
        
    </script>
</head>
<body onload="opennew();">
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
</body>
</html>
