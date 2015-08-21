<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Schedules.aspx.cs" Inherits="WMS.WebUI.InStock.Schedules"  MaintainScrollPositionOnPostback="true" %>
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
        function BindEvent() {
           
            $("#btnFact").bind("click", function () {
                getMultiItems("CMD_Factory", "FactoryID", this, '#hdnFact');
                return false;
            });
            $("#txtFact").bind("dblclick", function () {
                GetOtherValueNullClear('CMD_Factory', 'txtFactID,txtFact', "FactoryID,FactoryName", "1=1");
                return false;
            });
            
            $("#btnProduct").bind("click", function () {
               getMultiItems("CMD_Product","PRODUCT_CODE", this, '#HdnProduct');
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
                        <td  class="smalltitle" align="right" style="height: 24px; width:90px">
                           产品型号 <asp:textbox id="txtProductID"   runat="server" Width="0px" CssClass="TextBox" heigth="0px" style="display:none" ></asp:textbox>
                        </td>
                        <td align="left" style="height: 24px;width:120px ">
                          &nbsp;<asp:textbox id="txtProductModule" tabIndex="1" runat="server" Width="92%" CssClass="TextBox" heigth="16px"  AutoCompleteType="Disabled"></asp:textbox>
                        </td>
                        <td align="left" style="height: 24px;width:80px "  >
                               <asp:Button ID="btnProduct" runat="server" CssClass="but" Text="指定" Width="70px" Height="23px" />
                        </td>
                        <td class="smalltitle" align="right"  style="height: 24px; width:90px">
                           规格<asp:textbox id="txtColorID" tabIndex="1" runat="server" Width="0px" CssClass="TextBox" heigth="0px" style="display:none"></asp:textbox>
                        </td>
                        <td align="left" style="height: 24px;width:120px ">
                           &nbsp;<asp:textbox id="txtColor" tabIndex="1" runat="server" Width="92%" CssClass="TextBox" heigth="16px" AutoCompleteType="Disabled"></asp:textbox>
                        </td>
                        <td align="left" style="height: 24px;width:80px "  >
                            <asp:Button ID="btnColor" runat="server" CssClass="but" Text="指定" Width="70px" Height="23px" />
                        </td>
                        <td class="smalltitle" align="right" style="height: 24px; width:90px">
                           供应商 <asp:textbox id="txtFactID" tabIndex="1" runat="server" Width="0px" CssClass="TextBox" heigth="0px" style="display:none"></asp:textbox>
                        </td>
                        <td align="left" colspan="3" >
                          &nbsp;<asp:textbox id="txtFact" tabIndex="1" runat="server" Width="93%" CssClass="TextBox" heigth="16px" AutoCompleteType="Disabled"></asp:textbox>
                        </td>
                        <td align="left" style="height: 24px;width:80px "  >
                            <asp:Button ID="btnFact" runat="server" CssClass="but" Text="指定"   Width="70px" Height="22px" />
                        </td>
                        <td class="smalltitle" align="right" style="height: 24px; width:90px">
                            采购日期 
                        </td>
                        <td align="left" style="height: 24px; width:100px ">
                            &nbsp;<uc1:Calendar ID="txtStartDate" runat="server" />
                        </td>
                        <td align="center" style="height: 24px;width:20px  " >
                         To 
                        </td>
                        <td  style="height: 24px;width:100px  ">
                        &nbsp;<uc1:Calendar ID="txtEndDate" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="smalltitle" align="right" style="height: 24px; width:90px">
                           来源单号
                        </td>
                        <td align="left" colspan="2" >
                           &nbsp;<asp:textbox id="txtSourceNo" tabIndex="1" runat="server" Width="92%" CssClass="TextBox" heigth="16px" ></asp:textbox>
                        </td>
                        <td class="smalltitle" align="right" style="height: 24px; width:90px">
                           工厂订单号
                        </td>
                        <td align="left" colspan="2"  >
                           &nbsp;<asp:textbox id="txtOrderNo" tabIndex="1" runat="server" Width="92%" CssClass="TextBox"  heigth="16px" ></asp:textbox>
                        </td>
                        <td class="smalltitle" align="right" style="height: 24px; width:90px">
                            序列号 
                        </td>
                        <td align="left" style="height: 24px; width:150px ">
                            &nbsp;<asp:textbox id="txtStartNo" tabIndex="1" runat="server" Width="92%" CssClass="TextBox" Height="16px" ></asp:textbox>
                        
                        </td>
                        <td align="center" style="height: 24px;width:20px" >
                         To 
                        </td>
                        <td colspan="2">
                            &nbsp;<asp:textbox id="txtEndNo" tabIndex="1" runat="server" Width="90%"  CssClass="TextBox" heigth="16px" ></asp:textbox>                       
                        </td>
                        <td class="smalltitle" align="right" style="height: 24px;width:90px">
                           预入库日
                        </td>
                        <td align="left" style="height: 24px; width:100px">
                            &nbsp;<uc1:Calendar ID="txtPlanStartDate" runat="server"    />
                        </td>
                        <td align="center" style="height: 24px; width:20px" >
                         To 
                        </td>
                        <td  style="height: 24px; width:100px ">
                        &nbsp;<uc1:Calendar ID="txtPlanEndDate" runat="server"    />
                        </td>

                    </tr>
                    <tr>
                        <td class="smalltitle" align="right" style="height: 24px; width:90px"> 完成状态</td>
                        <td  >
                            <asp:DropDownList ID="ddlIsComplete" runat="server" Width="90%">
                                <asp:ListItem  Selected="True" Value="2">请选择</asp:ListItem>
                                <asp:ListItem  Value="1">已完成</asp:ListItem>
                                <asp:ListItem Value="0" >未完成</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="smalltitle" align="right" style="height: 24px; width:90px"> 表单状态</td>
                        <td colspan="3">
                            <asp:DropDownList ID="ddlState" runat="server" Width="90%">
                                <asp:ListItem  Selected="True" value="0">全部</asp:ListItem>
                                <asp:ListItem  Value="1">一审未审</asp:ListItem>
                                <asp:ListItem Value="2" >一审已审</asp:ListItem>
                                <asp:ListItem  Value="3">二审未审</asp:ListItem>
                                <asp:ListItem Value="4">二审已审</asp:ListItem>
                                <asp:ListItem Value="5" >已关闭</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                         <td class="smalltitle" align="right" style="height: 24px; width:90px"> 采购单号</td>
                        </td>
                        <td  colspan="2">
                            &nbsp;<asp:textbox id="txtScheduleNo" tabIndex="1" runat="server" Width="92%" CssClass="TextBox" heigth="16px" ></asp:textbox>
                        </td>
                        <td align="center" style="height: 24px;width:50px ">
                        </td>
                        <td align="center" style="height: 24px;width:80px ">
                            <asp:button id="btnSearch" tabIndex="2" runat="server" Width="58px" CssClass="ButtonQuery" Text="查询" onclick="btnSearch_Click" /> 
                        </td>
                        <td align="center"   >
                            <asp:Button ID="btnRefresh" runat="server" CssClass="ButtonRefresh" onclick="btnSearch_Click" OnClientClick="return Refresh()" tabIndex="2" Text="刷新" Width="58px" /> 
                         </td>
                         <td align="center"> 
                            <asp:Button ID="btnAdd" runat="server" Text="新增" CssClass="ButtonCreate" OnClientClick="Add();"  /> 
                         </td>
                         <td align= "right" colspan="2" >
                          <asp:Button ID="btnDelete" runat="server" Text="刪除" CssClass="ButtonDel" onclick="btnDeletet_Click" OnClientClick="return Delete('gvMain')" Width="51px"/> &nbsp;
                          <asp:Button ID="btnExit" runat="server" Text="离开" CssClass="ButtonExit" OnClientClick="return Exit()" Width="51px" />
                        </td>
                    </tr>
                
                </table>
                </div>
           <div id="Maincontainer" style="overflow:auto; WIDTH: 100%; height:200px;"  runat="server" onscroll="javascript:RecordPostion(this);">
                
            <asp:GridView ID="gvMain" runat="server" AutoGenerateColumns="False" 
                    SkinID="GridViewSkin" Width="98%" 
                    OnRowDataBound="gvMain_RowDataBound" onrowcreated="dgViewSub1_RowCreated" >
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
                <asp:TemplateField HeaderText="采购单号"  SortExpression="ScheduleNo" >
                    <ItemTemplate>
                     <asp:LinkButton  id="LinkButton1" runat="server" OnClientClick="return ShowViewForm(this);" Text='<%# DataBinder.Eval(Container.DataItem, "ScheduleNo") %>' >
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </asp:LinkButton>                       
                    </ItemTemplate>
                  <ItemStyle Width="10%" Wrap="False" HorizontalAlign="Left"/>
                  <HeaderStyle Width="10%" Wrap="False" />
               </asp:TemplateField>
                <asp:TemplateField HeaderText="采购日期" SortExpression="BillDate">
                    <ItemTemplate>
                        <%# ToYMD(DataBinder.Eval(Container.DataItem, "BillDate"))%>
                    </ItemTemplate>
                    <HeaderStyle Wrap="False" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="False" />
                </asp:TemplateField>
                  
                <asp:BoundField DataField="FactName" HeaderText="供应商" 
                    SortExpression="FactName"  >
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
              <%--  <asp:BoundField DataField="Updater" HeaderText="异动人员" 
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
                <asp:BoundField DataField="CheckUser" HeaderText="一审人员" 
                    SortExpression="CheckUser"  >
                    <ItemStyle HorizontalAlign="Left" Width="15%" Wrap="False" />
                    <HeaderStyle Wrap="False" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="一审日期" SortExpression="CheckDate">
                    <ItemTemplate>
                        <%# ToYMD(DataBinder.Eval(Container.DataItem, "CheckDate"))%>
                    </ItemTemplate>
                    <HeaderStyle Wrap="False" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="False" />
                </asp:TemplateField>
                <asp:BoundField DataField="ReCheckUser" HeaderText="二审人员" 
                    SortExpression="ReCheckUser"  >
                    <ItemStyle HorizontalAlign="Left" Width="15%" Wrap="False" />
                    <HeaderStyle Wrap="False" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="二审日期" SortExpression="ReCheckDate">
                    <ItemTemplate>
                        <%# ToYMD(DataBinder.Eval(Container.DataItem, "ReCheckDate"))%>
                    </ItemTemplate>
                    <HeaderStyle Wrap="False" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="False" />
                </asp:TemplateField>
              <%--  <asp:BoundField DataField="Closer" HeaderText="关闭人员" 
                    SortExpression="Closer"  >
                    <ItemStyle HorizontalAlign="Left" Width="15%" Wrap="False" />
                    <HeaderStyle Wrap="False" />
                </asp:BoundField>--%>
                <asp:TemplateField HeaderText="关闭日期" SortExpression="CloseDate">
                    <ItemTemplate>
                        <%# ToYMD(DataBinder.Eval(Container.DataItem, "CloseDate"))%>
                    </ItemTemplate>
                    <HeaderStyle Wrap="False" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="False" />
                </asp:TemplateField>
            </Columns>
            <PagerSettings Visible="False" />
        </asp:GridView>
            </div>
            <div>
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
            <div>
                <table class="maintable" cellpadding="0" cellspacing="0" style="width: 100%; height:28px">
                    <tr>
                        <td valign="middle" align="left" height="25px">
                          <b>采购订单明细 </b> 
                        </td>
                    </tr>
                </table>
                <div id="Sub-container" style="overflow: auto; width: 100%; height: 180px" >
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
                                <asp:Literal ID="Literal3" Text="每页" runat="server"  Visible="false" />
                                    <asp:DropDownList ID="ddlPageSizeSub1" runat="server" AutoPostBack="True"  
                                    Height="16px" onselectedindexchanged="ddlPageSizeSub1_SelectedIndexChanged" Visible="false" >
                    
                                    </asp:DropDownList>
                            </td>
                            <td width="3%">
                            </td>
                        </tr>
                   </table>
             </div>
            
             <div style="font-size: 0px; bottom: 0px; right: 0px;">
                <asp:Button ID="btnReload" runat="server" Text="" OnClick="btnReload_Click" Height="0px" Width="0px" style=" display:none" />
                <asp:HiddenField ID="hdnRowIndex" runat="server" Value="0" />
                <asp:HiddenField ID="hdnRowValue" runat="server"  />
                 <input type="hidden" id="hdnFact" runat="server" />
                 <input type="hidden" id="HdnProduct" runat="server" />
                  <input type="hidden" id="hdnColor" runat="server" />
                <input type="hidden" id="dvscrollX" runat="server" />
                <input type="hidden" id="dvscrollY" runat="server" />   
             </div>
            </ContentTemplate>
        </asp:UpdatePanel> 
    </form>
</body>
</html>
