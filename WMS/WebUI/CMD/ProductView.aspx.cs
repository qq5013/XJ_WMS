﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using IDAL;


namespace WMS.WebUI.CMD
{
    public partial class ProductView :App_Code.BasePage
    {
        private string strID;
        private string TableName = "CMD_Product";
        private string PrimaryKey = "ProductCode";
        BLL.BLLBase bll = new BLL.BLLBase();
        protected void Page_Load(object sender, EventArgs e)
        {
            strID = Request.QueryString["ID"] + "";
            if (!IsPostBack)
            {
                BindDropDownList();
                DataTable dt = bll.FillDataTable("Cmd.SelectProduct", new DataParameter[] { new DataParameter("{0}", string.Format("ProductCode='{0}'", strID)) });
                BindData(dt);
                writeJsvar(FormID, SqlCmd, strID);
            }
        }

        #region 绑定方法
        private void BindDropDownList()
        {
            DataTable dtArea = bll.FillDataTable("Cmd.SelectArea", new DataParameter[] { new DataParameter("{0}", "1=1") });
            this.ddlAreaCode.DataValueField = "AreaCode";
            this.ddlAreaCode.DataTextField = "AreaName";
            this.ddlAreaCode.DataSource = dtArea;
            this.ddlAreaCode.DataBind();



            DataTable ProductType = bll.FillDataTable("Cmd.SelectProductType", new DataParameter[] { new DataParameter("{0}", "1=1") });
            this.ddlProductTypeCode.DataValueField = "ProductTypeCode";
            this.ddlProductTypeCode.DataTextField = "ProductTypeName";
            this.ddlProductTypeCode.DataSource = ProductType;
            this.ddlProductTypeCode.DataBind();

            //ddlTrainTypeCode
            DataTable TrainType = bll.FillDataTable("Cmd.SelectTrainType", new DataParameter[] { new DataParameter("{0}", "1=1") });
            this.ddlTrainTypeCode.DataValueField = "TypeCode";
            this.ddlTrainTypeCode.DataTextField = "TypeName";
            this.ddlTrainTypeCode.DataSource = TrainType;
            this.ddlTrainTypeCode.DataBind();

            //ddlCCLX_Factory
            DataTable CCLX_Factory = bll.FillDataTable("Cmd.SelectFactory", new DataParameter[] { new DataParameter("{0}", "Flag=1") });
            this.ddlCCLX_Factory.DataValueField = "FactoryID";
            this.ddlCCLX_Factory.DataTextField = "FactoryName";
            this.ddlCCLX_Factory.DataSource = CCLX_Factory;
            this.ddlCCLX_Factory.DataBind();

            //ddlFCCLX_Factory

            DataTable FCCLX_Factory = bll.FillDataTable("Cmd.SelectFactory", new DataParameter[] { new DataParameter("{0}", "Flag=2") });
            this.ddlFCCLX_Factory.DataValueField = "FactoryID";
            this.ddlFCCLX_Factory.DataTextField = "FactoryName";
            this.ddlFCCLX_Factory.DataSource = FCCLX_Factory;
            this.ddlFCCLX_Factory.DataBind();

            ///ddlCX_Factory
            DataTable CX_Factory = bll.FillDataTable("Cmd.SelectFactory", new DataParameter[] { new DataParameter("{0}", "Flag=3") });
            this.ddlCX_Factory.DataValueField = "FactoryID";
            this.ddlCX_Factory.DataTextField = "FactoryName";
            this.ddlCX_Factory.DataSource = CX_Factory;
            this.ddlCX_Factory.DataBind();

        }

