<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TransferSales.aspx.cs" Inherits="WMS.WebUI.OutStock.TransferSales" culture="auto" uiculture="auto" MaintainScrollPositionOnPostback="true" %>
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
    <script type="text/javascript" src='<%=ResolveClientUrl("~/JScript/DataProcess.js") %>'></script>
    <script type="text/javascript" src='<%=ResolveClientUrl("~/ext-3.3.1/ext-base.js") %>'></script> 
    <script type="text/javascript" src='<%=ResolveClientUrl("~/ext-3.3.1/ext-all.js") %>'></script>
    <script type="text/javascript">
        $(document).ready(function () {
            BindEvent();
        });
        function BindEvent() {
            var activeTab = parseInt($('#HdfActiveTab').val());
            var tabPanel = new Ext.TabPanel({
                height: 210,
                width: "100%",

                autoTabs: true, //自动扫描页面中的div然后将其转换为标签页
                deferredRender: false, //不进行延时渲染
                activeTab: activeTab, //默认激活第一个tab页
                animScroll: true, //使用动画滚动效果
                enableTabScroll: true, //tab标签超宽时自动出现滚动按钮
                applyTo: 'tabs'
            });
            $("#btnFact").bind("click", function () {
                getMultiItems("CMD_CUSTOMER", "CUSTOMER_CODE", this, '#hdnFact');
                return false;
            });
            $("#txtFact").bind("dblclick", function () {
                GetOtherValueNullClear('CMD_CUSTOMER', 'txtFactID,txtFact', "CUSTOMER_CODE,CUSTOMER_NAME", "1=1");
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
        function content_resize() {

            //編輯頁面 div高度
            //            var div = $("#surdiv");
            //            var h = 300;
            //            if ($(window).height() <= 0) {
            //                h = document.body.clientHeight - 35;
            //            }
            //            else {
            //                h = $(window).height() - 35;
            //            }
            //            $("#surdiv").css("height", h);

            //            $("#Sub-container").css("height", h - 380); //设置S界面多明细设置  

        }
    </script>
</head>
<body >
    <form id="form1" runat="server">

    <asp:ScriptManager ID="ScriptManager1" runat="server" />  
    <asp:UpdateProgress ID="updateprogress" runat="server" AssociatedUpdatePanelID="UpdatePanel">
    <ProgressTemplate>            
             <div id="progressBackgroundFilter" style="display:none"></div>
        <div id="processMessage"> Loading...<br /><br />
             <img alt="Loading" src="../../images/process/loading.gif" />
        </div>            
 
        </ProgressTemplate>
 
    </asp:UpdateProgress>  
        <asp:UpdatePanel ID="UpdatePanel" runat="server" UpdateMode="Conditional">                
            <ContentTemplate>
            <div>
                    <table style="width: 100%; height: 20px;" class="maintable"  cellspacing="0" cellpadding="0" width="100%" align="center" bordercolor="#ffffff"	border="1">
                    <tr>
                        <td  class="smalltitle" align="right" style="height: 24px; width:75px">
                           产品型号 <asp:textbox id="txtProductID"   runat="server" Width="0px" CssClass="TextBox" heigth="0px" style="display:none" ></asp:textbox>
                        </td>
                        <td align="left" style="height: 24px; width:90px ">
                          &nbsp;<asp:textbox id="txtProductModule" tabIndex="1" runat="server" Width="92%" CssClass="TextBox" heigth="16px"  AutoCompleteType="Disabled"></asp:textbox>
                        </td>
                        <td align="left" colspan="2"  >
                               <asp:Button ID="btnProduct" runat="server" CssClass="but" Text="指定" Width="70px" Height="23px" />
                        </td>
                        <td  class="smalltitle" align="right" style="height: 24px; width:75px">
                           规格<asp:textbox id="txtColorID" tabIndex="1" runat="server" Width="0px" CssClass="TextBox" heigth="0px" style="display:none"></asp:textbox>
                        </td>
                        <td align="left" style="height: 24px;width:75px ">
                           &nbsp;<asp:textbox id="txtColor" tabIndex="1" runat="server" Width="93%" 
                                CssClass="TextBox" heigth="16px" AutoCompleteType="Disabled"></asp:textbox>
                        </td>
                        <td align="left" style="height: 24px;width:70px " >
                            <asp:Button ID="btnColor" runat="server" CssClass="but" Text="指定" Width="70px" Height="23px" />
                        </td>
                         <td  class="smalltitle" align="right" style="height: 24px; width:75px">
                            归属地 
                        </td>
                        <td  colspan="2">
                            <asp:DropDownList ID="ddlAreaSation" runat="server" Width="90%">
                            </asp:DropDownList>
                        </td>
                        
                        
                        <td  class="smalltitle" align="right" style="height: 24px; width:145px" >
                           经销商 <asp:textbox id="txtFactID" tabIndex="1" runat="server" Width="0px" CssClass="TextBox" heigth="0px" style="display:none"></asp:textbox>
                        </td>

                        <td align="left" style="height: 24px; width:170px"    >
                          &nbsp;<asp:textbox id="txtFact" tabIndex="1" runat="server" CssClass="TextBox" AutoCompleteType="Disabled" Width="96%"></asp:textbox>
                        </td>
                        <td align="left" style="height: 24px;width:70px " >
                            <asp:Button ID="btnFact" runat="server" CssClass="but" Text="指定"   Width="70px" Height="22px" />
                        </td>
                       
                    </tr>
                    <tr>
                          <td class="smalltitle" align="right" style="height: 24px; width:75px">
                            发货日期 
                        </td>
                        <td align="left" style="height: 24px; width:90px ">
                           <uc1:Calendar ID="txtStartDate" runat="server" />
                        </td>
                        <td align="center" style="height: 24px;width:10px  " >
                         To 
                        </td>
                        <td  align="left" style="height: 24px; width:90px ">
                          <uc1:Calendar ID="txtEndDate" runat="server" />
                        </td>
                        <td class="smalltitle" align="right" style="height: 24px; width:75px"  >
                           出货仓库
                        </td>
                         <td colspan="2">
                          <asp:DropDownList ID="ddlDriverType" runat="server" Width="90%" heigth="16px">
                                    <asp:ListItem Value="2">请选择</asp:ListItem>
                                    <asp:ListItem Value="0">库存单</asp:ListItem>
                                    <asp:ListItem Value="1">直出单</asp:ListItem>
                            </asp:DropDownList>
                        </td>

                        <td class="smalltitle" align="right" style="height: 24px; width:75px">
                           出库单号
                        </td>
                        
                         
                         <td  align="left" style="height: 24px; width:75px">
                          <asp:textbox id="txtLinkPerson" tabIndex="1" runat="server" Width="93%" 
                                 CssClass="TextBox" heigth="16px" ></asp:textbox>
                        </td>
                        
                        <td  class="smalltitle" align="right" style="height: 24px; width:50px">
                           状态
                        </td>
                        <td  align="right" style="height: 24px; width:145px" >
                           <asp:RadioButton ID="opt1" runat="server" Checked="True" GroupName="LabelSource" Text="全部" /> 
                           <asp:RadioButton ID="opt2" runat="server" GroupName="LabelSource" Text="已审" />
                           <asp:RadioButton ID="opt3" runat="server" GroupName="LabelSource" Text="未审" />
                                    
                        </td>
                        <td  colspan="2">
                           <asp:button id="btnSearch" tabIndex="2" runat="server" Width="58px" CssClass="ButtonQuery" Text="查询" onclick="btnSearch_Click" />&nbsp;
                         <asp:Button ID="btnRefresh" runat="server" CssClass="ButtonRefresh" onclick="btnSearch_Click" OnClientClick="return Refresh()" tabIndex="2" Text="刷新" Width="58px" /> &nbsp;
                          <asp:Button ID="btnAdd" runat="server" Text="新增" CssClass="ButtonCreate"  OnClientClick="Add();"  />&nbsp; 
                          <asp:Button ID="btnDelete" runat="server" Text="刪除" CssClass="ButtonDel" onclick="btnDeletet_Click" OnClientClick="return Delete('gvMain')" Width="51px"/> &nbsp;
                         <asp:Button ID="btnExit" runat="server" Text="离开" CssClass="ButtonExit" OnClientClick="return Exit()" Width="51px" /> 
                        </td>
                        
                    </tr>

                  
                
                 
                </table>
                </div>

            <div id="Maincontainer" style="overflow:auto; WIDTH: 100%; height:265px;"  runat="server" onscroll="javascript:RecordPostion(this);">
                
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
                <asp:TemplateField HeaderText="出库单号"  SortExpression="BillID" >
                    <ItemTemplate>
                    <asp:LinkButton  id="LinkButton1" runat="server" OnClientClick="return ShowViewForm(this);" Text='<%# DataBinder.Eval(Container.DataItem, "BillID") %>' >
                    </asp:LinkButton>                       
                    </ItemTemplate>
                  <ItemStyle Width="10%" Wrap="False" HorizontalAlign="Left"/>
                  <HeaderStyle Width="10%" Wrap="False" />
               </asp:TemplateField>
                <asp:TemplateField HeaderText="出库日期" SortExpression="BillDate">
                    <ItemTemplate>
                        <%# ToYMD(DataBinder.Eval(Container.DataItem, "BillDate"))%>
                    </ItemTemplate>
                    <HeaderStyle Wrap="False" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="False" />
                </asp:TemplateField> 
                <asp:BoundField DataField="WareHouseName" HeaderText="销售仓库" 
                    SortExpression="WareHouseName"  >
                    <ItemStyle HorizontalAlign="Left" Width="15%" Wrap="False" />
                    <HeaderStyle Wrap="False" />
                </asp:BoundField>
                   
               
                <asp:BoundField DataField="CustName" HeaderText="经销商" 
                    SortExpression="CustName"  >
                    <ItemStyle HorizontalAlign="Left" Width="15%" Wrap="False" />
                    <HeaderStyle Wrap="False" />
                </asp:BoundField>
                 
                 
                 <asp:BoundField DataField="TranCustomerName" HeaderText="子公司" 
                    SortExpression="TranCustomerName"  >
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
                <asp:BoundField DataField="Checker" HeaderText="审核人员" 
                    SortExpression="Checker"  >
                    <ItemStyle HorizontalAlign="Left" Width="15%" Wrap="False" />
                    <HeaderStyle Wrap="False" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="审核日期" SortExpression="CheckDate">
                    <ItemTemplate>
                        <%# ToYMD(DataBinder.Eval(Container.DataItem, "CheckDate"))%>
                    </ItemTemplate>
                    <HeaderStyle Wrap="False" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="False" />
                </asp:TemplateField>
                

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

            <div id='tabs'>
                 <div class='x-tab' title='出库明细'> 
            <div style="overflow: auto; width: 100%; height: 150px" >
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
                             <asp:BoundField DataField="Qty" HeaderText="数量" >
                                <ItemStyle HorizontalAlign="right" Width="8%" Wrap="False" />
                                <HeaderStyle Wrap="False" />
                            </asp:BoundField> 
                            <asp:BoundField DataField="Price" HeaderText="单价" >
                                    <ItemStyle HorizontalAlign="Right" Width="8%" Wrap="False" />
                                    <HeaderStyle Wrap="False" Width="8%" ForeColor="Blue" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Amount" HeaderText="金额" >
                                    <ItemStyle HorizontalAlign="Right" Width="8%" Wrap="False" />
                                    <HeaderStyle Wrap="False" Width="8%" ForeColor="Blue" />
                                </asp:BoundField>
                        </Columns>
                        <PagerSettings Visible="false" />
                    </asp:GridView>
            </div>
             <table style="width:100%; height:28px;" class="maintable"> 
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
                 <div class='x-tab' title='出库序号明细'> 
                    <div style="overflow: auto; width: 100%; height: 150px" >
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
                            
                            <asp:BoundField DataField="ColName" HeaderText="规格" 
                                SortExpression="ColName"  >
                                <ItemStyle HorizontalAlign="Left" Width="8%" Wrap="False" />
                                <HeaderStyle Wrap="False" />
                            </asp:BoundField>
                        </Columns>
                        <PagerSettings Visible="false" />
                    </asp:GridView>
                    </div>
                    <table style="width:100%; height:28px;" class="maintable"> 
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


            <div style="font-size: 0px; bottom: 0px; right: 0px;">
                <asp:Button ID="btnReload" runat="server" Text="" OnClick="btnReload_Click" Height="0px" Width="0px" />
                <asp:HiddenField ID="hdnRowIndex" runat="server" Value="0" />
                <asp:HiddenField ID="hdnRowValue" runat="server"  />
                <asp:HiddenField ID="HdfActiveTab" runat="server" />
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
