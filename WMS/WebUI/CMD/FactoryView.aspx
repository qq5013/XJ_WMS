<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FactoryView.aspx.cs" Inherits="WMS.WebUI.CMD.FactoryView" %>
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
                            OnClientClick="return ViewEdit();"  />
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
                             供应商编码
                        </td>
                        <td colspan="2">
                             &nbsp;<asp:TextBox ID="txtID" runat="server"   
                                 BorderWidth="0" CssClass="TextRead" Width="46%" MaxLength="10" 
                                 ReadOnly="True" ></asp:TextBox>
                        </td>
                         <td  align="center" class="musttitle" style="width:8%;" >
                             供应商名称
                        </td>
                        <td>
                         &nbsp;<asp:TextBox 
                                ID="txtFactoryName" runat="server"  BorderWidth="0"
                                CssClass="TextRead" Width="44%" ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" class="musttitle" style="width:8%;"  >
                             联系人
                        </td>
                        <td colspan="2">
                             &nbsp;<asp:TextBox ID="txtLinkPerson" runat="server"   
                                 BorderWidth="0" CssClass="TextRead" Width="46%" ReadOnly="True" ></asp:TextBox>
                        </td>
                         <td  align="center" class="musttitle" style="width:8%;" >
                             联系电话
                        </td>
                        <td>
                         &nbsp;<asp:TextBox 
                                ID="txtLinkPhone" runat="server"  BorderWidth="0"
                                CssClass="TextRead" Width="44%" ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>
                    
                    <tr>
                        <td align="center" class="smalltitle" style="width:8%;"  >
                             传真
                        </td>
                        <td style=" width:20%">
                            &nbsp;<asp:TextBox ID="txtFax" runat="server"   
                                 BorderWidth="0" CssClass="TextRead" Width="65%" ReadOnly="True" ></asp:TextBox>
                        </td>
                        <td  align="center" style=" width: 8%;" class="smalltitle"  >
                             地址
                        </td>
                        <td colspan="2">
                             &nbsp;<asp:TextBox ID="txtAddress" runat="server"   
                                 BorderWidth="0" CssClass="TextRead" Width="51%" Height="16px" 
                                 ReadOnly="True"></asp:TextBox>
                        </td>
                         
                    </tr>
                    <tr>
                        <td align="center"  class="smalltitle" style="width:8%;">
                            备注
                        </td>
                        <td colspan="4">
                            &nbsp;<asp:TextBox ID="txtMemo" runat="server" CssClass="TextRead" 
                                TextMode="MultiLine" Height="102px" Width="66%" ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>		
			</table>
			</ContentTemplate>
            </asp:UpdatePanel>
		</form>
</body>
</html>
