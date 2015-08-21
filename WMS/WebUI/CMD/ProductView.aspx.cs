using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using IDAL;

namespace WMS.WebUI.CMD
{
    public partial class ProductView : WMS.App_Code.BasePage
    {
        protected string FormID;
        protected string ID;
        protected string TableName = "CMD_PRODUCT";
        protected string PrimaryKey = "PRODUCT_CODE";

        BLL.BLLBase bll = new BLL.BLLBase();
        protected void Page_Load(object sender, EventArgs e)
        {
            ID = Request.QueryString["ID"] + "";
            FormID = Request.QueryString["FormID"] + "";
            if (!IsPostBack)
            {
                DataTable dt = bll.FillDataTable("Cmd.SelectProduct", new DataParameter[] { new DataParameter("{0}", string.Format("PRODUCT_CODE='{0}'", ID)) });
                BindData(dt);
                writeJsvar(FormID, TableName, PrimaryKey, ID);
            }
        }

        private void BindData(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                this.txtID.Text = dt.Rows[0]["PRODUCT_CODE"].ToString();
                this.txtPRODUCT_NAME.Text = dt.Rows[0]["PRODUCT_NAME"].ToString();
                this.txtPRODUCT_PartsName.Text = dt.Rows[0]["PRODUCT_PartsName"].ToString();
                this.txtPRODUCT_MODEL.Text = dt.Rows[0]["PRODUCT_MODEL"].ToString();
                this.txtPRODUCT_FMODEL.Text = dt.Rows[0]["PRODUCT_FMODEL"].ToString();
                this.txtPRODUCT_Class.Text = dt.Rows[0]["PRODUCT_Class"].ToString();
                this.txtPACK_QTY.Text = dt.Rows[0]["PACK_QTY"].ToString();
                this.txtPALLET_QTY.Text = dt.Rows[0]["PALLET_QTY"].ToString();
                this.chkIS_MIX.Checked = bool.Parse(dt.Rows[0]["IS_MIX"].ToString());
                this.chkIsBarCode.Checked = bool.Parse(dt.Rows[0]["IsBarCode"].ToString());
                this.chkIsInStock.Checked = bool.Parse(dt.Rows[0]["IsInStock"].ToString());
                this.txtMemo.Text = dt.Rows[0]["Memo"].ToString();
                this.txtProductVolume.Text = dt.Rows[0]["ProductVolume"].ToString();

            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string strID = this.txtID.Text;

            int Count = bll.GetRowCount("VUsed_CMD_Product", string.Format("ProductID='{0}' or ProductID=substring('{0}',1,5) ", strID));
            if (Count > 0)
            {
                WMS.App_Code.JScript.Instance.ShowMessage(this.updatePanel, "该规格编码还被其它单据使用，请调整后再删除！");
                return;
            }
            bll.ExecNonQuery("Cmd.DeleteProduct", new DataParameter[] { new DataParameter("{0}", "'" + strID + "'") });


            btnNext_Click(sender, e);
            if (this.txtID.Text == strID)
                btnPre_Click(sender, e);

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