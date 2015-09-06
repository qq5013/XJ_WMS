using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using IDAL;

namespace WMS.WebUI.OutStock
{
    public partial class DeliverEdit : WMS.App_Code.BasePage
    {
        protected string FormID;
        protected string ID;
        protected string TableName = "WMS_Stock";
        protected string PrimaryKey = "BillID";
        protected string SubKey = "Flag,BillID";
        protected int Flag = 2;
       
        BLL.BLLBase bll = new BLL.BLLBase();

        protected void Page_Load(object sender, EventArgs e)
        {
            ID = Request.QueryString["ID"] + "";
            FormID = Request.QueryString["FormID"] + "";
            if (!IsPostBack)
            {
                if (ID != "")
                {
                    DataTable dt = bll.FillDataTable("WMS.SelectOutStock", new DataParameter[] { new DataParameter("{0}", string.Format("{0}='{1}' and Flag={2}", PrimaryKey, ID, Flag)) });
                    BindData(dt);
                    SetTextReadOnly(this.txtID);
                    this.txtBillDate.ReadOnly = true;
                }
                else
                {
                    txtBillDate.changed = "$('#txtID').val(autoCode('OS','Flag=2','txtBillDate'));";
                    this.txtBillDate.DateValue = DateTime.Now;
                    this.txtID.Text = bll.GetAutoCode("OS", DateTime.Now, "Flag=2");
                    this.txtCreator.Text = Session["EmployeeCode"].ToString();
                     
                    this.txtPlanDate.DateValue = DateTime.Now;
                    BindDataSub();
                }
                BindPageSize();
                writeJsvar(FormID,SqlCmd, ID);
                SetTextReadOnly( this.txtCreator,txtScheduleNo,txtCustomerName,this.txtLinkPhone,txtLinkPerson,txtLinkAddress);

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
                this.txtPlanDate.DateValue = dt.Rows[0]["PlanDate"];
                this.txtScheduleNo.Text = dt.Rows[0]["ScheduleNo"].ToString();
                this.txtLinkPerson.Text = dt.Rows[0]["LinkPerson"].ToString();
                this.txtLinkPhone.Text = dt.Rows[0]["LinkPhone"].ToString();
                this.txtLinkAddress.Text = dt.Rows[0]["LinkAddress"].ToString();
                this.ddlDriverType.SelectedValue = dt.Rows[0]["DriverType"].ToString();
                this.txtMemo.Text = dt.Rows[0]["Memo"].ToString();
                this.txtCreator.Text = dt.Rows[0]["Creator"].ToString();
                BindDataSub();    
            }
        }

        private void BindDataSub()
        {
            DataTable dt = bll.FillDataTable("WMS.SelectOutStockSub", new DataParameter[] { new DataParameter("{0}", string.Format("BillID='{0}' and Flag={1}", this.txtID.Text, Flag)),new DataParameter("@BillID",this.txtID.Text) });
            if (!dt.Columns.Contains("PACK_QTY"))
            {
                DataColumn dc1 = new DataColumn("PACK_QTY", typeof(int));
                DataColumn dc2 = new DataColumn("SubCount", typeof(int));
                DataColumn dc3 = new DataColumn("ProductVolume", typeof(float));
                dt.Columns.Add(dc1);
                dt.Columns.Add(dc2);
                dt.Columns.Add(dc3);

            }

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
         

        protected void dgViewSub1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.RowIndex % 2 == 1)
                    e.Row.CssClass = " bottomtable";


