using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using IDAL;
using System.Reflection;
using System.IO;

namespace WMS.WebUI.InStock
{
    public partial class ScheduleView : WMS.App_Code.BasePage
    {
        protected string FormID;
        protected string ID;
        protected string TableName = "View_WMS_Schedule";
        protected string PrimaryKey = "ScheduleNo";
        protected int Flag = 1;
        private string Filter = "Flag=1";

        BLL.BLLBase bll = new BLL.BLLBase();

        protected void Page_Load(object sender, EventArgs e)
        {
            ID = Request.QueryString["ID"] + "";
            FormID = Request.QueryString["FormID"] + "";
            if (!IsPostBack)
            {
                DataTable dt = bll.FillDataTable("WMS.SelectSchedule", new DataParameter[] { new DataParameter("{0}", string.Format("{0}='{1}' and Flag={2}", PrimaryKey, ID, Flag)) });
                BindData(dt);
                writeJsvar(FormID, TableName, PrimaryKey, ID);
                BindPageSize();
            }
        }

        private void BindData(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                this.txtID.Text = dt.Rows[0]["ScheduleNo"].ToString();
                this.txtBillDate.Text = ToYMD(dt.Rows[0]["BillDate"]);
                
                this.txtFactoryID.Text = dt.Rows[0]["FactoryID"].ToString();
                this.txtFactoryName.Text = dt.Rows[0]["FactName"].ToString();
                this.txtMemo.Text = dt.Rows[0]["Memo"].ToString();
                this.txtCreator.Text = dt.Rows[0]["Creator"].ToString();
                this.txtCreateDate.Text = ToYMD(dt.Rows[0]["CreateDate"]);
               

                this.txtCheckUser.Text = dt.Rows[0]["CheckUser"].ToString();
                this.txtCheckDate.Text = ToYMD(dt.Rows[0]["CheckDate"]);
                this.txtReCheckUser.Text = dt.Rows[0]["ReCheckUser"].ToString();
                this.txtReCheckDate.Text = ToYMD(dt.Rows[0]["ReCheckDate"]);
                this.txtClose.Text = dt.Rows[0]["Closer"].ToString();
                this.txtCloseDate.Text = ToYMD(dt.Rows[0]["CloseDate"]);
                this.txtTechRequery.Text = dt.Rows[0]["TechRequery"].ToString();
                this.txtOrderFunction.Text = dt.Rows[0]["OrderFunction"].ToString();
                this.txtPackRequery.Text = dt.Rows[0]["PackRequery"].ToString();
                this.txtOrderParts.Text = dt.Rows[0]["OrderParts"].ToString();

                if (this.txtCheckUser.Text != "")
                    this.btnCheck.Text = "一审反审";
                else
                    this.btnCheck.Text = "一审审核";
                if (this.txtReCheckUser.Text != "")
                    this.btnReCheck.Text = "二审反审";
                else
                    this.btnReCheck.Text = "二审审核";
                if (this.txtClose.Text.Trim() == "")
                    this.btnClose.Text = "关闭采购单";
                else
                    this.btnClose.Text = "重启采购单";

              
                BindDataSub();
               
            }
            SetPermission();
            ScriptManager.RegisterStartupScript(this.updatePanel, this.GetType(), "Resize", "content_resize();", true);
        }
        /// <summary>
        /// 設定權限
        /// </summary>
        private void SetPermission()
        {

            bool blnDelete = false;
            bool blnEdit = false;
            bool blnCheck = false;
            bool blnReCheck = false;
            bool blnClose = false;
            DataTable dtOP = (DataTable)(Session["DT_UserOperation"]);
            DataRow[] drs = dtOP.Select(string.Format("SubModuleCode='{0}'", Session["SubModuleCode"].ToString()));
            
            foreach (DataRow dr in drs)
            {
                int op = int.Parse(dr["OperatorCode"].ToString());
                switch (op)
                {
                    case 1:
                        blnDelete = true;
                        break;
                    case 2: //修改
                        blnEdit = true;
                        break;
                    case 5: //审核
                        blnCheck = true;
                        break;
                    case 6: //二审
                        blnReCheck = true;
                        break;
                    case 7: //关闭
                        blnClose = true;
                        break;
                }
            }
          
             
            
            if (this.txtClose.Text != "")
            {
                this.btnDelete.Enabled = false;
                this.btnEdit.Enabled = false;
                this.btnCheck.Enabled = false;
                this.btnReCheck.Enabled = false;
            }
            else
            {
                if (this.txtReCheckUser.Text != "")
                {
                    this.btnDelete.Enabled = false;
                    this.btnEdit.Enabled = false;
                    this.btnCheck.Enabled = false;
                    this.btnReCheck.Enabled =blnReCheck;
                    this.btnClose.Enabled = blnClose;
                    if (bll.GetRowCount("Vused_WMS_ScheduleSub", string.Format("Flag=1 and ScheduleNo='{0}'", this.txtID.Text)) > 0)
                        this.btnReCheck.Enabled = false;
                    
                    
                }
                else
                {
                    if (this.txtCheckUser.Text != "")
                    {
                        this.btnDelete.Enabled = false;
                        this.btnEdit.Enabled = false;
                        this.btnCheck.Enabled = blnCheck;
                        this.btnReCheck.Enabled = blnReCheck;
                        this.btnClose.Enabled = false;

                    }
                    else
                    {
                        this.btnDelete.Enabled = blnDelete;
                        this.btnEdit.Enabled =blnEdit;
                        this.btnCheck.Enabled = blnCheck;
                        this.btnReCheck.Enabled = false;
                        this.btnClose.Enabled = false;
                    }
                }
            }
            if(bll.GetRowCount("Vused_WMS_SCHEDULE", string.Format("Flag=1 and ScheduleNo='{0}'", this.txtID.Text)) > 0)
            {
                this.btnDelete.Enabled = false;
                this.btnEdit.Enabled = false;
            }
        }



