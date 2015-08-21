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
    public partial class OutScheduleQuery :App_Code.BasePage
    {
        private string strWhere;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                rptview.Visible = false;
                BLL.BLLBase bll = new BLL.BLLBase();
                DataTable dtArea = bll.FillDataTable("CMD.SelectAreaSation", null);
                DataRow dr = dtArea.NewRow();
                dr[0] = "请选择";
                dtArea.Rows.InsertAt(dr, 0);
                dtArea.AcceptChanges();
                this.ddlAreaSation.DataValueField = "areasation";
                this.ddlAreaSation.DataTextField = "areasation";
                this.ddlAreaSation.DataSource = dtArea;
                this.ddlAreaSation.DataBind();
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
                if (this.HdnFProduct.Value.Length > 0)
                    this.btnFProduct.Text = "取消指定";
                else
                    this.btnFProduct.Text = "指定";

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

            SetTextReadOnly(this.txtColor, this.txtProductModule, this.txtFact,this.txtProductFModule);

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
            if (this.btnFProduct.Text == "指定")
            {
                if (this.txtFProductID.Text != "")
                {
                    strWhere += " and ProductID='" + this.txtFProductID.Text + "'";
                }
            }
            else
            {
                if (HdnFProduct.Value.Length > 0)
                    strWhere += " and ProductID in (" + HdnFProduct.Value + ")";
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
                    strWhere += " and CustomerID='" + this.txtFactID.Text + "'";
                }
            }
            else
            {
                if (hdnFact.Value.Length > 0)
                    strWhere += " and CustomerID in (" + hdnFact.Value + ")";
            }

            if (this.txtStartDate.tDate.Text != "")
            {
                strWhere += " and CONVERT(nvarchar(10),BillDate,111)>='" + this.txtStartDate.tDate.Text + "'";
            }
            if (this.txtEndDate.tDate.Text != "")
            {
                strWhere += " and CONVERT(nvarchar(10),BillDate,111)<='" + this.txtEndDate.tDate.Text + "'";
            }

            
            if (this.ddlAreaSation.SelectedItem.Text != "请选择")
            {
                strWhere += " and AreaSation like  '%" + this.ddlAreaSation.SelectedItem.Text + "%'";
            }

            if (opt2.Checked)
            {
                strWhere += " and sub.PlanQty-sub.RealQty=0 ";
            }
            else if (opt3.Checked)
            {
                strWhere += " and (sub.PlanQty-sub.RealQty>0) and sub.RealQty!=0";
            }
            else if (opt4.Checked)
            {
                strWhere += " and sub.RealQty=0 ";
            }
            

        }
        private bool LoadRpt()
        {
            try
            {
                GetStrWhere();
                WebReport1.Report = new Report();
                string frx = "OutScheduleQuery.frx";
                string Comds = "WMS.SelectOutScheduleQuery";

                if (rpt1.Checked)
                {
                    frx = "OutScheduleQuery.frx";
                    Comds = "WMS.SelectOutScheduleQuery";
                }
                else if (rpt2.Checked)
                {
                    frx = "OutScheduleCustQuery.frx";
                    Comds = "WMS.SelectOutScheduleCustQuery";

                }
                else
                {
                    frx = "OutScheduleProductQuery.frx";
                    Comds = "WMS.SelectOutScheduleProductQuery";
                }


                WebReport1.Report.Load(System.AppDomain.CurrentDomain.BaseDirectory + @"RptFiles\" + frx);

                BLL.BLLBase bll = new BLL.BLLBase();

                DataTable dt = bll.FillDataTable(Comds, new DataParameter[] { new DataParameter("{0}", strWhere) });
                if (dt.Rows.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('您所选择的条件没有资料!');", true);
                }

                WebReport1.Report.RegisterData(dt, "OutScheduleQuery");
            }
            catch (Exception ex)
            {
            }
            return true;
        }

    }
}