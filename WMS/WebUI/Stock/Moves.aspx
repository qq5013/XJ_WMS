﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Moves.aspx.cs" Inherits="WMS.WebUI.Stock.Moves" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Css/Main.css" type="text/css" rel="stylesheet" /> 
    <link href="~/Css/op.css" type="text/css" rel="stylesheet" /> 
    <script type="text/javascript" src="../../JQuery/jquery-1.8.3.min.js"></script>
    <script type="text/javascript" src= "../../JScript/Common.js"></script>
    <script type="text/javascript" language="javascript">
    $(document).ready(function () {     
            $(window).resize(function () {
                resize();
            });
        });
        function resize() {
            var h = document.documentElement.clientHeight - 260;
            $("#table-container").css("height", h);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />  
        <asp:UpdateProgress ID="updateprogress" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
            <ProgressTemplate>            
                <div id="progressBackgroundFilter" style="display:none"></div>
                <div id="processMessage"> Loading...<br /><br />
                        <img alt="Loading" src="../../images/loading.gif" />
                </div>      
            </ProgressTemplate> 
        </asp:UpdateProgress>  
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">                
            <ContentTemplate>
                <div>
                    <table  style="width: 100%; height: 20px;">
                    <tr>
						    <td class="smalltitle" align="center" width="7%" >
                                <asp:Literal ID="Literal1" Text="查询栏位" runat="server"  ></asp:Literal>
                             </td>
						    <td  width="15%" height="20">&nbsp;<asp:dropdownlist id="ddlField" runat="server" Width="85%" >
                                    <asp:ListItem Selected="True" Value="AreaName">库区</asp:ListItem>
                                    <asp:ListItem Value="BillID">移库单号</asp:ListItem>
                                    <asp:ListItem Value="Memo">备注</asp:ListItem>
                                 </asp:dropdownlist>
                            </td>
						    <td class="smalltitle" align="center" width="7%">
                                <asp:Literal ID="Literal2" Text="查询内容" runat="server"></asp:Literal>
                            </td>
						    <td  width="26%" height="20" valign="middle">&nbsp;<asp:textbox id="txtSearch" 
                                    tabIndex="1" runat="server" Width="90%" CssClass="TextBox"  
                                    heigth="16px" ></asp:textbox>
                               
                          </td>
                          <td width="15%" align="left">
                           &nbsp;<asp:button id="btnSearch" tabIndex="2" runat="server" Width="58px" 
                                    CssClass="ButtonQuery" Text="查询" OnClientClick="return Search()" 
                                    onclick="btnSearch_Click"></asp:button>&nbsp;&nbsp;
                              <asp:Button ID="btnRefresh" runat="server" CssClass="ButtonRefresh" 
                                  onclick="btnSearch_Click" OnClientClick="return Refresh()" tabIndex="2" 
                                  Text="刷新" Width="58px" />
                          
                          </td>
                          <td align="right"  style="width:30%" valign="middle">
                             <%-- <asp:Button ID="btnPrint" runat="server" Text="导出" CssClass="ButtonPrint" OnClientClick="return print();"/>--%>
                           
                            <asp:Button ID="btnAdd" runat="server" Text="新增" OnClientClick="return Add();" CssClass="ButtonCreate"/>&nbsp;
                            <asp:Button ID="btnDelete" runat="server" Text="刪除" CssClass="ButtonDel" onclick="btnDeletet_Click" OnClientClick="return Delete('GridView1')" Width="51px"/>&nbsp;
                             <asp:Button ID="btnPrint" runat="server" CssClass="ButtonPrint"   Text="打印" />&nbsp;
                            <asp:Button ID="btnExit" runat="server" Text="离开" CssClass="ButtonExit" OnClientClick="return Exit()" Width="51px" />&nbsp;&nbsp;
                            
                          </td>
                    </tr>
                </table>
                    
                </div>
                <div id="table-container" style="overflow: auto; WIDTH: 100%; HEIGHT: 200px">
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" SkinID="GridViewSkin" Width="100%" OnRowDataBound="GridView1_RowDataBound">
                        <Columns>
                            <asp:TemplateField >
                                <HeaderTemplate>
                                <input type="checkbox" onclick="selectAll('GridView1',this.checked);" />                    
                                </HeaderTemplate>
                                <ItemTemplate>
                                <asp:CheckBox id="cbSelect" runat="server" ></asp:CheckBox>                   
                                </ItemTemplate>
                              <HeaderStyle Width="60px"></HeaderStyle>
                             <ItemStyle Width="60px" HorizontalAlign="Center"></ItemStyle>
                           </asp:TemplateField>
                           <asp:TemplateField HeaderText="移库单号" SortExpression="BillID">
                                <ItemTemplate>
                                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# FormID+"View.aspx?SubModuleCode=" + SubModuleCode+"&FormID=" + FormID +"&SqlCmd="+SqlCmd+ "&ID="+DataBinder.Eval(Container.DataItem, "BillID") %>'
                                        Text='<%# DataBinder.Eval(Container.DataItem, "BillID")%>'></asp:HyperLink>
                                </ItemTemplate>
                                <ItemStyle Width="12%" Wrap="False" />
                                <HeaderStyle Width="12%" Wrap="False" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="移库日期" SortExpression="BillDate">
                                <ItemTemplate>
                                    <%# ToYMD(DataBinder.Eval(Container.DataItem, "BillDate"))%>
                                </ItemTemplate>
                                <HeaderStyle Wrap="False" />
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="False" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="StateDesc" HeaderText="单据状态" SortExpression="StateDesc">
                                <ItemStyle HorizontalAlign="Left" Width="10%" Wrap="False" />
                                <HeaderStyle Wrap="False" />
                            </asp:BoundField>
                            
                            <asp:BoundField DataField="AreaName" HeaderText="库区" SortExpression="AreaName">
                                <ItemStyle HorizontalAlign="Left" Width="10%" Wrap="False" />
                                <HeaderStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Memo" HeaderText="备注" 
                                SortExpression="Memo" >
                                <ItemStyle HorizontalAlign="Left" Width="15%" Wrap="False" />
                                <HeaderStyle Wrap="False" />
                            </asp:BoundField>
                             <asp:BoundField DataField="Checker" HeaderText="审核人员" 
                                SortExpression="Checker"  >
                                <ItemStyle HorizontalAlign="Left" Width="10%" Wrap="False" />
                                <HeaderStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="审核日期" SortExpression="CheckDate">
                                <ItemTemplate>
                                    <%# ToYMD(DataBinder.Eval(Container.DataItem, "CheckDate"))%>
                                </ItemTemplate>
                                <HeaderStyle Wrap="False" />
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="False" />
                            </asp:TemplateField>

                             <asp:BoundField DataField="Creator" HeaderText="建单人员" 
                                SortExpression="Creator"  >
                                <ItemStyle HorizontalAlign="Left" Width="10%" Wrap="False" />
                                <HeaderStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="建单日期" SortExpression="CreateDate">
                                <ItemTemplate>
                                    <%# ToYMD(DataBinder.Eval(Container.DataItem, "CreateDate"))%>
                                </ItemTemplate>
                                <HeaderStyle Wrap="False" />
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="False" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="Updater" HeaderText="修改人员" 
                                SortExpression="Updater"  >
                                <ItemStyle HorizontalAlign="Left" Width="10%" Wrap="False" />
                                <HeaderStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="修改日期" SortExpression="UpdateDate">
                                <ItemTemplate>
                                    <%# ToYMD(DataBinder.Eval(Container.DataItem, "UpdateDate"))%>
                                </ItemTemplate>
                                <HeaderStyle Wrap="False" />
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="False" />
                            </asp:TemplateField>
               
                        </Columns>
                        <PagerSettings Visible="False" />
                    </asp:GridView>
                </div>
                <div>
                    &nbsp;&nbsp;<asp:LinkButton ID="btnFirst" runat="server" OnClick="btnFirst_Click" Text="首页"></asp:LinkButton> 
                    &nbsp;<asp:LinkButton ID="btnPre" runat="server" OnClick="btnPre_Click" Text="上一页"></asp:LinkButton> 
                    &nbsp;<asp:LinkButton ID="btnNext" runat="server" OnClick="btnNext_Click" Text="下一页"></asp:LinkButton> 
                    &nbsp;<asp:LinkButton ID="btnLast" runat="server" OnClick="btnLast_Click" Text="尾页"></asp:LinkButton> 
                    &nbsp;<asp:textbox id="txtPageNo" onkeypress="return regInput(this,/^\d+$/,String.fromCharCode(event.keyCode))"
					        onpaste="return regInput(this,/^\d+$/,window.clipboardData.getData('Text'))" ondrop="return regInput(this,/^\d+$/,event.dataTransfer.getData('Text'))"
					        runat="server" Width="56px" CssClass="TextBox" ></asp:textbox>
                    &nbsp;<asp:linkbutton id="btnToPage" runat="server" onclick="btnToPage_Click" Text="跳转"></asp:linkbutton>
                    &nbsp;<asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="True"  Visible="false"></asp:DropDownList>
                    &nbsp;<asp:Label ID="lblCurrentPage" runat="server" ></asp:Label>
                </div>
                 <table class="maintable" cellpadding="0" cellspacing="0" style="width: 100%; height:24px">
                    <tr>
                        <td valign="middle" align="left" height="22px">
                            <b>移库单明细</b> 
                        </td>
                    </tr>
                </table>
                 <div id="divSub" style="overflow: auto; WIDTH: 100%; HEIGHT: 155px">
                    <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" AllowPaging="True" PageSize="10" SkinID="GridViewSkin" Width="100%">
                        <Columns>
                            <asp:BoundField DataField="RowID" HeaderText="序号" SortExpression="RowID">
                                <ItemStyle HorizontalAlign="Left" Width="7%" Wrap="False" />
                                <HeaderStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ProductCode" HeaderText="产品编号" SortExpression="ProductCode">
                                <ItemStyle HorizontalAlign="Left" Width="20%" Wrap="False" />
                                <HeaderStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ProductName" HeaderText="品名" SortExpression="ProductName">
                                <ItemStyle HorizontalAlign="Left" Width="25%" Wrap="False" />
                                <HeaderStyle Wrap="False" />
                            </asp:BoundField>
                           
                            <asp:BoundField DataField="Quantity" HeaderText="数量" SortExpression="Quantity">
                                <ItemStyle HorizontalAlign="Left" Width="10%" Wrap="False" />
                                <HeaderStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CellCode" HeaderText="货位" SortExpression="CellCode">
                                <ItemStyle HorizontalAlign="Left" Width="10%" Wrap="False" />
                                <HeaderStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="NewCellCode" HeaderText="新货位" SortExpression="NewCellCode">
                                <ItemStyle HorizontalAlign="Left" Width="10%" Wrap="False" />
                                <HeaderStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Memo" HeaderText="备注" 
                                SortExpression="Memo" >
                                <ItemStyle HorizontalAlign="Left" Width="38%" Wrap="False" />
                                <HeaderStyle Wrap="False" />
                            </asp:BoundField>
                        </Columns>
                        <PagerSettings Visible="False" />
                    </asp:GridView>
                </div>
                <div style="height:23px;">
                    &nbsp;&nbsp;<asp:LinkButton ID="btnFirstSub1" runat="server" 
                        OnClick="btnFirstSub1_Click" Text="首页"></asp:LinkButton> 
                    &nbsp;<asp:LinkButton ID="btnPreSub1" runat="server" OnClick="btnPreSub1_Click" 
                        Text="上一页"></asp:LinkButton> 
                    &nbsp;<asp:LinkButton ID="btnNextSub1" runat="server" 
                        OnClick="btnNextSub1_Click" Text="下一页"></asp:LinkButton> 
                    &nbsp;<asp:LinkButton ID="btnLastSub1" runat="server" 
                        OnClick="btnLastSub1_Click" Text="尾页"></asp:LinkButton> 
                    &nbsp;<asp:textbox id="txtPageNoSub1" onkeypress="return regInput(this,/^\d+$/,String.fromCharCode(event.keyCode))"
					        onpaste="return regInput(this,/^\d+$/,window.clipboardData.getData('Text'))" ondrop="return regInput(this,/^\d+$/,event.dataTransfer.getData('Text'))"
					        runat="server" Width="56px" CssClass="TextBox" ></asp:textbox>
                    &nbsp;<asp:linkbutton id="btnToPageSub1" runat="server" 
                        onclick="btnToPageSub1_Click" Text="跳转"></asp:linkbutton>
                    &nbsp;<asp:DropDownList ID="ddlPageSizeSub" runat="server" AutoPostBack="True" Visible="false"></asp:DropDownList>
                    &nbsp;<asp:Label ID="lblCurrentPageSub1" runat="server" ></asp:Label>
                </div>
                <div>
                    <asp:Button ID="btnReload" runat="server" Text="" OnClick="btnReload_Click"  CssClass="HiddenControl" />
                    <asp:HiddenField ID="hdnRowIndex" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnRowValue" runat="server"  />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
