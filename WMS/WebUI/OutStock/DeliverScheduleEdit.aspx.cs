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
    public partial class DeliverScheduleEdit : WMS.App_Code.BasePage
    {
        protected string FormID;
        protected string ID;
        protected string TableName = "WMS_DeliverSCHEDULE";
        protected string PrimaryKey = "ScheduleNo";
        protected string SubKey = "Flag,ScheduleNo";
        protected int Flag = 1;
        protected string strCopy = "0";
       
        BLL.BLLBase bll = new BLL.BLLBase();

        protected void Page_Load(object sender, EventArgs e)
        {
            ID = Request.QueryString["ID"] + "";
            FormID = Request.QueryString["FormID"] + "";
            strCopy = Request.QueryString["FormCopy"] + "";
            if (!IsPostBack)
            {
                this.ddlAreaSation.Attributes["onchange"] = "ChangeAutoCode();";

                if (ID != "")
                {
                    DataTable dt = bll.FillDataTable("WMS.SelectDeliverSchedule", new DataParameter[] { new DataParameter("{0}", string.Format("{0}='{1}' and Flag={2}", PrimaryKey, ID, Flag)) });
                    BindCustomerInfo(dt.Rows[0]["CustomerID"].ToString());
                    BindData(dt);
                    if (strCopy == "1")
                    {
                        
                        txtBillDate.changed = "var Prefix = makePy($('#ddlAreaSation').val().substr(0, 2));$('#txtID').val(autoCodeByTableName(Prefix[0], 'Flag=1','WMS_DeliverSchedule', 'txtBillDate'));";
                        this.txtBillDate.DateValue = DateTime.Now;
                        this.txtID.Text = bll.GetAutoCodeByTableName(ID.Substring(0,2), "WMS_DeliverSchedule", DateTime.Now, "Flag=1");
                        this.txtCreator.Text = Session["EmployeeCode"].ToString();
                        this.txtCreateDate.Text = ToYMD(DateTime.Now);
                    }
                    else
                    {
                        SetTextReadOnly(this.txtID);
                        this.txtBillDate.ReadOnly = true;
                         
                    }
                }
                else
                {
                    txtBillDate.changed = "var Prefix = makePy($('#ddlAreaSation').val().substr(0, 2));$('#txtID').val(autoCodeByTableName(Prefix[0], 'Flag=1','WMS_DeliverSchedule', 'txtBillDate'));";
                    this.txtBillDate.DateValue = DateTime.Now;
                    this.txtID.Text = bll.GetAutoCodeByTableName("BJ", "WMS_DeliverSchedule", DateTime.Now, "Flag=1");
                    this.txtCreator.Text = Session["EmployeeCode"].ToString();
                    this.txtCreateDate.Text = ToYMD(DateTime.Now);

                    BindDataSub();
                }
                BindPageSize();
                writeJsvar(FormID,SqlCmd, ID);
                SetTextReadOnly(this.txtCreateDate, this.txtCreator,this.txtSourceNo);
                DataTable dtArea=bll.FillDataTable("CMD.SelectAreaSation",null);
                this.ddlAreaSation.DataValueField = "areasation";
                this.ddlAreaSation.DataTextField = "areasation";
                this.ddlAreaSation.DataSource = dtArea;
                this.ddlAreaSation.DataBind();
                
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "", "BindEvent()", true);
            }
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "Resize", "content_resize();", true);
            if (strCopy == "1")
                ID = "";
        }

        
   

        private void BindData(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                this.txtID.Text = dt.Rows[0]["ScheduleNo"].ToString();
                this.txtBillDate.DateValue = dt.Rows[0]["BillDate"];
                this.txtSourceNo.Text = dt.Rows[0]["SourceNo"].ToString();
               
                this.txtCustomerID.Text = dt.Rows[0]["CustomerID"].ToString();
                this.txtCustomerName.Text = dt.Rows[0]["CustName"].ToString();
                
                this.txtParCustomerID.Text = dt.Rows[0]["ParCustomerID"].ToString();
                this.txtParCustomerName.Text = dt.Rows[0]["ParCustName"].ToString();    
                this.txtCustPerson.Text = dt.Rows[0]["CustPerson"].ToString();
                 
                this.txtLinkPerson.Text = dt.Rows[0]["LinkPerson"].ToString();
                this.txtLinkPhone.Text = dt.Rows[0]["LinkPhone"].ToString();
                this.txtLinkAddress.Text = dt.Rows[0]["LinkAddress"].ToString();
                this.ddlTransport.SelectedValue = dt.Rows[0]["Transport"].ToString();
                this.txtMemo.Text = dt.Rows[0]["Memo"].ToString();
                this.txtCreator.Text = dt.Rows[0]["Creator"].ToString();
                this.txtCreateDate.Text = ToYMD(dt.Rows[0]["CreateDate"]);
                this.hdnCustomerType.Value = dt.Rows[0]["CustomerType"].ToString();
                this.ddlAreaSation.SelectedValue = dt.Rows[0]["AreaSation"].ToString();
               
                BindDataSub();    
            }
        }

        private void BindDataSub()
        {
            DataTable dt = bll.FillDataTable("WMS.SelectDeliverScheduleSub", new DataParameter[] { new DataParameter("{0}", string.Format("ScheduleNo='{0}' and Flag={1}", this.txtID.Text, Flag)) });
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

                object o = dt.Compute("SUM(PlanQty)", "");
                object o1 = dt.Compute("SUM(Amount)", "");
                this.txtTotalCount.Text = o1.ToString();
                this.txtTotalQty.Text = o.ToString(); 
            }
            SetPageSubBtnEnabled();
            
        }

        protected void dgViewSub1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {//在数据绑定时，设置各列在宽度较小时，数据自动换行
                e.Row.Cells[i].Style.Add("word-break", "break-all");
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.RowIndex % 2 == 1)
                    e.Row.CssClass = " bottomtable";

                DataRowView drv = e.Row.DataItem as DataRowView;
                //txt 
                if (drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("RealQty")].ToString() != "")
                {

                    if ((int)drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("RealQty")] > 0  && strCopy == "0")
                    {
                        ((CheckBox)e.Row.FindControl("cbSelect")).Enabled = false;
                        ((Button)e.Row.FindControl("btnProduct")).Enabled = false;

                    }
                }
                SetTextReadOnly((TextBox)e.Row.FindControl("ProductModel"),(TextBox)e.Row.FindControl("ProductFModel"),(TextBox)e.Row.FindControl("ProductName"), (TextBox)e.Row.FindControl("ColorName"), (TextBox)e.Row.FindControl("RealQty"));

                ((Label)e.Row.FindControl("RowID")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("RowID")].ToString();
                ((TextBox)e.Row.FindControl("ProductID")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("ProductID")].ToString();
                ((TextBox)e.Row.FindControl("ProductModel")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("ProductModel")].ToString();
                ((TextBox)e.Row.FindControl("ProductFModel")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("ProductFModel")].ToString();
                ((TextBox)e.Row.FindControl("ProductName")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("ProductName")].ToString();
                 
                ((TextBox)e.Row.FindControl("ColorID")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("ColorID")].ToString();
                ((TextBox)e.Row.FindControl("ColorName")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("ColorName")].ToString();
                ((TextBox)e.Row.FindControl("PlanQty")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("PlanQty")].ToString();
                ((TextBox)e.Row.FindControl("Price")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("Price")].ToString();
                ((TextBox)e.Row.FindControl("Amount")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("Amount")].ToString();
                ((TextBox)e.Row.FindControl("Memo")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("Memo")].ToString();
                ((UserControl.Calendar)e.Row.FindControl("PlanDate")).DateValue = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("PlanDate")];
                if (strCopy == "1")
                {
                    ((TextBox)e.Row.FindControl("RealQty")).Text = "0";
                    ((UserControl.Calendar)e.Row.FindControl("PlanDate")).DateValue = DateTime.Now;
                    ((TextBox)e.Row.FindControl("Memo")).Text = "";

                }
                else
                    ((TextBox)e.Row.FindControl("RealQty")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("RealQty")].ToString();

            }
        }

        private void UpdateTempSub(GridView dgv)
        {
            DataTable dt1 = (DataTable)Session[FormID + "_Edit_" + dgv.ID];
            if (dt1.Rows.Count == 0)
                return;
            DataRow dr;
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                dr = dt1.Rows[i + dgv.PageSize * dgv.PageIndex];
                dr.BeginEdit();
                dr["Flag"] = Flag;
                dr["ScheduleNo"] = this.txtID.Text.Trim();
                dr["RowID"] = ((Label)dgv.Rows[i].FindControl("RowID")).Text;
                dr["ProductID"] = ((TextBox)dgv.Rows[i].FindControl("ProductID")).Text ;
                dr["ProductName"] = ((TextBox)dgv.Rows[i].FindControl("ProductName")).Text;
                dr["ProductModel"] = ((TextBox)dgv.Rows[i].FindControl("ProductModel")).Text;
               
                dr["ColorID"] = ((TextBox)dgv.Rows[i].FindControl("ColorID")).Text;
                dr["ColorName"] = ((TextBox)dgv.Rows[i].FindControl("ColorName")).Text;
                dr["PlanQty"] = ((TextBox)dgv.Rows[i].FindControl("PlanQty")).Text ;
                dr["Price"] = ((TextBox)dgv.Rows[i].FindControl("Price")).Text;
                dr["Amount"] = ((TextBox)dgv.Rows[i].FindControl("Amount")).Text;
                
                dr["PlanDate"] = ((UserControl.Calendar)dgv.Rows[i].FindControl("PlanDate")).DateValue;
                dr["RealQty"] = ((TextBox)dgv.Rows[i].FindControl("RealQty")).Text;
                dr["Memo"] = ((TextBox)dgv.Rows[i].FindControl("Memo")).Text;
                dr.EndEdit();
            }
            dt1.AcceptChanges();
            Session[FormID + "_Edit_" + dgv.ID] = dt1;
        }

        protected void btnProduct_Click(object sender, EventArgs e)
        {

            DataTable dt = (DataTable)Session[FormID + "_Edit_dgViewSub1"];
            DataTable dt1 = Util.JsonHelper.Json2Dtb(hdnMulSelect.Value);
            int cur = int.Parse(((Button)sender).ClientID.Split('_')[1].Replace("ctl", "")) - 2 + this.dgViewSub1.PageSize * dgViewSub1.PageIndex;

            if (dt1.Rows.Count > 0)
            {
                DataRow[] drs = dt.Select("RowID>" + (cur + 1));
                for (int j = 0; j < drs.Length; j++)
                {
                    drs[j].BeginEdit();
                    drs[j]["RowID"] = cur + j + 1 + dt1.Rows.Count;
                    drs[j].EndEdit();
                }
            }
            DataView dv = dt.DefaultView;
            dv.Sort = "RowID";
            dt = dv.ToTable();

            DataRow dr;
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                if (i == 0)
                {
                    dr = dt.Rows[cur];
                }
                else
                {
                    dr = dt.NewRow();
                    dt.Rows.InsertAt(dr, cur + i);
                    dr["SubID"] = GetMaxSubID(dt);

                }
                dr["RowID"] = i + cur + 1;
                dr["Flag"] = Flag;
                dr["ScheduleNo"] = this.txtID.Text.Trim();
                dr["ProductID"] = dt1.Rows[i]["PRODUCT_CODE"];
                dr["ProductName"] = dt1.Rows[i]["Name"];
                dr["ProductModel"] = dt1.Rows[i]["ProductModel"];
                dr["ProductFModel"] = dt1.Rows[i]["ProductFModel"];
                dr["ColorID"] = dt1.Rows[i]["ColorID"];
                dr["ColorName"] = dt1.Rows[i]["ColorName"];    
                dr["PlanQty"] = 0;
                dr["Price"] = dt1.Rows[i]["Price" + this.hdnCustomerType.Value];
                dr["Amount"] = 0;
                dr["PlanDate"] = DateTime.Now;
                dr["RealQty"] = 0;
            }
            //UpdateTempSub(this.dgViewSub1);
            this.dgViewSub1.DataSource = dt;
            this.dgViewSub1.DataBind();
            object o = dt.Compute("SUM(PlanQty)", "");
            object o1 = dt.Compute("SUM(Amount)", "");
            this.txtTotalCount.Text = o1.ToString();
            this.txtTotalQty.Text = o.ToString();
            Session[FormID + "_Edit_dgViewSub1"] = dt;
            SetPageSubBtnEnabled();
        }

        protected void btnAddDetail_Click(object sender, EventArgs e)
        {
            if (this.txtCustomerID.Text == "")
            {

                WMS.App_Code.JScript.Instance.ShowMessage(this.UpdatePanel1, "销售客户不能为空，请先选择！");
                DataTable dt1 = (DataTable)Session[FormID + "_Edit_dgViewSub1"];
                if (dt1.Rows.Count == 0)
                {
                    DataTable dtNew = dt1.Clone();
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
                return;
            }



            UpdateTempSub(this.dgViewSub1);
            int pagecount = this.dgViewSub1.PageCount;

            DataTable dt = (DataTable)Session[FormID + "_Edit_dgViewSub1"];
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[dt.Rows.Count - 1]["ProductID"].ToString() == "")
                {
                    return;
                }
            }
            DataRow dr;


            dr = dt.NewRow();
            dr["RowID"] = dt.Rows.Count + 1;
            dr["SubID"] = GetMaxSubID(dt);
            dr["PlanDate"] = DateTime.Now;
            dr["RealQty"] = 0;
            dr["PlanQty"] = 0;
            dr["Price"] = 0;
            dr["Amount"] = 0;

            dt.Rows.Add(dr);
            this.dgViewSub1.DataSource = dt;
            this.dgViewSub1.DataBind();
            if (pagecount != this.dgViewSub1.PageCount)
                MovePage(this.dgViewSub1, this.dgViewSub1.PageCount - 1);

            SetPageSubBtnEnabled();

        }

        protected void btnDelDetail_Click(object sender, EventArgs e)
        {
            UpdateTempSub(this.dgViewSub1);
            DataTable dt = (DataTable)Session[FormID + "_Edit_" + dgViewSub1.ID];
            int RowID = 0;
            int count = 0;
            for (int i = 0; i < this.dgViewSub1.Rows.Count; i++)
            {
                CheckBox cb = (CheckBox)(this.dgViewSub1.Rows[i].FindControl("cbSelect"));
                if (cb != null && cb.Checked)
                {
                    Label hk = (Label)(this.dgViewSub1.Rows[i].Cells[1].FindControl("RowID"));
                    RowID = int.Parse(hk.Text);
                    DataRow[] drs = dt.Select(string.Format("RowID ={0}", hk.Text));
                    count++;
                    for (int j = 0; j < drs.Length; j++)
                        dt.Rows.Remove(drs[j]);

                }
            }
            if (RowID > 0)
            {
                DataRow[] drs = dt.Select("RowID>" + RowID, "RowID");
                for (int j = 0; j < drs.Length; j++)
                {
                    drs[j].BeginEdit();
                    drs[j]["RowID"] = int.Parse(drs[j]["RowID"].ToString()) - count;
                    drs[j].EndEdit();
                }

            }


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
                this.dgViewSub1.DataSource = dt;
                this.dgViewSub1.DataBind();

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
                    WMS.App_Code.JScript.Instance.ShowMessage(this.UpdatePanel1, "该计划单号已经存在！");
                    return;
                }
                para = new DataParameter[] { 
                                             new DataParameter("@Flag", Flag),
                                             new DataParameter("@ScheduleNo", this.txtID.Text.Trim()),
                                             new DataParameter("@BillDate", this.txtBillDate.DateValue),
                                             new DataParameter("@SourceNo", this.txtSourceNo.Text.Trim()),
                                             new DataParameter("@DriverType",0 ),
                                             new DataParameter("@Transport", this.ddlTransport.SelectedValue ),
                                             new DataParameter("@CustomerID", this.txtCustomerID.Text),
                                             new DataParameter("@ParCustomerID", this.txtParCustomerID.Text),
                                             new DataParameter("@CustPerson", this.txtCustPerson.Text),
                                             new DataParameter("@CustPhone",""),
                                             new DataParameter("@LinkPerson", this.txtLinkPerson.Text),
                                             new DataParameter("@LinkPhone", this.txtLinkPhone.Text),
                                             new DataParameter("@LinkAddress", this.txtLinkAddress.Text),
                                             new DataParameter("@AreaSation",this.ddlAreaSation.SelectedValue),
                                              
                                             new DataParameter("@Memo", this.txtMemo.Text.Trim()),
                                             new DataParameter("@Creator", this.txtCreator.Text.Trim()),
                                             new DataParameter("@Updater", Session["EmployeeCode"].ToString())
                                             
                                              };
                Commands[0] = "WMS.InsertDeliverSchedule";

            }
            else //修改
            {
                para = new DataParameter[] { 
                                             new DataParameter("@Flag", Flag),
                                             new DataParameter("@BillDate", this.txtBillDate.DateValue),
                                             new DataParameter("@SourceNo", this.txtSourceNo.Text.Trim()),
                                             new DataParameter("@DriverType", 0),
                                             new DataParameter("@Transport", this.ddlTransport.SelectedValue ),
                                             new DataParameter("@CustomerID", this.txtCustomerID.Text),
                                             new DataParameter("@ParCustomerID", this.txtParCustomerID.Text),
                                             new DataParameter("@CustPerson", this.txtCustPerson.Text),
                                              new DataParameter("@CustPhone",""),
                                             new DataParameter("@LinkPerson", this.txtLinkPerson.Text),
                                             new DataParameter("@LinkPhone", this.txtLinkPhone.Text),
                                             new DataParameter("@LinkAddress", this.txtLinkAddress.Text),
                                              new DataParameter("@AreaSation",this.ddlAreaSation.SelectedValue),
                                             new DataParameter("@Memo", this.txtMemo.Text.Trim()),
                                             new DataParameter("@Updater", Session["EmployeeCode"].ToString()),
                                             new DataParameter("{0}",string.Format("Flag={0} and ScheduleNo='{1}'",Flag, this.txtID.Text.Trim())) };
                Commands[0] = "WMS.UpdateDeliverSchedule";
            }

            DataTable dt = (DataTable)Session[FormID + "_Edit_dgViewSub1"];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if ((int)dt.Rows[i]["PlanQty"] - (int)dt.Rows[i]["RealQty"] < 0)
                {
                    WMS.App_Code.JScript.Instance.ShowMessage(this.UpdatePanel1, dt.Rows[i]["ProductName"].ToString() + " (" + dt.Rows[i]["ProductModel"].ToString() + ")" + " " + dt.Rows[i]["ColorName"].ToString() + "  已经出货数量大于排单数量，请重新修改后保存！");
                    return;
                }


                dt.Rows[i].BeginEdit();
                dt.Rows[i]["ScheduleNo"] = this.txtID.Text.Trim();
                dt.Rows[i]["NotOutStockQty"] = (int)dt.Rows[i]["PlanQty"] - (int)dt.Rows[i]["RealQty"];
                dt.Rows[i].EndEdit();
            }


            DataTable dtProduct = dt.DefaultView.ToTable("Product", true, new string[] { "ProductID", "ColorID", "ProductName", "ProductModel", "ColorName" });
            for (int i = 0; i < dtProduct.Rows.Count; i++)
            {
                object o = dt.Compute("Count(ProductID)", string.Format("ProductID='{0}' and ColorID='{1}'", dtProduct.Rows[i]["ProductID"], dtProduct.Rows[i]["ColorID"]));
                if (o != null)
                {
                    int Qty = int.Parse(o.ToString());
                    if (Qty > 1)
                    {
                        WMS.App_Code.JScript.Instance.ShowMessage(this.UpdatePanel1, dtProduct.Rows[i]["ProductName"].ToString() + " (" + dtProduct.Rows[i]["ProductModel"].ToString() + ")" + " " + dtProduct.Rows[i]["ColorName"].ToString() + " 重复，请重新修改后保存！");
                        return;
                    }

                }

                //o = dt.Compute("Sum(NotOutStockQty)", string.Format("ProductID='{0}' and ColorID='{1}'", dtProduct.Rows[i]["ProductID"], dtProduct.Rows[i]["ColorID"]));
                //if (o != null)
                //{
                //    int Qty = int.Parse(o.ToString());

                //    DataTable dtProductQty = bll.FillDataTable("WMS.SelectScheduleProductRemainQty", new DataParameter[] { new DataParameter("@ScheduleNo", this.txtID.Text), new DataParameter("@ProductID", dtProduct.Rows[i]["ProductID"].ToString()), new DataParameter("@ColorID", dtProduct.Rows[i]["ColorID"].ToString()) });
                //    bool blnvalue = false;
                //    if (dtProductQty.Rows.Count == 0)
                //    {
                //        blnvalue = true;
                //    }
                //    else
                //    {
                //        if (Qty > int.Parse(dtProductQty.Rows[0]["RemainQty"].ToString()))
                //            blnvalue = true;
                //    }
                //    if (blnvalue)
                //    {
                //        WMS.App_Code.JScript.Instance.ShowMessage(this.UpdatePanel1, dtProduct.Rows[i]["ProductName"].ToString() + " (" + dtProduct.Rows[i]["ProductModel"].ToString() + ")" + " " + dtProduct.Rows[i]["ColorName"].ToString() + " 库存不足，请修改出库数量。");
                //        return;

                //    }
                //}
            }



            Commands[1] = "WMS.DeleteDeliverScheduleSub";
            Commands[2] = "WMS.InsertDeliverScheduleSub";
            bll.ExecTran(Commands, para, SubKey, new DataTable[] { dt });



            Response.Redirect(FormID + "View.aspx?SubModuleCode=" + SubModuleCode + "&FormID=" + Server.UrlEncode(FormID) + "&ID=" + Server.UrlEncode(this.txtID.Text));
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {

        }



        #region 从表资料绑定

        private int GetMaxSubID(DataTable dt)
        {
            int i = 1;
            object obj = dt.Compute("Max(SubID)", "");
            if (obj != null && obj.ToString() != "")
            {
                i = (int)obj + 1;
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

        protected void btnCustomer_Click(object sender, EventArgs e)
        {

            BindCustomerInfo(this.txtCustomerID.Text);
            DataTable dt = (DataTable)Session[FormID + "_Edit_dgViewSub1"];

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

        }
        private void BindCustomerInfo(string CustomerID)
        {
            //绑定收货人，收货地址，收货人

            DataTable dtPerson = bll.FillDataTable("Cmd.SelectLinkPerson", new DataParameter[] { new DataParameter("@CUSTOMER_CODE", CustomerID) });
            this.txtLinkPerson.Style = "WIDTH:" + this.txtLinkPerson.Width + ";";
            this.txtLinkPerson.DataSource = dtPerson;
            if (dtPerson.Rows.Count > 0)
                this.txtLinkPerson.Text = dtPerson.Rows[0][0].ToString();


            DataTable dtPhone = bll.FillDataTable("Cmd.SelectLinkPhone", new DataParameter[] { new DataParameter("@CUSTOMER_CODE", CustomerID) });
            this.txtLinkPhone.Style = "WIDTH:" + this.txtLinkPhone.Width + ";";
            this.txtLinkPhone.DataSource = dtPhone;
            if (dtPhone.Rows.Count > 0)
                this.txtLinkPhone.Text = dtPhone.Rows[0][0].ToString();

            DataTable dtAddress = bll.FillDataTable("Cmd.SelectLinkAddress", new DataParameter[] { new DataParameter("@CUSTOMER_CODE", CustomerID) });
            this.txtLinkAddress.Style = "WIDTH:" + this.txtLinkAddress.Width + ";";
            this.txtLinkAddress.DataSource = dtAddress;
            if (dtAddress.Rows.Count > 0)
                this.txtLinkAddress.Text = dtAddress.Rows[0][0].ToString();
        }

        protected void btnInsert_Click(object sender, EventArgs e)
        {
            UpdateTempSub(this.dgViewSub1);
            DataTable dt = (DataTable)Session[FormID + "_Edit_" + dgViewSub1.ID];
            for (int i = 0; i < this.dgViewSub1.Rows.Count; i++)
            {
                CheckBox cb = (CheckBox)(this.dgViewSub1.Rows[i].FindControl("cbSelect"));
                if (cb != null && cb.Checked)
                {
                    Label hk = (Label)(this.dgViewSub1.Rows[i].Cells[1].FindControl("RowID"));

                    DataRow[] drs = dt.Select("RowID>=" + hk.Text);
                    for (int j = 0; j < drs.Length; j++)
                    {
                        drs[j].BeginEdit();
                        drs[j]["RowID"] = int.Parse(hk.Text) + j + 1;
                        drs[j].EndEdit();
                    }
                    
                    
                    DataRow dr;
                    dr = dt.NewRow();
                    dr["RowID"] = int.Parse(hk.Text);
                    dr["SubID"] = GetMaxSubID(dt);
                    dr["PlanDate"] = DateTime.Now;
                    dr["RealQty"] = 0;
                    dr["PlanQty"] = 0;
                    dr["Price"] = 0;
                    dr["Amount"] = 0;

                    dt.Rows.InsertAt(dr, int.Parse(hk.Text));
                    break;

                }
            }
            DataView dv = dt.DefaultView;
            dv.Sort = "RowID";
            dt = dv.ToTable();
            this.dgViewSub1.DataSource = dt;
            this.dgViewSub1.DataBind();

            SetPageSubBtnEnabled();
        }

        protected void dgViewSub1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {

                for (int i = 0; i < e.Row.Cells.Count; i++)
                {
                    e.Row.Cells[i].Attributes.Add("onmousemove", "TableOnMouseMove(this)");
                    e.Row.Cells[i].Attributes.Add("onmousedown", "TableOnMouseDown(this)");
                    e.Row.Cells[i].Attributes.Add("onmouseup", "TableOnMouseUp(this)");
                    //e.Row.Cells[i].Attributes.Add("onmousemove", "SyDG_moveOnTd(this)");
                    //e.Row.Cells[i].Attributes.Add("onmousedown", "SyDG_downOnTd(this)");
                    //e.Row.Cells[i].Attributes.Add("onmouseup", "this.mouseDown=false");
                    //e.Row.Cells[i].Attributes.Add("onmouseout", "this.mouseDown=false");
                }

            }
        }

       
    }
}