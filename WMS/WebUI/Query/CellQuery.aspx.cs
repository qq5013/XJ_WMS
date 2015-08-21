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
 
namespace WMS.WebUI.Query
{
    public partial class CellQuery : WMS.App_Code.BasePage
    {
         
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindOther();
                BLL.BLLBase bll = new BLL.BLLBase();
                DataTable dt = bll.FillDataTable("Cmd.selectWareHouseQuery", null);
                this.hidetree.Value = Util.JsonHelper.DataTableToJSON(dt);
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.UpdatePanel1.GetType(), "Resize", "treeview_resize();", true);

            }
            else
            {
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
            DataTable dt = bll.FillDataTable("Cmd.selectAreaQuery", null);
            DataRow dr = dt.NewRow();
            dr["AREA_CODE"] = "000";
            dr["AREA_NAME"] = "请选择";
            dt.Rows.InsertAt(dr, 0);
            this.ddlArea.DataValueField = "AREA_CODE";
            this.ddlArea.DataTextField = "AREA_NAME";
            this.ddlArea.DataSource = dt;
            this.ddlArea.DataBind();
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            string strAreaWhere = "1=1";
            string strShelfWhere = "1=1";
            if (ddlArea.SelectedItem.Value != "000")
            {
                strAreaWhere += " and AREA_CODE='" + ddlArea.SelectedItem.Value + "'";
                strShelfWhere += " and AREA_CODE='" + ddlArea.SelectedItem.Value + "'";
            }
            if (ddlActive.SelectedItem.Value == "1")
            {
                strAreaWhere += " and AREA_CODE in (select AREA_CODE from CMD_WH_SHELF where IsActive=1) and AREA_CODE in (select distinct AREA_CODE from CMD_WH_CELL where IS_ACTIVE=1)";
                strShelfWhere += " and IsActive=1 and SHELF_CODE in (select distinct SHELF_CODE from CMD_WH_CELL where IS_ACTIVE=1)";
 
            }
            else if (ddlActive.SelectedItem.Value == "0")
            {
                strAreaWhere += " and AREA_CODE in (select AREA_CODE from CMD_WH_SHELF where IsActive=0) and AREA_CODE in (select distinct AREA_CODE from CMD_WH_CELL where IS_ACTIVE=0)";
                strShelfWhere += " and IsActive=0 and SHELF_CODE in (select distinct SHELF_CODE from CMD_WH_CELL where IS_ACTIVE=0)";
            }


            if (this.HdnProduct.Value.Length == 0)
            {
                if (this.txtProductID.Text.Length > 0)
                {
                    strAreaWhere += " and AREA_CODE in (select AREA_CODE from CMD_WH_SHELF where substring(DefaultProduct,1,5)='" + this.txtProductID.Text + "')";
                    strShelfWhere += " and substring(DefaultProduct,1,5)='" + this.txtProductID.Text + "'";
                }
                else
                {
                    strAreaWhere += " and AREA_CODE in (select AREA_CODE from CMD_WH_SHELF where DefaultProduct='')";
                    strShelfWhere += " and DefaultProduct=''";
                }
            }
            else
            {
                strAreaWhere += " and AREA_CODE in (select AREA_CODE from CMD_WH_SHELF where substring(DefaultProduct,1,5) in (" + this.HdnProduct.Value + "))";
                strShelfWhere += " and substring(DefaultProduct,1,5) in (" + this.HdnProduct.Value + ")";

                
            }
            if (this.hdnColor.Value.Length == 0)
            {
                if (this.txtColorID.Text.Length > 0)
                {
                    strAreaWhere += " and AREA_CODE in (select AREA_CODE from CMD_WH_SHELF where DefaultColor='" + this.txtColorID.Text + "')";
                    strShelfWhere += " and DefaultColor='" + this.txtColorID.Text + "'";
                }
               
            }
            else
            {
                strAreaWhere += " and AREA_CODE in (select AREA_CODE from CMD_WH_SHELF where DefaultColor in (" + this.hdnColor.Value + "))";
                strShelfWhere += " and DefaultColor in (" + this.hdnColor.Value + ")";
            }
            hdnShelfWhere.Value = strShelfWhere;


            BLL.BLLBase bll = new BLL.BLLBase();
            DataTable dt = bll.FillDataTable("Cmd.selectWareHouseQueryByWhere", new DataParameter[] { new DataParameter("{0}", strAreaWhere), new DataParameter("{1}", strShelfWhere) });
            this.hidetree.Value = Util.JsonHelper.DataTableToJSON(dt);
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.UpdatePanel1.GetType(), "Resize", "SetTree();treeview_resize();", true);
        }
        

    }
}