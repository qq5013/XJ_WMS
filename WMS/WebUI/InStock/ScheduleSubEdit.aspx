<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ScheduleSubEdit.aspx.cs" Inherits="WMS.WebUI.InStock.ScheduleSubEdit" %>

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
                $("[ID$='IsComplete']").bind("change", function () {
                    ChangeComplete(this);
                });

                $("[ID$='IsPlanComplete']").bind("change", function () {
                    ChangePlanComplete(this);
                });


                $("[ID$='PlanDeliverQty']").bind("change", function () {
                    ChangePlanDeliverQty(this);
                });
                $("[ID$='DeliveredQty']").bind("change", function () {
                    ChangeDeliverQty(this);
                });
            }
            function ChangeComplete(obj) {
                var ddl = document.getElementById(obj.id)
                var index = ddl.selectedIndex;
                var Value = ddl.options[index].value;   

                if (Value == "True") {
                    var RNames = obj.id.split('_');
                    var RowName = RNames[0] + "_" + RNames[1] + "_";
                    var PlanQty = $('#' + RowName + 'PlanQty').val();
                    var Price = $('#' + RowName + 'Price').val();
                    $('#' + RowName + 'DeliveredQty').val(PlanQty);
                    $('#' + RowName + 'DeliverAccount').val(PlanQty * Price);
                    $('#' + RowName + 'DeliverNoQty').val(0);
                    $('#' + RowName + 'DeliverNoAccount').val(0);
                    $('#' + RowName + 'CompleteRate').val(100);
                }
            }
            function ChangePlanComplete(obj) {
                var ddl = document.getElementById(obj.id)
                var index = ddl.selectedIndex;
                var Value = ddl.options[index].value;
                if (Value == "True") {
                    var RNames = obj.id.split('_');
                    var RowName = RNames[0] + "_" + RNames[1] + "_";
                    var PlanQty = $('#' + RowName + 'PlanQty').val();
                    var Price = $('#' + RowName + 'Price').val();
                    $('#' + RowName + 'DeliveredQty').val(PlanQty);
                    $('#' + RowName + 'DeliverAccount').val(PlanQty * Price);
                    $('#' + RowName + 'DeliverNoQty').val(0);
                    $('#' + RowName + 'DeliverNoAccount').val(0);
                    $('#' + RowName + 'CompleteRate').val(100);

                    $('#' + RowName + 'PlanDeliverQty').val(PlanQty);
                    $('#' + RowName + 'PlanDeliverAccount').val(PlanQty * Price);
                    $('#' + RowName + 'PlanDeliverNoQty').val(0);
                    $('#' + RowName + 'PlanDeliverNoAccount').val(0);
                    $('#' + RowName + 'PlanCompleteRate').val(100);
                }
            }
            function ChangePlanDeliverQty(obj) {
                var RNames = obj.id.split('_');

                var RowName = RNames[0] + "_" + RNames[1] + "_";
                var Qty = parseInt($('#' + obj.id).val());

                var PlanQty = parseInt($('#' + RowName + 'PlanQty').val());
                var Price = $('#' + RowName + 'Price').val();
                if (Qty <= PlanQty) {
                    if ($('#' + RowName + 'DeliveredQty').val() == "0") {
                        $('#' + RowName + 'DeliveredQty').val(Qty);
                        $('#' + RowName + 'DeliverAccount').val(Qty * Price);
                        $('#' + RowName + 'DeliverNoQty').val(PlanQty - Qty);
                        $('#' + RowName + 'DeliverNoAccount').val((PlanQty - Qty) * Price);
                        $('#' + RowName + 'CompleteRate').val((Qty / PlanQty) * 100);
                    }
                    $('#' + RowName + 'PlanDeliverAccount').val(Qty * Price);
                    $('#' + RowName + 'PlanDeliverNoQty').val(PlanQty - Qty);
                    $('#' + RowName + 'PlanDeliverNoAccount').val((PlanQty - Qty) * Price);
                    $('#' + RowName + 'PlanCompleteRate').val((Qty / PlanQty) * 100);
                }
                else {
                    alert("请输入正确数量。");
                    return false;
                }

            }
            function ChangeDeliverQty(obj) {
                var RNames = obj.id.split('_');

                var RowName = RNames[0] + "_" + RNames[1] + "_";
                var Qty = parseInt($('#' + obj.id).val());

                var PlanQty = parseInt($('#' + RowName + 'PlanQty').val());
                var Price = $('#' + RowName + 'Price').val();
                if (Qty <= PlanQty) {
                    $('#' + RowName + 'DeliverAccount').val(Qty * Price);
                    $('#' + RowName + 'DeliverNoQty').val(PlanQty - Qty);
                    $('#' + RowName + 'DeliverNoAccount').val((PlanQty - Qty) * Price);
                    $('#' + RowName + 'CompleteRate').val((Qty / PlanQty) * 100);
                }
                else {
                    alert("请输入正确数量。");
                    return false;
                }
            }
            function SaveClose() {
                window.parent.returnValue = "1";
                window.close();
            }
            function Close() {
                window.parent.returnValue = "";
                window.close();
            }
            function ChangePrice(obj) {
                var RNames = obj.id.split('_');
                var RowName = RNames[0] + "_" + RNames[1] + "_";
                var PlanQty = $('#' + RowName + 'PlanQty').val();
                var Price = $('#' + RowName + 'Price').val();
                $('#' + RowName + 'Amount').val(PlanQty * Price);

            }
        </script>
	</head>
	<body >
		<form id="form1" runat="server">
        <%--<asp:ScriptManager ID="ScriptManager1" runat="server" />  
            <asp:UpdateProgress ID="updateprogress" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
            <ProgressTemplate>            
                     <div id="progressBackgroundFilter" style="display:none"></div>
                <div id="processMessage"> Loading...<br /><br />
                     <img alt="Loading" src="../../images/process/loading.gif" />
                </div>            
 
                </ProgressTemplate>
 
            </asp:UpdateProgress>  
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">                
                <ContentTemplate>--%>
                    <div id="sub1" style="overflow: auto; width: 100%; height: 550px" >
                        <asp:GridView ID="dgViewSub1" runat="server" AutoGenerateColumns="False" SkinID="GridViewSkin"
                            AllowPaging="True"  Width="3000px" PageSize="20" onrowdatabound="dgViewSub1_RowDataBound" onrowcreated="dgViewSub1_RowCreated">
                            <Columns>
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
                                    <asp:TemplateField HeaderText="数量">
                                        <ItemTemplate>
                                            <asp:TextBox ID="PlanQty" runat="server" Width="100%" CssClass="detailtext" style="text-align:right;" 
                                            onkeypress="return regInput(this,/^\d+$/,String.fromCharCode(event.keyCode))" 	onpaste="return regInput(this,/^\d+$/,window.clipboardData.getData('Text'))" 
                                            ondrop="return regInput(this,/^\d+$/,event.dataTransfer.getData('Text'))"  onfocus="TextFocus(this);"></asp:TextBox> 
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle Width="6%" ForeColor="Blue" />
                                    </asp:TemplateField>

                                    
                                    <asp:BoundField DataField="RealQty" HeaderText="实入数量" >
                                        <ItemStyle HorizontalAlign="right" Width="8%" Wrap="False" />
                                        <HeaderStyle Wrap="False" />
                                    </asp:BoundField>
                                      <asp:TemplateField HeaderText="单价">
                                        <ItemTemplate>
                                            <asp:TextBox ID="Price" runat="server" Width="100%" CssClass="detailtext" style="text-align:right;" onfocus="TextFocus(this);" ></asp:TextBox> 
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle Width="6%" ForeColor="Blue" />
                                    </asp:TemplateField> 
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
                                    <asp:TemplateField HeaderText="来源单号">
                                        <ItemTemplate>
                                           <asp:TextBox ID="SourceNo" runat="server" Width="98%" CssClass="detailtext"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle Width="8%" ForeColor="Blue" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="工厂订单号">
                                        <ItemTemplate>
                                           <asp:TextBox ID="OrderNo" runat="server" Width="98%" CssClass="detailtext"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle Width="8%" ForeColor="Blue" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="下单日期">
                                        <ItemTemplate>
                                           <uc1:Calendar ID="OrderDate" runat="server" CssClass="detailtext"/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle Width="100px" ForeColor="Blue" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="计划交期">
                                        <ItemTemplate>
                                           <uc1:Calendar ID="PlanDeliverDate" runat="server" CssClass="detailtext"/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle Width="100px" ForeColor="Blue" />
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField HeaderText="答复交期">
                                        <ItemTemplate>
                                           <uc1:Calendar ID="ReplyDate" runat="server" CssClass="detailtext"/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle Width="100px" ForeColor="Blue" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="调整交期">
                                        <ItemTemplate>
                                           <uc1:Calendar ID="ChangeDate" runat="server" CssClass="detailtext"/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle Width="100px" ForeColor="Blue" />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="考核交期">
                                        <ItemTemplate>
                                           <uc1:Calendar ID="CheckDate" runat="server" CssClass="detailtext"/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle Width="100px" ForeColor="Blue" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="实际交期">
                                        <ItemTemplate>
                                           <uc1:Calendar ID="FactDate" runat="server" CssClass="detailtext"/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle Width="100px" ForeColor="Blue" />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="报表月份">
                                        <ItemTemplate>
                                           <uc1:Calendar ID="ReportDate" runat="server" CssClass="detailtext"/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle Width="100px" ForeColor="Blue" />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="品质确认日期">
                                        <ItemTemplate>
                                           <uc1:Calendar ID="QualityDate" runat="server" CssClass="detailtext"/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle Width="100px" ForeColor="Blue" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="验货结果">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="Result" runat="server" Width="90%">
                                                 <asp:ListItem  Selected="True" Value=""></asp:ListItem>
                                                <asp:ListItem   Value="OK">OK</asp:ListItem>
                                                <asp:ListItem Value="NG/OK" >NG/OK</asp:ListItem>
                                                <asp:ListItem Value="NG/特采">NG/特采</asp:ListItem>
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle  ForeColor="Blue" Wrap="false" />
                                    </asp:TemplateField>
                                
                                   <asp:TemplateField HeaderText="订单完成状态">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="IsComplete" runat="server" Width="90%">
                                                <asp:ListItem  Selected="True" Value="True">已完成</asp:ListItem>
                                                <asp:ListItem Value="False" >未完成</asp:ListItem>
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle Wrap="false" ForeColor="Blue" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="交期是否达成">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="IsPlanComplete" runat="server" Width="90%">
                                                <asp:ListItem  Selected="True" Value="True">是</asp:ListItem>
                                                <asp:ListItem Value="False" >否</asp:ListItem>
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle Wrap="false" ForeColor="Blue" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="备注">
                                        <ItemTemplate>
                                           <asp:TextBox ID="Memo" runat="server" Width="120px" CssClass="detailtext"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Width="120px" />
                                        <HeaderStyle Width="120px" ForeColor="Blue" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="交期内完成数量">
                                        <ItemTemplate>
                                            <asp:TextBox ID="PlanDeliverQty" runat="server" Width="100%" CssClass="detailtext" style="text-align:right;" 
                                            onkeypress="return regInput(this,/^\d+$/,String.fromCharCode(event.keyCode))" 	onpaste="return regInput(this,/^\d+$/,window.clipboardData.getData('Text'))" 
                                            ondrop="return regInput(this,/^\d+$/,event.dataTransfer.getData('Text'))"  onfocus="TextFocus(this);"></asp:TextBox> 
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle Width="6%" ForeColor="Blue" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="交期内交货额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="PlanDeliverAccount" runat="server" Width="100%" CssClass="detailtext" style="text-align:right;" 
                                            onkeypress="return regInput(this,/^\d+$/,String.fromCharCode(event.keyCode))" 	onpaste="return regInput(this,/^\d+$/,window.clipboardData.getData('Text'))" 
                                            ondrop="return regInput(this,/^\d+$/,event.dataTransfer.getData('Text'))"  onfocus="TextFocus(this);"></asp:TextBox> 
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle Width="6%" ForeColor="Blue" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="交期内未交货数量">
                                        <ItemTemplate>
                                            <asp:TextBox ID="PlanDeliverNoQty" runat="server" Width="100%" CssClass="detailtext" style="text-align:right;" 
                                            onkeypress="return regInput(this,/^\d+$/,String.fromCharCode(event.keyCode))" 	onpaste="return regInput(this,/^\d+$/,window.clipboardData.getData('Text'))" 
                                            ondrop="return regInput(this,/^\d+$/,event.dataTransfer.getData('Text'))"  onfocus="TextFocus(this);"></asp:TextBox> 
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle Width="6%" ForeColor="Blue" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="交期内未交货额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="PlanDeliverNoAccount" runat="server" Width="100%" CssClass="detailtext" style="text-align:right;" 
                                            onkeypress="return regInput(this,/^\d+$/,String.fromCharCode(event.keyCode))" 	onpaste="return regInput(this,/^\d+$/,window.clipboardData.getData('Text'))" 
                                            ondrop="return regInput(this,/^\d+$/,event.dataTransfer.getData('Text'))"  onfocus="TextFocus(this);"></asp:TextBox> 
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle Width="6%" ForeColor="Blue" />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="交期达成率%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="PlanCompleteRate" runat="server" Width="100%" CssClass="detailtext" style="text-align:right;" 
                                            onkeypress="return regInput(this,/^\d+$/,String.fromCharCode(event.keyCode))" 	onpaste="return regInput(this,/^\d+$/,window.clipboardData.getData('Text'))" 
                                            ondrop="return regInput(this,/^\d+$/,event.dataTransfer.getData('Text'))"  onfocus="TextFocus(this);"></asp:TextBox> 
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle Width="6%" ForeColor="Blue" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="已交货数量">
                                        <ItemTemplate>
                                            <asp:TextBox ID="DeliveredQty" runat="server" Width="100%" CssClass="detailtext" style="text-align:right;" 
                                            onkeypress="return regInput(this,/^\d+$/,String.fromCharCode(event.keyCode))" 	onpaste="return regInput(this,/^\d+$/,window.clipboardData.getData('Text'))" 
                                            ondrop="return regInput(this,/^\d+$/,event.dataTransfer.getData('Text'))"  onfocus="TextFocus(this);"></asp:TextBox> 
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle Width="6%" ForeColor="Blue" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="已交货额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="DeliverAccount" runat="server" Width="100%" CssClass="detailtext" style="text-align:right;" 
                                            onkeypress="return regInput(this,/^\d+$/,String.fromCharCode(event.keyCode))" 	onpaste="return regInput(this,/^\d+$/,window.clipboardData.getData('Text'))" 
                                            ondrop="return regInput(this,/^\d+$/,event.dataTransfer.getData('Text'))"  onfocus="TextFocus(this);"></asp:TextBox> 
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle Width="6%" ForeColor="Blue" />
                                    </asp:TemplateField>

                                      <asp:TemplateField HeaderText="未交货数量">
                                        <ItemTemplate>
                                            <asp:TextBox ID="DeliverNoQty" runat="server" Width="100%" CssClass="detailtext" style="text-align:right;" 
                                            onkeypress="return regInput(this,/^\d+$/,String.fromCharCode(event.keyCode))" 	onpaste="return regInput(this,/^\d+$/,window.clipboardData.getData('Text'))" 
                                            ondrop="return regInput(this,/^\d+$/,event.dataTransfer.getData('Text'))"  onfocus="TextFocus(this);"></asp:TextBox> 
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle Width="6%" ForeColor="Blue" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="未交货金额">
                                        <ItemTemplate>
                                            <asp:TextBox ID="DeliverNoAccount" runat="server" Width="100%" CssClass="detailtext" style="text-align:right;" 
                                            onkeypress="return regInput(this,/^\d+$/,String.fromCharCode(event.keyCode))" 	onpaste="return regInput(this,/^\d+$/,window.clipboardData.getData('Text'))" 
                                            ondrop="return regInput(this,/^\d+$/,event.dataTransfer.getData('Text'))"  onfocus="TextFocus(this);"></asp:TextBox> 
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle Width="6%" ForeColor="Blue" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="订单完成率%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="CompleteRate" runat="server" Width="100%" CssClass="detailtext" style="text-align:right;" 
                                            onkeypress="return regInput(this,/^\d+$/,String.fromCharCode(event.keyCode))" 	onpaste="return regInput(this,/^\d+$/,window.clipboardData.getData('Text'))" 
                                            ondrop="return regInput(this,/^\d+$/,event.dataTransfer.getData('Text'))"  onfocus="TextFocus(this);"></asp:TextBox> 
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle Width="6%" ForeColor="Blue" />
                                    </asp:TemplateField> 
                                    <asp:TemplateField HeaderText="未完成原因">
                                        <ItemTemplate>
                                           <asp:TextBox ID="NoCompleteMemo" runat="server" Width="98%" CssClass="detailtext"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle Width="8%" ForeColor="Blue" />
                                    </asp:TemplateField>

                            </Columns>
                        
                            <PagerSettings Visible="false" />
                    </asp:GridView>
                    </div>
                    <table width="100%" class="maintable"  bordercolor="#ffffff" border="1" > 
                        <tr>
                            <td align="left">
                               
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
                                     <asp:Label ID="lblCurrentPageSub1" runat="server" ></asp:Label>&nbsp;&nbsp; 
                            </td>
                            <td align="right"">
                                <asp:Button ID="btnCancel" runat="server" Text="放弃" OnClientClick="Close();" CssClass="ButtonCancel" />&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="ButtonSave" onclick="btnSave_Click" />
                            </td>
                            <td width="3%">
                            </td>
                        </tr>
                      </table>
                     
                 <%-- </ContentTemplate>
                   
              </asp:UpdatePanel>   --%>    
             
             
		</form>
	</body>
</html>
