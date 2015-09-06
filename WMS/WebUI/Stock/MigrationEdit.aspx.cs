using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using IDAL;

namespace WMS.WebUI.Stock
{
    public partial class MigrationEdit : WMS.App_Code.BasePage
    {
        protected string FormID;
        protected string ID;
        protected string TableName = "WMS_Migration";
        protected string PrimaryKey = "BillID";
        protected string SubKey = "Flag,BillID";
        protected int Flag = 1;
       
        BLL.BLLBase bll = new BLL.BLLBase();

        protected void Page_Load(object sender, EventArgs e)
        {
            ID = Request.QueryString["ID"] + "";
            FormID = Request.QueryString["FormID"] + "";
            if (!IsPostBack)
            {
                if (ID != "")
                {
                    DataTable dt = bll.FillDataTable("WMS.SelectMigration", new DataParameter[] { new DataParameter("{0}", string.Format("{0}='{1}' and Flag={2}", PrimaryKey, ID, Flag)) });
                    BindData(dt);
                    SetTextReadOnly(this.txtID);
                    this.txtBillDate.ReadOnly = true;
                }
                else
                {
                    txtBillDate.changed = "$('#txtID').val(autoCode('MS','Flag=1','txtBillDate'));";
                    this.txtBillDate.DateValue = DateTime.Now;
                    this.txtID.Text = bll.GetAutoCode("MS", DateTime.Now, "Flag=1");
                    this.txtCreator.Text = Session["EmployeeCode"].ToString();
                    this.txtCreateDate.Text = ToYMD(DateTime.Now);
                    
                    
                   
                    BindDataSub();
                }
                BindPageSize();
                writeJsvar(FormID,SqlCmd, ID);
                SetTextReadOnly(this.txtCreateDate, this.txtCreator);

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
                this.txtFactoryID.Text = dt.Rows[0]["FactoryID"].ToString(); ;
                this.txtFactoryName.Text = dt.Rows[0]["FactoryName"].ToString();
                this.ddlStockType.SelectedValue = dt.Rows[0]["OutStockType"].ToString();
                this.txtMemo.Text = dt.Rows[0]["Memo"].ToString();
                this.txtCreator.Text = dt.Rows[0]["Creator"].ToString();
                this.txtCreateDate.Text = ToYMD(dt.Rows[0]["CreateDate"]);

                BindDataSub();    
            }
        }

