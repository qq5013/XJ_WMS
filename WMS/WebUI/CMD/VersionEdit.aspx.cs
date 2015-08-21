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
    public partial class VersionEdit : WMS.App_Code.BasePage
    {
        protected string FormID;
        protected string ID;
        protected string TableName = "CMD_VERSION";
        protected string PrimaryKey = "ID";
        BLL.BLLBase bll = new BLL.BLLBase();

        protected void Page_Load(object sender, EventArgs e)
        {
            ID = Request.QueryString["ID"] + "";
            FormID = Request.QueryString["FormID"] + "";
            if (!IsPostBack)
            {
                if (ID != "")
                {
                    DataTable dt = bll.FillDataTable("Cmd.SelectVersion", new DataParameter[] { new DataParameter("{0}", string.Format("ID={0}", ID)) });
                    BindData(dt);

                    this.txtID.ReadOnly = true;
                }
                else
                {
                    this.txtVersionID.Text = bll.GetNewID(TableName, "VERSION_CODE", "1=1");
                    this.txtID.Text = bll.GetNewID(TableName, "ID", "1=1");
                }
                writeJsvar(FormID, TableName, PrimaryKey, ID);
                
            }
        }
        private void BindData(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                this.txtID.Text = dt.Rows[0]["ID"].ToString();
                this.txtVersionID.Text = dt.Rows[0]["Version_CODE"].ToString();
                this.txtVersion_Name.Text = dt.Rows[0]["Version_NAME"].ToString();
                this.txtProduct.Text = dt.Rows[0]["PRODUCT_CODE"].ToString();
                this.txtProductName.Text = dt.Rows[0]["PRODUCT_NAME"].ToString();
                this.txtMemo.Text = dt.Rows[0]["Memo"].ToString();
                    
            }
        }
      
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (ID == "") //新增
            {
                int Count = bll.GetRowCount(TableName, string.Format("Version_CODE='{0}' and PRODUCT_CODE='{1}'", this.txtVersionID.Text.Trim(), this.txtProduct.Text.Trim()));
               if(Count>0)
                {
                    WMS.App_Code.JScript.Instance.ShowMessage(this.Page, "该版本编码已经存在！");
                    return;
                }

               bll.ExecNonQuery("Cmd.InsertVersion", new DataParameter[] { new DataParameter("@ID", this.txtID.Text.Trim()),
                                                                           new DataParameter("@VERSION_CODE", this.txtVersionID.Text.Trim()),
                                                                           new DataParameter("@VERSION_NAME", this.txtVersion_Name.Text.Trim()), 
                                                                           new DataParameter("@PRODUCT_CODE", this.txtProduct.Text.Trim()), 
                                                                           new DataParameter("@Memo", this.txtMemo.Text.Trim()) });
            }
            else //修改
            {
                bll.ExecNonQuery("Cmd.UpdateVersion", new DataParameter[] { new DataParameter("@VERSION_CODE", this.txtVersionID.Text.Trim()),
                                                                                  new DataParameter("@VERSION_NAME", this.txtVersion_Name.Text.Trim()),
                                                                                   new DataParameter("@PRODUCT_CODE", this.txtProduct.Text.Trim()), 
                                                                                   new DataParameter("@Memo", this.txtMemo.Text.Trim()),
                                                                                   new DataParameter("{0}", this.txtID.Text.Trim()) });
            }

            Response.Redirect(FormID + "View.aspx?SubModuleCode=" + SubModuleCode + "&FormID=" + Server.UrlEncode(FormID) + "&ID=" + Server.UrlEncode(this.txtID.Text));
        }
    }
}