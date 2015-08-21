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
    public partial class ProductEdit : WMS.App_Code.BasePage
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
                DataTable dtClass = bll.FillDataTable("Cmd.SelectProductClass", null);
                this.txtPRODUCT_Class.DataSource = dtClass;
                if (ID != "")
                {
                    DataTable dt = bll.FillDataTable("Cmd.SelectProduct", new DataParameter[] { new DataParameter("{0}", string.Format("PRODUCT_CODE='{0}'", ID)) });
                    BindData(dt);
                    this.txtID.ReadOnly = true;
                    
                }
                else
                {
                 
                    if (dtClass.Rows.Count > 0)
                        this.txtPRODUCT_Class.Text = dtClass.Rows[0][0].ToString();
                    this.chkIsBarCode.Checked = true;
                    this.chkIsInStock.Checked = true;
                }
                
               

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
                this.chkIsInStock.Checked = bool.Parse(dt.Rows[0]["IsInStock"].ToString());
                this.chkIsBarCode.Checked = bool.Parse(dt.Rows[0]["IsBarCode"].ToString());
                this.txtMemo.Text = dt.Rows[0]["Memo"].ToString();
                this.txtProductVolume.Text = dt.Rows[0]["ProductVolume"].ToString();
            }
        }
      
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (ID == "") //新增
            {
                int Count = bll.GetRowCount(TableName, string.Format("PRODUCT_CODE='{0}'", this.txtID.Text.Trim()));
                if (Count > 0)
                {
                    WMS.App_Code.JScript.Instance.ShowMessage(this.Page, "该用产品编码已经存在！");
                    return;
                }

                bll.ExecNonQuery("Cmd.InsertProduct", new DataParameter[] { new DataParameter("@PRODUCT_CODE", this.txtID.Text.Trim()),  
                                                                           new DataParameter("@PRODUCT_PartsName", this.txtPRODUCT_PartsName.Text.Trim()), 
                                                                           new DataParameter("@PRODUCT_Class", this.txtPRODUCT_Class.Text.Trim()),
                                                                           new DataParameter("@PRODUCT_NAME", this.txtPRODUCT_NAME.Text.Trim()), 
                                                                           new DataParameter("@PRODUCT_FMODEL", this.txtPRODUCT_FMODEL.Text.Trim()), 
                                                                           new DataParameter("@PRODUCT_MODEL", this.txtPRODUCT_MODEL.Text.Trim()), 
                                                                           new DataParameter("@IS_MIX", this.chkIS_MIX.Checked), 
                                                                           new DataParameter("@PALLET_QTY", this.txtPALLET_QTY.Text.Trim()), 
                                                                           new DataParameter("@PACK_QTY", this.txtPACK_QTY.Text.Trim()), 
                                                                           new DataParameter("@IsBarCode", this.chkIsBarCode.Checked),  
                                                                           new DataParameter("@IsInStock", this.chkIsInStock.Checked),  
                                                                           new DataParameter("@MEMO", this.txtMemo.Text.Trim()),
                                                                           new DataParameter("@ProductVolume", this.txtProductVolume.Text.Trim()),
                                                                            new DataParameter("@Creator", Session["EmployeeCode"].ToString()),
                                                                            new DataParameter("@Updater",Session["EmployeeCode"].ToString())  });
                                                                           
            }
            else //修改
            {
                bll.ExecNonQuery("Cmd.UpdateProduct", new DataParameter[] {new DataParameter("@PRODUCT_PartsName", this.txtPRODUCT_PartsName.Text.Trim()), 
                                                                           new DataParameter("@PRODUCT_Class", this.txtPRODUCT_Class.Text.Trim()),
                                                                           new DataParameter("@PRODUCT_NAME", this.txtPRODUCT_NAME.Text.Trim()), 
                                                                           new DataParameter("@PRODUCT_FMODEL", this.txtPRODUCT_FMODEL.Text.Trim()), 
                                                                           new DataParameter("@PRODUCT_MODEL", this.txtPRODUCT_MODEL.Text.Trim()), 
                                                                           new DataParameter("@IS_MIX", this.chkIS_MIX.Checked), 
                                                                           new DataParameter("@PALLET_QTY", this.txtPALLET_QTY.Text.Trim()), 
                                                                           new DataParameter("@PACK_QTY", this.txtPACK_QTY.Text.Trim()), 
                                                                            new DataParameter("@IsBarCode", this.chkIsBarCode.Checked),
                                                                              new DataParameter("@IsInStock", this.chkIsInStock.Checked),  
                                                                            new DataParameter("@MEMO", this.txtMemo.Text.Trim()),
                                                                             new DataParameter("@ProductVolume", this.txtProductVolume.Text.Trim()),
                                                                             new DataParameter("@Updater",Session["EmployeeCode"].ToString()),
                                                                           new DataParameter("{0}", this.txtID.Text.Trim()), 
                                                                        
                                                                            });

            }

            Response.Redirect(FormID + "View.aspx?SubModuleCode=" + SubModuleCode + "&FormID=" + Server.UrlEncode(FormID) + "&ID=" + Server.UrlEncode(this.txtID.Text));
        }
    }
}