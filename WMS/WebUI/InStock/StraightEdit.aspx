<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StraightEdit.aspx.cs" Inherits="WMS.WebUI.InStock.StraightEdit" EnableEventValidation = "false" %>

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

//                //編輯頁面 div高度
//                var div = $("#surdiv");
//                var h = 300;
//                if ($(window).height() <= 0) {
//                    h = document.body.clientHeight -35;
//                }
//                else {
//                    h = $(window).height() -35;
//                }
//                $("#surdiv").css("height", h);

//                $("#Sub-container").css("height", h - 195); //设置S界面多明细设置  

            }

            function BindEvent() {

                $("#btnQuery").bind("click", function () {
                    var where ="";
                    if ($('#txtScheduleNo').val() != "")
                        where = where +" BillID='" + $('#txtScheduleNo').val() + "'";

                    var returnvalue = window.showModalDialog('../../WebUI/OutStock/StraightOutQuery.aspx?Where=' + escape(where), window, 'DialogHeight:660px;DialogWidth:800px;help:no;scroll:auto;Resizable:yes');
                    if (returnvalue != '' && returnvalue != undefined) {
                        $('#txtScheduleNo').val(returnvalue);

                        return true;
                    }
                    return false;
                });
                
                $("[ID$='InStockQty']").bind("change", function () {
                    ChangePrice(this);
                });
                $("[ID$='InStockQty']").bind("focus", function () {
                    $(this).attr('oldvalue', this.value);
                });
            }
            function ChangePrice(obj) {
                var total = $('#txtTotalQty').val();
                total = parseInt(total) - parseInt($(obj).attr("oldvalue")) + parseInt(obj.value);
                $('#txtTotalQty').val(total);
            }

            function Save() {

                if ($("#txtBillDate_txtDate").val() == "") {
                    alert("日期不能为空，请输入！");
                    $("#txtBillDate_txtDate").focus();
                    return false;
                }
                if ($("#txtID").val() == "") {
                    alert("单号不能为空，请输入！");
                    $("#txtID").focus();
                    return false;
                }
                if ($("#txtScheduleNo").val() == "") {
                    alert("直出单号不能为空，请输入！");
                    $("#txtScheduleNo").focus();
                    return false;
                }
                 var blnvalue = true;
                 ctls = $("[id$='PlanQty']");
                 ctls.each(function () {
                     var rowName = this.id.replace('PlanQty', '');
                     var Qty = $(this).val();
                     var endNo = $('#' + rowName + 'EndNo').val();
                     var StarNo = $('#' + rowName + 'StarNo').val();
                     if (StarNo == '' && endNo == '') {

                     }
                     else {
                         if (StarNo == '') {
                             alert("起始序号不能为空，请输入！");
                             $('#' + rowName + 'EndNo').focus();
                             blnvalue = false;
                             return false;
                         }
                         if (StarNo.length != 11) {
                             alert("起始序号位数必须为11码，请输入！");
                             $('#' + rowName + 'EndNo').focus();
                             blnvalue = false;
                             return false;
                         }

                         if (endNo == '') {
                             alert("结束序号不能为空，请输入！");
                             $('#' + rowName + 'EndNo').focus();
                             blnvalue = false;
                             return false;
                         }
                         if (endNo.length != 11) {
                             alert("结束序号位数必须为11码，请输入！");
                             $('#' + rowName + 'EndNo').focus();
                             blnvalue = false;
                             return false;
                         }
                         var q = parseFloat(endNo) - parseFloat(StarNo) + 1;
                         if (Qty != q) {
                             alert("起始序号与结束序号区间数量与计划数量不相等，请重新输入！");
                             $(this).focus();
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
                                <td align="center" class="musttitle" style=" width: 8%">
                                        日期
                                </td>
                                <td  style="width:15%">
                                        &nbsp;<uc1:Calendar ID="txtBillDate" runat="server" />
                                </td>
                                <td  align="center" class="musttitle" style="width:8%;" >
                                        单号
                                </td>
                                <td style="width:15%" >
                                    &nbsp;<asp:TextBox ID="txtID" runat="server" BorderWidth="0" CssClass="TextBox" 
                                        MaxLength="20" Width="94%"></asp:TextBox>
                                          
                                </td>
                                  
                                <td align="center" class="musttitle" style="width:8%" >
                                    发货通知单</td>
                                  <td  style="width:18%">
                                        &nbsp;<asp:TextBox ID="txtScheduleNo" runat="server" BorderWidth="0" CssClass="Textbox" 
                                        Width="81%" Height="16px"></asp:TextBox>
                                        <asp:Button ID="btnQuery" runat="server" CssClass="ButtonCss" 
                                            onclick="btnQuery_Click" Text="..." Width="20px" />
                                </td>
                                 <td align="center" class="musttitle" style="width:8%" >
                                     销售单号
                                </td>
                                  <td  style="width:18%">
                                          &nbsp;<asp:TextBox ID="txtSourceNo" runat="server" BorderWidth="0" 
                                             CssClass="TextRead" Height="16px" ReadOnly="True" Width="91%"></asp:TextBox>
                                </td>
                               
                            </tr>
                             <tr>
                                <td align="center" style="width:8%;" class="musttitle">
                                        经销商&nbsp; 
                                     
                                 </td>
                                <td colspan="3" >
                                       &nbsp;<asp:TextBox ID="txtCustomerName" runat="server" BorderWidth="0" 
                                            CssClass="TextRead" Height="16px" Width="97%" ReadOnly="True"></asp:TextBox>
                                </td>
                               
                                
                                <td style="width:8%;" align="center" class="smalltitle" >
                                        收货人
                                 </td>
                                    <td  >
                                         &nbsp;<asp:TextBox ID="txtLinkPerson" runat="server" BorderWidth="0" 
                                             CssClass="TextRead" Height="16px" Width="91%" ReadOnly="True"></asp:TextBox>
                                      
                                       </td>
                                <td align="center" class="smalltitle" style="width:8%">
                                    收货电话 
                                </td>
                                <td>
                                    &nbsp;<asp:TextBox ID="txtLinkPhone" runat="server" BorderWidth="0" 
                                        CssClass="TextRead" Height="16px" ReadOnly="True" Width="91%"></asp:TextBox>
                                </td>
                                
                                
                            </tr>
                           <tr>
                                <td align="center" style="width:8%;" class="smalltitle">
                                        收货地址
                                </td> 
                                <td  colspan="3">
                                    &nbsp;<asp:TextBox ID="txtLinkAddress" runat="server" BorderWidth="0" 
                                        CssClass="TextRead" Height="16px" Width="97%" ReadOnly="True"></asp:TextBox>
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
                         &nbsp;&nbsp;<asp:Button  id="btnDelDetail" CssClass="ButtonCss" runat="server" Text="删除明细" 
                                    style="width:60px;" onclick="btnDelDetail_Click" />
                             
                                </td>
                                
                        <td width="3%">
                        </td>
                                
                        </tr>
                    </table> 
                    <div  id="Sub-container" style="overflow: auto; width: 100%; height: 320px" >
                        <asp:GridView ID="dgViewSub1" runat="server" AutoGenerateColumns="False"  SkinID="GridViewSkin"
                            AllowPaging="True" Width="98%" PageSize="10" onrowdatabound="dgViewSub1_RowDataBound" >
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
                                    <HeaderStyle Width="6%" ForeColor="Blue" />
                                </asp:TemplateField>
                            <asp:BoundField DataField="ProductName" HeaderText="产品名称" >
                                <ItemStyle HorizontalAlign="Left" Width="20%" Wrap="False" />
                               <HeaderStyle Wrap="False" Width="8%" ForeColor="Blue" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ProductModel" HeaderText="产品型号" >
                                <ItemStyle HorizontalAlign="Left" Width="9%" Wrap="False" />
                                <HeaderStyle Wrap="False" Width="9%" ForeColor="Blue" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ColorName" HeaderText="规格" >
                                <ItemStyle HorizontalAlign="Left" Width="10%" Wrap="False" />
                                <HeaderStyle Wrap="False" Width="10%" ForeColor="Blue" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="数量">
                                <ItemTemplate>
                                   <asp:TextBox ID="PlanQty" runat="server" Width="100%" CssClass="detailtext" style="text-align:right;" 
                                    onkeypress="return regInput(this,/^\d+$/,String.fromCharCode(event.keyCode))" 	onpaste="return regInput(this,/^\d+$/,window.clipboardData.getData('Text'))" 
                                    ondrop="return regInput(this,/^\d+$/,event.dataTransfer.getData('Text'))"></asp:TextBox> 
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                                <HeaderStyle Wrap="False" Width="8%" ForeColor="Blue" />
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
             
                 <table class="maintable" cellspacing="0" cellpadding="0" width="100%" align="center" style="border-color:#ffffff;"  border="1" > 
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
                    <input type="hidden" runat="server" id="hdnMulSelect" /> 
                  </ContentTemplate>
              </asp:UpdatePanel>       
             
             
		</form>
	</body>
</html>