        private void BindDataSub()
        {

            DataTable dt = bll.FillDataTable("WMS.SelectScheduleSub", new DataParameter[] { new DataParameter("{0}", string.Format("ScheduleNo='{0}' and Flag={1}", this.txtID.Text, Flag)) });
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
            SetPageSubBtnEnabled();

        }
        
        protected void dgViewSub1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                if (e.Row.Cells[0].Text == "True")
                {
                    e.Row.Cells[0].Text = "已完成";
                }
                else
                {
                    e.Row.Cells[0].Text = "未完成";
                }
                if (e.Row.Cells[24].Text == "True")
                {
                    e.Row.Cells[24].Text = "是";
                }
                else
                {
                    e.Row.Cells[24].Text = "否";
                }
                
            }
        }
        
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string strID = this.txtID.Text;
            string[] comds = new string[2];
            comds[0] = "WMS.DeleteSchedule";
            comds[1] = "WMS.DeleteScheduleSub";
            List<DataParameter[]> paras = new List<DataParameter[]>();
            paras.Add(new DataParameter[] { new DataParameter("{0}", string.Format("ScheduleNo='{0}' and Flag={1}", strID, Flag)) });
            paras.Add(new DataParameter[] { new DataParameter("{0}", string.Format("ScheduleNo='{0}' and Flag={1}", strID, Flag)) });
            bll.ExecTran(comds, paras);
            AddOperateLog("采购订单", "删除单号：" + strID);
            btnNext_Click(sender, e);
            if (this.txtID.Text == strID)
                btnPre_Click(sender, e);
        }

        #region ButtonClick
        protected void btnCheck_Click(object sender, EventArgs e)
        {
            
            DataParameter[] paras = new DataParameter[3];
            if (this.btnCheck.Text == "一审审核")
            {
                paras[0] = new DataParameter("@CheckUser", Session["EmployeeCode"].ToString());
                paras[1] = new DataParameter("{1}", "getdate()");
            }
            else
            {
                paras[0] = new DataParameter("@CheckUser", "");
                paras[1] = new DataParameter("{1}", null);
            }
            paras[2] = new DataParameter("{0}", string.Format("ScheduleNo='{0}' and Flag={1}", this.txtID.Text, Flag));
           

            bll.ExecNonQuery("WMS.UpdateScheduleCheck", paras);
            AddOperateLog("采购订单", btnCheck.Text + ":" + this.txtID.Text);
            DataTable dt = bll.FillDataTable("WMS.SelectSchedule", new DataParameter[] { new DataParameter("{0}", string.Format("{0}='{1}' and Flag={2}", PrimaryKey, this.txtID.Text, Flag)) });
            BindData(dt);
        }
        protected void btnReCheck_Click(object sender, EventArgs e)
        {
            DataParameter[] paras = new DataParameter[3];
            
            string[] comds = new string[2];

            List<DataParameter[]> Lparas = new List<DataParameter[]>();
            if (this.btnReCheck.Text == "二审审核")
            {
                paras[0] = new DataParameter("@ReCheckUser", Session["EmployeeCode"].ToString());
                paras[1] = new DataParameter("{1}", "getdate()");

                comds[0] = "WMS.UpdateScheduleReCheck";
                comds[1] = "WMS.SP_CreateScheduleBarCode";

            }
            else
            {
                paras[0] = new DataParameter("@ReCheckUser", "");
                paras[1] = new DataParameter("{1}", null);

                comds[0] = "WMS.UpdateScheduleReCheck";
                comds[1] = "WMS.DeleteScheduleBarCode";
            }
            paras[2] = new DataParameter("{0}", string.Format("ScheduleNo='{0}' and Flag={1}", this.txtID.Text, Flag));
            DataParameter[] paras2 = new DataParameter[] { new DataParameter("@Flag", Flag), new DataParameter("@ScheduleNo", this.txtID.Text) };
            Lparas.Add(paras);
            Lparas.Add(paras2);

            try
            {

                bll.ExecTran(comds, Lparas);

            }
            catch (Exception ex)
            {
                WMS.App_Code.JScript.Instance.ShowMessage(this.updatePanel, ex.Message);
            }

            AddOperateLog("采购订单", btnReCheck.Text+":" +this.txtID.Text);


            DataTable dt = bll.FillDataTable("WMS.SelectSchedule", new DataParameter[] { new DataParameter("{0}", string.Format("{0}='{1}' and Flag={2}", PrimaryKey, this.txtID.Text, Flag)) });
            BindData(dt);

        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            if (this.btnClose.Text == "关闭采购单")
            {
                DataParameter[] paras = new DataParameter[3];

                paras[0] = new DataParameter("@Closer", Session["EmployeeCode"].ToString());


                paras[1] = new DataParameter("{0}", string.Format("ScheduleNo='{0}' and Flag={1}", this.txtID.Text, Flag));
                paras[2] = new DataParameter("{1}", "getdate()");

                string[] cmds = new string[] { "WMS.UpdateScheduleClose", "WMS.UpdateScheduleBarCodeClose" };
                List<DataParameter[]> paraList = new List<DataParameter[]>();
                paraList.Add(paras);
                paraList.Add(new DataParameter[] {new DataParameter("{1}", 2), new DataParameter("{0}", string.Format("barcode.ScheduleNo='{0}' and barcode.Flag={1} and barcode.State=0 ", this.txtID.Text, Flag)) });

                bll.ExecTran(cmds, paraList);

                //bll.ExecNonQuery("WMS.UpdateScheduleClose", paras);
               
            }
            else
            {
                DataParameter[] paras = new DataParameter[3];

                paras[0] = new DataParameter("@Closer", "");


                paras[1] = new DataParameter("{0}", string.Format("ScheduleNo='{0}' and Flag={1}", this.txtID.Text, Flag));
                paras[2] = new DataParameter("{1}", "null");

                string[] cmds = new string[] { "WMS.UpdateScheduleClose", "WMS.UpdateScheduleBarCodeClose" };
                List<DataParameter[]> paraList = new List<DataParameter[]>();
                paraList.Add(paras);
                paraList.Add(new DataParameter[] {new DataParameter("{1}", 0), new DataParameter("{0}", string.Format("barcode.ScheduleNo='{0}' and barcode.Flag={1}  and barcode.State=2", this.txtID.Text, Flag)) });

                bll.ExecTran(cmds, paraList);
            }
            AddOperateLog("采购订单", btnClose.Text + " " + this.txtID.Text);
            DataTable dt = bll.FillDataTable("WMS.SelectSchedule", new DataParameter[] { new DataParameter("{0}", string.Format("{0}='{1}' and Flag={2}", PrimaryKey, this.txtID.Text, Flag)) });
            BindData(dt);

        }
        #endregion

        #region 上下笔事件

        protected void btnFirst_Click(object sender, EventArgs e)
        {
            BindData(bll.GetRecord("F", TableName, Filter, PrimaryKey, this.txtID.Text));
        }
        protected void btnPre_Click(object sender, EventArgs e)
        {
            BindData(bll.GetRecord("P", TableName, Filter, PrimaryKey, this.txtID.Text));
        }
        protected void btnNext_Click(object sender, EventArgs e)
        {
            BindData(bll.GetRecord("N", TableName, Filter, PrimaryKey, this.txtID.Text));
        }
        protected void btnLast_Click(object sender, EventArgs e)
        {
            BindData(bll.GetRecord("L", TableName, Filter, PrimaryKey, this.txtID.Text));
        }
        #endregion

        #region 从表资料显示
      

        private void SetPageSubBtnEnabled()
        {
            DataTable dt = (DataTable)Session[FormID + "_View_dgViewSub1"];
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
            SetPermission();
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
        }

        protected void ddlPageSizeSub1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            DataTable dt1 = (DataTable)Session[FormID + "_View_dgViewSub1"];
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
            DataTable dt1 = (DataTable)Session[FormID + "_View_" + dgv.ID];
            dgv.PageIndex = pindex;
            dgv.DataSource = dt1;
            dgv.DataBind();

            SetPageSubBtnEnabled();
            SetPermission();
            ScriptManager.RegisterStartupScript(this.updatePanel, this.GetType(), "Resize", "content_resize();", true);
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

        protected void btnPrint_Click(object sender, EventArgs e)
        {

            System.Collections.ArrayList al = new System.Collections.ArrayList();

            string Path = System.AppDomain.CurrentDomain.BaseDirectory + @"ExcelLoad\";
            //string Path = HttpRuntime.BinDirectory + @"ExcelLoad\";

            DirectoryInfo di = new DirectoryInfo(Path);
            foreach (FileInfo fi in di.GetFiles())
            {
                if (fi.Name.ToLower().IndexOf("stock") < 0)
                {
                    TimeSpan ts = DateTime.Now - fi.CreationTime;
                    if (ts.Hours >= 2)
                    {
                        fi.Delete();
                    }
                }
            }

            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();

            Microsoft.Office.Interop.Excel.Workbook wb = excel.Workbooks._Open(Path + "InStockSchedule.xls", Missing.Value, Missing.Value, Missing.Value, Missing.Value
                    , Missing.Value, Missing.Value, Missing.Value, Missing.Value
                    , Missing.Value, Missing.Value, Missing.Value, Missing.Value);

            Microsoft.Office.Interop.Excel.Worksheet wsheet = wb.Sheets[1];
            ((Microsoft.Office.Interop.Excel._Worksheet)wb.Sheets[1]).Activate();
            Microsoft.Office.Interop.Excel.Range excelRange;

            excel.Cells[3, 7] = this.txtBillDate.Text;
            excel.Cells[4, 3] = this.txtID.Text;
            excel.Cells[4, 7] = this.txtFactoryName.Text;
           
           
           

           int  k = 8;
           DataTable dtView = (DataTable)Session[FormID + "_View_dgViewSub1"];


            for (int i = 0; i < dtView.Rows.Count; i++)
            {
                if (i > 0)
                {
                    Microsoft.Office.Interop.Excel.Range xlsRow = (Microsoft.Office.Interop.Excel.Range)wsheet.Rows[k, Missing.Value];
                    xlsRow.Insert(Microsoft.Office.Interop.Excel.XlDirection.xlUp, Missing.Value);
                    //((Microsoft.Office.Interop.Excel.Range)wsheet.Range[excel.Cells[k, 2], excel.Cells[k, 3]]).Merge();
                    //((Microsoft.Office.Interop.Excel.Range)excel.Cells[k, 2]).HorizontalAlignment =Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                    ((Microsoft.Office.Interop.Excel.Range)wsheet.Range[excel.Cells[k, 1], excel.Cells[k, 9]]).Borders.LineStyle = 1;
                    //((Microsoft.Office.Interop.Excel.Range)excel.Cells[k - 1, 7]).Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight).LineStyle = 1;
                }

                excel.Cells[k, 1] = dtView.Rows[i]["RowID"].ToString();
                excel.Cells[k, 2] = dtView.Rows[i]["ProductName"].ToString();
                excel.Cells[k, 3] = dtView.Rows[i]["ProductModel"].ToString() ;
                excel.Cells[k, 4] = dtView.Rows[i]["ProductFModel"].ToString();
                excel.Cells[k, 5] = dtView.Rows[i]["ColorName"].ToString();
                excel.Cells[k, 6] = dtView.Rows[i]["PlanQty"];
                excel.Cells[k, 7] = dtView.Rows[i]["Price"];
                excel.Cells[k, 8] = dtView.Rows[i]["Amount"];
                excel.Cells[k, 9] = ToYMD(dtView.Rows[i]["PlanDate"]);
                k++;
            }

            //求和计算
            if (dtView.Rows.Count > 1)
            {
                excelRange = (Microsoft.Office.Interop.Excel.Range)excel.Cells[k, 6];
                excelRange.Formula = "=SUM(F8:F" + (k - 1).ToString() + ")";
                excelRange.Calculate();

                excelRange = (Microsoft.Office.Interop.Excel.Range)excel.Cells[k, 8];
                excelRange.Formula = "=SUM(H8:H" + (k - 1).ToString() + ")";
                excelRange.Calculate();

            }

            excel.Cells[k + 1, 2] = this.txtTechRequery.Text;//工艺要求
            excel.Cells[k + 2,2] = this.txtOrderFunction.Text;//功能

            string section = "";
            for (int i = 0; i < dtView.Rows.Count; i++)
            {
                if (dtView.Rows[i]["StarNo"].ToString().Length > 0)
                {
                    if (section.Length > 0)
                        section += Environment.NewLine;
                    section += dtView.Rows[i]["RowID"].ToString() + "):" + dtView.Rows[i]["ColorName"].ToString() + " ";


                    DataTable dtProduct = bll.FillDataTable("CMD.SelectProduct", new DataParameter[] { new DataParameter("{0}", "SUBSTRING(PRODUCT_CODE,1,5)='" + dtView.Rows[i]["ProductID"].ToString() + "'") });
                    if (dtProduct.Rows.Count > 0)
                    {
                        for (int Row = 0; Row < dtProduct.Rows.Count; Row++)
                        {
                            string[] Parts = dtProduct.Rows[Row]["PRODUCT_PartsName"].ToString().Split('-');
                            string PartsName = "";
                            if (Parts.Length > 1)
                                PartsName = Parts[1];
                            section += PartsName + ":";

                            section += dtView.Rows[i]["ProductID"].ToString() + dtView.Rows[i]["ColorID"].ToString() + dtProduct.Rows[Row]["PRODUCT_CODE"].ToString().Substring(5, 2) + dtView.Rows[i]["StarNo"].ToString();
                            section += "～" + int.Parse(dtView.Rows[i]["EndNo"].ToString().Substring(6, 5)).ToString() + "；";

                        }

                    }
                }
            }

            excel.Cells[k + 3, 2] = section;//序号


          


            excel.Cells[k + 4, 2] = this.txtPackRequery.Text;//包装要求
            excel.Cells[k + 5, 2] = this.txtOrderParts.Text;//免费随货配件
            excel.Cells[k + 6, 2] = this.txtMemo.Text;//备注

            excel.Cells[k + 8, 2] = this.txtReCheckUser.Text;//备注
            excel.Cells[k + 8, 5] = this.txtCheckUser.Text;//审核
            excel.Cells[k + 8, 8] = this.txtCreator.Text;//制单
            excel.Visible = false;


            string fileName = "采购订单" + this.txtID.Text;
            string path = Path + fileName + ".xls";

            wb.SaveCopyAs(path);
            wb.Close(false, null, null);

            excel.Quit();


            System.Runtime.InteropServices.Marshal.ReleaseComObject(wb);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(wsheet);
            wb = null;
            excel = null;
            wsheet = null;

            System.IO.FileInfo file = new System.IO.FileInfo(path);
            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.Charset = "UTF-8";
            System.Web.HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
            // 添加头信息，为"文件下载/另存为"对话框指定默认文件名 
            System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + System.Web.HttpContext.Current.Server.UrlEncode(file.Name));
            // 添加头信息，指定文件大小，让浏览器能够显示下载进度 
            System.Web.HttpContext.Current.Response.AddHeader("Content-Length", file.Length.ToString());

            // 指定返回的是一个不能被客户端读取的流，必须被下载 
            System.Web.HttpContext.Current.Response.ContentType = "application/ms-excel";

            // 把文件流发送到客户端 
            System.Web.HttpContext.Current.Response.WriteFile(file.FullName);
            // 停止页面的执行 

            System.Web.HttpContext.Current.Response.End();
        }

       

       
    }
}