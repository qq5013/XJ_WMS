using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using IDAL;

namespace WMS.WebUI.SysInfo.RoleManage
{
    public partial class RoleManage : System.Web.UI.Page
    {
        private string strTableView = "v_sys_GroupList";
        private string strPrimaryKey = "GroupID";
        //private string strOrderByFields = "GroupName ASC";
        private string strQueryFields = "GroupID,GroupName,Memo,State";
        bool PostBack = false;
        BLL.BLLBase bll = new BLL.BLLBase();

        #region 窗体加载
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (Request.QueryString["SubModuelCode"] != null)
                    {
                        Session["SubModuleCode"] = Request.QueryString["SubModuelCode"];
                    }
                    else
                    {
                        Session["SubModuleCode"] = "";
                    }
                    PostBack = false;
                }
                else
                {
                    PostBack = true;
                }
                GridDataBind();
            }
            catch (Exception exp)
            {

            }
        }

        private void GridDataBind()
        {
            int TotalCount=0;
            int PageCount=0;
            DataTable dtGroup = bll.GetDataPage("Security.SelectGroup", 1, 10000, out TotalCount, out PageCount,new DataParameter[] { new DataParameter("{0}", "1=1") });
            this.gvGroupList.DataSource = dtGroup;
            this.gvGroupList.DataBind();
            if (!PostBack)
            {
                string script = string.Format("document.getElementById('iframeGroupUserList').src='GroupUserList.aspx?GroupID={0}&GroupName={1}' ;", dtGroup.Rows[0]["GroupID"].ToString(), dtGroup.Rows[0]["GroupName"].ToString());
                script += string.Format("document.getElementById('iframeRoleSet').src='RoleSet.aspx?GroupID={0}&GroupName={1}' ;", dtGroup.Rows[0]["GroupID"].ToString(), dtGroup.Rows[0]["GroupName"].ToString());
               WMS.App_Code.JScript.Instance.RegisterScript(this, script);
            }
        }

        #endregion

        /// <summary>
        /// GridView行绑定事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvGroupList_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Visible = false;
                ////Label lbEdit = new Label();
                ////lbEdit.Text = "权限分配  ";

                ////lbEdit.Attributes.Add("onmouseover", "currentcolor=this.style.color;this.style.fontWeight=''; this.style.cursor='hand';this.style.color='orange'");
                ////lbEdit.Attributes.Add("onmouseout", "this.style.color=currentcolor,this.style.fontWeight='';");

                ////lbEdit.Attributes.Add("onclick", string.Format("RoleSet('{0}','{1}');",e.Row.Cells[0].Text,e.Row.Cells[1].Text));
                ////e.Row.Cells[2].Controls.Add(lbEdit);

                Label lbAdd = new Label();
                lbAdd.Text = "  添加用户";
                lbAdd.Attributes.Add("onmouseover", "currentcolor=this.style.color;this.style.fontWeight=''; this.style.cursor='hand';this.style.color='Orange'");
                lbAdd.Attributes.Add("onmouseout", "this.style.color=currentcolor,this.style.fontWeight='';");

                lbAdd.Attributes.Add("onclick", string.Format("UserSet('{0}','{1}');", e.Row.Cells[0].Text, e.Row.Cells[1].Text));
                e.Row.Cells[2].Controls.Add(lbAdd);

                e.Row.Attributes.Add("onclick", string.Format("ShowGroupUserList('{0}','{1}')", e.Row.Cells[0].Text, e.Row.Cells[1].Text));

                //当鼠标放上去的时候 先保存当前行的颜色 并给附一规格
                e.Row.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.fontWeight=''; this.style.cursor='hand';this.style.backgroundColor='WhiteSmoke'");
                //当鼠标离开的时候 将规格还原的以前的颜色


                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor,this.style.fontWeight='';");

            }
            else if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false;
            }
        }
        protected void gvGroupList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvGroupList.PageIndex = e.NewPageIndex;
            GridDataBind();
        }
    }
}