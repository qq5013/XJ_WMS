 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FastReport;
using FastReport.Data;
using FastReport.Utils;
using System.Data;
using System.IO;
using IDAL;


namespace WMS.WebUI.Query
{
    public partial class InScheduleQuery :App_Code.BasePage
    {
        private string strWhere;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                rptview.Visible = false;
            }
            else
            {
                string hdnwh = HdnWH.Value;
                int W = int.Parse(hdnwh.Split('#')[0]);
                int H = int.Parse(hdnwh.Split('#')[1]);
                WebReport1.Width = W - 60;
                WebReport1.Height = H - 100;

                if (this.HdnProduct.Value.Length > 0)
                    this.btnProduct.Text = "取消指定";
                else
                    this.btnProduct.Text = "指定";
                if (this.hdnColor.Value.Length > 0)
                    this.btnColor.Text = "取消指定";
                else
                    this.btnColor.Text = "指定";
                if (this.hdnFact.Value.Length > 0)
                    this.btnFact.Text = "取消指定";
                else
                    this.btnFact.Text = "指定";


                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "", "BindEvent();", true);
            }

            SetTextReadOnly(this.txtColor, this.txtProductModule, this.txtFact);


        }
        protected void WebReport1_StartReport(object sender, EventArgs e)
        {
            if (!rptview.Visible) return;
            LoadRpt();
        }
        protected void btnPreview_Click(object sender, EventArgs e)
        {
            rptview.Visible = true;
            WebReport1.Refresh();
        }
        private void GetStrWhere()
        {
             strWhere = "1=1";
            if (this.btnProduct.Text == "指定")
            {
                if (this.txtProductID.Text != "")
                {
                    strWhere += " and ProductID='" + this.txtProductID.Text + "'";
                }
            }
            else
            {
                if (HdnProduct.Value.Length > 0)
                    strWhere += " and ProductID in (" + HdnProduct.Value + ")";
            }

            if (this.btnColor.Text == "指定")
            {
                if (this.txtColorID.Text != "")
                {
                    strWhere += " and ColorID='" + this.txtColorID.Text + "'";
                }
            }
            else
            {
                if (hdnColor.Value.Length > 0)
                    strWhere += " and ColorID in (" + hdnColor.Value + ")";
            }

            if (this.btnFact.Text == "指定")
            {
                if (this.txtFactID.Text != "")
                {
                    strWhere += " and FactoryID='" + this.txtFactID.Text + "'";
                }
            }
            else
            {
                if (hdnFact.Value.Length > 0)
                    strWhere += " and FactoryID in (" + hdnFact.Value + ")";
            }
            //下单日期
            if (this.txtStartDate.tDate.Text != "")
            {
                strWhere += " and CONVERT(nvarchar(10),OrderDate,111)>='" + this.txtStartDate.tDate.Text + "'";
            }
            if (this.txtEndDate.tDate.Text != "")
            {
                strWhere += " and CONVERT(nvarchar(10),OrderDate,111)<='" + this.txtEndDate.tDate.Text + "'";
            }
            //预入库日
            if (this.txtPlanStartDate.tDate.Text != "")
            {
                strWhere += " and CONVERT(nvarchar(10),PlanDate,111)>='" + this.txtPlanStartDate.tDate.Text + "'";
            }
            if (this.txtPlanEndDate.tDate.Text != "")
            {
                strWhere += " and CONVERT(nvarchar(10),PlanDate,111)<='" + this.txtPlanEndDate.tDate.Text + "'";
            }
            //考核交期
            if (this.txtCheckStartDate.tDate.Text != "")
            {
                strWhere += " and CONVERT(nvarchar(10),Sub.CheckDate,111)>='" + this.txtCheckStartDate.tDate.Text + "'";
            }
            if (this.txtCheckEndDate.tDate.Text != "")
            {
                strWhere += " and CONVERT(nvarchar(10),Sub.CheckDate,111)<='" + this.txtCheckEndDate.tDate.Text + "'";
            }
            //实际交期
            if (this.txtFactStartDate.tDate.Text != "")
            {
                strWhere += " and CONVERT(nvarchar(10),FactDate,111)>='" + this.txtFactStartDate.tDate.Text + "'";
            }
            if (this.txtFactEndDate.tDate.Text != "")
            {
                strWhere += " and CONVERT(nvarchar(10),FactDate,111)<='" + this.txtFactEndDate.tDate.Text + "'";
            }
            if (this.txtSourceNo.Text.Trim() != "")
            {
                strWhere += " and Sub.SourceNo like '%" + this.txtSourceNo.Text + "%'";
            }
            
            if (this.txtOrderNo.Text.Trim() != "")
            {
                strWhere += " and Sub.OrderNo like '%" + this.txtOrderNo.Text + "%'";
            }
            if (this.txtStartNo.Text.Trim() != "")
            {
                strWhere += " and StarNo >='" + this.txtStartNo.Text + "'";
            }
            if (this.txtEndNo.Text.Trim() != "")
            {
                strWhere += " and EndNo <='" + this.txtEndNo.Text + "'";
            }

            if (ddlIsComplete.SelectedItem.Value == "1")
            {
                strWhere += " and IsComplete=1";
            }
            else if (ddlIsComplete.SelectedItem.Value == "0")
            {
                strWhere += " and IsComplete=0 ";
            }
            
        }
        private bool LoadRpt()
        {
            try
            {
                GetStrWhere();
                WebReport1.Report = new Report();
                
                WebReport1.Report.Load(System.AppDomain.CurrentDomain.BaseDirectory + @"RptFiles\" + "InScheduleQuery.frx");

                BLL.BLLBase bll = new BLL.BLLBase();
                DataTable dt = bll.FillDataTable("WMS.SelectScheduleQuery", new DataParameter[] { new DataParameter("{0}", strWhere) });
                if (dt.Rows.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('您所选择的条件没有资料!');", true);
                }

                WebReport1.Report.RegisterData(dt, "InScheduleQuery");
            }
            catch (Exception ex)
            {
            }
            return true;
        }

    }
}