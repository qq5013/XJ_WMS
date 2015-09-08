<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CellQuery.aspx.cs" Inherits="WMS.WebUI.Query.CellQuery" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>仓库资料设置</title>
    <script type="text/javascript" src="../../JScript/Check.js?t=00"></script>
    <script type="text/javascript" src="../../JScript/Common.js"></script>
    <link href="../../css/css.css?t=98" rel="Stylesheet" type="text/css" />
    <link href="../../css/op.css" rel="Stylesheet" type="text/css" /> 
    <link href="~/Css/Main.css" type="text/css" rel="stylesheet" /> 
    <link href="~/Css/Detail.css" type="text/css" rel="stylesheet" />
    <link href="../../css/zTreeStyle/zTreeStyle.css" type="text/css" rel="stylesheet"/>

  <script type="text/javascript" src='<%=ResolveClientUrl("~/JQuery/jquery-1.8.3.min.js") %>'></script>
  <script type="text/javascript" src='<%=ResolveClientUrl("~/JScript/Common.js") %>'></script>
  <script type="text/javascript" src='<%=ResolveClientUrl("~/JScript/DataProcess.js") %>'></script>
  <script type="text/javascript" src='<%=ResolveClientUrl("~/JQuery/jquery.ztree.core-3.5.js") %>'></script>
     
    
    <script type="text/javascript">
        function SetTree() {
            zNodes = eval($('#hidetree').val()); //.replace('[','').replace(']','')
            $.fn.zTree.init($("#treeDemo"), setting, zNodes);
            var zTree = $.fn.zTree.getZTreeObj("treeDemo");
            var tNodeID = zTree.getNodes(0)[0];
            tNodeID.iconSkin = "../../images/leftmenu/in_warehouse.gif";
            zTree.updateNode(tNodeID);
            zTree.expandNode(tNodeID, true, null, null, false);
            var ShelfWhere = $("#hdnShelfWhere").val();
           

            document.getElementById("lblCurrentNode").innerHTML = zTree.getNodes(0)[0].children[0].name;
            $("#frame").attr("src", "Cell.aspx?ShelfCode=&AreaCode=" + zTree.getNodes(0)[0].children[0].id + "&ShelfWhere=" + ShelfWhere);

        }
        function BindEvent() {
            $('#btnUpdateSelected').bind('click', function () {
                return false;
            });

            $("#btnProduct").bind("click", function () {
                getMultiItems("CMD_Product", "PRODUCT_CODE", this, '#HdnProduct');
                return false;
            });
            $("#txtProductModule").bind("dblclick", function () {
                GetOtherValueNullClear("CMD_Product", "txtProductModule,txtProductID", "PRODUCT_MODEL,PRODUCT_CODE", "1=1");
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

        function treeview_resize() {

            var h = 300;
            if ($(window).height() <= 0) {
                h = document.body.clientHeight - $("#toptable")[0].offsetHeight - 40;
            }
            else {
                h = document.body.clientHeight - $("#toptable")[0].offsetHeight - 40;
            }

            $("#div_tree").css("height", h);
            $("#ShowPic").css("height", h);
            $("#frame").css("height", h -5);
             
        }

        var setting = {
            view: {
                showIcon: showIconForTree
            },
            data: {
                simpleData: {
                    enable: true,
                    IdKey: "id",
                    pIdKey: "pid"
                }
            },
            callback: {
                onClick: onClick
            }
        };
        function onClick(event, treeId, treeNode, clickFlag) {
            var tNodeID = treeNode.id;
            var tNodeIDLen = tNodeID.length;
            var sShelfCode = "";
            var sAreaCode = "";
            var ShelfWhere = "";
            if (tNodeIDLen == 3) {
                //库区
                sAreaCode = tNodeID;
                ShelfWhere = $("#hdnShelfWhere").val();
            }
            if (tNodeIDLen == 6) {
                //货架
                sShelfCode = tNodeID;
            }
            document.getElementById("lblCurrentNode").innerHTML = treeNode.name;

            $("#frame").attr("src", "Cell.aspx?ShelfCode=" + sShelfCode + "&AreaCode=" + sAreaCode + "&ShelfWhere=" + ShelfWhere);
 
        };
        function showIconForTree(treeId, treeNode) {
            return !treeNode.isParent;
        };
        $(document).ready(function () {
            BindEvent();
            SetTree();
        });
    </script>
    <style type="text/css">
        .SideBar
        {
           background-image: url(../../images/bar_bg.gif);
           background-position:right;
           background-repeat:no-repeat;
        
           padding-top:5px;
           vertical-align:top; 
           width:214px; 
           padding-right:4px;
        }
        .topic
        {
           padding-top:10px;
        }
        .topic2
        {
           text-align:center; 
           padding-top:3px;
           height:25px; 
           width:72px; 
           background-image:url(../../images/topic.gif);
           background-repeat:no-repeat;
        }
    </style>
</head>
<body >
    <form id="form1" runat="server">
         <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
           <ContentTemplate>
              
        <table style="width:100%; background-color:WHITE;" cellpadding="0" cellspacing="0">
           <tr>
             <td  colspan="2" align="right">
                <table style="width:100%"  class="maintable"   width="100%" align="center"  >
                    <tr>
                        <td class="smalltitle" align="right" style="height: 24px; width:5%">
                             库区   
                        </td>
                        <td align="left" style="height: 24px;width:10% ">
                           &nbsp;<asp:DropDownList ID="ddlArea" runat="server" Height="28px" Width="90%">
                                <asp:ListItem Value="2">展览区</asp:ListItem>
                                <asp:ListItem Value="3">不良品区</asp:ListItem>
                          </asp:DropDownList> 
                            
                        </td>
                        <td class="smalltitle" align="right" style="height: 24px; width:5%">
                            状态
                        </td>
                        <td align="left" style="height: 24px;width:8% ">
                            <asp:DropDownList ID="ddlActive" runat="server" Height="28px" Width="90%">
                                 <asp:ListItem Value="2">请选择</asp:ListItem>
                                <asp:ListItem Value="1">启用</asp:ListItem>
                                <asp:ListItem Value="0">未启用</asp:ListItem>
                          </asp:DropDownList> 
                            
                        </td>
                         <td  class="smalltitle" align="right" style="height: 24px; width:5%">
                           产品型号 <asp:textbox id="txtProductID"   runat="server" Width="0px" CssClass="TextBox" heigth="0px" style="display:none" ></asp:textbox>
                        </td>
                        <td align="left" style="height: 24px;width:8% ">
                          &nbsp;<asp:textbox id="txtProductModule" tabIndex="1" runat="server" Width="92%" CssClass="TextBox" heigth="16px"  AutoCompleteType="Disabled"></asp:textbox>
                        </td>
                        <td align="left"  style="height: 24px;width:6% "  >
                               <asp:Button ID="btnProduct" runat="server" CssClass="but" Text="指定" Width="70px" Height="23px" />
                        </td>
                        <td class="smalltitle" align="right" style="height: 24px; width:5%">
                           规格<asp:textbox id="txtColorID" tabIndex="1" runat="server" Width="0px" CssClass="TextBox" heigth="0px" style="display:none"></asp:textbox>
                        </td>
                        <td align="left" style="height: 24px; width:8%">
                           &nbsp;<asp:textbox id="txtColor" tabIndex="1" runat="server" Width="93%" 
                                CssClass="TextBox" heigth="16px" AutoCompleteType="Disabled"></asp:textbox>
                        </td>
                        <td align="left" style="height: 24px; width:6%" >
                            <asp:Button ID="btnColor" runat="server" CssClass="but" Text="指定" Width="70px" Height="23px" />
                        </td>   
                        <td  align="left" style="height: 24px; width:24%">
                             <asp:Button ID="btnSearch" runat="server" CssClass="ButtonQuery" onclick="btnPreview_Click" tabIndex="2" Text="查询" Width="58px" /> &nbsp;&nbsp;&nbsp;&nbsp;
                             <asp:Button ID="btnRefresh" runat="server" CssClass="ButtonRefresh"  OnClientClick="return Refresh()" tabIndex="2" Text="刷新" Width="58px" /> &nbsp;&nbsp;&nbsp;&nbsp;
                           </td>
                        <td  align=  "right"> 
                            <asp:Button ID="btnCancel" Text="退出" runat="server" CssClass="ButtonExit" OnClientClick="Exit();"  />   
                        </td>
                    </tr>
                </table>
                
                              
             </td>
           </tr>
           <tr>
              <td class="SideBar">
                <div id="div_tree" style="overflow:auto; width:280px; height:500px;">
		            <ul id="treeDemo" class="ztree"></ul>
	            </div>

              </td>
          
           <td style="vertical-align:top; padding-left:10px;"> <!--编辑区-->
            <div  id="toptable" style="height:24px;vertical-align:middle; width:100% ">
               <img src="../../images/ico_home.gif" border="0"  />当前选中的节点：
               <asp:Label ID="lblCurrentNode" runat="server" ForeColor="#404040"></asp:Label>
            </div>
            <div id="ShowPic" style="width:100%; height:500px;">
               <iframe id="frame" runat="server" src="" style="width:100%; height:480px;" bordercolor="white" frameborder="no"></iframe>
            </div>
          </td>
       </tr>
        </table>
    

     <div>
        <asp:HiddenField ID="hidetree" runat="server" />
        <input id="HdnProduct" type="hidden" runat="server" />
        <input id="hdnColor" type="hidden" runat="server" />
         <asp:Button ID="btnUpdateSelected" runat="server" CssClass="HiddenControl" Text=""/>  
          <input id="hdnShelfWhere" type="hidden" runat="server" />
     </div>  
  
  <%--
     </ContentTemplate>
        </asp:UpdatePanel>  --%>
         </ContentTemplate>
        </asp:UpdatePanel>  

    </form>  
</body>
</html>