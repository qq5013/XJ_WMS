<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GroupUserList.aspx.cs" Inherits="WMS.WebUI.SysInfo.RoleManage.GroupUserList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>无标题页</title>
    <script type="text/javascript" src="../../../JQuery/jquery-2.1.3.min.js"></script>
    <script type="text/javascript" src="../../../JScript/Check.js?t=00"></script>
    <script type="text/javascript" src="../../../JScript/Common.js"></script>
    <link href="../../../css/main.css" rel="Stylesheet" type="text/css" />
    
    <link href="../../../css/op.css" rel="Stylesheet" type="text/css" />
   
 
    <style type="text/css">
        .style1
        {
            height: 13px;
            width: 336px;
        }
        .style2
        {
            width: 336px;
        }
    </style>
   
 
</head>
<body style="margin:0 0 0 0;">
    <form id="form1" runat="server">
    <div>
      <table  class="maintable" cellspacing="0" cellpadding="0" width="100%" align="center" bordercolor="#ffffff" border="1" runat="server">
        <tr>
          <td  ><asp:Label ID="Label1" runat="server" Text="用户组成员" Height="21px" Width="167px" Font-Bold="True" Font-Size="10pt"></asp:Label></td>
        </tr>
        <tr>
          <td  style=" width:100%">
              <asp:DataGrid ID="dgGroupUser" runat="server" AutoGenerateColumns="False"    SkinID="GridViewSkin"
                  CellPadding="4" OnDeleteCommand="dgGroupUser_DeleteCommand" 
              AllowPaging="True" OnPageIndexChanged="dgGroupUser_PageIndexChanged" 
                  OnItemDataBound="dgGroupUser_ItemDataBound" PageSize="8" width="100%">
                  <Columns>
                      <asp:BoundColumn DataField="UserID" HeaderText="ID"></asp:BoundColumn>
                      <asp:BoundColumn DataField="UserName" HeaderText="用户名">
                          <HeaderStyle Width="20%" />
                      </asp:BoundColumn>
                      <asp:BoundColumn DataField="GroupName" HeaderText="用户组">
                          <HeaderStyle Width="30%" />
                      </asp:BoundColumn>
                      <asp:ButtonColumn CommandName="Delete" HeaderText="操作" Text="删除">
                          <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                              Font-Underline="False" ForeColor="#000066" HorizontalAlign="Center" VerticalAlign="Middle" />
                          <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                              Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" Width="50%" />
                      </asp:ButtonColumn>
                  </Columns>
                 <HeaderStyle Height="25px" BackColor="WhiteSmoke" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                 <PagerStyle Mode="NumericPages" />
                  

              </asp:DataGrid>
          </td>
        </tr>
      </table>           
    </div>
    </form>
</body>
</html>
