<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ScheduleView.aspx.cs" Inherits="WMS.WebUI.InStock.ScheduleView" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
      <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />  
        <title></title> 
        <link href="~/Css/Main.css" type="text/css" rel="stylesheet" /> 
        <link href="~/Css/op.css" type="text/css" rel="stylesheet" /> 
        <link href="~/Css/Detail.css" type="text/css" rel="stylesheet" /> 
        <link rel="stylesheet" type="text/css" href="~/ext-3.3.1/resources/css/ext-all.css" />
        <script type="text/javascript" src='<%=ResolveClientUrl("~/ext-3.3.1/ext-base.js") %>'></script> 
        <script type="text/javascript" src='<%=ResolveClientUrl("~/ext-3.3.1/ext-all.js") %>'></script>
        <script type="text/javascript" src='<%=ResolveClientUrl("~/JQuery/jquery-1.8.3.min.js") %>'></script>
        <script type="text/javascript" src='<%=ResolveClientUrl("~/JScript/Common.js") %>'></script>
        <script type="text/javascript" src='<%=ResolveClientUrl("~/JScript/DataProcess.js") %>'></script>        
        <script type="text/javascript">
           function content_resize() {
               var activeTab = parseInt($('#HdfActiveTab').val());
               
               var tabPanel = new Ext.TabPanel({
                   height: 420,
                   width: "100%",
                   autoTabs: true, //自动扫描页面中的div然后将其转换为标签页
                   deferredRender: false, //不进行延时渲染
                   activeTab: activeTab, //默认激活第一个tab页
                   animScroll: true, //使用动画滚动效果
                   enableTabScroll: true, //tab标签超宽时自动出现滚动按钮
                   applyTo: 'tabs'
               });
              

           }


           function ScheduleSubEdit() {
               var returnvalue = window.showModalDialog('TempPage.aspx?ID=' + $('#txtID').val(), window, 'DialogHeight:600px;DialogWidth:1200px;help:no;scroll:auto;Resizable:yes');
               if (returnvalue == "1")
                   location.replace(location.href);
               return false;
           }

           function FormCopy() {
               var hrefs = location.href.split("/");
               var h = "/" + hrefs[3] + "/" + hrefs[4] + "/" + hrefs[5] + "/" + FormID + "Edit.aspx?SubModuleCode=" + SubModuleCode + "&FormID=" + FormID + "&ID=" + document.getElementById("txtID").value + "&FormCopy=1";
               window.parent.parent.mainFrame.addTab(h, SubModuleTitle, "tab_" + SubModuleCode + "_ADD");
//               location.href = FormID + "Edit.aspx?SubModuleCode=" + SubModuleCode + "&FormID=" + FormID + "&ID=" + document.getElementById("txtID").value + "&FormCopy=1";
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
                        <asp:Button ID="btnPrint" runat="server" Text="导出" CssClass="ButtonPrint" onclick="btnPrint_Click" 
                             />
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
                                <td align="center" class="musttitle" style="width:8%;">
                                        采购日期
                                </td>
                                <td style="width:10%;">
                                        &nbsp;<asp:TextBox ID="txtBillDate" runat="server" BorderWidth="0" CssClass="TextRead" 
                                        MaxLength="20" Width="92%" ReadOnly="True" Height="16px"></asp:TextBox>
                                </td>
                                <td  align="center" class="musttitle" style="width:8%;" >
                                        采购单号
                                </td>
                                <td  style=" width:10%">
                                    &nbsp;<asp:TextBox ID="txtID" runat="server" BorderWidth="0" CssClass="TextRead" 
                                        MaxLength="20" Width="94%" ReadOnly="True"></asp:TextBox>
                                </td>
                                  
                                <td  align="center" class="musttitle" style="width:8%;" >
                                    供应商 
                                </td>
                                <td  colspan="3" >
                                    &nbsp;<asp:TextBox ID="txtFactoryID" runat="server" BorderWidth="0" CssClass="TextBox" 
                                        Width="0px" Height="0px"></asp:TextBox>
                                    <asp:TextBox ID="txtFactoryName" runat="server" BorderWidth="0" CssClass="TextRead" 
                                        Width="96%" Height="16px" ReadOnly="True"></asp:TextBox>
                                </td>
                                 <td align="center" class="smalltitle" style="width:8%;">
                                        建单人员
                                </td> 
                                <td     style="width:10%;">
                                       &nbsp;<asp:TextBox ID="txtCreator" runat="server" BorderWidth="0" 
                                           CssClass="TextRead" Width="91%" Height="16px" ReadOnly="True"></asp:TextBox>
                                </td>
                            </tr>
                               
                          
                             <tr>
                                  <td align="center" class="smalltitle" style="width:8%;">
                                        建单日期
                                </td> 
                                <td      style="width:10%;">
                                        &nbsp;<asp:TextBox ID="txtCreateDate" runat="server" BorderWidth="0" 
                                            CssClass="TextRead" Width="92%" Height="16px" ReadOnly="True"></asp:TextBox>
                                </td>

                                <td align="center" class="smalltitle" style="width:8%;">
                                        一审人员
                                </td> 
                                <td     style="width:10%;">
                                       &nbsp;<asp:TextBox ID="txtCheckUser" runat="server" BorderWidth="0" 
                                           CssClass="TextRead" Width="91%" Height="16px" ReadOnly="True"></asp:TextBox>
                                </td>
                                <td align="center" class="smalltitle" style="width:8%;">
                                        一审日期
                                </td> 
                                <td    style="width:10%;">
                                        &nbsp;<asp:TextBox ID="txtCheckDate" runat="server" BorderWidth="0" 
                                            CssClass="TextRead" Width="92%" Height="16px" ReadOnly="True"></asp:TextBox>
                                </td>
                                 
                                 <td align="center" class="smalltitle" style="width:8%;">
                                        二审人员
                                </td> 
                                <td    style="width:10%;">
                                       &nbsp;<asp:TextBox ID="txtReCheckUser" runat="server" BorderWidth="0" 
                                           CssClass="TextRead" Width="90%" Height="16px" ReadOnly="True"></asp:TextBox>
                                </td>
                                <td align="center" class="smalltitle" style="width:8%;">
                                        二审日期
                                </td> 
                                <td style="width:10%">
                                      &nbsp;<asp:TextBox ID="txtReCheckDate" runat="server" BorderWidth="0" 
                                          CssClass="TextRead" Width="91%" Height="16px" ReadOnly="True"></asp:TextBox>
                                </td>
                                 
                            </tr>
                               <tr>

                                 <td align="center" class="smalltitle" style="width:8%;">
                                        关闭人员
                                </td> 
                                <td    style="width:10%;">
                                     &nbsp;<asp:TextBox ID="txtClose" runat="server" BorderWidth="0" CssClass="TextRead" 
                                            Height="16px" ReadOnly="True" Width="92%"></asp:TextBox>   &nbsp; 
                                       
                                </td>
                                 <td align="center" class="smalltitle" style="width:8%;">
                                        关闭日期
                                </td> 
                                <td    style="width:10%;">
                                       &nbsp;<asp:TextBox ID="txtCloseDate" runat="server" BorderWidth="0" 
                                           CssClass="TextRead" Height="16px" ReadOnly="True" Width="89%"></asp:TextBox>
                                </td>
                                <td  colspan="6">
                                          &nbsp;<asp:Button  id="btnCheck" CssClass="but" runat="server" Text="一审审核" 
                                         Height="23px" Width="80px" onclick="btnCheck_Click"/> 
                                         &nbsp;&nbsp;<asp:Button ID="btnReCheck" runat="server" CssClass="but" Height="23px" 
                                        onclick="btnReCheck_Click" Text="二审审核" Width="80px" />
                                    &nbsp;&nbsp;<asp:Button ID="btnClose" runat="server" CssClass="but" Height="23px" 
                                        onclick="btnClose_Click" Text="关闭计划单" Width="80px" />
                                        &nbsp;&nbsp;<asp:Button ID="btnEditDetail" runat="server" CssClass="but" Height="23px" Text="编辑明细" Width="80px" OnClientClick="return ScheduleSubEdit();" />
                                         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button 
                                              ID="btnCopy" runat="server" CssClass="but" Height="23px" 
                                              OnClientClick="return FormCopy();" Text="复制订单" Width="80px" />
                                 </td> 
                                 
                                 
                            </tr>
                            
                            	
			        </table>
                <div id='tabs'>
                       <div class='x-tab' title='采购明细'> 
                          <div id="sub1"  style="overflow: auto; width: 100%; height: 340px" >
                                <asp:GridView ID="dgViewSub1" runat="server" AutoGenerateColumns="False" SkinID="GridViewSkin"
                                    AllowPaging="True" Width="1300px" PageSize="10" 
                                    onrowdatabound="dgViewSub1_RowDataBound">
                                 <Columns>  
                                    <asp:BoundField DataField="IsComplete" HeaderText="订单状态" >
                                        <ItemStyle HorizontalAlign="Left" Width="8%" Wrap="False" />
                                        <HeaderStyle Wrap="False" />
                                    </asp:BoundField>
                                     <asp:BoundField DataField="RowID" HeaderText="序号" >
                                        <ItemStyle HorizontalAlign="Center" Width="4%" Wrap="False" />
                                        <HeaderStyle Wrap="False" />
                                    </asp:BoundField>
                                      <asp:BoundField DataField="ProductName" HeaderText="产品名称" >
                                        <ItemStyle HorizontalAlign="Left" Width="10%" Wrap="False" />
                                        <HeaderStyle Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ProductModel" HeaderText="产品型号" >
                                        <ItemStyle HorizontalAlign="Left" Width="8%" Wrap="False" />
                                        <HeaderStyle Wrap="False" />
                                    </asp:BoundField>
                                     <asp:BoundField DataField="ProductFModel" HeaderText="工厂型号" >
                                        <ItemStyle HorizontalAlign="Left" Width="8%" Wrap="False" />
                                        <HeaderStyle Wrap="False" />
                                    </asp:BoundField>
                            
                                    <asp:BoundField DataField="ColorName" HeaderText="规格" >
                                        <ItemStyle HorizontalAlign="Left" Width="8%" Wrap="False" />
                                        <HeaderStyle Wrap="False" />
                                    </asp:BoundField>
                                     <asp:BoundField DataField="PlanQty" HeaderText="数量" >
                                        <ItemStyle HorizontalAlign="right" Width="8%" Wrap="False" />
                                        <HeaderStyle Wrap="False" />
                                    </asp:BoundField> 
                                    <asp:BoundField DataField="RealQty" HeaderText="实入数量" >
                                        <ItemStyle HorizontalAlign="right" Width="100px" Wrap="False" />
                                        <HeaderStyle Wrap="False" />
                                    </asp:BoundField>  
                                      <asp:BoundField DataField="Price" HeaderText="单价" >
                                        <ItemStyle HorizontalAlign="right" Width="8%" Wrap="False" />
                                        <HeaderStyle Wrap="False" />
                                    </asp:BoundField>  
                                      <asp:BoundField DataField="Amount" HeaderText="金额" >
                                        <ItemStyle HorizontalAlign="right" Width="8%" Wrap="False" />
                                        <HeaderStyle Wrap="False" />
                                    </asp:BoundField>  
                                     <asp:BoundField DataField="StarNo" HeaderText="起始序号" >
                                        <ItemStyle HorizontalAlign="Left" Width="140px" Wrap="False" />
                                        <HeaderStyle Wrap="False" />
                                    </asp:BoundField> 
                                    <asp:BoundField DataField="EndNo" HeaderText="结束序号" >
                                        <ItemStyle HorizontalAlign="Left" Width="140px" Wrap="False" />
                                        <HeaderStyle Wrap="False" />
                                    </asp:BoundField> 
                                    <asp:TemplateField HeaderText="预入库日">
                                        <ItemTemplate>
                                            <%# ToYMD(DataBinder.Eval(Container.DataItem, "PlanDate"))%>
                                        </ItemTemplate>
                                        <HeaderStyle Wrap="False" />
                                        <ItemStyle HorizontalAlign="Left"  Width="100px" VerticalAlign="Middle" Wrap="False" />
                                    </asp:TemplateField> 
                                     <asp:BoundField DataField="SourceNo" HeaderText="来源单号" >
                                        <ItemStyle HorizontalAlign="Left" Width="8%" Wrap="False" />
                                        <HeaderStyle Wrap="False" />
                                    </asp:BoundField> 
                                     <asp:BoundField DataField="OrderNo" HeaderText="工厂订单号" >
                                        <ItemStyle HorizontalAlign="Left" Width="8%" Wrap="False" />
                                        <HeaderStyle Wrap="False" />
                                    </asp:BoundField>       
                                     <asp:TemplateField HeaderText="下单日期">
                                        <ItemTemplate>
                                            <%# ToYMD(DataBinder.Eval(Container.DataItem, "OrderDate"))%>
                                        </ItemTemplate>
                                        <HeaderStyle Wrap="False" />
                                        <ItemStyle HorizontalAlign="Left"  Width="100px" VerticalAlign="Middle" Wrap="False" />
                                    </asp:TemplateField> 
                                    
                                    <asp:TemplateField HeaderText="计划交期">
                                        <ItemTemplate>
                                            <%# ToYMD(DataBinder.Eval(Container.DataItem, "PlanDeliverDate"))%>
                                        </ItemTemplate>
                                        <HeaderStyle Wrap="False" />
                                        <ItemStyle HorizontalAlign="Left"  Width="100px" VerticalAlign="Middle" Wrap="False" />
                                    </asp:TemplateField> 
                                     <asp:TemplateField HeaderText="答复交期">
                                        <ItemTemplate>
                                            <%# ToYMD(DataBinder.Eval(Container.DataItem, "ReplyDate"))%>
                                        </ItemTemplate>
                                        <HeaderStyle Wrap="False" />
                                        <ItemStyle HorizontalAlign="Left"  Width="100px" VerticalAlign="Middle" Wrap="False" />
                                    </asp:TemplateField> 
                                     <asp:TemplateField HeaderText="调整交期">
                                        <ItemTemplate>
                                            <%# ToYMD(DataBinder.Eval(Container.DataItem, "ChangeDate"))%>
                                        </ItemTemplate>
                                        <HeaderStyle Wrap="False" />
                                        <ItemStyle HorizontalAlign="Left"  Width="100px" VerticalAlign="Middle" Wrap="False" />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="考核交期">
                                        <ItemTemplate>
                                            <%# ToYMD(DataBinder.Eval(Container.DataItem, "CheckDate"))%>
                                        </ItemTemplate>
                                        <HeaderStyle Wrap="False" />
                                        <ItemStyle HorizontalAlign="Left"  Width="100px" VerticalAlign="Middle" Wrap="False" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="实际交期">
                                        <ItemTemplate>
                                            <%# ToYMD(DataBinder.Eval(Container.DataItem, "FactDate"))%>
                                        </ItemTemplate>
                                        <HeaderStyle Wrap="False" />
                                        <ItemStyle HorizontalAlign="Left"  Width="100px" VerticalAlign="Middle" Wrap="False" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="报表月份">
                                        <ItemTemplate>
                                            <%# ToYMD(DataBinder.Eval(Container.DataItem, "ReportDate"))%>
                                        </ItemTemplate>
                                        <HeaderStyle Wrap="False" />
                                        <ItemStyle HorizontalAlign="Left"  Width="100px" VerticalAlign="Middle" Wrap="False" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="品质确认日期">
                                        <ItemTemplate>
                                            <%# ToYMD(DataBinder.Eval(Container.DataItem, "QualityDate"))%>
                                        </ItemTemplate>
                                        <HeaderStyle Wrap="False" />
                                        <ItemStyle HorizontalAlign="Left"  Width="100px" VerticalAlign="Middle" Wrap="False" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Result" HeaderText="验货结果" >
                                        <ItemStyle HorizontalAlign="Left" Width="8%" Wrap="False" />
                                        <HeaderStyle Wrap="False" />
                                    </asp:BoundField>
                                     
                                    <asp:BoundField DataField="IsPlanComplete" HeaderText="交期是否达成" >
                                        <ItemStyle HorizontalAlign="Left" Width="8%" Wrap="False" />
                                        <HeaderStyle Wrap="False" />
                                    </asp:BoundField>  
                                    <asp:BoundField DataField="Memo" HeaderText="备注" >
                                        <ItemStyle HorizontalAlign="Left" Width="8%" Wrap="False" />
                                        <HeaderStyle Wrap="False" />
                                    </asp:BoundField>  
                                    <asp:BoundField DataField="PlanDeliverQty" HeaderText="交期内完成数量" >
                                        <ItemStyle HorizontalAlign="Left" Width="8%" Wrap="False" />
                                        <HeaderStyle Wrap="False" />
                                    </asp:BoundField>  
                                    <asp:BoundField DataField="PlanDeliverAccount" HeaderText="交期内交货额" >
                                        <ItemStyle HorizontalAlign="Left" Width="8%" Wrap="False" />
                                        <HeaderStyle Wrap="False" />
                                    </asp:BoundField> 
                                    <asp:BoundField DataField="PlanDeliverNoQty" HeaderText="交期内未交货数量" >
                                        <ItemStyle HorizontalAlign="Left" Width="8%" Wrap="False" />
                                        <HeaderStyle Wrap="False" />
                                    </asp:BoundField>  
                                    <asp:BoundField DataField="PlanDeliverNoAccount" HeaderText="交期内未交货额" >
                                        <ItemStyle HorizontalAlign="Left" Width="8%" Wrap="False" />
                                        <HeaderStyle Wrap="False" />
                                    </asp:BoundField> 
                                    <asp:BoundField DataField="PlanCompleteRate" HeaderText="交期达成率%" >
                                        <ItemStyle HorizontalAlign="Left" Width="8%" Wrap="False" />
                                        <HeaderStyle Wrap="False" />
                                    </asp:BoundField>
                                    
                                    <asp:BoundField DataField="DeliverQty" HeaderText="已交货数量" >
                                        <ItemStyle HorizontalAlign="Left" Width="8%" Wrap="False" />
                                        <HeaderStyle Wrap="False" />
                                    </asp:BoundField>  
                                    <asp:BoundField DataField="DeliverAccount" HeaderText="已交货额" >
                                        <ItemStyle HorizontalAlign="Left" Width="8%" Wrap="False" />
                                        <HeaderStyle Wrap="False" />
                                    </asp:BoundField> 
                                    <asp:BoundField DataField="DeliverNoQty" HeaderText="未交货数量" >
                                        <ItemStyle HorizontalAlign="Left" Width="8%" Wrap="False" />
                                        <HeaderStyle Wrap="False" />
                                    </asp:BoundField>  
                                    <asp:BoundField DataField="DeliverNoAccount" HeaderText="未交货金额" >
                                        <ItemStyle HorizontalAlign="Left" Width="8%" Wrap="False" />
                                        <HeaderStyle Wrap="False" />
                                    </asp:BoundField> 
                                    <asp:BoundField DataField="CompleteRate" HeaderText="订单完成率%" >
                                        <ItemStyle HorizontalAlign="Left" Width="8%" Wrap="False" />
                                        <HeaderStyle Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NoCompleteMemo" HeaderText="未完成原因" >
                                        <ItemStyle HorizontalAlign="Left" Width="8%" Wrap="False" />
                                        <HeaderStyle Wrap="False" />
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
                       <div class='x-tab' title='其它'>
                          <div id="sub2" style="overflow: auto; width: 100%; height: 340px">
                            <table width="100%" class="maintable"  bordercolor="#ffffff" border="1" >
                                 <tr>
                                    <td align="center"  class="smalltitle" style="width:8%;">
                                        工艺要求
                                    </td>
                                    <td align="left"  class="smalltitle" style="width:92%;" >
                                        &nbsp;<asp:TextBox ID="txtTechRequery" runat="server" CssClass="TextRead" 
                                            TextMode="MultiLine" Height="51px" Width="98%" ReadOnly="True"></asp:TextBox>
                                    </td>
                                </tr>
                                 <tr>
                                    <td align="center"  class="smalltitle" style="width:8%;">
                                        功能
                                    </td>
                                    <td align="left"  class="smalltitle" style="width:92%;" >
                                        &nbsp;<asp:TextBox ID="txtOrderFunction" runat="server" CssClass="TextRead" 
                                            TextMode="MultiLine" Height="89px" Width="98%" ReadOnly="True"></asp:TextBox>
                                    </td>
                                </tr>
                                 <tr>
                                    <td align="center"  class="smalltitle" style="width:8%;">
                                        包装要求
                                    </td>
                                    <td align="left"  class="smalltitle" style="width:92%;" >
                                        &nbsp;<asp:TextBox ID="txtPackRequery" runat="server" CssClass="TextRead" 
                                            TextMode="MultiLine" Height="51px" Width="98%" ReadOnly="True"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center"  class="smalltitle" style="width:8%;">
                                        免费随货配件
                                    </td>
                                    <td align="left"  class="smalltitle" style="width:92%;" >
                                        &nbsp;<asp:TextBox ID="txtOrderParts" runat="server" CssClass="TextRead" 
                                            TextMode="MultiLine" Height="51px" Width="98%" ReadOnly="True"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center"  class="smalltitle" style="width:8%;">
                                        备注
                                    </td>
                                    <td align="left"  class="smalltitle" style="width:92%;" >
                                        &nbsp;<asp:TextBox ID="txtMemo" runat="server" CssClass="TextRead" 
                                            TextMode="MultiLine" Height="88px" Width="98%" ReadOnly="True"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                          </div> 
                      </div>
                </div>
                   </div>
            
			</ContentTemplate>
             <Triggers>  
                <asp:PostBackTrigger ControlID="btnPrint" />  
              </Triggers>
            </asp:UpdatePanel>
            <asp:HiddenField ID="HdfActiveTab" runat="server" Value="0" />
		</form>
</body>
</html>
