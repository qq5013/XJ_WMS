<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeliverScheduleEdit.aspx.cs" Inherits="WMS.WebUI.OutStock.DeliverScheduleEdit" EnableEventValidation = "false" %>

<%@ Register src="../../UserControl/Calendar.ascx" tagname="Calendar" tagprefix="uc1" %>

<%@ Register assembly="Util" namespace="Util" tagprefix="cc1" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head id="Head1" runat="server">
		<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />  
        <title></title> 
        <link href="~/Css/Main.css" type="text/css" rel="stylesheet" /> 
        <link href="~/Css/op.css" type="text/css" rel="stylesheet" /> 
        <link href="~/Css/Detail.css" type="text/css" rel="stylesheet" /> 
        
        <script type="text/javascript" src='<%=ResolveClientUrl("~/JQuery/jquery-1.8.3.min.js") %>'></script>
        <script type="text/javascript" src='<%=ResolveClientUrl("~/JScript/Common.js") %>'></script>
       <script type="text/javascript" src='<%=ResolveClientUrl("~/JScript/DataProcess.js") %>'></script>
       <script type="text/javascript" src='<%=ResolveClientUrl("~/JScript/JsDDL.js") %>'></script>
        <script type="text/javascript">
            $(document).ready(function () {
                BindEvent();
            });
            function content_resize() {

                //編輯頁面 div高度
               

            }
            function BindEvent() {

                $("#btnCustomer").bind("click", function () {
                    GetOtherValue('CMD_CUSTOMER', 'txtCustomerID,txtCustomerName,txtParCustomerID,txtParCustomerName,hdnCustomerType', "CUSTOMER_CODE,CUSTOMER_NAME,PLAYCUSTOMER_CODE,PlayCUSTOMER_NAME,CustomerType", escape("areasation='" + $('#ddlAreaSation').val() + "'"));
                    if ($('#txtCustomerID').val() != "")
                        return true;
                    else
                        return false;
                });
                $("#btnParCust").bind("click", function () {
                    GetOtherValue('CMD_CUSTOMER', 'txtParCustomerID,txtParCustomerName', "CUSTOMER_CODE,CUSTOMER_NAME", "1=1");
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
               

//                $("[ID$='btnColor']").bind("click", function () {
//                    var RNames = this.id.split('_');
//                    var RowName = RNames[0] + "_" + RNames[1] + "_";
//                    var ProductID = $('#' + RowName + 'ProductID').val();
//                    if (ProductID == "") {
//                        alert("请先选择产品资料！");
//                        return false;
//                    }
//                    GetOtherValue('CMD_COLOR', RowName + "ColorID," + RowName + "ColorName", "COLOR_CODE,COLOR_NAME", "PRODUCT_CODE='" + ProductID + "'");
//                    return false;

//                });
            }
            function ChangePrice(obj) {
                var RNames = obj.id.split('_');
                var RowName = RNames[0] + "_" + RNames[1] + "_";
                var PlanQty = $('#' + RowName + 'PlanQty').val();
                var Price = $('#' + RowName + 'Price').val();
                $('#' + RowName + 'Amount').val(PlanQty * Price);
                TotalCount();
            }
            
            function ChangeAutoCode() {

                var Prefix = "BJ";
                Prefix = makePy($('#ddlAreaSation').val().substr(0, 2));
                $('#txtID').val(autoCodeByTableName(Prefix[0], 'Flag=1', 'WMS_DeliverSchedule', 'txtBillDate'));

            }
            function TotalCount() {
                var ctls = $("[id$='PlanQty']");
                var Qty = 0;
                ctls.each(function () {
                    Qty =Qty+ parseFloat($(this).val());
                });
                $('#txtTotalQty').val(Qty);
               var  ctls1 = $("[id$='Amount']");
                Qty = 0;
                ctls1.each(function () {
                    Qty = Qty + parseFloat($(this).val());
                });
                $('#txtTotalCount').val(Qty);
            }

            function Save() {

                if ($("#txtBillDate_txtDate").val() == "") {
                    alert("接单日期不能为空，请输入！");
                    $("#txtBillDate_txtDate").focus();
                    return false;
                }
                if ($("#txtID").val() == "") {
                    alert("销售单号不能为空，请输入！");
                    $("#txtID").focus();
                    return false;
                }
                if ($("#ddlTransport").val() == 8) {
                    if ($("#txtCustPerson").val() == "") {
                        alert("运费结算方式不能为空，请输入！");
                        $("#txtCustPerson").focus();
                        return false;
                    }
                }


                if (!ChkDelMustValue("dgViewSub1", "ProductID", "产品型号"))
                    return false;

                if (!ChkDelMustNumericValue("dgViewSub1", "PlanQty", "计划数量"))
                    return false;
 
                if (!ChkDelMustValue("dgViewSub1", "PlanDate_txtDate", "预出库日"))
                    return false;


                var blnvalue = true;

                ctls = $("[id$='PlanQty']");
                ctls.each(function () {
                    var rowName = this.id.replace('PlanQty', '');
                    var Qty = $(this).val();
                    var RealQty = $('#' + rowName + 'RealQty').val();
                    if (RealQty > Qty) {
                        alert("出库数量必须大于实际出库数量，请修正！");
                        $(this).focus();
                        blnvalue = false;
                        return false;
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
                                <td align="center" class="musttitle" style=" width: 8%">
                                        接单日期
                                </td>
                                <td  style="width:15%">
                                        &nbsp;<uc1:Calendar ID="txtBillDate" runat="server" />
                                </td>
                                <td  align="center" class="musttitle" style="width:8%;" >
                                        销售单号
                                </td>
                                <td style="width:15%" >
                                    &nbsp;<asp:TextBox ID="txtID" runat="server" BorderWidth="0" CssClass="TextBox" 
                                        MaxLength="20" Width="94%"></asp:TextBox>
                                          
                                </td>
                                  
                                <td align="center" class="musttitle" style="width:8%" >
                                   来源单号  
                                </td>
                                  <td  style="width:18%">
                                        &nbsp;<asp:TextBox ID="txtSourceNo" runat="server" BorderWidth="0" CssClass="Textbox" 
                                        Width="93%" Height="16px"></asp:TextBox>
                                </td>
                                
                                <td align="center" class="musttitle" style="width:8%" >
                                     
                                </td>
                                  <td  style="width:18%">
                                        
                                </td>
                               
                            </tr>
                            <tr>
                                 <td align="center" class="musttitle" style="width:8%" >
                                      归属地</td>
                                  <td  style="width:18%">
                                         &nbsp;<asp:DropDownList ID="ddlAreaSation" runat="server" Width="90%">
                                             
                                        </asp:DropDownList></td>
                                <td align="center" style="width:8%;" class="musttitle">
                                        经销商<asp:TextBox ID="txtCustomerID" runat="server" BorderWidth="0" CssClass="TextBox" 
                                            Height="0px" Width="0px"></asp:TextBox>
                                </td>
                                <td  colspan="3"  >
                                         &nbsp;<asp:TextBox ID="txtCustomerName" runat="server" BorderWidth="0" 
                                            CssClass="TextRead" Height="16px" Width="92%"></asp:TextBox><asp:Button ID="btnCustomer" runat="server" CssClass="ButtonCss" Text="..." 
                                            Width="20px" onclick="btnCustomer_Click" />
                                      
                                </td>
                               
                                
                                <td  align="center" class="musttitle" style="width:8%;" >
                                    子公司&nbsp;&nbsp;<asp:TextBox ID="txtParCustomerID" runat="server" BorderWidth="0" CssClass="TextBox" 
                                            Height="10px" Width="0px"></asp:TextBox>
                                </td>
                                <td  >
                                    &nbsp;<asp:TextBox ID="txtParCustomerName" runat="server" BorderWidth="0" 
                                        CssClass="TextRead" Height="16px" Width="80%"></asp:TextBox>
                                    <asp:Button ID="btnParCust" runat="server" CssClass="ButtonCss" Text="..." 
                                        Width="20px" />
                                </td>
                                
                                
                                
                                
                            </tr>
                           <tr>
                               
                                    <td style="width:8%;" align="center" class="smalltitle" >
                                        收货人</td>
                                    <td  >
                                         &nbsp;<cc1:DDL ID="txtLinkPerson" runat="server" Width="93%" />
                                      
                                         
                                </td>
                                <td align="center" class="smalltitle" style="width:8%">
                                    收货电话 
                                </td>
                                <td   style="width:18%">
                                     &nbsp;<cc1:DDL ID="txtLinkPhone" runat="server" Width="91%" />
                                </td >
                                 <td align="center" style="width:8%;" class="smalltitle">
                                        收货地址
                                  
                                    
                                </td> 
                                <td  colspan="3">
                                    &nbsp;<cc1:DDL ID="txtLinkAddress" runat="server" Width="97%" />
                                      
                                    
                                 </td>
                            </tr>
                            
                           
                             <tr>
                              <td align="center"  style="width:8%;" class="musttitle">
                                    运费结算
                                 </td>
                                <td  colspan="3" >
                                     &nbsp;<asp:DropDownList ID="ddlTransport" runat="server" Width="45%">
                                             <asp:ListItem Value="1">月结送货上门</asp:ListItem>
                                             <asp:ListItem Value="2">月结送货入户</asp:ListItem>
                                             <asp:ListItem Value="3">售后仓自提</asp:ListItem>
                                             <asp:ListItem Value="4">前埔仓自提</asp:ListItem>
                                             <asp:ListItem Value="5">运费到付</asp:ListItem>
                                             <asp:ListItem Value="6">快递月结</asp:ListItem>
                                             <asp:ListItem Value="7">快递到付</asp:ListItem>
                                             <asp:ListItem Value="8">其他</asp:ListItem>
                                        </asp:DropDownList> &nbsp;<asp:TextBox ID="txtCustPerson" runat="server" BorderWidth="0" 
                                        CssClass="TextBox" Height="16px" Width="45%"></asp:TextBox>
                              
                                 </td>
                               
                               
                                 
                                 <td align="center" class="smalltitle" style="width:8%;">
                                        建单人员</td> 
                                <td    style="width:18%">
                                       &nbsp; 
                                       <asp:TextBox ID="txtCreator" runat="server" BorderWidth="0" CssClass="TextRead" 
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
                                    &nbsp;<asp:TextBox ID="txtMemo" runat="server" CssClass="MultiLineTextBox" 
                                        TextMode="MultiLine" Height="51px" Width="99%"></asp:TextBox>
                                </td>
                            </tr>
                             		
			        </table>
			      
                    <table style="width:100%">
                        <tr>
                        
                       
                        
                            <td class="table_titlebgcolor" height="25px">
                                <asp:Button  id="btnAddDetail" CssClass="ButtonCss" runat="server" Text="新增明细" 
                                    style="width:60px;" onclick="btnAddDetail_Click"  />  
                         &nbsp;&nbsp;<asp:Button  id="btnInsert" CssClass="ButtonCss" runat="server" Text="插入明细" 
                                    style="width:60px;" onclick="btnInsert_Click"   />
                                    &nbsp;&nbsp;<asp:Button  id="btnDelDetail" CssClass="ButtonCss" runat="server" Text="删除明细" 
                                    style="width:60px;" onclick="btnDelDetail_Click" />
                               
                        </td>
                        </tr>
                    </table> 
                    <div id="Sub-container" style="overflow: auto; width: 100%; height: 280px" >
                        <asp:GridView ID="dgViewSub1" runat="server" AutoGenerateColumns="False" SkinID="GridViewSkin"
                            AllowPaging="True" Width="98%" PageSize="10" 
                            onrowdatabound="dgViewSub1_RowDataBound" onrowcreated="dgViewSub1_RowCreated" >
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
                                        <asp:TextBox ID="ProductName" runat="server"  Width="98%" CssClass="detailtext"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle Width="11%" ForeColor="Blue" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="产品型号">
                                    <ItemTemplate>
                                        <asp:TextBox ID="ProductID" runat="server" Width="0px" CssClass="detailtext" ></asp:TextBox><asp:TextBox ID="ProductModel" runat="server" Width="70%" CssClass="detailtext"  MaxLength="0"></asp:TextBox><asp:Button
                                            ID="btnProduct"  CssClass="ButtonCss" Width="20%" runat="server"  Text="..." OnClick="btnProduct_Click" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle Width="10%" ForeColor="Blue" />
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="工厂型号">
                                    <ItemTemplate>
                                        <asp:TextBox ID="ProductFModel" runat="server" Width="98%"  CssClass="detailtext" ></asp:TextBox> 
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle  Width="10%"  ForeColor="Blue"/>
                                </asp:TemplateField>
                                  
                                <asp:TemplateField HeaderText="规格">
                                     <ItemTemplate>
                                       <%-- <asp:Label ID="ColorName" runat="server" Text=""></asp:Label>--%>
                                          <asp:TextBox ID="ColorID" runat="server" Width="0px" CssClass="detailtext"  MaxLength="1">
                                          </asp:TextBox><asp:TextBox ID="ColorName" runat="server" Width="90%" CssClass="detailtext"></asp:TextBox> 
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle Width="8%" ForeColor="Blue" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="数量">
                                    <ItemTemplate>
                                        <asp:TextBox ID="PlanQty" runat="server" Width="100%" CssClass="detailtext" style="text-align:right;" 
                                        onkeypress="return regInput(this,/^\d+$/,String.fromCharCode(event.keyCode))" 	onpaste="return regInput(this,/^\d+$/,window.clipboardData.getData('Text'))" 
                                        ondrop="return regInput(this,/^\d+$/,event.dataTransfer.getData('Text'))" onfocus="TextFocus(this);"></asp:TextBox> 
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle Width="8%" ForeColor="Blue" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="单价">
                                    <ItemTemplate>
                                        <asp:TextBox ID="Price" runat="server" Width="100%" CssClass="detailtext" style="text-align:right;" 
                                        onkeypress="return regInput(this,/^\d*\.?\d{0,2}$/,String.fromCharCode(event.keyCode))" 	onpaste="return regInput(this,/^\d*\.?\d{0,2}$/,window.clipboardData.getData('Text'))" 
                                        ondrop="return regInput(this,/^\d*\.?\d{0,2}$/,event.dataTransfer.getData('Text'))"  onfocus="TextFocus(this);"></asp:TextBox> 
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle Width="8%" ForeColor="Blue" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="(金额)">
                                    <ItemTemplate>
                                        <asp:TextBox ID="Amount" runat="server" Width="100%" CssClass="detailtext" style="text-align:right;" 
                                        onkeypress="return regInput(this,/^\d*\.?\d{0,2}$/,String.fromCharCode(event.keyCode))" 	onpaste="return regInput(this,/^\d*\.?\d{0,2}$/,window.clipboardData.getData('Text'))" 
                                        ondrop="return regInput(this,/^\d*\.?\d{0,2}$/,event.dataTransfer.getData('Text'))"  onfocus="TextFocus(this);"></asp:TextBox> 
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle Width="10%" ForeColor="Blue" />
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="预出货日">
                                    <ItemTemplate>
                                       <uc1:Calendar ID="PlanDate" runat="server" CssClass="detailtext"/>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle Width="100px" ForeColor="Blue" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="答复交期">
                                    <ItemTemplate>
                                        <asp:TextBox ID="Memo" runat="server" Width="98%" CssClass="detailtext"   onfocus="TextFocus(this);"></asp:TextBox> 
                                        <%-- <asp:Literal ID="RealQty" runat="server"  ></asp:Literal>--%>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle Width="10%" ForeColor="Blue"/>
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="(实际排单数量)">
                                    <ItemTemplate>
                                        <asp:TextBox ID="RealQty" runat="server" Width="98%" style="text-align:right;" CssClass="detailtext" ></asp:TextBox> 
                                        <%-- <asp:Literal ID="RealQty" runat="server"  ></asp:Literal>--%>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle   ForeColor="Blue"/>
                                </asp:TemplateField>
                            </Columns>
                        
                            <PagerSettings Visible="false" />
                    </asp:GridView>
                 </div>
             
                 <table width="100%" class=" maintable" > 
                    <tr>
                         <td align="center"  style="width:8%;" class="smalltitle">
                                数量合计
                            </td>
                            <td style="width:15%">
                                &nbsp;<asp:TextBox ID="txtTotalQty" runat="server" BorderWidth="0" 
                                    CssClass="TextRead" Height="16px" ReadOnly="True" Width="83%" 
                                    style="text-align:right"></asp:TextBox>
                            </td>
                            <td align="center"  style="width:8%;" class="smalltitle">
                                金额合计
                            </td>
                            <td style="width:15%">
                                &nbsp;<asp:TextBox ID="txtTotalCount" runat="server" BorderWidth="0" 
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
                            <asp:Literal ID="Literal1" Text="每页" runat="server"  Visible="false" />
                                <asp:DropDownList ID="ddlPageSizeSub1" runat="server" AutoPostBack="True"  
                                Height="16px" onselectedindexchanged="ddlPageSizeSub1_SelectedIndexChanged" Visible="false" >
                    
                                </asp:DropDownList>
                        </td>
                        <td width="3%">
                        </td>
                    </tr>
                  </table>
                    <input type="hidden" runat="server" id="hdnCustomerType" /> 
                    <input type="hidden" runat="server" id="hdnMulSelect" /> 
                </div>
                  </ContentTemplate>
              </asp:UpdatePanel>       
             
             
		</form>
	</body>
</html>
