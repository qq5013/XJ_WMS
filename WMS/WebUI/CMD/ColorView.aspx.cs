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
    public partial class ColorView : WMS.App_Code.BasePage
    {
        protected string FormID;
        protected string ID;
        protected string TableName = "View_CMD_Color";
        protected string PrimaryKey = "ID";

        BLL.BLLBase bll = new BLL.BLLBase();
        protected void Page_Load(object sender, EventArgs e)
        {
            ID = Request.QueryString["ID"] + "";
            FormID = Request.QueryString["FormID"] + "";
            if (!IsPostBack)
            {
                DataTable dt = bll.FillDataTable("Cmd.SelectColor", new DataParameter[] { new DataParameter("{0}", string.Format("ID={0}", ID)) });
                BindData(dt);
                writeJsvar(FormID,SqlCmd, ID);
            }
        }

        private void BindData(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                this.txtID.Text = dt.Rows[0]["ID"].ToString();
                this.txtColorID.Text = dt.Rows[0]["COLOR_CODE"].ToString();
                this.txtColor_Name.Text = dt.Rows[0]["COLOR_NAME"].ToString();
                this.txtProduct.Text = dt.Rows[0]["PRODUCT_CODE"].ToString();
                this.txtProductModel.Text = dt.Rows[0]["PRODUCT_Model"].ToString();
                this.txtProductName.Text = dt.Rows[0]["PRODUCT_NAME"].ToString();
                this.txtProductCode.Text = dt.Rows[0]["Product_Code"].ToString();
                this.txtMemo.Text = dt.Rows[0]["Memo"].ToString();

            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string strID =this.txtID.Text;
            int Count = bll.GetRowCount("VUsed_CMD_Color", string.Format("ProductID='{1}' and ColorID='{0}'", this.txtColorID.Text.Trim(),this.txtProductCode.Text));
            if (Count > 0)
            {
                WMS.App_Code.JScript.Instance.ShowMessage(this.updatePanel, "该规格编码还被其它单据使用，请调整后再删除！");
                return;
            }
            bll.ExecNonQuery("Cmd.DeleteColor", new DataParameter[] { new DataParameter("{0}", strID) });


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