using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using IDAL;

namespace WMS.WebUI.CMD
{
    public partial class FactoryView : WMS.App_Code.BasePage
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
                DataTable dt = bll.FillDataTable("Cmd.SelectFactory", new DataParameter[] { new DataParameter("{0}", string.Format("FactoryID='{0}'", ID)) });
                BindData(dt);
                writeJsvar(FormID, TableName, PrimaryKey, ID);
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

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string strID = this.txtID.Text;
            int Count = bll.GetRowCount("VUsed_CMD_Factory", string.Format("FactoryID='{0}'", this.txtID.Text.Trim()));
            if (Count > 0)
            {
                WMS.App_Code.JScript.Instance.ShowMessage(this.updatePanel, "该厂商编号还被其它单据使用，请调整后再删除！");
                return;
            }
            bll.ExecNonQuery("Cmd.DeleteFactory", new DataParameter[] { new DataParameter("{0}", "'" + strID + "'") });


            btnNext_Click(sender, e);
            if (this.txtID.Text == strID)
                btnPre_Click(sender, e);

        }
        
        

        #region 上下笔事件
        protected void btnFirst_Click(object sender, EventArgs e)
        {
            BindData(bll.GetRecord("F", TableName, "1=1", PrimaryKey, this.txtID.Text));
        }
        protected void btnPre_Click(object sender, EventArgs e)
        {
            BindData(bll.GetRecord("P", TableName, "1=1", PrimaryKey, this.txtID.Text));
        }
        protected void btnNext_Click(object sender, EventArgs e)
        {
            BindData(bll.GetRecord("N", TableName, "1=1", PrimaryKey, this.txtID.Text));
        }
        protected void btnLast_Click(object sender, EventArgs e)
        {
            BindData(bll.GetRecord("L", TableName, "1=1", PrimaryKey, this.txtID.Text));
        }
        #endregion

    }
}