                DataRowView drv = e.Row.DataItem as DataRowView;
                //txt 
                ((Label)e.Row.Cells[1].FindControl("RowID")).Text = "" + ((e.Row.RowIndex + 1) + dgViewSub1.PageSize * dgViewSub1.PageIndex);
                ((TextBox)e.Row.Cells[5].FindControl("InStockQty")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("InStockQty")].ToString();
 
            }
        }

        private void UpdateTempSub(GridView dgv)
        {
            DataTable dt1 = (DataTable)Session[FormID + "_Edit_" + dgv.ID];
            DataRow dr;
            if (dt1.Rows.Count == 0)
                return;
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                dr = dt1.Rows[i + dgv.PageSize * dgv.PageIndex];
                dr.BeginEdit();
                dr["RowID"] = ((Label)dgv.Rows[i].Cells[5].FindControl("RowID")).Text;
                dr["InStockQty"] = ((TextBox)dgv.Rows[i].Cells[5].FindControl("InStockQty")).Text;
                dr.EndEdit();
            }
            dt1.AcceptChanges();
            Session[FormID + "_Edit_" + dgv.ID] = dt1;
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
            if (dt.Rows.Count > 0)
            {
                this.dgViewSub1.DataSource = dt;
                this.dgViewSub1.DataBind();
            }
            else
            {
                BindDataSubNotDetail(this.dgViewSub1, dt);

                this.txtScheduleNo.Text = "";
            }
           
            SetPageSubBtnEnabled();

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            UpdateTempSub(this.dgViewSub1);
            string[] Commands = new string[3];
            DataParameter[] para;

            if (ID == "") //新增   
            {
                int Count = bll.GetRowCount(TableName, string.Format("BillID='{0}' and Flag={1}", this.txtID.Text.Trim(), Flag));
                if (Count > 0)
                {
                    WMS.App_Code.JScript.Instance.ShowMessage(this.UpdatePanel1, "该出库单号已经存在！");
                    return;
                }

                
                para = new DataParameter[] { 
                                             new DataParameter("@Flag", Flag),  
                                             new DataParameter("@BillID", this.txtID.Text.Trim()),
                                             new DataParameter("@BillDate", this.txtBillDate.DateValue),
                                             new DataParameter("@ScheduleNo", this.txtScheduleNo.Text.Trim()),
                                             new DataParameter("@Memo", this.txtMemo.Text.Trim()),
                                             new DataParameter("@Creator", this.txtCreator.Text.Trim()),
                                             new DataParameter("@Updater", Session["EmployeeCode"].ToString()),
                                             new DataParameter("@PlanDate",this.txtPlanDate.DateValue),
                                             new DataParameter("@DriverType",this.ddlDriverType.SelectedValue)
                                             
                                              };
                Commands[0] = "WMS.InsertOutStock";

            }
            else //修改
            {
                  para = new DataParameter[] { 
                                             new DataParameter("@Flag", Flag),  
                                             new DataParameter("@BillID", this.txtID.Text.Trim()),
                                             new DataParameter("@BillDate", this.txtBillDate.DateValue),
                                             new DataParameter("@ScheduleNo", this.txtScheduleNo.Text.Trim()),
                                             new DataParameter("@Memo", this.txtMemo.Text.Trim()),
                                             new DataParameter("@Creator", this.txtCreator.Text.Trim()),
                                             new DataParameter("@Updater", Session["EmployeeCode"].ToString()),
                                             new DataParameter("@PlanDate",this.txtPlanDate.DateValue),
                                             new DataParameter("@DriverType",this.ddlDriverType.SelectedValue),
                                             new DataParameter("{0}",string.Format("Flag={0} and BillID='{1}'",Flag, this.txtID.Text.Trim())) };
                  Commands[0] = "WMS.UpdateOutStock";
            }

            DataTable dt = (DataTable)Session[FormID + "_Edit_dgViewSub1"];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataTable dtProduct = bll.FillDataTable("WMS.SelectNoDeliverScheduleSubByProduct", new DataParameter[] { new DataParameter("@BillID", this.txtID.Text),
                                                                                                                         new DataParameter("@ScheduleNo",this.txtScheduleNo.Text),
                                                                                                                         new DataParameter("@ProductID", dt.Rows[i]["ProductID"].ToString()), 
                                                                                                                         new DataParameter("@ColorID", dt.Rows[i]["ColorID"].ToString()) });


                int NotOutQty = int.Parse(dtProduct.Rows[0]["NotOutStockQty"].ToString());

                if (int.Parse(dt.Rows[i]["InStockQty"].ToString()) > NotOutQty)
                {
                    WMS.App_Code.JScript.Instance.ShowMessage(this.UpdatePanel1, "序号:" + dt.Rows[i]["RowID"].ToString() + " 出库数量大于销售单未出库数量 " + NotOutQty.ToString());
                    return;
                }

                dt.Rows[i].BeginEdit();
                dt.Rows[i]["BillID"] = this.txtID.Text.Trim();

                int Qty = (int)dt.Rows[i]["InStockQty"];
                int PackQty = (int)dt.Rows[i]["PACK_QTY"];
                int PQty = 0;
                if (int.Parse(dt.Rows[i]["SubCount"].ToString()) > 1)
                {

                    dt.Rows[i]["Volume"] = Qty * double.Parse(dt.Rows[i]["ProductVolume"].ToString());
                }
                else
                {
                    if (Qty % PackQty != 0)
                        PQty = Qty / PackQty + 1;
                    else
                        PQty = Qty / PackQty;


                    dt.Rows[i]["Volume"] = PQty * double.Parse(dt.Rows[i]["ProductVolume"].ToString());
                }
                dt.Rows[i].EndEdit();

            }
            if (ddlDriverType.SelectedValue == "0")
            {
                DataTable dtProduct = dt.DefaultView.ToTable("Product", true, new string[] { "ProductID", "ColorID", "ProductName", "ProductModel", "ColorName" });

                for (int i = 0; i < dtProduct.Rows.Count; i++)
                {
                    object o = dt.Compute("Sum(InStockQty)", string.Format("ProductID='{0}' and ColorID='{1}'", dtProduct.Rows[i]["ProductID"], dtProduct.Rows[i]["ColorID"]));
                    if (o != null)
                    {
                        int Qty = int.Parse(o.ToString());

                        DataTable dtProductQty =  bll.FillDataTable("WMS.SelectProductStockQty", new DataParameter[] { new DataParameter("@BillID", this.txtID.Text), new DataParameter("@ProductID", dtProduct.Rows[i]["ProductID"].ToString()), new DataParameter("@ColorID", dtProduct.Rows[i]["ColorID"].ToString()) });
                        bool blnvalue = false;
                        if (dtProductQty.Rows.Count == 0)
                        {
                            blnvalue = true;
                        }
                        else
                        {
                            if (Qty > int.Parse(dtProductQty.Rows[0]["StockQty"].ToString()))
                                blnvalue = true;
                        }
                        if (blnvalue)
                        {

                            WMS.App_Code.JScript.Instance.ShowMessage(this.UpdatePanel1, dtProduct.Rows[i]["ProductName"].ToString() + " (" + dtProduct.Rows[i]["ProductModel"].ToString() + ")" + " " + dtProduct.Rows[i]["ColorName"].ToString() + " 库存不足，请修改出库数量。");
                            return;

                        }
                    }

                }
            }



            Commands[1] = "WMS.DeleteOutStockSub";
            Commands[2] = "WMS.InsertOutStockSub";
            bll.ExecTran(Commands, para, SubKey, new DataTable[] { dt });



            Response.Redirect(FormID + "View.aspx?SubModuleCode=" + SubModuleCode + "&FormID=" + Server.UrlEncode(FormID) + "&ID=" + Server.UrlEncode(this.txtID.Text));
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {

        }



        #region 从表资料绑定

        private int GetSumQty(DataTable dt)
        {
            int i = 0;
            object obj = dt.Compute("Sum(InStockQty)", "");
            if (obj != null && obj.ToString() != "")
            {
                i = int.Parse(obj.ToString());
            }

            
            return i;

        }
        protected void ddlPageSizeSub1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTempSub(this.dgViewSub1);
            DataTable dt1 = (DataTable)Session[FormID + "_Edit_dgViewSub1"];
            this.dgViewSub1.DataSource = dt1;
            this.dgViewSub1.PageSize = int.Parse(ddlPageSizeSub1.Text);
            this.dgViewSub1.DataBind();
            SetPageSubBtnEnabled();
        }
        private void MovePage(GridView dgv, int pageindex)
        {
            UpdateTempSub(dgv);
            int pindex = pageindex;
            if (pindex < 0) pindex = 0;
            if (pindex >= dgv.PageCount) pindex = dgv.PageCount - 1;
            DataTable dt1 = (DataTable)Session[FormID + "_Edit_" + dgv.ID];
            dgv.PageIndex = pindex;
            dgv.DataSource = dt1;
            dgv.DataBind();

            SetPageSubBtnEnabled();
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "Resize", "content_resize();", true);

        }

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
            this.ddlPageSizeSub1.Items.Add(new ListItem("10", "10"));
            this.ddlPageSizeSub1.Items.Add(new ListItem("20", "20"));
            this.ddlPageSizeSub1.Items.Add(new ListItem("30", "30"));
            this.ddlPageSizeSub1.Items.Add(new ListItem("40", "40"));
            this.ddlPageSizeSub1.Items.Add(new ListItem("50", "50"));
            this.ddlPageSizeSub1.Items.Add(new ListItem("100", "100"));
        }

        protected void btnFirstSub1_Click(object sender, EventArgs e)
        {
            MovePage(this.dgViewSub1, 0);
        }

        protected void btnPreSub1_Click(object sender, EventArgs e)
        {
            MovePage(this.dgViewSub1, this.dgViewSub1.PageIndex - 1); 
        }

        protected void btnNextSub1_Click(object sender, EventArgs e)
        {
            MovePage(this.dgViewSub1, this.dgViewSub1.PageIndex + 1); 
        }

        protected void btnLastSub1_Click(object sender, EventArgs e)
        {
            MovePage(this.dgViewSub1, this.dgViewSub1.PageCount - 1);  
        }

        protected void btnToPageSub1_Click(object sender, EventArgs e)
        {
            MovePage(this.dgViewSub1, int.Parse(this.txtPageNoSub1.Text) - 1);  
        }

       
        #endregion  

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            //绑定明细;
            UpdateTempSub(this.dgViewSub1);
            DataTable dt = (DataTable)Session[FormID + "_Edit_dgViewSub1"];
            DataTable dtSchedule = bll.FillDataTable("WMS.SelectDeliverSchedule", new DataParameter[] { new DataParameter("{0}", string.Format("ScheduleNo='{0}' and Flag=1", this.txtScheduleNo.Text)) });
            if (dtSchedule.Rows.Count > 0)
            {
               
                this.txtCustomerName.Text = dtSchedule.Rows[0]["CustName"].ToString();
                this.txtLinkPerson.Text = dtSchedule.Rows[0]["LinkPerson"].ToString();
                this.txtLinkPhone.Text = dtSchedule.Rows[0]["LinkPhone"].ToString();
                this.txtLinkAddress.Text = dtSchedule.Rows[0]["LinkAddress"].ToString();
                this.ddlTransport.SelectedValue = dtSchedule.Rows[0]["Transport"].ToString();
                this.txtCustPerson.Text = dtSchedule.Rows[0]["CustPerson"].ToString();
                this.txtMemo.Text = dtSchedule.Rows[0]["Memo"].ToString();
            }
            string strSubID = "-1";

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strSubID += "," + dt.Rows[i]["IsCode"].ToString();
                }

            }
            DataTable dtScheduleSub = bll.FillDataTable("WMS.SelectNoDeliverScheduleSub", new DataParameter[] { new DataParameter("@BillID", this.txtID.Text), new DataParameter("{0}", string.Format("ScheduleNo='{0}' and Flag=1 and SubID not in ({1})", this.txtScheduleNo.Text, strSubID)) });
            if (dtScheduleSub.Rows.Count > 0)
            {

                for (int i = 0; i < dtScheduleSub.Rows.Count; i++)
                {
                    DataRow dr = dt.NewRow();

                    dr["RowID"] = dt.Rows.Count + 1;
                    dr["Flag"] = Flag;
                    dr["BillID"] = this.txtID.Text.Trim();
                    dr["ProductID"] = dtScheduleSub.Rows[i]["ProductID"];
                    dr["ProductName"] = dtScheduleSub.Rows[i]["ProductName"];
                    dr["ProductModel"] = dtScheduleSub.Rows[i]["ProductModel"];
                    dr["InStockQty"] = dtScheduleSub.Rows[i]["NotOutStockQty"];
                    dr["ColorID"] = dtScheduleSub.Rows[i]["ColorID"];
                    dr["ColorName"] = dtScheduleSub.Rows[i]["ColorName"];
                    dr["IsCode"] = dtScheduleSub.Rows[i]["SubID"];
                    dr["ScanQty"] =0;
                    dr["PACK_QTY"] = dtScheduleSub.Rows[i]["PACK_QTY"];
                    dr["SubCount"] = dtScheduleSub.Rows[i]["SubCount"];
                    dr["ProductVolume"] = dtScheduleSub.Rows[i]["ProductVolume"];
                    dr["StockQty"] = dtScheduleSub.Rows[i]["StockQty"];

                    int Qty = (int)dr["InStockQty"];
                    int PackQty = (int)dr["PACK_QTY"];
                    int PQty = 0;
                    if (int.Parse(dr["SubCount"].ToString()) > 1)
                    {

                        dr["Volume"] = Qty * double.Parse(dr["ProductVolume"].ToString());
                    }
                    else
                    {
                        if (Qty % PackQty != 0)
                            PQty = Qty / PackQty + 1;
                        else
                            PQty = Qty / PackQty;


                        dr["Volume"] = PQty * double.Parse(dr["ProductVolume"].ToString());
                    }
                    dt.Rows.Add(dr);
                }
                dt.AcceptChanges();
                Session[FormID + "_Edit_dgViewSub1"] = dt;
                //UpdateTempSub(this.dgViewSub1);
                this.dgViewSub1.DataSource = dt;
                this.dgViewSub1.DataBind();
                SetPageSubBtnEnabled();
            }
            if (dt.Rows.Count == 0)
            {
                BindDataSubNotDetail(this.dgViewSub1, dt);
            }
            this.txtTotalQty.Text = "" + GetSumQty(dt);
            


        }

       
    }
}