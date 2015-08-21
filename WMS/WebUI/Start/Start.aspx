﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Start.aspx.cs" Inherits="WMS.WebUI.Start.Start" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head id="Head1" runat="server">
    <title>登录页面</title>
    <script type="text/javascript" src='<%=ResolveClientUrl("~/JQuery/jquery-1.8.3.min.js") %>'></script> 
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            window.moveTo(0, 0);
            window.resizeTo(screen.availWidth, screen.availHeight);
            var top = screen.availHeight / 2 - 153;
            var left = screen.availWidth / 2 - 275;
            divLogin.style.top = top;
            divLogin.style.left = left;
        });
        function document.onkeydown() {
            if (event.keyCode == 13 && event.srcElement.type != 'button' && event.srcElement.type != 'submit' && event.srcElement.type != 'reset' && event.srcElement.type != 'textarea' && event.srcElement.type != '') {
                event.keyCode = 9;
                GotoLogin();
            }
        }
        function GotoLogin() {
            document.getElementById("btnLogin").click();
        }
        function CheckCode() {

            return true;
        }
 										 
   </script>
    <style type="text/css">
    
    body {
	    margin-left: 0px;
	    margin-top: 0px;
	    margin-right: 0px;
	    margin-bottom: 0px;
	    background-image:url(../../images/login/login_bg.jpg);
	    font-size:11pt;
    }
    
    .buttonLogin
    {
       border: 0px #ff0000 solid; 
       background-image:url(../../images/login/button.jpg);
       width:85px;
       height:22px;
    }
    .login_input
    {
       border:solid 1px white;
       background-color:transparent;
    }
    	
    
    </style>
</head>
<body style="text-align:center; vertical-align:middle;margin:auto;">
    <form id="Form1" runat="server" defaultfocus="txtUserName">

<div id="divLogin"  style=" background-image:url(../../images/login/login_bg02.jpg); background-repeat:no-repeat; text-align:center; width:551px; height:306px; position:absolute; top:200px; left:400px">
     <table style="width:100%;" cellpadding="0" cellspacing="0">
        <tr style="height:100px;">
           <td style="width:230px;">&nbsp;</td>
           <td>&nbsp;</td>
        </tr>
        <tr>
           <td></td>
           <td>
                <table  border="0" cellpadding="0" cellspacing="0" style=" font-weight:bold; font-family:楷体_GB2312;">
                          <tr><td colspan="3" style="height: 30px"></td></tr>
                          <tr style="height:40px;">
                            <td style="height: 32px; color:White;">用户名：</td>
                            <td style="height: 32px; text-align:left; width: 217px;">
                                <asp:TextBox ID="txtUserName" CssClass="login_input"  runat="server" 
                                    TabIndex="1" Width="149px" Height="22" ForeColor="Black" ></asp:TextBox></td>
                            <td></td>
                          </tr>
                          <tr style="height:40px;">
                            <td style="color:White;">密　码：</td>
                            <td style="text-align:left; width: 217px;"><asp:TextBox ID="txtPassWord" 
                                    runat="server" CssClass="login_input"  TextMode="Password" TabIndex="2" 
                                    Width="149px" Height="22" ForeColor="Black"></asp:TextBox></td>
                            <td></td>
                          </tr>
                </table>
                <table  border="0" cellpadding="0" cellspacing="0">
                        <tr>
                          <td  style="text-align: left; width: 275px;height:46px">&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnLogin" runat="server" CssClass="buttonLogin" Text="登  录" OnClick="btnLogin_Click" TabIndex="4" ForeColor="DimGray" />
                            <asp:Button ID="btnReset" runat="server" CssClass="buttonLogin" Text="退  出" TabIndex="5" ForeColor="DimGray" OnClientClick="javascript:window.opener=null;window.close();" /></td>
                        </tr>
                        <tr><td style="height: 24px; text-align: center">
                            <asp:Label ID="labMessage" runat="server" ForeColor="Red" Font-Size="X-Small"></asp:Label><br /></td></tr>
                </table>
           </td>
        </tr>    
     </table>    
     <table style="width:100%">
        <tr><td style=" font-size:12px; text-align:center; padding:10px 8px 8px 8px; height: 60px;">版权所有：厦门晶璟自动化有限公司</td></tr>
     </table>
</div> 
    </form>
</body>
</html>
