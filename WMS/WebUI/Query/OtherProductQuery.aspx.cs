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
    public partial class OtherProductQuery :App_Code.BasePage
    {
        private string strWhere;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                rptview.Visible = false;
                this.txtEndDate.DateValue = DateTime.Now;
                this.txtStartDate.DateValue = DateTime.Now.AddMonths(-1);
                BindOther();
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

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "", "BindEvent();", true);
            }
            SetTextReadOnly( this.txtProductModule, this.txtColor);

        }

        private void BindOther()
        {
            BLL.BLLBase bll = new BLL.BLLBase();
            DataTable dt = bll.FillDataTable("Cmd.SelectWarehouse", new DataParameter[] { new DataParameter("{0}", "IsManage=0") });
            DataRow dr = dt.NewRow();
            dr["CMD_WAREHOUSE_ID"] = 0;
            dr["WAREHOUSE_NAME"] = "请选择";
            dt.Rows.InsertAt(dr, 0);
            this.ddlStockFunction.DataValueField = "CMD_WAREHOUSE_ID";
            this.ddlStockFunction.DataTextField = "WAREHOUSE_NAME";
            this.ddlStockFunction.DataSource = dt;
            this.ddlStockFunction.DataBind();
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
            strWhere="1=1";
            if (this.HdnProduct.Value.Length == 0)
            {
                if (this.txtProductID.Text.Length > 0)
                    strWhere += string.Format(" and product.Product_Code='{0}'", this.txtProductID.Text);
            }
            else
            {
                strWhere += "and product.Product_Code in (" + this.HdnProduct.Value + ") ";
            }
            if (this.hdnColor.Value.Length == 0)
            {
                if (this.txtColorID.Text.Length > 0)
                    strWhere += string.Format(" and Color.COLOR_CODE='{0}'", this.txtColorID.Text);
            }
            else
            {
                strWhere += " and Color.COLOR_CODE in (" + this.hdnColor.Value + ") ";
            }

        }
        private bool LoadRpt()
        {
            try
            {
                GetStrWhere();
                WebReport1.Report = new Report();
                WebReport1.Report.Load(System.AppDomain.CurrentDomain.BaseDirectory + @"RptFiles\"+"OtherProductQuery.frx");

                BLL.BLLQuery bll = new BLL.BLLQuery();
                DataTable dt = bll.GetProductQuery(this.txtStartDate.tDate.Text, this.txtEndDate.tDate.Text, strWhere, Session["EmployeeCode"].ToString(),int.Parse(this.ddlStockFunction.SelectedItem.Value));
                if (dt.Rows.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('您所选择的条件没有资料!');", true);
                }

                WebReport1.Report.RegisterData(dt, "ProductQuery");
            }
            catch(Exception ex)
            {
            }
            return true;
        }

    }
}