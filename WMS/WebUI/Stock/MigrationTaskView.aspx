<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MigrationTaskView.aspx.cs" Inherits="WMS.WebUI.Stock.MigrationTaskView" %>


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
                                   <asp:ListItem Value="AREA_NAME">库区</asp:ListItem>  	 			
                                    <asp:ListItem Value="SHELF_NAME">货架</asp:ListItem>
                                    <asp:ListItem Value="PRODUCT_MODEL">产品型号</asp:ListItem>
                                    <asp:ListItem Value="PRODUCT_NAME">产品名称</asp:ListItem>
                                    <asp:ListItem Value="COLOR_NAME">规格</asp:ListItem>
                                    <asp:ListItem Value="PalletCode">托盘条码</asp:ListItem>
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
                          <td >
                          
                          </td>
                          <td align="right"  style="width:30%">
                             <%-- <asp:Button ID="btnPrint" runat="server" Text="导出" CssClass="ButtonPrint" 
                                 OnClientClick="return print();"/>--%>
                            <asp:Button ID="btnExit" runat="server" Text="离开" CssClass="ButtonExit" OnClientClick="return Exit()" 
                                 Width="51px" />
                          </td>
                    </tr>
                </table>
              
                </div>

            <div id="table-container" style="overflow: auto; WIDTH: 100%; HEIGHT: 480px">
                
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                    SkinID="GridViewSkin" Width="98%" 
                    OnRowDataBound="gvMain_RowDataBound">
            <Columns>
                <asp:BoundField DataField="AREA_NAME" HeaderText="库区" 
                    SortExpression="AREA_NAME"  >
                    <ItemStyle HorizontalAlign="Left" Width="15%" Wrap="False" />
                    <HeaderStyle Wrap="False" />
                </asp:BoundField> 
                 <asp:BoundField DataField="SHELF_NAME" HeaderText="货架" 
                    SortExpression="SHELF_NAME"  >
                    <ItemStyle HorizontalAlign="Left" Width="15%" Wrap="False" />
                    <HeaderStyle Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="CELL_NAME" HeaderText="货位" 
                    SortExpression="CELL_NAME"  >
                    <ItemStyle HorizontalAlign="Left" Width="15%" Wrap="False" />
                    <HeaderStyle Wrap="False" />
                </asp:BoundField>
                 <asp:BoundField DataField="PRODUCT_NAME" HeaderText="产品部件" 
                    SortExpression="PRODUCT_NAME"  >
                    <ItemStyle HorizontalAlign="Left" Width="15%" Wrap="False" />
                    <HeaderStyle Wrap="False" />
                </asp:BoundField>
                 <asp:BoundField DataField="PRODUCT_MODEL" HeaderText="产品型号" 
                    SortExpression="PRODUCT_MODEL"  >
                    <ItemStyle HorizontalAlign="Left" Width="15%" Wrap="False" />
                    <HeaderStyle Wrap="False" />
                </asp:BoundField>
                 <asp:BoundField DataField="COLOR_NAME" HeaderText="规格" 
                    SortExpression="COLOR_NAME"  >
                    <ItemStyle HorizontalAlign="Left" Width="15%" Wrap="False" />
                    <HeaderStyle Wrap="False" />
                </asp:BoundField>
                 <asp:BoundField DataField="PalletCode" HeaderText="托盘条码" 
                    SortExpression="PalletCode"  >
                    <ItemStyle HorizontalAlign="Left" Width="15%" Wrap="False" />
                    <HeaderStyle Wrap="False" />
                </asp:BoundField>
             
                <asp:BoundField DataField="Qty" HeaderText="数量" 
                    SortExpression="Qty"  >
                    <ItemStyle HorizontalAlign="Right" Width="15%" Wrap="False" />
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
            </ContentTemplate>
        </asp:UpdatePanel> 
    </form>
</body>
</html>
