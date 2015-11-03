﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using IDAL;
using System.Drawing;

namespace WMS.WebUI.OutStock
{

    public partial class OutStockTasks : App_Code.BasePage
    {
        private string Filter = "BillID like 'OS%' and State in (1,2,3)";
        protected void Page_Load(object sender, EventArgs e)
        {

            this.GridView2.PageSize = pageSubSize;
            if (!IsPostBack)
            {
                ViewState["filter"] = Filter;
                ViewState["CurrentPage"] = 1;

                try
                {
                   DataTable dt= SetBtnEnabled(int.Parse(ViewState["CurrentPage"].ToString()),SqlCmd, ViewState["filter"].ToString(), pageSize, GridView1, btnFirst, btnPre, btnNext, btnLast, btnToPage, lblCurrentPage, this.UpdatePanel1);
                   SetBindDataSub(dt);
                }
                catch (Exception exp)
                {
                    WMS.App_Code.JScript.Instance.ShowMessage(this.UpdatePanel1, exp.Message);
                }
               
            }
            writeJsvar(FormID, SqlCmd, "");
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.UpdatePanel1.GetType(), "Resize", "resize();", true);


        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.RowIndex == Convert.ToInt32(this.hdnRowIndex.Value))
                {
                    e.Row.BackColor = ColorTranslator.FromHtml("#60c0ff");
                }
                e.Row.Attributes.Add("onclick", string.Format("$('#hdnRowValue').val('{1}');selectRow({0});", e.Row.RowIndex, e.Row.Cells[1].Text));
                e.Row.Attributes.Add("style", "cursor:pointer;");
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {

            try
            {
                ViewState["filter"] = Filter + " and " + string.Format("{0} like '%{1}%'", this.ddlField.SelectedValue, this.txtSearch.Text.Trim().Replace("'", ""));
                ViewState["CurrentPage"] = 1;
                DataTable dt = SetBtnEnabled(int.Parse(ViewState["CurrentPage"].ToString()), SqlCmd, ViewState["filter"].ToString(), pageSize, GridView1, btnFirst, btnPre, btnNext, btnLast, btnToPage, lblCurrentPage, this.UpdatePanel1);
                SetBindDataSub(dt);
            }
            catch (Exception exp)
            {
                WMS.App_Code.JScript.Instance.ShowMessage(this.UpdatePanel1, exp.Message);
            }
        }

      
        #region 主档事件
        protected void btnFirst_Click(object sender, EventArgs e)
        {
            ViewState["CurrentPage"] = 1;
            this.hdnRowIndex.Value = "0";
           DataTable dt= SetBtnEnabled(int.Parse(ViewState["CurrentPage"].ToString()), SqlCmd, ViewState["filter"].ToString(), pageSize, GridView1, btnFirst, btnPre, btnNext, btnLast, btnToPage, lblCurrentPage, this.UpdatePanel1);
           SetBindDataSub(dt);
        }
      

        protected void btnPre_Click(object sender, EventArgs e)
        {
            ViewState["CurrentPage"] = int.Parse(ViewState["CurrentPage"].ToString()) - 1;
            this.hdnRowIndex.Value = "0";
           DataTable dt= SetBtnEnabled(int.Parse(ViewState["CurrentPage"].ToString()), SqlCmd, ViewState["filter"].ToString(), pageSize, GridView1, btnFirst, btnPre, btnNext, btnLast, btnToPage, lblCurrentPage, this.UpdatePanel1);
           SetBindDataSub(dt);
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            ViewState["CurrentPage"] = int.Parse(ViewState["CurrentPage"].ToString()) + 1;
            this.hdnRowIndex.Value = "0";
           DataTable dt= SetBtnEnabled(int.Parse(ViewState["CurrentPage"].ToString()), SqlCmd, ViewState["filter"].ToString(), pageSize, GridView1, btnFirst, btnPre, btnNext, btnLast, btnToPage, lblCurrentPage, this.UpdatePanel1);
           SetBindDataSub(dt);
        }

        protected void btnLast_Click(object sender, EventArgs e)
        {
            ViewState["CurrentPage"] = 0;
            this.hdnRowIndex.Value = "0";
            DataTable dt= SetBtnEnabled(int.Parse(ViewState["CurrentPage"].ToString()), SqlCmd, ViewState["filter"].ToString(), pageSize, GridView1, btnFirst, btnPre, btnNext, btnLast, btnToPage, lblCurrentPage, this.UpdatePanel1);
            SetBindDataSub(dt);
        }