        private void BindDataSub()
        {
            DataTable dt = bll.FillDataTable("WMS.SelectMigrationSub", new DataParameter[] { new DataParameter("{0}", string.Format("BillID='{0}' and Flag={1}", this.txtID.Text, Flag)) });
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
               
                SetTextReadOnly((TextBox)e.Row.FindControl("ProductModel"),(TextBox)e.Row.FindControl("ProductName"), 
                                      (TextBox)e.Row.FindControl("ColorName"));



                ((Label)e.Row.FindControl("RowID")).Text = "" + ((e.Row.RowIndex + 1) + dgViewSub1.PageSize * dgViewSub1.PageIndex);
                ((TextBox)e.Row.FindControl("ProductID")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("ProductID")].ToString();
                ((TextBox)e.Row.FindControl("ProductModel")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("ProductModel")].ToString();
                ((TextBox)e.Row.FindControl("ProductName")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("ProductName")].ToString();
                 
                ((TextBox)e.Row.FindControl("ColorID")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("ColorID")].ToString();
                ((TextBox)e.Row.FindControl("ColorName")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("ColorName")].ToString();
                ((TextBox)e.Row.FindControl("Qty")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("Qty")].ToString();
               
                ((TextBox)e.Row.FindControl("StarNo")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("StarNo")].ToString();
                ((TextBox)e.Row.FindControl("EndNo")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("EndNo")].ToString();
                

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
                //dr["RowID"] = ((Label)dgv.Rows[i].Cells[1].FindControl("RowID")).Text;
                //dr["ProductID"] = ((TextBox)dgv.Rows[i].Cells[2].FindControl("ProductID")).Text ;
                //dr["ProductName"] = ((TextBox)dgv.Rows[i].Cells[3].FindControl("ProductName")).Text;
                //dr["ProductModel"] = ((TextBox)dgv.Rows[i].Cells[2].FindControl("ProductModel")).Text;
               
                //dr["ColorID"] = ((TextBox)dgv.Rows[i].Cells[4].FindControl("ColorID")).Text;
                //dr["ColorName"] = ((TextBox)dgv.Rows[i].Cells[4].FindControl("ColorName")).Text;
                //dr["Qty"] = ((TextBox)dgv.Rows[i].Cells[5].FindControl("Qty")).Text ;
                
                //dr["StarNo"] = ((TextBox)dgv.Rows[i].Cells[6].FindControl("StarNo")).Text ;
                //dr["EndNo"] = ((TextBox)dgv.Rows[i].Cells[7].FindControl("EndNo")).Text;


                dr["RowID"] = ((Label)dgv.Rows[i].FindControl("RowID")).Text;
                dr["ProductID"] = ((TextBox)dgv.Rows[i].FindControl("ProductID")).Text;
                dr["ProductName"] = ((TextBox)dgv.Rows[i].FindControl("ProductName")).Text;
                dr["ProductModel"] = ((TextBox)dgv.Rows[i].FindControl("ProductModel")).Text;

                dr["ColorID"] = ((TextBox)dgv.Rows[i].FindControl("ColorID")).Text;
                dr["ColorName"] = ((TextBox)dgv.Rows[i].FindControl("ColorName")).Text;
                dr["Qty"] = ((TextBox)dgv.Rows[i].FindControl("Qty")).Text;

                dr["StarNo"] = ((TextBox)dgv.Rows[i].FindControl("StarNo")).Text;
                dr["EndNo"] = ((TextBox)dgv.Rows[i].FindControl("EndNo")).Text;
                 
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
                dr["ColorID"] = dt1.Rows[i]["ColorID"];
                dr["ColorName"] = dt1.Rows[i]["ColorName"];
                dr["Qty"] = 1;
               
            }
            //UpdateTempSub(this.dgViewSub1);
            this.dgViewSub1.DataSource = dt;
            this.dgViewSub1.DataBind();
            SetPageSubBtnEnabled();
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
                    WMS.App_Code.JScript.Instance.ShowMessage(this.UpdatePanel1, "该计划单号已经存在！");
                    return;
                }
                para = new DataParameter[] { 
                                             new DataParameter("@Flag", Flag),
                                             new DataParameter("@BillID", this.txtID.Text.Trim()),
                                             new DataParameter("@BillDate", this.txtBillDate.DateValue),
                                              new DataParameter("@FactoryID", this.txtFactoryID.Text),
                                             new DataParameter("@StockFunction", 0),
                                             new DataParameter("@OutStockType", this.ddlStockType.SelectedValue),
                                             new DataParameter("@Memo", this.txtMemo.Text.Trim()),
                                             new DataParameter("@Creator", this.txtCreator.Text.Trim()),
                                             new DataParameter("@Updater",Session["EmployeeCode"].ToString())
                                             
                                              };
                Commands[0] = "WMS.InsertMigration";

            }
            else //修改
            {
                para = new DataParameter[] {   new DataParameter("@Flag", Flag),
                                             new DataParameter("@BillID", this.txtID.Text.Trim()),
                                             new DataParameter("@BillDate", this.txtBillDate.DateValue),
                                              new DataParameter("@FactoryID", this.txtFactoryID.Text),
                                             new DataParameter("@StockFunction", 0),
                                             new DataParameter("@OutStockType", this.ddlStockType.SelectedValue),
                                             new DataParameter("@Memo", this.txtMemo.Text.Trim()),
                                             new DataParameter("@Updater",Session["EmployeeCode"].ToString()),
                                             new DataParameter("{0}",string.Format("Flag={0} and BillID='{1}'",Flag, this.txtID.Text.Trim())) };
                Commands[0] = "WMS.UpdateMigration";
            }




            DataTable dt = (DataTable)Session[FormID + "_Edit_dgViewSub1"];
            
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i].BeginEdit();
                dt.Rows[i]["BillID"] = this.txtID.Text.Trim();
                if (this.ddlStockType.SelectedValue != "0")
                {
                    dt.Rows[i]["Qty"] = 0;
                }
                else
                {
                    dt.Rows[i]["StarNo"] = "";
                    dt.Rows[i]["EndNo"] = "";

                }
                dt.Rows[i].EndEdit();
            }

            if (this.ddlStockType.SelectedValue == "0")
            {
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

                        DataTable dtProductQty = bll.FillDataTable("WMS.SelectProductStockQty", new DataParameter[] { new DataParameter("@BillID", this.txtID.Text), new DataParameter("@ProductID", dtProduct.Rows[i]["ProductID"].ToString()), new DataParameter("@ColorID", dtProduct.Rows[i]["ColorID"].ToString()) });
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



            Commands[1] = "WMS.DeleteMigrationSub";
            Commands[2] = "WMS.InsertMigrationSub";
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

       
    }
}