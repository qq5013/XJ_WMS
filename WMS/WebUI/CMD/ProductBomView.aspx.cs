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
    public partial class ProductBomView : WMS.App_Code.BasePage
    {
        protected string FormID;
        protected string ID;
        protected string ColorID;
        protected string TableName = "View_CMD_ProductBom";
        protected string PrimaryKey = "ProductNo";

        BLL.BLLBase bll = new BLL.BLLBase();
        protected void Page_Load(object sender, EventArgs e)
        {
            ID = Request.QueryString["ProductCode"] + "";
            ColorID = Request.QueryString["ColorID"] + "";
            FormID = Request.QueryString["FormID"] + "";
            if (!IsPostBack)
            {
                DataTable dt = bll.FillDataTable("Cmd.SelectProductBOM", new DataParameter[] { new DataParameter("{0}", string.Format("Product_Code='{0}' and ColorID='{1}'", ID, ColorID)) });
                BindData(dt);
                writeJsvar(FormID, TableName, PrimaryKey, ID);
            }
        }

        private void BindData(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                this.txtID.Text = dt.Rows[0]["ProductNo"].ToString();
                this.txtName.Text = dt.Rows[0]["Name"].ToString();
                this.txtUnit.Text = dt.Rows[0]["Unit"].ToString();
                this.txtProduct.Text = dt.Rows[0]["Product_Code"].ToString();
                this.txtProductModel.Text = dt.Rows[0]["ProductModel"].ToString();
                this.txtColorID.Text = dt.Rows[0]["ColorID"].ToString();
                this.txtColorName.Text = dt.Rows[0]["ColorName"].ToString();
                this.txtMemo.Text = dt.Rows[0]["Memo"].ToString();
                this.txtStandPrice.Text = dt.Rows[0]["StandPrice"].ToString();
                this.txtPrice1.Text = dt.Rows[0]["Price1"].ToString();
                this.txtPrice2.Text = dt.Rows[0]["Price2"].ToString();
                this.txtPrice3.Text = dt.Rows[0]["Price3"].ToString();
                this.txtPrice4.Text = dt.Rows[0]["Price4"].ToString();
                this.txtPrice5.Text = dt.Rows[0]["Price5"].ToString();
                this.txtOrderNum.Text = dt.Rows[0]["OrderNum"].ToString();
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string strID = this.txtID.Text;
            int Count = bll.GetRowCount("VUsed_CMD_Product", string.Format("ProductID='{0}'", this.txtProduct.Text.Trim()));
            if (Count > 0)
            {
                WMS.App_Code.JScript.Instance.ShowMessage(this.updatePanel, "该厂商编号还被其它单据使用，请调整后再删除！");
                return;
            }
            bll.ExecNonQuery("Cmd.DeleteProductBOM", new DataParameter[] { new DataParameter("{0}", "'" + strID + "'") });


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