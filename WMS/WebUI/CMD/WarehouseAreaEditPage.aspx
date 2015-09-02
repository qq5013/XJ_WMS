<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WarehouseAreaEditPage.aspx.cs" Inherits="WMS.WebUI.CMD.WarehouseAreaEditPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>库区</title>
    <base target="_self" />
    <script type="text/javascript" src="../../JScript/Check.js?t=00"></script>
     <script type="text/javascript" src="../../JScript/SelectDialog.js?t=00"></script>
    <link href="../../css/FieldsetCss.css" rel="Stylesheet" type="text/css" />
    <link href="../../css/op.css" rel="Stylesheet" type="text/css" />
    
    <script src="../../JScript/InputLength.js" type="text/javascript"></script>
    <script type="text/javascript">
        function RefreshParent(path) {
            alert('库区删除成功！');
            window.parent.document.getElementById('hdnRemovePath').value = path;
            window.parent.document.getElementById('btnRemoveNode').click();
        }

        function UpdateParent() {
            alert('库区修改成功！');
            window.parent.document.getElementById('btnUpdateSelected').click();
        }

        function ReloadParent() {
            alert('库区添加成功！');
            window.parent.document.getElementById('btnReload').click();
        }
        function openwin() {
            window.open("BatchAssignedProduct.aspx", "", "height=410, width=600,top=200px,left=300px, toolbar=no, menubar=no, scrollbars=no, resizable=no, location=no, status=no")
        }
        function CheckBeforeSubmit() {
            var areacode = document.getElementById('txtAreaCode').value;
            var areaname = document.getElementById('txtAreaName').value; //document.getElementById('txtTitle').value.trim();


            if (areacode == "") {
                alert('库区编码不能为空！');
                return false;
            }
            if (areacode.length != 3) {
                alert('库区编码长度为三码！');
                return false;
            }
            if (areaname == "") {
                alert('库区名称不能为空！');
                return false;
            }
        }
    </script>
     
</head>
<body style="margin-left:20px; height: 286px;">
    <form id="form1" runat="server">
              <fieldset style="width: 509px; height: 260px;" >
                   <legend>仓库库区</legend>   
                   <table  style=" width:99%; height:98%">
                      <tr style="display:none;">
                      <td colspan="4">
                          <asp:TextBox ID="txtAreaID" runat="server" CssClass="HiddenControl" 
                              Width="71px"></asp:TextBox>
                          <input class="ButtonCreate" name="btnBack" onclick="openwin()" type="button" value="批量分配指定卷烟" />
                        </td>
                       </tr>
                      <tr>
                         <td class="tdTitle" style="height: 26px"><font color="red">*</font>仓库编码</td>
                         <td style="height: 26px" align="left">
                         <asp:TextBox ID="txtWHID" runat="server" CssClass="TextBox" Width="140px" ></asp:TextBox>
                         </td>
                          
                         <td class="tdTitle" style="height: 26px"><font color="red">*</font>仓库名称</td>
                         <td style="height: 26px" align="left"><asp:TextBox ID="txtWhName" runat="server" CssClass="TextBox" Width="140px"></asp:TextBox></td>
                      </tr>                   
                      <tr>
                         <td class="tdTitle"><font color="red">*</font>库区编码</td> 
                         <td style="height: 26px" align="left"><asp:TextBox ID="txtAreaCode" runat="server" CssClass="TextBox" Width="140px"></asp:TextBox>
                             
                         </td>
                         <td class="tdTitle">
                             <font color="red">*</font>库区名称
                          </td> 
                         <td style="height: 26px" align="left" >
                             <asp:TextBox ID="txtAreaName" runat="server"  CssClass="TextBox" Width="140px" onpropertychange="javascript:setMaxLength(this,20);"></asp:TextBox>
                         </td>
                      </tr>
                     
                      <tr>
                         <td class="tdTitle">备注</td>
                         <td colspan="3" align="left">
                             <asp:TextBox ID="txtMemo" runat="server" CssClass="MultilineTextbox" 
                                 Width="379px" Rows="10" TextMode="MultiLine" Height="161px"></asp:TextBox>
                         </td>
                      </tr>
                      <tr><td  align="center" colspan="4" style="height:35px; text-align:center;">
                          &nbsp;
                          <asp:Button ID="btnSave" runat="server" CssClass="button" Text="保存" OnClick="btnSave_Click"  OnClientClick="return CheckBeforeSubmit()"/>
                          
                      </td></tr>
                    </table>  
              </fieldset>
    </form>
    
</body>
</html>
