<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductView.aspx.cs" Inherits="WMS.WebUI.CMD.ProductView" %>
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
                        <td class="musttitle" align="center" width="15%" >
                            <asp:Literal ID="ltlWarehouseID" Text="产品编号"
                                runat="server" ></asp:Literal>
                        </td>
						<td >&nbsp;<asp:textbox id="txtID" runat="server" CssClass="TextRead" Width="91%" 
                                MaxLength="7" ReadOnly="True" ></asp:textbox>
                        </td>
                        <td align="center" class="musttitle" style="width:16%">
                            产品部件</td>
						<td width="21%">
                             &nbsp;<asp:textbox id="txtPRODUCT_PartsName" 
                                 runat="server" CssClass="TextRead" Width="91%" ReadOnly="True" ></asp:textbox>
                             </td>
						
                        <td align="center" class="smalltitle" style="width:13%">
                            产品类别</td>
                        <td>&nbsp;<asp:textbox ID="txtPRODUCT_Class" runat="server" Width = "91%" 
                                CssClass="TextRead" ReadOnly="True"></asp:textbox>
                            </td>
                    </tr>
                    <tr>
                        <td class="musttitle" align="center" width="15%" >
                            产品名称</td>
						<td >&nbsp;<asp:textbox id="txtPRODUCT_NAME" runat="server" CssClass="TextRead" 
                                Width="91%" ReadOnly="True" ></asp:textbox>
                        </td>
                        <td align="center" class="musttitle" style="width:13%">
                            产品型号</td>
						<td width="21%" >
                            &nbsp;<asp:textbox id="txtPRODUCT_MODEL" 
                                runat="server" CssClass="TextRead" Width="91%" ReadOnly="True" ></asp:textbox>
                             </td>
						
                        <td align="center" class="musttitle" style="width:13%">
                            工厂型号</td>
                        <td>&nbsp;<asp:textbox id="txtPRODUCT_FMODEL" runat="server" CssClass="TextRead" 
                                Width="91%" ReadOnly="True" ></asp:textbox>
                        </td>
                    </tr>
                    <tr>
                        <td class="smalltitle" align="center" width="15%" >
                            <asp:Literal ID="Literal3" Text="每箱数量"
                                runat="server" ></asp:Literal>
                        </td>
						<td style="width:17%">&nbsp;<asp:textbox id="txtPACK_QTY" runat="server" 
                                CssClass="TextRead" Width="91%" ReadOnly="True" 
                                ></asp:textbox>
                        </td>
                        <td  style="width:15%" align="center" class="smalltitle">&nbsp;
								<asp:Literal 
                                ID="Literal4" Text="托盘数量" runat="server" ></asp:Literal>
								</td>
						<td style="width:18%">
								&nbsp;<asp:textbox id="txtPALLET_QTY" runat="server" 
                                    CssClass="TextRead" Width="91%" ReadOnly="True" 
                                 ></asp:textbox>
                        </td>
						 <td  colspan="2" align="center" >
                             <asp:CheckBox ID="chkIS_MIX" runat="server" Text="混合组盘" Enabled="False" />
                             &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; 
								<asp:CheckBox ID="chkIsBarCode" runat="server" Text="扫描条码" Enabled="False" />
                                &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; 
                                <asp:CheckBox ID="chkIsInStock" runat="server" Text="是否入库" Enabled="False" />
								</td>
                        
                    </tr>
                     <tr>
                        <td class="smalltitle" align="center" width="15%" >
                            <asp:Literal ID="Literal1" Text="每箱体积"
                                runat="server" ></asp:Literal>
                        </td>
						<td style="width:17%">&nbsp;<asp:textbox id="txtProductVolume" runat="server" 
                                CssClass="TextRead" Width="91%" style="text-align:right"
                                onkeypress="return regInput(this,/^\d*\.?\d{0,6}$/,String.fromCharCode(event.keyCode)) ;" 
                                ondrop="return regInput(this,/^\d*\.?\d{0,6}$/,event.dataTransfer.getData('Text'));" 
                                
                                onpaste="return regInput(this,/^\d*\.?\d{0,6}$/,window.clipboardData.getData('Text'));" ReadOnly="True" 
                                >0</asp:textbox>
                        </td>
                        <td align="center" style="width:16%"   >&nbsp;
								 
								</td>
						<td style="width:18%">
							 
                        </td>
						 <td  colspan="2" align="center" >
                             
						 </td>
                        
                    </tr>  
                    <tr>
                        <td class="smalltitle" align="center" width="15%" >
                            <asp:Literal ID="Literal7" Text="备注"
                                runat="server" ></asp:Literal>
                        </td>
						<td  colspan="5">&nbsp;<asp:TextBox ID="txtMemo" runat="server" CssClass="TextRead" 
                                TextMode="MultiLine" Height="102px" Width="96%" ReadOnly="True"></asp:TextBox>
                        </td>
						 
                    </tr>

                    <tr>
                        <td colspan="6">
                            <b> 名称解释：</b>     
                        </td>
						 
						 
                    </tr>
                    <tr>
                        <td class="smalltitle" align= "center" width="15%" > 
                           栏目名称
                         </td>
                         <td class="smalltitle" align= "center" width="15%">
                         填写范本
                         </td>
						<td class="smalltitle" align= "center"  colspan="4"> 
                               规则及填写说明
                        </td>
						 
                    </tr>
                    <tr>
                        <td class="smalltitle" align="right" width="15%" >
                             
                            产品编号：</td>
                         <td>
                         
                             7558231</td>
						<td  colspan="4"> 
                                4位型号编码+1位版本号+1位总箱数+1位分箱号，不区分颜色&nbsp;</td>
						 
                    </tr>
                     <tr>
                        <td class="smalltitle" align="right" width="15%" >
                             
                            产品部件：</td>
						<td>
                         
                            大师椅（2015版）-椅架</td>
						<td  colspan="4">
                                小件整体包装的产品品名同产品名称一致。分体包装的按摩椅填写分箱部件的名称，如大师椅（2015版）-椅架,大师椅（2015版）-侧板，大师椅（2015版）-靠背。&nbsp;</td>
						 
                    </tr>
                     <tr>
                        <td class="smalltitle" align="right" width="15%" >
                             
                            产品名称：</td>
						<td>
                         
                            大师椅（2015版）</td>
						<td  colspan="4">
                                &nbsp;指的是产品的具体名称。如 7588133，7588131 ，7588132 的产品名称为大师椅，020111的产品名称为 森活家
                        </td>
						 
                    </tr>
                    <tr>
                        <td class="smalltitle" align="right" width="15%" >
                             
                            产品类别：</td>
						<td>
                         
                           按摩椅
                        </td>
						<td  colspan="4">
                               分箱包装的大件产品，<b style="color:Red">各个部件的产品类别应相同</b>，如大师椅（2015版）-椅架,大师椅（2015版）-侧板，大师椅（2015版）-靠背 都为按摩椅
                        </td>
						 
                    </tr>
                     <tr>
                        <td class="smalltitle" align="right" width="15%" >
                             
                            每箱数量：</td>
						<td>
                         
                            1</td>
						<td  colspan="4">
                                &nbsp;每个箱子包装产品的数量
                        </td>
						 
                    </tr>
                     <tr>
                        <td class="smalltitle" align="right" width="15%" >
                             
                            托盘数量：</td>
						<td>
                         
                            1</td>
						<td  colspan="4"> 
                                &nbsp;一个托盘组盘时能够扫描产品的数量
                        </td>
						 
                    </tr>
                    <tr>
                        <td class="smalltitle" align="right" width="15%" >
                             
                            混合组盘：</td>
						<td>
                         
                            是</td>
						<td  colspan="4"> 
                                &nbsp;产品由多个部件组成时，一个多盘能否包含多个部件。如：7588133大师椅靠背，7588131大师椅椅架组盘时存放在同一个托盘上，则这两个部件都为混合组盘，而7588132大师椅侧板不能混合组盘。
                        </td>
						 
                    </tr>
                    <tr>
                        <td class="smalltitle" align="right" width="15%" >
                             
                            条码扫描：</td>
						<td>
                         
                            是</td>
						<td  colspan="4">
                                &nbsp;产品是否需要扫描条码，或者该产品作为无条码产品。组盘或者出库时，只需输入数量，无需扫描条码。
                        </td>
						 
                    </tr>
                     <tr>
                        <td class="smalltitle" align="right" width="15%" >
                             
                            是否入库：</td>
						<td>
                         
                            是</td>
						<td  colspan="4">
                                &nbsp;产品由多个部件组成时出入库可排除部分部件由人工自己出库，如：7588133大师椅靠背，7588131大师椅椅架 需要扫描出入库，而7588132大师椅侧板不需要扫描出入库。
                        </td>
						 
                    </tr>
			</table>
			</ContentTemplate>
            </asp:UpdatePanel>
		</form>
</body>
</html>
