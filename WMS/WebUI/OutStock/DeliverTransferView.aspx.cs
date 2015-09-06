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
    public partial class DeliverTransferView : WMS.App_Code.BasePage
    {
        protected string FormID;
        protected string ID;
        protected string TableName = "WMS_Migration";
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
                BindOther();
                DataTable dt = bll.FillDataTable("WMS.SelectDeliverTransfer", new DataParameter[] { new DataParameter("{0}", string.Format("{0}='{1}'  and Flag={2}", PrimaryKey, ID, Flag)) });
                BindData(dt);
                writeJsvar(FormID,SqlCmd, ID);
                BindPageSize();
                this.HdfActiveTab.Value = "0";
               
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.updatePanel, this.GetType(), "", "BindEvent()", true);
            }
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
                this.txtBillDate.Text = ToYMD(dt.Rows[0]["BillDate"]);
                this.ddlStockFunction.SelectedValue = dt.Rows[0]["StockFunction"].ToString();
               
                this.txtMemo.Text = dt.Rows[0]["Memo"].ToString();
                this.txtCreator.Text = dt.Rows[0]["Creator"].ToString();
                this.txtCreateDate.Text = ToYMD(dt.Rows[0]["CreateDate"]);
                 
                this.txtTaskChecker.Text = dt.Rows[0]["TaskChecker"].ToString();
                this.txtTaskCheckDate.Text = ToYMD(dt.Rows[0]["TaskCheckDate"]);
                
                this.txtChecker.Text = dt.Rows[0]["Checker"].ToString();
                this.txtCheckDate.Text = ToYMD(dt.Rows[0]["CheckDate"]);

                if (this.txtTaskChecker.Text != "")
                    this.btnCheck.Text = "反审";
                else
                    this.btnCheck.Text = "审核";
                this.hdfState.Value = dt.Rows[0]["State"].ToString();

                
                BindDataSub();
                BindDataSub2();
              
            }
            SetPermission();
            ScriptManager.RegisterStartupScript(this.updatePanel, this.GetType(), "Resize", "content_resize();", true);
        }
        /// <summary>
        /// 設定權限
        /// </summary>
        private void SetPermission()
        {

            bool blnCheck = false;
            bool blnCancel = false;
           
            
            DataTable dtOP = (DataTable)(Session["DT_UserOperation"]);
            DataRow[] drs = dtOP.Select(string.Format("SubModuleCode='{0}'", Session["SubModuleCode"].ToString()));

            foreach (DataRow dr in drs)
            {
                int op = int.Parse(dr["OperatorCode"].ToString());
                switch (op)
                {
                    case 5:
                        blnCheck = true;
                        break;
                    case 7: //修改
                        blnCancel = true;
                        break;
                }
            }



            this.btnCheck.Enabled = blnCheck;
            if (int.Parse(this.hdfState.Value) >= 2)
                this.btnCheck.Enabled = false;
            if (this.txtTaskChecker.Text != "")
            {

                if (((DataTable)Session[FormID + "_View_dgViewSub2"]).Rows.Count > 0)
                {
                    this.btnClose.Enabled = blnCancel;
                    this.btnCheck.Enabled = false;
                    this.btnEdit.Enabled = false;
                    this.btnDelete.Enabled = false;
                }
                else
                {
                    this.btnClose.Enabled = false;
                }
            }
            else
            {
                this.btnClose.Enabled = false;
            }
             
        }



        private void BindDataSub()
        {

            DataTable dt = bll.FillDataTable("WMS.SelectDeliverTransferSub", new DataParameter[] { new DataParameter("{0}", string.Format("BillID='{0}' and Flag={1}", this.txtID.Text, Flag)),new DataParameter("@BillID",this.txtID.Text) });
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
            DataTable dt = bll.FillDataTable("WMS.SelectDeliverTransferDetail", new DataParameter[] { new DataParameter("{0}", string.Format("BillID='{0}' and Flag={1}", this.txtID.Text, Flag)) });
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
        protected void dgViewSub1_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
        
       

        #region ButtonClick
        protected void btnCheck_Click(object sender, EventArgs e)
        {
            try
            {
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


                bll.ExecNonQuery("WMS.UpdateDeliverTransferTaskCheck", paras);
                AddOperateLog("调拨单审核", btnCheck.Text + ":" + this.txtID.Text);
                DataTable dt = bll.FillDataTable("WMS.SelectDeliverTransfer", new DataParameter[] { new DataParameter("{0}", string.Format("{0}='{1}' and Flag={2}", PrimaryKey, this.txtID.Text, Flag)) });
                BindData(dt);

            }
            catch (Exception ex)
            {
                WMS.App_Code.JScript.Instance.ShowMessage(this.updatePanel, ex.Message);
                BindDataSub2();
                return;
            }

          
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            //调拨出库写入K3

            int StartToK3 = int.Parse(System.Configuration.ConfigurationManager.AppSettings["StartToK3"]);
            if (StartToK3 > 0)
            {
                DataTable dtK3Main = bll.FillDataTable("WebService.SelectTransferToK3Main", new DataParameter[] { new DataParameter("@BillID", this.txtID.Text) });
                DataTable dtK3Sub = bll.FillDataTable("WebService.SelectTransferToK3Sub", new DataParameter[] { new DataParameter("@BillID", this.txtID.Text) });

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
                    DataTable dtK3 = bllK3.FillDataTable("WebService.SpStockBillTOK3", new DataParameter[] { new DataParameter("@xml", strXML) });
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

                try
                {
                    DataParameter[] paras = new DataParameter[2];

                    paras[0] = new DataParameter("@BillID", this.txtID.Text);
                    paras[1] = new DataParameter("@UserName", Session["EmployeeCode"].ToString());

                    bll.ExecNonQueryTran("WMS.SPCheckDeliverTransfer", paras);
                    AddOperateLog("调拨单出库审核", this.txtID.Text);
                    DataTable dt = bll.FillDataTable("WMS.SelectDeliverTransfer", new DataParameter[] { new DataParameter("{0}", string.Format("{0}='{1}' and Flag={2}", PrimaryKey, this.txtID.Text, Flag)) });
                    BindData(dt);
                }
                catch (Exception ex)
                {
                    WMS.App_Code.JScript.Instance.ShowMessage(this.updatePanel, ex.Message);
                    BindDataSub2();
                    return;
                }

            }
            else
            {
                try
                {
                    DataParameter[] paras = new DataParameter[2];

                    paras[0] = new DataParameter("@BillID", this.txtID.Text);
                    paras[1] = new DataParameter("@UserName", Session["EmployeeCode"].ToString());

                    bll.ExecNonQueryTran("WMS.SPCheckDeliverTransfer", paras);
                    AddOperateLog("调拨单出库审核", this.txtID.Text);
                    DataTable dt = bll.FillDataTable("WMS.SelectDeliverTransfer", new DataParameter[] { new DataParameter("{0}", string.Format("{0}='{1}' and Flag={2}", PrimaryKey, this.txtID.Text, Flag)) });
                    BindData(dt);
                }
                catch (Exception ex)
                {
                    WMS.App_Code.JScript.Instance.ShowMessage(this.updatePanel, ex.Message);
                    BindDataSub2();
                    return;
                }
            }
        }
        
        #endregion

        #region 上下笔事件

        protected void btnFirst_Click(object sender, EventArgs e)
        {
            this.HdfActiveTab.Value = "0";
            BindData(bll.GetRecord("F", TableName, Filter, PrimaryKey, this.txtID.Text));
        }
        protected void btnPre_Click(object sender, EventArgs e)
        {
            this.HdfActiveTab.Value = "0";
            BindData(bll.GetRecord("P", TableName, Filter, PrimaryKey, this.txtID.Text));
        }
        protected void btnNext_Click(object sender, EventArgs e)
        {
            this.HdfActiveTab.Value = "0";
            BindData(bll.GetRecord("N", TableName, Filter, PrimaryKey, this.txtID.Text));
        }
        protected void btnLast_Click(object sender, EventArgs e)
        {
            this.HdfActiveTab.Value = "0";
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

            this.ddlPageSizeSub2.Items.Add(new ListItem("10", "10"));
            this.ddlPageSizeSub2.Items.Add(new ListItem("20", "20"));
            this.ddlPageSizeSub2.Items.Add(new ListItem("30", "30"));
            this.ddlPageSizeSub2.Items.Add(new ListItem("40", "40"));
            this.ddlPageSizeSub2.Items.Add(new ListItem("50", "50"));
            this.ddlPageSizeSub2.Items.Add(new ListItem("100", "100"));
        }

        protected void ddlPageSizeSub1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.HdfActiveTab.Value = "0";
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


            if (dgv.ID.IndexOf("1") > 1)
                SetPageSubBtnEnabled();
            else
                SetPageSub2BtnEnabled();
            SetPermission();
            ScriptManager.RegisterStartupScript(this.updatePanel, this.GetType(), "Resize", "content_resize();", true);
        }

        protected void btnFirstSub1_Click(object sender, EventArgs e)
        {
            this.HdfActiveTab.Value = "0";
            MovePage(this.dgViewSub1, 0);
        }

        protected void btnPreSub1_Click(object sender, EventArgs e)
        {
            this.HdfActiveTab.Value = "0";
            MovePage(this.dgViewSub1, this.dgViewSub1.PageIndex - 1);
        }

        protected void btnNextSub1_Click(object sender, EventArgs e)
        {
            this.HdfActiveTab.Value = "0";
            MovePage(this.dgViewSub1, this.dgViewSub1.PageIndex + 1);
        }

        protected void btnLastSub1_Click(object sender, EventArgs e)
        {
            this.HdfActiveTab.Value = "0";
            MovePage(this.dgViewSub1, this.dgViewSub1.PageCount - 1);
        }

        protected void btnToPageSub1_Click(object sender, EventArgs e)
        {
            this.HdfActiveTab.Value = "0";
            MovePage(this.dgViewSub1, int.Parse(this.txtPageNoSub1.Text) - 1);
        }
        #endregion

        #region 从表2显示
        protected void ddlPageSizeSub2_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.HdfActiveTab.Value = "1";
            DataTable dt1 = (DataTable)Session[FormID + "_View_dgViewSub2"];
            this.dgViewSub2.DataSource = dt1;
            this.dgViewSub2.PageSize = int.Parse(ddlPageSizeSub2.Text);
            this.dgViewSub2.DataBind();
            SetPageSub2BtnEnabled();
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

        protected void btnFirstSub2_Click(object sender, EventArgs e)
        {
            this.HdfActiveTab.Value = "1";
            MovePage(this.dgViewSub2, 0);
        }

        protected void btnPreSub2_Click(object sender, EventArgs e)
        {
            this.HdfActiveTab.Value = "1";
            MovePage(this.dgViewSub2, this.dgViewSub2.PageIndex - 1);
        }

        protected void btnNextSub2_Click(object sender, EventArgs e)
        {
            this.HdfActiveTab.Value = "1";
            MovePage(this.dgViewSub2, this.dgViewSub2.PageIndex + 1);
        }

        protected void btnLastSub2_Click(object sender, EventArgs e)
        {
            this.HdfActiveTab.Value = "1";
            MovePage(this.dgViewSub2, this.dgViewSub2.PageCount - 1);
        }

        protected void btnToPageSub2_Click(object sender, EventArgs e)
        {
            this.HdfActiveTab.Value = "1";
            MovePage(this.dgViewSub2, int.Parse(this.txtPageNoSub2.Text) - 1);
        }
        #endregion

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            int membercount = bll.GetRowCount("VUsed_WMS_Migration", string.Format("Flag=2 and BillID='{0}'", this.txtID.Text));
            if (membercount > 0)
            {
                WMS.App_Code.JScript.Instance.ShowMessage(this.updatePanel, txtID.Text + "已经被使用，不能删除！");
                return;
            }
            string strID = this.txtID.Text;
            string[] comds = new string[2];
            comds[0] = "WMS.DeleteDeliverTransfer";
            comds[1] = "WMS.DeleteDeliverTransferSub";
            List<DataParameter[]> paras = new List<DataParameter[]>();
            paras.Add(new DataParameter[] { new DataParameter("{0}", string.Format("BillID='{0}' and Flag={1}", strID, Flag)) });
            paras.Add(new DataParameter[] { new DataParameter("{0}", string.Format("BillID='{0}' and Flag={1}", strID, Flag)) });
            bll.ExecTran(comds, paras);
            AddOperateLog("调拨单", "删除单号：" + strID);
            btnNext_Click(sender, e);
            if (this.txtID.Text == strID)
                btnPre_Click(sender, e);
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            int membercount = bll.GetRowCount("VUsed_WMS_Migration", string.Format("Flag=2 and BillID='{0}'", this.txtID.Text));
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

            Microsoft.Office.Interop.Excel.Workbook wb = excel.Workbooks._Open(Path + "OutStockTran.xls", Missing.Value, Missing.Value, Missing.Value, Missing.Value
                    , Missing.Value, Missing.Value, Missing.Value, Missing.Value
                    , Missing.Value, Missing.Value, Missing.Value, Missing.Value);

            Microsoft.Office.Interop.Excel.Worksheet wsheet = wb.Sheets[1];
            ((Microsoft.Office.Interop.Excel._Worksheet)wb.Sheets[1]).Activate();






            DataTable dtView = bll.FillDataTable("WMS.SelectDeliverTransferSubByReport", new DataParameter[] { new DataParameter("{0}", string.Format("BillID='{0}' and Flag={1}", this.txtID.Text, Flag)), new DataParameter("@BillID", this.txtID.Text) });
            int index = dtView.Rows.Count / 5 + 1;

            index = 0;
            if (dtView.Rows.Count % 5 == 0)
                index = dtView.Rows.Count / 5;
            else
                index = dtView.Rows.Count / 5 + 1;

            int k;
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
                wsheet.Name = "调拨单" + j.ToString();

                excel.Cells[2, 2] = this.txtID.Text;
                excel.Cells[2, 5] = this.txtBillDate.Text;
                k = 4;
                //写入明细
                for (int i = 0; i < 5; i++)
                {
                    int RowIndex = (j - 1) * 5 + i;
                    if (RowIndex >= dtView.Rows.Count)
                        break;
                    excel.Cells[k, 1] = dtView.Rows[RowIndex]["ProductNo"].ToString();
                    excel.Cells[k, 3] = dtView.Rows[RowIndex]["ProductName"].ToString();
                    excel.Cells[k, 4] = dtView.Rows[RowIndex]["ProductModel"].ToString() + "(" + dtView.Rows[RowIndex]["ProductFModel"].ToString() + ")  " + dtView.Rows[RowIndex]["ColorName"].ToString();
                    excel.Cells[k, 6] = dtView.Rows[RowIndex]["Unit"].ToString();
                    excel.Cells[k, 7] = dtView.Rows[RowIndex]["Qty"];
                    excel.Cells[k, 10] = this.ddlStockFunction.SelectedItem.Text;
                    k++;
                }
                //excel.Cells[11, 7] = this.txtChecker.Text;
                //excel.Cells[11, 9] = this.txtCreator.Text;

            }


            Microsoft.Office.Interop.Excel.Range excelRange;

            index++;
            wsheet = wb.Sheets[index];
            ((Microsoft.Office.Interop.Excel._Worksheet)wb.Sheets[index]).Activate();

            DataTable dtDetail = bll.FillDataTable("WMS.SelectDeliverTransferDetailReport", new DataParameter[] { new DataParameter("@BillID", this.txtID.Text) });

            excel.Cells[3, 9] = this.txtBillDate.Text;
            excel.Cells[4, 3] = this.ddlStockFunction.SelectedItem.Text;
            excel.Cells[4, 9] = "";
            excel.Cells[4, 12] = "";
            excel.Cells[5, 3] = "";


            if (dtDetail.Rows.Count > 50)
            {
                int AddCount = 0;
                if ((dtDetail.Rows.Count - 50) % 2 == 0)
                    AddCount = (dtDetail.Rows.Count - 50) / 2;
                else
                    AddCount = (dtDetail.Rows.Count - 50) / 2 + 1;

                for (int i = 0; i < AddCount; i++)
                {
                    Microsoft.Office.Interop.Excel.Range xlsRow = (Microsoft.Office.Interop.Excel.Range)wsheet.Rows[10, Missing.Value];
                    xlsRow.Insert(Microsoft.Office.Interop.Excel.XlDirection.xlUp, Missing.Value);
                }
            }
            k = 8;
            int RowCount = 0;
            if (dtDetail.Rows.Count % 2 == 0)
                RowCount = dtDetail.Rows.Count / 2;
            else
                RowCount = dtDetail.Rows.Count / 2 + 1;
            if (RowCount < 25)
                RowCount = 25;
            for (int i = 0; i < RowCount; i++)
            {
                if (i < dtDetail.Rows.Count)
                {
                    excel.Cells[k, 1] = i + 1;
                    excel.Cells[k, 2] = dtDetail.Rows[i]["PRODUCT_MODEL"].ToString() + dtDetail.Rows[i]["ColName"].ToString();
                    excel.Cells[k, 4] = dtDetail.Rows[i]["BarCode"].ToString();
                }
                int TwoRow = i + RowCount;
                if (TwoRow < dtDetail.Rows.Count)
                {
                    excel.Cells[k, 7] = TwoRow + 1;
                    excel.Cells[k, 8] = dtDetail.Rows[TwoRow]["PRODUCT_MODEL"].ToString() + dtDetail.Rows[TwoRow]["ColName"].ToString();
                    excel.Cells[k, 10] = dtDetail.Rows[TwoRow]["BarCode"].ToString();
                }
                excelRange = wsheet.Range[excel.Cells[k, 2], excel.Cells[k, 3]];
                excelRange.Merge();
                excelRange = wsheet.Range[excel.Cells[k, 4], excel.Cells[k, 6]];
                excelRange.Merge();

                excelRange = wsheet.Range[excel.Cells[k, 8], excel.Cells[k, 9]];
                excelRange.Merge();
                excelRange = wsheet.Range[excel.Cells[k, 10], excel.Cells[k, 12]];
                excelRange.Merge();
                excelRange = wsheet.Range[excel.Cells[k, 13], excel.Cells[k, 14]];
                excelRange.Merge();
                k++;
            }
            k = k + 2;

            RowCount = 0;
            if (dtView.Rows.Count % 2 == 0)
                RowCount = dtView.Rows.Count / 2;
            else
                RowCount = dtView.Rows.Count / 2 + 1;
            if (RowCount > 9)
            {
                for (int i = 9; i < RowCount; i++)
                {
                    Microsoft.Office.Interop.Excel.Range xlsRow = (Microsoft.Office.Interop.Excel.Range)wsheet.Rows[k + 1, Missing.Value];
                    xlsRow.Insert(Microsoft.Office.Interop.Excel.XlDirection.xlUp, Missing.Value);
                }
            }
            if (RowCount < 9)
                RowCount = 9;
            for (int i = 0; i < RowCount; i++)
            {

                if (i < dtView.Rows.Count)
                {
                    excel.Cells[k, 3] = i + 1;
                    excel.Cells[k, 4] = dtView.Rows[i]["ProductModel"].ToString();
                    excel.Cells[k, 5] = dtView.Rows[i]["ColorName"].ToString();
                    excel.Cells[k, 7] = dtView.Rows[i]["Qty"].ToString();

                    int Qty = (int)dtView.Rows[i]["Qty"];
                    int PackQty = (int)dtView.Rows[i]["PACK_QTY"];
                    int PQty = 0;

                    //箱数，体积
                    if (int.Parse(dtView.Rows[i]["SubCount"].ToString()) > 1)
                    {
                        excel.Cells[k, 8] = Qty * (int)dtView.Rows[i]["SubCount"];

                    }
                    else
                    {
                        if (Qty % PackQty != 0)
                            PQty = Qty / PackQty + 1;
                        else
                            PQty = Qty / PackQty;

                        excel.Cells[k, 8] = PQty;
                    }

                }
                else
                {
                    excel.Cells[k, 3] = i + 1;
                }
                int TwoRow = i + RowCount;
                if (TwoRow < dtView.Rows.Count)
                {
                    excel.Cells[k, 9] = TwoRow + 1;
                    excel.Cells[k, 10] = dtView.Rows[TwoRow]["ProductModel"].ToString();
                    excel.Cells[k, 12] = dtView.Rows[TwoRow]["ColorName"].ToString();
                    excel.Cells[k, 13] = dtView.Rows[TwoRow]["Qty"].ToString();

                    int Qty = (int)dtView.Rows[i]["Qty"];
                    int PackQty = (int)dtView.Rows[i]["PACK_QTY"];
                    int PQty = 0;
                    //箱数，体积
                    if (int.Parse(dtView.Rows[i]["SubCount"].ToString()) > 1)
                    {
                        excel.Cells[k, 14] = Qty * (int)dtView.Rows[i]["SubCount"];

                    }
                    else
                    {
                        if (Qty % PackQty != 0)
                            PQty = Qty / PackQty + 1;
                        else
                            PQty = Qty / PackQty;
                        excel.Cells[k, 14] = PQty;
                    }
                }
                else
                {
                    excel.Cells[k, 9] = TwoRow + 1;
                }

                excelRange = wsheet.Range[excel.Cells[k, 5], excel.Cells[k, 6]];
                excelRange.Merge();
                excelRange = wsheet.Range[excel.Cells[k, 10], excel.Cells[k, 11]];
                excelRange.Merge();
                k++;
            }
            if (dtView.Rows.Count > 18)
            {
                excelRange = (Microsoft.Office.Interop.Excel.Range)excel.Cells[k, 4];
                excelRange.Formula = "=SUM(G" + (k - RowCount).ToString() + ":G" + (k - 1).ToString() + ")+SUM(M" + (k - RowCount).ToString() + ":M" + (k - 1).ToString() + ")";
                excelRange.Calculate();

                excelRange = (Microsoft.Office.Interop.Excel.Range)excel.Cells[k, 6];
                excelRange.Formula = "=SUM(H" + (k - RowCount).ToString() + ":H" + (k - 1).ToString() + ")+SUM(N" + (k - RowCount).ToString() + ":N" + (k - 1).ToString() + ")";
                excelRange.Calculate();
            }




            excel.Visible = false;
            string fileName = "调拨单" + this.txtID.Text;
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