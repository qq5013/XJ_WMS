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
using System.Text;
using IDAL;

namespace WMS.WebUI.SysInfo.RoleManage
{
    public partial class RoleSet : System.Web.UI.Page
    {
        BLL.BLLBase bll = new BLL.BLLBase();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitSmartTree();
                if (Request.QueryString["GroupID"] != null)
                {
                    ViewState["GroupID"] = Request.QueryString["GroupID"].ToString();
                    string GroupName = Server.UrlDecode(Request.QueryString["GroupName"].ToString());
                    this.lbTitle.Text = "用户组<font color='Gray'>" + GroupName + "</font>权限设置";
                    GroupOperationBind();
                    if (GroupName == "admin")
                    {
                        this.lnkBtnSave.Enabled = false;
                    }
                }
            }
            if (ViewState["GroupID"] != null)
            {
                //JScript.Instance.RegisterScript(this, "document.getElementById('divOp').style.display='block';");
                this.lnkBtnSave.Visible = true;
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Resize", "resize();", true);
        }

        #region SmartTreeView

        public void InitSmartTree()
        {
            this.sTreeModule.Nodes.Clear();
            DataTable dtModules = bll.FillDataTable("Security.SelectSystemModules", new DataParameter[] { new DataParameter("@SystemName", "WMS") });
            DataTable dtSubModules = bll.FillDataTable("Security.SelectSystemSubModules", new DataParameter[] { new DataParameter("@SystemName", "WMS") }); 
            DataTable dtOperations = bll.FillDataTable("Security.SelectSystemOperations", new DataParameter[] { new DataParameter("@SystemName", "WMS") }); 
            foreach (DataRow dr in dtModules.Rows)
            {
                TreeNode tnRoot = new TreeNode(dr["MenuTitle"].ToString(), dr["ModuleCode"].ToString().Trim());
                tnRoot.SelectAction = TreeNodeSelectAction.Expand;
                tnRoot.ShowCheckBox = true;
                this.sTreeModule.Nodes.Add(tnRoot);
            }
            //为第一级菜单增加子级菜单


            if (dtModules.Rows.Count > 0)
            {
                foreach (DataRow drSub in dtSubModules.Rows)
                {
                    for (int i = 0; i < sTreeModule.Nodes.Count; i++)
                    {
                        if (sTreeModule.Nodes[i].Value == drSub["ModuleCode"].ToString().Trim())
                        {
                            TreeNode tnChild = new TreeNode(drSub["MenuTitle"].ToString(), drSub["SubModuleCode"].ToString().Trim());
                            tnChild.ShowCheckBox = true;
                            tnChild.SelectAction = TreeNodeSelectAction.Expand;
                            this.sTreeModule.Nodes[i].ChildNodes.Add(tnChild);
                            break;
                        }

                    }
                }
            }

            foreach (DataRow drOp in dtOperations.Rows)
            {
                for (int i = 0; i < sTreeModule.Nodes.Count; i++)
                {
                    for (int j = 0; j < sTreeModule.Nodes[i].ChildNodes.Count; j++)
                    {
                        if (sTreeModule.Nodes[i].ChildNodes[j].Value == drOp["SubModuleCode"].ToString().Trim())
                        {
                            TreeNode tnOp = new TreeNode(drOp["OperatorDescription"].ToString(), drOp["ModuleID"].ToString());
                            tnOp.ShowCheckBox = true;
                            tnOp.SelectAction = TreeNodeSelectAction.None;
                            sTreeModule.Nodes[i].ChildNodes[j].ChildNodes.Add(tnOp);
                        }
                    }
                }
            }

        }
        #endregion


        protected void lnkBtnSave_Click(object sender, EventArgs e)
        {
            string GroupID = ViewState["GroupID"].ToString();
            int iGroupID = int.Parse(ViewState["GroupID"].ToString());



            bll.ExecNonQuery("Security.DeleteGroupOperation", new DataParameter[] { new DataParameter("@GroupID", iGroupID), new DataParameter("@SystemName", "WMS") });

            foreach (TreeNode tnRoot in this.sTreeModule.Nodes)
            {
                foreach (TreeNode tnSub in tnRoot.ChildNodes)
                {
                    foreach (TreeNode tnOp in tnSub.ChildNodes)
                    {
                        if (tnOp.Checked)
                        {

                            string ModuleID = tnOp.Value;
                            bll.ExecNonQuery("Security.InsertGroupOperation", new DataParameter[] { new DataParameter("@GroupID", iGroupID), new DataParameter("@ModuleID", ModuleID) });

                        }
                    }
                }
            }

            WMS.App_Code.JScript.Instance.ShowMessage(this, "保存成功！");
            GroupOperationBind();


        }

        public void GroupOperationBind()
        {
            DataTable dtOP = bll.FillDataTable("Security.SelectGroupOperation", new DataParameter[] { new DataParameter("@GroupID",ViewState["GroupID"].ToString()) });  
            if (dtOP.Rows.Count > 0)
            {
                foreach (DataRow dr in dtOP.Rows)
                {
                    foreach (TreeNode tnRoot in this.sTreeModule.Nodes)
                    {
                        bool IsAllSelected = true;
                        foreach (TreeNode tnSub in tnRoot.ChildNodes)
                        {
                            bool IsSubAllSelected = true;
                            foreach (TreeNode tnOp in tnSub.ChildNodes)
                            {
                                if (tnOp.Value == dr["ModuleID"].ToString())
                                {
                                    tnOp.Checked = true;
                                    break;
                                }
                                if (!tnOp.Checked)
                                {
                                    IsSubAllSelected = false;
                                }
                            }
                            if (IsSubAllSelected)
                            {
                                tnSub.Checked = true;
                            }
                            else
                            {
                                IsAllSelected = false;
                            }
                        }
                        if (IsAllSelected)
                        {
                            tnRoot.Checked = true;
                        }
                    }
                }

            }
        }

        protected void lnkBtnCollapse_Click(object sender, EventArgs e)
        {
            //this.tvModuleList.CollapseAll();
            this.sTreeModule.CollapseAll();
        }
        protected void lnkBtnExpand_Click(object sender, EventArgs e)
        {
            //this.tvModuleList.ExpandAll();
            this.sTreeModule.ExpandAll();
        }

        protected void tvModuleList_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
        {
            //Response.Write(e.Node.Value);
        }
        protected void tvModuleList_TreeNodeCheckChanged1(object sender, TreeNodeEventArgs e)
        {
            //JScript.Instance.ShowMessage(this, e.Node.Value);
        }
        protected void tvModuleList_SelectedNodeChanged(object sender, EventArgs e)
        {
            //JScript.Instance.ShowMessage(this, "hi");
        }
        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            //JScript.Instance.ShowMessage(this, tvModuleList.Nodes[0].ChildNodes[0].ChildNodes[0].ValuePath);

        }
    }
}