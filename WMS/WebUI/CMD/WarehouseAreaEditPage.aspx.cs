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
    public partial class WarehouseAreaEditPage : WMS.App_Code.BasePage
    {
        
        DataTable dtArea;

        private string TableName = "CMD_WH_AREA";
        private string ColumeName;
        BLL.BLLBase bll = new BLL.BLLBase();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["CMD_WH_AREA_ID"] != null)
                {
                    dtArea = bll.FillDataTable("Cmd.SelectArea", new DataParameter[] { new DataParameter("{0}", "AREA_CODE='" + Request.QueryString["CMD_WH_AREA_ID"] + "'") });
                    if (this.dtArea.Rows.Count > 0)
                    {
                        this.txtAreaID.Text = dtArea.Rows[0]["CMD_WH_AREA_ID"].ToString();
                        this.txtWhCode.Text = dtArea.Rows[0]["WAREHOUSE_CODE"].ToString();
                        this.txtAreaCode.Text = dtArea.Rows[0]["AREA_CODE"].ToString();
                        this.txtAreaName.Text = dtArea.Rows[0]["AREA_NAME"].ToString();
                        this.txtMemo.Text = dtArea.Rows[0]["MEMO"].ToString();
                        
                        this.ddlFunction.SelectedValue = dtArea.Rows[0]["AREA_Function"].ToString();
                        this.ddlStockType.SelectedValue = dtArea.Rows[0]["AREA_StockType"].ToString();
                        this.ddlType.SelectedValue = dtArea.Rows[0]["AREA_Type"].ToString();
                      

                        int Count = bll.GetRowCount("CMD_WH_CELL", string.Format("AREA_CODE='{0}' and  PALLET_CODE is not null", this.txtAreaCode.Text));
                        if (Count > 0)
                            this.ddlFunction.Enabled = false;



                    }
                }
                else if (Request.QueryString["WHCODE"] != null)
                {
                    this.txtWhCode.Text = Request.QueryString["WHCODE"];
                    this.txtAreaCode.Text = bll.GetNewID("CMD_WH_AREA", "AREA_CODE", "1=1");
                    this.ddlFunction.Enabled = false;
                    this.ddlType.Enabled = false;
                }
                SetTextReadOnly(this.txtAreaCode, this.txtWhCode);
            }
            else
            {

            }
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {

            if (this.txtAreaID.Text.Trim().Length == 0)//新增
            {
                int count = bll.GetRowCount(TableName, string.Format("AREA_CODE='{0}'",this.txtAreaCode.Text));
                if (count > 0)
                {
                    WMS.App_Code.JScript.Instance.ShowMessage(this, "此仓库编码已存在，不能新增！");
                    return;
                }

                bll.ExecNonQuery("Cmd.InsertArea", new DataParameter[] {
                                                                        new DataParameter("@AREA_CODE", this.txtAreaCode.Text),
                                                                        new DataParameter("@AREA_NAME", this.txtAreaName.Text.Trim().Replace("\'", "\''")),
                                                                        new DataParameter("@WAREHOUSE_CODE",  this.txtWhCode.Text),
                                                                        new DataParameter("@MEMO", this.txtMemo.Text.Trim()),
                                                                        new DataParameter("@AREA_Function", this.ddlFunction.SelectedValue),
                                                                        new DataParameter("@AREA_StockType", this.ddlStockType.SelectedValue),
                                                                        new DataParameter("@AREA_Type", this.ddlType.SelectedValue)});

                WMS.App_Code.JScript.Instance.RegisterScript(this, "ReloadParent();");
                AddOperateLog("库区管理", "添加库区信息");
            }
            else
            {
                bll.ExecNonQuery("Cmd.UpdateArea", new DataParameter[] {
                                                                        new DataParameter("@AREA_CODE", this.txtAreaCode.Text),
                                                                        new DataParameter("@AREA_NAME", this.txtAreaName.Text.Trim().Replace("\'", "\''")),
                                                                        new DataParameter("@WAREHOUSE_CODE",  this.txtWhCode.Text),
                                                                        new DataParameter("@MEMO", this.txtMemo.Text.Trim()),
                                                                        new DataParameter("@AREA_Function", this.ddlFunction.SelectedValue),
                                                                        new DataParameter("@AREA_StockType", this.ddlStockType.SelectedValue),
                                                                        new DataParameter("@AREA_Type", this.ddlType.SelectedValue),
                                                                        new DataParameter("{0}",this.txtAreaID.Text) });


                WMS.App_Code.JScript.Instance.RegisterScript(this, "UpdateParent();");
                AddOperateLog("库区管理", "修改库区信息");
            }

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            WMS.App_Code.JScript.Instance.RegisterScript(this, "window.close();");
        }


        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string whcode = this.txtWhCode.Text;
            string areaCode = this.txtAreaCode.Text;
            string areaid = this.txtAreaID.Text;
            DataTable dtTemp = bll.FillDataTable("Cmd.SelectShelf", new DataParameter[] { new DataParameter("{0}", "AREA_CODE='" + areaid + "'") });
            int count = dtTemp.Rows.Count;
            if (count > 0)
            {
                WMS.App_Code.JScript.Instance.ShowMessage(this, areaCode + "库区还有下属货架，不能删除！");
                return;
            }
            else
            {
                bll.ExecNonQuery("Cmd.DeleteArea", new DataParameter[] { new DataParameter("{0}", areaid) });
                this.txtAreaID.Text = "";
                this.txtAreaCode.Text = "";
                this.txtAreaName.Text = "";
                this.txtMemo.Text = "";
                this.txtAreaCode.ReadOnly = false;
               
                string path = whcode + "/" + areaCode;
                WMS.App_Code.JScript.Instance.RegisterScript(this, "RefreshParent('" + path + "');");
                AddOperateLog("库区管理", "删除库区信息");
            }
        }
    }
}