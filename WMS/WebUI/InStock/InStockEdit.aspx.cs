using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using IDAL;

namespace WMS.WebUI.InStock
{
    public partial class InStockEdit : WMS.App_Code.BasePage
    {
        protected string FormID;
        protected string ID;
        protected string TableName = "WMS_Stock";
        protected string PrimaryKey = "BillID";
        protected int Flag = 1;
       
        BLL.BLLBase bll = new BLL.BLLBase();

        protected void Page_Load(object sender, EventArgs e)
        {
            ID = Request.QueryString["ID"] + "";
            FormID = Request.QueryString["FormID"] + "";
            if (!IsPostBack)
            {
                if (ID != "")
                {
                    DataTable dt = bll.FillDataTable("WMS.SelectInStock", new DataParameter[] { new DataParameter("{0}", string.Format("{0}='{1}' and Flag={2}", PrimaryKey, ID, Flag)) });
                    BindData(dt);
                    SetTextReadOnly(this.txtID);
                    this.txtBillDate.ReadOnly = true;


                }

                SetTextReadOnly(this.txtCreator, this.txtCreateDate, this.txtChecker, this.txtCheckDate, this.txtFactoryName);
                BindPageSize();
                writeJsvar(FormID,SqlCmd, ID);


            }

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "", "BindEvent()", true);

        }
       
