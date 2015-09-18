<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="WMS.Index.Main" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >


<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server"> 
<style type="text/css">
</style>
    <title>主页</title>
    <link href="../Css/css.css" type="text/css" rel="stylesheet" />
       <script type="text/javascript" src='<%=ResolveClientUrl("~/JScript/Common.js") %>'></script>
<script type="text/javascript" language="javascript">
    function SetNewColor(source) {
        _oldColor = source.style.backgroundColor;
        source.style.backgroundColor = '#C0E4EE';
        source.style.cursor = "pointer";

    }
    function SetOldColor(source) {
        source.style.backgroundColor = _oldColor;
        source.style.cursor = "default";
    }
   
</script>
</head>
<body bgcolor="#F8FCFF" style="margin-top:30px;">
    <form id="form1" runat="server">
         <input type="hidden" runat="server" id="hdnMsg" /> 
         <input type="hidden" runat="server" id="hdnProduct" /> 
         <input type="hidden" runat="server" id="hdnTask" />
    </form>
        
</body>
</html>
<script type="text/javascript">
    if (document.getElementById('pnlRemind') != null) {
        //      ShowMessage();
        //      window.setInterval(ShowMessage(),10)
        document.getElementById('pnlRemind').style.display = 'none';
        showMsg('oDialog');
    }

    function ShowMessage() {
        //   //TimeoutFlag=window.setTimeout( richDialog, 1000 );
        //   document.getElementById('oDialog').style.display='block';
        //   document.getElementById('pnlRemind').style.display='none';
        //   window.setTimeout( 'HideMessage()', 10000);

        document.getElementById('pnlRemind').style.display = 'none';
        showMsg('oDialog');
    }
    function HideMessage() {
        document.getElementById('oDialog').style.display = 'none';
        document.getElementById('pnlRemind').style.display = 'block';
    }


    //设置透明度
    function setOpacity(obj, value) {
        if (document.all) {
            if (value == 100) {
                obj.style.filter = "";
            } else {
                //alert(value);
                obj.style.filter = "alpha(opacity=" + value + ")";
            }
        } else {
            obj.style.opacity = value / 100;

        }
    }
    //用setTimeout循环减少透明度
    function changeOpacity(obj, startValue, endValue, step, speed) {
        if (startValue >= endValue) {
            //document.body.removeChild(obj);
            document.getElementById('oDialog').style.display = 'block';
            return;
        }
        if (!obj) {
            return;
        }
        if (startValue >= 100) {
            //document.body.removeChild(obj);
            document.getElementById('oDialog').style.display = 'block';
            return;
        }
        // alert(startValue);
        setOpacity(obj, startValue);
        setTimeout(function () { changeOpacity(obj, startValue + step, endValue, step, speed); }, speed);
    }
    //设置隐藏速度和id
    function showMsg(id) {
        var msg = document.getElementById(id);
        var step = 5, speed = 80;
        //    if(msg.style.display=="none")
        //    {
        //      msg.style.display="";
        //    }
        msg.style.display = 'block';
        changeOpacity(msg, 0, 100, step, speed);
    }


</script>