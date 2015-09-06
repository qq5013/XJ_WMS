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
    public partial class FactoryEdit : WMS.App_Code.BasePage
    {
        protected string FormID;
        protected string ID;
        protected string TableName = "CMD_Factory";
        protected string PrimaryKey = "FactoryID";
        BLL.BLLBase bll = new BLL.BLLBase();

        protected void Page_Load(object sender, EventArgs e)
        {
            ID = Request.QueryString["ID"] + "";
            FormID = Request.QueryString["FormID"] + "";
            if (!IsPostBack)
            {
                if (ID != "")
                {
                    DataTable dt = bll.FillDataTable("Cmd.SelectFactory", new DataParameter[] { new DataParameter("{0}", string.Format("FactoryID='{0}'", ID)) });
                    BindData(dt);

                    this.txtID.ReadOnly = true;
                }
                else
                {
                    this.txtID.Text = bll.GetNewID(TableName, PrimaryKey, "1=1");
                }
                writeJsvar(FormID,SqlCmd, ID);
                
            }
        }
        private void BindData(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                this.txtID.Text = dt.Rows[0]["FactoryID"].ToString();
                this.txtFactoryName.Text = dt.Rows[0]["FactoryName"].ToString();
                this.txtLinkPerson.Text = dt.Rows[0]["LinkPerson"].ToString();
                this.txtLinkPhone.Text = dt.Rows[0]["LinkPhone"].ToString();
                this.txtFax.Text = dt.Rows[0]["Fax"].ToString();
                this.txtAddress.Text = dt.Rows[0]["Address"].ToString();
                this.txtMemo.Text = dt.Rows[0]["Memo"].ToString();
                    
            }
        }
      
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (ID == "") //新增   FactoryID, FactoryName, LinkPerson, LinkPhone, Fax, Address, MEMO
            {
                int Count = bll.GetRowCount(TableName, string.Format("FactoryID='{0}'", this.txtID.Text.Trim()));
                if (Count > 0)
                {
                    WMS.App_Code.JScript.Instance.ShowMessage(this.Page, "该厂商编码已经存在！");
                    return;
                }

                bll.ExecNonQuery("Cmd.InsertFactory", new DataParameter[] { new DataParameter("@FactoryID", this.txtID.Text.Trim()),
                                                                             new DataParameter("@FactoryName", this.txtFactoryName.Text.Trim()),
                                                                             new DataParameter("@LinkPerson", this.txtLinkPerson.Text.Trim()),
                                                                             new DataParameter("@LinkPhone", this.txtLinkPhone.Text.Trim()),
                                                                             new DataParameter("@Fax", this.txtFax.Text.Trim()), 
                                                                             new DataParameter("@Address", this.txtAddress.Text.Trim()),
                                                                             new DataParameter("@MEMO", this.txtMemo.Text.Trim()),
                                                                            new DataParameter("@Creator", Session["EmployeeCode"].ToString()),
                                                                            new DataParameter("@Updater",Session["EmployeeCode"].ToString())  });
            }
            else //修改
            {
                bll.ExecNonQuery("Cmd.UpdateFactory", new DataParameter[] {  new DataParameter("@FactoryName", this.txtFactoryName.Text.Trim()),
                                                                             new DataParameter("@LinkPerson", this.txtLinkPerson.Text.Trim()),
                                                                             new DataParameter("@LinkPhone", this.txtLinkPhone.Text.Trim()),
                                                                             new DataParameter("@Fax", this.txtFax.Text.Trim()), 
                                                                             new DataParameter("@Address", this.txtAddress.Text.Trim()),
                                                                             new DataParameter("@MEMO", this.txtMemo.Text.Trim()),
                                                                             new DataParameter("@Updater",Session["EmployeeCode"].ToString()),
                                                                             new DataParameter("{0}", this.txtID.Text.Trim()) });
            }

            Response.Redirect(FormID + "View.aspx?SubModuleCode=" + SubModuleCode + "&FormID=" + Server.UrlEncode(FormID) + "&ID=" + Server.UrlEncode(this.txtID.Text));
        }
    }
}