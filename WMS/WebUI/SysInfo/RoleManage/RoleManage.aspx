<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RoleManage.aspx.cs" Inherits="WMS.WebUI.SysInfo.RoleManage.RoleManage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>无标题页</title>
     
     <link href="../../../css/main.css" type="text/css" rel="Stylesheet" />
      <link href="../../../css/op.css" type="text/css" rel="Stylesheet" />
     <script type="text/javascript" src="../../../JQuery/jquery-2.1.3.min.js"></script>
    <script type="text/javascript" src="../../../JScript/Common.js"></script>
    <script language="javascript" type="text/javascript">

        $(document).ready(function () {
            $(window).resize(function () {
                resize();
            });
        });
        function resize() {
            var h = document.documentElement.clientHeight - 5;
            $("#table-container").css("height", h);
            $("#dvGroupUser").css("height", h - 285);
            $('#fieldset').css("height", h);
            $('#iframeRoleSet').css("height", h);
        }


        function RoleSet(GroupID, GroupName) {
            var date = new Date();
            var t = date.getMinutes() + date.getSeconds() + date.getMilliseconds();
            var iframeRoleSet = document.getElementById("iframeRoleSet");
            iframeRoleSet.src = "RoleSet.aspx?GroupID=" + GroupID + "&GroupName=" + encodeURI(GroupName) + "&time=" + t;
        }

        function UserSet(GroupID, GroupName) {
            var now = new Date();
            var temp = now.getMilliseconds();
            window.showModalDialog('GroupUserManage.aspx?GroupID=' + GroupID + '&GroupName=' + encodeURI(GroupName) + '&temp=' + temp, temp, 'top=0;left=0;toolbar=no;menubar=yes;scrollbars=no;resizable=yes;location=no;status=no;dialogWidth=450px;dialogHeight=500px');
        }

        function ShowGroupUserList(GroupID, GroupName) {
//            var d = new Date();
//            var t = d.getMinutes() + d.getMilliseconds();
//            document.getElementById("iframeGroupUserList").src = "GroupUserList.aspx?GroupName=" + encodeURI(GroupName) + "&GroupID=" + GroupID + "&t=" + t;
//            RoleSet(GroupID, GroupName);
        } 
  </script>  
