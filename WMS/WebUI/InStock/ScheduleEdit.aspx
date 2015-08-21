<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ScheduleEdit.aspx.cs" Inherits="WMS.WebUI.InStock.ScheduleEdit" %>

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

        <script type="text/javascript">
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
                $("[ID$='btnProduct']").bind("click", function () {
//                    var RNames = this.id.split('_');
//                    var RowName = RNames[0] + "_" + RNames[1] + "_";
//                    GetOtherValue('CMD_Product', RowName + "ProductID," + RowName + "ProductName", "PRODUCT_CODE,PRODUCT_NAME", "1=1", 'hdnMulSelect');
//                    return false;

                    return GetMulSelectValue('CMD_ProductBom', 'hdnMulSelect', '1=1');
                });
                $("[ID$='Price']").bind("change", function () {
                    ChangePrice(this);
                });
                $("[ID$='PlanQty']").bind("change", function () {
                    ChangePrice(this);
                });
               

                $("[ID$='btnColor']").bind("click", function () {
                    var RNames = this.id.split('_');
                    var RowName = RNames[0] + "_" + RNames[1] + "_";
                    var ProductID = $('#' + RowName + 'ProductID').val();
                    if (ProductID == "") {
                        alert("请先选择产品资料！");
                        return false;
                    }
                    GetOtherValue('CMD_COLOR', RowName + "ColorID," + RowName + "ColorName", "COLOR_CODE,COLOR_NAME", "PRODUCT_CODE='" + ProductID + "'");
                    return false;

                });

                var activeTab = parseInt($('#HdfActiveTab').val());
                
                var tabPanel = new Ext.TabPanel({
                    height: 440,
                    width: "100%",
                    autoTabs: true, //自动扫描页面中的div然后将其转换为标签页
                    deferredRender: false, //不进行延时渲染
                    activeTab: activeTab, //默认激活第一个tab页
                    animScroll: true, //使用动画滚动效果
                    enableTabScroll: true, //tab标签超宽时自动出现滚动按钮
                    applyTo: 'tabs'
                });
               
            }
            function ChangePrice(obj) {
                var RNames = obj.id.split('_');
                var RowName = RNames[0] + "_" + RNames[1] + "_";
                var PlanQty = $('#' + RowName + 'PlanQty').val();
                var Price = $('#' + RowName + 'Price').val();
                $('#' + RowName + 'Amount').val(PlanQty * Price);

            }

            function Save() {

                if ($("#txtBillDate_txtDate").val() == "") {
                    alert("采购日期不能为空，请输入！");
                    $("#txtBillDate_txtDate").focus();
                    return false;
                }
                if ($("#txtID").val() == "") {
                    alert("采购单号不能为空，请输入！");
                    $("#txtID").focus();
                    return false;
                }
                if ($("#txtFactoryID").val() == "") {
                    alert("供应商不能为空，请输入！");
                    $("#txtFactoryName").focus();
                    return false;
                }

                if (!ChkDelMustValue("dgViewSub1", "ProductID", "产品型号"))
                    return false;

                if (!ChkDelMustNumericValue("dgViewSub1", "PlanQty", "计划数量"))
                    return false;
                if (!ChkDelMustValue("dgViewSub1", "PlanDate_txtDate", "预入库日"))
                    return false;


                var blnvalue = true;

                ctls = $("[id$='PlanQty']");
                ctls.each(function () {
                    var rowName = this.id.replace('PlanQty', '');
                    var Qty = $(this).val();
                    var endNo = $('#' + rowName + 'EndNo').val();
                    var StarNo = $('#' + rowName + 'StarNo').val();
                    if (endNo == '' && StarNo == '') {

                    }
                    else {

                        if (endNo == '') {
                            alert("结束序号不能为空，请输入！");
                            $('#' + rowName + 'EndNo').focus();
                            blnvalue = false;
                            return false;
                        }
                        if (endNo.length != 11) {
                            alert("序列号必须是6位年月日+5位流水号，请输入！");
                            $('#' + rowName + 'EndNo').focus();
                            blnvalue = false;
                            return false;
                        }

                        if (StarNo == '') {
                            alert("起始序号不能为空，请输入！");
                            $('#' + rowName + 'EndNo').focus();
                            blnvalue = false;
                            return false;
                        }
                        if (StarNo.length != 11) {
                            alert("序列号必须是6位年月日+5位流水号，请输入！");
                            $('#' + rowName + 'EndNo').focus();
                            blnvalue = false;
                            return false;
                        }
                        var ColorID = $('#' + rowName + 'ColorID').val();
                        if (ColorID == "") {
                            alert("规格不能为空，请输入！");
                            $('#' + rowName + 'ColorName').focus();
                            blnvalue = false;
                            return false;
                        }

                        var q = parseFloat(endNo) - parseFloat(StarNo) + 1;
                        if (Qty != q) {
                            alert("起始序号与结束序号区间数量与计划数量不相等，请重新输入！");
                            $('#' + rowName + 'EndNo').focus();
                            $('#' + rowName + 'EndNo').select();
                            blnvalue = false;
                            return false;
                        }
                    }
                });
                return blnvalue;
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
                                <td align="center" class="musttitle" style="width:6%;">
                                        采购日期
                                </td>
                                <td  style="width:10%;">
                                        &nbsp;<uc1:Calendar ID="txtBillDate" runat="server" />
                                </td>
                                <td  align="center" class="musttitle" style="width:6%;" >
                                        采购单号
                                </td>
                                <td   style=" width:8%">
                                    &nbsp;<asp:TextBox ID="txtID" runat="server" BorderWidth="0" CssClass="TextBox" 
                                        MaxLength="20" Width="92%"></asp:TextBox>
                                          
                                </td>
                                  
                                <td  align="center" class="musttitle" style="width:6%;" >
                                        供应商
                                </td>
                                <td  style=" width:20%">
                                   &nbsp;
                                    <asp:TextBox ID="txtFactoryID" runat="server" BorderWidth="0" CssClass="TextBox" 
                                        Width="0px" Height="0px"></asp:TextBox>
                                    <asp:TextBox ID="txtFactoryName" runat="server" BorderWidth="0" CssClass="TextRead" 
                                        Width="83%" Height="16px"></asp:TextBox>
                                    <asp:Button
                                            ID="btnFactory" Width="20px"  CssClass="ButtonCss" runat="server"  
                                        Text="..." />
                                </td>
                                 <td align="center" class="smalltitle" style="width:6%;">
                                        建单人员
                                </td> 
                                <td     style="width:10%;">
                                       &nbsp;<asp:TextBox ID="txtCreator" runat="server" BorderWidth="0" CssClass="TextRead" 
                                        Width="91%" Height="16px"></asp:TextBox>
                                       
                                </td>
                                <td align="center" class="smalltitle" style="width:6%;">
                                        建单日期
                                </td> 
                                <td      style="width:10%;">
                                        &nbsp;<asp:TextBox ID="txtCreateDate" runat="server" BorderWidth="0" CssClass="TextRead" 
                                        Width="94%" Height="16px"></asp:TextBox>
                                </td>
                            </tr>
                            
			        </table>
			       <div id='tabs'>
                       <div class='x-tab' title='采购明细'>          
                            <table style="width:100%">
                                <tr>
                                    <td class="table_titlebgcolor" height="25px">
                                   
                                        <asp:Button  id="btnAddDetail" CssClass="ButtonCss" runat="server" Text="新增明细" 
                                            style="width:60px;" onclick="btnAddDetail_Click"  />  &nbsp;&nbsp;&nbsp;
                                        <asp:Button  id="btnDelDetail" CssClass="ButtonCss" runat="server" Text="删除明细" 
                                            style="width:60px;" onclick="btnDelDetail_Click" /> 
                                            &nbsp;&nbsp;&nbsp;
                                     <asp:Button ID="btnGetSection" Width="143px"  CssClass="ButtonCss" runat="server"  
                                        Text="获取序号区间" onclick="btnGetSection_Click" />
                                        </td>
                                
                                    <td width="3%">
                                    </td>
                                
                                </tr>
                            </table> 
                    <div id="sub1" style="overflow: auto; width: 100%; height: 340px" >
                        <asp:GridView ID="dgViewSub1" runat="server" AutoGenerateColumns="False" SkinID="GridViewSkin"
                            AllowPaging="True"  Width="1300px" PageSize="10" onrowdatabound="dgViewSub1_RowDataBound" onrowcreated="dgViewSub1_RowCreated">
                            <Columns>
                                <asp:TemplateField  >
                                    <HeaderTemplate>
                                    <input type="checkbox" onclick="selectAll('dgViewSub1',this.checked);" />                    
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                    <asp:CheckBox id="cbSelect" runat="server" ></asp:CheckBox>                   
                                    </ItemTemplate>
                                    <HeaderStyle Width="3%"></HeaderStyle>
                                    <ItemStyle Width="3%" HorizontalAlign="Center"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="序号">
                                    <ItemTemplate>
                                       <asp:Label ID="RowID" runat="server" Text=""></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                    <HeaderStyle Width="4%" ForeColor="Blue" />
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="(产品名称)">
                                     <ItemTemplate>
                                        <%--<asp:Label ID="ProductName" runat="server" Text=""></asp:Label>--%>
                                        <asp:TextBox ID="ProductName" runat="server"  Width="98%" CssClass="detailtext"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle Width="120px" ForeColor="Blue" />
                                </asp:TemplateField>    
                                <asp:TemplateField HeaderText="产品型号">
                                    <ItemTemplate>
                                        <asp:TextBox ID="ProductID" runat="server" Width="0px" CssClass="detailtext"   ></asp:TextBox><asp:TextBox ID="ProductModel" runat="server" Width="70%" CssClass="detailtext"  MaxLength="0"></asp:TextBox><asp:Button
                                            ID="btnProduct"  CssClass="ButtonCss" Width="20%" runat="server"  Text="..." OnClick="btnProduct_Click" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle Width="8%" ForeColor="Blue" />
                                </asp:TemplateField>
                               
                                  <asp:TemplateField HeaderText="(工厂型号)">
                                     <ItemTemplate>
                                        <%--<asp:Label ID="ProductName" runat="server" Text=""></asp:Label>--%>
                                        <asp:TextBox ID="ProductFModel" runat="server"  Width="98%" CssClass="detailtext"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle Width="8%" ForeColor="Blue" />
                                </asp:TemplateField> 
                                <asp:TemplateField HeaderText="规格">
                                     <ItemTemplate>
                                       <%-- <asp:Label ID="ColorName" runat="server" Text=""></asp:Label>--%>
                                          <asp:TextBox ID="ColorID" runat="server" Width="0px" CssClass="detailtext"  MaxLength="1">
                                          </asp:TextBox><asp:TextBox ID="ColorName" runat="server" Width="98%" CssClass="detailtext"></asp:TextBox> 
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle Width="8%" ForeColor="Blue" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="数量">
                                    <ItemTemplate>
                                        <asp:TextBox ID="PlanQty" runat="server" Width="100%" CssClass="detailtext" style="text-align:right;" 
                                        onkeypress="return regInput(this,/^\d+$/,String.fromCharCode(event.keyCode))" 	onpaste="return regInput(this,/^\d+$/,window.clipboardData.getData('Text'))" 
                                        ondrop="return regInput(this,/^\d+$/,event.dataTransfer.getData('Text'))"  onfocus="TextFocus(this);"></asp:TextBox> 
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle Width="6%" ForeColor="Blue" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="单价">
                                    <ItemTemplate>
                                        <asp:TextBox ID="Price" runat="server" Width="100%" CssClass="detailtext" style="text-align:right;" 
                                        onkeypress="return regInput(this,/^\d*\.?\d{0,2}$/,String.fromCharCode(event.keyCode))" 	onpaste="return regInput(this,/^\d*\.?\d{0,2}$/,window.clipboardData.getData('Text'))" 
                                        ondrop="return regInput(this,/^\d*\.?\d{0,2}$/,event.dataTransfer.getData('Text'))"  onfocus="TextFocus(this);"></asp:TextBox> 
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle Width="6%" ForeColor="Blue" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="(金额)">
                                    <ItemTemplate>
                                        <asp:TextBox ID="Amount" runat="server" Width="100%" CssClass="detailtext" style="text-align:right;" 
                                        onkeypress="return regInput(this,/^\d*\.?\d{0,2}$/,String.fromCharCode(event.keyCode))" 	onpaste="return regInput(this,/^\d*\.?\d{0,2}$/,window.clipboardData.getData('Text'))" 
                                        ondrop="return regInput(this,/^\d*\.?\d{0,2}$/,event.dataTransfer.getData('Text'))"  onfocus="TextFocus(this);"></asp:TextBox> 
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle Width="6%" ForeColor="Blue" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="起始序号">
                                    <ItemTemplate>
                                        <asp:TextBox ID="StarNo" runat="server" Width="100%" CssClass="detailtext" 
                                        onkeypress="return regInput(this,/^\d+$/,String.fromCharCode(event.keyCode))" 	onpaste="return regInput(this,/^\d+$/,window.clipboardData.getData('Text'))" 
                                        ondrop="return regInput(this,/^\d+$/,event.dataTransfer.getData('Text'))" MaxLength="11"></asp:TextBox> 
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle Width="140px" ForeColor="Blue" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="结束序号">
                                    <ItemTemplate>
                                        <asp:TextBox ID="EndNo" runat="server" Width="100%" CssClass="detailtext"
                                        onkeypress="return regInput(this,/^\d+$/,String.fromCharCode(event.keyCode))" 	onpaste="return regInput(this,/^\d+$/,window.clipboardData.getData('Text'))" 
                                        ondrop="return regInput(this,/^\d+$/,event.dataTransfer.getData('Text'))"  MaxLength="11"></asp:TextBox> 
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle Width="140px" ForeColor="Blue" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="预入库日">
                                    <ItemTemplate>
                                       <uc1:Calendar ID="PlanDate" runat="server" CssClass="detailtext"/>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle Width="100px" ForeColor="Blue" />
                                </asp:TemplateField>
                                
                                 <asp:TemplateField HeaderText="(实入数量)">
                                    <ItemTemplate>
                                        <asp:TextBox ID="RealQty" runat="server" Width="98%" style="text-align:right;" CssClass="detailtext" ></asp:TextBox> 
                                        <%-- <asp:Literal ID="RealQty" runat="server"  ></asp:Literal>--%>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle Width="80px" ForeColor="Blue"/>
                                </asp:TemplateField>
                            </Columns>
                        
                            <PagerSettings Visible="false" />
                    </asp:GridView>
                    </div>
                     <table width="100%" class="maintable"  bordercolor="#ffffff" border="1" > 
                        <tr>
                            <td align="right">
                                <asp:Label ID="lblCurrentPageSub1" runat="server" ></asp:Label>&nbsp;&nbsp; 
                                <asp:LinkButton ID="btnFirstSub1" runat="server"  
                                            Text="首页" onclick="btnFirstSub1_Click"></asp:LinkButton>  &nbsp;<asp:LinkButton ID="btnPreSub1" runat="server"  
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
                                    &nbsp;<asp:TextBox ID="txtTechRequery" runat="server" CssClass="MultiLineTextBox" 
                                        TextMode="MultiLine" Height="51px" Width="98%"></asp:TextBox>
                                </td>
                            </tr>
                             <tr>
                                <td align="center"  class="smalltitle" style="width:8%;">
                                    功能
                                </td>
                                <td align="left"  class="smalltitle" style="width:92%;" >
                                    &nbsp;<asp:TextBox ID="txtOrderFunction" runat="server" CssClass="MultiLineTextBox" 
                                        TextMode="MultiLine" Height="89px" Width="98%"></asp:TextBox>
                                </td>
                            </tr>
                             <tr>
                                <td align="center"  class="smalltitle" style="width:8%;">
                                    包装要求
                                </td>
                                <td align="left"  class="smalltitle" style="width:92%;" >
                                    &nbsp;<asp:TextBox ID="txtPackRequery" runat="server" CssClass="MultiLineTextBox" 
                                        TextMode="MultiLine" Height="51px" Width="98%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="center"  class="smalltitle" style="width:8%;">
                                    免费随货配件
                                </td>
                                <td align="left"  class="smalltitle" style="width:92%;" >
                                    &nbsp;<asp:TextBox ID="txtOrderParts" runat="server" CssClass="MultiLineTextBox" 
                                        TextMode="MultiLine" Height="51px" Width="98%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="center"  class="smalltitle" style="width:8%;">
                                    备注
                                </td>
                                <td align="left"  class="smalltitle" style="width:92%;" >
                                    &nbsp;<asp:TextBox ID="txtMemo" runat="server" CssClass="MultiLineTextBox" 
                                        TextMode="MultiLine" Height="73px" Width="98%"></asp:TextBox>
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
