using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using IDAL;
 

namespace WMS.WebUI.CMD
{
    public partial class WarehouseShelfEditPage :WMS.App_Code.BasePage
    {
        
        DataTable dtShelf;
        BLL.BLLBase bll = new BLL.BLLBase();
        private string TableName;
        private string ColumnName;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["CMD_WH_SHELF_ID"] != null)
                {
                    dtShelf = bll.FillDataTable("Cmd.SelectShelf", new DataParameter[] { new DataParameter("{0}", " SHELF_CODE='" + Request.QueryString["CMD_WH_SHELF_ID"] + "'") });

                    this.txtShelfID.Text = dtShelf.Rows[0]["CMD_WH_SHELF_ID"].ToString();
                    this.txtWhCode.Text = dtShelf.Rows[0]["WAREHOUSE_CODE"].ToString();
                    this.txtAreaCode.Text = dtShelf.Rows[0]["AREA_CODE"].ToString();
                    this.txtShelfCode.Text = dtShelf.Rows[0]["SHELF_CODE"].ToString();
                    this.txtShelfName.Text = dtShelf.Rows[0]["SHELF_NAME"].ToString();
                    this.txtCellRows.Text = dtShelf.Rows[0]["ROW_COUNT"].ToString();
                    this.txtCellCols.Text = dtShelf.Rows[0]["COLUMN_COUNT"].ToString();
                    this.txtProductID.Text = dtShelf.Rows[0]["DefaultProduct"].ToString();
                    this.txtProductName.Text = bll.GetFieldValue("CMD_PRODUCT", "PRODUCT_PartsName", string.Format("PRODUCT_CODE = '{0}'", this.txtProductID.Text));
                    this.txtColorID.Text = dtShelf.Rows[0]["DefaultColor"].ToString();
                    this.txtColorName.Text = bll.GetFieldValue("CMD_COLOR", "COLOR_NAME", string.Format("PRODUCT_CODE = '{0}' and COLOR_CODE='{1}'", this.txtProductID.Text.Length==0?"": this.txtProductID.Text.Substring(0,5),this.txtColorID.Text)); ;
                    this.txtMemo.Text = dtShelf.Rows[0]["MEMO"].ToString();
                    this.ddlActive.SelectedValue = dtShelf.Rows[0]["IsActive"].ToString();

                    int rowCount = bll.GetRowCount("CMD_WH_CELL", "SHELF_CODE='" + dtShelf.Rows[0]["SHELF_CODE"].ToString() + "' and PALLET_CODE!=''");
                    if (rowCount > 0)
                    {
                        this.Button1.Visible = false;
                        this.Button2.Visible = false;
                    }
                    else
                    {
                        this.Button1.Visible = true;
                        this.Button2.Visible = true;
                    }
                }
                else if (Request.QueryString["AREACODE"] != null)
                {

                    this.txtWhCode.Text = "01";
                    this.txtAreaCode.Text = Request.QueryString["AREACODE"];
                    int RowCount = bll.GetRowCount("CMD_WH_SHELF",string.Format( "AREA_CODE='{0}'",Request.QueryString["AREACODE"])) + 1;
                    this.txtShelfCode.Text = Request.QueryString["AREACODE"] + RowCount.ToString().PadLeft(3, '0');

                   
                }
                SetTextReadOnly(txtShelfCode, this.txtWhCode, this.txtAreaCode, this.txtCellCols, this.txtCellRows);
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.txtShelfID.Text.Trim().Length == 0)
                {
                    int count = bll.GetRowCount("CMD_WH_SHELF", string.Format("SHELF_CODE='{0}'", this.txtShelfCode.Text));
                    if (count > 0)
                    {
                        WMS.App_Code.JScript.Instance.ShowMessage(this, "此仓库编码已存在，不能新增！");
                        return;
                    }
                     
                    bll.ExecNonQuery("Cmd.InsertShelf", new DataParameter[] { 
                                                                                new DataParameter("@SHELF_CODE", this.txtShelfCode.Text),
                                                                                new DataParameter("@SHELF_NAME", this.txtShelfName.Text.Trim().Replace("\'", "\''")),
                                                                                new DataParameter("@ROW_COUNT",  this.txtCellRows.Text),
                                                                                new DataParameter("@COLUMN_COUNT", this.txtCellCols.Text.Trim()),
                                                                                new DataParameter("@WAREHOUSE_CODE",  this.txtWhCode.Text),
                                                                                new DataParameter("@AREA_CODE", this.txtAreaCode.Text.Trim()),
                                                                                new DataParameter("@IsActive", this.ddlActive.SelectedValue),
                                                                                new DataParameter("@MEMO", this.txtMemo.Text.Trim()),
                                                                                new DataParameter("@DefaultProduct",  this.txtProductID.Text),
                                                                                new DataParameter("@DefaultColor", this.txtColorID.Text.Trim())});
                    //this.btnContinue.Enabled = true;
                    //this.btnSave.Enabled = false;
                    WMS.App_Code.JScript.Instance.RegisterScript(this, "ReloadParent();");
                    AddOperateLog("货贺管理", "添加货贺信息");
                }
                else
                {
                    bll.ExecNonQuery("Cmd.UpdateShelf", new DataParameter[] { new DataParameter("@SHELF_CODE", this.txtShelfCode.Text),
                                                                              new DataParameter("@SHELF_NAME", this.txtShelfName.Text.Trim().Replace("\'", "\''")), 
                                                                              new DataParameter("@ROW_COUNT",  this.txtCellRows.Text),
                                                                              new DataParameter("@COLUMN_COUNT", this.txtCellCols.Text.Trim()),
                                                                              new DataParameter("@WAREHOUSE_CODE",  this.txtWhCode.Text),
                                                                              new DataParameter("@AREA_CODE", this.txtAreaCode.Text.Trim()),
                                                                              new DataParameter("@IsActive", this.ddlActive.SelectedValue),
                                                                              new DataParameter("@MEMO", this.txtMemo.Text.Trim()), 
                                                                              new DataParameter("@DefaultProduct",  this.txtProductID.Text),
                                                                              new DataParameter("@DefaultColor", this.txtColorID.Text.Trim()),
                                                                              new DataParameter("{0}",  this.txtShelfID.Text)});

                    
                    WMS.App_Code.JScript.Instance.RegisterScript(this, "UpdateParent();");
                    AddOperateLog("货贺管理", "修改货贺信息");
                }
            }
            catch (Exception exp)
            {
                WMS.App_Code.JScript.Instance.ShowMessage(this, exp.Message);
            }
            //JScript.Instance.RegisterScript(this, "window.close();");
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            WMS.App_Code.JScript.Instance.RegisterScript(this, "window.close();");
        }

        protected void btnContinue_Click(object sender, EventArgs e)
        {
            this.txtShelfID.Text = "";
            
            this.txtShelfName.Text = "";
            this.txtMemo.Text = "";
            this.btnSave.Enabled = true;
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string whcode = this.txtWhCode.Text;
            string areacode = this.txtAreaCode.Text;
            string shelfCode = this.txtShelfCode.Text;
            string shelfid = this.txtShelfID.Text;


            DataTable dtTemp = bll.FillDataTable("Cmd.SelectCell", new DataParameter[] { new DataParameter("{0}", "SHELF_CODE='" + shelfCode + "'") });
            int count = dtTemp.Rows.Count;
            if (count > 0)
            {
                WMS.App_Code.JScript.Instance.ShowMessage(this, shelfCode + "货架还有下属货位，不能删除！");
                return;
            }
            else
            {
                bll.ExecNonQuery("Cmd.DeleteShelf", new DataParameter[] { new DataParameter("{0}", shelfid) });
                this.txtShelfID.Text = "";
                this.txtShelfCode.Text = "";
                this.txtShelfName.Text = "";
                this.txtMemo.Text = "";
                this.btnSave.Enabled = true;
               
                string path = whcode + "/" + areacode + "/" + shelfCode;
                WMS.App_Code.JScript.Instance.RegisterScript(this, "RefreshParent('" + path + "');");
            }
            AddOperateLog("货贺管理", "删除货贺信息");
        }
    }
}