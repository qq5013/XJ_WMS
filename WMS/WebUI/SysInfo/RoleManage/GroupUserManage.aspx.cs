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
    public partial class GroupUserManage : System.Web.UI.Page
    {
        string GroupID = "";
        string GroupName = "";
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["GroupID"] != null)
            {
                GroupID = Request.QueryString["GroupID"].ToString();
                GroupName = Request.QueryString["GroupName"].ToString();
                this.Label1.Text = "用户组" + GroupName + "成员设置";
                BLL.BLLBase bll = new BLL.BLLBase();

                this.dgUser.DataSource = bll.FillDataTable("Security.SelectAllUser", null);
                this.dgUser.DataBind();
                this.Title = "用户组" + GroupName + "成员设置";
            }
        }
        protected void dgUser_ItemDataBound(object sender, DataGridItemEventArgs e)
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
           
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                CheckBox chk = new CheckBox();
                chk.Text = "加入" + GroupName;
                e.Item.Cells[2].Controls.Add(chk);
                if (e.Item.Cells[1].Text.Replace("&bsp;", "").Trim() == GroupName.Trim())
                {
                    chk.Checked = true;
                }
            }
            e.Item.Cells[3].Visible = false;
        }

        /// <summary>
        /// 保存当前组用户

        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string users = "-1,";
                foreach (DataGridItem item in dgUser.Items)
                {
                    if (item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Item)
                    {
                        CheckBox chk = (CheckBox)item.Cells[2].Controls[1];
                        if (chk.Checked)
                        {
                            users += item.Cells[3].Text + ",";
                        }
                    }
                }
                users += "-1";

                BLL.BLLBase bll = new BLL.BLLBase();
                bll.ExecNonQuery("Security.UpdateUserGroup", new DataParameter[] { new DataParameter("@GroupID", GroupID), new DataParameter("{0}", users) });


                WMS.App_Code.JScript.Instance.ShowMessage(this, "添加成功！");
            }
            catch(Exception ex)
            {
                System.Diagnostics.StackFrame frame = new System.Diagnostics.StackFrame(0);
                Session["ModuleName"] = this.Page.Title;
                Session["FunctionName"] = frame.GetMethod().Name;
                Session["ExceptionalType"] = ex.GetType().FullName;
                Session["ExceptionalDescription"] = ex.Message;
                Response.Redirect("../../../Common/MistakesPage.aspx");

            }
            

        }

        protected void dgUser_ItemCreated(object sender, DataGridItemEventArgs e)
        {
           
        }
    }
}