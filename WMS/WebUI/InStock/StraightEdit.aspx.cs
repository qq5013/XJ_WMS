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
    public partial class StraightEdit : WMS.App_Code.BasePage
    {
        protected string FormID;
        protected string ID;
        protected string TableName = "WMS_SCHEDULE";
        protected string PrimaryKey = "ScheduleNo";
        protected string SubKey = "Flag,ScheduleNo";
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
                    DataTable dt = bll.FillDataTable("WMS.SelectStraight", new DataParameter[] { new DataParameter("{0}", string.Format("{0}='{1}' and Flag={2}", PrimaryKey, ID, Flag)) });
                    BindData(dt);
                    SetTextReadOnly(this.txtID);
                    this.txtBillDate.ReadOnly = true;
                }
                else
                {
                    txtBillDate.changed = "$('#txtID').val(autoCode('DOS','Flag=2','txtBillDate'));";
                    this.txtBillDate.DateValue = DateTime.Now;
                    this.txtID.Text = bll.GetAutoCode("DOS", DateTime.Now, "Flag=2");
                    this.txtCreator.Text = Session["EmployeeCode"].ToString();
                    this.txtCreateDate.Text = ToYMD(DateTime.Now);
                     
                    BindDataSub();
                }
                BindPageSize();
                writeJsvar(FormID, TableName, PrimaryKey, ID);
                SetTextReadOnly(this.txtCreateDate, this.txtCreator,txtSourceNo,txtScheduleNo,txtCustomerName,this.txtLinkPhone,txtLinkPerson,txtLinkAddress);

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
                this.txtID.Text = dt.Rows[0]["ScheduleNo"].ToString();
                this.txtBillDate.DateValue = dt.Rows[0]["BillDate"];

                this.txtScheduleNo.Text = dt.Rows[0]["DeliverScheduleNo"].ToString();
                this.txtSourceNo.Text = dt.Rows[0]["SourceNo"].ToString();
            
                this.txtLinkPerson.Text = dt.Rows[0]["LinkPerson"].ToString();
                this.txtLinkPhone.Text = dt.Rows[0]["LinkPhone"].ToString();
                this.txtLinkAddress.Text = dt.Rows[0]["LinkAddress"].ToString();

                this.txtMemo.Text = dt.Rows[0]["Memo"].ToString();
                this.txtCreator.Text = dt.Rows[0]["Creator"].ToString();
                this.txtCreateDate.Text = ToYMD(dt.Rows[0]["CreateDate"]);
               
                BindDataSub();    
            }
        }

        private void BindDataSub()
        {
            DataTable dt = bll.FillDataTable("WMS.SelectStraightSub", new DataParameter[] { new DataParameter("{0}", string.Format("ScheduleNo='{0}' and Flag={1}", this.txtID.Text, Flag)) });
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
                ((Label)e.Row.FindControl("RowID")).Text = "" + ((e.Row.RowIndex + 1) + dgViewSub1.PageSize * dgViewSub1.PageIndex);
                ((TextBox)e.Row.FindControl("PlanQty")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("PlanQty")].ToString();
                ((TextBox)e.Row.FindControl("StarNo")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("StarNo")].ToString();
                ((TextBox)e.Row.FindControl("EndNo")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("EndNo")].ToString();
 
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
                dr["PlanQty"] = ((TextBox)dgv.Rows[i].FindControl("PlanQty")).Text;
                dr["StarNo"] = ((TextBox)dgv.Rows[i].FindControl("StarNo")).Text;
                dr["EndNo"] = ((TextBox)dgv.Rows[i].FindControl("EndNo")).Text;
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
                int Count = bll.GetRowCount(TableName, string.Format("ScheduleNo='{0}' and Flag={1}", this.txtID.Text.Trim(), Flag));
                if (Count > 0)
                {
                    WMS.App_Code.JScript.Instance.ShowMessage(this.UpdatePanel1, "该单号已经存在！");
                    return;
                }

                
                para = new DataParameter[] { 
                                             new DataParameter("@Flag", Flag),  
                                             new DataParameter("@ScheduleNo", this.txtID.Text.Trim()),
                                             new DataParameter("@BillDate", this.txtBillDate.DateValue),
                                             new DataParameter("@SourceNo", this.txtScheduleNo.Text.Trim()),
                                             new DataParameter("@Memo", this.txtMemo.Text.Trim()),
                                             new DataParameter("@Creator", this.txtCreator.Text.Trim()),
                                             new DataParameter("@Updater", Session["EmployeeCode"].ToString())
                                             
                                              };
                Commands[0] = "WMS.InsertStraight";

            }
            else //修改
            {
                  para = new DataParameter[] { 
                                             new DataParameter("@Flag", Flag),  
                                             new DataParameter("@ScheduleNo", this.txtID.Text.Trim()),
                                             new DataParameter("@BillDate", this.txtBillDate.DateValue),
                                             new DataParameter("@SourceNo", this.txtScheduleNo.Text.Trim()),
                                             new DataParameter("@Memo", this.txtMemo.Text.Trim()),
                                             new DataParameter("@Creator", this.txtCreator.Text.Trim()),
                                             new DataParameter("@Updater", Session["EmployeeCode"].ToString()),
                                             new DataParameter("{0}",string.Format("Flag={0} and ScheduleNo='{1}'",Flag, this.txtID.Text.Trim())) };
                  Commands[0] = "WMS.UpdateStraight";
            }

            DataTable dt = (DataTable)Session[FormID + "_Edit_dgViewSub1"];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i].BeginEdit();
                dt.Rows[i]["ScheduleNo"] = this.txtID.Text.Trim();
                dt.Rows[i].EndEdit();
            }

            DataTable dtProduct = dt.DefaultView.ToTable("Product", true, new string[] { "ProductID", "ColorID", "ProductName", "ProductModel", "ColorName" });
            for (int i = 0; i < dtProduct.Rows.Count; i++)
            {
                object o = dt.Compute("SUM(PlanQty)", string.Format("ProductID='{0}' and ColorID='{1}'", dtProduct.Rows[i]["ProductID"], dtProduct.Rows[i]["ColorID"]));
                if (o != null)
                {
                    int Qty = int.Parse(o.ToString());

                    int NotOutQty = int.Parse(bll.GetFieldValue("WMS_StockSub", "InStockQty", string.Format("Flag=2 and BillID='{0}' and ProductID='{1}' and ColorID='{2}'", this.txtScheduleNo.Text, dt.Rows[i]["ProductID"], dt.Rows[i]["ColorID"])));
                     

                    if (Qty != NotOutQty)
                    {
                        WMS.App_Code.JScript.Instance.ShowMessage(this.UpdatePanel1, dtProduct.Rows[i]["ProductName"].ToString() + " (" + dtProduct.Rows[i]["ProductModel"].ToString() + ")" + " " + dtProduct.Rows[i]["ColorName"].ToString() + " 出库数量不等于发货通知单数量 " + NotOutQty.ToString());
                        return;
                    }
                    
                    
                    DataTable dtProductQty = bll.FillDataTable("WMS.SelectNoStraightOutSubByProduct", new DataParameter[] { new DataParameter("@BillID", this.txtID.Text), new DataParameter("@SourceNo", this.txtScheduleNo.Text), new DataParameter("@ProductID", dtProduct.Rows[i]["ProductID"].ToString()), new DataParameter("@ColorID", dtProduct.Rows[i]["ColorID"].ToString()) });
                    bool blnvalue = false;
                    if (dtProductQty.Rows.Count == 0)
                    {
                        blnvalue = true;
                    }
                    else
                    {
                        if (Qty > int.Parse(dtProductQty.Rows[0]["NotOutStockQty"].ToString()))
                            blnvalue = true;
                    }
                    if (blnvalue)
                    {
                        WMS.App_Code.JScript.Instance.ShowMessage(this.UpdatePanel1, dtProduct.Rows[i]["ProductName"].ToString() + " (" + dtProduct.Rows[i]["ProductModel"].ToString() + ")" + " " + dtProduct.Rows[i]["ColorName"].ToString() + " 大于发货通知单的数量！");
                        return;

                    }
                }
            }

            Commands[1] = "WMS.DeleteStraightSub";
            Commands[2] = "WMS.InsertStraightSub";
            bll.ExecTran(Commands, para, SubKey, new DataTable[] { dt });



            Response.Redirect(FormID + "View.aspx?SubModuleCode=" + SubModuleCode + "&FormID=" + Server.UrlEncode(FormID) + "&ID=" + Server.UrlEncode(this.txtID.Text));
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {

        }

        #region 从表资料绑定

        private int GetSumQty(DataTable dt,string ProductID,string ColorID)
        {
            int i = 0;
            object obj = dt.Compute("Sum(PlanQty)", string.Format("ProductID='{0}' and ColorID='{1}'", ProductID, ColorID));
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
            DataTable dtSchedule = bll.FillDataTable("WMS.SelectNoStraightOut", new DataParameter[] { new DataParameter("{0}", string.Format("BillID='{0}' and Flag=2", this.txtScheduleNo.Text)) });
            if (dtSchedule.Rows.Count > 0)
            {
                this.txtSourceNo.Text = dtSchedule.Rows[0]["SourceNo"].ToString();
                this.txtCustomerName.Text = dtSchedule.Rows[0]["CustName"].ToString();
                this.txtLinkPerson.Text = dtSchedule.Rows[0]["LinkPerson"].ToString();
                this.txtLinkPhone.Text = dtSchedule.Rows[0]["LinkPhone"].ToString();
                this.txtLinkAddress.Text = dtSchedule.Rows[0]["LinkAddress"].ToString();
            }

            DataTable dtScheduleSub = bll.FillDataTable("WMS.SelectNoStraightOutSub", new DataParameter[] { new DataParameter("{0}", string.Format("BillID='{0}' and Flag=2 ", this.txtScheduleNo.Text)) });
            if (dtScheduleSub.Rows.Count > 0)
            {
                

                for (int i = 0; i < dtScheduleSub.Rows.Count; i++)
                {

                    DataRow[] drs = dt.Select(string.Format("ProductID='{0}' and ColorID='{1}' and PlanQty={2}", dtScheduleSub.Rows[i]["ProductID"], dtScheduleSub.Rows[i]["ColorID"], dtScheduleSub.Rows[i]["NotOutStockQty"]));
                    if (drs.Length > 0)
                        continue;
                    if ((int)dtScheduleSub.Rows[i]["NotOutStockQty"] - GetSumQty(dt, dtScheduleSub.Rows[i]["ProductID"].ToString(), dtScheduleSub.Rows[i]["ColorID"].ToString()) > 0)
                    {
                        DataRow dr = dt.NewRow();

                        dr["RowID"] = dt.Rows.Count + 1;
                        dr["Flag"] = Flag;
                        dr["ScheduleNo"] = this.txtID.Text.Trim();
                        dr["ProductID"] = dtScheduleSub.Rows[i]["ProductID"];
                        dr["ProductName"] = dtScheduleSub.Rows[i]["ProductName"];
                        dr["ProductModel"] = dtScheduleSub.Rows[i]["ProductModel"];
                        dr["PlanQty"] = (int)dtScheduleSub.Rows[i]["NotOutStockQty"] - GetSumQty(dt, dtScheduleSub.Rows[i]["ProductID"].ToString(), dtScheduleSub.Rows[i]["ColorID"].ToString());
                        dr["ColorID"] = dtScheduleSub.Rows[i]["ColorID"];
                        dr["ColorName"] = dtScheduleSub.Rows[i]["ColorName"];

                        dt.Rows.Add(dr);
                    }
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
        }

       
    }
}