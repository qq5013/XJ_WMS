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
    public partial class ScheduleEdit : WMS.App_Code.BasePage
    {
        protected string FormID;
        protected string strID;
        protected string TableName = "WMS_SCHEDULE";
        protected string PrimaryKey = "ScheduleNo";
        protected string SubKey = "Flag,ScheduleNo";
        protected int Flag = 1;
        protected string strCopy = "0";
       
        BLL.BLLBase bll = new BLL.BLLBase();

        protected void Page_Load(object sender, EventArgs e)
        {
            strID = Request.QueryString["ID"] + "";
            FormID = Request.QueryString["FormID"] + "";
            strCopy = Request.QueryString["FormCopy"] + "";
            if (!IsPostBack)
            {
                if (strID != "")
                {


                    DataTable dt = bll.FillDataTable("WMS.SelectSchedule", new DataParameter[] { new DataParameter("{0}", string.Format("{0}='{1}' and Flag={2}", PrimaryKey, strID, Flag)) });
                    BindData(dt);
                    if (strCopy == "1")
                    {
                        strID = "";
                        txtBillDate.changed = "$('#txtID').val(autoCode('OG','Flag=1','txtBillDate'));";
                        this.txtBillDate.DateValue = DateTime.Now;
                        this.txtID.Text = bll.GetAutoCode("OG", DateTime.Now, "Flag=1");
                        this.txtCreator.Text = Session["EmployeeCode"].ToString();
                        this.txtCreateDate.Text = ToYMD(DateTime.Now);
                       
                        SetDataSub();
                    }
                    else
                    {
                        SetTextReadOnly(this.txtID);
                        this.txtBillDate.ReadOnly = true;
                    }


                }
                else
                {
                    txtBillDate.changed = "$('#txtID').val(autoCode('OG','Flag=1','txtBillDate'));";
                    this.txtBillDate.DateValue = DateTime.Now;
                    this.txtID.Text = bll.GetAutoCode("OG", DateTime.Now, "Flag=1");
                    this.txtCreator.Text = Session["EmployeeCode"].ToString();
                    this.txtCreateDate.Text = ToYMD(DateTime.Now);



                    BindDataSub();
                }
                BindPageSize();
                writeJsvar(FormID, SqlCmd, strID);
                SetTextReadOnly(this.txtCreateDate, this.txtCreator, this.txtFactoryName);

            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "", "BindEvent()", true);
            }
            if (strCopy == "1")
                strID = "";
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "Resize", "content_resize();", true);
        }
        private void BindData(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                this.txtID.Text = dt.Rows[0]["ScheduleNo"].ToString();
                this.txtBillDate.DateValue = dt.Rows[0]["BillDate"];
               
                this.txtFactoryID.Text = dt.Rows[0]["FactoryID"].ToString();
                this.txtFactoryName.Text = dt.Rows[0]["FactName"].ToString();
                this.txtMemo.Text = dt.Rows[0]["Memo"].ToString();
                this.txtCreator.Text = dt.Rows[0]["Creator"].ToString();
                this.txtCreateDate.Text = ToYMD(dt.Rows[0]["CreateDate"]);
                this.txtTechRequery.Text = dt.Rows[0]["TechRequery"].ToString();
                this.txtOrderFunction.Text = dt.Rows[0]["OrderFunction"].ToString();
                this.txtPackRequery.Text = dt.Rows[0]["PackRequery"].ToString();
                this.txtOrderParts.Text = dt.Rows[0]["OrderParts"].ToString();
                BindDataSub();    
            }
        }

        private void BindDataSub()
        {
            DataTable dt = bll.FillDataTable("WMS.SelectScheduleSub", new DataParameter[] { new DataParameter("{0}", string.Format("ScheduleNo='{0}' and Flag={1}", this.txtID.Text, Flag)) });
            DataColumn dc = new DataColumn("dcNewPlan", Type.GetType("System.String"));
            dt.Columns.Add(dc);
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
        private void SetDataSub()
        {
            DataTable dtSub = (DataTable)Session[FormID + "_Edit_" + dgViewSub1.ID];


            for (int i = 0; i < dtSub.Rows.Count; i++)
            {
                dtSub.Rows[i]["StarNo"] = "";
                dtSub.Rows[i]["EndNo"] = "";
                
            }
            dtSub.AcceptChanges();
            this.dgViewSub1.DataSource = dtSub;
            this.dgViewSub1.DataBind();
            Session[FormID + "_Edit_" + dgViewSub1.ID] = dtSub;



        }

        protected void dgViewSub1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.RowIndex % 2 == 1)
                    e.Row.CssClass = " bottomtable";

                DataRowView drv = e.Row.DataItem as DataRowView;
                //txt 
                if (drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("RealQty")].ToString() != "")
                {

                    if ((int)drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("RealQty")] > 0  && strCopy=="0")
                    {
                        SetTextReadOnly((TextBox)e.Row.FindControl("PlanQty"),(TextBox)e.Row.FindControl("StarNo"), (TextBox)e.Row.FindControl("EndNo"));

                        ((CheckBox)e.Row.FindControl("cbSelect")).Enabled = false;
                        ((Button)e.Row.FindControl("btnProduct")).Enabled = false;
                    }
                }
                SetTextReadOnly((TextBox)e.Row.FindControl("ProductModel"),(TextBox)e.Row.FindControl("ProductName"), (TextBox)e.Row.FindControl("ProductFModel"),
                                       (TextBox)e.Row.FindControl("ColorName"),(TextBox)e.Row.FindControl("Amount"),(TextBox)e.Row.FindControl("RealQty"));



                ((Label)e.Row.FindControl("RowID")).Text = "" + ((e.Row.RowIndex + 1) + dgViewSub1.PageSize * dgViewSub1.PageIndex);
                ((TextBox)e.Row.FindControl("ProductID")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("ProductID")].ToString();
                ((TextBox)e.Row.FindControl("ProductModel")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("ProductModel")].ToString();
                ((TextBox)e.Row.FindControl("ProductName")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("ProductName")].ToString();
                ((TextBox)e.Row.FindControl("ProductFModel")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("ProductFModel")].ToString();
                ((TextBox)e.Row.FindControl("ColorID")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("ColorID")].ToString();
                ((TextBox)e.Row.FindControl("ColorName")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("ColorName")].ToString();
                ((TextBox)e.Row.FindControl("PlanQty")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("PlanQty")].ToString();
                ((TextBox)e.Row.FindControl("Price")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("Price")].ToString();
                ((TextBox)e.Row.FindControl("Amount")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("Amount")].ToString();
                ((TextBox)e.Row.FindControl("StarNo")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("StarNo")].ToString();
                ((TextBox)e.Row.FindControl("EndNo")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("EndNo")].ToString();
                ((UserControl.Calendar)e.Row.FindControl("PlanDate")).DateValue = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("PlanDate")];
                if (strCopy == "1")
                    ((TextBox)e.Row.FindControl("RealQty")).Text = "0";
                else
                    ((TextBox)e.Row.FindControl("RealQty")).Text = drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("RealQty")].ToString();

            }
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
                dr["ProductFModel"] = ((TextBox)dgv.Rows[i].FindControl("ProductFModel")).Text;
               
                dr["ColorID"] = ((TextBox)dgv.Rows[i].FindControl("ColorID")).Text;
                dr["ColorName"] = ((TextBox)dgv.Rows[i].FindControl("ColorName")).Text;
                if (((TextBox)dgv.Rows[i].FindControl("PlanQty")).Text.Trim().Length == 0)
                    dr["PlanQty"] = 0;
                else
                    dr["PlanQty"] = ((TextBox)dgv.Rows[i].FindControl("PlanQty")).Text;
                if (((TextBox)dgv.Rows[i].FindControl("Price")).Text.Trim().Length == 0)
                    dr["Price"] = 0;
                else
                    dr["Price"] = ((TextBox)dgv.Rows[i].FindControl("Price")).Text;
                if (((TextBox)dgv.Rows[i].FindControl("Amount")).Text.Trim().Length == 0)
                    dr["Amount"] = 0;
                else
                    dr["Amount"] = ((TextBox)dgv.Rows[i].FindControl("Amount")).Text;
                dr["StarNo"] = ((TextBox)dgv.Rows[i].FindControl("StarNo")).Text ;
                dr["EndNo"] = ((TextBox)dgv.Rows[i].FindControl("EndNo")).Text;
                dr["PlanDate"] = ((UserControl.Calendar)dgv.Rows[i].FindControl("PlanDate")).DateValue;
                dr["dcNewPlan"] = ((UserControl.Calendar)dgv.Rows[i].FindControl("PlanDate")).tDate.Text.Substring(2, 2) + ((UserControl.Calendar)dgv.Rows[i].FindControl("PlanDate")).tDate.Text.Substring(5, 2);
                if (strCopy == "1")
                    dr["RealQty"] = 0;
                else
                    dr["RealQty"] = ((TextBox)dgv.Rows[i].FindControl("RealQty")).Text;
                
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
                dr["PlanQty"] = 1;
                dr["Price"] = dt1.Rows[i]["StandPrice"];
                dr["Amount"] = dt1.Rows[i]["StandPrice"];
                dr["PlanDate"] = DateTime.Now;
                 
                dr["RealQty"] = 0;
            }
            //UpdateTempSub(this.dgViewSub1);
            this.dgViewSub1.DataSource = dt;
            this.dgViewSub1.DataBind();
            SetPageSubBtnEnabled();
        }

        protected void btnAddDetail_Click(object sender, EventArgs e)
        {
            if (this.txtFactoryID.Text == "")
            {

                WMS.App_Code.JScript.Instance.ShowMessage(this.UpdatePanel1, "供应商不能为空，请先选择！");
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
            dr["PlanQty"] = 1;
            dr["Price"] = 0;
            dr["Amount"] = 0;
             
            dt.Rows.Add(dr);
            this.dgViewSub1.DataSource = dt;
            this.dgViewSub1.DataBind();
            if (pagecount != this.dgViewSub1.PageCount)
                MovePage(this.dgViewSub1, this.dgViewSub1.PageCount - 1);

            SetPageSubBtnEnabled();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "", "BindEvent()", true);
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
            //if (this.txtSourceNo.Text.Trim().Length > 0)
            //{
            //    int Count = bll.GetRowCount(TableName, string.Format("ScheduleNo<>'{0}' and Flag={1} and SourceNo='{2}'", this.txtID.Text.Trim(), Flag, this.txtSourceNo.Text));
            //    if (Count > 0)
            //    {
            //        WMS.App_Code.JScript.Instance.ShowMessage(this.UpdatePanel1, "该采购单号已经存在！");
            //        return;
            //    }
            //}


            if (strID == "") //新增
            {
                int Count = bll.GetRowCount(TableName, string.Format("ScheduleNo='{0}' and Flag={1}", this.txtID.Text.Trim(), Flag));
                if (Count > 0)
                {
                    WMS.App_Code.JScript.Instance.ShowMessage(this.UpdatePanel1, "该采购单号已经存在！");
                    return;
                }
                para = new DataParameter[] { 
                                             new DataParameter("@Flag", Flag),
                                             new DataParameter("@ScheduleNo", this.txtID.Text.Trim()),
                                             new DataParameter("@BillDate", this.txtBillDate.DateValue),
                                             new DataParameter("@SourceNo",""),
                                             new DataParameter("@FactoryOrderNo", ""),
                                             new DataParameter("@FactoryID", this.txtFactoryID.Text.Trim()),
                                             new DataParameter("@Memo", this.txtMemo.Text.Trim()),
                                             new DataParameter("@Creator", this.txtCreator.Text.Trim()),
                                             new DataParameter("@Updater",Session["EmployeeCode"].ToString()),
                                             new DataParameter("@TechRequery",this.txtTechRequery.Text),
                                             new DataParameter("@OrderFunction",this.txtOrderFunction.Text),
                                             new DataParameter("@PackRequery",this.txtPackRequery.Text),
                                             new DataParameter("@OrderParts",this.txtOrderParts.Text)
                                              }; 
                Commands[0] = "WMS.InsertSchedule";

            }
            else //修改
            {
                para = new DataParameter[] { new DataParameter("@BillDate", this.txtBillDate.DateValue),
                                             new DataParameter("@SourceNo", ""),
                                             new DataParameter("@FactoryOrderNo", ""),
                                             new DataParameter("@FactoryID", this.txtFactoryID.Text.Trim()),
                                             new DataParameter("@Memo", this.txtMemo.Text.Trim()),
                                             new DataParameter("@Updater",Session["EmployeeCode"].ToString()),
                                             new DataParameter("@TechRequery",this.txtTechRequery.Text),
                                             new DataParameter("@OrderFunction",this.txtOrderFunction.Text),
                                             new DataParameter("@PackRequery",this.txtPackRequery.Text),
                                             new DataParameter("@OrderParts",this.txtOrderParts.Text),
                                             new DataParameter("{0}",string.Format("Flag={0} and ScheduleNo='{1}'",Flag, this.txtID.Text.Trim())) };
                Commands[0] = "WMS.UpdateSchedule";
            }

            DataTable dt = (DataTable)Session[FormID + "_Edit_dgViewSub1"];
            DataRow[] drs = dt.Select(string.Format("ScheduleNo<>'{0}'", this.txtID.Text));
            for (int i = 0; i < drs.Length; i++)
            {
                drs[i].BeginEdit();
                drs[i]["ScheduleNo"] = this.txtID.Text.Trim();
                drs[i].EndEdit();
            }

           
            //判断是否重复
            for (int j = 0; j < dt.Rows.Count; j++)
            {
                if (dt.Rows[j]["StarNo"].ToString() != "")
                {
                    if (bll.GetRowCount("WMS_SCHEDULESUB", string.Format("Flag=1 and ColorID='{0}' and ProductID='{1}' and	(StarNo<='{3}' and EndNo>'{2}' or EndNo<='{3}' and EndNo>='{2}') and ScheduleNo!='{4}'", dt.Rows[j]["ColorID"].ToString(), dt.Rows[j]["ProductID"].ToString(), dt.Rows[j]["StarNo"].ToString(), dt.Rows[j]["EndNo"].ToString(), this.txtID.Text)) > 0)
                    {
                        WMS.App_Code.JScript.Instance.ShowMessage(this.UpdatePanel1, "序号:" + dt.Rows[j]["RowID"].ToString() + "( " + dt.Rows[j]["ProductModel"].ToString() + " )序列号区间重复！");
                        return;
                    }
                }
            }
            DataView dv = dt.DefaultView;
            dv.RowFilter = "StarNo<>''";
            DataTable dtNewSub = dv.ToTable(true, new string[] { "ProductID", "ColorID" });
            for (int i = 0; i < dtNewSub.Rows.Count; i++)
            {
                drs = dt.Select(string.Format("ProductID='{0}' and ColorID='{1}'", dtNewSub.Rows[i]["ProductID"].ToString(), dtNewSub.Rows[i]["ColorID"].ToString()));
                for (int j = 0; j < drs.Length; j++)
                {

                    if (bll.GetRowCount("WMS_SCHEDULESUB", string.Format("Flag=1 and ScheduleNo='{0}'and RowID={1} and	ProductID='{2}' and PlanQty={3} and	StarNo='{4}' and EndNo='{5}' and CONVERT(nvarchar(10),plandate,111)='{6}'", this.txtID.Text, drs[j]["RowID"].ToString(), drs[j]["ProductID"].ToString(), drs[j]["PlanQty"].ToString(), drs[j]["StarNo"].ToString(), drs[j]["EndNo"].ToString(), ToYMD(drs[j]["plandate"]))) > 0)
                        continue;

                    double A1 = double.Parse(drs[j]["StarNo"].ToString());
                    double A2 = double.Parse(drs[j]["EndNo"].ToString());



                    for (int k = j + 1; k < drs.Length; k++)
                    {
                        double Begin = A1;
                        double End = A2;
                        
                        double B1 = double.Parse(drs[k]["StarNo"].ToString());
                        double B2 = double.Parse(drs[k]["EndNo"].ToString());
                        if (B1 > A1)
                            Begin = B1;
                        if (B2 < A2)
                            End = B2;
                        if (End - Begin >= 0)
                        {
                            WMS.App_Code.JScript.Instance.ShowMessage(this.UpdatePanel1, "序号:" + dt.Rows[k]["RowID"].ToString() + "( " + dt.Rows[k]["ProductModel"].ToString() + " )序列号区间重复！");
                            return;
                        }
                    }
                }
            
            }



            dt.Columns.Remove("dcNewPlan");

            Commands[1] = "WMS.DeleteScheduleSub";
            Commands[2] = "WMS.InsertScheduleSub";
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

        protected void btnGetSection_Click(object sender, EventArgs e)
        {
            UpdateTempSub(this.dgViewSub1);
            DataTable dtSub = (DataTable)Session[FormID + "_Edit_" + dgViewSub1.ID];

            DataTable dtNewSub = dtSub.DefaultView.ToTable(true, new string[] { "ProductID", "dcNewPlan" });
            for (int i = 0; i < dtNewSub.Rows.Count; i++)
            {
                DataTable dtSection = bll.FillDataTable("WMS.SelectMaxEndNo", new DataParameter[] { new DataParameter("@ProductID", dtNewSub.Rows[i]["ProductID"].ToString()), new DataParameter("@PlanDate", dtNewSub.Rows[i]["dcNewPlan"].ToString()), new DataParameter("@ScheduleNo", this.txtID.Text) });
                int MaxID = int.Parse(dtSection.Rows[0][0].ToString().Substring(6, 5));

                DataRow[] drs = dtSub.Select(string.Format("ProductID='{0}' and dcNewPlan='{1}'", dtNewSub.Rows[i]["ProductID"].ToString(), dtNewSub.Rows[i]["dcNewPlan"].ToString()), "RowID");
                for (int j = 0; j < drs.Length; j++)
                {
                        //if (bll.GetRowCount("WMS_SCHEDULESUB", string.Format("Flag=1 and ScheduleNo='{0}'and RowID={1} and	ProductID='{2}' and PlanQty={3} and	StarNo='{4}' and EndNo='{5}' and CONVERT(nvarchar(10),plandate,111)='{6}'", this.txtID.Text, drs[j]["RowID"].ToString(), drs[j]["ProductID"].ToString(), drs[j]["PlanQty"].ToString(), drs[j]["StarNo"].ToString(), drs[j]["EndNo"].ToString(), ToYMD(drs[j]["plandate"]))) > 0)
                        //    continue;
                    int planQty = int.Parse(drs[j]["PlanQty"].ToString());
                    string date = ToYMD(drs[j]["PlanDate"]).Replace("/", "").Substring(2);
                    drs[j]["StarNo"] = date + (MaxID + 1).ToString().PadLeft(5, '0');
                    drs[j]["EndNo"] = date + (MaxID + planQty).ToString().PadLeft(5, '0');
                    MaxID = MaxID + planQty;
                }

            }
            dtSub.AcceptChanges();
            this.dgViewSub1.DataSource = dtSub;
            this.dgViewSub1.DataBind();
            Session[FormID + "_Edit_" + dgViewSub1.ID] = dtSub;





        }

       
    }
}