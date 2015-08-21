using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using IDAL;

namespace WMS.WebUI.InStock
{
    public partial class InStockReturnEdit : WMS.App_Code.BasePage
    {
        protected string FormID;
        protected string ID;
        protected string TableName = "View_WMS_InStockReturn";
        protected string PrimaryKey = "BillID";
        protected string SubKey = "Flag,BillID";
        protected int Flag =4;

        BLL.BLLBase bll = new BLL.BLLBase();

        protected void Page_Load(object sender, EventArgs e)
        {
            ID = Request.QueryString["ID"] + "";
            FormID = Request.QueryString["FormID"] + "";
            if (!IsPostBack)
            {
                if (ID != "")
                {
                    DataTable dt = bll.FillDataTable("WMS.SelectInStockReturn", new DataParameter[] { new DataParameter("{0}", string.Format("{0}='{1}' and Flag={2}", PrimaryKey, ID, Flag)) });
                    BindData(dt);
                    SetTextReadOnly(this.txtID);
                    this.txtBillDate.ReadOnly = true;
                    this.btnQuery.Visible = false;
                }
                else
                {
                    txtBillDate.changed = "$('#txtID').val(autoCode('IR','Flag=4','txtBillDate'));";
                    this.txtBillDate.DateValue = DateTime.Now;
                    this.txtID.Text = bll.GetAutoCode("IR", DateTime.Now, "Flag=4");
                    this.txtCreator.Text = Session["EmployeeCode"].ToString();
                    this.txtCreateDate.Text = ToYMD(DateTime.Now);

                    BindDataSub();
                    BindDataSub2();
                }
                BindPageSize();
                writeJsvar(FormID, TableName, PrimaryKey, ID);
                SetTextReadOnly(this.txtCreateDate,this.txtFactoryName,   this.txtCreator);

            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "", "BindEvent()", true);
            }
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "Resize", "content_resize();", true);
        }
        private void BindData(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                this.txtID.Text = dt.Rows[0]["BillID"].ToString();
                this.txtBillDate.DateValue = dt.Rows[0]["BillDate"];
              
                this.txtFactoryID.Text = dt.Rows[0]["CustomerID"].ToString();
                this.txtFactoryName.Text = dt.Rows[0]["CustName"].ToString();
               
                this.txtMemo.Text = dt.Rows[0]["Memo"].ToString();
                this.txtCreator.Text = dt.Rows[0]["Creator"].ToString();
                this.txtCreateDate.Text = ToYMD(dt.Rows[0]["CreateDate"]);

                BindDataSub();
                BindDataSub2();
            }
        }

        private void BindDataSub()
        {
            DataTable dt = bll.FillDataTable("WMS.SelectTransferSub", new DataParameter[] { new DataParameter("{0}", string.Format("BillID='{0}' and Flag={1}", this.txtID.Text, Flag)) });
            Session[FormID + "_Edit_dgViewSub1"] = dt;

            if (dt.Rows.Count == 0)
            {
                BindDataSubNotDetail(this.dgViewSub1, dt);
            }
            else
            {
                dgViewSub1.DataSource = dt;
                dgViewSub1.DataBind();
            }
            SetPageSubBtnEnabled();

        }

        private void BindDataSub2()
        {
            DataTable dt = bll.FillDataTable("WMS.SelectTransferDetail", new DataParameter[] { new DataParameter("{0}", string.Format("BillID='{0}' and Flag={1}", this.txtID.Text, Flag)) });
            Session[FormID + "_Edit_dgViewSub2"] = dt;

            if (dt.Rows.Count == 0)
            {
                BindDataSubNotDetail(this.dgViewSub2, dt);
            }
            else
            {
                dgViewSub2.DataSource = dt;
                dgViewSub2.DataBind();
            }
            SetPageSub2BtnEnabled();

        }

      

        private void UpdateTempSub(GridView dgv)
        {
            DataTable dt1 = (DataTable)Session[FormID + "_Edit_" + dgv.ID];
            DataRow dr;
            if (dt1.Rows.Count == 0)
            {
                BindDataSubNotDetail(dgv, dt1);
                return;
            }
            if (dgv.ID.IndexOf("1") > 0)
            {
                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    dr = dt1.Rows[i + dgv.PageSize * dgv.PageIndex];
                    dr.BeginEdit();
                    dr["Qty"] = ((TextBox)dgv.Rows[i].FindControl("Qty")).Text;
                    dr.EndEdit();
                }
            }
            dt1.AcceptChanges();
            Session[FormID + "_Edit_" + dgv.ID] = dt1;
        }



        protected void btnSave_Click(object sender, EventArgs e)
        {
            UpdateTempSub(this.dgViewSub1);
            string[] Commands = new string[5];
            DataParameter[] para;

            if (ID == "") //新增   
            {
                int Count = bll.GetRowCount(TableName, string.Format("BillID='{0}' and Flag={1}", this.txtID.Text.Trim(), Flag));
                if (Count > 0)
                {
                    WMS.App_Code.JScript.Instance.ShowMessage(this.UpdatePanel1, "该调拨单号已经存在！");
                    return;
                }


                para = new DataParameter[] { 
                                             new DataParameter("@Flag", Flag),  
                                             new DataParameter("@BillID", this.txtID.Text.Trim()),
                                             new DataParameter("@BillDate", this.txtBillDate.DateValue),
                                             new DataParameter("@OutStockID", 3),
                                             new DataParameter("@CustomerID", this.txtFactoryID.Text),
                                             new DataParameter("@TranCustomerID", ""),
                                             new DataParameter("@Memo", this.txtMemo.Text.Trim()),
                                             new DataParameter("@Creator", this.txtCreator.Text.Trim()),
                                             new DataParameter("@Updater", Session["EmployeeCode"].ToString())
                                             
                                              };
                Commands[0] = "WMS.InsertOtherDeliver";

            }
            else //修改
            {
                para = new DataParameter[] { 
                                             new DataParameter("@Flag", Flag),  
                                             new DataParameter("@BillID", this.txtID.Text.Trim()),
                                             new DataParameter("@BillDate", this.txtBillDate.DateValue),
                                             new DataParameter("@OutStockID", 3),
                                             new DataParameter("@CustomerID", this.txtFactoryID.Text),
                                             new DataParameter("@TranCustomerID", ""),
                                             new DataParameter("@Memo", this.txtMemo.Text.Trim()),
                                             new DataParameter("@Creator", this.txtCreator.Text.Trim()),
                                             new DataParameter("@Updater", Session["EmployeeCode"].ToString()),
                                             new DataParameter("{0}",string.Format("Flag={0} and BillID='{1}'",Flag, this.txtID.Text.Trim())) };
                Commands[0] = "WMS.UpdateOtherDeliver";
            }

            DataTable dtDetail = (DataTable)Session[FormID + "_Edit_dgViewSub2"];
            DataRow[] drs = dtDetail.Select("BillID<>'" + this.txtID.Text + "'");
            for (int i = 0; i < drs.Length; i++)
            {
                drs[i].BeginEdit();
                drs[i]["BillID"] = this.txtID.Text.Trim();
                drs[i].EndEdit();
            }

            DataTable dt = (DataTable)Session[FormID + "_Edit_dgViewSub1"];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i].BeginEdit();
                dt.Rows[i]["BillID"] = this.txtID.Text.Trim();
                dt.Rows[i].EndEdit();

              
                int Qty = int.Parse(dt.Rows[i]["Qty"].ToString());

                //获取未出库数量
                int ProductQty = int.Parse(bll.GetFieldValue("CMD_WareHouseProduct", "Qty", string.Format("Flag={0} and ProductID='{1}' and ColorID='{2}'", 3, dt.Rows[i]["ProductID"], dt.Rows[i]["ColorID"])));
                //获取未审核数量

                int UnCheckQty = 0;
                DataTable dtValue = bll.FillDataTable("WMS.SelectOtherDeliverNoCheck",new DataParameter[]{ new DataParameter("@BillID",this.txtID.Text),new DataParameter("@ProductID",dt.Rows[i]["ProductID"]),new DataParameter("@ColorID",dt.Rows[i]["ColorID"]),new DataParameter("@Flag", 3)});
                   
                if (dt.Rows.Count >0)
                    UnCheckQty = int.Parse(dtValue.Rows[0][0].ToString());
                if (ProductQty - Qty - UnCheckQty < 0)
                {
                    WMS.App_Code.JScript.Instance.ShowMessage(this.UpdatePanel1, dt.Rows[i]["ProductModel"].ToString() + "( " + dt.Rows[i]["ColorName"].ToString() + " ) 库存数量不足，请修改！");
                    return;
                }



                //判断序号组合是否成套。
                int ProdCount = bll.GetRowCount("CMD_PRODUCT", string.Format("substring(PRODUCT_CODE,1,5)='{0}' and IsInStock=1", dt.Rows[i]["ProductID"]));
                
                DataRow[] drsDetail = dtDetail.Select(string.Format("substring(ProductID,1,5)='{0}' and ColorID='{1}'", dt.Rows[i]["ProductID"], dt.Rows[i]["ColorID"]));
                if (drsDetail.Length > 0)
                {

                    int ProdQty = drsDetail.Length / ProdCount;
                    if (Qty != ProdQty)
                    {
                        WMS.App_Code.JScript.Instance.ShowMessage(this.UpdatePanel1, dt.Rows[i]["ProductModel"].ToString() + "( " + dt.Rows[i]["ColorName"].ToString() + " )" + " 出库数量与出库序号不成比例！");
                        return;
 
                    }
                }

            }


            Commands[1] = "WMS.DeleteOtherDeliverSub";
            Commands[2] = "WMS.InsertOtherDeliverSub";

            Commands[3] = "WMS.DeleteOtherDeliverDetail";
            Commands[4] = "WMS.InsertOtherDeliverDetail";

            bll.ExecTran(Commands, para, SubKey, new DataTable[] { dt,dtDetail });



            Response.Redirect(FormID + "View.aspx?SubModuleCode=" + SubModuleCode + "&FormID=" + Server.UrlEncode(FormID) + "&ID=" + Server.UrlEncode(this.txtID.Text));
        }

        #region 从表资料显示


        private void SetPageSubBtnEnabled()
        {
            DataTable dt = (DataTable)Session[FormID + "_Edit_dgViewSub1"];
            if (dt.Rows.Count > 0)
            {
                bool blnvalue = dgViewSub1.PageCount > 1 ? true : false;
                this.btnLastSub1.Enabled = blnvalue;
                this.btnFirstSub1.Enabled = blnvalue;
                this.btnToPageSub1.Enabled = blnvalue;


                if (this.dgViewSub1.PageIndex >= 1)
                    this.btnPreSub1.Enabled = true;
                else
                    this.btnPreSub1.Enabled = false;

                if ((this.dgViewSub1.PageIndex + 1) < dgViewSub1.PageCount)
                    this.btnNextSub1.Enabled = true;
                else
                    this.btnNextSub1.Enabled = false;

                lblCurrentPageSub1.Visible = true;
                lblCurrentPageSub1.Text = "共 [" + dt.Rows.Count.ToString() + "] 条记录  页次 " + (dgViewSub1.PageIndex + 1) + " / " + dgViewSub1.PageCount.ToString();
            }
            else
            {
                this.btnFirstSub1.Enabled = false;
                this.btnPreSub1.Enabled = false;
                this.btnNextSub1.Enabled = false;
                this.btnLastSub1.Enabled = false;
                this.btnToPageSub1.Enabled = false;
                lblCurrentPageSub1.Visible = false;
            }

        }


        private void BindPageSize()
        {
            this.ddlPageSizeSub1.Items.Clear();
            this.ddlPageSizeSub1.Items.Add(new ListItem("10", "10"));
            this.ddlPageSizeSub1.Items.Add(new ListItem("20", "20"));
            this.ddlPageSizeSub1.Items.Add(new ListItem("30", "30"));
            this.ddlPageSizeSub1.Items.Add(new ListItem("40", "40"));
            this.ddlPageSizeSub1.Items.Add(new ListItem("50", "50"));
            this.ddlPageSizeSub1.Items.Add(new ListItem("100", "100"));

            this.ddlPageSizeSub2.Items.Add(new ListItem("10", "10"));
            this.ddlPageSizeSub2.Items.Add(new ListItem("20", "20"));
            this.ddlPageSizeSub2.Items.Add(new ListItem("30", "30"));
            this.ddlPageSizeSub2.Items.Add(new ListItem("40", "40"));
            this.ddlPageSizeSub2.Items.Add(new ListItem("50", "50"));
            this.ddlPageSizeSub2.Items.Add(new ListItem("100", "100"));
        }

        protected void ddlPageSizeSub1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.HdfActiveTab.Value = "0";
            DataTable dt1 = (DataTable)Session[FormID + "_Edit_dgViewSub1"];
            this.dgViewSub1.DataSource = dt1;
            this.dgViewSub1.PageSize = int.Parse(ddlPageSizeSub1.Text);
            this.dgViewSub1.DataBind();
            SetPageSubBtnEnabled();
        }

        public void MovePage(GridView dgv, int pageindex)
        {

            int pindex = pageindex;
            if (pindex < 0) pindex = 0;
            if (pindex >= dgv.PageCount) pindex = dgv.PageCount - 1;
            DataTable dt1 = (DataTable)Session[FormID + "_Edit_" + dgv.ID];
            dgv.PageIndex = pindex;
            dgv.DataSource = dt1;
            dgv.DataBind();


            if (dgv.ID.IndexOf("1") > 1)
                SetPageSubBtnEnabled();
            else
                SetPageSub2BtnEnabled();
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "Resize", "content_resize();", true);
        }

        protected void btnFirstSub1_Click(object sender, EventArgs e)
        {
            this.HdfActiveTab.Value = "0";
            MovePage(this.dgViewSub1, 0);
        }

        protected void btnPreSub1_Click(object sender, EventArgs e)
        {
            this.HdfActiveTab.Value = "0";
            MovePage(this.dgViewSub1, this.dgViewSub1.PageIndex - 1);
        }

        protected void btnNextSub1_Click(object sender, EventArgs e)
        {
            this.HdfActiveTab.Value = "0";
            MovePage(this.dgViewSub1, this.dgViewSub1.PageIndex + 1);
        }

        protected void btnLastSub1_Click(object sender, EventArgs e)
        {
            this.HdfActiveTab.Value = "0";
            MovePage(this.dgViewSub1, this.dgViewSub1.PageCount - 1);
        }

        protected void btnToPageSub1_Click(object sender, EventArgs e)
        {
            this.HdfActiveTab.Value = "0";
            MovePage(this.dgViewSub1, int.Parse(this.txtPageNoSub1.Text) - 1);
        }
        #endregion

        #region 从表2显示
        protected void ddlPageSizeSub2_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.HdfActiveTab.Value = "1";
            DataTable dt1 = (DataTable)Session[FormID + "_Edit_dgViewSub2"];
            this.dgViewSub2.DataSource = dt1;
            this.dgViewSub2.PageSize = int.Parse(ddlPageSizeSub2.Text);
            this.dgViewSub2.DataBind();
            SetPageSub2BtnEnabled();
        }

        private void SetPageSub2BtnEnabled()
        {
            DataTable dt = (DataTable)Session[FormID + "_Edit_dgViewSub2"];
            if (dt.Rows.Count > 0)
            {
                bool blnvalue = dgViewSub2.PageCount > 1 ? true : false;
                this.btnLastSub2.Enabled = blnvalue;
                this.btnFirstSub2.Enabled = blnvalue;
                this.btnToPageSub2.Enabled = blnvalue;


                if (this.dgViewSub2.PageIndex >= 1)
                    this.btnPreSub2.Enabled = true;
                else
                    this.btnPreSub2.Enabled = false;

                if ((this.dgViewSub2.PageIndex + 1) < dgViewSub2.PageCount)
                    this.btnNextSub2.Enabled = true;
                else
                    this.btnNextSub2.Enabled = false;

                lblCurrentPageSub2.Visible = true;
                lblCurrentPageSub2.Text = "共 [" + dt.Rows.Count.ToString() + "] 条记录  页次 " + (dgViewSub2.PageIndex + 1) + " / " + dgViewSub2.PageCount.ToString();
            }
            else
            {
                this.btnFirstSub2.Enabled = false;
                this.btnPreSub2.Enabled = false;
                this.btnNextSub2.Enabled = false;
                this.btnLastSub2.Enabled = false;
                this.btnToPageSub2.Enabled = false;
                lblCurrentPageSub2.Visible = false;
            }
        }

        protected void btnFirstSub2_Click(object sender, EventArgs e)
        {
            this.HdfActiveTab.Value = "1";
            MovePage(this.dgViewSub2, 0);
        }

        protected void btnPreSub2_Click(object sender, EventArgs e)
        {
            this.HdfActiveTab.Value = "1";
            MovePage(this.dgViewSub2, this.dgViewSub2.PageIndex - 1);
        }

        protected void btnNextSub2_Click(object sender, EventArgs e)
        {
            this.HdfActiveTab.Value = "1";
            MovePage(this.dgViewSub2, this.dgViewSub2.PageIndex + 1);
        }

        protected void btnLastSub2_Click(object sender, EventArgs e)
        {
            this.HdfActiveTab.Value = "1";
            MovePage(this.dgViewSub2, this.dgViewSub2.PageCount - 1);
        }

        protected void btnToPageSub2_Click(object sender, EventArgs e)
        {
            this.HdfActiveTab.Value = "1";
            MovePage(this.dgViewSub2, int.Parse(this.txtPageNoSub2.Text) - 1);
        }
        #endregion

        //明细
        protected void btnInsertSubQuery_Click(object sender, EventArgs e)
        {
            this.HdfActiveTab.Value = "0";
            DataTable dt = (DataTable)Session[FormID + "_Edit_dgViewSub1"];
            DataTable dt1 = Util.JsonHelper.Json2Dtb(hdnMulSelect.Value);

            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                DataRow[] drs = dt.Select(string.Format("ProductID='{0}' and ColorID='{1}'", dt1.Rows[i]["ProductID"].ToString(), dt1.Rows[i]["ColorID"].ToString()));
                if (drs.Length == 0)
                {
                    DataRow dr = dt.NewRow();
                    dr["RowID"] = dt.Rows.Count + 1;
                    dr["Flag"] = Flag;
                    dr["BillID"] = this.txtID.Text.Trim();
                    dr["ProductModel"] = dt1.Rows[i]["ProductModel"];
                    dr["ProductName"] = dt1.Rows[i]["ProductName"];
                    dr["ColorName"] = dt1.Rows[i]["ColorName"];
                    dr["Qty"] = dt1.Rows[i]["Qty"];
                    dr["ColorID"] = dt1.Rows[i]["ColorID"];
                    dr["ProductID"] = dt1.Rows[i]["ProductID"];
                    dt.Rows.Add(dr);
                }
            }
            //UpdateTempSub(this.dgViewSub1);
            this.dgViewSub1.DataSource = dt;
            this.dgViewSub1.DataBind();
            SetPageSubBtnEnabled();

            UpdateTempSub(dgViewSub2);
           
        }

        protected void btnDeleteSubQuery_Click(object sender, EventArgs e)
        {
            this.HdfActiveTab.Value = "0";
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
            if (dt.Rows.Count > 0)
            {
                this.dgViewSub1.DataSource = dt;
                this.dgViewSub1.DataBind();
            }
            else
            {
                BindDataSubNotDetail(this.dgViewSub1, dt);
                DataTable dt2 = (DataTable)Session[FormID + "_Edit_" + dgViewSub2.ID];
            }

            SetPageSubBtnEnabled();
            UpdateTempSub(this.dgViewSub2);

        }

        //序号明细
        protected void btnInsertDetailQuery_Click(object sender, EventArgs e)
        {
            this.HdfActiveTab.Value = "1";
            DataTable dt = (DataTable)Session[FormID + "_Edit_dgViewSub2"];
            DataTable dt1 = Util.JsonHelper.Json2Dtb(hdnMulSelect.Value);

            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                DataRow[] drs = dt.Select(string.Format("BarCode='{0}'", dt1.Rows[i]["BarCode"].ToString()));
                if (drs.Length == 0)
                {
                    DataRow dr = dt.NewRow();
                    dr["RowID"] = dt.Rows.Count + 1;
                    dr["Flag"] = Flag;
                    dr["BillID"] = this.txtID.Text.Trim();
                    dr["BarCode"] = dt1.Rows[i]["BarCode"];
                    dr["ProductID"] = dt1.Rows[i]["ProductID"];
                    dr["ProdName"] = dt1.Rows[i]["ProdName"];
                    dr["ColorID"] = dt1.Rows[i]["ColorID"];
                    dr["ColName"] = dt1.Rows[i]["ColName"];
                    dt.Rows.Add(dr);
                }
            }
            //UpdateTempSub(this.dgViewSub1);
            this.dgViewSub2.DataSource = dt;
            this.dgViewSub2.DataBind();
            SetPageSub2BtnEnabled();

            UpdateTempSub(dgViewSub1);
           

        }

        protected void btnDeleteDetailQuery_Click(object sender, EventArgs e)
        {
            this.HdfActiveTab.Value = "1";

            UpdateTempSub(this.dgViewSub2);
            DataTable dt = (DataTable)Session[FormID + "_Edit_" + dgViewSub2.ID];
            for (int i = 0; i < this.dgViewSub2.Rows.Count; i++)
            {
                CheckBox cb = (CheckBox)(this.dgViewSub2.Rows[i].FindControl("cbSelect"));
                if (cb != null && cb.Checked)
                {
                    Label hk = (Label)(this.dgViewSub2.Rows[i].Cells[1].FindControl("RowID"));
                    DataRow[] drs = dt.Select(string.Format("RowID ={0}", hk.Text));
                    for (int j = 0; j < drs.Length; j++)
                    {

                        dt.Rows.Remove(drs[j]);
                    }

                }
            }
            if (dt.Rows.Count > 0)
            {
                this.dgViewSub2.DataSource = dt;
                this.dgViewSub2.DataBind();
            }
            else
            {
                BindDataSubNotDetail(this.dgViewSub2, dt);
                DataTable dt2 = (DataTable)Session[FormID + "_Edit_" + dgViewSub1.ID];
                
            }

            SetPageSub2BtnEnabled();
            UpdateTempSub(dgViewSub1);
        }

        protected void dgViewSub2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = e.Row.DataItem as DataRowView;
                ((Label)e.Row.Cells[1].Controls[1]).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("RowID")].ToString();
            }
        }

        protected void dgViewSub1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = e.Row.DataItem as DataRowView;
                //txt 
                ((Label)e.Row.FindControl("RowID")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("RowID")].ToString();
                ((TextBox)e.Row.FindControl("Qty")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("Qty")].ToString();
                //((Label)e.Row.Cells[1].Controls[1]).Text = "" + ((e.Row.RowIndex + 1) + dgViewSub1.PageSize * dgViewSub1.PageIndex);
                //((TextBox)e.Row.Cells[2].Controls[1]).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("PRODUCT_CODE")].ToString();
                //((TextBox)e.Row.Cells[3].Controls[1]).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("PRODUCT_NAME")].ToString();
            }
        }
    }
}