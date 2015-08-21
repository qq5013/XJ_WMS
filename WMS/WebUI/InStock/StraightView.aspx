<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StraightView.aspx.cs" Inherits="WMS.WebUI.InStock.StraightView" %>
 
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
      <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />  
        <title></title> 
        <link href="~/Css/Main.css" type="text/css" rel="stylesheet" /> 
        <link href="~/Css/op.css" type="text/css" rel="stylesheet" /> 
         <script type="text/javascript" src='<%=ResolveClientUrl("~/JQuery/jquery-1.8.3.min.js") %>'></script>
         <script type="text/javascript" src='<%=ResolveClientUrl("~/JScript/Common.js") %>'></script>
        <script type="text/javascript" src='<%=ResolveClientUrl("~/JScript/Ajax.js") %>'></script>        
         <script type="text/javascript">
             function content_resize() {

                 //編輯頁面 div高度
//                 var div = $("#surdiv");
//                 var h = 300;
//                 if ($(window).height() <= 0) {
//                     h = document.body.clientHeight -35;
//                 }
//                 else {
//                     h = $(window).height() -35;
//                 }
//                 $("#surdiv").css("height", h);

//                 $("#Sub-container").css("height", h - 195); //设置S界面多明细设置  

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
                        <asp:Button ID="btnPrint" runat="server" Text="导出" CssClass="ButtonPrint" 
                             onclick="btnPrint_Click" />
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
             <div id="surdiv" style="overflow: auto">
			    <table id="Table1" class="maintable" cellspacing="0" cellpadding="0" width="100%" align="center" bordercolor="#ffffff"
				        border="1" runat="server">			
					        <tr>
                                <td align="center" class="musttitle" style=" width: 8%">
                                        直出日期
                                </td>
                                <td  style="width:15%">
                                        &nbsp;<asp:TextBox ID="txtBillDate" runat="server" BorderWidth="0" CssClass="TextRead" 
                                            MaxLength="20" ReadOnly="True" Width="65%"></asp:TextBox>
                                </td>
                                <td  align="center" class="musttitle" style="width:8%;" >
                                        直出单号
                                </td>
                                <td style="width:15%" >
                                    &nbsp;<asp:TextBox ID="txtID" runat="server" BorderWidth="0" CssClass="TextRead" 
                                        MaxLength="20" Width="90%"></asp:TextBox>
                                          
                                </td>
                                  
                                <td align="center" class="musttitle" style="width:8%" >
                                    发货通知单</td>
                                  <td  style="width:18%">
                                        &nbsp; <asp:TextBox ID="txtSourceNo" runat="server" BorderWidth="0" 
                                             CssClass="TextRead" Height="16px" ReadOnly="True" Width="91%"></asp:TextBox>
                                </td>
                                 <td align="center" class="musttitle" style="width:8%" >
                                     销售单号
                                </td>
                                  <td  style="width:18%">
                                          &nbsp;<asp:TextBox ID="txtScheduleNo" runat="server" BorderWidth="0" CssClass="TextRead" 
                                        Width="91%" Height="16px" ReadOnly="True"></asp:TextBox>
                                </td>
                               
                            </tr>
                             <tr>
                                <td align="center" style="width:8%;" class="musttitle">
                                        经销商</td>
                                <td colspan="3" >
                                       &nbsp;<asp:TextBox ID="txtCustomerName" runat="server" BorderWidth="0" 
                                            CssClass="TextRead" Height="16px" Width="96%" ReadOnly="True"></asp:TextBox>
                                </td>
                               
                                
                                <td style="width:8%;" align="center" class="smalltitle" >
                                        收货人</td>
                                    <td  >
                                         &nbsp;<asp:TextBox ID="txtLinkPerson" runat="server" BorderWidth="0" 
                                             CssClass="TextRead" Height="16px" Width="91%" ReadOnly="True"></asp:TextBox>
                                </td>
                                <td align="center" class="smalltitle" style="width:8%">
                                    收货电话 
                                </td>
                                <td>
                                    &nbsp;<asp:TextBox ID="txtLinkPhone" runat="server" BorderWidth="0" 
                                        CssClass="TextRead" Height="16px" ReadOnly="True" Width="91%"></asp:TextBox>
                                </td>
                                
                                
                            </tr>
                           <tr>
                                <td align="center" style="width:8%;" class="smalltitle">
                                        收货地址
                                   
                                </td> 
                                <td  colspan="3">
                                    &nbsp;<asp:TextBox ID="txtLinkAddress" runat="server" BorderWidth="0" 
                                        CssClass="TextRead" Height="16px" Width="97%" ReadOnly="True"></asp:TextBox>
                                 </td>
                                    <td align="center" class="smalltitle" style="width:8%;">
                                        建单人员</td> 
                                <td    style="width:18%">
                                       &nbsp;<asp:TextBox ID="txtCreator" runat="server" BorderWidth="0" CssClass="TextRead" 
                                           Height="16px" Width="91%"></asp:TextBox>
                                </td>
                                <td align="center" class="smalltitle" style="width:8%;">
                                         建单日期</td> 
                                <td  style="width:18%">
                                     &nbsp;<asp:TextBox ID="txtCreateDate" runat="server" BorderWidth="0" 
                                          CssClass="TextRead" Height="16px" Width="92%"></asp:TextBox>
                                </td>
                                        
                               
                            </tr>
                            
                           
                           
                             
                             <tr>
                                <td align="center"  style="width:8%;" class="smalltitle">
                                     &nbsp;备注
                                 
                                </td>
                                <td colspan="7">
                                    &nbsp;<asp:TextBox ID="txtMemo" runat="server" CssClass="TextRead" 
                                        TextMode="MultiLine" Height="51px" Width="99%" ReadOnly="True"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="center"  style="width:8%;" class="smalltitle">
                                    审核人员
                                 </td>
                                <td  style="width:15%;" >
                                     &nbsp;<asp:TextBox ID="txtCheckUser" runat="server" BorderWidth="0" 
                                        CssClass="TextRead" Height="16px" Width="89%" ReadOnly="True"></asp:TextBox>
                                    
                                </td>
                                    <td align="center" class="smalltitle"  >
                                        审核日期
                                </td>
                                    <td  style="width:15%;">
                                         &nbsp;<asp:TextBox ID="txtCheckDate" runat="server" BorderWidth="0" 
                                            CssClass="TextRead" Height="16px" Width="93%" ReadOnly="True"></asp:TextBox>
                                </td>
                                <td colspan="4" align="center">
                                    <asp:TextBox ID="txtState" runat="server" BorderWidth="0" 
                                            CssClass="TextRead" Height="0px" Width="0px"></asp:TextBox>
                                    <asp:Button ID="btnCheck" runat="server" CssClass="but" Height="23px" 
                                        onclick="btnCheck_Click" Text="审核" Width="80px" />
                                    &nbsp;&nbsp;
                                    <asp:Button ID="btnCheck0" runat="server" CssClass="but" Height="23px" 
                                        onclick="btnCheck0_Click" Text="转入K3" Width="80px" />
                                    
                                </td>
                                   
                            </tr>	
                             		
			        </table>
			      
               
                  <div id="Sub-container" style="overflow: auto; width: 100%; height: 320px" >
                        <asp:GridView ID="dgViewSub1" runat="server" AutoGenerateColumns="False" SkinID="GridViewSkin"
                            AllowPaging="True" Width="98%" PageSize="10" onrowdatabound="dgViewSub1_RowDataBound" >
                            <Columns>
                             <asp:BoundField DataField="RowID" HeaderText="序号" >
                                <ItemStyle HorizontalAlign="Left" Width="9%" Wrap="False" />
                                <HeaderStyle Wrap="False" Width="9%" ForeColor="Blue" />
                             </asp:BoundField>
                            <asp:BoundField DataField="ProductName" HeaderText="产品名称" >
                                <ItemStyle HorizontalAlign="Left" Width="20%" Wrap="False" />
                               <HeaderStyle Wrap="False" Width="8%" ForeColor="Blue" />
                            </asp:BoundField>  
                            <asp:BoundField DataField="ProductModel" HeaderText="产品型号" >
                                <ItemStyle HorizontalAlign="Left" Width="9%" Wrap="False" />
                                <HeaderStyle Wrap="False" Width="9%" ForeColor="Blue" />
                            </asp:BoundField>
                            
                            
                            <asp:BoundField DataField="ColorName" HeaderText="规格" >
                                <ItemStyle HorizontalAlign="Left" Width="10%" Wrap="False" />
                                <HeaderStyle Wrap="False" Width="10%" ForeColor="Blue" />
                            </asp:BoundField>
                             <asp:BoundField DataField="PlanQty" HeaderText="数量" >
                                <ItemStyle HorizontalAlign="Right" Width="8%" Wrap="False" />
                                <HeaderStyle Wrap="False" Width="8%" ForeColor="Blue" />
                            </asp:BoundField>

                            <asp:BoundField DataField="StarNo" HeaderText="起始序号" >
                                <ItemStyle HorizontalAlign="Left" Width="10%" Wrap="False" />
                                <HeaderStyle Wrap="False" Width="10%" ForeColor="Blue" />
                            </asp:BoundField>
                            <asp:BoundField DataField="EndNo" HeaderText="结束序号" >
                                <ItemStyle HorizontalAlign="Left" Width="10%" Wrap="False" />
                                <HeaderStyle Wrap="False" Width="10%" ForeColor="Blue" />
                            </asp:BoundField>

                            </Columns>
                        
                            <PagerSettings Visible="false" />
                    </asp:GridView>
                 </div>
             
                    <table width="100%" class="maintable"  bordercolor="#ffffff" border="1"> 
                        
                        <tr>
                             
                            <td align="right">
                             <asp:Label ID="lblCurrentPageSub1" runat="server" ></asp:Label>&nbsp;&nbsp; 
                                <asp:LinkButton ID="btnFirstSub1" runat="server"  
                                            Text="首页" onclick="btnFirstSub1_Click"></asp:LinkButton> 
                                &nbsp;<asp:LinkButton ID="btnPreSub1" runat="server"  
                                        Text="上一页" onclick="btnPreSub1_Click"></asp:LinkButton> 
                               
                                &nbsp;<asp:LinkButton ID="btnNextSub1" runat="server" 
                                        Text="下一页" onclick="btnNextSub1_Click"></asp:LinkButton> 
                                &nbsp;<asp:LinkButton ID="btnLastSub1" runat="server"  
                                            Text="尾页" onclick="btnLastSub1_Click"></asp:LinkButton> 
                        &nbsp;<asp:textbox id="txtPageNoSub1" onkeypress="return regInput(this,/^\d+$/,String.fromCharCode(event.keyCode))"
					                    onpaste="return regInput(this,/^\d+$/,window.clipboardData.getData('Text'))" ondrop="return regInput(this,/^\d+$/,event.dataTransfer.getData('Text'))"
					                    runat="server" Width="56px" CssClass="TextBox" >
                                        </asp:textbox>&nbsp;<asp:linkbutton id="btnToPageSub1" runat="server" Text="跳转" onclick="btnToPageSub1_Click"></asp:linkbutton>
                                    &nbsp;
                                <asp:Literal ID="Literal1" Text="每页" runat="server"  Visible="false" />
                                    <asp:DropDownList ID="ddlPageSizeSub1" runat="server" AutoPostBack="True"  
                                    Height="16px" onselectedindexchanged="ddlPageSizeSub1_SelectedIndexChanged" Visible="false" >
                    
                                    </asp:DropDownList>
                            </td>
                            <td width="3%">
                            </td>
                        </tr>
                   </table>
             </div>
			</ContentTemplate>
              <Triggers>  
                <asp:PostBackTrigger ControlID="btnPrint" />  
              </Triggers>
            </asp:UpdatePanel>
            <asp:HiddenField ID="HdnPDA" runat="server" />
		</form>
</body>
</html>
