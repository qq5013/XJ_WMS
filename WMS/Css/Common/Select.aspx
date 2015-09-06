<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Select.aspx.cs" Inherits="WMS.Common.Select" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <link href="~/Css/Main.css" type="text/css" rel="stylesheet" /> 
    <script type="text/javascript" src='<%=ResolveClientUrl("~/JQuery/jquery-1.8.3.min.js") %>'></script>
    <script type="text/javascript" src='<%=ResolveClientUrl("~/JScript/Common.js") %>'></script>
    <script type="text/javascript">

        function AddValues(oChk, strReturn) {
            var chkSelect = document.getElementById(oChk);
            if (chkSelect.checked) {
                (chkSelect.parentElement).parentElement.className = "bottomtable";
                if (SelectPage.HdnSelectedValues.value == '')
                    SelectPage.HdnSelectedValues.value += strReturn;
                else
                    SelectPage.HdnSelectedValues.value += ',' + strReturn;
            }
            else {
                (chkSelect.parentElement).parentElement.className = "";
                SelectPage.HdnSelectedValues.value = SelectPage.HdnSelectedValues.value.replace(',' + strReturn, '');
                SelectPage.HdnSelectedValues.value = SelectPage.HdnSelectedValues.value.replace(strReturn + ',', '');
                SelectPage.HdnSelectedValues.value = SelectPage.HdnSelectedValues.value.replace(strReturn, '');
            }
        }
        function SelectAll(tempControl) {
            var theBox = tempControl;
            xState = theBox.checked;

            elem = theBox.form.elements;
            for (i = 0; i < elem.length; i++)
            //if(elem[i].type=="checkbox" && elem[i].id!=theBox.id && elem[i].id.substring(elem[i].id.length-2,elem[i].id.length)==theBox.id.substring(theBox.id.length-2,theBox.id.length))
                if (elem[i].id.indexOf("chkSelect") >= 0) {
                    if (elem[i].checked != xState) {
                        elem[i].click();
                    }
                }
        }
        function SelectedCell() {
            elem = SelectPage.elements;
            for (i = 0; i < elem.length; i++)
                if (elem[i].type == 'checkbox' && elem[i].checked == true) {
                    (elem[i].parentElement).parentElement.className = "bottomtable";
                }
        }
        function AllClear(bln) {
            elem = SelectPage.elements;
            for (i = 0; i < elem.length; i++)
                if (elem[i].type == "checkbox" && elem[i].id.substring(elem[i].id.length - 6, elem[i].id.length) == "Select") {
                    if (bln == 0)
                        elem[i].checked = true;
                    else
                        elem[i].checked = false;
                    elem[i].click();
                }
        }
        function Close() {
            SelectPage.HdnSelectedValues.value = "";
            window.returnValue = document.getElementById('HdnSelectedValues').value;
            window.close();
        }
        function Select() {
            window.returnValue = "[" + document.getElementById('HdnSelectedValues').value + "]";
            window.close();
        }
        function SelectSearch() {
            if (trim(document.getElementById("txtContent").value) == "") {
                alert("请输入查询内容！");
                document.getElementById("txtContent").focus();
                return false;
            }
        }
  </script>
  </head>
