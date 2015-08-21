using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using IDAL;

namespace WMS.WebUI.OutStock
{
    public partial class OtherDeliverView : WMS.App_Code.BasePage
    {
        protected string FormID;
        protected string ID;
        protected string TableName = "View_WMS_OtherDeliver";
        protected string PrimaryKey = "BillID";
        protected int Flag = 3;
        private string Filter = "Flag=3";

        BLL.BLLBase bll = new BLL.BLLBase();

        protected void Page_Load(object sender, EventArgs e)
        {
            ID = Request.QueryString["ID"] + "";
            FormID = Request.QueryString["FormID"] + "";
            if (!IsPostBack)
            {
                DataTable dt = bll.FillDataTable("WMS.SelectOtherDeliver", new DataParameter[] { new DataParameter("{0}", string.Format("{0}='{1}' and Flag={2}", PrimaryKey, ID, Flag)) });
                BindData(dt);
                writeJsvar(FormID, TableName, PrimaryKey, ID);
                BindPageSize();
                this.HdfActiveTab.Value = "0";
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
                this.txtBillDate.Text =ToYMD( dt.Rows[0]["BillDate"]);
                this.ddlStockFunction.SelectedValue = dt.Rows[0]["OutStockID"].ToString();
                this.txtCustomerID.Text = dt.Rows[0]["CustomerID"].ToString();
                this.txtCustomerName.Text = dt.Rows[0]["CustName"].ToString();
                this.txtTranCustID.Text = dt.Rows[0]["TranCustomerID"].ToString();
                this.txtTranCustName.Text = dt.Rows[0]["TranCustomerName"].ToString();
                this.txtMemo.Text = dt.Rows[0]["Memo"].ToString();
                this.txtCreator.Text = dt.Rows[0]["Creator"].ToString();
                this.txtCreateDate.Text = ToYMD(dt.Rows[0]["CreateDate"]);
                this.txtCheckDate.Text = ToYMD(dt.Rows[0]["CheckDate"]);
                this.txtChecker.Text = dt.Rows[0]["Checker"].ToString();
                this.txtTotalQty.Text = dt.Rows[0]["TotalQty"].ToString();
                

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
                   
                }
            }

            this.btnCheck.Enabled = blnCheck;
            if (this.txtChecker.Text != "")
            {
                this.btnDelete.Enabled = false;
                this.btnEdit.Enabled = false;
                this.btnCheck.Enabled = false;
            }
            
            
            
             
        }



        private void BindDataSub()
        {

            DataTable dt = bll.FillDataTable("WMS.SelectOtherDeliverSub", new DataParameter[] { new DataParameter("{0}", string.Format("BillID='{0}' and Flag={1}", this.txtID.Text, Flag)) });
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
            DataTable dt = bll.FillDataTable("WMS.SelectOtherDeliverDetail", new DataParameter[] { new DataParameter("{0}", string.Format("BillID='{0}' and Flag={1}", this.txtID.Text, Flag)) });
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

            int StartToK3 = int.Parse(System.Configuration.ConfigurationManager.AppSettings["StartToK3"]);
            if (StartToK3 > 0)
            {
                DataTable dtK3Main = bll.FillDataTable("WebService.SelectOtherToK3Main", new DataParameter[] { new DataParameter("{0}", string.Format("stock.Flag={0} and Stock.BillID='{1}'", Flag, this.txtID.Text)) });
                DataTable dtK3Sub = bll.FillDataTable("WebService.SelectOtherToK3Sub", new DataParameter[] { new DataParameter("{0}", string.Format("stock.Flag={0} and Stock.BillID='{1}'", Flag, this.txtID.Text)) });

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
                    this.HdfActiveTab.Value = "0";

                    DataParameter[] paras = new DataParameter[2];
                    paras[0] = new DataParameter("@BillID", this.txtID.Text);
                    paras[1] = new DataParameter("@UserName", Session["EmployeeCode"].ToString());


                    bll.ExecNonQueryTran("WMS.SpCheckOtherDeliver", paras);

                    AddOperateLog("其它出库单审核", this.txtID.Text);
                    DataTable dt = bll.FillDataTable("WMS.SelectOtherDeliver", new DataParameter[] { new DataParameter("{0}", string.Format("{0}='{1}' and Flag={2}", PrimaryKey, this.txtID.Text, Flag)) });
                    BindData(dt);
                }
                catch (Exception ex)
                {
                    WMS.App_Code.JScript.Instance.ShowMessage(this.updatePanel, ex.Message);
                    return;
                }
            }
            else
            {
                try
                {
                    this.HdfActiveTab.Value = "0";

                    DataParameter[] paras = new DataParameter[2];
                    paras[0] = new DataParameter("@BillID", this.txtID.Text);
                    paras[1] = new DataParameter("@UserName", Session["EmployeeCode"].ToString());


                    bll.ExecNonQueryTran("WMS.SpCheckOtherDeliver", paras);

                    AddOperateLog("其它出库单审核", this.txtID.Text);
                    DataTable dt = bll.FillDataTable("WMS.SelectOtherDeliver", new DataParameter[] { new DataParameter("{0}", string.Format("{0}='{1}' and Flag={2}", PrimaryKey, this.txtID.Text, Flag)) });
                    BindData(dt);
                }
                catch (Exception ex)
                {
                    WMS.App_Code.JScript.Instance.ShowMessage(this.updatePanel, ex.Message);
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
            string strID = this.txtID.Text;
            string[] comds = new string[3];
            comds[0] = "WMS.DeleteOtherDeliver";
            comds[1] = "WMS.DeleteOtherDeliverSub";
            comds[2] = "WMS.DeleteOtherDeliverDetail";
            List<DataParameter[]> paras = new List<DataParameter[]>();
            paras.Add(new DataParameter[] { new DataParameter("{0}", string.Format("BillID='{0}' and Flag={1}", strID, Flag)) });
            paras.Add(new DataParameter[] { new DataParameter("{0}", string.Format("BillID='{0}' and Flag={1}", strID, Flag)) });
            paras.Add(new DataParameter[] { new DataParameter("{0}", string.Format("BillID='{0}' and Flag={1}", strID, Flag)) });
            bll.ExecTran(comds, paras);
            AddOperateLog("其它出库单", "删除单号：" + strID);

         
            btnNext_Click(sender, e);
            if (this.txtID.Text == strID)
                btnPre_Click(sender, e);
        }

       


    }
}