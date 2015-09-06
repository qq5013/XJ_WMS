using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using IDAL;
using System.Drawing;

namespace WMS.WebUI.OutStock
{
    public partial class OtherDelivers : WMS.App_Code.BasePage
    {
        #region 参数设置
        int pageIndex = 1;
        int pageSize = 15;
        int totalCount = 0;
        int pageCount = 0;
        protected string FormID = "OtherDeliver";
        protected string TableName = "View_WMS_OtherDeliver";
        protected string PrimaryKey = "Flag,BillID";

        int Flag = 3;
        
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
                    ViewState["filter"] = "Flag=" + Flag;
                    ViewState["CurrentPage"] = 1;
                    BindPageSize();
                    writeJsvar(FormID,SqlCmd, "");
                    SetBtnEnabled("");
                    HdfActiveTab.Value = "0";
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel, this.GetType(), "", "BindEvent();GetResultFromServer();", true);
                }
               

            }
            catch (Exception exp)
            {
                WMS.App_Code.JScript.Instance.ShowMessage(this.UpdatePanel, exp.Message);
            }
        }
       
        protected void btnDeletet_Click(object sender, EventArgs e)
        {
            string strGroupID = "'-1',";

            for (int i = 0; i < this.gvMain.Rows.Count; i++)
            {
                CheckBox cb = (CheckBox)(this.gvMain.Rows[i].FindControl("cbSelect"));
                if (cb != null && cb.Checked)
                {
                    LinkButton hk = (LinkButton)(this.gvMain.Rows[i].FindControl("LinkButton1"));
                    //判断能否删除
                    int membercount = bll.GetRowCount("Vused_WMS_OtherDeliver", string.Format("Flag=3 and BillID='{0}'", hk.Text));
                    if (membercount > 0)
                    {
                        WMS.App_Code.JScript.Instance.ShowMessage(this.UpdatePanel, hk.Text + "已经被使用，不能删除！");
                        return;
                    }

                    strGroupID += "'"+hk.Text + "',";
                }
            }
            strGroupID += "'-1'";
            string[] comds = new string[3];
            comds[0] = "WMS.DeleteOtherDeliver";
            comds[1] = "WMS.DeleteOtherDeliverSub";
            comds[2] = "WMS.DeleteOtherDeliverDetail";
            List<DataParameter[]> paras = new List<DataParameter[]>();
            paras.Add(new DataParameter[] { new DataParameter("{0}", string.Format("BillID in ({0}) and Flag={1}", strGroupID, Flag)) });
            paras.Add(new DataParameter[] { new DataParameter("{0}", string.Format("BillID in({0}) and Flag={1}", strGroupID, Flag)) });
            paras.Add(new DataParameter[] { new DataParameter("{0}", string.Format("BillID in({0}) and Flag={1}", strGroupID, Flag)) });
            bll.ExecTran(comds, paras);
            AddOperateLog("其它出货单", "删除单号：" + strGroupID.Replace("'-1',", "").Replace(",'-1'", ""));
 

            SetBtnEnabled("");
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
                

                if (e.Row.RowIndex == Convert.ToInt32(this.hdnRowIndex.Value))
                {
                    e.Row.BackColor = ColorTranslator.FromHtml("#60c0ff");
                }
                e.Row.Attributes.Add("onclick", string.Format("$('#hdnRowValue').val('{1}');selectRow({0});", e.Row.RowIndex, ((LinkButton)e.Row.Cells[2].FindControl("LinkButton1")).Text));
                e.Row.Attributes.Add("style", "cursor:pointer;");
            }

        }
        protected void dgViewSub1_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        #region 翻页處理 +查询按钮+笔数切换
        protected void btnFirst_Click(object sender, System.EventArgs e)
        {
            hdnRowIndex.Value = "0";
            dvscrollX.Value = "0";
            dvscrollY.Value = "0";
            HdfActiveTab.Value = "0";
            SetBtnEnabled("F");
        }

        protected void btnLast_Click(object sender, System.EventArgs e)
        {
            hdnRowIndex.Value = "0";
            dvscrollX.Value = "0";
            dvscrollY.Value = "0";
            HdfActiveTab.Value = "0";
            SetBtnEnabled("L");
        }

        protected void btnPre_Click(object sender, System.EventArgs e)
        {
            hdnRowIndex.Value = "0";
            dvscrollX.Value = "0";
            dvscrollY.Value = "0";
            HdfActiveTab.Value = "0";
            SetBtnEnabled("P");
        }

        protected void btnNext_Click(object sender, System.EventArgs e)
        {
            hdnRowIndex.Value = "0";
            dvscrollX.Value = "0";
            dvscrollY.Value = "0";
            HdfActiveTab.Value = "0";
            SetBtnEnabled("N");
        }
        protected void btnToPage_Click(object sender, System.EventArgs e)
        {
            hdnRowIndex.Value = "0";
            dvscrollX.Value = "0";
            dvscrollY.Value = "0";
            HdfActiveTab.Value = "0";
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
            ScriptManager.RegisterStartupScript(this.UpdatePanel, this.GetType(), "Resize", "content_resize();", true);
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
                WMS.App_Code.JScript.Instance.ShowMessage(this.UpdatePanel, exp.Message);
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

            this.ddlPageSizeSub1.Items.Clear();
            this.ddlPageSizeSub1.Items.Add(new ListItem("10", "10"));
            this.ddlPageSizeSub1.Items.Add(new ListItem("20", "20"));
            this.ddlPageSizeSub1.Items.Add(new ListItem("30", "30"));
            this.ddlPageSizeSub1.Items.Add(new ListItem("40", "40"));
            this.ddlPageSizeSub1.Items.Add(new ListItem("50", "50"));
            this.ddlPageSizeSub1.Items.Add(new ListItem("100", "100"));

            this.ddlPageSizeSub2.Items.Clear();
            this.ddlPageSizeSub2.Items.Add(new ListItem("10", "10"));
            this.ddlPageSizeSub2.Items.Add(new ListItem("20", "20"));
            this.ddlPageSizeSub2.Items.Add(new ListItem("30", "30"));
            this.ddlPageSizeSub2.Items.Add(new ListItem("40", "40"));
            this.ddlPageSizeSub2.Items.Add(new ListItem("50", "50"));
            this.ddlPageSizeSub2.Items.Add(new ListItem("100", "100"));
        }

        /// <summary>
        /// 綁定GirdView
        /// </summary>
        /// <param name="pageIndex"></param>
        private void BindGrid(int pageIndex)
        {
            dtGroup = bll.GetDataPage("WMS.SelectOtherDeliver", int.Parse(ViewState["CurrentPage"].ToString()), int.Parse(ViewState["PageSize"].ToString()), out totalCount, out pageCount, new DataParameter[] { new DataParameter("{0}", ViewState["filter"].ToString()) });
            pageCount = GetPageCount(totalCount, pageSize);
            if (ViewState["CurrentPage"].ToString() == "0" || int.Parse(ViewState["CurrentPage"].ToString()) > pageCount)
                ViewState["CurrentPage"] = pageCount;

            if (dtGroup.Rows.Count == 0)
            {
                dtGroup.Rows.Add(dtGroup.NewRow());
                gvMain.DataSource = dtGroup;
                gvMain.DataBind();
                int columnCount = gvMain.Rows[0].Cells.Count;
                gvMain.Rows[0].Cells.Clear();
                gvMain.Rows[0].Cells.Add(new TableCell());
                gvMain.Rows[0].Cells[0].ColumnSpan = columnCount;
                gvMain.Rows[0].Cells[0].Text = "没有符合以上条件的数据,请重新查询 ";
                gvMain.Rows[0].Visible = true;

                this.btnFirst.Enabled = false;
                this.btnPre.Enabled = false;
                this.btnNext.Enabled = false;
                this.btnLast.Enabled = false;
                this.btnToPage.Enabled = false;
                lblCurrentPage.Visible = false;
                BindDataSub("");
                BindDataSub2("");

            }
            else
            {
                this.gvMain.DataSource = dtGroup;
                this.gvMain.DataBind();

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

                BindDataSub(dtGroup.Rows[0]["BillID"].ToString());
                BindDataSub2(dtGroup.Rows[0]["BillID"].ToString());
               
            }
        }

        private void BindDataSub(string ID)
        {

            DataTable dt = bll.FillDataTable("WMS.SelectOtherDeliverSub", new DataParameter[] { new DataParameter("{0}", string.Format("BillID='{0}' and Flag={1}", ID, Flag)) });
            Session[FormID + "_S_dgViewSub1"] = dt;

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
                dgViewSub1.Rows[0].Cells[0].Text = "没有明细资料！";
                dgViewSub1.Rows[0].Visible = true;
            }
            else
            {
                dgViewSub1.DataSource = dt;
                dgViewSub1.DataBind();
            }
            SetPageSubBtnEnabled();

        }

        private void BindDataSub2(string ID)
        {
            DataTable dt = bll.FillDataTable("WMS.SelectOtherDeliverDetail", new DataParameter[] { new DataParameter("{0}", string.Format("BillID='{0}' and Flag={1}", ID, Flag)) });
            Session[FormID + "_S_dgViewSub2"] = dt;

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

        protected void btnReload_Click(object sender, EventArgs e)
        {
            int i = Convert.ToInt32(this.hdnRowIndex.Value);

            BindDataSub(this.hdnRowValue.Value);
            BindDataSub2(this.hdnRowValue.Value);

            for (int j = 0; j < this.gvMain.Rows.Count; j++)
            {
                if (j % 2 == 0)
                    this.gvMain.Rows[j].BackColor = ColorTranslator.FromHtml("#ffffff");
                else
                    this.gvMain.Rows[j].BackColor = ColorTranslator.FromHtml("#E9F2FF");
                if (j == i)
                    this.gvMain.Rows[j].BackColor = ColorTranslator.FromHtml("#60c0ff");
            }
            ScriptManager.RegisterStartupScript(this.UpdatePanel, this.GetType(), "Resize", "content_resize();", true);
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

        #region 从表资料显示


        private void SetPageSubBtnEnabled()
        {
            DataTable dt = (DataTable)Session[FormID + "_S_dgViewSub1"];
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
        protected void ddlPageSizeSub1_SelectedIndexChanged(object sender, EventArgs e)
        {
            HdfActiveTab.Value = "0";
            DataTable dt1 = (DataTable)Session[FormID + "_S_dgViewSub1"];
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
            DataTable dt1 = (DataTable)Session[FormID + "_S_" + dgv.ID];
            dgv.PageIndex = pindex;
            dgv.DataSource = dt1;
            dgv.DataBind();


            if (dgv.ID.IndexOf("1") > 1)
                SetPageSubBtnEnabled();
            else
                SetPageSub2BtnEnabled();
            ScriptManager.RegisterStartupScript(this.UpdatePanel, this.GetType(), "Resize", "content_resize();", true);
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
        #endregion
        #region 从表2显示

        private void SetPageSub2BtnEnabled()
        {
            DataTable dt = (DataTable)Session[FormID + "_S_dgViewSub2"];
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

        protected void ddlPageSizeSub2_SelectedIndexChanged(object sender, EventArgs e)
        {
            HdfActiveTab.Value = "1";
            DataTable dt1 = (DataTable)Session[FormID + "_S_dgViewSub2"];
            this.dgViewSub2.DataSource = dt1;
            this.dgViewSub2.PageSize = int.Parse(ddlPageSizeSub2.Text);
            this.dgViewSub2.DataBind();
            SetPageSub2BtnEnabled();
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



    }
}