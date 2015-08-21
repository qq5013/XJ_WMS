<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FactoryEdit.aspx.cs" Inherits="WMS.WebUI.CMD.FactoryEdit" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head id="Head1" runat="server">
		<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />  
        <title></title> 
        <link href="~/Css/Main.css" type="text/css" rel="stylesheet" /> 
        <link href="~/Css/op.css" type="text/css" rel="stylesheet" /> 
        
        <script type="text/javascript" src='<%=ResolveClientUrl("~/JQuery/jquery-1.8.3.min.js") %>'></script>
        <script type="text/javascript" src='<%=ResolveClientUrl("~/JScript/Common.js") %>'></script>
      

        <script type="text/javascript">

            function Save() {

                if (trim($("#txtID").val()) == "") {
                    alert("供应商编码不能为空!");
                    $("#txtID").focus();
                    return false;
                }

                if (trim($("#txtCustomer_Name").val()) == "") {
                    alert("供应商名称不能为空!");
                    $("#txtCustomer_Name").focus();
                    return false;
                }
//                if (trim($("#txtCustomer_Person").val()) == "") {
//                    alert("联络人不能为空!");
//                    $("#txtCustomer_Person").focus();
//                    return false;
//                }
//                if (trim($("#txtCustomer_Phone").val()) == "") {
//                    alert("联络电话不能为空!");
//                    $("#txtCustomer_Phone").focus();
//                    return false;
//                }
                 
                return true;
            }
           
        </script>
	   
	</head>
	<body >
		<form id="form1" runat="server">
        <table style="width: 100%; height: 20px;" class="OperationBar">
                <tr>
                    <td align="right">
                        <asp:Button ID="btnCancel" runat="server" Text="放弃" 
                            OnClientClick="return Cancel();" CssClass="ButtonCancel" 
                             />
                        <asp:Button ID="btnSave" runat="server" Text="保存" OnClientClick="return Save()" 
                            CssClass="ButtonSave" onclick="btnSave_Click" 
                             />
                        <asp:Button ID="btnExit" runat="server" Text="离开" OnClientClick="return Exit();" 
                            CssClass="ButtonExit"  />
                    </td>
                </tr>
            </table>
			<table id="Table1" class="maintable" cellspacing="0" cellpadding="0" width="100%" align="center" bordercolor="#ffffff"
				border="1" runat="server">			
					<tr>
                        <td align="center" class="musttitle" style="width:8%;"  >
                             供应商编码
                        </td>
                        <td colspan="2">
                             &nbsp;<asp:TextBox ID="txtID" runat="server"   
                                 BorderWidth="0" CssClass="TextBox" Width="46%" MaxLength="10" ></asp:TextBox>
                        </td>
                         <td  align="center" class="musttitle" style="width:8%;" >
                             供应商名称
                        </td>
                        <td>
                         &nbsp;<asp:TextBox 
                                ID="txtFactoryName" runat="server"  BorderWidth="0"
                                CssClass="TextBox" Width="44%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" class="smalltitle" style="width:8%;"  >
                             联系人
                        </td>
                        <td colspan="2">
                             &nbsp;<asp:TextBox ID="txtLinkPerson" runat="server"   
                                 BorderWidth="0" CssClass="TextBox" Width="46%" ></asp:TextBox>
                        </td>
                         <td  align="center" class="smalltitle" style="width:8%;" >
                             联系电话
                        </td>
                        <td>
                         &nbsp;<asp:TextBox 
                                ID="txtLinkPhone" runat="server"  BorderWidth="0"
                                CssClass="TextBox" Width="44%"></asp:TextBox>
                        </td>
                    </tr>
                    
                    <tr>
                        <td align="center" class="smalltitle" style="width:8%;"  >
                             传真
                        </td>
                        <td style=" width:20%">
                            &nbsp;<asp:TextBox ID="txtFax" runat="server"   
                                 BorderWidth="0" CssClass="TextBox" Width="65%" ></asp:TextBox>
                        </td>
                        <td  align="center"  style=" width: 8%;" class="smalltitle"  >
                             地址
                        </td>
                        <td colspan="2">
                             &nbsp;<asp:TextBox ID="txtAddress" runat="server"   
                                 BorderWidth="0" CssClass="TextBox" Width="51%" Height="16px"></asp:TextBox>
                        </td>
                         
                    </tr>
                    <tr>
                        <td align="center"  class="smalltitle" style="width:8%;">
                            备注
                        </td>
                        <td colspan="4">
                            &nbsp;<asp:TextBox ID="txtMemo" runat="server" CssClass="MultiLineTextBox" 
                                TextMode="MultiLine" Height="102px" Width="66%"></asp:TextBox>
                        </td>
                    </tr>		
			</table>
			
		</form>
	</body>
</html>
