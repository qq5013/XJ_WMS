using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using IDAL;

namespace WMS.WebUI.CMD
{
    public partial class ProductBomEdit : WMS.App_Code.BasePage
    {
        protected string FormID;
        protected string ID;
        protected string ColorID;
        protected string TableName = "CMD_ProductBOM";
        protected string PrimaryKey = "ProductNo";
        BLL.BLLBase bll = new BLL.BLLBase();

        protected void Page_Load(object sender, EventArgs e)
        {
            ID = Request.QueryString["ProductCode"] + "";
            ColorID=Request.QueryString["ColorID"] + "";
            FormID = Request.QueryString["FormID"] + "";
            if (!IsPostBack)
            {
                if (ID != "")
                {
                    DataTable dt = bll.FillDataTable("Cmd.SelectProductBOM", new DataParameter[] { new DataParameter("{0}", string.Format("Product_Code='{0}' and ColorID='{1}'", ID, ColorID)) });
                    BindData(dt);

                    this.txtID.ReadOnly = true;
                }
                else
                {
                    this.txtID.Text = "";
                }
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
      
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (ID == "") //新增   FactoryID, FactoryName, LinkPerson, LinkPhone, Fax, Address, MEMO
            {
                int Count = bll.GetRowCount(TableName, string.Format("Product_Code='{0}' and ColorID='{1}'", this.txtProduct.Text, this.txtColorID.Text));
                if (Count > 0)
                {
                    WMS.App_Code.JScript.Instance.ShowMessage(this.Page, "该产品编码已经存在！");
                    return;
                }

                bll.ExecNonQuery("Cmd.InsertProductBOM", new DataParameter[] { new DataParameter("@ProductNo", this.txtID.Text.Trim()),
                                                                             new DataParameter("@Name", this.txtName.Text.Trim()),
                                                                             new DataParameter("@Unit", this.txtUnit.Text.Trim()),
                                                                             new DataParameter("@Product_Code", this.txtProduct.Text.Trim()),
                                                                             new DataParameter("@ColorID", this.txtColorID.Text.Trim()), 
                                                                             new DataParameter("@MEMO", this.txtMemo.Text.Trim()),
                                                                             new DataParameter("@StandPrice", this.txtStandPrice.Text.Trim()==""?"0": this.txtStandPrice.Text.Trim()),
                                                                             new DataParameter("@Price1", this.txtPrice1.Text.Trim()==""?"0": this.txtPrice1.Text.Trim()),
                                                                             new DataParameter("@Price2", this.txtPrice2.Text.Trim()==""?"0": this.txtPrice2.Text.Trim()),
                                                                             new DataParameter("@Price3", this.txtPrice3.Text.Trim()==""?"0": this.txtPrice3.Text.Trim()),
                                                                             new DataParameter("@Price4", this.txtPrice4.Text.Trim()==""?"0": this.txtPrice4.Text.Trim()),
                                                                             new DataParameter("@Price5", this.txtPrice5.Text.Trim()==""?"0": this.txtPrice5.Text.Trim()),
                                                                             new DataParameter("@OrderNum", this.txtOrderNum.Text.Trim()),
                                                                            new DataParameter("@Creator", Session["EmployeeCode"].ToString()),
                                                                            new DataParameter("@Updater",Session["EmployeeCode"].ToString())  });
            }
            else //修改
            {
                bll.ExecNonQuery("Cmd.UpdateProductBOM", new DataParameter[] {   new DataParameter("@Name", this.txtName.Text.Trim()),
                                                                             new DataParameter("@Unit", this.txtUnit.Text.Trim()),
                                                                             new DataParameter("@Product_Code", this.txtProduct.Text.Trim()),
                                                                             new DataParameter("@ColorID", this.txtColorID.Text.Trim()), 
                                                                             new DataParameter("@MEMO", this.txtMemo.Text.Trim()),
                                                                             new DataParameter("@StandPrice", this.txtStandPrice.Text.Trim()==""?"0": this.txtStandPrice.Text.Trim()),
                                                                             new DataParameter("@Price1", this.txtPrice1.Text.Trim()==""?"0": this.txtPrice1.Text.Trim()),
                                                                             new DataParameter("@Price2", this.txtPrice2.Text.Trim()==""?"0": this.txtPrice2.Text.Trim()),
                                                                             new DataParameter("@Price3", this.txtPrice3.Text.Trim()==""?"0": this.txtPrice3.Text.Trim()),
                                                                             new DataParameter("@Price4", this.txtPrice4.Text.Trim()==""?"0": this.txtPrice4.Text.Trim()),
                                                                             new DataParameter("@Price5", this.txtPrice5.Text.Trim()==""?"0": this.txtPrice5.Text.Trim()),
                                                                             new DataParameter("@OrderNum", this.txtOrderNum.Text.Trim()),
                                                                             new DataParameter("@Updater",Session["EmployeeCode"].ToString()),
                                                                             new DataParameter("{0}", this.txtID.Text.Trim()) });
            }

            Response.Redirect(FormID + "View.aspx?SubModuleCode=" + SubModuleCode + "&FormID=" + Server.UrlEncode(FormID) + "&ProductCode=" + Server.UrlEncode(this.txtProduct.Text) + "&ColorID=" + Server.UrlEncode(this.txtColorID.Text));
        }
    }
}