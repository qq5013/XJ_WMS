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
    public partial class DeliverTransferEdit : WMS.App_Code.BasePage
    {
        protected string FormID;
        protected string ID;
        protected string TableName = "WMS_Migration";
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
                BindOther();
                if (ID != "")
                {
                    DataTable dt = bll.FillDataTable("WMS.SelectDeliverTransfer", new DataParameter[] { new DataParameter("{0}", string.Format("{0}='{1}' and Flag={2}", PrimaryKey, ID, Flag)) });
                    BindData(dt);
                    SetTextReadOnly(this.txtID);
                    this.txtBillDate.ReadOnly = true;
                }
                else
                {
                    txtBillDate.changed = "$('#txtID').val(autoCode('DT','Flag=2','txtBillDate'));";
                    this.txtBillDate.DateValue = DateTime.Now;
                    this.txtID.Text = bll.GetAutoCode("DT", DateTime.Now, "Flag=2");
                    this.txtCreator.Text = Session["EmployeeCode"].ToString();
                    this.txtCreateDate.Text = ToYMD(DateTime.Now);
                    
                    
                   
                    BindDataSub();
                }
                BindPageSize();
                writeJsvar(FormID, TableName, PrimaryKey, ID);
                SetTextReadOnly(this.txtCreateDate, this.txtCreator);

            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "", "BindEvent()", true);
            }
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "Resize", "content_resize();", true);
        }
        private void BindOther()
        {
            DataTable dt = bll.FillDataTable("Cmd.SelectWarehouse", new DataParameter[] { new DataParameter("{0}", "IsManage=0") });
            this.ddlStockFunction.DataValueField = "CMD_WAREHOUSE_ID";
            this.ddlStockFunction.DataTextField = "WAREHOUSE_NAME";
            this.ddlStockFunction.DataSource = dt;
            this.ddlStockFunction.DataBind();
        }

        private void BindData(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                this.txtID.Text = dt.Rows[0]["BillID"].ToString();
                this.txtBillDate.DateValue = dt.Rows[0]["BillDate"];
                this.ddlStockFunction.SelectedValue = dt.Rows[0]["StockFunction"].ToString();
              
                this.txtMemo.Text = dt.Rows[0]["Memo"].ToString();
                this.txtCreator.Text = dt.Rows[0]["Creator"].ToString();
                this.txtCreateDate.Text = ToYMD(dt.Rows[0]["CreateDate"]);

                BindDataSub();    
            }
        }

        private void BindDataSub()
        {
            DataTable dt = bll.FillDataTable("WMS.SelectDeliverTransferSub", new DataParameter[] { new DataParameter("{0}", string.Format("BillID='{0}' and Flag={1}", this.txtID.Text, Flag)), new DataParameter("@BillID", this.txtID.Text) });
            Session[FormID + "_Edit_dgViewSub1"] = dt;
            if (!dt.Columns.Contains("PACK_QTY"))
            {
                DataColumn dc1 = new DataColumn("PACK_QTY", typeof(int));
                DataColumn dc2 = new DataColumn("SubCount", typeof(int));
                DataColumn dc3 = new DataColumn("ProductVolume", typeof(float));
                dt.Columns.Add(dc1);
                dt.Columns.Add(dc2);
                dt.Columns.Add(dc3);

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
                dgViewSub1.DataSource = dt;
                dgViewSub1.DataBind();
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
                
                SetTextReadOnly((TextBox)e.Row.FindControl("ProductModel"), (TextBox)e.Row.FindControl("ProductFModel"), (TextBox)e.Row.FindControl("ProductName"), (TextBox)e.Row.FindControl("ColorName"), (TextBox)e.Row.FindControl("RealQty"));

                ((Label)e.Row.FindControl("RowID")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("RowID")].ToString();
                ((TextBox)e.Row.FindControl("ProductID")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("ProductID")].ToString();
                ((TextBox)e.Row.FindControl("ProductModel")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("ProductModel")].ToString();
                ((TextBox)e.Row.FindControl("ProductFModel")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("ProductFModel")].ToString();
                ((TextBox)e.Row.FindControl("ProductName")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("ProductName")].ToString();

                ((TextBox)e.Row.FindControl("ColorID")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("ColorID")].ToString();
                ((TextBox)e.Row.FindControl("ColorName")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("ColorName")].ToString();
                ((TextBox)e.Row.FindControl("Qty")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("Qty")].ToString();
                
                

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
                dr["BillID"] = this.txtID.Text.Trim();
                dr["RowID"] = ((Label)dgv.Rows[i].FindControl("RowID")).Text;
                dr["ProductID"] = ((TextBox)dgv.Rows[i].FindControl("ProductID")).Text;
                dr["ProductName"] = ((TextBox)dgv.Rows[i].FindControl("ProductName")).Text;
                dr["ProductModel"] = ((TextBox)dgv.Rows[i].FindControl("ProductModel")).Text;

                dr["ColorID"] = ((TextBox)dgv.Rows[i].FindControl("ColorID")).Text;
                dr["ColorName"] = ((TextBox)dgv.Rows[i].FindControl("ColorName")).Text;
                dr["Qty"] = ((TextBox)dgv.Rows[i].FindControl("Qty")).Text;
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
                }
                dr["RowID"] = i + cur + 1;
                dr["Flag"] = Flag;
                dr["BillID"] = this.txtID.Text.Trim();
                dr["ProductID"] = dt1.Rows[i]["PRODUCT_CODE"];
                dr["ProductName"] = dt1.Rows[i]["Name"];
                dr["ProductModel"] = dt1.Rows[i]["ProductModel"];
                dr["ProductFModel"] = dt1.Rows[i]["ProductFModel"];
                dr["ColorID"] = dt1.Rows[i]["ColorID"];
                dr["ColorName"] = dt1.Rows[i]["ColorName"];
                dr["PACK_QTY"] = dt1.Rows[i]["PACK_QTY"];
                dr["SubCount"] = dt1.Rows[i]["SubCount"];
                dr["ProductVolume"] = dt1.Rows[i]["ProductVolume"];
                
                dr["Qty"] = 0;
                dr["Volume"] = 0;
                dr["StockQty"] = GetStockQty(dr["ProductID"].ToString(), dr["ColorID"].ToString());
               
            }
            //UpdateTempSub(this.dgViewSub1);
            this.dgViewSub1.DataSource = dt;
            this.dgViewSub1.DataBind();
            object o = dt.Compute("SUM(Qty)", "");
           
            this.txtTotalQty.Text = o.ToString();
            Session[FormID + "_Edit_dgViewSub1"] = dt;
            SetPageSubBtnEnabled();
        }

        private int  GetStockQty(string ProductID, string ColorID)
        {
            int Qty = 0;
            DataTable dtQty = bll.FillDataTable("WMS.SelectProductStockQty", new DataParameter[] { new DataParameter("@BillID", this.txtID.Text), new DataParameter("@ProductID", ProductID), new DataParameter("@ColorID", ColorID) });
            if (dtQty.Rows.Count > 0)
            {
                Qty = int.Parse(dtQty.Rows[0]["StockQty"].ToString());
            }
            return Qty;
        }

        protected void btnAddDetail_Click(object sender, EventArgs e)
        {
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
            dr["Qty"] = 1;
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
                    WMS.App_Code.JScript.Instance.ShowMessage(this.UpdatePanel1, "该调拨单号已经存在！");
                    return;
                }
                para = new DataParameter[] { 
                                             new DataParameter("@Flag", Flag),
                                             new DataParameter("@BillID", this.txtID.Text.Trim()),
                                             new DataParameter("@BillDate", this.txtBillDate.DateValue),
                                             new DataParameter("@StockFunction", this.ddlStockFunction.SelectedValue),
                                       
                                             new DataParameter("@Memo", this.txtMemo.Text.Trim()),
                                             new DataParameter("@Creator", this.txtCreator.Text.Trim()),
                                             new DataParameter("@Updater",Session["EmployeeCode"].ToString())
                                             
                                              };
                Commands[0] = "WMS.InsertDeliverTransfer";

            }
            else //修改
            {
                para = new DataParameter[] {   new DataParameter("@Flag", Flag),
                                             new DataParameter("@BillID", this.txtID.Text.Trim()),
                                             new DataParameter("@BillDate", this.txtBillDate.DateValue),
                                             new DataParameter("@StockFunction", this.ddlStockFunction.SelectedValue),
                                         
                                             new DataParameter("@Memo", this.txtMemo.Text.Trim()),
                                             new DataParameter("@Updater",Session["EmployeeCode"].ToString()),
                                             new DataParameter("{0}",string.Format("Flag={0} and BillID='{1}'",Flag, this.txtID.Text.Trim())) };
                Commands[0] = "WMS.UpdateDeliverTransfer";
            }
            DataTable dt = (DataTable)Session[FormID + "_Edit_dgViewSub1"];

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

                o = dt.Compute("Sum(Qty)", string.Format("ProductID='{0}' and ColorID='{1}'", dtProduct.Rows[i]["ProductID"], dtProduct.Rows[i]["ColorID"]));
                if (o != null)
                {
                    int Qty = int.Parse(o.ToString());
                    int  StockQty = GetStockQty(dtProduct.Rows[i]["ProductID"].ToString(), dtProduct.Rows[i]["ColorID"].ToString());
                    if (Qty > StockQty)
                    {
                        WMS.App_Code.JScript.Instance.ShowMessage(this.UpdatePanel1, dtProduct.Rows[i]["ProductName"].ToString() + " (" + dtProduct.Rows[i]["ProductModel"].ToString() + ")" + " " + dtProduct.Rows[i]["ColorName"].ToString() + " 库存不足，请修改出库数量。");
                        return;

                    }
                }

            }


           
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i].BeginEdit();
                dt.Rows[i]["BillID"] = this.txtID.Text.Trim();

                int Qty = (int)dt.Rows[i]["Qty"];
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
            Commands[1] = "WMS.DeleteDeliverTransferSub";
            Commands[2] = "WMS.InsertDeliverTransferSub";
            bll.ExecTran(Commands, para, SubKey, new DataTable[] { dt });



            Response.Redirect(FormID + "View.aspx?SubModuleCode=" + SubModuleCode + "&FormID=" + Server.UrlEncode(FormID) + "&ID=" + Server.UrlEncode(this.txtID.Text));
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

       
    }
}