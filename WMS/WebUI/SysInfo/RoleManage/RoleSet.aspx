<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RoleSet.aspx.cs" Inherits="WMS.WebUI.SysInfo.RoleManage.RoleSet" %>
<%@ Register Assembly="YYControls" Namespace="YYControls" TagPrefix="yyc" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>用户信息</title>
    <script type="text/javascript" src="../../../JQuery/jquery-2.1.3.min.js"></script>
    <script type="text/javascript" src="../../../JScript/Common.js"></script>
    <link href="../../../css/main.css" rel="Stylesheet" type="text/css" />
    
    <link href="../../../css/op.css" rel="Stylesheet" type="text/css" />
    <style type="text/css">
   
     
        
    </style>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $(window).resize(function () {
                resize();
            });
        });
        function resize() {
            var h = document.documentElement.clientHeight - 40;
            $("#plTree").css("height", h);
        }

        function Exit() {
            var message = "您确定要离开吗？";
            if (confirm(message))
                window.parent.parent.removetab();
            return false;
        }
    </script>
</head>
<body style=" margin:0px 0 0 0;" >
    <form id="form1" runat="server">
    
     <table class="maintable" style="width:100%; height:35px" >
        <tr style="font-size:10pt; font-weight:bold; color:Black;">
            <td style=" width:40%">  <asp:Label ID="lbTitle" runat="server" Font-Bold="True" Font-Size="10pt" Height="24px"
                    Width="284px">用户组权限设置</asp:Label>&nbsp; 
            </td>
            <td  style="width:30%; height: 30px;">
                <asp:LinkButton ID="lnkBtnExpand" runat="server" OnClick="lnkBtnExpand_Click" BorderStyle="None" Width="45px">展开</asp:LinkButton> &nbsp;&nbsp;&nbsp;
                <asp:LinkButton ID="lnkBtnCollapse" runat="server" OnClick="lnkBtnCollapse_Click"  Width="45px">折叠</asp:LinkButton>
            </td>
            <td align="right" style="width:30%; height: 30px;">
                <asp:Button ID="lnkBtnSave" runat="server" CssClass="ButtonSave" OnClick="lnkBtnSave_Click"  Text="保存" />  &nbsp;&nbsp;
                <asp:Button ID="lnkBtnCancle" runat="server" CssClass="ButtonExit" Text="离开" 
                    OnClientClick="return Exit();"/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            </td>
         
        </tr>
      </table>
      
    
        
    <div>
        <div id="plTree"  style="height:443px; width:100%; overflow: auto; " >
           <yyc:smarttreeview id="sTreeModule" runat="server" allowcascadecheckbox="True"
                showlines="True" Font-Size="9pt" ExpandDepth="1">
               <LeafNodeStyle ForeColor="MidnightBlue" />
           </yyc:smarttreeview>
        </div>
    </div>
    
    </form>
</body>
</html>
