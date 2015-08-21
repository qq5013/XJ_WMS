﻿using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Text;
using IDAL;

namespace WMS.WebUI.SysInfo.ConfigPlan
{
    public partial class ConfigPlan : System.Web.UI.Page
    {
        public string strImagePath;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                InitSmartTree();
                QuickDestopBind();
            }
        }

        public void InitSmartTree()
        {
            this.sTreeModule.Nodes.Clear();
            try
            {
                string strUserName = Session["G_User"].ToString();
                BLL.BLLBase bll = new BLL.BLLBase();
                DataTable dtModules = bll.FillDataTable("Security.SelectUserOperateModule", new DataParameter[] { new DataParameter("@UserName", strUserName) });
                DataTable dtSubModules = bll.FillDataTable("Security.SelectUserOperateSubModule", new DataParameter[] { new DataParameter("@UserName", strUserName) });

                foreach (DataRow dr in dtModules.Rows)
                {
                    TreeNode tnRoot = new TreeNode(dr["MenuTitle"].ToString(), dr["ID"].ToString());
                    tnRoot.SelectAction = TreeNodeSelectAction.Expand;
                    tnRoot.ShowCheckBox = true;
                    this.sTreeModule.Nodes.Add(tnRoot);
                }

                if (dtModules.Rows.Count > 0)
                {
                    foreach (DataRow drSub in dtSubModules.Rows)
                    {
                        for (int i = 0; i < sTreeModule.Nodes.Count; i++)
                        {
                            if (sTreeModule.Nodes[i].Text == drSub["MenuParent"].ToString())
                            {
                                TreeNode tnChild = new TreeNode(drSub["MenuTitle"].ToString(), drSub["ID"].ToString());
                                tnChild.ShowCheckBox = true;
                                tnChild.SelectAction = TreeNodeSelectAction.Expand;
                                this.sTreeModule.Nodes[i].ChildNodes.Add(tnChild);
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Session["ModuleName"] = "浏览公共模块";
                Session["FunctionName"] = "Page_Load";
                Session["ExceptionalType"] = e.GetType().FullName;
                Session["ExceptionalDescription"] = e.Message;
                Response.Redirect("~/Common/MistakesPage.aspx");
            }
        }
        private void QuickDestopBind()
        {
            BLL.BLLBase bll = new BLL.BLLBase();
            DataTable dtOP = bll.FillDataTable("Security.SelectUserQuickDesktop", new DataParameter[] { new DataParameter("@UserID", Convert.ToInt32(Session["UserID"].ToString())) });
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

                            if (tnSub.Value == dr["ModuleID"].ToString())
                            {
                                tnSub.Checked = true;
                                break;
                            }
                            if (!tnSub.Checked)
                            {
                                IsSubAllSelected = false;
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


        protected void btnSave_Click(object sender, EventArgs e)
        {
            BLL.BLLBase bll = new BLL.BLLBase();
            bll.ExecNonQuery("Security.DeleteQuickDestop", new DataParameter[] { new DataParameter("@UserID", Convert.ToInt32(Session["UserID"].ToString())) });

            foreach (TreeNode tnRoot in this.sTreeModule.Nodes)
            {
                foreach (TreeNode tnSub in tnRoot.ChildNodes)
                {
                    if (tnSub.Checked)
                    {
                        string strModuleID = tnSub.Value;

                        bll.ExecNonQuery("Security.InsertQuickDestop", new DataParameter[] { new DataParameter("@UserID", Convert.ToInt32(Session["UserID"].ToString())), new DataParameter("@ModuleID", strModuleID) });
                    }
                }
            }

            WMS.App_Code.JScript.Instance.ShowMessage(this, "设置成功！");

        }
    }
}