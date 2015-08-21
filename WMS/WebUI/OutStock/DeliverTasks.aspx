<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeliverTasks.aspx.cs" Inherits="WMS.WebUI.OutStock.DeliverTasks" culture="auto" uiculture="auto" MaintainScrollPositionOnPostback="true" %>


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

//            $("#Sub-container").css("height", h - 385); //设置S界面多明细设置  

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
						    <td class="smalltitle" align="center" width="6%" >
                                <asp:Literal ID="Literal1" Text="查询栏位"
                                    runat="server" ></asp:Literal></td>
						    <td  width="8%" height="20">&nbsp;
                            <asp:dropdownlist id="ddlField" runat="server" Width="90%" >
                                    <asp:ListItem Value="BillID">出库单号</asp:ListItem>
                                    <asp:ListItem Value="BillDate">出库日期</asp:ListItem>
                                    <asp:ListItem Value="销售单号">销售单号</asp:ListItem>
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
						    <td  width="15%" height="20" valign="middle">&nbsp;<asp:textbox id="txtSearch" 
                                    tabIndex="1" runat="server" Width="94%" CssClass="TextBox"  
                                    heigth="16px" ></asp:textbox>
                               
                          </td>
                         
                          <td class="smalltitle" align="center" width="3%">
                            状态
                          </td>
                          <td style="width:20%">
                             &nbsp;<asp:RadioButton ID="opt1" runat="server" Checked="True" GroupName="LabelSource" Text="全部" />&nbsp;
                                   <asp:RadioButton ID="opt2" runat="server" GroupName="LabelSource" Text="已作业" />&nbsp;
                                   <asp:RadioButton ID="opt3" runat="server" GroupName="LabelSource" Text="未作业" />&nbsp;
                          </td>
                           <td align="left" style="width:20%">
                             &nbsp;<asp:button id="btnSearch" tabIndex="2" runat="server" Width="58px" CssClass="ButtonQuery" Text="查询" onclick="btnSearch_Click"></asp:button>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnRefresh" runat="server" CssClass="ButtonRefresh" OnClientClick="return Refresh()" tabIndex="2" Text="刷新" Width="58px" />
                          </td>
                          <td align="right"  style="width:25%">
                             <%-- <asp:Button ID="btnPrint" runat="server" Text="导出" CssClass="ButtonPrint" 
                                 OnClientClick="return print();"/>--%>
                            <asp:Button ID="btnCheck" runat="server" Text="出库作业" CssClass="ButtonCreate" 
                                  onclick="btnCheck_Click" /> &nbsp;
                            <asp:Button ID="btnReCheck" runat="server" Text="取消作业" CssClass="ButtonDel" 
                                onclick="btnDeletet_Click" 
                                    />&nbsp;
                            <asp:Button ID="btnExit" runat="server" Text="离开" CssClass="ButtonExit" OnClientClick="return Exit()" 
                                   />&nbsp;
                          </td>
                    </tr>
                </table>
              
                </div>
         
          <div id="Maincontainer" style="overflow:auto; WIDTH: 100%; height:290px;"  runat="server" onscroll="javascript:RecordPostion(this);">
                
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
                <asp:BoundField DataField="BillID" HeaderText="出库单号" 
                    SortExpression="BillID"  >
                    <ItemStyle HorizontalAlign="Left" Width="15%" Wrap="False" />
                    <HeaderStyle Wrap="False" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="出库日期" SortExpression="BillDate">
                    <ItemTemplate>
                        <%# ToYMD(DataBinder.Eval(Container.DataItem, "BillDate"))%>
                    </ItemTemplate>
                    <HeaderStyle Wrap="False" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="False" />
                </asp:TemplateField> 
                   
                 <asp:BoundField DataField="SourceNo" HeaderText="销售单号" 
                    SortExpression="SourceNo"  >
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
                 <%--<asp:BoundField DataField="Creator" HeaderText="建单人员" 
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
                </asp:TemplateField>--%>
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
                <asp:BoundField DataField="Tasker" HeaderText="排单人员" 
                    SortExpression="Tasker"  >
                    <ItemStyle HorizontalAlign="Left" Width="15%" Wrap="False" />
                    <HeaderStyle Wrap="False" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="排单日期" SortExpression="TaskDate">
                    <ItemTemplate>
                        <%# ToYMD(DataBinder.Eval(Container.DataItem, "TaskDate"))%>
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

          <div>
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
                        </Columns>
                        <PagerSettings Visible="false" />
                    </asp:GridView>
            </div>
             <table width="100%" class="maintable"> 
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
                                <asp:Literal ID="Literal3" Text="每页" runat="server" Visible="false"  />
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
