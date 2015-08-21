<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerView.aspx.cs" Inherits="WMS.WebUI.CMD.CustomerView" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
      <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />  
        <title></title> 
        <link href="~/Css/Main.css" type="text/css" rel="stylesheet" /> 
        <link href="~/Css/op.css" type="text/css" rel="stylesheet" /> 
         <script type="text/javascript" src='<%=ResolveClientUrl("~/JQuery/jquery-1.8.2.min.js") %>'></script> 
         <script type="text/javascript" src='<%=ResolveClientUrl("~/JScript/Common.js") %>'></script>
        <script type="text/javascript" src='<%=ResolveClientUrl("~/JScript/Ajax.js") %>'></script>        

      <style type="text/css">
          .style1
          {
              width: 34%;
          }
          .style2
          {
              width: 35%;
          }
      </style>

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
                                     客户编码
                                </td>
                                <td class="style2">
                                     &nbsp;<asp:TextBox ID="txtID" runat="server"   
                                         BorderWidth="0" CssClass="TextRead" Width="46%" ReadOnly="True" ></asp:TextBox>
                                </td>
                                 <td  align="center" class="musttitle" style="width:8%;" >
                                     客户名称
                                </td>
                                <td>
                                 &nbsp;<asp:TextBox 
                                        ID="txtCustomer_Name" runat="server"  BorderWidth="0"
                                        CssClass="TextRead" Width="42%" ReadOnly="True"></asp:TextBox>
                                </td>
                            </tr>
                             <tr>
                                <td align="center" class="smalltitle" style="width:8%;"  >
                                        子公司
                                </td>
                                <td class="style2"  >
                                        &nbsp;<asp:TextBox 
                                            ID="txtPLAYCUSTOMER_CODE" runat="server"   
                                            BorderWidth="0" CssClass="TextBox" Width="0px" ></asp:TextBox>
                                            <asp:TextBox ID="txtPlayCustomer_Name" runat="server"  BorderWidth="0"
                                        CssClass="TextRead" Width="95%" ReadOnly="True"></asp:TextBox>
                                </td>
                                    <td  align="center" class="musttitle" style="width:8%;" >
                                        传真
                                </td>
                                <td>
                                    &nbsp;<asp:TextBox 
                                        ID="txtFax" runat="server"  BorderWidth="0"
                                        CssClass="TextRead" Width="42%" ReadOnly="True"></asp:TextBox>
                                </td>
                            </tr>
                             <tr>
                                    <td align="center"  class="smalltitle" style="width:8%;">
                                        公司类别
                                    </td>
                                    <td class="style2"  >
                                        &nbsp;<asp:RadioButton ID="opt1" runat="server" Checked="True" 
                                GroupName="LabelSource" Text="子公司直营" Enabled="False" />&nbsp;&nbsp;&nbsp;<asp:RadioButton ID="opt2" 
                                            runat="server" GroupName="LabelSource" 
                                Text="子公司经销商" Enabled="False" />&nbsp;<asp:RadioButton ID="opt3" 
                                            runat="server" GroupName="LabelSource" 
                                Text="区域经销商" Enabled="False" />&nbsp;<asp:RadioButton ID="opt4" 
                                            runat="server" GroupName="LabelSource" 
                                Text="网渠" Enabled="False" />&nbsp;<asp:RadioButton ID="opt5" runat="server" GroupName="LabelSource" 
                                Text="其它" Enabled="False" />             
                                    </td>
                                 <td  align="center" class="smalltitle" style="width:8%;" >
                                         归属地区
                                    </td>
                                    <td>
                                     &nbsp;<asp:TextBox 
                                            ID="txtAreaSation" runat="server"  BorderWidth="0"
                                            CssClass="TextRead" Width="42%" ReadOnly="True"></asp:TextBox>
                                    </td>
                                </tr>
                            <tr>
                                <td align="center" class="musttitle" style="width:8%;"  >
                                     联络人
                                </td>
                                <td class="style2">
                                     &nbsp;<asp:TextBox ID="txtCustomer_Person" runat="server"   
                                         BorderWidth="0" CssClass="TextRead" Width="95%" ReadOnly="True" ></asp:TextBox>
                                </td>
                                 <td  align="center" class="musttitle" style="width:8%;" >
                                     联络电话
                                </td>
                                <td>
                                 &nbsp;<asp:TextBox 
                                        ID="txtCustomer_Phone" runat="server"  BorderWidth="0"
                                        CssClass="TextRead" Width="42%" ReadOnly="True"></asp:TextBox>
                                </td>
                            </tr>
                            	
                            <tr>
                                <td align="center"  class="smalltitle" style="width:8%;">
                                    备注
                                </td>
                                <td colspan="3">
                                    &nbsp;<asp:TextBox ID="txtMemo" runat="server" CssClass="TextRead" 
                                        TextMode="MultiLine" Height="102px" Width="69%" ReadOnly="True"></asp:TextBox>
                                </td>
                            </tr>		
			        </table>
                    
                    <div style="overflow: auto; width: 100%; height: 320px" >
                        <asp:GridView ID="dgViewSub1" runat="server" AutoGenerateColumns="False" SkinID="GridViewSkin"
                            Width="100%"  >
                            <Columns>
                                <asp:BoundField DataField="RowID" HeaderText="序号" >
                                    <ItemStyle HorizontalAlign="Center" Width="4%" Wrap="False" />
                                    <HeaderStyle Wrap="False" />
                                </asp:BoundField>
                                 <asp:BoundField DataField="PERSON" HeaderText="收货人" >
                                    <ItemStyle HorizontalAlign="left" Width="9%" Wrap="False" />
                                    <HeaderStyle Wrap="False" />
                                </asp:BoundField>
                                 <asp:BoundField DataField="PHONE" HeaderText="收货电话" >
                                    <ItemStyle HorizontalAlign="left" Width="12%" Wrap="False" />
                                    <HeaderStyle Wrap="False" />
                                </asp:BoundField>
                                 <asp:BoundField DataField="Address" HeaderText="收货地址" >
                                    <ItemStyle HorizontalAlign="left" Width="30%" Wrap="False" />
                                    <HeaderStyle Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField DataField="OrderNum" HeaderText="排序" >
                                    <ItemStyle HorizontalAlign="left" Width="10%" Wrap="False" />
                                    <HeaderStyle Wrap="False" />
                                </asp:BoundField>
                            </Columns>
                            <PagerSettings Visible="false" />
                        </asp:GridView>
                    </div>
			    </ContentTemplate>
            </asp:UpdatePanel>
		</form>
</body>
</html>
