using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using IDAL;
using Microsoft.Office.Interop;
using System.Reflection;
using System.IO;


namespace WMS.WebUI.InStock
{
    public partial class InStockView : WMS.App_Code.BasePage
    {
        protected string FormID;
        protected string ID;
        protected string TableName = "View_WMS_InStock";
        protected string PrimaryKey = "BillID";
        protected int Flag = 1;
        private string Filter = "Flag=1";

        BLL.BLLBase bll = new BLL.BLLBase();

        protected void Page_Load(object sender, EventArgs e)
        {
            ID = Request.QueryString["ID"] + "";
            FormID = Request.QueryString["FormID"] + "";
            if (!IsPostBack)
            {
                DataTable dt = bll.FillDataTable("WMS.SelectInStock", new DataParameter[] { new DataParameter("{0}", string.Format("{0}='{1}'", PrimaryKey, ID)) });
                BindData(dt);
                writeJsvar(FormID, TableName, PrimaryKey, ID);
                BindPageSize();
                HdfActiveTab.Value = "0";
            }
            else
            {
                
                ScriptManager.RegisterStartupScript(this.updatePanel, this.GetType(), "", "BindEvent()", true);
            }
        }

        private void BindData(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                this.txtID.Text = dt.Rows[0]["BillID"].ToString();
                this.txtBillDate.Text = ToYMD(dt.Rows[0]["BillDate"]);
                this.txtSourceNo.Text = dt.Rows[0]["ScheduleNo"].ToString();
                this.txtMemo.Text = dt.Rows[0]["Memo"].ToString();
                this.txtCreator.Text = dt.Rows[0]["Creator"].ToString();
                this.txtCreateDate.Text = ToYMD(dt.Rows[0]["CreateDate"]);
                this.txtTaskID.Text = dt.Rows[0]["TaskID"].ToString();
                this.txtChecker.Text = dt.Rows[0]["Checker"].ToString();
                this.txtCheckDate.Text = ToYMD(dt.Rows[0]["CheckDate"]);
                this.txtFactoryName.Text = dt.Rows[0]["FactoryName"].ToString();
                this.txtFactoryOrderNo.Text = dt.Rows[0]["FactoryOrderNo"].ToString();
                this.txtTotalQty.Text = dt.Rows[0]["TotalQty"].ToString();
                
               
                BindDataSub();
                BindDataSub2();
              
            }
            SetPermission();
            ScriptManager.RegisterStartupScript(this.updatePanel, this.GetType(), "Resize", "content_resize();", true);
        }

        private void BindDataSub()
        {
            DataTable dt = bll.FillDataTable("WMS.SelectInStockSub", new DataParameter[] { new DataParameter("{0}", string.Format("BillID='{0}' and Flag={1}", this.txtID.Text, Flag)) });
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
        private void BindDataSub2()
        {
            DataTable dt = bll.FillDataTable("WMS.SelectInStockDetail", new DataParameter[] { new DataParameter("{0}", string.Format("BillID='{0}' and Flag={1}", this.txtID.Text, Flag)) });
            Session[FormID + "_View_dgViewSub2"] = dt;

            if (dt.Rows.Count == 0)
            {
                DataTable dtNew = dt.Clone();
                dtNew.Rows.Add(dtNew.NewRow());
                dgViewSub2.DataSource = dtNew;
                dgViewSub2.DataBind();
                int columnCount = dgViewSub2.Rows[0].Cells.Count;
                dgViewSub2.Rows[0].Cells.Clear();
                dgViewSub2.Rows[0].Cells.Add(new TableCell());
                dgViewSub2.Rows[0].Cells[0].ColumnSpan = columnCount;
                dgViewSub2.Rows[0].Cells[0].Text = "没有查询到相应的资料！";
                dgViewSub2.Rows[0].Visible = true;
            }
            else
            {
                dgViewSub2.DataSource = dt;
                dgViewSub2.DataBind();
            }
            SetPageSub2BtnEnabled();

        }
        /// <summary>
        /// 設定權限
        /// </summary>
        private void SetPermission()
        {

            //bool blnDelete = false;
            bool blnEdit = false;
            bool blnCheck = false;
            
            DataTable dtOP = (DataTable)(Session["DT_UserOperation"]);
            DataRow[] drs = dtOP.Select(string.Format("SubModuleCode='{0}'", Session["SubModuleCode"].ToString()));

            foreach (DataRow dr in drs)
            {
                int op = int.Parse(dr["OperatorCode"].ToString());
                switch (op)
                {
                    //case 1:
                    //    blnDelete = true;
                    //    break;
                    case 2: //修改
                        blnEdit = true;
                        break;
                    case 5: //审核
                        blnCheck = true;
                        break;
                    
                }
            }



            if (this.txtChecker.Text != "")
            {
                //this.btnDelete.Enabled = false;
                this.btnEdit.Enabled = false;
                this.btnCheck.Enabled = false;
            }
            else
            {
                //this.btnDelete.Enabled = blnDelete;
                this.btnEdit.Enabled = blnEdit;
                this.btnCheck.Enabled = blnCheck;
               
            }

            //if (bll.GetRowCount("Vused_WMS_Stock", string.Format("Flag=1 and BillID='{0}'", this.txtID.Text)) > 0)
            //{
            //    this.btnDelete.Enabled = false;
                 
            //}
        }

        protected void dgViewSub1_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
        
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string strID = this.txtID.Text;
            string[] comds = new string[3];
            comds[0] = "WMS.DeleteInStock";
            comds[1] = "WMS.DeleteInStockSub";
            comds[2] = "WMS.DeleteInStockDetail";
            List<DataParameter[]> paras = new List<DataParameter[]>();
            paras.Add(new DataParameter[] { new DataParameter("{0}", string.Format("BillID='{0}' and Flag={1}", strID, Flag)) });
            paras.Add(new DataParameter[] { new DataParameter("{0}", string.Format("BillID='{0}' and Flag={1}", strID, Flag)) });
            paras.Add(new DataParameter[] { new DataParameter("{0}", string.Format("BillID='{0}' and Flag={1}", strID, Flag)) });
            bll.ExecTran(comds, paras);
            AddOperateLog("入库单", "删除单号：" + strID);
            btnNext_Click(sender, e);
            if (this.txtID.Text == strID)
                btnPre_Click(sender, e);
        }

