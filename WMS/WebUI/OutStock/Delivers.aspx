<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Delivers.aspx.cs" Inherits="WMS.WebUI.OutStock.Delivers"  culture="auto" uiculture="auto" MaintainScrollPositionOnPostback="true" %>
<%@ Register src="../../UserControl/Calendar.ascx" tagname="Calendar" tagprefix="uc1" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <title></title> 
    <link href="~/Css/Main.css" type="text/css" rel="stylesheet" /> 
    <link href="~/Css/op.css" type="text/css" rel="stylesheet" /> 
     <script type="text/javascript" src='<%=ResolveClientUrl("~/JQuery/jquery-1.8.3.min.js") %>'></script>
    <script type="text/javascript" src='<%=ResolveClientUrl("~/JScript/Common.js") %>'></script>
    <script type="text/javascript" src='<%=ResolveClientUrl("~/JScript/DataProcess.js") %>'></script>
     
    <script type="text/javascript">
        $(document).ready(function () {
            BindEvent();
        });
        function BindEvent() {
            $("#btnFact").bind("click", function () {
                var where = "1=1";
                if ($("#ddlAreaSation").val() != "请选择") {
                    where = escape("areasation='" + $('#ddlAreaSation').val() + "'");
                }

                getMultiItems("CMD_CUSTOMER", "CUSTOMER_CODE", this, '#hdnFact', where);
                return false;
            });
            $("#txtFact").bind("dblclick", function () {
                var where = "1=1";
                if ($("#ddlAreaSation").val() != "请选择") {
                    where = escape("areasation='" + $('#ddlAreaSation').val() + "'");
                }
                GetOtherValueNullClear('CMD_CUSTOMER', 'txtFactID,txtFact', "CUSTOMER_CODE,CUSTOMER_NAME", where);
                return false;
            });

            $("#btnProduct").bind("click", function () {
                getMultiItems("CMD_Product", "PRODUCT_CODE", this, '#HdnProduct');
                return false;
            });
            $("#txtProductModule").bind("dblclick", function () {
                GetOtherValueNullClear("CMD_Product", "txtProductModule,txtProductID", "PRODUCT_MODEL,PRODUCT_CODE", "1=1");
                return false;
            });
            //规格
            $("#btnColor").bind("click", function () {
                var ProductID = "";
                if ($("#btnProduct").val() == "指定") {
                    if ($('#txtProductID').val() == "") {
                        alert("请先选择产品型号！");
                        return false;
                    }
                    ProductID = "'" + $('#txtProductID').val() + "'";
                }
                else {
                    ProductID = $('#HdnProduct').val();
                }

                getMultiItems("CMD_COLOR", "COLOR_CODE", this, '#hdnColor', "PRODUCT_CODE in (" + ProductID + ")");
                return false;
            });
            $("#txtColor").bind("dblclick", function () {
                var ProductID = "";
                if ($("#btnProduct").val() == "指定") {
                    if ($('#txtProductID').val() == "") {
                        alert("请先选择产品型号！");
                        return false;
                    }
                    ProductID = "'" + $('#txtProductID').val() + "'";
                }
                else {
                    ProductID = $('#HdnProduct').val();
                }
                GetOtherValueNullClear('CMD_COLOR', "txtColorID,txtColor", "COLOR_CODE,COLOR_NAME", "PRODUCT_CODE in (" + ProductID + ")");
                return false;
            });
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
             <div>
                    <table style="width: 100%; height: 20px;" class="maintable"  cellspacing="0" cellpadding="0" width="100%" align="center" bordercolor="#ffffff"	border="1">
                    <tr>
                        <td  class="smalltitle" align="right" style="height: 24px; width:75px">
                           产品型号 <asp:textbox id="txtProductID"   runat="server" Width="0px" CssClass="TextBox" heigth="0px" style="display:none" ></asp:textbox>
                        </td>
                        <td align="left" style="height: 24px; width:100px ">
                          &nbsp;<asp:textbox id="txtProductModule" tabIndex="1" runat="server" Width="92%" CssClass="TextBox" heigth="16px"  AutoCompleteType="Disabled"></asp:textbox>
                        </td>
                        <td align="left" colspan="2"  >
                               <asp:Button ID="btnProduct" runat="server" CssClass="but" Text="指定" Width="70px" Height="23px" />
                        </td>
                        <td class="smalltitle" align="right" style="height: 24px; width:80px">
                           规格<asp:textbox id="txtColorID" tabIndex="1" runat="server" Width="0px" CssClass="TextBox" heigth="0px" style="display:none"></asp:textbox>
                        </td>
                        <td align="left" style="height: 24px;width:75px ">
                           &nbsp;<asp:textbox id="txtColor" tabIndex="1" runat="server" Width="93%" 
                                CssClass="TextBox" heigth="16px" AutoCompleteType="Disabled"></asp:textbox>
                        </td>
                        <td align="left" style="height: 24px;width:75px " >
                            <asp:Button ID="btnColor" runat="server" CssClass="but" Text="指定" Width="70px" Height="23px" />
                        </td>
                         <td class="smalltitle" align="right" style="height: 24px; width:80px">
                            归属地 
                        </td>
                        <td  colspan="2">
                            <asp:DropDownList ID="ddlAreaSation" runat="server" Width="90%">
                            </asp:DropDownList>
                        </td>
                        
                        
                        <td class="smalltitle" align="right" style="height: 24px; width:80px"  >
                           经销商 <asp:textbox id="txtFactID" tabIndex="1" runat="server" Width="0px" CssClass="TextBox" heigth="0px" style="display:none"></asp:textbox>
                        </td>

                        <td align="left" colspan="2"  >
                          &nbsp;<asp:textbox id="txtFact" tabIndex="1" runat="server" Width="92%" CssClass="TextBox" heigth="16px" AutoCompleteType="Disabled"></asp:textbox>
                        </td>
                        <td align="left" style="height: 24px;width:80px " >
                            <asp:Button ID="btnFact" runat="server" CssClass="but" Text="指定"   Width="70px" Height="22px" />
                        </td>
                       
                    </tr>
                    <tr>
                          <td class="smalltitle" align="right" style="height: 24px; width:75px">
                            排单日期 
                        </td>
                        <td align="left" style="height: 24px; width:100px ">
                            &nbsp; <uc1:Calendar ID="txtStartDate" runat="server" />
                        </td>
                        <td align="center" style="height: 24px;width:20px  " >
                         To 
                        </td>
                        <td  align="left" style="height: 24px; width:100px ">
                          &nbsp;<uc1:Calendar ID="txtEndDate" runat="server" />
                        </td>
                        <td class="smalltitle" align="right" style="height: 24px; width:75px"  >
                           出库方式
                        </td>
                         <td  style="height: 24px;width:80px ">
                          <asp:DropDownList ID="ddlDriverType" runat="server" Width="90%">
                                    <asp:ListItem Value="2">请选择</asp:ListItem>
                                    <asp:ListItem Value="0">库存单</asp:ListItem>
                                    <asp:ListItem Value="1">直出单</asp:ListItem>
                            </asp:DropDownList>
                        </td>

                        <td class="smalltitle" align="right" style="height: 24px; width:75px">
                           销售订单
                        </td>
                        <td align="left" style="height: 24px;width:80px " >
                           &nbsp;<asp:textbox id="txtScheduleNo" tabIndex="1" runat="server" Width="93%" CssClass="TextBox" heigth="16px" ></asp:textbox>
                        </td>
                         
                         <td class="smalltitle" align="right" style="height: 24px; width:80px">
                           收货人
                        </td>
                         <td align="left" style="height: 24px;width:80px ">
                           &nbsp;<asp:textbox id="txtLinkPerson" tabIndex="1" runat="server" Width="92%" 
                                 CssClass="TextBox" heigth="16px" ></asp:textbox>
                        </td>
                        <td class="smalltitle" align="right" style="height: 24px; width:80px">
                           状态
                        </td>
                        <td   >
                           &nbsp; <asp:RadioButton ID="opt1" runat="server" Checked="True" GroupName="LabelSource" Text="全部" />&nbsp;
                           <asp:RadioButton ID="opt2" runat="server" GroupName="LabelSource" Text="已审" />&nbsp;
                           <asp:RadioButton ID="opt3" runat="server" GroupName="LabelSource" Text="未审" />&nbsp;
                                    
                        </td>
                        <td class="smalltitle" align="right" style="height: 24px; width:70px">
                            发货单
                        </td>
                        <td align="left" style="height: 24px;width:80px ">
                             &nbsp;<asp:textbox id="txtBillID" tabIndex="1" runat="server" Width="93%" CssClass="TextBox" heigth="16px" ></asp:textbox>
                        </td>

                    </tr>

                    <tr>
                        <td align="right" colspan="14">
                         <asp:button id="btnSearch" tabIndex="2" runat="server" Width="58px" CssClass="ButtonQuery" Text="查询" onclick="btnSearch_Click" />&nbsp;&nbsp;
                         <asp:Button ID="btnRefresh" runat="server" CssClass="ButtonRefresh" onclick="btnSearch_Click" OnClientClick="return Refresh()" tabIndex="2" Text="刷新" Width="58px" /> &nbsp;&nbsp;
                          <asp:Button ID="btnAdd" runat="server" Text="新增" CssClass="ButtonCreate"  OnClientClick="Add();"  />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                          <asp:Button ID="btnDelete" runat="server" Text="刪除" CssClass="ButtonDel" onclick="btnDeletet_Click" OnClientClick="return Delete('gvMain')" Width="51px"/> &nbsp;&nbsp;
                         <asp:Button ID="btnExit" runat="server" Text="离开" CssClass="ButtonExit" OnClientClick="return Exit()" Width="51px" />
                        </td>
                    </tr>
                
                 
                </table>
                </div>   
                
           <div id="Maincontainer" style="overflow:auto; WIDTH: 100%; height:245px;"  runat="server" onscroll="javascript:RecordPostion(this);">
                
            <asp:GridView ID="gvMain" runat="server" AutoGenerateColumns="False" 
                    SkinID="GridViewSkin" Width="98%" 
                    OnRowDataBound="gvMain_RowDataBound">
            <Columns>
                <asp:TemplateField  >
                    <HeaderTemplate>
                    <input type="checkbox" onclick="selectAll('gvMain',this.checked);" />                    
                    </HeaderTemplate>
                    <ItemTemplate>
                    <asp:CheckBox id="cbSelect" runat="server" ></asp:CheckBox>                   
                    </ItemTemplate>
                  <HeaderStyle Width="60px"></HeaderStyle>
                 <ItemStyle Width="60px" HorizontalAlign="Center"></ItemStyle>
               </asp:TemplateField>
               <asp:TemplateField HeaderText="发货通知单"  SortExpression="BillID" >
                    <ItemTemplate>
                    <asp:LinkButton  id="LinkButton1" runat="server" OnClientClick="return ShowViewForm(this);" Text='<%# DataBinder.Eval(Container.DataItem, "BillID") %>' >
                    </asp:LinkButton>                       
                    </ItemTemplate>
                  <ItemStyle Width="10%" Wrap="False" HorizontalAlign="Left"/>
                  <HeaderStyle Width="10%" Wrap="False" />
               </asp:TemplateField>
                <asp:TemplateField HeaderText="排单日期" SortExpression="BillDate">
                    <ItemTemplate>
                        <%# ToYMD(DataBinder.Eval(Container.DataItem, "BillDate"))%>
                    </ItemTemplate>
                    <HeaderStyle Wrap="False" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="False" />
                </asp:TemplateField> 
                   
                 <asp:BoundField DataField="ScheduleNo" HeaderText="销售单号" 
                    SortExpression="ScheduleNo"  >
                    <ItemStyle HorizontalAlign="Left" Width="15%" Wrap="False" />
                    <HeaderStyle Wrap="False" />
                </asp:BoundField>
                 <asp:BoundField DataField="TaskChecker" HeaderText="审核人员" 
                    SortExpression="TaskChecker"  >
                    <ItemStyle HorizontalAlign="Left" Width="15%" Wrap="False" />
                    <HeaderStyle Wrap="False" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="审核日期" SortExpression="TaskCheckDate">
                    <ItemTemplate>
                        <%# ToYMD(DataBinder.Eval(Container.DataItem, "TaskCheckDate"))%>
                    </ItemTemplate>
                    <HeaderStyle Wrap="False" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="False" />
                </asp:TemplateField>
                  <asp:BoundField DataField="DriverType" HeaderText="出库方式" 
                    SortExpression="DriverType"  >
                    <ItemStyle HorizontalAlign="Left" Width="15%" Wrap="False" />
                    <HeaderStyle Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="CustName" HeaderText="经销商" 
                    SortExpression="CustName"  >
                    <ItemStyle HorizontalAlign="Left" Width="15%" Wrap="False" />
                    <HeaderStyle Wrap="False" />
                </asp:BoundField>
                 
                 
                 <asp:BoundField DataField="LinkPerson" HeaderText="收货人" 
                    SortExpression="LinkPerson"  >
                    <ItemStyle HorizontalAlign="Left" Width="15%" Wrap="False" />
                    <HeaderStyle Wrap="False" />
                </asp:BoundField>
                 <asp:BoundField DataField="LinkPhone" HeaderText="收货电话" 
                    SortExpression="LinkPhone"  >
                    <ItemStyle HorizontalAlign="Left" Width="15%" Wrap="False" />
                    <HeaderStyle Wrap="False" />
                </asp:BoundField>
                 <asp:BoundField DataField="LinkAddress" HeaderText="收货地址" 
                    SortExpression="LinkAddress"  >
                    <ItemStyle HorizontalAlign="Left" Width="15%" Wrap="False" />
                    <HeaderStyle Wrap="False" />
                </asp:BoundField>
                 <asp:BoundField DataField="Creator" HeaderText="建单人员" 
                    SortExpression="Creator"  >
                    <ItemStyle HorizontalAlign="Left" Width="15%" Wrap="False" />
                    <HeaderStyle Wrap="False" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="建单日期" SortExpression="CreateDate">
                    <ItemTemplate>
                        <%# ToYMD(DataBinder.Eval(Container.DataItem, "CreateDate"))%>
                    </ItemTemplate>
                    <HeaderStyle Wrap="False" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="False" />
                </asp:TemplateField>
               <%-- <asp:BoundField DataField="Updater" HeaderText="异动人员" 
                    SortExpression="Updater"  >
                    <ItemStyle HorizontalAlign="Left" Width="15%" Wrap="False" />
                    <HeaderStyle Wrap="False" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="异动日期" SortExpression="UpdateDate">
                    <ItemTemplate>
                        <%# ToYMD(DataBinder.Eval(Container.DataItem, "UpdateDate"))%>
                    </ItemTemplate>
                    <HeaderStyle Wrap="False" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="False" />
                </asp:TemplateField>--%>
               
                

                <asp:BoundField DataField="Memo" HeaderText="备注" 
                    SortExpression="Memo"  >
                    <ItemStyle HorizontalAlign="Left" Width="15%" Wrap="False" />
                    <HeaderStyle Wrap="False" />
                </asp:BoundField>
            </Columns>
            <PagerSettings Visible="False" />
        </asp:GridView>
            </div>
            <div class="maintable">
             <asp:LinkButton ID="btnFirst" runat="server" OnClick="btnFirst_Click" Text="首页"></asp:LinkButton> 
            &nbsp;<asp:LinkButton ID="btnPre" runat="server" OnClick="btnPre_Click" Text="上一页"></asp:LinkButton> 
            &nbsp;<asp:LinkButton ID="btnNext" runat="server" OnClick="btnNext_Click" Text="下一页"></asp:LinkButton> 
            &nbsp;<asp:LinkButton ID="btnLast" runat="server" OnClick="btnLast_Click" Text="尾页"></asp:LinkButton> 
            &nbsp;<asp:textbox id="txtPageNo" onkeypress="return regInput(this,/^\d+$/,String.fromCharCode(event.keyCode))"
					onpaste="return regInput(this,/^\d+$/,window.clipboardData.getData('Text'))" ondrop="return regInput(this,/^\d+$/,event.dataTransfer.getData('Text'))"
					runat="server" Width="56px" CssClass="TextBox" ></asp:textbox>
            &nbsp;<asp:linkbutton id="btnToPage" runat="server" onclick="btnToPage_Click" Text="跳转"></asp:linkbutton>
            &nbsp;<asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="True" onselectedindexchanged="ddlPageSize_SelectedIndexChanged" Visible="false"></asp:DropDownList>
            &nbsp;<asp:Label ID="lblCurrentPage" runat="server" ></asp:Label>
        </div>

          <div >
                <table class="maintable" cellpadding="0" cellspacing="0" style="width: 100%;">
                    <tr>
                        <td valign="middle" align="left" height="22" >
                            <b>出库单明细</b>
                        </td>
                    </tr>
                </table>
            
            <div id="Sub-container" style="overflow: auto; width: 100%; height: 150px" >
              <asp:GridView ID="dgViewSub1" runat="server" AutoGenerateColumns="False" SkinID="GridViewSkin"
                            AllowPaging="True" Width="98%" PageSize="10" 
                            onrowdatabound="dgViewSub1_RowDataBound">
                         <Columns>  
                             <asp:BoundField DataField="RowID" HeaderText="序号" >
                                <ItemStyle HorizontalAlign="Center" Width="4%" Wrap="False" />
                                <HeaderStyle Wrap="False" />
                            </asp:BoundField>
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
                             <asp:BoundField DataField="InStockQty" HeaderText="数量" >
                                <ItemStyle HorizontalAlign="right" Width="8%" Wrap="False" />
                                <HeaderStyle Wrap="False" />
                            </asp:BoundField> 
                             
                                 
                              <asp:BoundField DataField="ScanQty" HeaderText="实出数量" >
                                <ItemStyle HorizontalAlign="right" Width="8%" Wrap="False" />
                                <HeaderStyle Wrap="False" />
                            </asp:BoundField>  
                        </Columns>
                        <PagerSettings Visible="false" />
                    </asp:GridView>
            </div>
             <table class="maintable" width="100%"> 
                        <tr>
                            <td align="right">
                              
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
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="lblCurrentPageSub1" runat="server" ></asp:Label>
                                </td>
                            <td width="3%">
                            </td>
                        </tr>
                   </table>
            </div>
              <div style="font-size: 0px; bottom: 0px; right: 0px;">
                <asp:Button ID="btnReload" runat="server" Text="" OnClick="btnReload_Click" Height="0px" Width="0px" />
                <asp:HiddenField ID="hdnRowIndex" runat="server" Value="0" />
               <asp:HiddenField ID="hdnRowValue" runat="server"  />
                <input type="hidden" id="dvscrollX" runat="server" />
                <input type="hidden" id="dvscrollY" runat="server" />   

                <input type="hidden" id="hdnFact" runat="server" />
                <input id="HdnProduct" type="hidden" runat="server" />
                <input type="hidden" id="hdnColor" runat="server" />

            </div>
             
            </ContentTemplate>
        </asp:UpdatePanel> 
    </form>
</body>
</html>
