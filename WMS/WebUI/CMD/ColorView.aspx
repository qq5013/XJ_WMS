<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ColorView.aspx.cs" Inherits="WMS.WebUI.CMD.ColorView" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
      <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />  
        <title></title> 
        <link href="~/Css/Main.css" type="text/css" rel="stylesheet" /> 
        <link href="~/Css/op.css" type="text/css" rel="stylesheet" /> 
         <script type="text/javascript" src='<%=ResolveClientUrl("~/JQuery/jquery-1.8.2.min.js") %>'></script> 
         <script type="text/javascript" src='<%=ResolveClientUrl("~/JScript/Common.js") %>'></script>
        <script type="text/javascript" src='<%=ResolveClientUrl("~/JScript/Ajax.js") %>'></script>        

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
                        <asp:Button ID="btnFirst" runat="server" Text="首笔" CssClass="ButtonFirst" 
                            onclick="btnFirst_Click"  />
                        <asp:Button ID="btnPre" runat="server" Text="上一笔" CssClass="ButtonPre" 
                            onclick="btnPre_Click"  />
                        <asp:Button ID="btnNext" runat="server" Text="下一笔" CssClass="ButtonNext" 
                            onclick="btnNext_Click"  />
                        <asp:Button ID="btnLast" runat="server" Text="尾笔" CssClass="ButtonLast" 
                            onclick="btnLast_Click"  />
                    </td>
                    <td align="right">
                       <%-- <asp:Button ID="btnPrint" runat="server" Text="导出" CssClass="ButtonPrint" 
                            OnClientClick="return print();" />--%>
                        <asp:Button ID="btnAdd" runat="server" Text="新增" CssClass="ButtonCreate" 
                            OnClientClick="Add()"  />
                        <asp:Button ID="btnDelete" runat="server" Text="刪除" CssClass="ButtonDel" OnClientClick="return ViewDelete();"
                            onclick="btnDelete_Click"  />
                        <asp:Button ID="btnEdit" runat="server" Text="修改" CssClass="ButtonModify" 
                            OnClientClick="return ViewEdit();" />
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
                        <td align="center" class="musttitle" style="width:8%;"  >
                             规格编码
                        </td>
                        <td style="width:30%;" >
                             &nbsp;<asp:TextBox ID="txtColorID" runat="server"   
                                 BorderWidth="0" CssClass="TextRead" Width="47%" ReadOnly="True"></asp:TextBox>
                                  <asp:TextBox ID="txtID" runat="server"   
                                 BorderWidth="0" CssClass="TextBox" Width="0px" Height="0px"   ></asp:TextBox>
                        </td>
                         <td  align="center" class="musttitle" style="width:8%;" >
                             规格名称
                        </td>
                        <td>
                         &nbsp;<asp:TextBox 
                                ID="txtColor_Name" runat="server"  BorderWidth="0" width="65%"
                                CssClass="TextRead" ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" class="musttitle" style="width:8%;"  >
                             产品编码
                        </td>
                        <td colspan="3">
                             &nbsp;<asp:TextBox 
                                 ID="txtProductModel" runat="server"   
                                 BorderWidth="0" CssClass="TextRead" Width="15%" 
                                 ReadOnly="True" ></asp:TextBox>&nbsp;<asp:TextBox 
                                 ID="txtProductCode" runat="server"   
                                 BorderWidth="0" CssClass="TextRead"  style="display:none" ReadOnly="True" ></asp:TextBox>
                             <asp:TextBox 
                                ID="txtProductName" runat="server"  BorderWidth="0"
                                CssClass="TextRead" Width="64%" ReadOnly="True"></asp:TextBox><asp:TextBox 
                                 ID="txtProduct" runat="server"   
                                 BorderWidth="0" CssClass="TextRead" Width="0px" 
                                 ReadOnly="True" ></asp:TextBox>
                        </td>
                         
                    </tr>
                    <tr>
                        <td align="center"  class="smalltitle" style="width:8%;">
                            备注
                        </td>
                        <td colspan="3">
                            &nbsp;<asp:TextBox ID="txtMemo" runat="server" CssClass="TextRead" 
                                TextMode="MultiLine" Height="102px" Width="80%" ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>			
			</table>
			</ContentTemplate>
            </asp:UpdatePanel>
		</form>
</body>
</html>
