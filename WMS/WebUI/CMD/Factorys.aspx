<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Factorys.aspx.cs" Inherits="WMS.WebUI.CMD.Factorys" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <title></title> 
    <link href="~/Css/Main.css" type="text/css" rel="stylesheet" /> 
    <link href="~/Css/op.css" type="text/css" rel="stylesheet" /> 
    <script type="text/javascript" src="../../JQuery/jquery-1.8.3.min.js"></script>
    <script type="text/javascript" src= "../../JScript/Common.js"></script>
    <script type="text/javascript" src='<%=ResolveUrl("~/JScript/Ajax.js") %>'></script> 
    <script type="text/javascript" src='<%=ResolveUrl("~/JScript/Resize.js") %>'></script>
    <script type='text/javascript'>
        
     
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
                                    <asp:ListItem Selected="True" Value="FactoryID">供应商编码</asp:ListItem>
                                    <asp:ListItem Value="FactoryName">厂商名称</asp:ListItem>
                                    <asp:ListItem Value="LinkPerson">联络人</asp:ListItem>
                                    <asp:ListItem Value="LinkPhone">联络电话</asp:ListItem>
                                    <asp:ListItem Value="Memo">备注</asp:ListItem>
                                 </asp:dropdownlist>
                            </td>
						    <td class="smalltitle" align="center" width="6%">
                                <asp:Literal ID="Literal2" Text="查询内容"
                                    runat="server" ></asp:Literal>
                            </td>
						    <td  width="34%" height="20" valign="middle">&nbsp;<asp:textbox id="txtSearch" 
                                    tabIndex="1" runat="server" Width="74%" CssClass="TextBox" MaxLength="100" 
                                    heigth="16px" ></asp:textbox>
                                &nbsp;<asp:button id="btnSearch" tabIndex="2" runat="server" Width="58px" 
                                    CssClass="ButtonQuery" Text="查询" OnClientClick="return Search()"  
                                    onclick="btnSearch_Click"></asp:button>
                          </td>
                          <td >
                          
                              <asp:Button ID="btnRefresh" runat="server" CssClass="ButtonRefresh" 
                                  onclick="btnSearch_Click" OnClientClick="return Refresh()" tabIndex="2" 
                                  Text="刷新" Width="58px" />
                          
                          </td>
                          <td align="right"  style="width:30%">
                            <%--  <asp:Button ID="btnPrint" runat="server" Text="导出" CssClass="ButtonPrint" 
                                 OnClientClick="return print();"/>--%>
                              <asp:Button ID="btnPrint" runat="server" CssClass="ButtonPrint" 
                                  onclick="btnPrint_Click" Text="导出" />
                            <asp:Button ID="btnAdd" runat="server" Text="新增" OnClientClick="Add();" CssClass="ButtonCreate"                            
                                 />
                            <asp:Button ID="btnDelete" runat="server" Text="刪除" CssClass="ButtonDel" 
                                onclick="btnDeletet_Click" OnClientClick="return Delete('GridView1')" 
                                   Width="51px"/>
                            <asp:Button ID="btnExit" runat="server" Text="离开" CssClass="ButtonExit" OnClientClick="return Exit()" 
                                 Width="51px" />
                          </td>
                    </tr>
                </table>
              
                </div>

            <div id="table-container" style="overflow: auto; WIDTH: 100%; HEIGHT: 470px">
                
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                    SkinID="GridViewSkin" Width="1100px"   
                    OnSorting="GridView1_Sorting" OnRowDataBound="GridView1_RowDataBound">
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>
                    <input type="checkbox" onclick="selectAll('GridView1',this.checked);" />                    
                    </HeaderTemplate>
                    <ItemTemplate>
                    <asp:CheckBox id="cbSelect" runat="server" ></asp:CheckBox>                   
                    </ItemTemplate>
                  <HeaderStyle Width="60px"></HeaderStyle>
                 <ItemStyle Width="60px" HorizontalAlign="Center"></ItemStyle>
               </asp:TemplateField>
                <asp:TemplateField HeaderText="供应商编码"  SortExpression="FactoryID" 
                    >
                    <ItemTemplate>
                     <asp:LinkButton  id="LinkButton1" runat="server" OnClientClick="return ShowViewForm(this);" Text='<%# DataBinder.Eval(Container.DataItem, "FactoryID") %>' >
                    </asp:LinkButton>                          
                    </ItemTemplate>
                  <ItemStyle Width="10%" Wrap="False" HorizontalAlign="Left"/>
                  <HeaderStyle Width="10%" Wrap="False" />
               </asp:TemplateField>
                <asp:BoundField DataField="FactoryName" HeaderText="供应商名称" 
                    SortExpression="FactoryName" >
                    <ItemStyle HorizontalAlign="Left" Width="15%" Wrap="False" />
                    <HeaderStyle Wrap="False" />
                </asp:BoundField>
                 <asp:BoundField DataField="LinkPerson" HeaderText="联系人" 
                    SortExpression="LinkPerson" >
                    <ItemStyle HorizontalAlign="Left" Width="15%" Wrap="False" />
                    <HeaderStyle Wrap="False" />
                </asp:BoundField>
                 <asp:BoundField DataField="LinkPhone" HeaderText="联系电话" 
                    SortExpression="LinkPhone" >
                    <ItemStyle HorizontalAlign="Left" Width="15%" Wrap="False" />
                    <HeaderStyle Wrap="False" />
                </asp:BoundField>
                 <asp:BoundField DataField="Fax" HeaderText="传真" 
                    SortExpression="Fax" >
                    <ItemStyle HorizontalAlign="Left" Width="15%" Wrap="False" />
                    <HeaderStyle Wrap="False" />
                </asp:BoundField>
                 <asp:BoundField DataField="Address" HeaderText="地址" 
                    SortExpression="Address" >
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
                <asp:BoundField DataField="Updater" HeaderText="修改人员" 
                    SortExpression="Updater"  >
                    <ItemStyle HorizontalAlign="Left" Width="15%" Wrap="False" />
                    <HeaderStyle Wrap="False" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="修改日期" SortExpression="UpdateDate">
                    <ItemTemplate>
                        <%# ToYMD(DataBinder.Eval(Container.DataItem, "UpdateDate"))%>
                    </ItemTemplate>
                    <HeaderStyle Wrap="False" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="False" />
                </asp:TemplateField>
                <asp:BoundField DataField="MEMO" HeaderText="备注" 
                    SortExpression="MEMO" >
                    <ItemStyle HorizontalAlign="Left" Width="70%" Wrap="False" />
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
       </ContentTemplate>
       <Triggers>  
            <asp:PostBackTrigger ControlID="btnPrint" />  
       </Triggers>
            </asp:UpdatePanel> 
    </form>
</body>
</html>
