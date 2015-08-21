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
    public partial class ScheduleSubEdit : WMS.App_Code.BasePage
    {
        protected string FormID;
        protected string strID;
        protected string TableName = "WMS_SCHEDULE";
        protected string PrimaryKey = "ScheduleNo";
        protected string SubKey = "Flag,ScheduleNo";
        protected int Flag = 1;
        protected string strCopy = "0";
       
        BLL.BLLBase bll = new BLL.BLLBase();

        protected void Page_Load(object sender, EventArgs e)
        {
            strID = Request.QueryString["ID"] + "";
            if (!IsPostBack)
            {
                BindDataSub();
            }
            else
            {
                ClientScript.RegisterStartupScript( this.GetType(), "", "BindEvent()", true);
            }
        }
      
        private void BindDataSub()
        {
            DataTable dt = bll.FillDataTable("WMS.SelectScheduleSub", new DataParameter[] { new DataParameter("{0}", string.Format("ScheduleNo='{0}' and Flag={1}", strID, Flag)) });
            DataColumn dc = new DataColumn("dcNewPlan", Type.GetType("System.String"));
            dt.Columns.Add(dc);
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
       

        protected void dgViewSub1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.RowIndex % 2 == 1)
                    e.Row.CssClass = " bottomtable";

                SetTextReadOnly((TextBox)e.Row.FindControl("PlanQty"), (TextBox)e.Row.FindControl("Price"), (TextBox)e.Row.FindControl("PlanDeliverAccount"), (TextBox)e.Row.FindControl("PlanDeliverNoQty"),
                    (TextBox)e.Row.FindControl("PlanDeliverNoAccount"), (TextBox)e.Row.FindControl("DeliverAccount"), (TextBox)e.Row.FindControl("DeliverNoQty"), (TextBox)e.Row.FindControl("DeliverNoAccount"));

                DataRowView drv = e.Row.DataItem as DataRowView;
                ((TextBox)e.Row.FindControl("PlanQty")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("PlanQty")].ToString();
                ((TextBox)e.Row.FindControl("Price")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("Price")].ToString();
                ((TextBox)e.Row.FindControl("SourceNo")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("SourceNo")].ToString();
                ((TextBox)e.Row.FindControl("OrderNo")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("OrderNo")].ToString();
                ((UserControl.Calendar)e.Row.FindControl("OrderDate")).tDate.Text = ToYMD(drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("OrderDate")]);
                ((UserControl.Calendar)e.Row.FindControl("PlanDeliverDate")).tDate.Text = ToYMD(drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("PlanDeliverDate")]);
                ((UserControl.Calendar)e.Row.FindControl("ReplyDate")).tDate.Text = ToYMD(drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("ReplyDate")]);
                ((UserControl.Calendar)e.Row.FindControl("ChangeDate")).tDate.Text = ToYMD(drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("ChangeDate")]);
                ((UserControl.Calendar)e.Row.FindControl("CheckDate")).tDate.Text = ToYMD(drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("CheckDate")]);
                ((UserControl.Calendar)e.Row.FindControl("FactDate")).tDate.Text = ToYMD(drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("FactDate")]);
                ((UserControl.Calendar)e.Row.FindControl("ReportDate")).tDate.Text = ToYMD(drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("ReportDate")]);
                ((UserControl.Calendar)e.Row.FindControl("QualityDate")).tDate.Text = ToYMD(drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("QualityDate")]);
                ((DropDownList)e.Row.FindControl("Result")).SelectedValue = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("Result")].ToString();
                ((DropDownList)e.Row.FindControl("IsComplete")).SelectedValue = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("IsComplete")].ToString();
                ((DropDownList)e.Row.FindControl("IsPlanComplete")).SelectedValue = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("IsPlanComplete")].ToString();
                ((TextBox)e.Row.FindControl("Memo")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("Memo")].ToString();
                ((TextBox)e.Row.FindControl("PlanDeliverQty")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("PlanDeliverQty")].ToString();
                ((TextBox)e.Row.FindControl("PlanDeliverAccount")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("PlanDeliverAccount")].ToString();
                ((TextBox)e.Row.FindControl("PlanDeliverNoQty")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("PlanDeliverNoQty")].ToString();
                ((TextBox)e.Row.FindControl("PlanDeliverNoAccount")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("PlanDeliverNoAccount")].ToString();
                ((TextBox)e.Row.FindControl("PlanCompleteRate")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("PlanCompleteRate")].ToString();
                ((TextBox)e.Row.FindControl("DeliveredQty")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("DeliverQty")].ToString();
                ((TextBox)e.Row.FindControl("DeliverAccount")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("DeliverAccount")].ToString();
                ((TextBox)e.Row.FindControl("DeliverNoQty")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("DeliverNoQty")].ToString();
                ((TextBox)e.Row.FindControl("DeliverNoAccount")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("DeliverNoAccount")].ToString();
                ((TextBox)e.Row.FindControl("CompleteRate")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("CompleteRate")].ToString();
                ((TextBox)e.Row.FindControl("NoCompleteMemo")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("NoCompleteMemo")].ToString();

            }
        }

        protected void dgViewSub1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {

                for (int i = 0; i < e.Row.Cells.Count; i++)
                {
                    e.Row.Cells[i].Attributes.Add("onmousemove", "TableOnMouseMove(this)");
                    e.Row.Cells[i].Attributes.Add("onmousedown", "TableOnMouseDown(this)");
                    e.Row.Cells[i].Attributes.Add("onmouseup", "TableOnMouseUp(this)");
                    //e.Row.Cells[i].Attributes.Add("onmousemove", "SyDG_moveOnTd(this)");
                    //e.Row.Cells[i].Attributes.Add("onmousedown", "SyDG_downOnTd(this)");
                    //e.Row.Cells[i].Attributes.Add("onmouseup", "this.mouseDown=false");
                    //e.Row.Cells[i].Attributes.Add("onmouseout", "this.mouseDown=false");
                }

            }
        }

        private void UpdateTempSub(GridView dgv)
        {
            DataTable dt1 = (DataTable)Session[FormID + "_Edit_" + dgv.ID];
            if (dt1.Rows.Count == 0)
                return;
            DataRow dr;
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                dr = dt1.Rows[i + dgv.PageSize * dgv.PageIndex];
                dr.BeginEdit();


                dr["SourceNo"] = ((TextBox)dgv.Rows[i].FindControl("SourceNo")).Text;
                dr["OrderNo"] = ((TextBox)dgv.Rows[i].FindControl("OrderNo")).Text;
                dr["OrderDate"] = ((UserControl.Calendar)dgv.Rows[i].FindControl("OrderDate")).DateValue;
                dr["PlanDeliverDate"] = ((UserControl.Calendar)dgv.Rows[i].FindControl("PlanDeliverDate")).DateValue;
                dr["ReplyDate"] = ((UserControl.Calendar)dgv.Rows[i].FindControl("ReplyDate")).DateValue;
                dr["ChangeDate"] = ((UserControl.Calendar)dgv.Rows[i].FindControl("ChangeDate")).DateValue;
                dr["CheckDate"] = ((UserControl.Calendar)dgv.Rows[i].FindControl("CheckDate")).DateValue;
                dr["FactDate"] = ((UserControl.Calendar)dgv.Rows[i].FindControl("FactDate")).DateValue;
                dr["ReportDate"] = ((UserControl.Calendar)dgv.Rows[i].FindControl("ReportDate")).DateValue;
                dr["QualityDate"] = ((UserControl.Calendar)dgv.Rows[i].FindControl("QualityDate")).DateValue;
                dr["Result"] = ((DropDownList)dgv.Rows[i].FindControl("Result")).SelectedValue;
                dr["IsComplete"] = ((DropDownList)dgv.Rows[i].FindControl("IsComplete")).SelectedValue;
                dr["IsPlanComplete"] = ((DropDownList)dgv.Rows[i].FindControl("IsPlanComplete")).SelectedValue;
                dr["Memo"] = ((TextBox)dgv.Rows[i].FindControl("Memo")).Text;
                dr["PlanDeliverQty"] = ((TextBox)dgv.Rows[i].FindControl("PlanDeliverQty")).Text;
                dr["PlanDeliverAccount"] = ((TextBox)dgv.Rows[i].FindControl("PlanDeliverAccount")).Text;
                dr["PlanDeliverNoQty"] = ((TextBox)dgv.Rows[i].FindControl("PlanDeliverNoQty")).Text;
                dr["PlanDeliverNoAccount"] = ((TextBox)dgv.Rows[i].FindControl("PlanDeliverNoAccount")).Text;
                dr["PlanCompleteRate"] = ((TextBox)dgv.Rows[i].FindControl("PlanCompleteRate")).Text;
                dr["DeliverQty"] = ((TextBox)dgv.Rows[i].FindControl("DeliveredQty")).Text;
                dr["DeliverAccount"] = ((TextBox)dgv.Rows[i].FindControl("DeliverAccount")).Text;
                dr["DeliverNoQty"] = ((TextBox)dgv.Rows[i].FindControl("DeliverNoQty")).Text;
                dr["DeliverNoAccount"] = ((TextBox)dgv.Rows[i].FindControl("DeliverNoAccount")).Text;
                dr["CompleteRate"] = ((TextBox)dgv.Rows[i].FindControl("CompleteRate")).Text;
                dr["NoCompleteMemo"] = ((TextBox)dgv.Rows[i].FindControl("NoCompleteMemo")).Text;




                dr.EndEdit();
            }
            dt1.AcceptChanges();
            Session[FormID + "_Edit_" + dgv.ID] = dt1;
        }

        

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateTempSub(this.dgViewSub1);
                DataTable dt = (DataTable)Session[FormID + "_Edit_dgViewSub1"];
                string[] Commands = new string[2];

                Commands[0] = "WMS.DeleteScheduleSub";
                Commands[1] = "WMS.InsertScheduleSubNew";
                bll.ExecTran(Commands, SubKey, new DataTable[] { dt });
            }
            catch (Exception ex)
            {
                WMS.App_Code.JScript.Instance.ShowMessage(this.Page, ex.Message);
                return;
            }

            Response.Write("<script>window.opener=null;window.parent.returnValue='1'; window.close();</script>");  
            
        }

      
        #region 从表资料绑定

      
        protected void ddlPageSizeSub1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTempSub(this.dgViewSub1);
            DataTable dt1 = (DataTable)Session[FormID + "_Edit_dgViewSub1"];
            this.dgViewSub1.DataSource = dt1;
            this.dgViewSub1.PageSize = int.Parse(ddlPageSizeSub1.Text);
            this.dgViewSub1.DataBind();
            SetPageSubBtnEnabled();
        }
        private void MovePage(GridView dgv, int pageindex)
        {
            UpdateTempSub(dgv);
            int pindex = pageindex;
            if (pindex < 0) pindex = 0;
            if (pindex >= dgv.PageCount) pindex = dgv.PageCount - 1;
            DataTable dt1 = (DataTable)Session[FormID + "_Edit_" + dgv.ID];
            dgv.PageIndex = pindex;
            dgv.DataSource = dt1;
            dgv.DataBind();

            SetPageSubBtnEnabled();
            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "Resize", "content_resize();", true);

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

        private void BindPageSize()
        {
            this.ddlPageSizeSub1.Items.Add(new ListItem("10", "10"));
            this.ddlPageSizeSub1.Items.Add(new ListItem("20", "20"));
            this.ddlPageSizeSub1.Items.Add(new ListItem("30", "30"));
            this.ddlPageSizeSub1.Items.Add(new ListItem("40", "40"));
            this.ddlPageSizeSub1.Items.Add(new ListItem("50", "50"));
            this.ddlPageSizeSub1.Items.Add(new ListItem("100", "100"));
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

       
        #endregion  

       
       
    }
}