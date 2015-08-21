using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WMS.WebUI.InStock
{
    public partial class TempPage : System.Web.UI.Page
    {
        
        protected string ID;
      
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ID = Request.Params["ID"] + "";
            }
        }
    }
}