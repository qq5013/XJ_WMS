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
    public partial class CustomerEdit : WMS.App_Code.BasePage
    {
        protected string FormID;
        protected string ID;
        protected string TableName = "CMD_CUSTOMER";
        protected string PrimaryKey = "CUSTOMER_CODE";
        BLL.BLLBase bll = new BLL.BLLBase();

        protected void Page_Load(object sender, EventArgs e)
        {
            ID = Request.QueryString["ID"] + "";
            FormID = Request.QueryString["FormID"] + "";
            if (!IsPostBack)
            { 
                if (ID != "")
                {
                    DataTable dt = bll.FillDataTable("Cmd.SelectCustomer", new DataParameter[] { new DataParameter("{0}", string.Format("CUSTOMER_CODE='{0}'", ID)) });
                    BindData(dt);

                    this.txtID.ReadOnly = true;
                }
                else
                {
                    this.txtID.Text = bll.GetNewID(TableName, PrimaryKey, "1=1");
                    BindDataSub();
                }
                SetTextReadOnly(this.txtPLAYCUSTOMER_CODE, this.txtPlayCustomer_Name);

                writeJsvar(FormID,SqlCmd, ID);
                
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
                this.txtAreaStation.Text = dt.Rows[0]["AreaSation"].ToString();
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
            Session[FormID + "_Edit_dgViewSub1"] = dt;

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


        protected void dgViewSub1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.RowIndex % 2 == 1)
                    e.Row.CssClass = " bottomtable";

                DataRowView drv = e.Row.DataItem as DataRowView;

                ((Label)e.Row.Cells[1].FindControl("RowID")).Text = "" + ((e.Row.RowIndex + 1) + dgViewSub1.PageSize * dgViewSub1.PageIndex);
                ((TextBox)e.Row.Cells[2].FindControl("PERSON")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("PERSON")].ToString();
                ((TextBox)e.Row.Cells[3].FindControl("PHONE")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("PHONE")].ToString();
                ((TextBox)e.Row.Cells[4].FindControl("Address")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("Address")].ToString();
                ((TextBox)e.Row.Cells[4].FindControl("OrderNum")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("OrderNum")].ToString();
                 

            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            UpdateTempSub(this.dgViewSub1);
            string[] Commands = new string[3];
            DataParameter[] para;

            int CustomerType=0;
            if (this.opt1.Checked)
                CustomerType = 1;
            else if (this.opt2.Checked)
                CustomerType = 2;
            else if (this.opt3.Checked)
                CustomerType = 3;
            else if (this.opt4.Checked)
                CustomerType = 4;
            else if (this.opt5.Checked)
                CustomerType = 5;

            if (ID == "") //新增
            {
                int Count = bll.GetRowCount(TableName, string.Format("CUSTOMER_CODE='{0}'", this.txtID.Text.Trim()));
                if (Count > 0)
                {
                    WMS.App_Code.JScript.Instance.ShowMessage(this.Page, "该客户编码已经存在！");
                    return;
                }

                para = new DataParameter[] { new DataParameter("@CUSTOMER_CODE", this.txtID.Text.Trim()),
                                            new DataParameter("@CUSTOMER_NAME", this.txtCustomer_Name.Text.Trim()),
                                            new DataParameter("@CUSTOMER_PERSON", this.txtCustomer_Person.Text.Trim()),
                                            new DataParameter("@CUSTOMER_PHONE", this.txtCustomer_Phone.Text.Trim()),
                                            new DataParameter("@CUSTOMER_Fax", this.txtFax.Text.Trim()), 
                                            new DataParameter("@PLAYCUSTOMER_CODE",(this.txtPLAYCUSTOMER_CODE.Text.Trim()==""?this.txtID.Text.Trim():this.txtPLAYCUSTOMER_CODE.Text.Trim())), 
                                            new DataParameter("@CustomerType",CustomerType),
                                            new DataParameter("@MEMO", this.txtMemo.Text.Trim()),
                                            new DataParameter("@AreaSation",this.txtAreaStation.Text.Trim()),
                                            new DataParameter("@Creator", Session["EmployeeCode"].ToString()),
                                            new DataParameter("@Updater",Session["EmployeeCode"].ToString())  };
                Commands[0] = "Cmd.InsertCustomer";
            }
            else //修改
            {
                para = new DataParameter[] { new DataParameter("@CUSTOMER_NAME", this.txtCustomer_Name.Text.Trim()),
                                            new DataParameter("@CUSTOMER_PERSON", this.txtCustomer_Person.Text.Trim()),
                                            new DataParameter("@CUSTOMER_PHONE", this.txtCustomer_Phone.Text.Trim()),
                                            new DataParameter("@CUSTOMER_Fax", this.txtFax.Text.Trim()), 
                                            new DataParameter("@PLAYCUSTOMER_CODE", (this.txtPLAYCUSTOMER_CODE.Text.Trim()==""?this.txtID.Text.Trim():this.txtPLAYCUSTOMER_CODE.Text.Trim())),
                                            new DataParameter("@MEMO", this.txtMemo.Text.Trim()),
                                            new DataParameter("@CustomerType",CustomerType),
                                            new DataParameter("@AreaSation",this.txtAreaStation.Text.Trim()),
                                            new DataParameter("@Updater",Session["EmployeeCode"].ToString()),
                                            new DataParameter("{0}", this.txtID.Text.Trim()) };
                Commands[0] = "Cmd.UpdateCustomer";
            }

            DataTable dt = (DataTable)Session[FormID + "_Edit_dgViewSub1"];
            DataRow[] drs = dt.Select(string.Format("CUSTOMER_CODE<>'{0}'", this.txtID.Text));
            for (int i = 0; i < drs.Length; i++)
            {
                drs[i].BeginEdit();
                drs[i]["CUSTOMER_CODE"] = this.txtID.Text.Trim();
                drs[i].EndEdit();
            }



            Commands[1] = "Cmd.DeleteCustomerSub";
            Commands[2] = "Cmd.InsertCustomerSub";
            bll.ExecTran(Commands, para, PrimaryKey, new DataTable[] { dt });


            Response.Redirect(FormID + "View.aspx?SubModuleCode=" + SubModuleCode + "&FormID=" + Server.UrlEncode(FormID) + "&ID=" + Server.UrlEncode(this.txtID.Text));
        }

        protected void btnAddDetail_Click(object sender, EventArgs e)
        { 
            UpdateTempSub(this.dgViewSub1);
            DataTable dt = (DataTable)Session[FormID + "_Edit_dgViewSub1"];
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[dt.Rows.Count - 1]["PERSON"].ToString() == "")
                {
                    return;
                }
            }
            DataRow dr;

            dr = dt.NewRow();
            dr["RowID"] = dt.Rows.Count + 1;
            dt.Rows.Add(dr);
            this.dgViewSub1.DataSource = dt;
            this.dgViewSub1.DataBind();

        }

        protected void btnDelDetail_Click(object sender, EventArgs e)
        {
            UpdateTempSub(this.dgViewSub1);
            DataTable dt = (DataTable)Session[FormID + "_Edit_" + dgViewSub1.ID];
            for (int i = 0; i < this.dgViewSub1.Rows.Count; i++)
            {
                CheckBox cb = (CheckBox)(this.dgViewSub1.Rows[i].FindControl("cbSelect"));
                if (cb != null && cb.Checked)
                {
                    Label hk = (Label)(this.dgViewSub1.Rows[i].Cells[1].FindControl("RowID"));
                    DataRow[] drs = dt.Select(string.Format("RowID ={0}", hk.Text));
                    for (int j = 0; j < drs.Length; j++)
                        dt.Rows.Remove(drs[j]);

                }
            }
            this.dgViewSub1.DataSource = dt;
            this.dgViewSub1.DataBind();
        }
        private void UpdateTempSub(GridView dgv)
        {
            DataTable dt1 = (DataTable)Session[FormID + "_Edit_" + dgv.ID];
            if (dt1.Rows.Count == 0)
                return;
            DataRow dr;
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                dr = dt1.Rows[i];
                dr.BeginEdit();
                dr["CUSTOMER_CODE"] = this.txtID.Text.Trim();
                dr["RowID"] = i + 1;
                dr["PERSON"] = ((TextBox)dgv.Rows[i].Cells[2].FindControl("PERSON")).Text;
                dr["PHONE"] = ((TextBox)dgv.Rows[i].Cells[3].FindControl("PHONE")).Text;
                dr["Address"] = ((TextBox)dgv.Rows[i].Cells[4].FindControl("Address")).Text;
                dr["OrderNum"] = ((TextBox)dgv.Rows[i].Cells[4].FindControl("OrderNum")).Text;
                dr.EndEdit();
            }
            dt1.AcceptChanges();
            Session[FormID + "_Edit_" + dgv.ID] = dt1;
        }
    }
}