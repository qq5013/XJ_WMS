using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using IDAL;
using System.Reflection;
using System.Configuration;
using System.IO;

namespace WMS.WebUI.OutStock
{
    public partial class DeliverView : WMS.App_Code.BasePage
    {
        protected string FormID;
        protected string ID;
        protected string TableName = "View_WMS_OutStock";
        protected string PrimaryKey = "BillID";
        protected int Flag = 2;
        private string Filter = "Flag=2";

        BLL.BLLBase bll = new BLL.BLLBase();

        protected void Page_Load(object sender, EventArgs e)
        {
            ID = Request.QueryString["ID"] + "";
            FormID = Request.QueryString["FormID"] + "";
            if (!IsPostBack)
            {
                DataTable dt = bll.FillDataTable("WMS.SelectOutStock", new DataParameter[] { new DataParameter("{0}", string.Format("{0}='{1}' and Flag={2}", PrimaryKey, ID, Flag)) });
                BindData(dt);
                writeJsvar(FormID, TableName, PrimaryKey, ID);
                BindPageSize();
            }
        }

        private void BindData(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                this.txtID.Text = dt.Rows[0]["BillID"].ToString();
                this.txtBillDate.Text = ToYMD(dt.Rows[0]["BillDate"]);
                this.txtPlanDate.Text = ToYMD(dt.Rows[0]["PlanDate"]);
                this.txtScheduleNo.Text = dt.Rows[0]["ScheduleNo"].ToString();
                this.txtCustPerson.Text = dt.Rows[0]["CustPerson"].ToString();
                this.txtCustomerName.Text = dt.Rows[0]["CustName"].ToString();  
                this.txtLinkPerson.Text = dt.Rows[0]["LinkPerson"].ToString();
                this.txtLinkPhone.Text = dt.Rows[0]["LinkPhone"].ToString();
                this.txtLinkAddress.Text = dt.Rows[0]["LinkAddress"].ToString();
                this.ddlTransport.SelectedValue = dt.Rows[0]["Transport"].ToString();
                this.txtMemo.Text = dt.Rows[0]["Memo"].ToString();
                this.txtCreator.Text = dt.Rows[0]["Creator"].ToString();
                this.ddlDriverType.SelectedValue = dt.Rows[0]["DriverType"].ToString();
                this.txtCheckUser.Text = dt.Rows[0]["TaskChecker"].ToString();
                this.txtCheckDate.Text = ToYMD(dt.Rows[0]["TaskCheckDate"]);
                this.txtTotalQty.Text = dt.Rows[0]["TotalQty"].ToString();
                this.txtTotalVolume.Text = dt.Rows[0]["TotalVolume"].ToString();
                this.txtState.Text = dt.Rows[0]["State"].ToString();
                if (this.txtCheckUser.Text == "")
                {
                    this.btnCheck.Text = "审核";
                }
                else
                {
                    this.btnCheck.Text = "反审";
                }


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
                }
            }

