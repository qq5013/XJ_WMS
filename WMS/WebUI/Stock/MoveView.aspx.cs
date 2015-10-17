﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using IDAL;


namespace WMS.WebUI.Stock
{
    public partial class MoveView :App_Code.BasePage
    {
        private string strID;
        private string TableName = "WMS_BillMaster";
        private string PrimaryKey = "BillID";
        BLL.BLLBase bll = new BLL.BLLBase();
        private string Filter = "BillID like 'MS%'";
        protected void Page_Load(object sender, EventArgs e)
        {
            strID = Request.QueryString["ID"] + "";
            this.dgViewSub1.PageSize = pageSubSize;
            if (!IsPostBack)
            {
                BindDropDownList();
                DataTable dt = bll.FillDataTable("WMS.SelectBillMaster", new DataParameter[] { new DataParameter("{0}", string.Format("BillID='{0}'", strID)) });
                BindData(dt);
              
            }
           
            ScriptManager.RegisterStartupScript(this.updatePanel, this.updatePanel.GetType(), "Resize", "resize();", true);
            writeJsvar(FormID, SqlCmd, strID);
        }

        #region 绑定方法
        private void BindDropDownList()
        {
            DataTable dtArea = bll.FillDataTable("Cmd.SelectArea");
            this.ddlAreaCode.DataValueField = "AreaCode";
            this.ddlAreaCode.DataTextField = "AreaName";
            this.ddlAreaCode.DataSource = dtArea;
            this.ddlAreaCode.DataBind();
        }


        private void BindData(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                this.txtID.Text = dt.Rows[0]["BillID"].ToString();
                this.txtBillDate.Text =ToYMD(dt.Rows[0]["BillDate"]);
                this.ddlAreaCode.SelectedValue = dt.Rows[0]["AreaCode"].ToString();
               
                this.txtMemo.Text = dt.Rows[0]["Memo"].ToString();
                this.txtCreator.Text = dt.Rows[0]["Creator"].ToString();
                this.txtCreatDate.Text = ToYMD(dt.Rows[0]["CreateDate"]);
                this.txtUpdater.Text = dt.Rows[0]["Updater"].ToString();
                this.txtUpdateDate.Text = ToYMD(dt.Rows[0]["UpdateDate"]);
                this.txtChecker.Text = dt.Rows[0]["Checker"].ToString();
                this.txtCheckDate.Text = ToYMD(dt.Rows[0]["CheckDate"]);
                hdnState.Value = dt.Rows[0]["State"].ToString();
                if (this.txtChecker.Text.Trim()!="")
                {
                    this.btnCheck.Text = "反审";
                    this.btnCheck.CssClass = "ButtonAudit2";
                }
                else
                {
                    this.btnCheck.Text = "审核";
                    this.btnCheck.CssClass = "ButtonAudit";
                }

            }
            BindDataSub();
            SetPermission();
        }
        /// <summary>
        /// 設定權限
        /// </summary>
        private void SetPermission()
        {

            bool blnDelete = false;
            bool blnEdit = false;
              bool blnCheck=false;
            DataTable dtOP = (DataTable)(Session["DT_UserOperation"]);
            DataRow[] drs = dtOP.Select(string.Format("SubModuleCode='{0}'", Session["SubModuleCode"].ToString()));

            foreach (DataRow dr in drs)
            {
                int op = int.Parse(dr["OperatorCode"].ToString());
                switch (op)
                {
                    case 1:
                        blnDelete = true;
                        break;
                    case 2: //修改
                        blnEdit = true;
                        break;
                    case 5:
                        blnCheck=true;
                        break;
                }
            }
            this.btnDelete.Enabled = blnDelete;
            this.btnEdit.Enabled = blnEdit;
            this.btnCheck.Enabled = blnCheck;


            int State = int.Parse(hdnState.Value);
            if (State == 1)
            {
                this.btnDelete.Enabled = false;
                this.btnEdit.Enabled = false;
            }
            if (State > 1)
            {
                this.btnDelete.Enabled = false;
                this.btnEdit.Enabled = false;
                this.btnCheck.Enabled = false;
            }


        }
        private void BindDataSub()
        {
            DataTable dt = bll.FillDataTable("WMS.SelectBillDetail", new DataParameter[] { new DataParameter("{0}", string.Format("BillID='{0}'", this.txtID.Text)) });
            Session[FormID + "_View_dgViewSub1"] = dt;
            this.dgViewSub1.DataSource = dt;
            this.dgViewSub1.DataBind();
            object o = dt.Compute("SUM(Quantity)", "");
            this.txtTotalQty.Text = o.ToString();
            MovePage("View", this.dgViewSub1, 0, btnFirstSub1, btnPreSub1, btnNextSub1, btnLastSub1, btnToPageSub1, lblCurrentPageSub1);

        }
        private void BindDataNull()
        {
            this.txtID.Text = "";
            this.txtBillDate.Text = "";
            this.txtMemo.Text = "";
            this.txtCreator.Text = "";
            this.txtCreatDate.Text = "";
            this.txtUpdater.Text = "";
            this.txtUpdateDate.Text = "";
            this.txtChecker.Text = "";
            this.txtCheckDate.Text = "";
            hdnState.Value = "0";
        }
        #endregion
        
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string strID = this.txtID.Text;
            int Count = bll.GetRowCount("VUsed_WMS_BillMaster", string.Format("BillID='{0}'", this.txtID.Text.Trim()));
            if (Count > 0)
            {
                WMS.App_Code.JScript.Instance.ShowMessage(this.updatePanel, "该移库单号已被其它单据使用，请调整后再删除！");
                return;
            }
 
