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
    public partial class CustomerView : WMS.App_Code.BasePage
    {
        protected string FormID;
        protected string ID;
        protected string TableName = "View_CMD_CUSTOMER";
        protected string PrimaryKey = "CUSTOMER_CODE";

        BLL.BLLBase bll = new BLL.BLLBase();
        protected void Page_Load(object sender, EventArgs e)
        {
            ID = Request.QueryString["ID"] + "";
            FormID = Request.QueryString["FormID"] + "";
            if (!IsPostBack)
            {
                DataTable dt = bll.FillDataTable("Cmd.SelectCustomer", new DataParameter[] { new DataParameter("{0}", string.Format("CUSTOMER_CODE='{0}'", ID)) });
                BindData(dt);
                writeJsvar(FormID, TableName, PrimaryKey, ID);
            }
        }

        private void BindData(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                this.txtID.Text = dt.Rows[0]["CUSTOMER_CODE"].ToString();
                this.txtCustomer_Name.Text = dt.Rows[0]["CUSTOMER_NAME"].ToString();
                this.txtPLAYCUSTOMER_CODE.Text = dt.Rows[0]["PLAYCUSTOMER_CODE"].ToString();
                this.txtPlayCustomer_Name.Text = dt.Rows[0]["PlayCustomer_Name"].ToString();

                this.txtCustomer_Person.Text = dt.Rows[0]["CUSTOMER_PERSON"].ToString();
                this.txtCustomer_Phone.Text = dt.Rows[0]["CUSTOMER_PHONE"].ToString();
                this.txtFax.Text = dt.Rows[0]["CUSTOMER_Fax"].ToString();
                this.txtMemo.Text = dt.Rows[0]["Memo"].ToString();
                this.txtAreaSation.Text = dt.Rows[0]["AreaSation"].ToString();
                switch (dt.Rows[0]["CustomerType"].ToString())
                {
                    case "1":
                        this.opt1.Checked = true;
                        break;
                    case "2":
                        this.opt2.Checked = true;
                        break;
                    case "3":
                        this.opt3.Checked = true;
                        break;
                    case "4":
                        this.opt4.Checked = true;
                        break;
                    case "5":
                        this.opt5.Checked = true;
                        break;
                }
                BindDataSub();
                
            }
        }
        private void BindDataSub()
        {
            DataTable dt = bll.FillDataTable("CMD.SelectCustomerSub", new DataParameter[] { new DataParameter("{0}", string.Format("CUSTOMER_CODE='{0}'", this.txtID.Text)) });
            Session[FormID + "_View_dgViewSub1"] = dt;

            if (dt.Rows.Count == 0)
            {
                DataTable dtNew = dt.Clone();
                dtNew.Rows.Add(dtNew.NewRow());
                dgViewSub1.DataSource = dtNew;
                dgViewSub1.DataBind();
                int columnCount = dgViewSub1.Rows[0].Cells.Count;
                dgViewSub1.Rows[0].Cells.Clear();
                dgViewSub1.Rows[0].Cells.Add(new TableCell());
                dgViewSub1.Rows[0].Cells[0].ColumnSpan = columnCount;
                dgViewSub1.Rows[0].Cells[0].Text = "没有明细资料，请新增";
                dgViewSub1.Rows[0].Visible = true;
            }
            else
            {
                dgViewSub1.DataSource = dt;
                dgViewSub1.DataBind();
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string strID = this.txtID.Text;

            int Count = bll.GetRowCount("VUsed_CMD_Customer", string.Format("CustomerID='{0}'", this.txtID.Text.Trim()));
            if (Count > 0)
            {
                WMS.App_Code.JScript.Instance.ShowMessage(this.updatePanel, "该客户编号还被其它单据使用，请调整后再删除！");
                return;
            }

            string[] comds = new string[2];
            comds[0] = "Cmd.DeleteCustomer";
            comds[1] = "Cmd.DeleteCustomerSub";
            List<DataParameter[]> paras = new List<DataParameter[]>();
            paras.Add(new DataParameter[] { new DataParameter("{0}", string.Format("CUSTOMER_CODE='{0}'", strID)) });
            paras.Add(new DataParameter[] { new DataParameter("{0}", string.Format("CUSTOMER_CODE='{0}'", strID)) });
            bll.ExecTran(comds, paras);
            AddOperateLog("客户资料", "删除单号：" + strID);

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