<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InStockEdit.aspx.cs" Inherits="WMS.WebUI.InStock.InStockEdit" %>

<%@ Register src="../../UserControl/Calendar.ascx" tagname="Calendar" tagprefix="uc1" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
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
        
       <script type= "text/javascript">
           $(document).ready(function () {
               BindEvent();
           });
           function content_resize() {
               //編輯頁面 div高度
               
           }
           function BindEvent() {
               $("#btnFactory").bind("click", function () {
                   GetOtherValue('CMD_Factory', 'txtFactoryID,txtFactoryName', "FactoryID,FactoryName", "1=1");
                   return false;
               });
                 


               var activeTab = parseInt($('#HdfActiveTab').val());
               var h = 300;
               if ($(window).height() <= 0) {
                   h = document.body.clientHeight - 30;
               }
               else {
                   h = $(window).height() - 30;
               }
               h = h - 145;
               var tabPanel = new Ext.TabPanel({
                   height: h,
                   width: "100%",
                   autoTabs: true, //自动扫描页面中的div然后将其转换为标签页
                   deferredRender: false, //不进行延时渲染
                   activeTab: activeTab, //默认激活第一个tab页
                   animScroll: true, //使用动画滚动效果
                   enableTabScroll: true, //tab标签超宽时自动出现滚动按钮
                   applyTo: 'tabs'
               });
               $("#sub1").css("height", h - 55);
               $("#sub2").css("height", h - 55);
           }
           function Save() {

               if ($("#txtTaskID").val() == "") {
                   alert("工厂销售单号不能为空，请输入！");
                   $("#txtTaskID").focus();
                   return false;
               }
                
               return true;
           }
      </script>  
	</head>
	<body >
		<form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />  
            <asp:UpdateProgress ID="updateprogress" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
            <ProgressTemplate>            
                     <div id="progressBackgroundFilter" style="display:none"></div>
                <div id="processMessage"> Loading...<br /><br />
                     <img alt="Loading" src="../../images/process/loading.gif" />
                </div>            
 
                </ProgressTemplate>
 
            </asp:UpdateProgress>  
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">                
                <ContentTemplate>
                    <table style="width: 100%; height: 20px;" class="OperationBar">
                            <tr>
                                <td align="right">
                                    <asp:Button ID="btnCancel" runat="server" Text="放弃" 
                                        OnClientClick="return Cancel();" CssClass="ButtonCancel" 
                                         />
                                    <asp:Button ID="btnSave" runat="server" Text="保存"
                                        CssClass="ButtonSave" onclick="btnSave_Click" OnClientClick="return Save();"/>
                                    <asp:Button ID="btnExit" runat="server" Text="离开" OnClientClick="return Exit();" 
                                        CssClass="ButtonExit"  />
                                </td>
                            </tr>
                        </table>
			        <div id="surdiv" style="overflow: auto">
			        <table id="Table1" class="maintable" cellspacing="0" cellpadding="0" width="100%" align="center" bordercolor="#ffffff"
				        border="1" runat="server">			
					        <tr>
                                <td align="center" class="musttitle" style="width:10%;">
                                        入库日期
                                </td>
                                <td colspan="2" style="width:23%" >
                                      &nbsp;<asp:TextBox ID="txtBillDate" runat="server" BorderWidth="0" 
                                            CssClass="TextRead" MaxLength="20" ReadOnly="True" Width="45%" 
                                          Height="16px"></asp:TextBox>
                                </td>
                                <td  align="center" class="musttitle" style="width:10%;" >
                                        入库单号
                                </td>
                                <td colspan="3" style=" width:25%">
                                    &nbsp;
                                    <asp:TextBox ID="txtID" runat="server" BorderWidth="0" CssClass="TextRead" 
                                        MaxLength="20" Width="94%" ReadOnly="True" Height="16px"></asp:TextBox>
                                </td>
                                  
                                <td  align="center" class="smalltitle" style="width:10%;" >
                                    采购单号
                                </td>
                                <td style="width:15%">
                                    &nbsp;<asp:TextBox ID="txtSourceNo" runat="server" BorderWidth="0" CssClass="TextRead" 
                                        MaxLength="20" Width="91%" ReadOnly="True" Height="16px"></asp:TextBox>
                                </td>
                            </tr>
                             <tr>
                                <td align="center" class="musttitle" style="width:10%;">
                                        供应商
                                </td>
                                <td colspan="2" style="width:23%" >
                                         <asp:TextBox ID="txtFactoryID" runat="server" BorderWidth="0" CssClass="TextBox" 
                                        Width="0px" Height="0px"></asp:TextBox>
                                      &nbsp;<asp:TextBox ID="txtFactoryName" runat="server" BorderWidth="0" 
                                            CssClass="TextRead" MaxLength="20" ReadOnly="True" Width="90%" 
                                             Height="16px"></asp:TextBox>  <asp:Button
                                            ID="btnFactory" Width="20px"  CssClass="ButtonCss" runat="server"  
                                        Text="..." />
                                </td>
                                <td  align="center" class="musttitle" style="width:10%;" >
                                        工厂订单号
                                </td>
                                <td colspan="3" style=" width:25%">
                                    &nbsp;
                                    <asp:TextBox ID="txtFactoryOrderNo" runat="server" BorderWidth="0" CssClass="TextRead" 
                                        MaxLength="20" Width="94%" ReadOnly="True" Height="16px"></asp:TextBox>
                                </td>
                                  
                               <td  align="center" class="musttitle" style="width:10%;" >
                                    工厂销售单号
                                </td>
                                <td style="width:15%">
                                    &nbsp;<asp:TextBox ID="txtTaskID" runat="server" BorderWidth="0" CssClass="TextBox" 
                                        MaxLength="20" Width="91%" Height="16px"  ></asp:TextBox>
                                </td>
                                 
                            </tr>
                            
                            <tr>
                                <td align="center"  class="smalltitle" style="width:8%;">
                                    备注
                                    
                                </td>
                                <td colspan="8">
                                    &nbsp;<asp:TextBox ID="txtMemo" runat="server" CssClass="TextBox" 
                                        TextMode="MultiLine" Height="51px" Width="99%"></asp:TextBox>
                                    
                                </td>
                            </tr>
                             <tr>
                                <td align="center" class="smalltitle" style="width:10%;">
                                        建单人员
                                </td> 
                                <td     style="width:10%;">
                                       &nbsp;<asp:TextBox ID="txtCreator" runat="server" BorderWidth="0" CssClass="TextRead" 
                                        Width="91%" Height="16px" ReadOnly="True"></asp:TextBox>
                                </td>
                                <td align="center" class="smalltitle" style="width:10%;">
                                        建单日期
                                </td> 
                                <td  colspan="2"   style="width:10%;">
                                        &nbsp;<asp:TextBox ID="txtCreateDate" runat="server" BorderWidth="0" CssClass="TextRead" 
                                        Width="92%" Height="16px" ReadOnly="True"></asp:TextBox>
                                </td>
                                 
                                 <td align="center" class="smalltitle" style="width:10%;">
                                        审核人员
                                </td> 
                                <td    style="width:10%;">
                                       &nbsp; 
                                       <asp:TextBox ID="txtChecker" runat="server" BorderWidth="0" CssClass="TextRead" 
                                           Height="16px" ReadOnly="True" Width="91%"></asp:TextBox>
                                </td>
                                <td align="center" class="smalltitle" style="width:10%;">
                                        审核日期
                                </td> 
                                <td style="width:15%">
                                      <asp:TextBox ID="txtCheckDate" runat="server" BorderWidth="0" 
                                          CssClass="TextRead" Height="16px" ReadOnly="True" Width="96%"></asp:TextBox>&nbsp;</td>
                                 
                            </tr>
                             
                             		
			        </table>
                    <div id='tabs'>
                          <div class='x-tab' title='入库明细'>   
                            <div id="sub1" style="overflow: auto; width: 100%; height: 335px" >
                                <asp:GridView ID="dgViewSub1" runat="server" AutoGenerateColumns="False" SkinID="GridViewSkin"
                            AllowPaging="True" Width="98%" PageSize="10" onrowdatabound="dgViewSub1_RowDataBound">
                                    <Columns>
                                         <asp:BoundField DataField="RowID" HeaderText="序号" 
                                            SortExpression="RowID"  >
                                            <ItemStyle HorizontalAlign="Center" Width="5%" Wrap="False" />
                                            <HeaderStyle Wrap="False" />
                                        </asp:BoundField>
                                         
                                         <asp:BoundField DataField="ProdName" HeaderText="产品名称" 
                                            SortExpression="ProdName"  >
                                            <ItemStyle HorizontalAlign="Left" Width="10%" Wrap="False" />
                                            <HeaderStyle Wrap="False" />
                                        </asp:BoundField>
                                         <asp:BoundField DataField="ProdModel" HeaderText="产品型号" 
                                            SortExpression="ProdModel"  >
                                            <ItemStyle HorizontalAlign="Left" Width="8%" Wrap="False" />
                                            <HeaderStyle Wrap="False" />
                                        </asp:BoundField>
                                      <%--  <asp:BoundField DataField="VersionID" HeaderText="版本号" 
                                            SortExpression="VersionID"  >
                                            <ItemStyle HorizontalAlign="Left" Width="6%" Wrap="False" />
                                            <HeaderStyle Wrap="False" />
                                        </asp:BoundField>
                                         <asp:BoundField DataField="VerName" HeaderText="版本名称" 
                                            SortExpression="VerName"  >
                                            <ItemStyle HorizontalAlign="Left" Width="8%" Wrap="False" />
                                            <HeaderStyle Wrap="False" />
                                        </asp:BoundField>--%>
                                      <%--  <asp:BoundField DataField="ColorID" HeaderText="规格代码" 
                                            SortExpression="ColorID"  >
                                            <ItemStyle HorizontalAlign="Left" Width="6%" Wrap="False" />
                                            <HeaderStyle Wrap="False" />
                                        </asp:BoundField>--%>
                                         <asp:BoundField DataField="ColName" HeaderText="规格" 
                                            SortExpression="ColName"  >
                                            <ItemStyle HorizontalAlign="Left" Width="8%" Wrap="False" />
                                            <HeaderStyle Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="InStockQty" HeaderText="数量" 
                                            SortExpression="InStockQty"  >
                                            <ItemStyle  Width="10%" Wrap="False" HorizontalAlign="Right"/>
                                            <HeaderStyle Wrap="False" />
                                        </asp:BoundField>
                                    </Columns>
                        
                                    <PagerSettings Visible="false" />
                                </asp:GridView>
                            </div>
             
                            <table   width="100%" class="maintable"  bordercolor="#ffffff" border="1" style="height:25px"> 
                                <tr>
                                    <td align="center"  style="width:8%;" class="smalltitle">
                                        数量合计
                                    </td>
                                    <td style="width:15%">
                                        &nbsp;<asp:TextBox ID="txtTotalQty" runat="server" BorderWidth="0" 
                                            CssClass="TextRead" Height="16px" ReadOnly="True" Width="89%" style="text-align:right"></asp:TextBox>
                                
                                    </td>
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
                                        <asp:Literal ID="Literal1" Text="每页" runat="server" Visible="false"   />
                                            <asp:DropDownList ID="ddlPageSizeSub1" runat="server" AutoPostBack="True"  
                                            Height="16px" onselectedindexchanged="ddlPageSizeSub1_SelectedIndexChanged" Visible="false" >
                    
                                            </asp:DropDownList>
                                    </td>
                                    <td width="3%">
                                    </td>
                                </tr>
                              </table>
                          </div>
                          <div class='x-tab' title='入库序号明细'> 
                              <div  id="sub2" style="overflow: auto; width: 100%; height: 335px" >
                                    <asp:GridView ID="dgViewSub2" runat="server" AutoGenerateColumns="False" SkinID="GridViewSkin"
                                        AllowPaging="True" Width="98%" PageSize="10">
                                      <Columns>
                                         <asp:BoundField DataField="RowID" HeaderText="序号" 
                                            SortExpression="RowID"  >
                                            <ItemStyle HorizontalAlign="Center" Width="5%" Wrap="False" />
                                            <HeaderStyle Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="BarCode" HeaderText="条形码" 
                                            SortExpression="BarCode"  >
                                            <ItemStyle HorizontalAlign= "Left" Width="10%" Wrap="False" />
                                            <HeaderStyle Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ProductID" HeaderText="产品编码" 
                                            SortExpression="ProductID"  >
                                            <ItemStyle HorizontalAlign="Left" Width="8%" Wrap="False" />
                                            <HeaderStyle Wrap="False" />
                                        </asp:BoundField>
                                         <asp:BoundField DataField="ProdName" HeaderText="产品部件" 
                                            SortExpression="ProdName"  >
                                            <ItemStyle HorizontalAlign="Left" Width="10%" Wrap="False" />
                                            <HeaderStyle Wrap="False" />
                                        </asp:BoundField>
                                       <%-- <asp:BoundField DataField="VersionID" HeaderText="版本号" 
                                            SortExpression="VersionID"  >
                                            <ItemStyle HorizontalAlign="Left" Width="6%" Wrap="False" />
                                            <HeaderStyle Wrap="False" />
                                        </asp:BoundField>
                                         <asp:BoundField DataField="VerName" HeaderText="版本名称" 
                                            SortExpression="VerName"  >
                                            <ItemStyle HorizontalAlign="Left" Width="8%" Wrap="False" />
                                            <HeaderStyle Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ColorID" HeaderText="规格代码" 
                                            SortExpression="ColorID"  >
                                            <ItemStyle HorizontalAlign="Left" Width="6%" Wrap="False" />
                                            <HeaderStyle Wrap="False" />
                                        </asp:BoundField>--%>
                                         <asp:BoundField DataField="ColName" HeaderText="规格" 
                                            SortExpression="ColName"  >
                                            <ItemStyle HorizontalAlign="Left" Width="8%" Wrap="False" />
                                            <HeaderStyle Wrap="False" />
                                        </asp:BoundField>
                                    </Columns>
                                    <PagerSettings Visible="false" />
                                </asp:GridView>
                             </div>
                              <table width="100%" class="maintable"  bordercolor="#ffffff" border="1" style="height:25px" > 
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblCurrentPageSub2" runat="server" ></asp:Label>&nbsp;&nbsp; 
                                        <asp:LinkButton ID="btnFirstSub2" runat="server"  
                                                    Text="首页" onclick="btnFirstSub2_Click"></asp:LinkButton> 
                                        &nbsp;<asp:LinkButton ID="btnPreSub2" runat="server"  
                                                Text="上一页" onclick="btnPreSub2_Click"></asp:LinkButton> 
                               
                                        &nbsp;<asp:LinkButton ID="btnNextSub2" runat="server" 
                                                Text="下一页" onclick="btnNextSub2_Click"></asp:LinkButton> 
                                        &nbsp;<asp:LinkButton ID="btnLastSub2" runat="server"  
                                                    Text="尾页" onclick="btnLastSub2_Click"></asp:LinkButton> 
                                &nbsp;<asp:textbox id="txtPageNoSub2" onkeypress="return regInput(this,/^\d+$/,String.fromCharCode(event.keyCode))"
					                            onpaste="return regInput(this,/^\d+$/,window.clipboardData.getData('Text'))" ondrop="return regInput(this,/^\d+$/,event.dataTransfer.getData('Text'))"
					                            runat="server" Width="56px" CssClass="TextBox" >
                                                </asp:textbox>&nbsp;<asp:linkbutton id="btnToPageSub2" runat="server" Text="跳转" onclick="btnToPageSub2_Click"></asp:linkbutton>
                                            &nbsp;
                                        <asp:Literal ID="Literal2" Text="每页" runat="server"   Visible="false"/>
                                            <asp:DropDownList ID="ddlPageSizeSub2" runat="server" AutoPostBack="True"  
                                            Height="16px" onselectedindexchanged="ddlPageSizeSub2_SelectedIndexChanged" Visible="false">
                    
                                            </asp:DropDownList>
                                    </td>
                                    <td width="3%">
                                    </td>
                                </tr>
                              </table>
                        </div>  
                      </div>
                 </div>  
                    <asp:HiddenField ID="HdfActiveTab" runat="server" Value="0" />     
                    <input type="hidden" runat="server" id="hdnMulSelect" /> 
                  </ContentTemplate>
              </asp:UpdatePanel>       
             
             
		</form>
	</body>
</html>