        private void BindData(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                this.txtID.Text = dt.Rows[0]["BillID"].ToString();
                this.txtBillDate.Text = ToYMD(dt.Rows[0]["BillDate"]);
                this.txtSourceNo.Text = dt.Rows[0]["ScheduleNo"].ToString();
                this.txtMemo.Text = dt.Rows[0]["Memo"].ToString();
                this.txtCreator.Text = dt.Rows[0]["Creator"].ToString();
                this.txtCreateDate.Text = ToYMD(dt.Rows[0]["CreateDate"]);
                this.txtTaskID.Text = dt.Rows[0]["TaskID"].ToString();
                this.txtChecker.Text = dt.Rows[0]["Checker"].ToString();
                this.txtCheckDate.Text = ToYMD(dt.Rows[0]["CheckDate"]);
                this.txtFactoryName.Text = dt.Rows[0]["FactoryName"].ToString();
                this.txtFactoryOrderNo.Text = dt.Rows[0]["FactoryOrderNo"].ToString();
                this.txtTotalQty.Text = dt.Rows[0]["TotalQty"].ToString();
                if (this.txtSourceNo.Text != "")
                    this.btnFactory.Visible = false;
                else
                    this.btnFactory.Visible = true;
                this.txtFactoryID.Text = dt.Rows[0]["TaskChecker"].ToString();

                BindDataSub();
                BindDataSub2();
            }
        }

        private void BindDataSub()
        {
            DataTable dt = bll.FillDataTable("WMS.SelectInStockSub", new DataParameter[] { new DataParameter("{0}", string.Format("BillID='{0}' and Flag={1}", this.txtID.Text, Flag)) });
            Session[FormID + "_Edit_dgViewSub1"] = dt;

            if (dt.Rows.Count == 0)
            {
                DataTable dtNew = dt.Clone();
                dtNew.Rows.Add(dtNew.NewRow());
                dgViewSub1.DataSource = dtNew;
                dgViewSub1.DataBind();
                int columnCount = dgViewSub1.Rows[0].Cells.Count;
                dgViewSub1.Rows[0].Cells.Clear();
                dgViewSub1.Rows[0].Cells.Add(new TableCell());
                dgViewSub1.Rows[0].Cells[0].ColumnSpan = columnCount;
                dgViewSub1.Rows[0].Cells[0].Text = "没有明细资料，请新增";
                dgViewSub1.Rows[0].Visible = true;
            }
            else
            {
                dgViewSub1.DataSource = dt;
                dgViewSub1.DataBind();
            }
            SetPageSubBtnEnabled();
            
        }
        private void BindDataSub2()
        {
            DataTable dt = bll.FillDataTable("WMS.SelectInStockDetail", new DataParameter[] { new DataParameter("{0}", string.Format("BillID='{0}' and Flag={1}", this.txtID.Text, Flag)) });
            Session[FormID + "_Edit_dgViewSub2"] = dt;

            if (dt.Rows.Count == 0)
            {
                DataTable dtNew = dt.Clone();
                dtNew.Rows.Add(dtNew.NewRow());
                dgViewSub2.DataSource = dtNew;
                dgViewSub2.DataBind();
                int columnCount = dgViewSub2.Rows[0].Cells.Count;
                dgViewSub2.Rows[0].Cells.Clear();
                dgViewSub2.Rows[0].Cells.Add(new TableCell());
                dgViewSub2.Rows[0].Cells[0].ColumnSpan = columnCount;
                dgViewSub2.Rows[0].Cells[0].Text = "没有查询到相应的资料！";
                dgViewSub2.Rows[0].Visible = true;
            }
            else
            {
                dgViewSub2.DataSource = dt;
                dgViewSub2.DataBind();
            }
            SetPageSub2BtnEnabled();

        }

        protected void dgViewSub1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            
        }

       

        protected void btnSave_Click(object sender, EventArgs e)
        {
             
            DataParameter[] para = new DataParameter[] { 
                                              new DataParameter("@TaskChecker",this.txtFactoryID.Text.Trim()),
                                              new DataParameter("@Memo", this.txtMemo.Text.Trim()),
                                              new DataParameter("@TaskID",this.txtTaskID.Text.Trim()),
                                              new DataParameter("@Updater", Session["EmployeeCode"].ToString()),
                                              new DataParameter("{0}",string.Format("Flag={0} and BillID='{1}'",Flag, this.txtID.Text.Trim())) };

            bll.ExecNonQuery("WMS.UpdateInStock", para);

            Response.Redirect(FormID + "View.aspx?SubModuleCode=" + SubModuleCode + "&FormID=" + Server.UrlEncode(FormID) + "&ID=" + Server.UrlEncode(this.txtID.Text));
        }

        #region 从表资料绑定

        protected void ddlPageSizeSub1_SelectedIndexChanged(object sender, EventArgs e)
        {
             
            DataTable dt1 = (DataTable)Session[FormID + "_Edit_dgViewSub1"];
            this.dgViewSub1.DataSource = dt1;
            this.dgViewSub1.PageSize = int.Parse(ddlPageSizeSub1.Text);
            this.dgViewSub1.DataBind();
            SetPageSubBtnEnabled();
        }
        protected void ddlPageSizeSub2_SelectedIndexChanged(object sender, EventArgs e)
        {
             
            DataTable dt1 = (DataTable)Session[FormID + "_Edit_dgViewSub2"];
            this.dgViewSub2.DataSource = dt1;
            this.dgViewSub2.PageSize = int.Parse(ddlPageSizeSub2.Text);
            this.dgViewSub2.DataBind();
            SetPageSub2BtnEnabled();
        }
        private void MovePage(GridView dgv, int pageindex)
        {
            int pindex = pageindex;
            if (pindex < 0) pindex = 0;
            if (pindex >= dgv.PageCount) pindex = dgv.PageCount - 1;
            DataTable dt1 = (DataTable)Session[FormID + "_Edit_" + dgv.ID];
            dgv.PageIndex = pindex;
            dgv.DataSource = dt1;
            dgv.DataBind();

            if (dgv.ID.IndexOf("1") > 1)
                SetPageSubBtnEnabled();
            else
                SetPageSub2BtnEnabled();
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "Resize", "content_resize();", true);

        }

        private void SetPageSubBtnEnabled()
        {
            DataTable dt = (DataTable)Session[FormID + "_Edit_dgViewSub1"];
            if (dt.Rows.Count > 0)
            {
                bool blnvalue = dgViewSub1.PageCount > 1 ? true : false;
                this.btnLastSub1.Enabled = blnvalue;
                this.btnFirstSub1.Enabled = blnvalue;
                this.btnToPageSub1.Enabled = blnvalue;


                if (this.dgViewSub1.PageIndex >= 1)
                    this.btnPreSub1.Enabled = true;
                else
                    this.btnPreSub1.Enabled = false;

                if ((this.dgViewSub1.PageIndex + 1) < dgViewSub1.PageCount)
                    this.btnNextSub1.Enabled = true;
                else
                    this.btnNextSub1.Enabled = false;

                lblCurrentPageSub1.Visible = true;
                lblCurrentPageSub1.Text = "共 [" + dt.Rows.Count.ToString() + "] 条记录  页次 " + (dgViewSub1.PageIndex + 1) + " / " + dgViewSub1.PageCount.ToString();
            }
            else
            {
                this.btnFirstSub1.Enabled = false;
                this.btnPreSub1.Enabled = false;
                this.btnNextSub1.Enabled = false;
                this.btnLastSub1.Enabled = false;
                this.btnToPageSub1.Enabled = false;
                lblCurrentPageSub1.Visible = false;
            }
        }
        private void SetPageSub2BtnEnabled()
        {
            DataTable dt = (DataTable)Session[FormID + "_Edit_dgViewSub2"];
            if (dt.Rows.Count > 0)
            {
                bool blnvalue = dgViewSub2.PageCount > 1 ? true : false;
                this.btnLastSub2.Enabled = blnvalue;
                this.btnFirstSub2.Enabled = blnvalue;
                this.btnToPageSub2.Enabled = blnvalue;


                if (this.dgViewSub2.PageIndex >= 1)
                    this.btnPreSub2.Enabled = true;
                else
                    this.btnPreSub2.Enabled = false;

                if ((this.dgViewSub2.PageIndex + 1) < dgViewSub2.PageCount)
                    this.btnNextSub2.Enabled = true;
                else
                    this.btnNextSub2.Enabled = false;

                lblCurrentPageSub2.Visible = true;
                lblCurrentPageSub2.Text = "共 [" + dt.Rows.Count.ToString() + "] 条记录  页次 " + (dgViewSub2.PageIndex + 1) + " / " + dgViewSub2.PageCount.ToString();
            }
            else
            {
                this.btnFirstSub2.Enabled = false;
                this.btnPreSub2.Enabled = false;
                this.btnNextSub2.Enabled = false;
                this.btnLastSub2.Enabled = false;
                this.btnToPageSub2.Enabled = false;
                lblCurrentPageSub2.Visible = false;
            }
        }

        private void BindPageSize()
        {
            this.ddlPageSizeSub1.Items.Add(new ListItem("10", "10"));
            this.ddlPageSizeSub1.Items.Add(new ListItem("20", "20"));
            this.ddlPageSizeSub1.Items.Add(new ListItem("30", "30"));
            this.ddlPageSizeSub1.Items.Add(new ListItem("40", "40"));
            this.ddlPageSizeSub1.Items.Add(new ListItem("50", "50"));
            this.ddlPageSizeSub1.Items.Add(new ListItem("100", "100"));

            this.ddlPageSizeSub2.Items.Add(new ListItem("10", "10"));
            this.ddlPageSizeSub2.Items.Add(new ListItem("20", "20"));
            this.ddlPageSizeSub2.Items.Add(new ListItem("30", "30"));
            this.ddlPageSizeSub2.Items.Add(new ListItem("40", "40"));
            this.ddlPageSizeSub2.Items.Add(new ListItem("50", "50"));
            this.ddlPageSizeSub2.Items.Add(new ListItem("100", "100"));
        }

        protected void btnFirstSub1_Click(object sender, EventArgs e)
        {
            MovePage(this.dgViewSub1, 0);
        }

        protected void btnPreSub1_Click(object sender, EventArgs e)
        {
            MovePage(this.dgViewSub1, this.dgViewSub1.PageIndex - 1); 
        }

        protected void btnNextSub1_Click(object sender, EventArgs e)
        {
            MovePage(this.dgViewSub1, this.dgViewSub1.PageIndex + 1); 
        }

        protected void btnLastSub1_Click(object sender, EventArgs e)
        {
            MovePage(this.dgViewSub1, this.dgViewSub1.PageCount - 1);  
        }

        protected void btnToPageSub1_Click(object sender, EventArgs e)
        {
            MovePage(this.dgViewSub1, int.Parse(this.txtPageNoSub1.Text) - 1);  
        }

        protected void btnFirstSub2_Click(object sender, EventArgs e)
        {
            MovePage(this.dgViewSub2, 0);
        }

        protected void btnPreSub2_Click(object sender, EventArgs e)
        {
            MovePage(this.dgViewSub2, this.dgViewSub2.PageIndex - 1);
        }

        protected void btnNextSub2_Click(object sender, EventArgs e)
        {
            MovePage(this.dgViewSub2, this.dgViewSub2.PageIndex + 1);
        }

        protected void btnLastSub2_Click(object sender, EventArgs e)
        {
            MovePage(this.dgViewSub2, this.dgViewSub2.PageCount - 1);
        }

        protected void btnToPageSub2_Click(object sender, EventArgs e)
        {
            MovePage(this.dgViewSub2, int.Parse(this.txtPageNoSub2.Text) - 1);
        }
       
        #endregion  

       
    }
}