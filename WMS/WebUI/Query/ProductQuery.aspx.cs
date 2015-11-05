﻿using System;
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
    public partial class ProductQuery :App_Code.BasePage
    {
        private string strWhere;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                rptview.Visible = false;
                BindOther();

            }
            else
            {
                string hdnwh = HdnWH.Value;
                int W = int.Parse(hdnwh.Split('#')[0]);
                int H = int.Parse(hdnwh.Split('#')[1]);
                WebReport1.Width = W - 60;
                WebReport1.Height = H - 55;
                if (this.HdnProduct.Value.Length > 0)
                    this.btnProduct.Text = "取消指定";
                else
                    this.btnProduct.Text = "指定";

                

                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "BindEvent();", true);
            }
            SetTextReadOnly(this.txtProductName);

        }
        private void BindOther()
        {
            BLL.BLLBase bll = new BLL.BLLBase();
            DataTable ProductType = bll.FillDataTable("Cmd.SelectProductType", new DataParameter[] { new DataParameter("{0}", "cmd.AreaCode='001' and ProductTypeCode<>'0001'") });
            DataRow dr = ProductType.NewRow();
            dr["ProductTypeCode"] = "";
            dr["ProductTypeName"] = "请选择";
            ProductType.Rows.InsertAt(dr, 0);
            ProductType.AcceptChanges();
            
            this.ddlProductType.DataValueField = "ProductTypeCode";
            this.ddlProductType.DataTextField = "ProductTypeName";
            this.ddlProductType.DataSource = ProductType;
            this.ddlProductType.DataBind();



          
            DataTable dtStateNo = bll.FillDataTable("Cmd.SelectProductState");
            dr = dtStateNo.NewRow();
            dr["StateNo"] = "";
            dr["StateName"] = "请选择";
            dtStateNo.Rows.InsertAt(dr, 0);
            dtStateNo.AcceptChanges();

            this.ddlStateNo.DataValueField = "StateNo";
            this.ddlStateNo.DataTextField = "StateName";
            this.ddlStateNo.DataSource = dtStateNo;
            this.ddlStateNo.DataBind();  

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
            if (this.ddlProductType.SelectedValue != "")
            {
                strWhere += string.Format(" and productTypeCode='{0}'", this.ddlProductType.SelectedValue);
            }


            if (this.HdnProduct.Value.Length == 0)
            {
                if (this.txtProductCode.Text.Trim().Length > 0)
                    strWhere += string.Format(" and ProductCode='{0}'", this.txtProductCode.Text);
            }
            else
            {
                strWhere += " and ProductCode in (" + this.HdnProduct.Value + ") ";
            }
            if (this.ddlStateNo.SelectedValue != "")
            {
                strWhere += " and StateNo='" + this.ddlStateNo.SelectedValue + "'";
            }

        }
        private bool LoadRpt()
        {
            try
            {
                GetStrWhere();
                string frx = "ProductDetailQuery.frx";
                string Comds = "WMS.SelectProductDetailQuery";

                if (rpt2.Checked)
                {
                    frx = "ProductTotalQuery.frx";
                    Comds = "WMS.SelectProductTotalQuery";

                }
                WebReport1.Report = new Report();
                WebReport1.Report.Load(System.AppDomain.CurrentDomain.BaseDirectory + @"RptFiles\" + frx);

                BLL.BLLBase bll = new BLL.BLLBase();

                DataTable dt = bll.FillDataTable(Comds, new DataParameter[] { new DataParameter("{0}", strWhere) });



                if (dt.Rows.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('您所选择的条件没有资料!');", true);
                }

                WebReport1.Report.RegisterData(dt, "ProductQuery");
            }
            catch (Exception ex)
            {
            }
            return true;
        }

    }
}