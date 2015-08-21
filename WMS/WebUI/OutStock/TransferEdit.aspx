<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TransferEdit.aspx.cs" Inherits="WMS.WebUI.OutStock.TransferEdit" %>

<%@ Register src="../../UserControl/Calendar.ascx" tagname="Calendar" tagprefix="uc1" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head id="Head1" runat="server">
		<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />  
        <title></title> 
         <link href="~/Css/Main.css" type="text/css" rel="stylesheet" /> 
        <link href="~/Css/op.css" type="text/css" rel="stylesheet" /> 
        <link rel="stylesheet" type="text/css" href="~/ext-3.3.1/resources/css/ext-all.css" />
        <script type="text/javascript" src='<%=ResolveClientUrl("~/JQuery/jquery-1.8.3.min.js") %>'></script>
        <script type="text/javascript" src='<%=ResolveClientUrl("~/JScript/Common.js") %>'></script>
        <script type="text/javascript" src='<%=ResolveClientUrl("~/ext-3.3.1/ext-base.js") %>'></script> 
        <script type="text/javascript" src='<%=ResolveClientUrl("~/ext-3.3.1/ext-all.js") %>'></script>
       <script type="text/javascript" src='<%=ResolveClientUrl("~/JScript/DataProcess.js") %>'></script>
         <script type="text/javascript">
             $(document).ready(function () {
                 BindEvent();
             });
             function content_resize() {
                 //編輯頁面 div高度
//                 var div = $("#surdiv");
//                 var h = 300;
//                 if ($(window).height() <= 0) {
//                     h = document.body.clientHeight - 30;
//                 }
//                 else {
//                     h = $(window).height() - 30;
//                 }
//                 $("#surdiv").css("height", h);
             }
             function BindEvent() {
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
                 $("#sub1").css("height", h - 90);
                 $("#sub2").css("height", h - 90);

                 $("#btnCustQuery").bind("click", function () {
                     if ($('#txtCustomerID').val() == '') {
                         alert("请先选择出库单号");
                         return false;
                     }
                     var where = "CUSTOMER_CODE !='" + $('#txtCustomerID').val() + "'";
                     GetOtherValue('CMD_CUSTOMER', 'txtTranCustID,txtTranCustName', "CUSTOMER_CODE,CUSTOMER_NAME", where);
                     return false;
                 });

                 $("#btnQuery").bind("click", function () {
                     GetOtherValue('WMS_Stock', 'txtStockID,txtSourceNo,txtCustomerID,txtCustomerName', "BillID,SourceNo,CustomerID,CustName", "State=5 and Flag=2 ");
                     return false;

                 });
                 $("#btnInsertDetailQuery").bind("click", function () {
                     if ($('#txtStockID').val() == '') {
                         alert("请先选择出库单号");
                         return false;
                     }
                     var Where = "BillID='" + $('#txtStockID').val() + "'";
                     return GetMulSelectValue('WMS_StockDetailNoTran', 'hdnMulSelect', Where);
                 });

                 $("#btnInsertSubQuery").bind("click", function () {
                     if ($('#txtStockID').val() == '') {
                         alert("请先选择出库单号");
                         return false;
                     }
                     var Where = "BillID='" + $('#txtStockID').val() + "'";
                     return GetMulSelectValue('WMS_StockSubNoTran', 'hdnMulSelect', Where);
                 });

             }
             function ChangePrice(obj) {
                 var total = $('#txtTotalQty').val();
                 total = parseInt(total) - parseInt($(obj).attr("oldvalue")) + parseInt(obj.value);
                 $('#txtTotalQty').val(total);
             }

             function Save() {

                 if ($("#txtBillDate_txtDate").val() == "") {
                     alert("变更日期不能为空，请输入！");
                     $("#txtBillDate_txtDate").focus();
                     return false;
                 }
                 if ($("#txtID").val() == "") {
                     alert("变更单号不能为空，请输入！");
                     $("#txtID").focus();
                     return false;
                 }
                 if ($("#txtStockID").val() == "") {
                     alert("出库单号不能为空，请输入！");
                     $("#txtStockID").focus();
                     return false;
                 }
                 if ($("#txtTranCustID").val() == "") {
                     alert("变更经销商不能为空，请输入！");
                     $("#txtScheduleNo").focus();
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
                                    <asp:Button ID="btnSave" runat="server" Text="保存" OnClientClick="return Save()" 
                                        CssClass="ButtonSave" onclick="btnSave_Click" 
                                         />
                                    <asp:Button ID="btnExit" runat="server" Text="离开" OnClientClick="return Exit();" 
                                        CssClass="ButtonExit"  />
                                </td>
                            </tr>
                        </table>
                    <div id="surdiv" style="overflow: auto">
			        <table id="Table1" class="maintable" cellspacing="0" cellpadding="0" width="100%" align="center" bordercolor="#ffffff"
				        border="1" runat="server">			
					        <tr>
                                <td align="center" class="musttitle" style=" width: 8%">
                                        变更日期
                                </td>
                                <td  style="width:15%">
                                        &nbsp;<uc1:Calendar ID="txtBillDate" runat="server" />
                                </td>
                                <td  align="center" class="musttitle" style="width:8%;" >
                                        变更单号
                                </td>
                                <td style="width:15%" >
                                    &nbsp;<asp:TextBox ID="txtID" runat="server" BorderWidth="0" CssClass="TextBox" 
                                        MaxLength="20" Width="94%"></asp:TextBox>
                                          
                                </td>
                                  
                                <td align="center" class="musttitle" style="width:8%" >
                                    出库单号</td>
                                  <td  style="width:18%">
                                        &nbsp;<asp:TextBox ID="txtStockID" runat="server" BorderWidth="0" CssClass="Textbox" 
                                        Width="81%" Height="16px"></asp:TextBox>
                                        <asp:Button ID="btnQuery" runat="server" CssClass="ButtonCss" Text="..." Width="20px" />
                                </td>
                                 <td align="center" class="musttitle" style="width:8%" >
                                     销售单号
                                </td>
                                  <td  style="width:18%">
                                          &nbsp;<asp:TextBox ID="txtSourceNo" runat="server" BorderWidth="0" 
                                             CssClass="TextRead" Height="16px"  Width="91%"></asp:TextBox>
                                </td>
                               
                            </tr>
                             <tr>
                                <td align="center" style="width:8%;" class="musttitle">
                                        经销商&nbsp; 
                                 </td>
                                <td colspan="3" >
                                       &nbsp;<asp:TextBox ID="txtCustomerID" runat="server" BorderWidth="0" 
                                            CssClass="TextRead" Height="16px" Width="0px"></asp:TextBox>
                                            <asp:TextBox ID="txtCustomerName" runat="server" BorderWidth="0" 
                                            CssClass="TextRead" Height="16px" Width="96%" ></asp:TextBox>
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
                                <td align="center" style="width:8%;" class="musttitle">
                                        变更经销商
                                   
                                </td> 
                                <td  colspan="3">
                                    &nbsp;<asp:TextBox ID="txtTranCustID" runat="server" BorderWidth="0" 
                                        CssClass="TextBox" Height="16px" Width="0px"></asp:TextBox>
                                        <asp:TextBox ID="txtTranCustName" runat="server" BorderWidth="0" 
                                        CssClass="TextBox" Height="16px" Width="92%"></asp:TextBox>
                                    <asp:Button ID="btnCustQuery" runat="server" CssClass="ButtonCss" Text="..." Width="20px" />
                                 </td>

                                  <td  colspan="4" >
                                        </td> 
                                
                               
                            </tr>
                             <tr>
                                <td align="center"  style="width:8%;" class="smalltitle">
                                     &nbsp;备注
                                  
                                 
                                </td>
                                <td colspan="7">
                                    &nbsp;<asp:TextBox ID="txtMemo" runat="server" CssClass="MultiLineTextBox" 
                                        TextMode="MultiLine" Height="51px" Width="99%"></asp:TextBox>
                                </td>
                            </tr>
                             		
			        </table>
                     <div id='tabs'>
                         <div class='x-tab' title='变更明细'> 
                            <table style="width:100%">
                                <tr>
                                    <td class="table_titlebgcolor" height="25px">
                                        &nbsp;&nbsp;<asp:Button  id="btnInsertSubQuery" CssClass="ButtonCss" runat="server" 
                                            Text="载入明细" Width="92px" onclick="btnInsertSubQuery_Click"   />&nbsp;&nbsp; 
                                            <asp:Button  id="btnDeleteSubQuery" CssClass="ButtonCss" runat="server" 
                                            Text="删除明细"   Width="66px" onclick="btnDeleteSubQuery_Click" />
                                        </td>
                                
                                    <td width="3%">
                                    </td>
                                </tr>
                            </table> 
                            <div id="sub1" style="overflow: auto; width: 100%; height: 320px" >
                              <asp:GridView ID="dgViewSub1" runat="server" AutoGenerateColumns="False" SkinID="GridViewSkin"
                                    AllowPaging="True" Width="98%" PageSize="10" 
                                    onrowdatabound="dgViewSub1_RowDataBound">
                                         <Columns>
                                            <asp:TemplateField  >
                                                    <HeaderTemplate>
                                                    <input type="checkbox" onclick="selectAll('dgViewSub1',this.checked);" />                    
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                    <asp:CheckBox id="cbSelect" runat="server" ></asp:CheckBox>                   
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="5%"></HeaderStyle>
                                                    <ItemStyle Width="5%" HorizontalAlign="Center"></ItemStyle>
                                              </asp:TemplateField>  
                                             <asp:TemplateField HeaderText="序号">
                                                <ItemTemplate>
                                                   <asp:Label ID="RowID" runat="server" Text=""></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                                <HeaderStyle Width="4%" ForeColor="Blue" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ProductModel" HeaderText="产品型号" >
                                                <ItemStyle HorizontalAlign="Left" Width="8%" Wrap="False" />
                                                <HeaderStyle Wrap="False" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ProductName" HeaderText="产品名称" >
                                                <ItemStyle HorizontalAlign="Left" Width="20%" Wrap="False" />
                                                <HeaderStyle Wrap="False" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ColorName" HeaderText="规格" >
                                                <ItemStyle HorizontalAlign="Left" Width="8%" Wrap="False" />
                                                <HeaderStyle Wrap="False" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="数量">
                                                <ItemTemplate>
                                                   <asp:TextBox ID="Qty" runat="server" Width="100%" CssClass="detailtext" style="text-align:right;" 
                                                    onkeypress="return regInput(this,/^\d+$/,String.fromCharCode(event.keyCode))" 	onpaste="return regInput(this,/^\d+$/,window.clipboardData.getData('Text'))" 
                                                    ondrop="return regInput(this,/^\d+$/,event.dataTransfer.getData('Text'))" onfocus="TextFocus(this);"></asp:TextBox> 
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                                <HeaderStyle Wrap="False" Width="8%" ForeColor="Blue" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerSettings Visible="false" />
                                    </asp:GridView>
                            </div>
                            <table class="maintable" style="width:100%; height:28px"> 
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
                                        <asp:Literal ID="Literal3" Text="每页" runat="server"  Visible="false" />
                                            <asp:DropDownList ID="ddlPageSizeSub1" runat="server" AutoPostBack="True"  
                                            Height="16px" onselectedindexchanged="ddlPageSizeSub1_SelectedIndexChanged" Visible="false">
                    
                                            </asp:DropDownList>
                                    </td>
                                    <td width="3%">
                                    </td>
                                </tr>
                           </table>
                        </div>
                        <div class='x-tab' title='变更序号明细'>
                            <table style="width:100%">
                                <tr>
                                    <td class="table_titlebgcolor" height="25px">
                                        &nbsp;&nbsp;<asp:Button  id="btnInsertDetailQuery" CssClass="ButtonCss" runat="server" 
                                            Text="载入明细" Width="92px" onclick="btnInsertDetailQuery_Click"     />&nbsp;&nbsp; 
                                            <asp:Button  id="btnDeleteDetailQuery" CssClass="ButtonCss" runat="server" 
                                            Text="删除明细"   Width="66px" onclick="btnDeleteDetailQuery_Click" />
                                        </td>
                                
                                    <td width="3%">
                                    </td>
                                </tr>
                            </table>  
                            <div id="sub2" style="overflow: auto; width: 100%; height: 320px" >
                                <asp:GridView ID="dgViewSub2" runat="server" AutoGenerateColumns="False"  SkinID="GridViewSkin"
                                        AllowPaging="True" Width="98%" PageSize="10" 
                                    onrowdatabound="dgViewSub2_RowDataBound">
                                        <Columns>
                                             <asp:TemplateField  >
                                                    <HeaderTemplate>
                                                    <input type="checkbox" onclick="selectAll('dgViewSub2',this.checked);" />                    
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                    <asp:CheckBox id="cbSelect" runat="server" ></asp:CheckBox>                   
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="5%"></HeaderStyle>
                                                    <ItemStyle Width="5%" HorizontalAlign="Center"></ItemStyle>
                                              </asp:TemplateField>
                                                <asp:TemplateField HeaderText="序号">
                                                    <ItemTemplate>
                                                       <asp:Label ID="RowID" runat="server" Text=""></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle Width="4%" ForeColor="Blue" />
                                                </asp:TemplateField>
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
                            <table class="maintable" style="width:100%; height:28px" > 
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
                                        <asp:Literal ID="Literal5" Text="每页" runat="server"  Visible="false" />
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
