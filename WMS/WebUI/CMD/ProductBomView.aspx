<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductBomView.aspx.cs" Inherits="WMS.WebUI.CMD.ProductBomView" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
      <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />  
        <title></title> 
        <link href="~/Css/Main.css" type="text/css" rel="stylesheet" /> 
        <link href="~/Css/op.css" type="text/css" rel="stylesheet" /> 
         <script type="text/javascript" src='<%=ResolveClientUrl("~/JQuery/jquery-1.8.2.min.js") %>'></script> 
         <script type="text/javascript" src='<%=ResolveClientUrl("~/JScript/Common.js") %>'></script>
        <script type="text/javascript" src='<%=ResolveClientUrl("~/JScript/Ajax.js") %>'></script>  
        <script type="text/javascript" language="javascript">
            function Edit() {
                location.href = FormID + "Edit.aspx?SubModuleCode=" + SubModuleCode + "&FormID=" + FormID + "&ProductCode=" + document.getElementById("txtProduct").value + "&ColorID=" + document.getElementById("txtColorID").value;
                return false;
             }
        </script>      

</head>
<body>
    <form id="form1" runat="server">
      <asp:ScriptManager ID="ScriptManager1" runat="server" />  
        <asp:UpdateProgress ID="updateprogress" runat="server" AssociatedUpdatePanelID="updatePanel">
        <ProgressTemplate>            
                 <div id="progressBackgroundFilter" style="display:none"></div>
            <div id="processMessage"> Loading...<br /><br />
                 <img alt="Loading" src="../../images/main/loading.gif" />
            </div>            
 
            </ProgressTemplate>
 
        </asp:UpdateProgress>  
                <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional">                
                <ContentTemplate>
    <table style="width: 100%; height: 20px;" class="OperationBar">
                <tr>
                    <td align="right" style="width:60%">
                        <asp:Button ID="btnFirst" runat="server" Text="首筆" CssClass="ButtonFirst" 
                            onclick="btnFirst_Click"  />
                        <asp:Button ID="btnPre" runat="server" Text="上一筆" CssClass="ButtonPre" 
                            onclick="btnPre_Click"  />
                        <asp:Button ID="btnNext" runat="server" Text="下一筆" CssClass="ButtonNext" 
                            onclick="btnNext_Click"  />
                        <asp:Button ID="btnLast" runat="server" Text="尾筆" CssClass="ButtonLast" 
                            onclick="btnLast_Click"  />
                    </td>
                    <td align="right">
                      <%--  <asp:Button ID="btnPrint" runat="server" Text="导出" CssClass="ButtonPrint" 
                            OnClientClick="return print();" />--%>
                        <asp:Button ID="btnAdd" runat="server" Text="新增" CssClass="ButtonCreate" 
                            OnClientClick="Add()"  />
                        <asp:Button ID="btnDelete" runat="server" Text="刪除" CssClass="ButtonDel" OnClientClick="return ViewDelete();"
                            onclick="btnDelete_Click"  />
                        <asp:Button ID="btnEdit" runat="server" Text="修改" CssClass="ButtonModify" 
                            OnClientClick="return Edit();"  />
                        <asp:Button ID="btnBack" runat="server" Text="返回" OnClientClick="return Back()" 
                            CssClass="ButtonBack" />
                        <asp:Button ID="btnExit" runat="server" Text="离开" OnClientClick="return Exit()" 
                            CssClass="ButtonExit"  />
                        
                    </td>
                </tr>
            </table>
			<table id="Table1" class="maintable" cellspacing="0" cellpadding="0" width="100%" align="center" bordercolor="#ffffff"
				border="1" runat="server">			
					<tr>
                        <td align="center" class="musttitle" style="width:9%" >
                             产品编码
                        </td>
                        <td class="ItemStyle" >
                             &nbsp;<asp:TextBox ID="txtID" runat="server"   
                                 BorderWidth="0" CssClass="TextRead" Width="69%" MaxLength="10" 
                                 ReadOnly="True" ></asp:TextBox>
                        </td>
                         <td  align="center" class="musttitle" style="width:10%" >
                             产品名称
                        </td>
                        <td>
                         &nbsp;<asp:TextBox 
                                ID="txtName" runat="server"  BorderWidth="0"
                                CssClass="TextRead" Width="69%" ReadOnly="True"></asp:TextBox>
                        </td>
                        <td align="center" class="musttitle" style="width:9%"  >
                             单位
                        </td>
                        <td style="width:32%;" >
                             &nbsp;<asp:TextBox ID="txtUnit" runat="server"   
                                 BorderWidth="0" CssClass="TextRead" Width="69%" ReadOnly="True" ></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        
                         <td align="center" class=" smalltitle" style="width:9%"   >
                             WMS产品型号
                             <asp:TextBox ID="txtProduct" runat="server"   
                                 BorderWidth="0" CssClass="TextRead" Width="0px" Height="0px" ></asp:TextBox>
                        </td>
                        <td class="ItemStyle">
                             &nbsp;<asp:TextBox ID="txtProductModel" runat="server" BorderWidth="0" 
                                 CssClass="TextRead" Height="16px"  Width="69%" ReadOnly="True"></asp:TextBox>
                             &nbsp;&nbsp;&nbsp;</td>
                         <td  align="center" style="width:10%" class="smalltitle" >
                             规格<asp:TextBox ID="txtColorID" runat="server" 
                                 Width="0px" CssClass="TextRead"  MaxLength="1"></asp:TextBox>
                        </td>
                        <td>
                         &nbsp;<asp:TextBox ID="txtColorName" runat="server" BorderWidth="0" Width="69%" 
                                CssClass="TextRead" ReadOnly="True"></asp:TextBox>&nbsp;</td>
                         <td align="center" style="width:9%" class="smalltitle" >
                            排列顺序
                         </td>
                         <td>
                         &nbsp;<asp:TextBox ID="txtOrderNum" runat="server"  CssClass="TextRead" 
                                 Width="69%" 
                                 
                                 onkeypress="return regInput(this,/^\d+$/,String.fromCharCode(event.keyCode))" 	onpaste="return regInput(this,/^\d+$/,window.clipboardData.getData('Text'))" 
                                        
                                 ondrop="return regInput(this,/^\d+$/,event.dataTransfer.getData('Text'))" 
                                 ReadOnly="True" BorderWidth="0px" ></asp:TextBox>
                         </td>
                    </tr>
                        
                    <tr>
                        <td align="center" style="width:9%" class="smalltitle"  >
                             子公司直营价格 
                        </td>
                        <td class="ItemStyle" >
                             &nbsp;<asp:TextBox ID="txtPrice1" runat="server"   
                                 BorderWidth="0" CssClass="TextRead" Width="69%" style="text-align:right;" 
                                        
                                 
                                 onkeypress="return regInput(this,/^\d+$/,String.fromCharCode(event.keyCode))" 	onpaste="return regInput(this,/^\d+$/,window.clipboardData.getData('Text'))" 
                                        
                                 ondrop="return regInput(this,/^\d+$/,event.dataTransfer.getData('Text'))" 
                                 ReadOnly="True" ></asp:TextBox>
                        </td>
                         <td  align="center" class="smalltitle" >
                             子公司经销商价格
                        </td>
                        <td>
                         &nbsp;<asp:TextBox 
                                ID="txtPrice2" runat="server"  
                                CssClass="TextRead" Width="69%" style="text-align:right;" 
                                        
                                
                                
                                onkeypress="return regInput(this,/^\d+$/,String.fromCharCode(event.keyCode))" 	onpaste="return regInput(this,/^\d+$/,window.clipboardData.getData('Text'))" 
                                        
                                ondrop="return regInput(this,/^\d+$/,event.dataTransfer.getData('Text'))" 
                                ReadOnly="True" BorderWidth="0px"></asp:TextBox>
                        </td>
                        <td align="center" style="width:9%" class="smalltitle"  >
                             区域经销商价格</td>
                        <td style="width:32%;" >
                             &nbsp;<asp:TextBox ID="txtPrice3" runat="server"   
                                 BorderWidth="0" CssClass="TextRead" Width="69%" style="text-align:right;" 
                                        
                                 
                                 onkeypress="return regInput(this,/^\d+$/,String.fromCharCode(event.keyCode))" 	onpaste="return regInput(this,/^\d+$/,window.clipboardData.getData('Text'))" 
                                        
                                 ondrop="return regInput(this,/^\d+$/,event.dataTransfer.getData('Text'))" 
                                 ReadOnly="True" ></asp:TextBox>
                        </td>
                    </tr>
                     <tr>
                        <td align="center" style="width:9%" class="smalltitle"  >
                             网渠价格
                        </td>
                        <td class="ItemStyle" >
                             &nbsp;<asp:TextBox ID="txtPrice4" runat="server"   
                                 BorderWidth="0" CssClass="TextRead" Width="69%" style="text-align:right;" 
                                        
                                 
                                 onkeypress="return regInput(this,/^\d+$/,String.fromCharCode(event.keyCode))" 	onpaste="return regInput(this,/^\d+$/,window.clipboardData.getData('Text'))" 
                                        
                                 ondrop="return regInput(this,/^\d+$/,event.dataTransfer.getData('Text'))" 
                                 ReadOnly="True"></asp:TextBox>
                        </td>
                         <td  align="center" class="smalltitle" >
                             其它价格</td>
                        <td>
                         &nbsp;<asp:TextBox 
                                ID="txtPrice5" runat="server"  BorderWidth="0"
                                CssClass="TextRead" Width="69%" style="text-align:right;" 
                                        
                                
                                onkeypress="return regInput(this,/^\d+$/,String.fromCharCode(event.keyCode))" 	onpaste="return regInput(this,/^\d+$/,window.clipboardData.getData('Text'))" 
                                        
                                ondrop="return regInput(this,/^\d+$/,event.dataTransfer.getData('Text'))" 
                                ReadOnly="True"></asp:TextBox>
                        </td>
                        <td align="center" style="width:9%" class="smalltitle"  >
                             成本价格</td>
                        <td style="width:32%;" >
                             &nbsp;<asp:TextBox ID="txtStandPrice" runat="server"   
                                 BorderWidth="0" CssClass="TextRead" Width="69%" style="text-align:right;" 
                                        
                                 
                                 onkeypress="return regInput(this,/^\d+$/,String.fromCharCode(event.keyCode))" 	onpaste="return regInput(this,/^\d+$/,window.clipboardData.getData('Text'))" 
                                        
                                 ondrop="return regInput(this,/^\d+$/,event.dataTransfer.getData('Text'))" 
                                 ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center"  style="width:9%" class="smalltitle">
                            备注
                         </td>
                        <td colspan="5">
                            &nbsp;<asp:TextBox ID="txtMemo" runat="server" CssClass="TextRead" 
                                TextMode="MultiLine" Height="102px" Width="89%" ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>		
			</table>
			</ContentTemplate>
            </asp:UpdatePanel>
		</form>
</body>
</html>