        protected void btnToPage_Click(object sender, EventArgs e)
        {
            int PageIndex = 0;
            int.TryParse(this.txtPageNo.Text, out PageIndex);
            if (PageIndex == 0)
                PageIndex = 1;

            ViewState["CurrentPage"] = PageIndex;
            this.hdnRowIndex.Value = "0";
            DataTable dt = SetBtnEnabled(int.Parse(ViewState["CurrentPage"].ToString()), SqlCmd, ViewState["filter"].ToString(), pageSize, GridView1, btnFirst, btnPre, btnNext, btnLast, btnToPage, lblCurrentPage, this.UpdatePanel1);
            SetBindDataSub(dt);
        }

        private void SetBindDataSub(DataTable dt)
        {
            string BillID = "";
            if (dt.Rows.Count > 0)
            {
                BillID = dt.Rows[0]["BillID"].ToString();
                hdnRowValue.Value = dt.Rows[0]["BillID"].ToString();
            }
            BindDataSub(BillID);
        }
        public override void BindDataSub(string BillID)
        {
            BLL.BLLBase bll = new BLL.BLLBase();
            DataTable dtSub = bll.FillDataTable("WMS.SelectBillTask", new DataParameter[] { new DataParameter("{0}", string.Format("BillID='{0}'", BillID)) });
            Session[FormID + "_S_GridView2"] = dtSub;
            this.GridView2.DataSource = dtSub;
            this.GridView2.DataBind();
            MovePage("S", this.GridView2, 0, btnFirstSub1, btnPreSub1, btnNextSub1, btnLastSub1, btnToPageSub1, lblCurrentPageSub1);
        }
       
     


        protected void btnReload_Click(object sender, EventArgs e)
        {
            BtnReloadSub(Convert.ToInt32(this.hdnRowIndex.Value), this.hdnRowValue.Value, this.GridView1);
        }
        #endregion

        #region 子表绑定

        protected void btnFirstSub1_Click(object sender, EventArgs e)
        {
            MovePage("S", this.GridView2, 0, btnFirstSub1, btnPreSub1, btnNextSub1, btnLastSub1, btnToPageSub1, lblCurrentPageSub1);
        }

        protected void btnPreSub1_Click(object sender, EventArgs e)
        {
            MovePage("S", this.GridView2, this.GridView2.PageIndex - 1, btnFirstSub1, btnPreSub1, btnNextSub1, btnLastSub1, btnToPageSub1, lblCurrentPageSub1);
        }

        protected void btnNextSub1_Click(object sender, EventArgs e)
        {
            MovePage("S", this.GridView2, this.GridView2.PageIndex + 1, btnFirstSub1, btnPreSub1, btnNextSub1, btnLastSub1, btnToPageSub1, lblCurrentPageSub1);
        }

        protected void btnLastSub1_Click(object sender, EventArgs e)
        {
            MovePage("S", this.GridView2, this.GridView2.PageCount - 1, btnFirstSub1, btnPreSub1, btnNextSub1, btnLastSub1, btnToPageSub1, lblCurrentPageSub1);
        }

        protected void btnToPageSub1_Click(object sender, EventArgs e)
        {
            MovePage("S", this.GridView2, int.Parse(this.txtPageNoSub1.Text) - 1, btnFirstSub1, btnPreSub1, btnNextSub1, btnLastSub1, btnToPageSub1, lblCurrentPageSub1);
        }



        #endregion

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string strColorCode = "'-1',";
            BLL.BLLBase bll = new BLL.BLLBase();
            for (int i = 0; i < this.GridView1.Rows.Count; i++)
            {
                CheckBox cb = (CheckBox)(this.GridView1.Rows[i].FindControl("cbSelect"));
                if (cb != null && cb.Checked)
                {
                    string hk = this.GridView1.Rows[i].Cells[1].Text;
                    //判断能否删除
                    int State = int.Parse(bll.GetFieldValue("WMS_BillMaster", "State", string.Format("BillID='{0}'", hk)));
                    if (State == 0)
                    {
                        WMS.App_Code.JScript.Instance.ShowMessage(this.UpdatePanel1, GridView1.Rows[i].Cells[2].Text + "单号还未审核不能作业，请审核后，再进行出库作业。");
                        BindDataSub(this.hdnRowValue.Value);
                        return;
                    }
                    if (State > 1)
                    {
                        WMS.App_Code.JScript.Instance.ShowMessage(this.UpdatePanel1, GridView1.Rows[i].Cells[2].Text + "单号已经作业，不能再进行出库作业。");
                        BindDataSub(this.hdnRowValue.Value);
                        return;
                    }

                    strColorCode += "'" + hk + "',";
                }
            }

