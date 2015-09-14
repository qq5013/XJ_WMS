<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GroupUserManage.aspx.cs" Inherits="WMS.WebUI.SysInfo.RoleManage.GroupUserManage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
    <base target="_self" />
    <link href="~/Css/Main.css" type="text/css" rel="stylesheet" /> 
        <link href="~/Css/op.css" type="text/css" rel="stylesheet" /> 
        <script type="text/javascript" src='<%=ResolveClientUrl("~/JQuery/jquery-1.8.3.min.js") %>'></script>
        <script type="text/javascript" src='<%=ResolveClientUrl("~/JScript/Common.js") %>'></script>
        <script type="text/javascript" src='<%=ResolveClientUrl("~/JScript/Ajax.js") %>'></script>  
    <script type="text/javascript">
        function okFunc(url) {
            //      var topFrame =parent.frames['mainFrame'];
            //      if(topFrame!=null)
            //         parent.frames['mainFrame'].location=url;
            //      else location.href=url;
        }
    </script>
     
</head>
<body  >
    <form id="form1" runat="server">
        <table  class="maintable" cellspacing="0" cellpadding="0" bordercolor="#ffffff" width="100%"
            border="1" runat="server">
            <tr>
                <td style="width:30%">
                   &nbsp;&nbsp;<asp:Label ID="Label1" runat="server" Font-Bold="True" Height="24px" Text="Label"
                        Width="161px" Font-Size="10pt"></asp:Label>
                   
                </td>
               <td style="width:70%" align="right" >
                 <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click"
                        Text="保 存" />&nbsp;&nbsp;
                </td>
            </tr>
        </table>
        <div id="Sub-container"  style="overflow: auto; width: 100%; height: 100%">
            <asp:DataGrid ID="dgUser"  runat="server" SkinID="GridViewSkin" AutoGenerateColumns="False" 
                    CellPadding="5" OnItemDataBound="dgUser_ItemDataBound" Width="95%" 
                onitemcreated="dgUser_ItemCreated">
                <Columns>
                    <asp:BoundColumn DataField="UserName" HeaderText="用户名"></asp:BoundColumn>
                    <asp:BoundColumn DataField="GroupName" HeaderText="所属用户组"></asp:BoundColumn>
                    <asp:ButtonColumn CommandName="Select" HeaderText="用户组设置">
                    </asp:ButtonColumn>
                    <asp:BoundColumn DataField="UserID"></asp:BoundColumn>
                </Columns>
                 <FooterStyle BackColor="White" ForeColor="#000066" />
                <SelectedItemStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" Mode="NumericPages" />
                <ItemStyle ForeColor="#000066" />
                <HeaderStyle BackColor="WhiteSmoke" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                Font-Strikeout="False" Font-Underline="False" ForeColor="Black" Wrap="False" />
            </asp:DataGrid>
         </div>
   
    </form>
</body>
</html>