<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MigrationEdit.aspx.cs" Inherits="WMS.WebUI.Stock.MigrationEdit" %>

<%@ Register src="../../UserControl/Calendar.ascx" tagname="Calendar" tagprefix="uc1" %>

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

        <script type="text/javascript">
            $(document).ready(function () {
                BindEvent();
            });
            function content_resize() {

                //編輯頁面 div高度
//                var div = $("#surdiv");
//                var h = 300;
//                if ($(window).height() <= 0) {
//                    h = document.body.clientHeight - 35;
//                }
//                else {
//                    h = $(window).height() - 35;
//                }
//                $("#surdiv").css("height", h);

//                $("#Sub-container").css("height", h - 150); //设置S界面多明细设置  

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

            }
            function changeItem(s) {
                if (s == "0")
                    $('#ddlStockFunction').val(2);
                else
                    $('#ddlStockFunction').val(3);
            }

            function Save() {

                if ($("#txtBillDate_txtDate").val() == "") {
                    alert("移库日期不能为空，请输入！");
                    $("#txtBillDate_txtDate").focus();
                    return false;
                }
                if ($("#txtID").val() == "") {
                    alert("移库单号不能为空，请输入！");
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

                var StockType = $('#ddlStockType').val();
                if (StockType == '0') {
                    if (!ChkDelMustValue("dgViewSub1", "Qty", "数量"))
                        return false;
                }
                else {
                    if (!ChkDelMustValue("dgViewSub1", "StarNo", "起始序号"))
                        return false;
                    if (!ChkDelMustValue("dgViewSub1", "EndNo", "结束序号"))
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
                                        返修日期
                                </td>
                                <td  style="width:15%">
                                        &nbsp;<uc1:Calendar ID="txtBillDate" runat="server" />
                                </td>
                                <td  align="center" class="musttitle" style="width:8%;" >
                                        返修单号
                                </td>
                                <td style="width:12%" >
                                    &nbsp;<asp:TextBox ID="txtID" runat="server" BorderWidth="0" CssClass="TextBox" 
                                        MaxLength="20" Width="94%"></asp:TextBox>
                                          
                                </td>
                                  
                                <td align="center" class="musttitle" style="width:8%" >
                                    返修供应商</td>
                                 <td  style="width:28%">
                                     &nbsp;
                                    <asp:TextBox ID="txtFactoryID" runat="server" BorderWidth="0" CssClass="TextBox" 
                                        Width="0px" Height="0px"></asp:TextBox>
                                    <asp:TextBox ID="txtFactoryName" runat="server" BorderWidth="0" CssClass="TextRead" 
                                        Width="83%" Height="16px"></asp:TextBox>
                                    <asp:Button
                                            ID="btnFactory" Width="20px"  CssClass="ButtonCss" runat="server"  
                                        Text="..." />
                                </td>
                                 <td   class="musttitle" align="center"  style=" width:8%"  >
                                      
                                     出库方式</td>
                                 <td  style=" width:10%"  >
                                      
                                     <asp:DropDownList ID="ddlStockType" runat="server" Height="28px" Width="90%" >
                                         <asp:ListItem Value="0" Selected="True">正常出库</asp:ListItem>
                                         <asp:ListItem Value="1">批次移库</asp:ListItem>
                                         
                                     </asp:DropDownList>
                                      
                                </td>
                            </tr>
                            <tr>
                                <td align="center"  class="smalltitle" style="width:8%;">
                                    备注
                                </td>
                                <td colspan="7">
                                    &nbsp;<asp:TextBox ID="txtMemo" runat="server" CssClass="MultiLineTextBox" 
                                        TextMode="MultiLine" Height="37px" Width="99%"></asp:TextBox>
                                </td>
                            </tr>
                             <tr>
                                <td align="center" class="smalltitle" style="width:10%;">
                                        建单人员
                                </td> 
                                <td     style="width:10%;">
                                       &nbsp;<asp:TextBox ID="txtCreator" runat="server" BorderWidth="0" CssClass="TextRead" 
                                        Width="91%" Height="16px"></asp:TextBox>
                                </td>
                                <td align="center" class="smalltitle" style="width:10%;">
                                        建单日期
                                </td> 
                                <td    style="width:10%;">
                                        &nbsp;<asp:TextBox ID="txtCreateDate" runat="server" BorderWidth="0" CssClass="TextRead" 
                                        Width="92%" Height="16px"></asp:TextBox>
                                </td>
                                 
                                 <td  colspan="4">
                                        
                                </td> 
                                 
                            </tr>
                             		
			        </table>
			      
                    <table style="width:100%">
                        <tr>
                            <td class="table_titlebgcolor" height="25px">
                                   
                                <asp:Button  id="btnAddDetail" CssClass="ButtonCss" runat="server" Text="新增明细" 
                                    style="width:60px;" onclick="btnAddDetail_Click"  /> &nbsp;
                                <asp:Button  id="btnDelDetail" CssClass="ButtonCss" runat="server" Text="删除明细" 
                                    style="width:60px;" onclick="btnDelDetail_Click" />
                                     &nbsp;
                                </td>
                                
                        </tr>
                    </table> 
                    <div id="Sub-container" style="overflow: auto; width: 100%; height: 370px" >
                        <asp:GridView ID="dgViewSub1" runat="server" AutoGenerateColumns="False" SkinID="GridViewSkin"
                            AllowPaging="True" Width="98%" PageSize="10" onrowdatabound="dgViewSub1_RowDataBound" 
                                >
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
                            
                                <asp:TemplateField HeaderText="产品型号">
                                    <ItemTemplate>
                                        <asp:TextBox ID="ProductID" runat="server" Width="0px" CssClass="detailtext" ></asp:TextBox><asp:TextBox ID="ProductModel" runat="server" Width="70%" CssClass="detailtext"  MaxLength="0"></asp:TextBox><asp:Button
                                            ID="btnProduct"  CssClass="ButtonCss" Width="20%" runat="server"  Text="..." OnClick="btnProduct_Click" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle Width="8%" ForeColor="Blue" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="(产品名称)">
                                     <ItemTemplate>
                                        <%--<asp:Label ID="ProductName" runat="server" Text=""></asp:Label>--%>
                                        <asp:TextBox ID="ProductName" runat="server"  Width="98%" CssClass="detailtext"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle Width="160px" ForeColor="Blue" />
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
                                        <asp:TextBox ID="Qty" runat="server" Width="100%" CssClass="detailtext" style="text-align:right;" 
                                        onkeypress="return regInput(this,/^\d+$/,String.fromCharCode(event.keyCode))" 	onpaste="return regInput(this,/^\d+$/,window.clipboardData.getData('Text'))" 
                                        ondrop="return regInput(this,/^\d+$/,event.dataTransfer.getData('Text'))"></asp:TextBox> 
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
                            </Columns>
                        
                            <PagerSettings Visible="false" />
                    </asp:GridView>
                 </div>
             
                 <table width="100%" class="maintable"  bordercolor="#ffffff" border="1" > 
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
                            <asp:Literal ID="Literal1" Text="每页" runat="server"   Visible="false"/>
                                <asp:DropDownList ID="ddlPageSizeSub1" runat="server" AutoPostBack="True"  
                                Height="16px" onselectedindexchanged="ddlPageSizeSub1_SelectedIndexChanged" Visible="false">
                    
                                </asp:DropDownList>
                        </td>
                        <td width="3%">
                        </td>
                    </tr>
                  </table>
                 </div>
                    <input type="hidden" runat="server" id="hdnMulSelect" /> 
                  </ContentTemplate>
              </asp:UpdatePanel>       
             
             
		</form>
	</body>
</html>
