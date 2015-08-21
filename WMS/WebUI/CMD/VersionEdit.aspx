<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VersionEdit.aspx.cs" Inherits="WMS.WebUI.CMD.VersionEdit" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head id="Head1" runat="server">
		<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />  
        <title></title> 
        <link href="~/Css/Main.css" type="text/css" rel="stylesheet" /> 
        <link href="~/Css/op.css" type="text/css" rel="stylesheet" /> 
        
        <script type="text/javascript" src='<%=ResolveClientUrl("~/JQuery/jquery-1.8.3.min.js") %>'></script>
        <script type="text/javascript" src='<%=ResolveClientUrl("~/JScript/Common.js") %>'></script>
       
       <script type="text/javascript" src='<%=ResolveClientUrl("~/JScript/DataProcess.js") %>'></script>

        <script type="text/javascript">
            $(document).ready(function () {
                BindEvent();
            });
            function BindEvent() {
                $("#txtProduct").bind("dblclick", function () {
                    GetOtherValue('CMD_Product', 'txtProduct,txtProductName', "PRODUCT_CODE,PRODUCT_NAME", "1=1");
                });
                $("#txtProduct").bind("change", function () {
                    getWhereBaseData('V4_CMD_PRODUCT', 'txtProduct,txtProductName', 'PRODUCT_CODE,PRODUCT_NAME', "PRODUCT_CODE='" + this.value + "'");

                });
            }
            function Save() {


                if (trim($("#txtVersionID").val()) == "") {
                    alert("版本编码不能为空!");
                    $("#txtVersionID").focus();
                    return false;
                }

                if (trim($("#txtColor_Name").val()) == "") {
                    alert("版本名称不能为空!");
                    $("#txtColor_Name").focus();
                    return false;
                }
                if (trim($("#txtProduct").val()) == "") {
                    alert("产品编码不能为空!");
                    $("#txtProduct").focus();
                    return false;
                }

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
                             版本编码
                        </td>
                        <td width="30%">
                              &nbsp;<asp:TextBox ID="txtVersionID" runat="server"   
                                 BorderWidth="0" CssClass="TextBox" Width="46%" MaxLength="1" ></asp:TextBox> 
                                 <asp:TextBox ID="txtID" runat="server"   
                                 BorderWidth="0" CssClass="TextBox" Width="0px" Height="0px"   ></asp:TextBox>
                        </td>
                         <td  align="center" class="musttitle" style="width:8%;" >
                             版本名称
                        </td>
                        <td>
                         &nbsp;<asp:TextBox 
                                ID="txtVersion_Name" runat="server"  BorderWidth="0"
                                CssClass="TextBox" Width="42%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" class="musttitle" style="width:8%;"  >
                             产品编码
                        </td>
                        <td colspan="3">
                             &nbsp;<asp:TextBox 
                                 ID="txtProduct" runat="server"   
                                 BorderWidth="0" CssClass="TextBox" Width="15%"   ></asp:TextBox>&nbsp;
                             <asp:TextBox 
                                ID="txtProductName" runat="server"  BorderWidth="0"
                                CssClass="TextRead" Width="50%"></asp:TextBox>
                        </td>
                         
                    </tr>
                    <tr>
                        <td align="center"  class="smalltitle" style="width:8%;">
                            备注
                        </td>
                        <td colspan="3">
                            &nbsp;<asp:TextBox ID="txtMemo" runat="server" CssClass="MultiLineTextBox" 
                                TextMode="MultiLine" Height="102px" Width="66%"></asp:TextBox>
                        </td>
                    </tr>		
			</table>
			
		</form>
	</body>
</html>