</head>
<body>
    <form id="form1" runat="server">
         <asp:ScriptManager ID="ScriptManager1" runat="server" />  
        <asp:UpdateProgress ID="updateprogress" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
            <ProgressTemplate>            
                <div id="progressBackgroundFilter" style="display:none"></div>
                <div id="processMessage"> Loading...<br /><br />
                        <img alt="Loading" src="../../../images/loading.gif" />
                </div>      
            </ProgressTemplate> 
        </asp:UpdateProgress>  
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">                
            <ContentTemplate>
              <table id="table-container" cellpadding="0" cellspacing="0" >
                <tr>
                  <td style=" vertical-align:top; width: 300px;"><!--GroupList-->
              
                        <div style="overflow: auto; WIDTH: 100%; height:200px">
                            <asp:GridView ID="gvGroupList" runat="server" SkinID="GridViewSkin" AllowPaging="false" Width="100%"
                             AutoGenerateColumns="False" OnRowDataBound="gvGroupList_RowDataBound"  >
                                <Columns>
                                    <asp:BoundField DataField="GroupID" HeaderText="ID">
                                        <HeaderStyle Width="0px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="GroupName" HeaderText="用户组名称">
                                        <ItemStyle Width="85px" />
                                    </asp:BoundField>
                                     <asp:TemplateField HeaderText="操作" >
                                        <ItemTemplate>
                                          <asp:Button ID="btnAddUser"  CssClass="ButtonCreate"  runat="server"  Text="添加用户" OnClick="btnAddUser_Click"/>   
                                        </ItemTemplate>
                                        <ItemStyle Width="85px" Wrap="False" />
                                        <HeaderStyle Width="85px" Wrap="False" />
                                    </asp:TemplateField>
                                </Columns>
                                 <PagerSettings Visible="False" />
                            </asp:GridView>
                        </div>
                          <div>
                            &nbsp;<asp:LinkButton ID="btnFirst" runat="server" OnClick="btnFirst_Click" Text="首页"></asp:LinkButton> 
                            &nbsp;<asp:LinkButton ID="btnPre" runat="server" OnClick="btnPre_Click" Text="上一页"></asp:LinkButton> 
                            &nbsp;<asp:LinkButton ID="btnNext" runat="server" OnClick="btnNext_Click" Text="下一页"></asp:LinkButton> 
                            &nbsp;<asp:LinkButton ID="btnLast" runat="server" OnClick="btnLast_Click" Text="尾页"></asp:LinkButton> 
                            &nbsp;<asp:textbox id="txtPageNo" onkeypress="return regInput(this,/^\d+$/,String.fromCharCode(event.keyCode))"
					                onpaste="return regInput(this,/^\d+$/,window.clipboardData.getData('Text'))" ondrop="return regInput(this,/^\d+$/,event.dataTransfer.getData('Text'))"
					                runat="server" Width="30px" CssClass="TextBox" ></asp:textbox>
                            &nbsp;<asp:linkbutton id="btnToPage" runat="server" onclick="btnToPage_Click" Text="跳转"></asp:linkbutton>
                            &nbsp;<asp:Label ID="lblCurrentPage" runat="server" Visible="False" ></asp:Label>
                        </div>
                 
                       <table class="maintable" style="width:298px; height:30px;">
                            <tr>
                                <td>
                                    <b>用户组成员</b>
                                </td>
                            </tr>
                        </table>
             
                        <div id="dvGroupUser" style="overflow: auto; WIDTH: 100%; height:200px">
                            <asp:GridView ID="gvGroupListUser" runat="server"   
                                OnRowDataBound="gvGroupListUser_RowDataBound"  AutoGenerateColumns="False" 
                                AllowPaging="True" PageSize="5" SkinID="GridViewSkin" Width="100%" >
                                <Columns>
                                    <asp:BoundField DataField="UserID" HeaderText="ID"></asp:BoundField>
                                      <asp:BoundField DataField="UserName" HeaderText="用户名">
                                          <HeaderStyle Width="20%" />
                                      </asp:BoundField>
                                      <asp:BoundField DataField="GroupName" HeaderText="用户组">
                                          <HeaderStyle Width="30%" />
                                      </asp:BoundField>

                                      
                                       <asp:TemplateField HeaderText="操作" >
                                        <ItemTemplate>
                                          <asp:Button ID="btnDeleteUser" CommandName="btnDeleteUser" CommandArgument= '<%# DataBinder.Eval(Container.DataItem, "UserID")%> ' CssClass="ButtonDel"  runat="server"  Text="删除用户" OnClick="btnDeleteUser_Click"/>   
                                        </ItemTemplate>
                                        <ItemStyle Width="85px" Wrap="False" />
                                        <HeaderStyle Width="85px" Wrap="False" />
                                    </asp:TemplateField>
                                </Columns>
                                 <PagerSettings Visible="False" />
                            </asp:GridView>
                        </div>
                         <div style="height:30px;">
                            &nbsp;&nbsp;<asp:LinkButton ID="btnFirstSub1" runat="server" 
                                OnClick="btnFirstSub1_Click" Text="首页"></asp:LinkButton> 
                            &nbsp;<asp:LinkButton ID="btnPreSub1" runat="server" OnClick="btnPreSub1_Click" 
                                Text="上一页"></asp:LinkButton> 
                            &nbsp;<asp:LinkButton ID="btnNextSub1" runat="server" 
                                OnClick="btnNextSub1_Click" Text="下一页"></asp:LinkButton> 
                            &nbsp;<asp:LinkButton ID="btnLastSub1" runat="server" 
                                OnClick="btnLastSub1_Click" Text="尾页"></asp:LinkButton> 
                            &nbsp;<asp:textbox id="txtPageNoSub1" onkeypress="return regInput(this,/^\d+$/,String.fromCharCode(event.keyCode))"
					                onpaste="return regInput(this,/^\d+$/,window.clipboardData.getData('Text'))" ondrop="return regInput(this,/^\d+$/,event.dataTransfer.getData('Text'))"
					                runat="server" Width="30px" CssClass="TextBox" ></asp:textbox>
                            &nbsp;<asp:linkbutton id="btnToPageSub1" runat="server" 
                                onclick="btnToPageSub1_Click" Text="跳转"></asp:linkbutton>
                            &nbsp;<asp:Label ID="lblCurrentPageSub1" runat="server" Visible="False" ></asp:Label>
                        </div>
                  </td>
          
                  <td style=" vertical-align:top; width: 100%;"><!--RoleSet-->
                    <table cellpadding="0" cellspacing="0" width="100%">
                       <tr>
                         <td style="vertical-align:top; ">
                           <fieldset id="fieldset" style=" width:98%; padding:0 0 0 0;">
                             <iframe id="iframeRoleSet"  style="width:100%;" frameborder="0" scrolling="no"></iframe>
                           </fieldset>
                         </td>
                       </tr>
                    </table>
                  </td>
                </tr>
              </table>  
    
            <div id="divHiden" style="display:none;">
            <div>
                <asp:Button ID="btnReload" runat="server" Text="" OnClick="btnReload_Click"  CssClass="HiddenControl" />
                <asp:HiddenField ID="hdnRowIndex" runat="server" Value="0" />
                <asp:HiddenField ID="hdnRowValue" runat="server"  />
                <asp:HiddenField ID="hdnRowGroupName" runat="server"  />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
  
</body>
</html>