<body>
    <form id="SelectPage" runat="server" >
     <asp:ScriptManager ID="ScriptManager1" runat="server" />  
    <asp:UpdateProgress ID="updateprogress" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
    <ProgressTemplate>            
             <div id="progressBackgroundFilter" style="display:none"></div>
        <div id="processMessage"> Loading...<br /><br />
             <img alt="Loading" src="../../images/process/loading.gif" />
        </div>            
 
        </ProgressTemplate>
 
    </asp:UpdateProgress>  
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">                
                <ContentTemplate>
         <table class="maintable" cellspacing="0" cellpadding="0" width="100%" align="center" border="1" >
				<tr> 
					<td class="smalltitle" align="center" width="10%" >查询栏位</td>
					<td  width="20%" height="20">&nbsp;
						<asp:dropdownlist id="ddlFieldName" runat="server" Width="90%"></asp:dropdownlist></td>
					<td class="smalltitle" align="center" width="10%">查询内容</td>
					<td  width="60%" height="20" valign="middle">&nbsp;<asp:textbox id="txtContent" 
                            tabIndex="1" runat="server" Width="60%" CssClass="TextBox"  heigth="16px"></asp:textbox>
                        &nbsp;<asp:button id="btnSearch" tabIndex="2" runat="server" Width="60px" CssClass="but" Text="立即查询" OnClientClick="return SelectSearch()" onclick="btnSearch_Click"></asp:button>
                        &nbsp;<asp:button id="btnRefresh" tabIndex="2" runat="server" Width="60px" 
                            CssClass="but" Text="刷新" onclick="btnRefresh_Click"></asp:button>
                        
                    </td>
				</tr>
				    
		</table>
        <div style="overflow: auto; width:600px; height:400px" id="SelectDiv">
            <asp:HiddenField ID="HiddenField1" runat="server" />
            <asp:GridView ID="GridView1" runat="server" SkinID="GridViewSkin" Height="20px" Width="100%" AllowPaging="True"  PageSize="12" OnRowDataBound="GridView1_RowDataBound" FooterStyle-Wrap="false" HeaderStyle-Wrap="false" RowStyle-Wrap="false" >

            <RowStyle Wrap="False"></RowStyle>
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>                        
				        <input type="checkbox" runat="server" id="chkHeadSelect" onclick="SelectAll(this);"/>                    
                    </HeaderTemplate>
			        <ItemTemplate>
				        <input type="checkbox" runat="server" id="chkSelect"/>
			        </ItemTemplate>
                    <HeaderStyle Width="20px" />
                    <ItemStyle HorizontalAlign="Center" />
		        </asp:TemplateField>
		        <asp:TemplateField HeaderText="選取">
			        <ItemTemplate>
                        <asp:Button ID="btnSingle" runat="server" Text="選取" CssClass="but"  />
			        </ItemTemplate>                    
                    <ControlStyle Width="50px" Height="20px" />
		        </asp:TemplateField>		        
               
	        </Columns>
	         <PagerSettings Visible="False" />
                
            <FooterStyle Wrap="False"></FooterStyle>

            <HeaderStyle Wrap="False"></HeaderStyle>
                
        </asp:GridView>
        
        </div>
        <table width="100%" class="table_bgcolor">
            <tr>
                <td style=" height:3px;"></td>
            </tr>
            <tr>
                 <td align="left" style=" width:25%"  >
                    &nbsp;&nbsp;<asp:Button ID="btnSelect" runat="server" Text="确定" Width="60px" 
                        CssClass="but" OnClientClick="Select();" />
                &nbsp;<asp:Button ID="btnClose" runat="server" Text="关闭" Width="60px" CssClass="but" OnClientClick="Close()" /></td>

                <td align= "right">
                    <asp:LinkButton ID="btnFirst" runat="server" OnClick="btnFirst_Click" >首頁</asp:LinkButton>&nbsp;
                    <asp:LinkButton ID="btnPre" runat="server"   OnClick="btnPre_Click" >上一頁</asp:LinkButton>&nbsp;
                    <asp:LinkButton ID="btnNext" runat="server"  OnClick="btnNext_Click" >下一頁</asp:LinkButton>&nbsp;
                    <asp:LinkButton ID="btnLast" runat="server"  OnClick="btnLast_Click" >尾頁</asp:LinkButton>&nbsp;&nbsp;
                    <asp:TextBox ID="txtPage" runat="server" Width="34px" CssClass="pagetext" onkeypress="return regInput(this,/^\d+$/,String.fromCharCode(event.keyCode)) ;"
                     ondrop="return regInput(this,/^\d+$/,event.dataTransfer.getData('Text'));" onpaste="return regInput(this,/^\d+$/,window.clipboardData.getData('Text'));"
                     onkeydown="if(event.keyCode==13){event.keyCode=9;document.getElementById('btnToPage').click();return false;}" ></asp:TextBox>
                     <asp:LinkButton ID="btnToPage" runat="server"  Width="40px" OnClick="btnToPage_Click" >跳轉</asp:LinkButton>&nbsp;&nbsp;  
                    <asp:Label ID="lblPage" runat="server" Text=""></asp:Label>&nbsp;     
                </td>
                    
            </tr>
        </table>
       
        <input id="HdnSelectedValues" type="hidden" name="HdnSelectedValues" runat="server" />
        <asp:HiddenField ID="hideTargetControls" runat="server" />
     </ContentTemplate>
     </asp:UpdatePanel> 
    </form>
</body>
</html>