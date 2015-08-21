<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerEdit.aspx.cs" Inherits="WMS.WebUI.CMD.CustomerEdit" %>

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
                BindSubEvent();
            });
            function BindSubEvent() {
                $("#btnCustomer").bind("click", function () {
                    GetOtherValue('CMD_CUSTOMER', 'txtPLAYCUSTOMER_CODE,txtPlayCustomer_Name', "CUSTOMER_CODE,CUSTOMER_NAME", "1=1");
                    return false;
                });
            }

            function Save() {

                if (trim($("#txtID").val()) == "") {
                    alert("客户编码不能为空!");
                    $("#txtID").focus();
                    return false;
                }

                if (trim($("#txtCustomer_Name").val()) == "") {
                    alert("客户名称不能为空!");
                    $("#txtCustomer_Name").focus();
                    return false;
                }
                return true;
            }
           
        </script>
	   
	    <style type="text/css">
            .style1
            {
                width: 35%;
            }
        </style>
	   
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
			            <table id="Table1" class="maintable" cellspacing="0" cellpadding="0" width="100%" align="center" bordercolor="#ffffff"
				            border="1" runat="server">			
					            <tr>
                                    <td align="center" class="musttitle" style="width:8%;"  >
                                         客户编码
                                    </td>
                                    <td class="style1">
                                         &nbsp;<asp:TextBox ID="txtID" runat="server"   
                                             BorderWidth="0" CssClass="TextBox" Width="46%" ></asp:TextBox>
                                    </td>
                                     <td  align="center" class="musttitle" style="width:8%;" >
                                         客户名称
                                    </td>
                                    <td>
                                     &nbsp;<asp:TextBox 
                                            ID="txtCustomer_Name" runat="server"  BorderWidth="0"
                                            CssClass="TextBox" Width="42%"></asp:TextBox>
                                    </td>
                                </tr>
                                 <tr>
                                    <td align="center" class="smalltitle" style="width:8%;"  >
                                         子公司
                                    </td>
                                    <td class="style1"  >
                                         &nbsp;<asp:TextBox 
                                             ID="txtPLAYCUSTOMER_CODE" runat="server"   
                                             BorderWidth="0" CssClass="TextBox" Width="0px" ></asp:TextBox>
                                             <asp:TextBox ID="txtPlayCustomer_Name" runat="server"  BorderWidth="0"
                                            CssClass="TextRead" Width="90%"></asp:TextBox><asp:Button ID="btnCustomer"  
                                             CssClass="ButtonCss" Width="20px" 
                                             runat="server"  Text="..." Height="17px" />
                                    </td>
                                     <td  align="center" class="smalltitle" style="width:8%;" >
                                         传真
                                    </td>
                                    <td>
                                     &nbsp;<asp:TextBox 
                                            ID="txtFax" runat="server"  BorderWidth="0"
                                            CssClass="TextBox" Width="42%"></asp:TextBox>
                                    </td>
                                </tr>
                                 <tr>
                                    <td align="center"  class="smalltitle" style="width:8%;">
                                        公司类别
                                    </td>
                                    <td class="style1" >
                                        &nbsp;<asp:RadioButton ID="opt1" runat="server" Checked="True" 
                                GroupName="LabelSource" Text="子公司直营" />&nbsp;<asp:RadioButton ID="opt2" 
                                            runat="server" GroupName="LabelSource" 
                                Text="子公司经销商" />&nbsp;<asp:RadioButton ID="opt3" runat="server" GroupName="LabelSource" 
                                Text="区域经销商" />&nbsp;<asp:RadioButton ID="opt4" runat="server" GroupName="LabelSource" 
                                Text="网渠" />&nbsp;<asp:RadioButton ID="opt5" runat="server" GroupName="LabelSource" 
                                Text="其它" />
                                    </td>
                                     <td  align="center" class="smalltitle" style="width:8%;" >
                                         归属地区
                                    </td>
                                    <td>
                                         &nbsp;<asp:TextBox ID="txtAreaStation" runat="server"  BorderWidth="0" CssClass="TextBox" Width="42%"></asp:TextBox>
                                    </td>
                                </tr>	
                                <tr>
                                    <td align="center" class="smalltitle" style="width:8%;"  >
                                         联系人
                                    </td>
                                    <td class="style1">
                                         &nbsp;<asp:TextBox ID="txtCustomer_Person" runat="server"   
                                             BorderWidth="0" CssClass="TextBox" Width="90%" ></asp:TextBox>
                                    </td>
                                     <td  align="center" class="smalltitle" style="width:8%;" >
                                         联系电话
                                    </td>
                                    <td>
                                     &nbsp;<asp:TextBox 
                                            ID="txtCustomer_Phone" runat="server"  BorderWidth="0"
                                            CssClass="TextBox" Width="42%"></asp:TextBox>
                                    </td>
                                </tr>
                               
                                <tr>
                                    <td align="center"  class="smalltitle" style="width:8%;">
                                        备注
                                    </td>
                                    <td colspan="3">
                                        &nbsp;<asp:TextBox ID="txtMemo" runat="server" CssClass="MultiLineTextBox" 
                                            TextMode="MultiLine" Height="102px" Width="69%"></asp:TextBox>
                                    </td>
                                </tr>		
			            </table>
			            <table style="width:100%">
                            <tr>
                                <td class="table_titlebgcolor" height="25px">
                                   
                                    <asp:Button  id="btnAddDetail" CssClass="ButtonCss" runat="server" Text="新增明细" 
                                        style="width:60px;" onclick="btnAddDetail_Click"  /> &nbsp;
                                    <asp:Button  id="btnDelDetail" CssClass="ButtonCss" runat="server" Text="删除明细" 
                                        style="width:60px;" onclick="btnDelDetail_Click" />
                                            &nbsp;
                                    </td>
                                
                            </tr>
                        </table> 
                        <div style="overflow: auto; width: 100%; height: 320px" >
                            <asp:GridView ID="dgViewSub1" runat="server" AutoGenerateColumns="False"  SkinID="GridViewSkin"
                                Width="100%" onrowdatabound="dgViewSub1_RowDataBound" >
                                <Columns>
                                    <asp:TemplateField  >
                                        <HeaderTemplate>
                                        <input type="checkbox" onclick="selectAll('dgViewSub1',this.checked);" />                    
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                        <asp:CheckBox id="cbSelect" runat="server" ></asp:CheckBox>                   
                                        </ItemTemplate>
                                        <HeaderStyle Width="4%"></HeaderStyle>
                                        <ItemStyle Width="4%" HorizontalAlign="Center"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="序号">
                                        <ItemTemplate>
                                            <asp:Label ID="RowID" runat="server" Text=""></asp:Label>
                                        
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                        <HeaderStyle Width="4%" ForeColor="Blue" />
                                    </asp:TemplateField>
                            
                                    <asp:TemplateField HeaderText="收货人">
                                        <ItemTemplate>
                                            <asp:TextBox ID="PERSON" runat="server" Width="98%" CssClass="detailtext"></asp:TextBox> 
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle Width="9%" ForeColor="Blue" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="收货电话">
                                            <ItemTemplate>
                                            <asp:TextBox ID="PHONE" runat="server"  Width="98%" CssClass="detailtext"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle Width="12%" ForeColor="Blue" />
                                    </asp:TemplateField>
                                        <asp:TemplateField HeaderText="收货地址">
                                        <ItemTemplate>
                                            <asp:TextBox ID="Address" runat="server" Width="98%" CssClass="detailtext"></asp:TextBox> 
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle Width="30%" ForeColor="Blue" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="排列顺序">
                                        <ItemTemplate>
                                            <asp:TextBox ID="OrderNum" runat="server" Width="98%" MaxLength="5" CssClass="detailtext"></asp:TextBox> 
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle Width="10%" ForeColor="Blue" />
                                    </asp:TemplateField>
                                </Columns>
                        
                                <PagerSettings Visible="false" />
                        </asp:GridView>
                        </div>
                  </ContentTemplate>
              </asp:UpdatePanel>       
		</form>
	</body>
</html>