            strColorCode += "'-1'";
            if (strColorCode.Replace("'-1','-1'", "").Trim().Length == 0)
            {
                WMS.App_Code.JScript.Instance.ShowMessage(this.UpdatePanel1,  "请选择单据！");
                SetGridViewEmptyRow(GridView1, (DataTable)ViewState[FormID + "_MainFormData"]);
                BindDataSub(this.hdnRowValue.Value);
                return;
            }
            try
            {

                bll.ExecNonQueryTran("WMS.SpOutStockTask", new DataParameter[] { new DataParameter("@strWhere", strColorCode), new DataParameter("@UserName", Session["EmployeeCode"].ToString()) });

                AddOperateLog("出库单", "出库作业单号：" + strColorCode.Replace("'-1',", "").Replace(",'-1'", ""));
                DataTable dt = SetBtnEnabled(int.Parse(ViewState["CurrentPage"].ToString()), SqlCmd, ViewState["filter"].ToString(), pageSize, GridView1, btnFirst, btnPre, btnNext, btnLast, btnToPage, lblCurrentPage, this.UpdatePanel1);
                SetBindDataSub(dt);
            }
            catch (Exception ex)
            {
                BindDataSub(this.hdnRowValue.Value);
                WMS.App_Code.JScript.Instance.ShowMessage(this.UpdatePanel1, ex.Message);
            }

        }

        protected void btnDeletet_Click(object sender, EventArgs e)
        {
            string strColorCode = "'-1',";
            BLL.BLLBase bll = new BLL.BLLBase();
            for (int i = 0; i < this.GridView1.Rows.Count; i++)
            {
                CheckBox cb = (CheckBox)(this.GridView1.Rows[i].FindControl("cbSelect"));
                if (cb != null && cb.Checked)
                {
                    string hk =  this.GridView1.Rows[i].Cells[1].Text;
                    //判断能否删除
                    int State = int.Parse(bll.GetFieldValue("WMS_BillMaster", "State", string.Format("BillID='{0}'", hk )));
                    if (State<2)
                    {
                        WMS.App_Code.JScript.Instance.ShowMessage(this.UpdatePanel1, GridView1.Rows[i].Cells[1].Text + "单号还未作业，不能进行取消作业。");
                        BindDataSub(this.hdnRowValue.Value);
                        return;
                    }
                    if (State > 2)
                    {
                        WMS.App_Code.JScript.Instance.ShowMessage(this.UpdatePanel1, GridView1.Rows[i].Cells[1].Text + "单号已经执行，不能再进行取消作业。");
                        BindDataSub(this.hdnRowValue.Value);
                        return;
                    }

                    strColorCode += "'" + hk + "',";
                }
            }
            strColorCode += "'-1'";
            if (strColorCode.Replace("'-1','-1'", "").Trim().Length == 0)
            {
                WMS.App_Code.JScript.Instance.ShowMessage(this.UpdatePanel1, "请选择单据！");
                SetGridViewEmptyRow(GridView1, (DataTable)ViewState[FormID + "_MainFormData"]);
                BindDataSub(this.hdnRowValue.Value);
                return;
            }

            bll.ExecNonQueryTran("WMS.SpCancelOutStockTask", new DataParameter[] { new DataParameter("@strWhere", strColorCode), new DataParameter("@UserName", Session["EmployeeCode"].ToString()) });

            AddOperateLog("出库单", "出库取消作业单号：" + strColorCode.Replace("'-1',", "").Replace(",'-1'", ""));
            DataTable dt = SetBtnEnabled(int.Parse(ViewState["CurrentPage"].ToString()), SqlCmd, ViewState["filter"].ToString(), pageSize, GridView1, btnFirst, btnPre, btnNext, btnLast, btnToPage, lblCurrentPage, this.UpdatePanel1);
            SetBindDataSub(dt);
        }
    }
}