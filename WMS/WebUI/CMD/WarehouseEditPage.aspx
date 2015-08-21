<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WarehouseEditPage.aspx.cs" Inherits="WMS.WebUI.CMD.WarehouseEditPage" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>仓库</title>
    <base target="_self" />
    <script type="text/javascript" src="../../JScript/Check.js?t=00"></script>
   <script type="text/javascript" src="../../JScript/SelectDialog.js?t=00"></script>
    <%--<script type="text/javascript" src="../../JScript/SelectDialog.js"></script>--%>
    <link href="../../css/FieldsetCss.css?0=t" rel="Stylesheet" type="text/css" />

    <script src="../../JScript/InputLength.js" type="text/javascript"></script>
    <link href="../../css/op.css" rel="Stylesheet" type="text/css" />
    <script>
        function RefreshParent(path) {
            alert('仓库删除成功！');
            window.parent.document.getElementById('hdnRemovePath').value = path;
            window.parent.document.getElementById('btnRemoveNode').click();
        }


        function UpdateParent() {
            alert('仓库修改成功！');
            window.parent.document.getElementById('btnUpdateSelected').click();
        }

        function ReloadParent() {
            alert('仓库添加成功！');
            window.parent.document.getElementById('btnReload').click();
        }
        function openwin() {
            window.open("BatchAssignedProduct.aspx", "", "height=410px, width=600px,top=200px,left=300px, toolbar=no, menubar=no, scrollbars=no, resizable=no, location=no, status=no")
        }
    </script>
</head>
<body style="margin-left:20px;">
    <form id="form1" runat="server">  
    <fieldset style="width: 509px">
        <legend>&nbsp;仓库</legend>
    <table>
      <tr style="display:none;">
         <td colspan="4"><asp:TextBox ID="txtWHID" runat="server" CssClass="HiddenControl" ></asp:TextBox>
             &nbsp;</td>
      </tr>
      <tr>
         <td class="tdTitle"><font color="red">*</font>仓库编码</td>
         <td><asp:TextBox ID="txtWhCode" runat="server" CssClass="TextBox" Width="140px" 
                 MaxLength="2"></asp:TextBox>
         </td>
         <td class="tdTitle"><font color="red">*</font>仓库名称</td> 
         <td><asp:TextBox ID="txtWhName" runat="server"  CssClass="TextBox" Width="140px" onpropertychange="javascript:setMaxLength(this,20);"></asp:TextBox>
         </td>
      </tr>
      <tr>
         <td class="tdTitle"><font color="red">*</font>K3编码</td>
         <td><asp:TextBox ID="txtK3" runat="server" CssClass="TextBox" Width="140px"></asp:TextBox>
         </td>
         <td class="tdTitle"></td> 
         <td> 
         </td>
      </tr>
      <tr>
         <td class="tdTitle">备注</td> 
         <td colspan="3"><asp:TextBox ID="txtMemo" runat="server" CssClass="MultilineTextbox" Width="376px" Rows="10" TextMode="MultiLine" MaxLength="246"></asp:TextBox>
         </td>
      </tr> 
              <tr>
	            <td align="center" style="height:35px; text-align:center;" colspan="4">
                          
                          <asp:Button ID="btnSave" runat="server" CssClass="button" Text="保存"  OnClick="btnSave_Click" OnClientClick="return CheckBeforeSubmit()" />&nbsp;
                  </td>  
              </tr> 
    </table>
   </fieldset> 
    </form>
<script>
    function CheckBeforeSubmit() {
        var code = document.getElementById('txtWhCode').value;
        var name = document.getElementById('txtWhName').value; //document.getElementById('txtTitle').value.trim();


        if (code == "") {
            alert('仓库编码不能为空！');
            return false;
        }
        if (name == "") {
            alert('仓库名称不能为空！');
            return false;
        }
    }    
</script>
</body>
</html>
