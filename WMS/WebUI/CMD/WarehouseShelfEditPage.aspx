<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WarehouseShelfEditPage.aspx.cs" Inherits="WMS.WebUI.CMD.WarehouseShelfEditPage" %>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>库区货架</title>
    <base target="_self" />
    <script type="text/javascript" src="../../JScript/Check.js?t=00"></script>
    <link href="../../css/FieldsetCss.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript" src="../../JScript/SelectDialog.js?t=00"></script>
    <link href="../../css/op.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript" src='<%=ResolveClientUrl("~/JQuery/jquery-1.8.3.min.js") %>'></script>
    <script type="text/javascript" src='<%=ResolveClientUrl("~/JScript/Common.js") %>'></script>
    <script type="text/javascript" src='<%=ResolveClientUrl("~/JScript/DataProcess.js") %>'></script>
    <script src="../../JScript/InputLength.js" type="text/javascript"></script>
    <script type="text/javascript">
        function RefreshParent(path) {
            alert('货架删除成功！');
            window.parent.document.getElementById('hdnRemovePath').value = path;
            window.parent.document.getElementById('btnRemoveNode').click();
        }
        function UpdateParent() {
            alert('货架修改成功！');
            window.parent.document.getElementById('btnUpdateSelected').click();
        }
        function ReloadParent() {
            alert('货架添加成功！');
            window.parent.document.getElementById('btnReload').click();
        }
        function openwin() {
            window.open("BatchAssignedProduct.aspx", "", "height=410, width=600,top=200px,left=300px, toolbar=no, menubar=no, scrollbars=no, resizable=no, location=no, status=no")
        }

        function CheckBeforeSubmit() {
            var shelfcode = document.getElementById('txtShelfCode').value;
            var shelfname = document.getElementById('txtShelfName').value; //document.getElementById('txtTitle').value.trim();
            var cols = document.getElementById('txtCellCols').value;
            var rows = document.getElementById('txtCellRows').value;

            if (shelfcode == "") {
                alert('货架编码不能为空！');
                return false;
            }
            if (shelfcode.length != 6) {
                alert('货架编码为6码！');
                return false;
            }
            if (shelfname == "") {
                alert('货架名称不能为空！');
                return false;
            }



        }
      

    </script>

    <style type="text/css">
        .MultilineTextBox
        {}
    </style>

</head>
<body style="margin-left:20px; height: 286px;">
    <form id="form1" runat="server">
            <fieldset style="width: 509px; height:260px">
                <legend>货架</legend>   
                   <table style=" width:99%; height:98%">
                      <tr style="display:none;"><td colspan="4">
                      <asp:TextBox ID="txtShelfID" runat="server" CssClass="HiddenControl" Width="61px"></asp:TextBox>
                          <asp:TextBox ID="txtWHID" runat="server" CssClass="HiddenControl" Width="61px"></asp:TextBox>
                          <asp:TextBox ID="txtAreaID" runat="server" CssClass="HiddenControl" 
                              Width="61px"></asp:TextBox>
                          <input class="ButtonCreate" name="btnBack" onclick="openwin()" type="button" value="批量分配指定卷烟" /></td>
                       </tr>
                      <tr>
                         <td class="tdTitle"><font color="red">*</font>仓库名称</td>
                         <td>
                             <asp:TextBox ID="txtWhName" runat="server" CssClass="TextBox" Width="140px"></asp:TextBox></td>
                         <td class="tdTitle"><font color="red">*</font>库区名称</td>
                         <td><asp:TextBox ID="txtAreaName" runat="server" CssClass="TextBox" Width="140px"></asp:TextBox></td>
                         
                      </tr>
                      <tr>
                         <td class="tdTitle"><font color="red">*</font>货架编码</td> 
                         <td><asp:TextBox ID="txtShelfCode" runat="server"  CssClass="TextBox" Width="140px" onpropertychange="javascript:setMaxLength(this,10);"></asp:TextBox>
                         </td>
                         <td class="tdTitle"><font color="red">*</font>货架名称</td> 
                         <td><asp:TextBox ID="txtShelfName" runat="server"  CssClass="TextBox" Width="140px" onpropertychange="javascript:setMaxLength(this,20);"></asp:TextBox>
                         </td>
                      </tr>

                     
                      
                      <tr>
                         <td class="tdTitle">货架层数</td> 
                         <td><asp:TextBox ID="txtCellRows" runat="server"  CssClass="TextBoxRight" Width="140px" onblur="IsNumber(this,'货架行数')">1</asp:TextBox>
                         </td>
                         <td class="tdTitle">货架列数</td> 
                         <td><asp:TextBox ID="txtCellCols" runat="server"  CssClass="TextBoxRight" Width="140px" onblur="IsNumber(this,'货架列数')">1</asp:TextBox>
                         </td>
                      </tr>  
                      
                      <tr>
                         <td class="tdTitle">是否启用</td> 
                         <td><asp:DropDownList ID="ddlActive" runat="server" Height="16px" Width="97px">
                                 <asp:ListItem Selected="True" Value="1">启用</asp:ListItem>
                                 <asp:ListItem Value="0">未启用</asp:ListItem>
                             </asp:DropDownList>
                         </td>
                         <td class="tdTitle">&nbsp;</td> 
                         <td>&nbsp;</td>
                      </tr>     
                      <tr>
                         <td class="tdTitle">备注</td> 
                         <td colspan="3" style="text-align: left">
                             <asp:TextBox ID="txtMemo" runat="server"  
                                 CssClass="MultilineTextBox" Width="390px" Rows="10" TextMode="MultiLine" 
                                 Height="85px"></asp:TextBox>
                         </td>
                      </tr> 
                     
                      <tr><td colspan="4" align="center"  style="height:25px; text-align:center;">
                          <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="button" OnClick="btnSave_Click" OnClientClick="return CheckBeforeSubmit()"/>
                         </td>
                         </tr>                                                                                
                    </table>  
              </fieldset>
    </form>
    
</body>
</html>
