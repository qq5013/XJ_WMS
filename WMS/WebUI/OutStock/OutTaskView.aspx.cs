using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using IDAL;
using System.Drawing;
using System.IO;
using Microsoft.Office.Interop;
using System.Reflection;

namespace WMS.WebUI.OutStock
{
    public partial class OutTaskView : WMS.App_Code.BasePage
    {
        #region 参数设置
        int pageIndex = 1;
        int pageSize = 15;
        int totalCount = 0;
        int pageCount = 0;
        protected string FormID = "Deliver";
        protected string TableName = "View_WMS_OutStock";
        protected string PrimaryKey = "BillID";

        int Flag = 2;
        
        string filter = "1=1";
        DataTable dtGroup;
      

        BLL.BLLBase bll = new BLL.BLLBase();
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["sys_PageCount"] != null)
                {
                    pageSize = Convert.ToInt32(Session["sys_PageCount"].ToString());
                    ViewState["PageSize"] = pageSize;
                }
                if (!IsPostBack)
                {
                    ViewState["OrderField"] = "";
                    ViewState["filter"] = "1=1";
                    ViewState["CurrentPage"] = 1;
                    BindPageSize();
                    writeJsvar(FormID,SqlCmd, "");
                    SetBtnEnabled("");
                }
               

            }
            catch (Exception exp)
            {
                WMS.App_Code.JScript.Instance.ShowMessage(this.UpdatePanel1, exp.Message);
            }
        }
       
      


        protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (ViewState["OrderField"].ToString() != "")
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    foreach (TableCell myheader in e.Row.Cells)
                    {
                        if (myheader.HasControls())
                        {
                            LinkButton button = myheader.Controls[0] as LinkButton;
                            string OrderField = ViewState["OrderField"].ToString();
                            OrderField = OrderField.Replace(" Asc", "");
                            OrderField = OrderField.Replace(" Desc", "").Trim();
                            if (button != null)
                                if (button.CommandArgument.Equals(OrderField))//如果有排序
                                {
                                    if (GridViewSortDirection == SortDirection.Ascending)//按照asc顺序加入箭头
                                        myheader.Controls.Add(new LiteralControl("▲"));
                                    else
                                        myheader.Controls.Add(new LiteralControl("▼"));
                                }
                        }
                    }
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                

              
            }

        }
        protected void dgViewSub1_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        #region 翻页處理 +查询按钮+笔数切换
        protected void btnFirst_Click(object sender, System.EventArgs e)
        {
            SetBtnEnabled("F");
        }

        protected void btnLast_Click(object sender, System.EventArgs e)
        {
            SetBtnEnabled("L");
        }

        protected void btnPre_Click(object sender, System.EventArgs e)
        {
            SetBtnEnabled("P");
        }

        protected void btnNext_Click(object sender, System.EventArgs e)
        {
            SetBtnEnabled("N");
        }
        protected void btnToPage_Click(object sender, System.EventArgs e)
        {
            int PageIndex = 0;
            int.TryParse(this.txtPageNo.Text, out PageIndex);
            if (PageIndex == 0)
                PageIndex = 1;

            ViewState["CurrentPage"] = PageIndex;
            SetBtnEnabled("");
        }

        private void SetBtnEnabled(string movePage)
        {
            switch (movePage)
            {
                case "F":
                    ViewState["CurrentPage"] = 1;
                    break;
                case "P":
                    ViewState["CurrentPage"] = int.Parse(ViewState["CurrentPage"].ToString()) - 1;
                    break;
                case "N":
                    ViewState["CurrentPage"] = int.Parse(ViewState["CurrentPage"].ToString()) + 1;
                    break;
                case "L":
                    ViewState["CurrentPage"] = 0;
                    break;
                default:
                    if (ViewState["CurrentPage"] == null)
                        ViewState["CurrentPage"] = 1;
                    break;
            }

            BindGrid(int.Parse(ViewState["CurrentPage"].ToString()));
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "Resize", "content_resize();", true);
        }

        protected void btnSearch_Click(object sender, System.EventArgs e)
        {

            try
            {
                if (this.ddlField.SelectedValue.IndexOf("Date") > 0)
                    filter = string.Format("CONVERT(nvarchar(10),{0},111) like '%{1}%'", this.ddlField.SelectedValue, this.txtSearch.Text.Trim().Replace("'", ""));
                else

                    filter = string.Format("{0} like '%{1}%'", this.ddlField.SelectedValue, this.txtSearch.Text.Trim().Replace("'", ""));
                filter += " and Flag=" + Flag;
                ViewState["filter"] = filter;

                ViewState["CurrentPage"] = 1;
                BindGrid(pageIndex);

            }
            catch (Exception exp)
            {
                WMS.App_Code.JScript.Instance.ShowMessage(this.UpdatePanel1, exp.Message);
            }
        }
        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["PageSize"] = this.ddlPageSize.SelectedValue;
            SetBtnEnabled("");
        }
        /// <summary>
        /// 浏览界面每页笔数
        /// </summary>
        private void BindPageSize()
        {
            this.ddlPageSize.Items.Add(new ListItem("15", "15"));
            this.ddlPageSize.Items.Add(new ListItem("20", "20"));
            this.ddlPageSize.Items.Add(new ListItem("25", "25"));
            this.ddlPageSize.Items.Add(new ListItem("30", "30"));
            this.ddlPageSize.Items.Add(new ListItem("40", "40"));
            this.ddlPageSize.Items.Add(new ListItem("50", "50"));
 
        }

        /// <summary>
        /// 綁定GirdView
        /// </summary>
        /// <param name="pageIndex"></param>
        private void BindGrid(int pageIndex)
        {
            dtGroup = bll.GetDataPage("WMS.SelectTaskTemp", int.Parse(ViewState["CurrentPage"].ToString()), int.Parse(ViewState["PageSize"].ToString()), out totalCount,out pageCount, new DataParameter[] { new DataParameter("{0}", ViewState["filter"].ToString()) });
            pageCount = GetPageCount(totalCount, pageSize);
            if (ViewState["CurrentPage"].ToString() == "0" || int.Parse(ViewState["CurrentPage"].ToString()) > pageCount)
                ViewState["CurrentPage"] = pageCount;

            if (dtGroup.Rows.Count == 0)
            {
                dtGroup.Rows.Add(dtGroup.NewRow());
                GridView1.DataSource = dtGroup;
                GridView1.DataBind();
                int columnCount = GridView1.Rows[0].Cells.Count;
                GridView1.Rows[0].Cells.Clear();
                GridView1.Rows[0].Cells.Add(new TableCell());
                GridView1.Rows[0].Cells[0].ColumnSpan = columnCount;
                GridView1.Rows[0].Cells[0].Text = "没有符合以上条件的数据,请重新查询 ";
                GridView1.Rows[0].Visible = true;

                this.btnFirst.Enabled = false;
                this.btnPre.Enabled = false;
                this.btnNext.Enabled = false;
                this.btnLast.Enabled = false;
                this.btnToPage.Enabled = false;
                lblCurrentPage.Visible = false;
                

            }
            else
            {
                this.GridView1.DataSource = dtGroup;
                this.GridView1.DataBind();

                this.btnLast.Enabled = true;
                this.btnFirst.Enabled = true;
                this.btnToPage.Enabled = true;

                if (int.Parse(ViewState["CurrentPage"].ToString()) > 1)
                    this.btnPre.Enabled = true;
                else
                    this.btnPre.Enabled = false;

                if (int.Parse(ViewState["CurrentPage"].ToString()) < pageCount)
                    this.btnNext.Enabled = true;
                else
                    this.btnNext.Enabled = false;

                lblCurrentPage.Visible = true;
                lblCurrentPage.Text = "共 [" + totalCount.ToString() + "] 笔记录  第 [" + ViewState["CurrentPage"] + "] 页  共 [" + pageCount.ToString() + "] 页";

               
               
            }
        }

  
        

        #endregion

        #region DataGrid排序设定 
       
        /// <summary>
        /// 排序方向屬性
        /// </summary>
        public SortDirection GridViewSortDirection
        {
            get
            {
                if (ViewState["sortDirection"] == null)
                    ViewState["sortDirection"] = SortDirection.Ascending;
                return (SortDirection)ViewState["sortDirection"];
            }
            set
            {
                ViewState["sortDirection"] = value;
            }
        }
        #endregion

        protected void btnPrint_Click(object sender, EventArgs e)
        {
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

            Microsoft.Office.Interop.Excel.Workbook wb = excel.Workbooks._Open(Path + "StockTaskTmp.xls", Missing.Value, Missing.Value, Missing.Value, Missing.Value
                    , Missing.Value, Missing.Value, Missing.Value, Missing.Value
                    , Missing.Value, Missing.Value, Missing.Value, Missing.Value);
            Microsoft.Office.Interop.Excel.Worksheet wsheet = null;

           

             wsheet = wb.Sheets[1];
            ((Microsoft.Office.Interop.Excel._Worksheet)wb.Sheets[1]).Activate();
            DataTable dtView = bll.FillDataTable("WMS.SelectTaskTemp", null);
            int k = 2;
            for (int i = 0; i < dtView.Rows.Count; i++)
            {
                excel.Cells[k, 1] = dtView.Rows[i]["AREA_NAME"].ToString();//货区
                excel.Cells[k, 2] = dtView.Rows[i]["SHELF_NAME"].ToString();//货架
                excel.Cells[k, 3] = dtView.Rows[i]["CELL_NAME"].ToString();
                excel.Cells[k, 4] = dtView.Rows[i]["PRODUCT_NAME"].ToString();
                excel.Cells[k, 5] = dtView.Rows[i]["PRODUCT_MODEL"].ToString();
                excel.Cells[k, 6] = dtView.Rows[i]["COLOR_NAME"].ToString();
                excel.Cells[k, 7] = dtView.Rows[i]["PalletCode"].ToString();
                excel.Cells[k, 8] = dtView.Rows[i]["Qty"];
                k++;
            }
           
            Microsoft.Office.Interop.Excel.Range excelRange = wsheet.Range[excel.Cells[k, 1], excel.Cells[k, 8]];
            excelRange.Merge();
            excelRange.Borders.LineStyle = 1;

            //出库明细
            k++;
            DataTable dtTaskID = dtView.DefaultView.ToTable(true, new string[] { "TaskID" });
            for (int j = 0; j < dtTaskID.Rows.Count; j++)
            {
                DataTable dtStockID = bll.FillDataTable("WMS.SelectStockByTaskID", new DataParameter[] { new DataParameter("@TaskID", dtTaskID.Rows[j][0].ToString()) });
                for (int h = 0; h < dtStockID.Rows.Count; h++)
                {
                    DataTable dtDetail = bll.FillDataTable("WMS.SelectOutStockSubByBillID", new DataParameter[] { new DataParameter("@BillID", dtStockID.Rows[h][0].ToString()) });

                    if (dtDetail.Rows.Count > 0)
                    {
                        string strBillID = "销售订单：" + dtDetail.Rows[0]["BillID"].ToString() + "  经销商：" + dtDetail.Rows[0]["CustName"].ToString();
                         excelRange = wsheet.Range[excel.Cells[k, 1], excel.Cells[k, 8]];
                        excelRange.Merge();
                        excelRange.Value = strBillID;

                        k++;
                        excel.Cells[k, 1] = "序号";
                        excel.Cells[k, 2] = "产品名称";
                        excel.Cells[k, 3] = "产品型号";
                        excel.Cells[k, 4] = "规格";
                        excel.Cells[k, 5] = "数量";
                        k++;

                        for (int i = 0; i < dtDetail.Rows.Count; i++)
                        {
                            excel.Cells[k, 1] = i + 1;//货区
                            excel.Cells[k, 2] = dtDetail.Rows[i]["ProductName"].ToString();//货架
                            excel.Cells[k, 3] = dtDetail.Rows[i]["ProductModel"].ToString();
                            excel.Cells[k, 4] = dtDetail.Rows[i]["ColorName"].ToString();
                            excel.Cells[k, 5] = dtDetail.Rows[i]["InStockQty"].ToString();
                            k++;
                        }
                        k++;
                    }
                }
            }

            excel.Visible = false;
            string fileName = "出库备货单";
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