<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductQuery.aspx.cs" Inherits="WMS.WebUI.Query.ProductQuery" %>
<%@ Register src="../../UserControl/Calendar.ascx" tagname="Calendar" tagprefix="uc1" %>
<%@ Register Assembly="FastReport.Web" Namespace="FastReport.Web" TagPrefix="cc1" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title> 
        <link href="~/Css/Main.css" type="text/css" rel="stylesheet" /> 
        <link href="~/Css/op.css" type="text/css" rel="stylesheet" /> 
        <link href="~/Css/Detail.css" type="text/css" rel="stylesheet" /> 
        
        <script type="text/javascript" src='<%=ResolveClientUrl("~/JQuery/jquery-1.8.3.min.js") %>'></script>
        <script type="text/javascript" src='<%=ResolveClientUrl("~/JScript/Common.js") %>'></script>
       <script type="text/javascript" src='<%=ResolveClientUrl("~/JScript/DataProcess.js") %>'></script>
       <script type="text/javascript" src='<%=ResolveClientUrl("~/JScript/JsDDL.js") %>'></script>
       <script type="text/javascript">
       $(document).ready(function () {
            BindEvent();
        });
        function BindEvent() {
            $("#btnProduct").bind("click", function () {
                var where = "1=1";
                if ($("#ddlProductClass").val() != "请选择") {
                    where = escape("PRODUCT_Class='" + $('#ddlProductClass').val() + "'");
                }

                getMultiItems("CMD_Product", "PRODUCT_CODE", this, '#HdnProduct',where);
                return false;
            });
            $("#txtProductModule").bind("dblclick", function () {
                var where = "1=1";
                if ($("#ddlProductClass").val() != "请选择") {
                    where = escape("PRODUCT_Class='" + $('#ddlProductClass').val() + "'");
                }
                GetOtherValueNullClear("CMD_Product", "txtProductModule,txtProductID", "PRODUCT_MODEL,PRODUCT_CODE", where);
                return false;
            });

            $("#btnFProduct").bind("click", function () {
                var ProductID = "";
                if ($("#btnProduct").val() == "指定") {
                    if ($('#txtProductID').val() == "") {
                        alert("请先选择产品型号！");
                        return false;
                    }
                    ProductID = "'" + $('#txtProductID').val() + "'";
                }
                else {
                    ProductID = $('#HdnProduct').val();
                }
                getMultiItems("CMD_Product", "PRODUCT_CODE", this, '#HdnFProduct', "PRODUCT_CODE in (" + ProductID + ")");
                return false;
            });

            $("#txtProductFModule").bind("dblclick", function () {
                var ProductID = "";
                if ($("#btnProduct").val() == "指定") {
                    if ($('#txtProductID').val() == "") {
                        alert("请先选择产品型号！");
                        return false;
                    }
                    ProductID = "'" + $('#txtProductID').val() + "'";
                }
                else {
                    ProductID = $('#HdnProduct').val();
                }
                GetOtherValueNullClear("CMD_Product", "txtProductFModule,txtFProductID", "PRODUCT_FMODEL,PRODUCT_CODE", "PRODUCT_CODE in (" + ProductID + ")");
                return false;
            });

            //规格
            $("#btnColor").bind("click", function () {
                var ProductID = "";
                if ($("#btnProduct").val() == "指定") {
                    if ($('#txtProductID').val() == "") {
                        alert("请先选择产品型号！");
                        return false;
                    }
                    ProductID = "'" + $('#txtProductID').val() + "'";
                }
                else {
                    ProductID = $('#HdnProduct').val();
                }

                getMultiItems("CMD_COLOR", "COLOR_CODE", this, '#hdnColor', "PRODUCT_CODE in (" + ProductID + ")");
                return false;
            });
            $("#txtColor").bind("dblclick", function () {
                var ProductID = "";
                if ($("#btnProduct").val() == "指定") {
                    if ($('#txtProductID').val() == "") {
                        alert("请先选择产品型号！");
                        return false;
                    }
                    ProductID = "'" + $('#txtProductID').val() + "'";
                }
                else {
                    ProductID = $('#HdnProduct').val();
                }
                GetOtherValueNullClear('CMD_COLOR', "txtColorID,txtColor", "COLOR_CODE,COLOR_NAME", "PRODUCT_CODE in (" + ProductID + ")");
                return false;
            });
        }


        function PrintClick() {


            if ($("#txtStartDate").val() == "") {
                alert("日期不能为空！");
                $("#txtStartDate").focus();
                return false;
            }
            if ($("#txtEndDate").val() == "") {
                alert("日期不能为空！");
                $("#txtEndDate").focus();
                return false;
            }
            $('#HdnWH').val(document.body.clientWidth + "#" + document.body.clientHeight);
            return true;
        }
        </script>
    <style type="text/css">
        .style1
        {
            width: 100%;
            height: 33px;
        }
    </style>
    </head>
