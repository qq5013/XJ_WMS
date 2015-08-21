<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConfigPlan.aspx.cs" Inherits="WMS.WebUI.SysInfo.ConfigPlan.ConfigPlan" %>

<%@ Register Assembly="YYControls" Namespace="YYControls" TagPrefix="yyc" %>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>无标题页</title>
   <link href="../../../Css/css.css" type="text/css" rel="stylesheet" />
   <link href="../../../Css/op.css" type="text/css" rel="stylesheet" />
   <script type="text/javascript" src="../../../JScript/Common.js"></script>
</head>
<body leftmargin="0" topmargin="0">
    <form id="form1" runat="server">
    <table  style="width:100%">
      <tr>
        <td class="OperationBar">
            &nbsp;
            <asp:Button ID="btnSave" runat="server" CssClass="ButtonSave" Text="保存" OnClick="btnSave_Click" />
            <asp:Button ID="btnExit" runat="server" CssClass="ButtonExit" Text="退出" OnClientClick="Exit();" />
         </td>
            
      </tr>
      <tr>
        <td  valign="top">
          <div style="width:100%; height:490px; overflow-x:hidden; overflow-y:auto;">
           <yyc:SmartTreeView ID="sTreeModule" runat="server" ShowLines="True" AllowCascadeCheckbox="True"></yyc:SmartTreeView>
          </div>
       </td>
      </tr>
</table>
    </form>
</body>
</html>
