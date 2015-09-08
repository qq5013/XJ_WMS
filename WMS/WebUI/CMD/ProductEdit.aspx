<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductEdit.aspx.cs" Inherits="WMS.WebUI.CMD.ProductEdit" %>
<%@ Register src="../../UserControl/Calendar.ascx" tagname="Calendar" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
        <link href="~/Css/Main.css" type="text/css" rel="stylesheet" /> 
        <link href="~/Css/op.css" type="text/css" rel="stylesheet" /> 
        <script type="text/javascript" src="../../JQuery/jquery-2.1.3.min.js"></script>
        <script type="text/javascript" src= "../../JScript/Common.js"></script>
         <script type="text/javascript" src= "../../JScript/DataProcess.js"></script>
        <script type="text/javascript">
            $(document).ready(function () {
                $("#ddlProductTypeCode").bind("change", function () {
                    getWhereBaseData('CMD_ProductType', 'ddlAreaCode', 'AreaCode', "ProductTypeCode='" + $("#ddlProductTypeCode").val() + "'");
                });
            });

            function Save() {
                if (trim($("#txtID").val()) == "") {
                    alert("产品编码不能为空!");
                    $("#txtID").focus();
                    return false;
                }
                if (trim($("#txtProductName").val()) == "") {
                    alert("产品名称不能为空!");
                    $("#txtProductName").focus();
                    return false;
                }
                if (trim($("#ddlProductTypeCode").val()) == "") {
                    alert("产品类型不能为空!");
                    $("#ddlProductTypeCode").focus();
                    return false;
                }
                if (trim($("#ddlTrainTypeCode").val()) == "") {
                    alert("车型不能为空!");
                    $("#ddlTrainTypeCode").focus();
                    return false;
                }
                if (trim($("#txtAxieNo").val()) == "") {
                    alert("车轴编号不能为空!");
                    $("#txtAxieNo").focus();
                    return false;
                }
                 
                return true;
            }
           
        </script>
    <style type="text/css">
        .laydate-icon
        {}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table style="width: 100%; height: 20px;" class="maintable">
            <tr>
                <td align="right">
                    <asp:Button ID="btnCancel" runat="server" Text="放弃" 
                        OnClientClick="return Cancel();" CssClass="ButtonCancel" />
                    <asp:Button ID="btnSave" runat="server" Text="保存" OnClientClick="return Save()" 
                        CssClass="ButtonSave" onclick="btnSave_Click"  />
                    <asp:Button ID="btnExit" runat="server" Text="离开" OnClientClick="return Exit();" 
                        CssClass="ButtonExit" />
                </td>
            </tr>
        </table>
        <table id="Table1" class="maintable"  width="100%" align="center" 
				 runat="server">			
				<tr>
                    <td align="center" class="musttitle" style="width:12%;"  >
                            产品编码
                    </td>
                    <td  width="21%">
                            &nbsp;<asp:TextBox 
                                ID="txtID" runat="server"  CssClass="TextBox" Width="90%" 
                                MaxLength="20"  ></asp:TextBox>
                    </td>
                    <td align="center" class="musttitle" style="width:12%;"  >
                           产品名称
                    </td>
                    <td width="21%">
                            &nbsp;<asp:TextBox 
                                ID="txtProductName" runat="server"  CssClass="TextBox" Width="90%" 
                                MaxLength="50" ></asp:TextBox> 
                          

                    </td>
                    <td align="center" class="musttitle" style="width:12%;">
                        产品类别</td>
                    <td width="21%">
                        &nbsp;<asp:DropDownList 
                            ID="ddlProductTypeCode" runat="server" Width="89%">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td align="center" class="musttitle" style="width:12%;"  >
                            车型
                    </td>
                    <td  width="21%">
                         &nbsp;<asp:DropDownList ID="ddlTrainTypeCode" runat="server" Width="89%">
                        </asp:DropDownList>
                    </td>
                    <td align="center" class="musttitle" style="width:12%;"  >
                           车轴编号 
                    </td>
                    <td width="21%">
                            &nbsp;<asp:TextBox ID="txtAxieNo" runat="server"   Width="90%" 
                                MaxLength="20" CssClass="TextBox" ></asp:TextBox> 

                    </td>
                    <td align="center" class="smalltitle" style="width:12%;">
                        轮径</td>
                    <td width="21%">
                        &nbsp;<asp:TextBox ID="txtWheelDiameter" runat="server" 
                                  Width="90%" CssClass="TextBox" MaxLength="25" ></asp:TextBox> 

                    </td>
                </tr>
                <tr>
                    <td align="center" class="smalltitle" style="width:12%;"  >
                            齿侧轴径
                    </td>
                    <td  width="21%">
                        &nbsp;<asp:TextBox ID="txtCCZ_Diameter" runat="server" 
                                  Width="90%" CssClass="TextBox"
                                MaxLength="25" ></asp:TextBox> 

                    </td>
                    <td align="center" class="smalltitle" style="width:12%;"  >
                           非齿侧轴径 
                    </td>
                    <td width="21%">
                            &nbsp;<asp:TextBox ID="txtFCCZ_Diameter" runat="server" 
                                  Width="90%" CssClass="TextBox"
                                MaxLength="25" ></asp:TextBox> 

                    </td>
                    <td align="center" class="smalltitle" style="width:12%;">
                        齿侧端轴直径</td>
                    <td width="21%">
                        &nbsp;<asp:TextBox ID="txtCCD_Diameter" runat="server" 
                                  Width="90%" CssClass="TextBox"
                                MaxLength="25" ></asp:TextBox> 

                    </td>
                </tr>
                 <tr>
                    <td align="center" class="smalltitle" style="width:12%;"  >
                            非齿侧端轴直径
                    </td>
                    <td  width="21%">
                        &nbsp;<asp:TextBox ID="txtFCCD_Diameter" runat="server" 
                                  Width="90%" CssClass="TextBox"
                                MaxLength="25" ></asp:TextBox> 

                    </td>
                    <td align="center" class="smalltitle" style="width:12%;"  >
                           齿侧选配抱轴瓦尺寸</td>
                    <td width="21%">
                            &nbsp;<asp:TextBox ID="txtCCXPBZW_Size" runat="server" 
                                  Width="90%" CssClass="TextBox"
                                MaxLength="25" ></asp:TextBox> 

                    </td>
                    <td align="center" class="smalltitle" style="width:12%;">
                        非齿侧选配抱轴瓦尺寸</td>
                    <td width="21%">
                        &nbsp;<asp:TextBox ID="txtFCCXPBZW_Size" runat="server" 
                                  Width="90%" CssClass="TextBox"
                                MaxLength="25" ></asp:TextBox> 

                    </td>
                </tr>
                 <tr>
                    <td align="center" class="smalltitle" style="width:12%;"  >
                            齿轮编号</td>
                    <td  width="21%">
                        &nbsp;<asp:TextBox ID="txtGearNo" runat="server" 
                                  Width="90%" CssClass="TextBox"
                                MaxLength="20" ></asp:TextBox> 

                    </td>
                    <td align="center" class="smalltitle" style="width:12%;"  >
                           齿侧轮芯标记 
                    </td>
                    <td width="21%">
                            &nbsp;<asp:TextBox ID="txtCCLX_Flag" runat="server" 
                                  Width="90%" CssClass="TextBox"
                                MaxLength="25" ></asp:TextBox> 

                    </td>
                    <td align="center" class="smalltitle" style="width:12%;">
                        非齿侧轮芯标记</td>
                    <td width="21%">
                        &nbsp;<asp:TextBox ID="txtFCCLX_Flag" runat="server" 
                                  Width="90%" CssClass="TextBox"
                                MaxLength="25" ></asp:TextBox> 

                    </td>
                </tr>
                 <tr>
                    <td align="center" class="smalltitle" style="width:12%;"  >
                            齿侧轮芯制造年代
                    </td>
                    <td  width="21%">
                        &nbsp;<asp:TextBox ID="txtCCLX_Year" runat="server" 
                                  Width="90%" CssClass="TextBox" 
                                MaxLength="25" ></asp:TextBox> 

                    </td>
                    <td align="center" class="smalltitle" style="width:12%;"  >
                           非齿侧轮芯制造年代 
                    </td>
                    <td width="21%">
                            &nbsp;<asp:TextBox ID="txtFCCLX_Year" runat="server" 
                                  Width="90%" CssClass="TextBox"
                                MaxLength="25" ></asp:TextBox> 

                    </td>
                    <td align="center" class="smalltitle" style="width:12%;">
                        齿侧轮芯生产厂家</td>
                    <td width="21%">
                        &nbsp;<asp:DropDownList 
                            ID="ddlCCLX_Factory" runat="server" Width="89%">
                        </asp:DropDownList>

                    </td>
                </tr>
                 <tr>
                    <td align="center" class="smalltitle" style="width:12%;"  >
                            非齿侧轮芯生产厂家
                    </td>
                    <td  width="21%">
                        &nbsp;<asp:DropDownList 
                            ID="ddlFCCLX_Factory" runat="server" Width="89%">
                        </asp:DropDownList>

                    </td>
                    <td align="center" class="smalltitle" style="width:12%;"  >
                           齿侧轮箍标记 
                    </td>
                    <td width="21%">
                            &nbsp;<asp:TextBox ID="txtCCLG_Flag" runat="server" 
                                  Width="90%" CssClass="TextBox"
                                MaxLength="25" ></asp:TextBox> 

                    </td>
                    <td align="center" class="smalltitle" style="width:12%;">
                        非齿侧轮箍标记</td>
                    <td width="21%">
                        &nbsp;<asp:TextBox ID="txtFCCLG_Flag" runat="server" 
                                  Width="90%" CssClass="TextBox"
                                MaxLength="25" ></asp:TextBox> 

                    </td>
                </tr>
                 <tr>
                    <td align="center" class="smalltitle" style="width:12%;"  >
                            &nbsp;轮对修程</td>
                    <td  width="21%">
                        &nbsp;<asp:TextBox ID="txtLDXC" runat="server" 
                                  Width="90%" CssClass="TextBox"
                                MaxLength="50" ></asp:TextBox> 

                    </td>
                    <td align="center" class="smalltitle" style="width:12%;"  >
                           承修时间 
                    </td>
                    <td width="21%">
                            &nbsp;<uc1:Calendar ID="txtCX_DateTime" runat="server" />
                    </td>
                    <td align="center" class="smalltitle" style="width:12%;">
                        承修厂家</td>
                    <td width="21%">
                        &nbsp;<asp:DropDownList ID="ddlCX_Factory" runat="server" Width="89%">
                        </asp:DropDownList>

                    </td>
                </tr>
                 <tr>
                  
                   <td align="center"  class="smalltitle" style="width:12%;">
                       存放库区</td>
                  <td width="21%">
                      &nbsp;<asp:DropDownList 
                            ID="ddlAreaCode" runat="server" Width="89%">
                        </asp:DropDownList>

                  </td>
                 
                   <td align="center"  class="smalltitle" style="width:12%;">
                       入库时间</td>
                  <td width="21%">
                      &nbsp;<asp:TextBox ID="txtInstockDate" runat="server"  
                          CssClass="TextRead" Width="90%"  ></asp:TextBox> 
                  </td>
                  <td align="center"  class="smalltitle" style="width:12%;">
                        建单人员
                  </td> 
                  <td>
                    &nbsp;<asp:TextBox ID="txtCreator" runat="server"  
                          CssClass="TextRead" Width="90%"  ></asp:TextBox> 
                  </td>
                </tr>
                <tr>
                   <td align="center" class="smalltitle" style="width:12%;">
                        建单日期
                  </td> 
                  <td>
                    &nbsp;<asp:TextBox ID="txtCreatDate" runat="server"  
                          CssClass="TextRead" Width="90%"  ></asp:TextBox> 
                  </td>
                  <td align="center"  class="smalltitle" style="width:12%;">
                        修改人员
                  </td> 
                  <td>
                     &nbsp;<asp:TextBox ID="txtUpdater" runat="server"  
                          CssClass="TextRead" Width="90%"  ></asp:TextBox> 
                  </td>
                  <td align="center"  class="smalltitle" style="width:12%;">
                        修改日期
                  </td> 
                  <td>
                    &nbsp;<asp:TextBox ID="txtUpdateDate" runat="server"  
                          CssClass="TextRead" Width="90%"  ></asp:TextBox> 
                  </td>
                  
                </tr>	
                <tr>
                    <td align="center"  class="smalltitle" style="width:12%;">
                        备注
                    </td>
                    <td colspan="5">
                        &nbsp;<asp:TextBox ID="txtMemo" runat="server" CssClass="MultiLineTextBox" 
                            TextMode="MultiLine" Height="72px" Width="97%"></asp:TextBox>
                    </td>
                </tr>
                	
		</table>
    </div>
    </form>
</body>
</html>
