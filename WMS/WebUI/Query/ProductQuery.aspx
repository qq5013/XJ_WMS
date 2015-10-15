﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductQuery.aspx.cs" Inherits="WMS.WebUI.Query.ProductQuery" %>
<%@ Register Assembly="FastReport.Web" Namespace="FastReport.Web" TagPrefix="cc1" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title> 
        <link href="~/Css/Main.css" type="text/css" rel="stylesheet" /> 
        <link href="~/Css/op.css" type="text/css" rel="stylesheet" /> 
   
        <script type="text/javascript" src='<%=ResolveClientUrl("~/JQuery/jquery-2.1.3.min.js") %>'></script>
        <script type="text/javascript" src='<%=ResolveClientUrl("~/JScript/Common.js") %>'></script>
       <script type="text/javascript" src='<%=ResolveClientUrl("~/JScript/DataProcess.js") %>'></script>
       <script type="text/javascript">
           $(document).ready(function () {
               $(window).resize(function () {
                   resize();
               });
               BindEvent();
           });
           function resize() {
               var h = document.documentElement.clientHeight - 30;
               $("#rptview").css("height", h);
           }
           function ProductClick() {
               var where = "1=1";
               if ($("#ddlProductType").val() != "") {
                   where += escape(" and ProductTypeCode='" + $('#ddlProductType').val() + "'");
               }
               if ($("#ddlStateNo").val() != "") {
                   where += escape(" and StateNo='" + $('#ddlStateNo').val() + "'");
               }
               getMultiItems("CMD_ProductInStock", "ProductCode", $('#btnProduct'), '#HdnProduct', where);
               return false;
           }
           function BindEvent() {
               $("#txtProductCode").bind("dblclick", function () {
                   var where = "1=1";
                   if ($("#ddlProductType").val() != "") {
                       where += escape(" and ProductTypeCode='" + $('#ddlProductType').val() + "'");
                   }
                   if ($("#ddlStateNo").val() != "") {
                       where += escape(" and StateNo='" + $('#ddlStateNo').val() + "'");
                   }

                   GetOtherValue("CMD_ProductInStock", "txtProductName,txtProductCode", "ProductName,ProductCode", where);
                   return false;
               });
               $("#txtProductCode").bind("change", function () {
                   var where = "1=1";
                   if ($("#ddlProductType").val() != "") {
                       where += escape("ProductTypeCode='" + $('#ddlProductType').val() + "'");
                   }
                   if ($("#ddlStateNo").val() != "") {
                       where += escape(" and StateNo='" + $('#ddlStateNo').val() + "'");
                   }
                   where += " and ProductCode='" + $('#txtProductCode').val() + "'";
                   getWhereBaseData('CMD_ProductInStock', "txtProductName,txtProductCode", "ProductName,ProductCode", where);
               });
           }


        function PrintClick() {
            $('#HdnWH').val(document.body.clientWidth + "#" + document.body.clientHeight);
            return true;
        }
        </script>
   
    </head>
<body  style="overflow:hidden;">
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
    <table  style="width:100%;height:100%;" >
        <tr runat ="server" id = "rptform" valign="top">
            <td align="left" style="width:100%; height:30px;" >
                <table class="maintable"  width="100%" align="center" >
                    <tr>
                         <td   align="center" class="musttitle" style="width:6%;">
                            产品类型 
                        </td>
                        <td align="left"   style="width:15%;" >
                            <asp:DropDownList ID="ddlProductType" runat="server" Width="90%">
                            </asp:DropDownList>
                        </td>
                         <td   align="center" class="musttitle" style="width:6%;">
                            产品状态 
                        </td>
                        <td align="left"   style="width:10%;" >
                            <asp:DropDownList ID="ddlStateNo" runat="server" Width="90%">
                            </asp:DropDownList>
                        </td>
                        <td   align="center" class="musttitle" style="width:5%;">
                            产品
                        </td>
                        <td align="left"   style="width:15%;" >
                         <asp:textbox id="txtProductCode"   runat="server"  Width="30%" CssClass="TextBox" ></asp:textbox>
                         <asp:textbox id="txtProductName" tabIndex="1" runat="server" Width="65%" CssClass="TextRead"></asp:textbox>
                        </td>
                        <td  align="left" style="width:5%;">
                               <asp:Button ID="btnProduct" runat="server" OnClientClick="return ProductClick();" CssClass="ButtonOption" Text="指定" Width="70px" Height="23px" />
                        </td>
                        <td class="musttitle" align="center" style="width:5%;" >
                            报表
                        </td>
                         <td width="15%" align="left">
                             <asp:RadioButton ID="rpt1" runat="server" Checked="True" GroupName="Rpt" Text="明细表" />&nbsp;
                             <asp:RadioButton ID="rpt2" runat="server" GroupName="Rpt" Text="统计表" />&nbsp;               
                        </td>
                        <td align= "left" style="border-left:2px solid #ffffff;">
                             &nbsp;<asp:Button ID="btnSearch" runat="server" CssClass="ButtonQuery" 
                                 onclick="btnPreview_Click" tabIndex="2" Text="查询" Width="58px" 
                                onclientclick="return PrintClick();" /> &nbsp;&nbsp;
                             <asp:Button ID="btnRefresh" runat="server" CssClass="ButtonRefresh" 
                                 OnClientClick="return Refresh()" tabIndex="2" 
                                 Text="刷新" Width="58px" />
                        </td>  
                                                                     
                    </tr>
                </table>  
            </td>
        </tr>
        <tr>
            <td runat ="server" id = "rptview" valign="top" align="left">
                <table style="width:90%;height:100%;">
                    <tr>
                        <td >           
                            <cc1:WebReport ID="WebReport1" runat="server" BackColor="White" ButtonsPath="images\buttons1"
                                Font-Bold="False" Height = "100%" OnStartReport="WebReport1_StartReport"
                                ToolbarColor="Lavender" Width="100%" Zoom="1" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
         
        <input id="HdnProduct" type="hidden" runat="server" />
      
       <input id="HdnWH" type="hidden" runat="server" value="0#0" />
        </ContentTemplate>
    </asp:UpdatePanel> 
   </form>
</body>
</html>