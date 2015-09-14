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
using System.Drawing;

namespace WMS.WebUI.SysInfo.RoleManage
{
    public partial class GroupUserList : System.Web.UI.Page
    {
        BLL.BLLBase bll = new BLL.BLLBase();
         
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Request.QueryString["GroupID"] != null)
            {
                ViewState["GroupID"] = Request.QueryString["GroupID"].ToString();
                if (this.Request.QueryString["GroupName"] != null)
                {
                    this.Label1.Text = "用户组 <font color='Gray'>" + this.Request.QueryString["GroupName"].ToString() + "</font>  成员";
                }
            }
            else
            {
                ViewState["GroupID"] = "-1";
            }
            GridDataBind();
        }
        protected void dgGroupUser_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            int UserID = Convert.ToInt32(e.Item.Cells[0].Text);
            int rowCount = dgGroupUser.Items.Count;
            bll.ExecNonQuery("Security.UpdateUserGroup", new DataParameter[] { new DataParameter("@GroupID", 0), new DataParameter("{0}", UserID) });


            this.dgGroupUser.DataSource = bll.FillDataTable("Security.SelectGroupUser", new DataParameter[] { new DataParameter("@GroupID", Convert.ToInt32(ViewState["GroupID"].ToString())) });
            if (rowCount == 1)
            {
                if (dgGroupUser.CurrentPageIndex == 0)
                {

                }
                else
                {
                    dgGroupUser.CurrentPageIndex = dgGroupUser.CurrentPageIndex - 1;
                }
            }
            this.dgGroupUser.DataBind();

        }
        protected void dgGroupUser_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            this.dgGroupUser.CurrentPageIndex = e.NewPageIndex;
            GridDataBind();
        }

        public void GridDataBind()
        {
            this.dgGroupUser.DataSource = bll.FillDataTable("Security.SelectGroupUser", new DataParameter[] { new DataParameter("@GroupID", Convert.ToInt32(ViewState["GroupID"].ToString())) });
            this.dgGroupUser.DataBind();
        }
        protected void dgGroupUser_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            DataGridItem row = (DataGridItem)e.Item;
            if (row.ItemIndex % 2 == 0)
            {
                row.BackColor = Color.White;
            }
            else
            {
                row.BackColor = ColorTranslator.FromHtml("#E9F2FF");
            }

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Header)
            {
                e.Item.Cells[0].Visible = false;
            }
        }
    }
}