﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
    <title></title>
    <link rel="stylesheet" type="text/css" href="Css/icon.css" />  
    <link rel="stylesheet" type="text/css" href="Css/default.css" />  
    <link rel="stylesheet" type="text/css" href="ext/packages/ext-theme-crisp/build/resources/ext-theme-crisp-all.css" /> 
    <script type="text/javascript" src="JQuery/jquery-2.1.3.min.js"></script>  
    <script type="text/javascript" src="Ext/ext-all.js"></script>  
    <script type="text/javascript" src="Ext/packages/ext-theme-crisp/build/ext-theme-crisp.js"></script>
     
<%--    <link rel="stylesheet" type="text/css" href="ext/packages/ext-theme-classic/build/resources/ext-theme-classic-all.css" />  
    <script type="text/javascript" src="Ext/ext-all.js"></script>  
    <script type="text/javascript" src="Ext/packages/ext-theme-classic/build/ext-theme-classic.js"></script>--%>

    <script language="javascript" type="text/javascript">

        Ext.onReady(function () {
            //            window.onresize = function () {
            //                alert(document.documentElement.clientHeight);
            //                alert(window.frames["frmMain"].Ext.getCmp("grid1"));
            //                grid = window.frames["frmMain"].Ext.getCmp("grid1");
            //                alert(grid);
            //                grid.setHeight(document.documentElement.clientHeight);
            //            }
            var topPanel = {
                region: "north",
                height: 120,
                title: '罗伯泰克自动化科技(苏州)有限公司',
                bodyStyle: 'border:true;border-width:1px 0 1px 0;background:gray',
                collapsible: true,

                html: '<div class="header"><table width="100%" height="80" border="0" cellpadding="0" cellspacing="0"  background="Images/top_bg.jpg" style="top: 0px; z-index: inherit"><tr><th align="left" valign="top" scope="col" style="height:80px; background-repeat:no-repeat; width: 100%;" background="Images/banner_1.jpg"><div class="topNav"><a href="javascript:void(0)" id="changeSystem" onclick="changeSystemClick() style="font">切换系统</a><span>|</span><a href="javascript:void(0)" id="changeUser">切换用户</a><span>|</span><a href="javascript:void(0)" id="changePassword">修改密码</a><span>|</span><a href="javascript:void(0)" id="loginOut">退出</a></div></th></tr></table></div>'
            };
            /** 
            * 左,panel.Panel 
            */
            var leftPanel = Ext.create('Ext.panel.Panel', {
                region: 'west',
                title: '导航栏',
                bodyStyle: 'border:true;border-width:1px 1 1px 1;background:blue',
                width: 230,
                minWidth: 90,
                //split: true,                
                split: {
                    size: 3
                },

                collapsible: true,
                //collapseMode: 'mini',
                defaults: {
                    // applied to each contained panel
                    //bodyStyle: 'padding:15px'
                },
                layout: {
                    // layout-specific configs go here
                    type: 'accordion',
                    //titleCollapse: false,
                    animate: true
                    //activeOnTop: true
                }
            });


            /** 
            * 组建树 
            */
            var buildTree = function (json) {
                return Ext.create('Ext.tree.Panel', {
                    rootVisible: false,
                    border: false,
                    store: Ext.create('Ext.data.TreeStore', {
                        root: {
                            expanded: true,
                            children: json.children
                        }
                    }),
                    listeners: {
                        //                        'itemclick': function (view, record, item,
                        //                                        index, e) {
                        //                            var id = record.get('id');
                        //                            var text = record.get('text');
                        //                            var leaf = record.get('leaf');
                        //                            if (leaf) {
                        //                                alert('id-' + id + ',text-' + text
                        //                                                + ',leaf-' + leaf);
                        //                            }
                        //                        },

                        'itemclick': function (view, record, item,
                                        index, e) {
                            var id = record.get('id');
                            var text = record.get('text');
                            var url = record.get('url');
                            var exist = false;
                            //debugger;
                            if (record.childNodes.length > 0)
                                return;
                            //addtab(id, "Main.aspx", "Main");
                            tabPanel.items.each(function (item) {
                                //debugger;
                                //alert(text);
                                if (item.title == text) {
                                    //debugger;
                                    var tab = Ext.getCmp(item.id);
                                    //if (tabPanel.findById(text) != null) {
                                    tabPanel.setActiveTab(tab);
                                    //                                if (url.indexOf("NoReadWhere") >= 0)
                                    //                                    window.frames["if_" + idName].location.href = url;
                                    //                                else
                                    //                                    window.frames["if_" + idName].location.reload();
                                    exist = true;
                                    return;
                                }
                            });
                            if (!exist) {
                                tabPanel.add({
                                    id: id,
                                    title: text,

                                    html: '<iframe id="frmMain" scrolling="auto" frameborder="1" width="100%" height="100%" src="' + url + '"> </iframe>',
                                    closable: true
                                }).show();
                            }
                        },
                        scope: this
                    }
                });

            };

            var treestore = Ext.create('Ext.data.TreeStore', {
                root: {
                    expanded: true,
                    children: [
            { text: "detention", leaf: true },
            { text: "homework", expanded: true, children: [
                { text: "book report", leaf: true },
                { text: "algebra", leaf: true }
            ]
            },
            { text: "buy lottery tickets", leaf: true }
        ]
                }
            });

            /** 
            * 加载菜单树 
            */
            Ext.Ajax.request({
                url: 'LeftTreeJson.ashx',
                async: false,
                method: 'get',
                success: function (response) {
                    var json = Ext.JSON.decode(response.responseText)
                    Ext.each(json, function (el) {
                       // debugger;
                        var panel = Ext.create('Ext.panel.Panel', {
                            //id: el.id, id不能加，加了会出错
                            title: el.text,
                            layout: 'fit'
                        });

                        var treePanel = buildTree(el);
                        panel.add(treePanel);

                        leftPanel.add(panel);
                    });
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
            //center
            var tabPanel = new Ext.TabPanel({
                region: "center",
                layout: "fit",
                plit: true,
                border: true,
                id: "main",
                enableTabScroll: true,
                deferredRender: false,
                activeTab: 0,
                items: [{
                    title: 'index',
                    loader: { url: 'Default.htm', scripts: true, nocache: true }
                }]
            });
            function addtab(id, link, name) {
                var tabId = "tab-" + id; //为id稍作修改。
                var tabTitle = name;
                var tabLink = link;

                var tab = tabPanel.getComponent(tabId); //得到tab组建

                var subMainId = 'tab-' + id + '-main';

                if (!tab) {

                    tab = tabPanel.add(new Ext.Panel({
                        id: tabId,
                        title: tabTitle,
                        //autoLoad:{url:tablink, scripts:true,nocache:true},
                        autoScroll: true,
                        iconCls: 'tabIconCss',
                        layout: 'fit',
                        border: false,
                        closable: true
                    })
                                        );

                    tabPanel.setActiveTab(tab);

                    tab.load({
                        url: tabLink,
                        method: 'post',
                        params: { subMainId: subMainId },
                        scope: this, // optional scope for the callback
                        discardUrl: true,
                        nocache: true,
                        text: "页面加载中,请稍候……",
                        timeout: 9000,
                        scripts: true
                    });

                } else {
                    tabPanel.setActiveTab(tab);
                }
            }

            //south
            var bottomPanel = {
                region: "south",
                //bodyStyle: 'border:1px solid #FF0000',
                height: 30,
                titleAlign: null,
                //bodyStyle: 'background:#cbe4f3',
                html: '<div style="border:1px solid #cbe4f3;line-height:30px;text-align: center;background:url(Images/bottom.jpg) repeat-x;"><a href="http://www.unitechnik.com.cn" target="_blank">罗伯泰克自动化科技（苏州）有限公司</a></div>'

            };
            //布局
            var viewport = new Ext.create('Ext.container.Viewport', {
                enableTabScroll: true,
                layout: "border",
                items: [
                topPanel,
                leftPanel,
                //accordion2,
              tabPanel,
              bottomPanel
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
