﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WarehouseSetPage2.aspx.cs" Inherits="WMS.WebUI.CMD.WarehouseSetPage2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
    <title></title>
    <link rel="stylesheet" type="text/css" href="../../Css/op.css" />  
    <link rel="stylesheet" type="text/css" href="../../Css/main.css" />  
    <link rel="stylesheet" type="text/css" href="../../ext/packages/ext-theme-crisp/build/resources/ext-theme-crisp-all.css" /> 
    <script type="text/javascript" src="../../JQuery/jquery-2.1.3.min.js"></script>  
    <script type="text/javascript" src= "../../JScript/Common.js"></script>
    <script type="text/javascript" src="../../Ext/ext-all.js"></script>  
    <script type="text/javascript" src="../../Ext/packages/ext-theme-crisp/build/ext-theme-crisp.js"></script>
    <script language="javascript" type="text/javascript">

        Ext.onReady(function () {
            var blnReload = false;
            var tree;

            var topPanel = {
                region: "north",
                height: 25,
                //bodyStyle: 'border:true;border-width:1px 0 1px 0;background:gray',
                collapsible: false,

                html: '<table style="width: 100%; height: 24px;" cellpadding="0" cellspacing="0"><tr class="maintable"><td style="height:24px" colspan="2" align="right"><input type="button" id="btnNewWarehouse" value="增加仓库" class="ButtonCreate" onclick="OpenEditWarehouse()" /><input type="button" id="btnNewArea" value="增加库区" class="ButtonCreate" onclick="OpenEditArea()" /><input type="button" id="btnNewShelf" value="增加货架" class="ButtonCreate" onclick="OpenEditShelf()" /><input type="button" id="btnNewCell" value="增加货位" class="ButtonCreate" onclick="OpenEditCell()" /><input type="button" id="btnExit" value="退出" class="ButtonExit" onclick="Exit()" /></td></tr></table>'
            };
            var leftPanel = Ext.create('Ext.panel.Panel', {
                region: 'west',
                //title: '仓库资料',
                bodyStyle: 'border:true;border-width:1px 1 1px 1;background:white',
                width: 230,
                minWidth: 90,
                layout: 'fit',
                //split: true,                
                split: {
                    size: 3
                },

                collapsible: false

            });
            var centerPanel = Ext.create('Ext.panel.Panel', {
                region: 'center',
                title: '仓库资料',
                //bodyStyle: 'border:true;border-width:1px 1 1px 1;background:blue',
                layout: "fit",
                plit: true,
                border: true,
                html: '<iframe id="frmMain_warehouse" scrolling="auto" frameborder="0" width="100%" height="100%" src=""> </iframe>',
                collapsible: false
            });
            var buildTree = function (json) {
                return Ext.create('Ext.tree.Panel', {
                    rootVisible: false,
                    title: '仓库资料',
                    border: true,
                    bodyStyle: 'background:#bad5eb;',
                    store: Ext.create('Ext.data.TreeStore', {
                        root: {
                            expanded: true,
                            children: json.children
                        }
                    }),
                    listeners: {
                        'itemclick': function (view, record, item, index, e) {
                            var tNodeID = record.get('id');
                            var text = record.get('text');
                            var tNodeIDLen = tNodeID.length;
                            centerPanel.setTitle("当前选中的节点：" + text);
                            $("#hdnNodeID").val(tNodeID);
                            if (tNodeIDLen == 2) {
                                //仓库
                                $("#frmMain_warehouse").attr("src", "WarehouseEditPage.aspx?WAREHOUSE_CODE=" + tNodeID);
                            }
                            if (tNodeIDLen == 3) {
                                //库区
                                $("#frmMain_warehouse").attr("src", "WarehouseAreaEditPage.aspx?CMD_WH_AREA_ID=" + tNodeID);
                            }
                            if (tNodeIDLen == 6) {
                                //货架
                                $("#frmMain_warehouse").attr("src", "WarehouseShelfEditPage.aspx?CMD_WH_SHELF_ID=" + tNodeID);
                            }
                            if (tNodeIDLen == 9) {
                                //货位
                                $("#frmMain_warehouse").attr("src", "WarehouseCellEditPage.aspx?CMD_CELL_ID=" + tNodeID);
                            }
                        },
                        scope: this
                    }
                });
            };
            Ext.Ajax.request({
                url: 'WareHouseTree.ashx',
                async: false,
                method: 'get',
                success: function (response) {

                    var json = Ext.JSON.decode(response.responseText)
                    Ext.each(json, function (el) {
                        //debugger
                        tree = buildTree(el);
                        leftPanel.add(tree);
                    });

                    if (!blnReload) {
                        var root = tree.getRootNode().firstChild;
                        centerPanel.setTitle("当前选中的节点：" + root.data.text);                        
                        $("#frmMain_warehouse").attr("src", "WarehouseEditPage.aspx?WAREHOUSE_CODE=" + root.id);                        
                        blnReload = false;
                    }

                },
                failure: function (request) {
                    Ext.MessageBox.show({
                        title: '操作提示',
                        msg: "连接服务器失败",
                        buttons: Ext.MessageBox.OK,
                        icon: Ext.MessageBox.ERROR
                    });
                }

            });

            
            //布局
            var viewport = new Ext.create('Ext.container.Viewport', {
                enableTabScroll: true,
                layout: "border",
                items: [topPanel,
                leftPanel,
              centerPanel
              ]
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
     <div>
     </div>
    </form>
</body>
</html>
