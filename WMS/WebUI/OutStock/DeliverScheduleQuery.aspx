<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeliverScheduleQuery.aspx.cs" Inherits="WMS.WebUI.OutStock.DeliverScheduleQuery" culture="auto" uiculture="auto" MaintainScrollPositionOnPostback="true" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <title></title> 
    <link href="~/Css/Main.css" type="text/css" rel="stylesheet" /> 
    <link href="~/Css/op.css" type="text/css" rel="stylesheet" /> 
    <script type="text/javascript" src="../../JQuery/jquery-1.8.3.min.js"></script>
    <script type="text/javascript" src= "../../JScript/Common.js"></script>
    <script type="text/javascript">
        function QueryClose() {
            window.returnValue = '';
            window.close();
            return false;
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
                <table style="width: 100%; height: 20px;" class="maintable">
                    <tr>
						    <td class="smalltitle" align="center" width="8%" >
                                <asp:Literal ID="Literal1" Text="查询栏位"
                                    runat="server" ></asp:Literal></td>
						    <td  width="15%" height="20">&nbsp;
                            <asp:dropdownlist id="ddlField" runat="server" Width="90%" >
                                   <asp:ListItem Value="ScheduleNo">销售单号</asp:ListItem>
                                    <asp:ListItem Value="BillDate">销售日期</asp:ListItem>
                                   <asp:ListItem Value="SourceNo">来源单号</asp:ListItem>
                                    <asp:ListItem Value="CustName">经销商</asp:ListItem>
                                    <asp:ListItem Value="ParCustName">子公司</asp:ListItem> 
                                    <asp:ListItem Value="LinkPerson">收货人</asp:ListItem>
                                    <asp:ListItem Value="LinkPhone">收货电话</asp:ListItem>
                                    <asp:ListItem Value="LinkAddress">收货地址</asp:ListItem>
                                    <asp:ListItem Value="CheckUser">审核人员</asp:ListItem>
                                    <asp:ListItem Value="CheckDate">审核日期</asp:ListItem>
                                 </asp:dropdownlist>
                            </td>
						    <td class="smalltitle" align="center" width="6%">
                                <asp:Literal ID="Literal2" Text="查询内容"
                                    runat="server" ></asp:Literal>
                            </td>
						    <td  width="34%" height="20" valign="middle">&nbsp;<asp:textbox id="txtSearch" 
                                    tabIndex="1" runat="server" Width="74%" CssClass="TextBox"  
                                    heigth="16px" ></asp:textbox>
                                &nbsp;<asp:button id="btnSearch" tabIndex="2" runat="server" Width="58px" 
                                    CssClass="ButtonQuery" Text="查询" OnClientClick="return Search()"  
                                    onclick="btnSearch_Click"></asp:button>
                          </td>
                          
                          <td align="right"  style="width:10%">
                              <input type="button"  class="ButtonExit" value="离开" onclick="return QueryClose();" />
                              
                          </td>
                    </tr>
                </table>
              
                </div>

            <div id="Maincontainer" style="overflow:auto; WIDTH: 100%; height:200px;"  runat="server" onscroll="javascript:RecordPostion(this);">
                
            <asp:GridView ID="gvMain" runat="server" AutoGenerateColumns="False" 
                    SkinID="GridViewSkin" Width="98%" 
                    OnRowDataBound="gvMain_RowDataBound">
            <Columns>
                 <asp:TemplateField HeaderText="選取">
			        <ItemTemplate>
                        <asp:Button ID="btnSingle" runat="server" Text="選取" CssClass="but"  />
			        </ItemTemplate>                    
                    <ControlStyle Width="50px" Height="20px" />
		        </asp:TemplateField>
                 <asp:BoundField DataField="ScheduleNo" HeaderText="销售单号" 
                    SortExpression="ScheduleNo"  >
                    <ItemStyle HorizontalAlign="Left" Width="15%" Wrap="False" />
                    <HeaderStyle Wrap="False" />
                </asp:BoundField>

                <asp:TemplateField HeaderText="销售日期" SortExpression="BillDate">
                    <ItemTemplate>
                        <%# ToYMD(DataBinder.Eval(Container.DataItem, "BillDate"))%>
                    </ItemTemplate>
                    <HeaderStyle Wrap="False" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="False" />
                </asp:TemplateField> 
                   
                 <asp:BoundField DataField="SourceNo" HeaderText="来源单号" 
                    SortExpression="SourceNo"  >
                    <ItemStyle HorizontalAlign="Left" Width="15%" Wrap="False" />
                    <HeaderStyle Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="CustName" HeaderText="经销商" 
                    SortExpression="CustName"  >
                    <ItemStyle HorizontalAlign="Left" Width="15%" Wrap="False" />
                    <HeaderStyle Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="ParCustName" HeaderText="子公司" 
                    SortExpression="ParCustName"  >
                    <ItemStyle HorizontalAlign="Left" Width="15%" Wrap="False" />
                    <HeaderStyle Wrap="False" />
                </asp:BoundField>
                 <asp:BoundField DataField="CustPerson" HeaderText="联系人" 
                    SortExpression="CustPerson"  >
                    <ItemStyle HorizontalAlign="Left" Width="15%" Wrap="False" />
                    <HeaderStyle Wrap="False" />
                </asp:BoundField>
                 <asp:BoundField DataField="CustPhone" HeaderText="联系电话" 
                    SortExpression="CustPhone"  >
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
                <asp:BoundField DataField="CheckUser" HeaderText="审核人员" 
                    SortExpression="CheckUser"  >
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

          <div >
                <table class="maintable" cellpadding="0" cellspacing="0" style="width: 100%;">
                    <tr>
                        <td valign="middle" align="left" height="22" class="title1">
                            <p>
                                <asp:Literal ID="ltlTitle" Text="销售订单明细" runat="server"></asp:Literal></p>
                        </td>
                    </tr>
                </table>
             
            <div style="overflow: auto; width: 100%; height: 140px" >
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
                             <asp:BoundField DataField="NotOutStockQty" HeaderText="数量" >
                                <ItemStyle HorizontalAlign="right" Width="8%" Wrap="False" />
                                <HeaderStyle Wrap="False" />
                            </asp:BoundField> 
                            <asp:TemplateField HeaderText="预出库日">
                                <ItemTemplate>
                                    <%# ToYMD(DataBinder.Eval(Container.DataItem, "PlanDate"))%>
                                </ItemTemplate>
                                <HeaderStyle Wrap="False" />
                                <ItemStyle HorizontalAlign="Left" Width="8%" VerticalAlign="Middle" Wrap="False" />
                            </asp:TemplateField>         
                        </Columns>
                        <PagerSettings Visible="false" />
                    </asp:GridView>
            </div>
             </div>
                <div style="font-size: 0px; bottom: 0px; right: 0px;">
                <asp:Button ID="btnReload" runat="server" Text="" OnClick="btnReload_Click" Height="0px" Width="0px" />
                <asp:HiddenField ID="hdnRowIndex" runat="server" Value="0" />
               <asp:HiddenField ID="hdnRowValue" runat="server"  />
               <input type="hidden" id="dvscrollX" runat="server" />
                <input type="hidden" id="dvscrollY" runat="server" />  
            </div>
            </ContentTemplate>
        </asp:UpdatePanel> 
    </form>
</body>
</html>