            if (int.Parse(this.txtState.Text) >= 2)
            {
                this.btnDelete.Enabled = false;
                this.btnEdit.Enabled = false;
                this.btnCheck.Enabled = false;
            }
            else
            {

                if (this.txtCheckUser.Text != "")
                {
                    this.btnDelete.Enabled = false;
                    this.btnEdit.Enabled = false;
                }
                else
                {
                    this.btnDelete.Enabled = blnDelete;
                    this.btnEdit.Enabled = blnEdit;
                }
            }
        }



        private void BindDataSub()
        {

            DataTable dt = bll.FillDataTable("WMS.SelectOutStockSub", new DataParameter[] { new DataParameter("{0}", string.Format("BillID='{0}' and Flag={1}", this.txtID.Text, Flag)), new DataParameter("@BillID", "") });
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

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            int membercount = bll.GetRowCount("Vused_WMS_OutStock", string.Format("Flag=2 and BillID='{0}'", this.txtID.Text));
            if (membercount > 0)
            {
                WMS.App_Code.JScript.Instance.ShowMessage(this.updatePanel, txtID.Text + "已经被使用，不能删除！");
                return;
            }
            string strID = this.txtID.Text;
            string[] comds = new string[2];
            comds[0] = "WMS.DeleteOutStock";
            comds[1] = "WMS.DeleteOutStockSub";
            List<DataParameter[]> paras = new List<DataParameter[]>();
            paras.Add(new DataParameter[] { new DataParameter("{0}", string.Format("BillID='{0}' and Flag={1}", strID, Flag)) });
            paras.Add(new DataParameter[] { new DataParameter("{0}", string.Format("BillID='{0}' and Flag={1}", strID, Flag)) });
            bll.ExecTran(comds, paras);
            AddOperateLog("出库单", "删除单号：" + strID);
            btnNext_Click(sender, e);
            if (this.txtID.Text == strID)
                btnPre_Click(sender, e);
        }

        #region ButtonClick
        protected void btnCheck_Click(object sender, EventArgs e)
        {
            if (this.btnCheck.Text == "反审")
            {
                int membercount = bll.GetRowCount("VUsed_OutStockDeliver", string.Format("Flag=2 and BillID='{0}'", this.txtID.Text));
                if (membercount > 0)
                {
                    WMS.App_Code.JScript.Instance.ShowMessage(this.updatePanel, txtID.Text + "已经被使用，不能删除！");
                    return;
                }
              
            }

            
            DataParameter[] paras = new DataParameter[4];
            if (this.btnCheck.Text == "审核")
            {
                paras[0] = new DataParameter("@TaskChecker", Session["EmployeeCode"].ToString());
                paras[1] = new DataParameter("{1}", "getdate()");
                paras[2] = new DataParameter("{2}", "1");
            }
            else
            {
                paras[0] = new DataParameter("@TaskChecker", "");
                paras[1] = new DataParameter("{1}", null);
                paras[2] = new DataParameter("{2}", "0");
            }
            paras[3] = new DataParameter("{0}", string.Format("BillID='{0}' and Flag={1}", this.txtID.Text, Flag));
          

            bll.ExecNonQuery("WMS.UpdateOutStockTaskCheck", paras);
            AddOperateLog("出库单作业审核", btnCheck.Text);
            DataTable dt = bll.FillDataTable("WMS.SelectOutStock", new DataParameter[] { new DataParameter("{0}", string.Format("{0}='{1}' and Flag={2}", PrimaryKey, this.txtID.Text, Flag)) });
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

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            int membercount = bll.GetRowCount("VUsed_OutStockDeliver", string.Format("Flag=2 and BillID='{0}'", this.txtID.Text));
            if (membercount > 0)
            {
                WMS.App_Code.JScript.Instance.ShowMessage(this.updatePanel, txtID.Text + "已经被使用，不能删除！");
                return;
            }

            ScriptManager.RegisterStartupScript(this.updatePanel, this.updatePanel.GetType(), "", "ViewEdit();", true);
        }

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

            Microsoft.Office.Interop.Excel.Workbook wb = excel.Workbooks._Open(Path + "OutStockSend.xls", Missing.Value, Missing.Value, Missing.Value, Missing.Value
                    , Missing.Value, Missing.Value, Missing.Value, Missing.Value
                    , Missing.Value, Missing.Value, Missing.Value, Missing.Value);

            Microsoft.Office.Interop.Excel.Worksheet wsheet = wb.Sheets[1];
            ((Microsoft.Office.Interop.Excel._Worksheet)wb.Sheets[1]).Activate();




            Microsoft.Office.Interop.Excel.Range excelRange;

            DataTable dtView = bll.FillDataTable("WMS.SelectOutStockSubReport", new DataParameter[] { new DataParameter("@BillID", this.txtID.Text) });

            //发货明细
            wsheet = wb.Sheets[1];

            ((Microsoft.Office.Interop.Excel._Worksheet)wb.Sheets[1]).Activate();


            excel.Cells[7, 2] = this.txtBillDate.Text;
            excel.Cells[8, 2] = this.txtCustomerName.Text;
            excel.Cells[9, 2] = this.txtLinkPerson.Text;
            excel.Cells[10, 2] = this.txtLinkPhone.Text;
            excel.Cells[11, 2] = this.txtLinkAddress.Text;

           int k = 15;
            for (int i = 0; i < dtView.Rows.Count; i++)
            {
                if (i > 18)
                {
                    Microsoft.Office.Interop.Excel.Range xlsRow = (Microsoft.Office.Interop.Excel.Range)wsheet.Rows[k, Missing.Value];
                    xlsRow.Insert(Microsoft.Office.Interop.Excel.XlDirection.xlUp, Missing.Value);
                    ((Microsoft.Office.Interop.Excel.Range)excel.Cells[k - 1, 7]).Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight).LineStyle = 1;
                }

                excel.Cells[k, 1] = i + 1;
                excel.Cells[k, 2] = dtView.Rows[i]["ProdModel"].ToString() + "(" + dtView.Rows[i]["ProdFModel"].ToString() + ")";
                excel.Cells[k, 3] = dtView.Rows[i]["ColName"].ToString();
                int Qty = (int)dtView.Rows[i]["InStockQty"];
                int PackQty = (int)dtView.Rows[i]["PACK_QTY"];
                int PQty = 0;
                excel.Cells[k, 4] = dtView.Rows[i]["InStockQty"];

                //箱数，体积
                if (int.Parse(dtView.Rows[i]["SubCount"].ToString()) > 1)
                {
                    excel.Cells[k, 5] = Qty * (int)dtView.Rows[i]["SubCount"];
                    excel.Cells[k, 6] = Qty * double.Parse(dtView.Rows[i]["ProductVolume"].ToString());
                }
                else
                {
                    if (Qty % PackQty != 0)
                        PQty = Qty / PackQty + 1;
                    else
                        PQty = Qty / PackQty;

                    excel.Cells[k, 5] = PQty;
                    excel.Cells[k, 6] = PQty * double.Parse(dtView.Rows[i]["ProductVolume"].ToString());
                }

                if (i == 18)
                {
                    wsheet.Range[excel.Cells[k, 1], excel.Cells[k, 7]].Borders.LineStyle = 0;
                }
                if (i > 18 && i == dtView.Rows.Count - 1)
                {
                    ((Microsoft.Office.Interop.Excel.Range)wsheet.Range[excel.Cells[k, 1], excel.Cells[k, 7]]).Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom).LineStyle = 1;
                }
                k++;
            }

            //求和计算
            if (dtView.Rows.Count > 18)
            {
                excelRange = (Microsoft.Office.Interop.Excel.Range)excel.Cells[k, 4];
                excelRange.Formula = "=SUM(D15:D" + (k - 1).ToString() + ")";
                excelRange.Calculate();

                excelRange = (Microsoft.Office.Interop.Excel.Range)excel.Cells[k, 5];
                excelRange.Formula = "=SUM(E15:E" + (k - 1).ToString() + ")";
                excelRange.Calculate();

                excelRange = (Microsoft.Office.Interop.Excel.Range)excel.Cells[k, 6];
                excelRange.Formula = "=SUM(F15:F" + (k - 1).ToString() + ")";
                excelRange.Calculate();

                excel.Cells[k + 3, 2] = this.txtCreator.Text;
                excel.Cells[k + 3, 4] = this.txtCheckUser.Text;
              


            }
            else
            {
                excel.Cells[37, 2] = this.txtCreator.Text;
                excel.Cells[37, 4] = this.txtCheckUser.Text;
              
            }


            excel.Visible = false;
            string fileName = "发货通知单" + this.txtID.Text;
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