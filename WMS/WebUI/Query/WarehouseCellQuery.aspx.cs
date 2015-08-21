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
    public partial class WarehouseCellQuery : WMS.App_Code.BasePage
    {
         
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BLL.BLLBase bll = new BLL.BLLBase();
                DataTable dt = bll.FillDataTable("Cmd.selectWareHouseQuery", null);
                this.hidetree.Value = Util.JsonHelper.DataTableToJSON(dt);
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.UpdatePanel1.GetType(), "Resize", "treeview_resize();", true);  

            }

        }

    }
}