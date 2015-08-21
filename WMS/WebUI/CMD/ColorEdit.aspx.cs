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
    public partial class ColorEdit : WMS.App_Code.BasePage
    {
        protected string FormID;
        protected string ID;
        protected string TableName = "CMD_COLOR";
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
                    DataTable dt = bll.FillDataTable("Cmd.SelectColor", new DataParameter[] { new DataParameter("{0}", string.Format("ID={0}", ID)) });
                    BindData(dt);

                    this.txtID.ReadOnly = true;
                }
                else
                {
                    this.txtID.Text = bll.GetNewID(TableName, "ID", "1=1");
                }
                SetTextReadOnly(this.txtProduct, this.txtProductName);
                writeJsvar(FormID, TableName, PrimaryKey, ID);
                
            }
        }
        private void BindData(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                this.txtID.Text = dt.Rows[0]["ID"].ToString();
                this.txtColorID.Text = dt.Rows[0]["COLOR_CODE"].ToString();
                this.txtColor_Name.Text = dt.Rows[0]["COLOR_NAME"].ToString();
                this.txtProductModel.Text = dt.Rows[0]["PRODUCT_Model"].ToString();

                this.txtProduct.Text = dt.Rows[0]["PRODUCT_CODE"].ToString();
                this.txtProductName.Text = dt.Rows[0]["PRODUCT_NAME"].ToString();
                this.txtMemo.Text = dt.Rows[0]["Memo"].ToString();
                    
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

          
            if (ID == "") //新增
            {
                int Count = bll.GetRowCount(TableName, string.Format("COLOR_CODE='{0}' and PRODUCT_CODE='{1}'", this.txtColorID.Text.Trim(), this.txtProduct.Text.Trim()));
                if (Count > 0)
                {
                    WMS.App_Code.JScript.Instance.ShowMessage(this.Page, "该规格编码已经存在！");
                    return;
                }

                bll.ExecNonQuery("Cmd.InsertColor", new DataParameter[] {new DataParameter("@ID", this.txtID.Text.Trim()), 
                                                                         new DataParameter("@COLOR_CODE", this.txtColorID.Text.Trim()),
                                                                         new DataParameter("@PRODUCT_CODE", this.txtProduct.Text.Trim()),
                                                                          new DataParameter("@PRODUCT_Model", this.txtProductModel.Text.Trim()),
                                                                               new DataParameter("@COLOR_NAME", this.txtColor_Name.Text.Trim()), 
                                                                               new DataParameter("@Memo", this.txtMemo.Text.Trim()),
                                                                               new DataParameter("@Creator", Session["EmployeeCode"].ToString()),
                                                                               new DataParameter("@Updater",Session["EmployeeCode"].ToString())  });
            }
            else //修改
            {
                bll.ExecNonQuery("Cmd.UpdateColor", new DataParameter[] {  new DataParameter("@COLOR_CODE", this.txtColorID.Text.Trim()),
                                                                           new DataParameter("@COLOR_NAME", this.txtColor_Name.Text.Trim()),
                                                                           new DataParameter("@PRODUCT_CODE", this.txtProduct.Text.Trim()), 
                                                                            new DataParameter("@PRODUCT_Model", this.txtProductModel.Text.Trim()),       
                                                                           new DataParameter("@Memo", this.txtMemo.Text.Trim()),
                                                                           new DataParameter("@Updater",Session["EmployeeCode"].ToString()),
                                                                                   new DataParameter("{0}", this.txtID.Text.Trim()) });
            }

            Response.Redirect(FormID + "View.aspx?SubModuleCode=" + SubModuleCode + "&FormID=" + Server.UrlEncode(FormID) + "&ID=" + Server.UrlEncode(this.txtID.Text));
        }
    }
}