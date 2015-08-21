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
    public partial class WarehouseEditPage : WMS.App_Code.BasePage
    {
        
        BLL.BLLBase bll = new BLL.BLLBase();
        DataTable dtHouse;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["WAREHOUSE_CODE"] != null)
                {
                   
                    dtHouse = bll.FillDataTable("Cmd.SelectWarehouse", new DataParameter[] { new DataParameter("{0}", "WAREHOUSE_CODE='" + Request.QueryString["WAREHOUSE_CODE"] + "'") });

                    this.txtWHID.Text = dtHouse.Rows[0]["CMD_WAREHOUSE_ID"].ToString();
                    this.txtWhCode.Text = dtHouse.Rows[0]["WAREHOUSE_CODE"].ToString();
                    this.txtWhName.Text = dtHouse.Rows[0]["WAREHOUSE_NAME"].ToString();
                    this.txtMemo.Text = dtHouse.Rows[0]["MEMO"].ToString();
                    this.txtK3.Text = dtHouse.Rows[0]["OtherCode"].ToString();
                    this.txtWhCode.ReadOnly = true;
                    
                }
                else
                {
                    this.txtWhCode.Text = bll.GetNewID("CMD_WAREHOUSE", "WAREHOUSE_CODE", "1=1");
                }
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (this.txtWHID.Text.Trim().Length == 0)//新增
            {
                int count = bll.GetRowCount("CMD_WAREHOUSE", string.Format("WAREHOUSE_CODE='{0}'", this.txtWhCode.Text));
                if (count > 0)
                {
                    WMS.App_Code.JScript.Instance.ShowMessage(this, "此仓库编码已存在，不能新增！");
                    return;
                }
                //新增
                bll.ExecNonQuery("Cmd.InsertWarehouse", new DataParameter[] { new DataParameter("@WAREHOUSE_CODE", this.txtWhCode.Text), new DataParameter("@WAREHOUSE_NAME", this.txtWhName.Text.Trim().Replace("\'", "\''")), new DataParameter("@MEMO", this.txtMemo.Text.Trim()),new DataParameter("@OtherCode",this.txtK3.Text) });
                WMS.App_Code.JScript.Instance.RegisterScript(this, "ReloadParent();");
                AddOperateLog("仓库管理", "添加仓库信息");
            }
            else
            {
                //更新
                bll.ExecNonQuery("Cmd.UpdateWarehouse", new DataParameter[] { new DataParameter("@WAREHOUSE_CODE", this.txtWhCode.Text), new DataParameter("@WAREHOUSE_NAME", this.txtWhName.Text.Trim().Replace("\'", "\''")), 
                                        new DataParameter("@MEMO", this.txtMemo.Text.Trim()),new DataParameter("@OtherCode",this.txtK3.Text),new DataParameter("{0}",this.txtWHID.Text) });

                WMS.App_Code.JScript.Instance.RegisterScript(this, "UpdateParent();");
                AddOperateLog("仓库管理", "修改仓库信息");
            }

            WMS.App_Code.JScript.Instance.RegisterScript(this, "window.close();");
        }


        protected void btnCancel_Click(object sender, EventArgs e)
        {
            WMS.App_Code.JScript.Instance.RegisterScript(this, "window.close();");
        }


        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string whcode = this.txtWhCode.Text;
            DataTable dtTemp = bll.FillDataTable("Cmd.SelectArea", new DataParameter[] { new DataParameter("{0}", "WAREHOUSE_CODE='" + whcode + "'") });
            int count = dtTemp.Rows.Count;

            
            if (count > 0)
            {
               WMS.App_Code.JScript.Instance.ShowMessage(this, whcode + "还有下属库区，不能删除！");
                return;
            }
            else
            {
                bll.ExecNonQuery("Cmd.DeleteWarehouse", new DataParameter[] { new DataParameter("{0}", whcode) });

                this.txtWHID.Text = "";
                this.txtWhName.Text = "";
                this.txtMemo.Text = "";
                this.txtWhCode.ReadOnly = false;
                
                string path = whcode;
                WMS.App_Code.JScript.Instance.RegisterScript(this, "RefreshParent('" + path + "');");
            }
            AddOperateLog("仓库管理", "删除仓库信息");
        }
    }
}