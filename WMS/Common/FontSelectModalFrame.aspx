﻿<%@ Page Language="C#" AutoEventWireup="true" Inherits="FontSelectModalFrame" Title="字体设置" Codebehind="FontSelectModalFrame.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>字体设置</title>
</head>
<frameset rows="20,*" cols="*" frameborder="no" border="0" framespacing="0">
    <frame id="aaa" name="aaa" scrolling=no noresize=noresize frameborder=0 />
    <frame src="FontSelectModal.aspx" name="FontFrame" scrolling="No" noresize="noresize" frameborder="0" id="FontFrame" title="字体设置" />
</frameset>
</html>