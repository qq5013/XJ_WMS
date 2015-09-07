<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductEdit.aspx.cs" Inherits="WMS.WebUI.CMD.ProductEdit" %>
<%@ Register src="../../UserControl/Calendar.ascx" tagname="Calendar" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
        <link href="~/Css/Main.css" type="text/css" rel="stylesheet" /> 
        <link href="~/Css/op.css" type="text/css" rel="stylesheet" /> 
        <script type="text/javascript" src="../../JQuery/jquery-2.1.3.min.js"></script>
        <script type="text/javascript" src= "../../JScript/Common.js"></script>
        <script type="text/javascript">
            function Save() {

                if (trim($("#txtID").val()) == "") {
                    alert("车型编码不能为空!");
                    $("#txtID").focus();
                    return false;
                }

                if (trim($("#txtTypeName").val()) == "") {
                    alert("车型名称不能为空!");
                    $("#txtTypeName").focus();
                    return false;
                }
                 
                return true;
            }
           
        </script>
    <style type="text/css">
        .laydate-icon
        {}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table style="width: 100%; height: 20px;" class="OperationBar">
            <tr>
                <td align="right">
                    <asp:Button ID="btnCancel" runat="server" Text="放弃" 
                        OnClientClick="return Cancel();" CssClass="ButtonCancel" />
                    <asp:Button ID="btnSave" runat="server" Text="保存" OnClientClick="return Save()" 
                        CssClass="ButtonSave" onclick="btnSave_Click" Height="16px" />
                    <asp:Button ID="btnExit" runat="server" Text="离开" OnClientClick="return Exit();" 
                        CssClass="ButtonExit" />
                </td>
            </tr>
        </table>
        <table id="Table1" class="maintable" cellspacing="0" cellpadding="0" width="100%" align="center" bordercolor="#ffffff"
				border="1" runat="server">			
				<tr>
                    <td align="center" class="musttitle" style="width:10%;"  >
                            产品编码
                    </td>
                    <td  width="23%">
                            &nbsp;<asp:TextBox 
                                ID="txtID" runat="server" BorderWidth="0" CssClass="TextBox" Width="40%" 
                                MaxLength="10"  ></asp:TextBox>
                    </td>
                    <td align="center" class="musttitle" style="width:10%;"  >
                           车型名称
                    </td>
                    <td width="23%">
                            &nbsp;<asp:TextBox ID="txtTypeName" runat="server" BorderWidth="0"  Width="2%" 
                                MaxLength="30" Height="16px"></asp:TextBox> 
                          &nbsp;<uc1:Calendar ID="txtBillDate" runat="server" Width="50%" />

                    </td>
                    <td align="center" class="musttitle" style="width:10%;">
                    </td>
                    <td width="23%">
                    </td>
                </tr>
              
                <tr>
                    <td align="center"  class="smalltitle" style="width:8%;">
                        备注
                    </td>
                    <td colspan="5">
                        &nbsp;<asp:TextBox ID="txtMemo" runat="server" CssClass="MultiLineTextBox" 
                            TextMode="MultiLine" Height="102px" Width="98%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                  <td align="center"  class="smalltitle" style="width:8%;">
                        建单人员
                  </td> 
                  <td>
                    &nbsp;<asp:TextBox ID="txtCreator" runat="server" BorderWidth="0" CssClass="TextRead" Width="40%"  ></asp:TextBox> 
                  </td>
                  <td align="center" class="smalltitle" style="width:8%;">
                        建单日期
                  </td> 
                  <td>
                    &nbsp;<asp:TextBox ID="txtCreatDate" runat="server" BorderWidth="0" 
                          CssClass="TextRead" Width="42%"  ></asp:TextBox> 
                  </td>
                  <td>
                  </td>
                  <td>
                  </td>
                </tr>
                <tr>
                  <td align="center"  class="smalltitle" style="width:8%;">
                        修改人员
                  </td> 
                  <td>
                     &nbsp;<asp:TextBox ID="txtUpdater" runat="server" BorderWidth="0" CssClass="TextRead" Width="40%"  ></asp:TextBox> 
                  </td>
                  <td align="center"  class="smalltitle" style="width:8%;">
                        修改日期
                  </td> 
                  <td>
                    &nbsp;<asp:TextBox ID="txtUpdateDate" runat="server" BorderWidth="0" 
                          CssClass="TextRead" Width="42%"  ></asp:TextBox> 
                  </td>
                  <td>
                  </td>
                  <td>
                  </td>
                </tr>		
		</table>
    </div>
    </form>
</body>
</html>