            string[] comds = new string[2];
            comds[0] = "WMS.DeleteBillMaster";
            comds[1] = "WMS.DeleteBillDetail";
            List<DataParameter[]> paras = new List<DataParameter[]>();
            paras.Add(new DataParameter[] { new DataParameter("{0}", "'" + strID + "'") });
            paras.Add(new DataParameter[] { new DataParameter("{0}", string.Format("BillID='{0}'", strID)) });
            bll.ExecTran(comds, paras);
           
            AddOperateLog("移库单", "删除单号：" + strID);

            btnNext_Click(sender, e);
            if (this.txtID.Text == strID)
            {
                btnPre_Click(sender, e);
                if (this.txtID.Text == strID)
                {
                    BindDataNull();
                }
            }

        }

        protected void dgViewSub1_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        #region 上下笔事件
        protected void btnFirst_Click(object sender, EventArgs e)
        {
            BindData(bll.GetRecord("F", TableName, Filter, PrimaryKey, this.txtID.Text));
        }
        protected void btnPre_Click(object sender, EventArgs e)
        {
            BindData(bll.GetRecord("P", TableName, Filter, PrimaryKey, this.txtID.Text));
        }
        protected void btnNext_Click(object sender, EventArgs e)
        {
            BindData(bll.GetRecord("N", TableName, Filter, PrimaryKey, this.txtID.Text));
        }
        protected void btnLast_Click(object sender, EventArgs e)
        {
            BindData(bll.GetRecord("L", TableName, Filter, PrimaryKey, this.txtID.Text));
        }
        #endregion

        #region 子表绑定

        protected void btnFirstSub1_Click(object sender, EventArgs e)
        {
            MovePage("View", this.dgViewSub1, 0, btnFirstSub1, btnPreSub1, btnNextSub1, btnLastSub1, btnToPageSub1, lblCurrentPageSub1);
        }

        protected void btnPreSub1_Click(object sender, EventArgs e)
        {
            MovePage("View", this.dgViewSub1, this.dgViewSub1.PageIndex - 1, btnFirstSub1, btnPreSub1, btnNextSub1, btnLastSub1, btnToPageSub1, lblCurrentPageSub1);
        }

        protected void btnNextSub1_Click(object sender, EventArgs e)
        {
            MovePage("View", this.dgViewSub1, this.dgViewSub1.PageIndex + 1, btnFirstSub1, btnPreSub1, btnNextSub1, btnLastSub1, btnToPageSub1, lblCurrentPageSub1);
        }

        protected void btnLastSub1_Click(object sender, EventArgs e)
        {
            MovePage("View", this.dgViewSub1, this.dgViewSub1.PageCount - 1, btnFirstSub1, btnPreSub1, btnNextSub1, btnLastSub1, btnToPageSub1, lblCurrentPageSub1);
        }

        protected void btnToPageSub1_Click(object sender, EventArgs e)
        {
            MovePage("View", this.dgViewSub1, int.Parse(this.txtPageNoSub1.Text) - 1, btnFirstSub1, btnPreSub1, btnNextSub1, btnLastSub1, btnToPageSub1, lblCurrentPageSub1);
        }



        #endregion

        protected void btnCheck_Click(object sender, EventArgs e)
        {

            List<string> Comd = new List<string>();
            Comd.Insert(0, "WMS.UpdateMoveCellLock");
            Comd.Insert(1, "WMS.UpdateCellLock");
            Comd.Insert(2, "WMS.UpdateCheckBillMaster");
            List<DataParameter[]> paras = new List<DataParameter[]>();
            if (this.btnCheck.Text == "审核")
            {
                DataTable dtSub = (DataTable)Session[FormID + "_View_dgViewSub1"];
                for (int i = 0; i < dtSub.Rows.Count; i++)
                {
                    int count = 0;
                    count = bll.GetRowCount("Cmd_Cell", string.Format("CellCode='{0}' and (IsLock=1 or ProductCode!='{1}')", dtSub.Rows[i]["CellCode"], dtSub.Rows[i]["ProductCode"]));
                    if (count > 0)
                    {
                        WMS.App_Code.JScript.Instance.ShowMessage(this.updatePanel, "货位 " + dtSub.Rows[i]["CellCode"].ToString() + "已经被其它单据锁定，不能移库！");
                        return;
                    }
                    count = bll.GetRowCount("Cmd_Cell", string.Format("CellCode='{0}' and (IsLock=1 or IsActive=0)", dtSub.Rows[i]["NewCellCode"]));
                    if (count > 0)
                    {
                        WMS.App_Code.JScript.Instance.ShowMessage(this.updatePanel, "货位 " + dtSub.Rows[i]["NewCellCode"].ToString() + "已经被其它单据锁定，不能移库！");
                        return;
                    }
                }
                paras.Insert(0, new DataParameter[] { new DataParameter("@Lock", 1), new DataParameter("{0}", string.Format("BillID='{0}'", this.txtID.Text.Trim())) });
                paras.Insert(1, new DataParameter[] { new DataParameter("@Lock", 1), new DataParameter("{0}", string.Format("BillID='{0}'", this.txtID.Text.Trim())) });

                paras.Insert(2, new DataParameter[]{ new DataParameter("@Checker", Session["EmployeeCode"].ToString()), new DataParameter("{0}", "getdate()"),
                 new DataParameter("@State", 1), new DataParameter("@BillID", this.txtID.Text)});
            }
            else
            {
                int State = int.Parse(bll.GetFieldValue("WMS_BillMaster", "State", string.Format("BillID='{0}'", this.txtID.Text)));
                if (State > 1)
                {
                    WMS.App_Code.JScript.Instance.ShowMessage(this.updatePanel, this.txtID.Text + " 单号已经作业，不能进行反审。");
                    return;
                }
                paras.Insert(0, new DataParameter[] { new DataParameter("@Lock", 0), new DataParameter("{0}", string.Format("BillID='{0}'", this.txtID.Text.Trim())) });
                paras.Insert(1, new DataParameter[] { new DataParameter("@Lock", 0), new DataParameter("{0}", string.Format("BillID='{0}'", this.txtID.Text.Trim())) });
                paras.Insert(2, new DataParameter[]{ new DataParameter("@Checker", ""), new DataParameter("{0}", "null"),
                 new DataParameter("@State", 0), new DataParameter("@BillID", this.txtID.Text)});
            }

            bll.ExecTran(Comd.ToArray(), paras);
           
            AddOperateLog("移库单 ", btnCheck.Text + " " + txtID.Text);

            DataTable dt = bll.FillDataTable("WMS.SelectBillMaster", new DataParameter[] { new DataParameter("{0}", string.Format("BillID='{0}'", strID)) });
            BindData(dt);
        }
    }
}