﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OutStockEdit.aspx.cs" Inherits="WMS.WebUI.OutStock.OutStockEdit" %>
<%@ Register src="../../UserControl/Calendar.ascx" tagname="Calendar" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
        <link href="~/Css/Main.css" type="text/css" rel="stylesheet" /> 
        <link href="~/Css/op.css" type="text/css" rel="stylesheet" /> 
        <script type="text/javascript" src="../../JQuery/jquery-1.8.3.min.js"></script>
        <script type="text/javascript" src= "../../JScript/Common.js"></script>
        <script type="text/javascript" src="../../JScript/DataProcess.js"></script>
        <script type="text/javascript">
            $(document).ready(function () {
                $(window).resize(function () {
                    resize();
                });
                BindEvent();
            });
            function resize() {
                var h = document.documentElement.clientHeight - 290;
                $("#Sub-container").css("height", h);
            }
            function SelectProduct() {
                var tableName = 'CMD_ProductInStock';
                var where = "AreaCode='" + $('#ddlAreaCode').val() + "' and ProductCode not in ('0001','0002') and ProductTypeCode='" + $('#ddlTrainTypeCode').val() + "'";
               
                return GetMulSelectValue(tableName, 'hdnMulSelect', where);
            }
            function BindEvent() {
                $("[ID$='ProductCode']").bind("change", function () {
                    var txtID = this.id;
                    var where = "AreaCode='" + $('#ddlAreaCode').val() + "' and ProductCode='" + $('#' + txtID).val() + "'  and ProductCode not in ('0001','0002') and ProductTypeCode='" + $('#ddlTrainTypeCode').val() + "'";

                    getWhereBaseData('CMD_Product', txtID + "," + txtID.replace("ProductCode", "ProductName"), 'ProductCode,ProductName', where);
                });
                $("[ID$='ProductCode']").bind("dblclick", function () {
                    var txtID = this.id;
                    $('#' + txtID.replace("ProductCode", "btnProduct"))[0].click();

                });
            }


            function Save() {

                if ($("#txtBillDate_txtDate").val() == "") {
                    alert("日期不能为空，请输入！");
                    $("#txtBillDate_txtDate").focus();
                    return false;
                }
                if (trim($("#txtID").val()) == "") {
                    alert("出库单号不能为空!");
                    $("#txtID").focus();
                    return false;
                }
                if (trim($("#ddlBillTypeCode").val()) == "") {
                    alert("出库类型不能为空!");
                    $("#ddlBillTypeCode").focus();
                    return false;
                }
                if (trim($("#ddlAreaCode").val()) == "") {
                    alert("库区不能为空!");
                    $("#ddlAreaCode").focus();
                    return false;
                }
                if (!ChkDelMustValue("dgViewSub1", "ProductCode", "产品编号"))
                    return false;
                if (!ChkDelMustNumericValue("dgViewSub1", "Quantity", "数量"))
                    return false;
                return true;
            }
           
        </script>
    
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager2" runat="server" />  
        <asp:UpdateProgress ID="updateprogress1" runat="server" AssociatedUpdatePanelID="updatePanel1">
            <ProgressTemplate>            
                <div id="progressBackgroundFilter" style="display:none"></div>
                <div id="processMessage"> Loading...<br /><br />
                        <img alt="Loading" src="../../images/loading.gif" />
                </div>            
                </ProgressTemplate>
        </asp:UpdateProgress>  
        <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional">                
            <ContentTemplate>
                <table style="width: 100%; height: 25px;" class="OperationBar">
                    <tr>
                        <td align="right">
                            <asp:Button ID="btnCancel" runat="server" Text="放弃" 
                                OnClientClick="return Cancel();" CssClass="ButtonCancel" />
                            <asp:Button ID="btnSave" runat="server" Text="保存" OnClientClick="return Save()" 
                                CssClass="ButtonSave" onclick="btnSave_Click" Height="16px" />
                            <asp:Button ID="btnExit" runat="server" Text="离开" OnClientClick="return Exit();" 
                                CssClass="ButtonExit" />
                        </td>
                    </tr>
                </table>
                <table id="Table1" class="maintable"  width="100%" align="center" cellspacing="0" cellpadding="0" bordercolor="#ffffff" border="1"
				         runat="server">			
				    <tr>
                        <td align="center" class="musttitle" style="width:8%;"  >
                                出库日期
                        </td>
                        <td  width="25%">
                                &nbsp;<uc1:Calendar ID="txtBillDate" runat="server"  /></td>
                        <td align="center" class="musttitle" style="width:8%;"  >
                                出库单号
                        </td>
                        <td width="25%">
                                &nbsp;<asp:TextBox ID="txtID" 
                                    runat="server"  CssClass="TextBox" Width="90%" 
                                    MaxLength="20" ></asp:TextBox> 
                        </td>
                            <td align="center" class="musttitle" style="width:8%;">
                                出库类型</td>
                        <td width="26%">
                            &nbsp;<asp:DropDownList ID="ddlBillTypeCode" runat="server" Width="90%">
                            </asp:DropDownList>
                    
                        </td>
                    </tr>
                    <tr>
                        <td align="center" class="musttitle" style="width:8%;"  >
                                库区</td>
                        <td  width="25%">
                                &nbsp;<asp:DropDownList ID="ddlAreaCode" runat="server" Width="90%" 
                                    onselectedindexchanged="ddlAreaCode_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td align="center" class="musttitle" style="width:8%;"  >
                                产品类型
                        </td>
                        <td width="25%">
                                &nbsp;<asp:DropDownList ID="ddlTrainTypeCode" runat="server" Width="90%">
                                </asp:DropDownList>
                        </td>
                        <td align="center" class="smalltitle"  style="width:8%;">
                            上车号
                        </td>
                        <td width="26%">
                        &nbsp;<asp:TextBox ID="txtTrainNo" runat="server" CssClass="TextBox" Width="90%"></asp:TextBox>
                        </td>
                    </tr>
                     <tr>
                        <td align="center"  class="smalltitle" style="width:8%;"  >
                                上车轴位</td>
                        <td  width="25%">
                             &nbsp;<asp:TextBox ID="txtAxieLocation" runat="server" CssClass="TextBox" 
                                 Width="90%"></asp:TextBox>        
                        </td>
                        <td align="center"  class="smalltitle" style="width:8%;"  >
                                修程
                        </td>
                        <td width="25%">
                             &nbsp;<asp:TextBox ID="txtXc" runat="server" CssClass="TextBox" Width="90%"></asp:TextBox>
                        </td>
                        <td align="center"  class="smalltitle"  style="width:8%;">
                            齿侧内轴
                        </td>
                        <td width="26%">
                        &nbsp;<asp:TextBox ID="txtCcnz" runat="server" CssClass="TextBox" Width="90%"></asp:TextBox>
                        </td>
                    </tr>
                     <tr>
                        <td align="center"  class="smalltitle" style="width:8%;"  >
                                齿侧外轴</td>
                        <td  width="25%">
                                &nbsp;<asp:TextBox ID="txtCcwz" runat="server" CssClass="TextBox" Width="90%"></asp:TextBox>
                        </td>
                        <td align="center"  class="smalltitle" style="width:8%;"  >
                                非齿侧内轴
                        </td>
                        <td width="25%">
                                &nbsp;<asp:TextBox ID="txtFccnz" runat="server" CssClass="TextBox" Width="90%"></asp:TextBox>
                        </td>
                        <td align="center" class="smalltitle"   style="width:8%;">
                            非齿侧外轴
                        </td>
                        <td width="26%">
                        &nbsp;<asp:TextBox ID="txtFccwz" runat="server" CssClass="TextBox" Width="90%"></asp:TextBox>
                        </td>
                    </tr>
              
                    <tr style="height:45px">
                        <td align="center" class="smalltitle"  >
                            备注
                        </td>
                        <td colspan="5"  valign="middle" >
                            &nbsp;<asp:TextBox ID="txtMemo" runat="server" CssClass="MultiLineTextBox" 
                                TextMode="MultiLine" Height="30px" Width="97%"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table style="width:100%; height:25px">
                    <tr>
                        <td class="table_titlebgcolor" height="25px">
                            <asp:Button  id="btnAddDetail" CssClass=" ButtonCreate" runat="server" 
                                Text="新增明细" onclick="btnAddDetail_Click" OnClientClick="return SelectProduct();"  Width="75px" Height="16px"  />  
                                &nbsp;&nbsp;
                                <asp:Button  id="btnDelDetail" CssClass=" ButtonDel" 
                                runat="server" Text="删除明细" onclick="btnDelDetail_Click" 
                                Width="75px" Height="16px" />
                               
                    </td>
                    </tr>
                </table> 
                <div id="Sub-container" style="overflow: auto; width: 100%; height: 280px" >
                    <asp:GridView ID="dgViewSub1" runat="server" AutoGenerateColumns="False" SkinID="GridViewSkin"
                        AllowPaging="True" Width="100%" PageSize="10" onrowdatabound="dgViewSub1_RowDataBound" >
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
                                <HeaderStyle Width="4%"  />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="产品编号">
                                    <ItemTemplate>
                                    <asp:TextBox ID="ProductCode" runat="server"  Width="80%" CssClass="TextBox"></asp:TextBox><asp:Button
                                        ID="btnProduct"  CssClass="ButtonOption" Width="20px" runat="server"  Text="..." OnClientClick="return SelectProduct();" OnClick="btnProduct_Click" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                                <HeaderStyle Width="20%"  />
                            </asp:TemplateField>
                                
                                <asp:TemplateField HeaderText="品名">
                                <ItemTemplate>
                                    <asp:TextBox ID="ProductName" runat="server" Width="98%"  CssClass="TextRead" ></asp:TextBox> 
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                                <HeaderStyle  Width="25%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="数量">
                                <ItemTemplate>
                                    <asp:TextBox ID="Quantity" runat="server" Width="100%" CssClass="TextBox" style="text-align:right;" 
                                    onkeypress="return regInput(this,/^\d+$/,String.fromCharCode(event.keyCode))" 	onpaste="return regInput(this,/^\d+$/,window.clipboardData.getData('Text'))" 
                                    ondrop="return regInput(this,/^\d+$/,event.dataTransfer.getData('Text'))" onfocus="TextFocus(this);"></asp:TextBox> 
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                                <HeaderStyle Width="10%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="备注">
                                <ItemTemplate>
                                    <asp:TextBox ID="SubMemo" runat="server" Width="98%"  CssClass="TextBox" ></asp:TextBox> 
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                                <HeaderStyle  Width="38%" />
                            </asp:TemplateField>
                        </Columns>
                        <PagerSettings Visible="false" />
                    </asp:GridView>
                </div>
                <table  class=" maintable" style="width:100%; height:25px" > 
                    <tr>
                        <td align="center"  style="width:8%;" class="smalltitle">
                            数量合计
                        </td>
                        <td style="width:17%">
                            &nbsp;<asp:TextBox ID="txtTotalQty" runat="server" CssClass="TextRead" Height="16px" ReadOnly="True" Width="90%" style="text-align:right"></asp:TextBox>
                        </td>
                          
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
                                &nbsp;
                                <asp:Label ID="lblCurrentPageSub1" runat="server" ></asp:Label>&nbsp;&nbsp; 
                            </td>
                        <td width="1%">
                        </td>
                    </tr>
                </table>
                <table  class="maintable"   style=" width:100%; height:25px" align="center" cellspacing="0" cellpadding="0" border="1">
                    <tr>
                        <td align="center"  class="smalltitle" style="width:8%;">
                            建单人员
                        </td> 
                        <td style="width:17%">
                        &nbsp;<asp:TextBox ID="txtCreator" runat="server"  CssClass="TextRead" Width="90%"  ></asp:TextBox> 
                        </td>
                        <td align="center" class="smalltitle" style="width:8%;">
                            建单日期
                        </td> 
                        <td style="width:17%">
                        &nbsp;<asp:TextBox ID="txtCreatDate" runat="server"  CssClass="TextRead" Width="90%"  ></asp:TextBox> 
                        </td>
                        <td align="center"  class="smalltitle" style="width:8%;">
                            修改人员
                        </td> 
                        <td style="width:17%">
                            &nbsp;<asp:TextBox ID="txtUpdater" runat="server"  CssClass="TextRead" Width="90%"  ></asp:TextBox> 
                        </td>
                        <td align="center"  class="smalltitle" style="width:8%;">
                            修改日期
                        </td> 
                        <td style="width:17%">
                        &nbsp;<asp:TextBox ID="txtUpdateDate" runat="server" CssClass="TextRead" Width="90%"  ></asp:TextBox> 
                        </td>
                </tr>
		    </table>

            <input type="hidden" runat="server" id="hdnMulSelect" /> 
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
