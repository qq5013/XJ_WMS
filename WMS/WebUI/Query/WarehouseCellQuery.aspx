<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WarehouseCellQuery.aspx.cs" Inherits="WMS.WebUI.Query.WarehouseCellQuery" %>

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
    <script type="text/javascript" src='<%=ResolveClientUrl("~/JQuery/jquery.ztree.core-3.5.js") %>'></script>
     
    
    <script type="text/javascript">
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
            if (tNodeIDLen == 3) {
                //库区
                sAreaCode = tNodeID;
            }
            if (tNodeIDLen == 6) {
                //货架
                sShelfCode = tNodeID;
            }
            document.getElementById("lblCurrentNode").innerHTML = treeNode.name;
             
            $("#frame").attr("src", " WarehouseCell.aspx?ShelfCode=" + sShelfCode + "&AreaCode=" + sAreaCode);
 
        };
        function showIconForTree(treeId, treeNode) {
            return !treeNode.isParent;
        };
        $(document).ready(function () {
            zNodes = eval($('#hidetree').val()); //.replace('[','').replace(']','')
            $.fn.zTree.init($("#treeDemo"), setting, zNodes);
            var zTree = $.fn.zTree.getZTreeObj("treeDemo");
            var tNodeID = zTree.getNodes(0)[0];
            tNodeID.iconSkin = "../../images/leftmenu/in_warehouse.gif";
            zTree.updateNode(tNodeID);
            zTree.expandNode(tNodeID, true, null, null, false);
            document.getElementById("lblCurrentNode").innerHTML = zTree.getNodes(0)[0].children[0].name;
            $("#frame").attr("src", "WarehouseCell.aspx?ShelfCode=&AreaCode=" + zTree.getNodes(0)[0].children[0].id);
            $('#btnUpdateSelected').bind('click', function () {
                return false;
            });

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
               </ContentTemplate>
               <Triggers>
                  
               </Triggers>
        </asp:UpdatePanel>  
        <table style="width:100%; background-color:WHITE;" cellpadding="0" cellspacing="0">
           <tr>
             <td class="maintable" colspan="2" align="right">
                     <asp:Button ID="btnCancel" Text="退出" runat="server" CssClass="ButtonExit" OnClientClick="Exit();"  />            
             </td>
           </tr>
           <tr>
              <td class="SideBar">
                <div id="div_tree" style="overflow-x:hidden; overflow-y:auto; width:280px; height:500px;">
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
         <asp:Button ID="btnUpdateSelected" runat="server" CssClass="HiddenControl" Text=""/>  
     </div>  
  
  <%--
     </ContentTemplate>
        </asp:UpdatePanel>  --%>


    </form>  
</body>
</html>