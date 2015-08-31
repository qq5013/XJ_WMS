<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WarehouseCellEditPage.aspx.cs" Inherits="WMS.WebUI.CMD.WarehouseCellEditPage" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>货架货位</title>
    <base target="_self" />
    <script type="text/javascript" src="../../JScript/Check.js?t=00"></script>
    <script type="text/javascript" src="../../JScript/SelectDialog.js?t=00"></script>
    <link href="../../css/FieldsetCss.css" rel="Stylesheet" type="text/css" />
    <link href="../../css/op.css" rel="Stylesheet" type="text/css" />

    <script src="../../JScript/InputLength.js" type="text/javascript"></script>
    <script type="text/javascript">
        function RefreshParent(path) {
            alert('货位删除成功！');
            window.parent.document.getElementById('hdnRemovePath').value = path;
            window.parent.document.getElementById('btnRemoveNode').click();
        }

        function UpdateParent() {
            alert('货位修改成功！');
            window.parent.document.getElementById('btnUpdateSelected').click();
        }

        function ReloadParent() {
            alert('货位添加成功！');
            window.parent.document.getElementById('btnReload').click();
        }
        function openwin() {
            window.open("BatchAssignedProduct.aspx", "", "height=410px, width=600px,top=200px,left=300px, toolbar=no, menubar=no, scrollbars=no, resizable=no, location=no, status=no")
        }
    </script>
</head>
<body style="margin-left:20px;">
    <form id="form1" runat="server">
         <fieldset style="width: 550px">
                  <legend>货位</legend>   
                   <table>
                      <tr style="display:none;"><td colspan="4"><asp:TextBox ID="txtCELLID" runat="server"  CssClass="HiddenControl"></asp:TextBox>
                      <asp:TextBox ID="txtAreaID" runat="server"  CssClass="HiddenControl"></asp:TextBox>
                      <asp:TextBox ID="txtShelfID" runat="server"  CssClass="HiddenControl"></asp:TextBox>
                          <input class="ButtonCreate" name="btnBack" onclick="openwin()" type="button" value="批量分配指定卷烟" /></td></tr>
                      <tr>                        
                         <td class="tdTitle"><font color="red">*</font>库区名称</td>
                         <td><asp:TextBox ID="txtAreaName" runat="server" CssClass="TextBox" ></asp:TextBox></td> 
                         <td class="tdTitle"><font color="red">*</font>货架名称</td>
                         <td  ><asp:TextBox ID="txtShelfName" runat="server" CssClass="TextBox" ></asp:TextBox>
                         </td>
                      </tr>
                      <tr>
                         <td class="tdTitle"><font color="red">*</font>货位编码</td> 
                         <td  ><asp:TextBox ID="txtCellCode" runat="server"  CssClass="TextBox"  onpropertychange="javascript:setMaxLength(this,10);"></asp:TextBox>
                         </td>
                         <td class="tdTitle"><font color="red">*</font>货位名称</td> 
                         <td><asp:TextBox ID="txtCellName" runat="server"  CssClass="TextBox"  onpropertychange="javascript:setMaxLength(this,20);"></asp:TextBox>
                         </td>
                      </tr>
                      
                      <tr>
                         <td class="tdTitle">货位层数</td> 
                         <td><asp:TextBox ID="txtCellRows" runat="server"  CssClass="TextBoxRight" 
                                 Width="126px" onblur="IsNumber(this,'货位层数')">1</asp:TextBox>
                         </td>
                         <td class="tdTitle">货位列数</td> 
                         <td><asp:TextBox ID="txtCellCols" runat="server"  CssClass="TextBoxRight" 
                                 Width="124px" onblur="IsNumber(this,'货位列数')"></asp:TextBox>
                         </td>
                      </tr> 
                      <tr>
                         <td class="tdTitle">
                             是否锁定</td> 
                         <td><asp:DropDownList ID="ddlLock" runat="server">
                                 <asp:ListItem Selected="True" Value="1">锁定</asp:ListItem>
                                 <asp:ListItem Value="0">解锁</asp:ListItem>
                             </asp:DropDownList></td>
                         <td class="tdTitle">
                             是否启用</td> 
                         <td><asp:DropDownList ID="ddlActive" runat="server">
                                 <asp:ListItem Selected="True" Value="1">启用</asp:ListItem>
                                 <asp:ListItem Value="0">未启用</asp:ListItem>
                             </asp:DropDownList></td>
                      </tr> 
                      <tr>
                         <td class="tdTitle">备注</td> 
                         <td colspan="3" style="text-align: left"><asp:TextBox ID="txtMemo" runat="server"  
                                 CssClass="MultilineTextBox" Width="376px" Rows="10" TextMode="MultiLine" 
                                 Height="71px"></asp:TextBox>
                         </td>
                      </tr> 
    
                      <tr><td colspan="4" align="center"  style="height:35px; text-align:center;">
                          <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="button" OnClick="btnSave_Click" OnClientClick="return CheckBeforeSubmit()"/>
                         </td></tr>                                                                                                                                                                      
                    </table>  
              </fieldset>
    </form>
<script type="text/javascript">
    function CheckBeforeSubmit() {
        var cellcode = document.getElementById('txtCellCode').value;
        var cellname = document.getElementById('txtCellName').value;
       
        if (cellcode == "") {
            alert('货位编码不能为空！');
            return false;
        }
        if (cellname == "") {
            alert('货位名称不能为空！');
            return false;
        }

    }

    function clear(id) {
        alert(id)
        document.getElementById(id).value = "";
    }
</script>
</body>
</html>
