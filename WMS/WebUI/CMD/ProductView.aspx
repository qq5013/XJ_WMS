<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductView.aspx.cs" Inherits="WMS.WebUI.CMD.ProductView" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Css/Main.css" type="text/css" rel="stylesheet" /> 
    <link href="~/Css/op.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="../../JQuery/jquery-2.1.3.min.js"></script>
    <script type="text/javascript" src= "../../JScript/Common.js"></script> 
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />  
        <asp:UpdateProgress ID="updateprogress" runat="server" AssociatedUpdatePanelID="updatePanel">
            <ProgressTemplate>            
                <div id="progressBackgroundFilter" style="display:none"></div>
                <div id="processMessage"> Loading...<br /><br />
                     <img alt="Loading" src="../../images/main/loading.gif" />
                </div>            
             </ProgressTemplate>
        </asp:UpdateProgress>  
        <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional">                
            <ContentTemplate>
                 <table   style="width: 100%; height: 25px;" class="OperationBar">
                    <tr>
                        <td align="right" style="width:60%">
                            <asp:Button ID="btnFirst" runat="server" Text="首笔" CssClass="ButtonFirst" 
                                onclick="btnFirst_Click"  />
                            <asp:Button ID="btnPre" runat="server" Text="上一笔" CssClass="ButtonPre" 
                                onclick="btnPre_Click"  />
                            <asp:Button ID="btnNext" runat="server" Text="下一笔" CssClass="ButtonNext" 
                                onclick="btnNext_Click"  />
                            <asp:Button ID="btnLast" runat="server" Text="尾笔" CssClass="ButtonLast" 
                                onclick="btnLast_Click"  />
                        </td>
                        <td align="right">
                           <%-- <asp:Button ID="btnPrint" runat="server" Text="导出" CssClass="ButtonPrint" 
                                OnClientClick="return print();" />--%>
                            <asp:Button ID="btnAdd" runat="server" Text="新增" CssClass="ButtonCreate" OnClientClick="return Add();"  />
                            <asp:Button ID="btnDelete" runat="server" Text="刪除" CssClass="ButtonDel" OnClientClick="return ViewDelete();"
                                onclick="btnDelete_Click"  />
                            <asp:Button ID="btnEdit" runat="server" Text="修改" CssClass="ButtonModify" 
                                OnClientClick="return ViewEdit();" />
                            <asp:Button ID="btnBack" runat="server" Text="返回" OnClientClick="return Back();" 
                                CssClass="ButtonBack" />
                            <asp:Button ID="btnExit" runat="server" Text="离开" OnClientClick="return Exit();" 
                                CssClass="ButtonExit"  />
                        
                        </td>
                    </tr>
                 </table>
                 <table id="Table1" class="maintable"  width="100%" align="center" cellspacing="0" cellpadding="0" bordercolor="#ffffff" border="1"
				     runat="server">			
				<tr>
                    <td align="center" class="musttitle" style="width:12%;"  >
                            产品编码
                    </td>
                    <td  width="21%">
                            &nbsp;<asp:TextBox ID="txtID" runat="server"  CssClass="TextRead" Width="90%" MaxLength="20" ReadOnly="True"></asp:TextBox>
                    </td>
                    <td align="center" class="musttitle" style="width:12%;"  >
                           产品名称
                    </td>
                    <td width="21%">
                            &nbsp;<asp:TextBox ID="txtProductName" CssClass="TextRead" runat="server"   Width="90%" MaxLength="50" ReadOnly="True"></asp:TextBox> 
                          

                    </td>
                    <td align="center" class="musttitle" style="width:12%;">
                        产品类别
                    </td>
                    <td width="21%">
                        &nbsp;<asp:DropDownList ID="ddlProductTypeCode" Enabled="false" runat="server" Width="89%"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td align="center" class="musttitle" style="width:12%;"  >
                            车型
                    </td>
                    <td  width="21%">
                         &nbsp;<asp:DropDownList ID="ddlTrainTypeCode" Enabled="false" runat="server" Width="89%"></asp:DropDownList>
                    </td>
                    <td align="center" class="musttitle" style="width:12%;"  >
                           车轴编号 
                    </td>
                    <td width="21%">
                            &nbsp;<asp:TextBox ID="txtAxieNo" runat="server"    Width="90%" CssClass="TextRead" MaxLength="20" ReadOnly="True"></asp:TextBox> 

                    </td>
                    <td align="center" class="smalltitle" style="width:12%;">
                        轮径</td>
                    <td width="21%">
                        &nbsp;<asp:TextBox ID="txtWheelDiameter" runat="server" CssClass="TextRead"   Width="90%" MaxLength="25" ReadOnly="True"></asp:TextBox> 

                    </td>
                </tr>
                <tr>
                    <td align="center" class="smalltitle" style="width:12%;"  >
                            齿侧轴径
                    </td>
                    <td  width="21%">
                        &nbsp;<asp:TextBox ID="txtCCZ_Diameter" runat="server"   Width="90%" CssClass="TextRead" MaxLength="25" ReadOnly="True"></asp:TextBox> 

                    </td>
                    <td align="center" class="smalltitle" style="width:12%;"  >
                           非齿侧轴径 
                    </td>
                    <td width="21%">
                            &nbsp;<asp:TextBox ID="txtFCCZ_Diameter" runat="server"  Width="90%" CssClass="TextRead" MaxLength="25" ReadOnly="True"></asp:TextBox> 

                    </td>
                    <td align="center" class="smalltitle" style="width:12%;">
                        齿侧端轴直径</td>
                    <td width="21%">
                        &nbsp;<asp:TextBox ID="txtCCD_Diameter" runat="server"  Width="90%" CssClass="TextRead" MaxLength="25" ReadOnly="True"></asp:TextBox> 

                    </td>
                </tr>
                 <tr>
                    <td align="center" class="smalltitle" style="width:12%;"  >
                            非齿侧端轴直径
                    </td>
                    <td  width="21%">
                        &nbsp;<asp:TextBox ID="txtFCCD_Diameter" runat="server"  Width="90%" CssClass="TextRead" MaxLength="25" ReadOnly="True"></asp:TextBox> 

                    </td>
                    <td align="center" class="smalltitle" style="width:12%;"  >
                           齿侧选配抱轴瓦尺寸</td>
                    <td width="21%">
                            &nbsp;<asp:TextBox ID="txtCCXPBZW_Size" runat="server"   Width="90%" CssClass="TextRead"  MaxLength="25" ReadOnly="True"></asp:TextBox> 

                    </td>
                    <td align="center" class="smalltitle" style="width:12%;">
                        非齿侧选配抱轴瓦尺寸</td>
                    <td width="21%">
                        &nbsp;<asp:TextBox ID="txtFCCXPBZW_Size" runat="server"   Width="90%" CssClass="TextRead" MaxLength="25" ReadOnly="True"></asp:TextBox> 

                    </td>
                </tr>
                 <tr>
                    <td align="center" class="smalltitle" style="width:12%;"  >
                            齿轮编号</td>
                    <td  width="21%">
                        &nbsp;<asp:TextBox ID="txtGearNo" runat="server"  Width="90%" CssClass="TextRead" MaxLength="20" ReadOnly="True"></asp:TextBox> 

                    </td>
                    <td align="center" class="smalltitle" style="width:12%;"  >
                           齿侧轮芯标记 
                    </td>
                    <td width="21%">
                            &nbsp;<asp:TextBox ID="txtCCLX_Flag" runat="server"  Width="90%" CssClass="TextRead" MaxLength="25" ReadOnly="True"></asp:TextBox> 

                    </td>
                    <td align="center" class="smalltitle" style="width:12%;">
                        非齿侧轮芯标记</td>
                    <td width="21%">
                        &nbsp;<asp:TextBox ID="txtFCCLX_Flag" runat="server"   Width="90%" CssClass="TextRead" MaxLength="25" ReadOnly="True"></asp:TextBox> 

                    </td>
                </tr>
                 <tr>
                    <td align="center" class="smalltitle" style="width:12%;"  >
                            齿侧轮芯制造年代
                    </td>
                    <td  width="21%">
                        &nbsp;<asp:TextBox ID="txtCCLX_Year" runat="server"   Width="90%" CssClass="TextRead" MaxLength="25" ReadOnly="True"></asp:TextBox> 

                    </td>
                    <td align="center" class="smalltitle" style="width:12%;"  >
                           非齿侧轮芯制造年代                    </td>
                    <td width="21%">
                            &nbsp;<asp:TextBox ID="txtFCCLX_Year" runat="server"   Width="90%" CssClass="TextRead" MaxLength="25" ReadOnly="True"></asp:TextBox> 

                    </td>
                    <td align="center" class="smalltitle" style="width:12%;">
                        齿侧轮芯生产厂家</td>
                    <td width="21%">
                        &nbsp;<asp:DropDownList ID="ddlCCLX_Factory" runat="server" Enabled="false" Width="89%"></asp:DropDownList>

                    </td>
                </tr>
                 <tr>
                    <td align="center" class="smalltitle" style="width:12%;"  >
                            非齿侧轮芯生产厂家
                    </td>
                    <td  width="21%">
                        &nbsp;<asp:DropDownList ID="ddlFCCLX_Factory" runat="server" Enabled="false" Width="89%"></asp:DropDownList>

                    </td>
                    <td align="center" class="smalltitle" style="width:12%;"  >
                           齿侧轮箍标记 
                    </td>
                    <td width="21%">
                            &nbsp;<asp:TextBox ID="txtCCLG_Flag" runat="server"   Width="90%" CssClass="TextRead" MaxLength="25" ReadOnly="True"></asp:TextBox> 

                    </td>
                    <td align="center" class="smalltitle" style="width:12%;">
                        非齿侧轮箍标记</td>
                    <td width="21%">
                        &nbsp;<asp:TextBox ID="txtFCCLG_Flag" runat="server"   Width="90%" CssClass="TextRead" MaxLength="25" ReadOnly="True"></asp:TextBox> 

                    </td>
                </tr>
                 <tr>
                    <td align="center" class="smalltitle" style="width:12%;"  >
                            &nbsp;轮对修程</td>
                    <td  width="21%">
                        &nbsp;<asp:TextBox ID="txtLDXC" runat="server"   Width="90%" CssClass="TextRead" MaxLength="50" ReadOnly="True"></asp:TextBox> 

                    </td>
                    <td align="center" class="smalltitle" style="width:12%;"  >
                           承修时间 
                    </td>
                    <td width="21%">
                           &nbsp;<asp:TextBox ID="txtCX_DateTime" runat="server"  CssClass="TextRead" Width="90%"  ReadOnly="True"></asp:TextBox>
                    </td>
                    <td align="center" class="smalltitle" style="width:12%;">
                        承修厂家</td>
                    <td width="21%">
                        &nbsp;<asp:DropDownList ID="ddlCX_Factory" runat="server" Enabled="false" Width="89%"></asp:DropDownList>

                    </td>
                </tr>
                 <tr>
                  
                   <td align="center"  class="smalltitle" style="width:12%;">
                       存放库区</td>
                  <td width="21%">
                      &nbsp;<asp:DropDownList ID="ddlAreaCode" runat="server" Enabled="false" Width="89%"></asp:DropDownList>

                  </td>
                 
                   <td align="center"  class="smalltitle" style="width:12%;">
                       入库时间</td>
                  <td width="21%">
                      &nbsp;<asp:TextBox ID="txtInstockDate" runat="server"  CssClass="TextRead" Width="90%" ReadOnly="True" ></asp:TextBox> 
                  </td>
                  <td align="center"  class="smalltitle" style="width:12%;">
                        建单人员
                  </td> 
                  <td>
                    &nbsp;<asp:TextBox ID="txtCreator" runat="server"  CssClass="TextRead" Width="90%" ReadOnly="True" ></asp:TextBox> 
                  </td>
                </tr>
                <tr>
                   <td align="center" class="smalltitle" style="width:12%;">
                        建单日期
                  </td> 
                  <td>
                    &nbsp;<asp:TextBox ID="txtCreatDate" runat="server"  CssClass="TextRead" Width="90%" ReadOnly="True" ></asp:TextBox> 
                  </td>
                  <td align="center"  class="smalltitle" style="width:12%;">
                        修改人员
                  </td> 
                  <td>
                     &nbsp;<asp:TextBox ID="txtUpdater" runat="server"  CssClass="TextRead" Width="90%"  ReadOnly="True"></asp:TextBox> 
                  </td>
                  <td align="center"  class="smalltitle" style="width:12%;">
                        修改日期
                  </td> 
                  <td>
                    &nbsp;<asp:TextBox ID="txtUpdateDate" runat="server"  CssClass="TextRead" Width="90%" ReadOnly="True" ></asp:TextBox> 
                  </td>
                  
                </tr>	
                <tr>
                    <td align="center"  class="smalltitle" style="width:12%;">
                        备注
                    </td>
                    <td colspan="5">
                        &nbsp;<asp:TextBox ID="txtMemo" runat="server" CssClass="TextRead" TextMode="MultiLine" Height="72px" Width="97%" ReadOnly="True"></asp:TextBox>
                    </td>
                </tr>
                	
		</table>
                
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