<body  style="overflow:hidden;">
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
    <table    style="width:100%;height:100%;" >
        <tr>
            <td align="left"  class="maintable" style="height:22px"  >
                列印
            </td>   
        </tr>
        <tr runat ="server" id = "rptform" valign="top">
            <td align="left" class="style1"  >
                <table class="maintable"  width="100%" align="center"   >
                    <tr>
                        <td class="smalltitle" align="right" style="height: 24px; width: 5%;">
                            日期区间
                        </td>
                        <td align="left" style="height: 24px; width: 7%">
                            &nbsp;<uc1:Calendar ID="txtStartDate" runat="server"/>
                        </td>
                            <td class="smalltitle" align="center" style="height: 24px; width: 2%;">
                            To
                            </td>
                            <td style="height: 24px; width: 7%;">
                            &nbsp;<uc1:Calendar ID="txtEndDate" runat="server"    />
                                                        
                        </td>
                         <td class="smalltitle" align="right" style="height: 24px; width:5%">
                            产品类型 
                        </td>
                        <td style="height: 24px; width:6%" >
                            <asp:DropDownList ID="ddlProductClass" runat="server" Width="90%">
                            </asp:DropDownList>
                        </td>
                        <td  class="smalltitle" align="right" style="height: 24px; width:5%">
                           产品型号 <asp:textbox id="txtProductID"   runat="server" Width="0px" CssClass="TextBox" heigth="0px" style="display:none" ></asp:textbox>
                        </td>
                        <td align="left" style="height: 24px;width:6% ">
                          &nbsp;<asp:textbox id="txtProductModule" tabIndex="1" runat="server" Width="92%" CssClass="TextBox" heigth="16px"  AutoCompleteType="Disabled"></asp:textbox>
                        </td>
                        <td align="left"  style="height: 24px;width:6% "  >
                               <asp:Button ID="btnProduct" runat="server" CssClass="but" Text="指定" Width="70px" Height="23px" />
                        </td>

                        <td   class="smalltitle" align="right" style="height: 24px; width:6%; display:none;">
                           工厂型号 <asp:textbox id="txtFProductID"   runat="server" Width="0px" CssClass="TextBox" heigth="0px" style="display:none" ></asp:textbox>
                        </td>
                        <td align="left" style="height: 24px;width:6%; display:none;">
                          &nbsp;<asp:textbox id="txtProductFModule" tabIndex="1" runat="server" Width="92%" CssClass="TextBox" heigth="16px" AutoCompleteType="Disabled"></asp:textbox>
                        </td>
                        <td align="left"  style="height: 24px;width:6%; display:none;"  >
                               <asp:Button ID="btnFProduct" runat="server" CssClass="but" Text="指定" Width="70px" Height="23px" />
                        </td>

                        <td class="smalltitle" align="right" style="height: 24px; width:4%">
                           规格<asp:textbox id="txtColorID" tabIndex="1" runat="server" Width="0px" CssClass="TextBox" heigth="0px" style="display:none"></asp:textbox>
                        </td>
                        <td align="left" style="height: 24px; width:6%">
                           &nbsp;<asp:textbox id="txtColor" tabIndex="1" runat="server" Width="93%" 
                                CssClass="TextBox" heigth="16px" AutoCompleteType="Disabled"></asp:textbox>
                        </td>
                        <td align="left" style="height: 24px; width:6%" >
                            <asp:Button ID="btnColor" runat="server" CssClass="but" Text="指定" Width="70px" Height="23px" />
                        </td>         
                        
                        <td    width="12%" align= "center"   >
                            <asp:Button ID="btnSearch" runat="server" CssClass="ButtonQuery" 
                                 onclick="btnPreview_Click" tabIndex="2" Text="查询" Width="58px" 
                                onclientclick="return PrintClick();" /> &nbsp;&nbsp;
                             <asp:Button ID="btnRefresh" runat="server" CssClass="ButtonRefresh" 
                                 OnClientClick="return Refresh()" tabIndex="2" 
                                 Text="刷新" Width="58px" />
                        </td>  
                        <td  width="1%" align="center">
                        </td>                                            
                    </tr>
                </table>  
                  
            </td>
        </tr>
        <tr>
            <td runat ="server" id = "rptview" valign="top" align="left">
                <table style="width:90%;height:100%;">
                    <tr>
                        <td  >           
                            <cc1:WebReport ID="WebReport1" runat="server" BackColor="White" ButtonsPath="images\buttons1"
                                Font-Bold="False" Height = "100%" OnStartReport="WebReport1_StartReport"
                                ToolbarColor="Lavender" Width="100%" Zoom="1" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
        <input id="HdnFProduct" type="hidden" runat="server" />
        <input id="HdnProduct" type="hidden" runat="server" />
      <input id="hdnColor" type="hidden" runat="server" />
       <input id="HdnWH" type="hidden" runat="server" value="0#0" />
        </ContentTemplate>
    </asp:UpdatePanel> 
   </form>
</body>
</html>