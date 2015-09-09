<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BarCodeQuery.aspx.cs" Inherits="WMS.WebUI.Query.BarCodeQuery" %>

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
        function BarCodeSearch() {
            if ($('#txtSearch').val() == "") {
                alert("产品条码为19位，不能为空！");
                $('#txtSearch').focus();
                return false;
            }
            if ($('#txtSearch').val().length != 19) {
                alert("产品条码为19位，请输入完整条码！");
                $('#txtSearch').focus();
                $('#txtSearch').select();
                return false;
            }
            return true;
        }
        function selectRow(index) {
            document.getElementById('hdnRowIndex').value = index;
            document.getElementById('btnReload').click();
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
                <table  style="width: 100%; height: 20px;" class="OperationBar">
                    <tr>
						    
						    <td class="smalltitle" align="center" width="6%">
                                <asp:Literal ID="Literal2" Text="条码"
                                    runat="server" ></asp:Literal>
                            </td>
						    <td  width="34%" height="20" valign="middle">&nbsp;<asp:textbox id="txtSearch" 
                                    tabIndex="1" runat="server" Width="74%" CssClass="TextBox" MaxLength="19" heigth="16px" ></asp:textbox>
                                &nbsp;<asp:button id="btnSearch" tabIndex="2" runat="server" Width="58px" 
                                    CssClass="ButtonQuery" Text="查询" OnClientClick="return BarCodeSearch();"  
                                    onclick="btnSearch_Click"></asp:button>
                          </td>
                          
                          <td align="right"  style="width:60%">
                            
                          </td>
                    </tr>
                </table>
              
                </div>

            <table  style="width: 100%;" class="maintable">
                <tr>
                <td style="width:50%" >
                     <div id="Div1" style="overflow: auto; WIDTH: 100%; HEIGHT: 500px">
                
                        <asp:GridView ID="dgMain" runat="server" AutoGenerateColumns="False" 
                             SkinID="GridViewSkin" Width="98%" 
                             onrowdatabound="dgMain_RowDataBound" >
                        <Columns>
                            <asp:BoundField DataField="RowID" HeaderText="序号" 
                                SortExpression="RowID" >
                                <ItemStyle HorizontalAlign="Left" Width="8%" Wrap="False" />
                                <HeaderStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Barcode" HeaderText="条码" SortExpression="Style" >
                                <ItemStyle HorizontalAlign="Left" Width="30%" Wrap="False" />
                                <HeaderStyle Wrap="False" />
                            </asp:BoundField>
                             <asp:BoundField DataField="ProductModel" HeaderText="产品型号" SortExpression="Style" >
                                <ItemStyle HorizontalAlign="Left" Width="20%" Wrap="False" />
                                <HeaderStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ProductName" HeaderText="产品品名" SortExpression="Style" >
                                <ItemStyle HorizontalAlign="Left" Width="20%" Wrap="False" />
                                <HeaderStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ColorName" HeaderText="规格" SortExpression="Style" >
                                <ItemStyle HorizontalAlign="Left" Width="10%" Wrap="False" />
                                <HeaderStyle Wrap="False" />
                            </asp:BoundField>
                        </Columns>
                        <PagerSettings Visible="False" />
                    </asp:GridView>
                    </div>
                </td>
                <td class="smalltitle" style="width:1%">
                </td>
                <td style="width:49%">
                    <div id="table-container" style="overflow: auto; WIDTH: 100%; HEIGHT: 500px">
                
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" SkinID="GridViewSkin" Width="98%"   >
            <Columns>
                <asp:BoundField DataField="Billid" HeaderText="单据内容" 
                    SortExpression="Billid" >
                    <ItemStyle HorizontalAlign="Left" Width="20%" Wrap="False" />
                    <HeaderStyle Wrap="False" />
                </asp:BoundField>

                
                <asp:BoundField DataField="Style" HeaderText="其他1" 
                    SortExpression="Style" >
                    <ItemStyle HorizontalAlign="Left" Width="40%" Wrap="False" />
                    <HeaderStyle Wrap="False" />
                </asp:BoundField>
                 <asp:BoundField DataField="BarState" HeaderText="其他2" 
                    SortExpression="BarState" >
                    <ItemStyle HorizontalAlign="Left" Width="40%" Wrap="False" />
                    <HeaderStyle Wrap="False" />
                </asp:BoundField>
            </Columns>
            <PagerSettings Visible="False" />
        </asp:GridView>
            </div>
                </td>
                </tr>
               
            </table>

              <div style="font-size: 0px; bottom: 0px; right: 0px;">
                <asp:Button ID="btnReload" runat="server" Text="" OnClick="btnReload_Click" Height="0px" Width="0px" style=" display:none" />
                <asp:HiddenField ID="hdnRowIndex" runat="server" Value="0" />
                <asp:HiddenField ID="hdnRowValue" runat="server"  />
             </div>

            
       </ContentTemplate>
            </asp:UpdatePanel> 
    </form>
</body>
</html>