        private void BindData(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {

                this.txtID.Text = dt.Rows[0]["ProductCode"].ToString();
                this.txtProductName.Text = dt.Rows[0]["ProductName"].ToString();
                this.ddlProductTypeCode.SelectedValue = dt.Rows[0]["ProductTypeCode"].ToString();
                this.ddlTrainTypeCode.SelectedValue = dt.Rows[0]["TrainTypeCode"].ToString();
                this.txtAxieNo.Text = dt.Rows[0]["AxieNo"].ToString();
                this.txtWheelDiameter.Text = dt.Rows[0]["WheelDiameter"].ToString();

                this.txtCCZ_Diameter.Text = dt.Rows[0]["CCZ_Diameter"].ToString();
                this.txtFCCZ_Diameter.Text = dt.Rows[0]["FCCZ_Diameter"].ToString();
                this.txtCCD_Diameter.Text = dt.Rows[0]["CCD_Diameter"].ToString();
                this.txtFCCD_Diameter.Text = dt.Rows[0]["FCCD_Diameter"].ToString();
                this.txtCCXPBZW_Size.Text = dt.Rows[0]["CCXPBZW_Size"].ToString();
                this.txtFCCXPBZW_Size.Text = dt.Rows[0]["FCCXPBZW_Size"].ToString();
                this.txtGearNo.Text = dt.Rows[0]["GearNo"].ToString();
                this.txtCCLX_Flag.Text = dt.Rows[0]["CCLX_Flag"].ToString();
                this.txtFCCLX_Flag.Text = dt.Rows[0]["FCCLX_Flag"].ToString();
                this.txtCCLX_Year.Text = dt.Rows[0]["CCLX_Year"].ToString();
                this.txtFCCLX_Year.Text = dt.Rows[0]["FCCLX_Year"].ToString();
                this.ddlCCLX_Factory.SelectedValue = dt.Rows[0]["CCLX_Factory"].ToString();
                this.ddlFCCLX_Factory.SelectedValue = dt.Rows[0]["FCCLX_Factory"].ToString();
                this.ddlCX_Factory.SelectedValue = dt.Rows[0]["CX_Factory"].ToString();
                this.txtCCLG_Flag.Text = dt.Rows[0]["CCLG_Flag"].ToString();
                this.txtFCCLG_Flag.Text = dt.Rows[0]["FCCLG_Flag"].ToString();
                this.ddlAreaCode.SelectedValue = dt.Rows[0]["AreaCode"].ToString();
                this.txtLDXC.Text = dt.Rows[0]["LDXC"].ToString();
                this.txtCX_DateTime.Text = ToYMD(dt.Rows[0]["CX_DateTime"]);
                this.txtInstockDate.Text = ToYMD(dt.Rows[0]["InstockDate"]);

                this.txtMemo.Text = dt.Rows[0]["Memo"].ToString();
                this.txtCreator.Text = dt.Rows[0]["Creator"].ToString();
                this.txtCreatDate.Text = ToYMD(dt.Rows[0]["CreateDate"]);
                this.txtUpdater.Text = dt.Rows[0]["Updater"].ToString();
                this.txtUpdateDate.Text = ToYMD(dt.Rows[0]["UpdateDate"]);

            }
            
        }
        private void BindDataNull()
        {

            this.txtID.Text = "";
            this.txtProductName.Text = "";
            this.txtAxieNo.Text = "";
            this.txtWheelDiameter.Text = "";

            this.txtCCZ_Diameter.Text = "";
            this.txtFCCZ_Diameter.Text = "";
            this.txtCCD_Diameter.Text = "";
            this.txtFCCD_Diameter.Text = "";
            this.txtCCXPBZW_Size.Text = "";
            this.txtFCCXPBZW_Size.Text = "";
            this.txtGearNo.Text = "";
            this.txtCCLX_Flag.Text = "";
            this.txtFCCLX_Flag.Text = "";
            this.txtCCLX_Year.Text = "";
            this.txtFCCLX_Year.Text = "";

            this.txtCCLG_Flag.Text = "";
            this.txtFCCLG_Flag.Text = "";

            this.txtLDXC.Text = "";
            this.txtCX_DateTime.Text = "";
            this.txtInstockDate.Text = "";

            this.txtMemo.Text = "";
            this.txtCreator.Text = "";
            this.txtCreatDate.Text = "";
            this.txtUpdater.Text = "";
            this.txtUpdateDate.Text = "";
        }
        #endregion
        
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string strID = this.txtID.Text;
            int Count = bll.GetRowCount("VUsed_CMD_Product", string.Format("ProductCode='{0}'", this.txtID.Text.Trim()));
            if (Count > 0)
            {
                WMS.App_Code.JScript.Instance.ShowMessage(this.updatePanel, "该产品编码还被其它单据使用，请调整后再删除！");
                return;
            }
            bll.ExecNonQuery("Cmd.DeleteProduct", new DataParameter[] { new DataParameter("{0}", "'" + strID + "'") });

            AddOperateLog("产品信息", "删除单号：" + strID);
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

        #region 上下笔事件
        protected void btnFirst_Click(object sender, EventArgs e)
        {
            BindData(bll.GetRecord("F", TableName, "1=1", PrimaryKey, this.txtID.Text));
        }
        protected void btnPre_Click(object sender, EventArgs e)
        {
            BindData(bll.GetRecord("P", TableName, "1=1", PrimaryKey, this.txtID.Text));
        }
        protected void btnNext_Click(object sender, EventArgs e)
        {
            BindData(bll.GetRecord("N", TableName, "1=1", PrimaryKey, this.txtID.Text));
        }
        protected void btnLast_Click(object sender, EventArgs e)
        {
            BindData(bll.GetRecord("L", TableName, "1=1", PrimaryKey, this.txtID.Text));
        }
        #endregion
        
    }
}