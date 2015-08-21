<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductBomEdit.aspx.cs" Inherits="WMS.WebUI.CMD.ProductBomEdit" %>

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
                $("#btnProduct").bind("click", function () {
                    GetOtherValue('CMD_Product', 'txtProductModel,txtProduct', "PRODUCT_MODEL,PRODUCT_CODE", "1=1");
                    return false;
                });
                $("#btnColor").bind("click", function () {
                    var ProductID = $('#txtProduct').val();
                    if (ProductID == "") {
                        alert("请先选择产品资料！");
                        return false;
                    }
                    GetOtherValue('CMD_COLOR', "txtColorID,txtColorName", "COLOR_CODE,COLOR_NAME", "PRODUCT_CODE='" + ProductID + "'");
                    return false;
                });

            }
            function Save() {

                if (trim($("#txtID").val()) == "") {
                    alert("产品编码不能为空!");
                    $("#txtID").focus();
                    return false;
                }

                if (trim($("#txtName").val()) == "") {
                    alert("产品名称不能为空!");
                    $("#txtCustomer_Name").focus();
                    return false;
                }
                if (trim($("#txtUnit").val()) == "") {
                    alert("单位不能为空!");
                    $("#txtUnit").focus();
                    return false;
                }
                if (trim($("#txtProductModel").val()) == "") {
                    alert("产品型号不能为空!");
                    $("#txtProductModel").focus();
                    return false;
                }
                if (trim($("#txtColorName").val()) == "") {
                    alert("规格不能为空!");
                    $("#txtColorName").focus();
                    return false;
                }
                return true;
            }
           
        </script>
	    <style type="text/css">
            .style1
            {
                width: 24%;
            }
            .style2
            {
                height: 28px;
                background-color: #7baed9;
                color: blue;
                font-family: 宋体;
                font-size: 9pt;
                width: 10%;
            }
            .style3
            {
                height: 28px;
                background-color: #7baed9;
                color: #ffffff;
                font-family: 宋体;
                font-size: 9pt;
                width: 10%;
            }
        </style>
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
                        <td align="center" class="style2"  >
                             产品编码
                        </td>
                        <td class="style1" >
                             &nbsp;<asp:TextBox 
                                 ID="txtID" runat="server"   
                                 BorderWidth="0" CssClass="TextBox" Width="62%" MaxLength="100" ></asp:TextBox>
                        </td>
                         <td  align="center" class="style2" >
                             产品名称
                        </td>
                        <td>
                         &nbsp;<asp:TextBox 
                                ID="txtName" runat="server"  BorderWidth="0"
                                CssClass="TextBox" Width="62%"></asp:TextBox>
                        </td>
                        <td align="center" class="style2"  >
                             单位
                        </td>
                        <td style="width:32%;" >
                             &nbsp;<asp:TextBox ID="txtUnit" runat="server"   
                                 BorderWidth="0" CssClass="TextBox" Width="46%" ></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        
                         <td align="center" class="style2"  >
                             WMS产品型号
                             <asp:TextBox ID="txtProduct" runat="server"   
                                 BorderWidth="0" CssClass="TextBox" Width="0px" Height="0px" ></asp:TextBox>
                        </td>
                        <td class="style1">
                             &nbsp;<asp:TextBox ID="txtProductModel" runat="server" BorderWidth="0" 
                                 CssClass="TextBox" Height="16px"  Width="62%"></asp:TextBox>&nbsp;<asp:Button ID="btnProduct" runat="server" CssClass="ButtonCss" Height="17px" 
                                 Text="..." Width="20px" />
                             &nbsp;&nbsp;</td>
                         <td  align="center" class="style2" >
                             规格<asp:TextBox ID="txtColorID" runat="server" 
                                 Width="0px" CssClass="TextBox"  MaxLength="1"></asp:TextBox>
                        </td>
                        <td>
                         &nbsp;<asp:TextBox ID="txtColorName" runat="server" 
                                BorderWidth="0" Width="62%" CssClass="TextBox"></asp:TextBox>&nbsp;<asp:Button ID="btnColor" Width="20px"  CssClass="ButtonCss" runat="server"  
                                Text="..." />
                         </td>
                         <td align="center" class="style3" >
                            排列顺序
                         </td>
                         <td>
                             &nbsp;<asp:TextBox ID="txtOrderNum" 
                                 runat="server"  CssClass="TextBox" Width="46%" 
                                 onkeypress="return regInput(this,/^\d+$/,String.fromCharCode(event.keyCode))" 	onpaste="return regInput(this,/^\d+$/,window.clipboardData.getData('Text'))" 
                                        
                                 ondrop="return regInput(this,/^\d+$/,event.dataTransfer.getData('Text'))" onfocus="TextFocus(this);" BorderWidth="0px" ></asp:TextBox>
                         </td>
                    </tr>
                        
                    <tr>
                        <td align="center" class="style3"  >
                             子公司直营价格 
                        </td>
                        <td class="style1" >
                             &nbsp;<asp:TextBox ID="txtPrice1" runat="server"   
                                 BorderWidth="0" CssClass="TextBox" Width="62%" style="text-align:right;" 
                                        
                                 onkeypress="return regInput(this,/^\d*\.?\d{0,2}$/,String.fromCharCode(event.keyCode))" 	onpaste="return regInput(this,/^\d*\.?\d{0,2}$/,window.clipboardData.getData('Text'))" 
                                        
                                 ondrop="return regInput(this,/^\d*\.?\d{0,2}$/,event.dataTransfer.getData('Text'))" onfocus="TextFocus(this);" ></asp:TextBox>
                        </td>
                         <td  align="center" class="style3" >
                             子公司经销商价格
                        </td>
                        <td>
                         &nbsp;<asp:TextBox 
                                ID="txtPrice2" runat="server"  
                                CssClass="TextBox" Width="62%" style="text-align:right;" 
                                        
                                onkeypress="return regInput(this,/^\d*\.?\d{0,2}$/,String.fromCharCode(event.keyCode))" 	onpaste="return regInput(this,/^\d*\.?\d{0,2}$/,window.clipboardData.getData('Text'))" 
                                        
                                ondrop="return regInput(this,/^\d*\.?\d{0,2}$/,event.dataTransfer.getData('Text'))" onfocus="TextFocus(this);"    BorderWidth="0px"></asp:TextBox>
                        </td>
                        <td align="center" class="style3"  >
                             区域经销商价格</td>
                        <td style="width:32%;" >
                             &nbsp;<asp:TextBox ID="txtPrice3" runat="server"   
                                 BorderWidth="0" CssClass="TextBox" Width="46%" style="text-align:right;" 
                                        onkeypress="return regInput(this,/^\d*\.?\d{0,2}$/,String.fromCharCode(event.keyCode))" 	onpaste="return regInput(this,/^\d*\.?\d{0,2}$/,window.clipboardData.getData('Text'))" 
                                        ondrop="return regInput(this,/^\d*\.?\d{0,2}$/,event.dataTransfer.getData('Text'))" onfocus="TextFocus(this);" ></asp:TextBox>
                        </td>
                    </tr>
                     <tr>
                        <td align="center" class="style3"  >
                             网渠价格
                        </td>
                        <td class="style1" >
                             &nbsp;<asp:TextBox ID="txtPrice4" runat="server"   
                                 BorderWidth="0" CssClass="TextBox" Width="62%" style="text-align:right;" 
                                        
                                 onkeypress="return regInput(this,/^\d*\.?\d{0,2}$/,String.fromCharCode(event.keyCode))" 	onpaste="return regInput(this,/^\d*\.?\d{0,2}$/,window.clipboardData.getData('Text'))" 
                                        
                                 ondrop="return regInput(this,/^\d*\.?\d{0,2}$/,event.dataTransfer.getData('Text'))" onfocus="TextFocus(this);"></asp:TextBox>
                        </td>
                         <td  align="center" class="style3" >
                             其它价格</td>
                        <td>
                         &nbsp;<asp:TextBox 
                                ID="txtPrice5" runat="server"  BorderWidth="0"
                                CssClass="TextBox" Width="62%" style="text-align:right;" 
                                        
                                onkeypress="return regInput(this,/^\d*\.?\d{0,2}$/,String.fromCharCode(event.keyCode))" 	onpaste="return regInput(this,/^\d*\.?\d{0,2}$/,window.clipboardData.getData('Text'))" 
                                        
                                ondrop="return regInput(this,/^\d*\.?\d{0,2}$/,event.dataTransfer.getData('Text'))" onfocus="TextFocus(this);"></asp:TextBox>
                        </td>
                        <td align="center" class="style3"  >
                             成本价格</td>
                        <td style="width:32%;" >
                             &nbsp;<asp:TextBox ID="txtStandPrice" runat="server"   
                                 BorderWidth="0" CssClass="TextBox" Width="46%" style="text-align:right;" 
                                        onkeypress="return regInput(this,/^\d*\.?\d{0,2}$/,String.fromCharCode(event.keyCode))" 	onpaste="return regInput(this,/^\d*\.?\d{0,2}$/,window.clipboardData.getData('Text'))" 
                                        ondrop="return regInput(this,/^\d*\.?\d{0,2}$/,event.dataTransfer.getData('Text'))" onfocus="TextFocus(this);"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center"  class="style3">
                            备注
                         </td>
                        <td colspan="5">
                            &nbsp;<asp:TextBox ID="txtMemo" runat="server" CssClass="MultiLineTextBox" 
                                TextMode="MultiLine" Height="102px" Width="81%"></asp:TextBox>
                        </td>
                    </tr>		
			</table>
			
		</form>
	</body>
</html>
