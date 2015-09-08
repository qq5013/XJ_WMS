﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExceptionLog.aspx.cs" Inherits="WMS.WebUI.SysInfo.SystemLog.ExceptionLog" %>
<%@ Register src="../../../UserControl/Calendar.ascx" tagname="Calendar" tagprefix="uc1" %>

<%@ Register Assembly="AspNetPager" Namespace="AspNetPager" TagPrefix="NetPager" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>系统异常日志</title>
    <script type="text/javascript" src="../../../JQuery/jquery-2.1.3.min.js"></script>
    <script type="text/javascript" src="../../../JScript/Check.js?t=00"></script>
    <script type="text/javascript" src="../../../JScript/Common.js"></script>
    <link href="../../../css/main.css" rel="Stylesheet" type="text/css" />
    <link href="../../../css/op.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript">
        $(document).ready(function () {
            $(window).resize(function () {
                resize();
            });
        });
        function resize() {
            var h = document.documentElement.clientHeight - 60;
            $("#pnlMain").css("height", h);
        }
        function DelConfirm(btnID) {
            var table = document.getElementById('gvMain');
            var hasChecked = false;
            for (var i = 1; i < table.rows.length; i++) {
                var cell = table.rows[i].cells[0];
                var chk = cell.getElementsByTagName("INPUT");
                if (chk[0].checked) {
                    hasChecked = true;
                    break;
                }
            }

            if (!hasChecked) {
                alert('请选择要删除的数据！');
                return false;
            }
            if (confirm('确定要删除选择的数据？', '删除提示')) {
                var btn = document.getElementById(btnID);
                btn.click();
                //window.location.reload();
            }
            else {
                return false;
            }
        }

        function ClearConfirm() {
            if (confirm('确定要清空所有日志记录', '清空提示')) {

            }
            else {
                return false;
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" Visible="true">
        <ContentTemplate>
            <!--数据显示-->
            <asp:Panel ID="pnlList" runat="server" Height="480px" Width="100%">
                <!--工具栏-->
             
                 <asp:Panel ID="pnlListToolbar" runat="server" Height="30px" Width="100%">
                    <table style="width: 100%; height: 30px;" class="maintable">
                        <tr>
                            <td style="width:60%;">
                                 <asp:DropDownList ID="ddl_Field" runat="server">
                                    <asp:ListItem Selected="True" Value="ModuleName">发生模块</asp:ListItem>
                                </asp:DropDownList>
                                <asp:TextBox ID="txtKeyWords" runat="server" CssClass="TextBox" Width="120"></asp:TextBox>
                                时间从
                                <uc1:Calendar ID="txtDateStart" runat="server" />
                                
                                至
                                <uc1:Calendar ID="txtDateEnd" runat="server" />
                                <asp:RadioButton GroupName="order" ID="rbASC" runat="server" Text="升" Checked="True" />
                                <asp:RadioButton GroupName="order" ID="rbDESC" runat="server" Text="降" />
                            </td>
                           
                            <td>
                                <asp:Button ID="btnQuery" runat="server" Text="查询" OnClick="btnQuery_Click" CssClass="ButtonQuery" />
                                <asp:Button ID="btnDelete" runat="server" Text="删除" CssClass="ButtonDel" OnClick="btnDelete_Click"
                                    OnClientClick="return DelConfirm('btnDelete')" Enabled="False" />
                                <asp:Button ID="btnDeleteAll" runat="server" Text="清空" CssClass="ButtonClearAll"
                                    OnClientClick="return ClearConfirm();" Enabled="False" OnClick="btnDeleteAll_Click" />
                                <asp:Button ID="btnExit" runat="server" Text="退出" OnClientClick="Exit();" CssClass="ButtonExit" />
                            </td>
                         
                        </tr>
                         
                    </table>
                   
                </asp:Panel>
                <!--数据-->
                <asp:Panel ID="pnlMain" runat="server" Height="480px" Style="overflow: auto;" Width="100%">
                    <asp:GridView ID="gvMain" runat="server"  Width="900px"  OnRowDataBound="gvMain_RowDataBound"
                          SkinID="GridViewSkin"  AutoGenerateColumns="False">
                        <RowStyle BackColor="WhiteSmoke" Height="28px" />
                        <HeaderStyle CssClass="GridHeader" />
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:TemplateField HeaderText="操作">
                                <HeaderStyle Width="30px" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="ModuleName" HeaderText="发生模块">
                                <HeaderStyle Width="120px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CatchTime" HeaderText="发生时间">
                                <HeaderStyle Width="120px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="FunctionName" HeaderText="功能名称">
                                <HeaderStyle Width="150px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ExceptionalType" HeaderText="异常类型">
                                <HeaderStyle Width="150px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ExceptionalDescription" HeaderText="异常描述"></asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
                <!--分页导航-->
                <asp:Panel ID="pnlNavigator" runat="server" Height="30px" Style="left: 0px; position: relative;
                    top: 0px" Width="100%">
                    <table id="paging" cellpadding="0" cellspacing="0" style="width: 500px;">
                        <tr>
                            <td>
                                <NetPager:AspNetPager ID="pager" runat="server" OnPageChanging="pager_PageChanging"
                                    ShowPageIndex="false" ShowInputBox="Always" AlwaysShow="true">
                                </NetPager:AspNetPager>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!--隐藏数据-->
    <div>
    </div>
    </form>
 

</body>
</html>
