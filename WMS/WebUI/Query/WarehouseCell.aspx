<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WarehouseCell.aspx.cs" Inherits="WMS.WebUI.Query.WarehouseCell" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>货位信息显示</title>
     <link href="~/Css/Main.css" type="text/css" rel="stylesheet" /> 
        <link href="~/Css/op.css" type="text/css" rel="stylesheet" /> 
        <script type="text/javascript" src="../../JQuery/jquery-2.1.3.min.js"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $(window).resize(function () {
                resize();
            });
        });
        function resize() {
            var h = document.documentElement.clientHeight;
            $("#pnlCell").css("height", h - 20);
        }
//        function ShowCellInfo(objID) {
//            window.showModalDialog('CellInfo.aspx?Where=' + objID, window, 'DialogHeight:390px;DialogWidth:350px;help:no;scroll:auto;Resizable:no');
        //        }


        function ShowCellInfo(obj) {
            var product = document.getElementById("productinfo");
            $.getJSON("/CellStateSearch/Getcellinfo/?cellcode=" + obj, function (json) {
                if (json) {
                    if (json.total > 0) {
                        document.getElementById("cellcode").innerText = "货位号:" + cellcode;
                        document.getElementById("Barcode").innerText = json.rows[0].Barcode;
                        document.getElementById("CIGARETTE").innerText = json.rows[0].CIGARETTE;
                        document.getElementById("FORMULA").innerText = json.rows[0].FORMULA;
                        document.getElementById("BILLNO").innerText = json.rows[0].BILLNO;
                        document.getElementById("ORIGINAL").innerText = json.rows[0].ORIGINAL;
                        document.getElementById("GRADE").innerText = json.rows[0].GRADE;
                        document.getElementById("YEARS").innerText = json.rows[0].YEARS;
                        document.getElementById("STYLENO").innerText = json.rows[0].STYLENO;
                        document.getElementById("REALWEIGHT").innerText = json.rows[0].REALWEIGHT + "公斤";
                        document.getElementById("INDATE").innerText = json.rows[0].INDATE;
                        showinfo(obj);
                    }
                    else {
                        closeinfo();
                    }
                }
                else {
                    closeinfo();
                }
            });
        }
        function showinfo(obj) {
            var product = document.getElementById("productinfo");
            var objtop = obj.offsetTop;
            var objheight = obj.clientHeight;
            var objleft = obj.offsetLeft;
            while (obj = obj.offsetParent) { objtop += obj.offsetTop; objleft += obj.offsetLeft; }
            if ((objtop + objheight + parseFloat(product.style.height) - xuanfu.offsetHeight) > parseFloat(main.style.height)) {
                product.style.top = parseFloat(objtop) * parseFloat(currZoom.replace('%', '')) / 100 - parseFloat(product.style.height) + "px";
                if ((objleft + parseFloat(product.style.width)) > document.body.clientWidth) {
                    product.style.left = (parseFloat(objleft)) * parseFloat(currZoom.replace('%', '')) / 100 - parseFloat(product.style.width) + "px";
                }
                else
                    product.style.left = parseFloat(objleft) * parseFloat(currZoom.replace('%', '')) / 100 + "px";
            }
            else {
                product.style.top = parseFloat(objtop + objheight) * parseFloat(currZoom.replace('%', '')) / 100 + "px";
                if ((objleft + parseFloat(product.style.width)) > document.body.clientWidth) {
                    product.style.left = (parseFloat(objleft)) * parseFloat(currZoom.replace('%', '')) / 100 - parseFloat(product.style.width) + "px";
                }
                else
                    product.style.left = parseFloat(objleft) * parseFloat(currZoom.replace('%', '')) / 100 + "px";
            }
            product.style.display = "block";
        }

        function closeinfo() {
            var product = document.getElementById("productinfo");
            product.style.display = "none";
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:Panel ID="pnlCell" runat="server"  Width="100%" Height="450px"  style="overflow:auto;" >
    </asp:Panel>
    <div id="productinfo"  style=" width:390px; height:350px; position:absolute; background-color:#dbe7fd;opacity:0.8; display:none; border:1px solid #000;">
        <div id="btclose" style=" width:100%; height:20px;">
          <span id="cellcode" style="float:left"></span>
          <span onclick="closeinfo()"  style=" float:right; width:15px; height:20px;  cursor:pointer">X</span>
        </div>
        <div>
             <table class="maintable"  width="100%" align="center" cellspacing="0" cellpadding="0"  border="1">
                   <tr>
                      <td colspan="2" class="musttitle">
                        <b>产品信息</b>
                      </td>
                   </tr>
                   <tr>
                      <td  class="smalltitle" style="width:20%;">
                            &nbsp;产品名称:
                      </td>
                      <td ID="lblProductName">
                        
                      </td>
                   </tr>
                   <tr>
                      <td   class="smalltitle" style="width:20%;">
                             &nbsp;条码:
                      </td>
                      <td ID="lblBarcode">
                        
                      </td>
                   </tr>
                   <tr>
                      <td   class="smalltitle"  style="width:20%;">
                             &nbsp;托盘:
                      </td>
                      <td ID="lblPalletCode">
                          
                      </td>
                   </tr>
                   <tr>
                      <td   class="smalltitle"  style="width:20%;">
                             &nbsp;单据号:
                      </td>
                      <td ID="lblBillNo">
                           
                      </td>
                   </tr>
                    <tr>
                      <td   class="smalltitle"  style="width:20%;">
                             &nbsp;入库时间:
                      </td>
                      <td ID="lblIndate">
                          
                      </td>
                   </tr>
                    <tr>
                      <td  colspan="2" class="musttitle">
                        <b>货架信息</b>
                      </td>
                   </tr>
                    <tr>
                      <td class="smalltitle" style="width:20%;" >
                             &nbsp;库区名称:
                      </td>
                      <td ID="lblAreaName">
                          
                      </td>
                   </tr>
                   <tr>
                      <td   class="smalltitle"  style="width:20%;">
                             &nbsp;货架名称:
                      </td>
                      <td ID="lblShelfName"> 
                          
                      </td>
                   </tr>
                   <tr>
                      <td   class="smalltitle" style="width:20%;">
                             &nbsp;列:
                      </td>
                      <td ID="lblCellColumn">
                          
                      </td>
                   </tr>
                   <tr>
                      <td  class="smalltitle"  style="width:20%;">
                             &nbsp;层:
                      </td>
                      <td ID="lblCellRow">
                          
                      </td>
                   </tr>
                    <tr>
                      <td  class="smalltitle"  style="width:20%;">
                             &nbsp;状态:
                      </td>
                      <td ID="lblState">
                          
                      </td>
                   </tr>
               </table>
        </div>
    </div>




    </form>
</body>
</html>