        #region ButtonClick
        protected void btnCheck_Click(object sender, EventArgs e)
        {
            int StartToK3 = int.Parse(System.Configuration.ConfigurationManager.AppSettings["StartToK3"]);
            if (StartToK3 > 0)
            {
                DataTable dtK3Main = bll.FillDataTable("WebService.SelectInStockToK3Main", new DataParameter[] { new DataParameter("{0}", string.Format("stock.BillID='{0}'", this.txtID.Text)) });
                DataTable dtK3Sub = bll.FillDataTable("WebService.SelectInStockToK3Sub", new DataParameter[] { new DataParameter("{0}", string.Format("sub.BillID='{0}'", this.txtID.Text)) });

                dtK3Main.TableName = "Head";
                dtK3Sub.TableName = "Datalist";
                string strXML = "<?xml version=\"1.0\" encoding=\"GB2312\" standalone=\"yes\"?>" + Environment.NewLine;

                strXML += "<dataset>" + Environment.NewLine;
                if (dtK3Main.Rows.Count > 0)
                {
                    for (int i = 0; i < dtK3Main.Rows.Count; i++)
                    {
                        strXML += "<" + dtK3Main.TableName + ">" + Environment.NewLine; ;
                        for (int j = 0; j < dtK3Main.Columns.Count; j++)
                        {
                            strXML += "<" + dtK3Main.Columns[j].ColumnName + ">" + dtK3Main.Rows[i][dtK3Main.Columns[j].ColumnName].ToString() + "</" + dtK3Main.Columns[j].ColumnName + ">" + Environment.NewLine;

                        }
                        strXML += "</" + dtK3Main.TableName + ">" + Environment.NewLine; ;
                    }
                }
                if (dtK3Sub.Rows.Count > 0)
                {
                    for (int i = 0; i < dtK3Sub.Rows.Count; i++)
                    {
                        strXML += "<" + dtK3Sub.TableName + ">" + Environment.NewLine; ;
                        for (int j = 0; j < dtK3Sub.Columns.Count; j++)
                        {
                            strXML += "<" + dtK3Sub.Columns[j].ColumnName + ">" + dtK3Sub.Rows[i][dtK3Sub.Columns[j].ColumnName].ToString() + "</" + dtK3Sub.Columns[j].ColumnName + ">" + Environment.NewLine;

                        }
                        strXML += "</" + dtK3Sub.TableName + ">" + Environment.NewLine; ;
                    }
                }

                strXML += "</dataset>" + Environment.NewLine;

                BLL.BLLBase bllK3 = new BLL.BLLBase("K3DB");
                DataTable dtFilter = bllK3.FillDataTable("WebService.SpStockBillTOK3Filter", new DataParameter[] { new DataParameter("@xml", strXML) });
                if (dtFilter.Rows.Count > 0)
                {
                    if (int.Parse(dtFilter.Rows[0]["isok"].ToString()) < 0)
                    {
                        WMS.App_Code.JScript.Instance.ShowMessage(this.updatePanel, dtFilter.Rows[0]["msg"].ToString());
                        return;
                    }
                }


                try
                {
                    DataTable dtK3= bllK3.FillDataTable("WebService.SpStockBillTOK3", new DataParameter[] { new DataParameter("@xml", strXML) });
                    if (dtK3.Rows.Count > 0)
                    {
                        if (int.Parse(dtK3.Rows[0]["isok"].ToString()) < 0)
                        {
                            WMS.App_Code.JScript.Instance.ShowMessage(this.updatePanel, dtK3.Rows[0]["msg"].ToString());
                            return;
                        }
                    }
                }
                catch (Exception exK3)
                {
                    WMS.App_Code.JScript.Instance.ShowMessage(this.updatePanel, exK3.Message);
                    return;
                }


                DataParameter[] paras = new DataParameter[3];

                paras[0] = new DataParameter("@UserName", Session["EmployeeCode"].ToString());
                paras[1] = new DataParameter("@ScheduleNo", this.txtSourceNo.Text);

                paras[2] = new DataParameter("@BillID", this.txtID.Text);


                DataTable dtResult = bll.FillDataTable("WMS.UpdateInStockCheck", paras);
                if (dtResult.Rows[0]["Compeleted"].ToString() == "F")
                {
                    WMS.App_Code.JScript.Instance.ShowMessage(this.updatePanel, dtResult.Rows[0]["MessageT"].ToString());
                    return;

                }


            }
            else
            {
                DataParameter[] paras = new DataParameter[3];

                paras[0] = new DataParameter("@UserName", Session["EmployeeCode"].ToString());
                paras[1] = new DataParameter("@ScheduleNo", this.txtSourceNo.Text);

                paras[2] = new DataParameter("@BillID", this.txtID.Text);


                DataTable dtResult = bll.FillDataTable("WMS.UpdateInStockCheck", paras);
                if (dtResult.Rows[0]["Compeleted"].ToString() == "F")
                {
                    WMS.App_Code.JScript.Instance.ShowMessage(this.updatePanel, dtResult.Rows[0]["MessageT"].ToString());
                    return;

                }

            }
            AddOperateLog("采购订单", "审核");
            DataTable dt = bll.FillDataTable("WMS.SelectInStock", new DataParameter[] { new DataParameter("{0}", string.Format("{0}='{1}' and Flag={2}", PrimaryKey, this.txtID.Text, Flag)) });
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

        #region 从表资料绑定

        protected void ddlPageSizeSub1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            DataTable dt1 = (DataTable)Session[FormID + "_View_dgViewSub1"];
            this.dgViewSub1.DataSource = dt1;
            this.dgViewSub1.PageSize = int.Parse(ddlPageSizeSub1.Text);
            this.dgViewSub1.DataBind();
            SetPageSubBtnEnabled();
 
        }
        protected void ddlPageSizeSub2_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt1 = (DataTable)Session[FormID + "_View_dgViewSub2"];
            this.dgViewSub2.DataSource = dt1;
            this.dgViewSub2.PageSize = int.Parse(ddlPageSizeSub2.Text);
            this.dgViewSub2.DataBind();
            SetPageSub2BtnEnabled();
        }
        private void MovePage(GridView dgv, int pageindex)
        {
            int pindex = pageindex;
            if (pindex < 0) pindex = 0;
            if (pindex >= dgv.PageCount) pindex = dgv.PageCount - 1;
            DataTable dt1 = (DataTable)Session[FormID + "_View_" + dgv.ID];
            dgv.PageIndex = pindex;
            dgv.DataSource = dt1;
            dgv.DataBind();

            if (dgv.ID.IndexOf("1") > 1)
                SetPageSubBtnEnabled();
            else
                SetPageSub2BtnEnabled();
            SetPermission();
            ScriptManager.RegisterStartupScript(this.updatePanel, this.GetType(), "Resize", "content_resize();", true);

        }

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
        }
        private void SetPageSub2BtnEnabled()
        {
            DataTable dt = (DataTable)Session[FormID + "_View_dgViewSub2"];
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

        private void BindPageSize()
        {
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

        protected void btnFirstSub1_Click(object sender, EventArgs e)
        {
            HdfActiveTab.Value = "0";
            MovePage(this.dgViewSub1, 0);
        }

        protected void btnPreSub1_Click(object sender, EventArgs e)
        {
            HdfActiveTab.Value = "0";
            MovePage(this.dgViewSub1, this.dgViewSub1.PageIndex - 1);
        }

        protected void btnNextSub1_Click(object sender, EventArgs e)
        {
            HdfActiveTab.Value = "0";
            MovePage(this.dgViewSub1, this.dgViewSub1.PageIndex + 1);
        }

        protected void btnLastSub1_Click(object sender, EventArgs e)
        {
            HdfActiveTab.Value = "0";
            MovePage(this.dgViewSub1, this.dgViewSub1.PageCount - 1);
        }

        protected void btnToPageSub1_Click(object sender, EventArgs e)
        {
            HdfActiveTab.Value = "0";
            MovePage(this.dgViewSub1, int.Parse(this.txtPageNoSub1.Text) - 1);
        }

        protected void btnFirstSub2_Click(object sender, EventArgs e)
        {
            HdfActiveTab.Value = "1";
            MovePage(this.dgViewSub2, 0);
        }

        protected void btnPreSub2_Click(object sender, EventArgs e)
        {
            HdfActiveTab.Value = "1";
            MovePage(this.dgViewSub2, this.dgViewSub2.PageIndex - 1);
        }

        protected void btnNextSub2_Click(object sender, EventArgs e)
        {
            HdfActiveTab.Value = "1";
            MovePage(this.dgViewSub2, this.dgViewSub2.PageIndex + 1);
        }

        protected void btnLastSub2_Click(object sender, EventArgs e)
        {
            HdfActiveTab.Value = "1";
            MovePage(this.dgViewSub2, this.dgViewSub2.PageCount - 1);
        }

        protected void btnToPageSub2_Click(object sender, EventArgs e)
        {
            HdfActiveTab.Value = "1";
            MovePage(this.dgViewSub2, int.Parse(this.txtPageNoSub2.Text) - 1);
        }

        #endregion  

        protected void btnPrint_Click(object sender, EventArgs e)
        {
           
            string Path = System.AppDomain.CurrentDomain.BaseDirectory + @"ExcelLoad\";

           // ScriptManager.RegisterStartupScript(this.updatePanel, this.GetType(), "Resize", "alert(" + Path + ");", true);

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
            
            Microsoft.Office.Interop.Excel.Workbook wb = excel.Workbooks._Open(Path + "InStock.xls", Missing.Value, Missing.Value, Missing.Value, Missing.Value
                    , Missing.Value, Missing.Value, Missing.Value, Missing.Value
                    , Missing.Value, Missing.Value, Missing.Value, Missing.Value);
            Microsoft.Office.Interop.Excel.Worksheet wsheet = null;
            DataTable dtView = bll.FillDataTable("WMS.SelectInStockReport", new DataParameter[] { new DataParameter("@BillID", this.txtID.Text) });

            int index = dtView.Rows.Count / 5 + 1;
            if (index >= 2)
            {
                for (int j = index; j > 1; j--)
                {
                    wsheet = wb.Sheets[1];
                    wsheet.Copy(Missing.Value, wsheet);
                }
            }


            for (int j = 1; j <= index; j++)
            {
                 wsheet = wb.Sheets[j];
                ((Microsoft.Office.Interop.Excel._Worksheet)wb.Sheets[j]).Activate();
                wsheet.Name = "入库单"+j.ToString();

                excel.Cells[2, 2] = this.txtID.Text;
                excel.Cells[2, 5] = this.txtFactoryName.Text;
                excel.Cells[3, 8] = this.txtBillDate.Text;
                int k = 5;
                //写入明细

                for (int i = 0; i < 5; i++)
                {
                    int RowIndex = (j - 1) * 5 + i;
                    if (RowIndex >= dtView.Rows.Count)
                        break;
                    excel.Cells[k, 1] = dtView.Rows[RowIndex]["ProductNo"].ToString();
                    excel.Cells[k, 3] = dtView.Rows[RowIndex]["ProdName"].ToString();
                    excel.Cells[k, 4] = dtView.Rows[RowIndex]["ProdModel"].ToString() + "(" + dtView.Rows[RowIndex]["ProdFModel"].ToString() + ")  " + dtView.Rows[RowIndex]["ColName"].ToString();
                    excel.Cells[k, 6] = dtView.Rows[RowIndex]["Unit"].ToString();
                    excel.Cells[k, 7] = dtView.Rows[RowIndex]["InStockQty"];
                    k++;
                }
                //excel.Cells[11, 7] = this.txtChecker.Text;
                //excel.Cells[11, 9] = this.txtCreator.Text;


                excel.Visible = false;
            }
          
            string fileName = "入库单" + this.txtID.Text;
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