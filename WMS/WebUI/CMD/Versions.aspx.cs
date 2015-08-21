using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using IDAL;

namespace WMS.WebUI.CMD
{
    public partial class Versions : WMS.App_Code.BasePage
    {
        #region 参数设置
        int pageIndex = 1;
        int pageSize = 15;
        int totalCount = 0;
        int pageCount = 0;
        protected string FormID = "Version";
        protected string TableName = "View_CMD_VERSION";
        protected string PrimaryKey = "ID";

     
        string filter = "1=1";
        DataTable dtGroup;
        private string strQueryFields = "VERSION_CODE, PRODUCT_CODE, VERSION_NAME, MEMO, Product_Name, ID";

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
                    BindGrid(pageIndex);
                    BindPageSize();
                    writeJsvar(FormID, TableName, PrimaryKey, "");
                }
                

            }
            catch (Exception exp)
            {
                WMS.App_Code.JScript.Instance.ShowMessage(this.UpdatePanel1, exp.Message);
            }
        }
       
        protected void btnDeletet_Click(object sender, EventArgs e)
        {
            string strVersionCode = "-1,";

            for (int i = 0; i < this.GridView1.Rows.Count; i++)
            {
                CheckBox cb = (CheckBox)(this.GridView1.Rows[i].FindControl("cbSelect"));
                if (cb != null && cb.Checked)
                {
                    LinkButton hk = (LinkButton)(this.GridView1.Rows[i].FindControl("LinkButton1"));
                    //判断能否删除
               
                    //int Count = bll.GetRowCount(TableName, string.Format("COLOR_CODE='{0}'", hk.Text.Trim()));
                    //if (Count > 0)
                    //{
                    //    WMS.App_Code.JScript.Instance.ShowMessage(this.UpdatePanel1, GridView1.Rows[i].Cells[2].Text + "用户组还有用户存在，请调整后再删除！");
                    //    return;
                    //}

                    strVersionCode += hk.Text + ",";
                }
            }
            strVersionCode += "-1";


            bll.ExecNonQuery("Cmd.DeleteVersion", new DataParameter[] { new DataParameter("{0}", strVersionCode) });

            SetBtnEnabled("");
        }


        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
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
            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "Resize", "content_resize();", true);
        }

        protected void btnSearch_Click(object sender, System.EventArgs e)
        {

            try
            {
                filter = string.Format("{0} like '%{1}%'", this.ddlField.SelectedValue, this.txtSearch.Text.Trim().Replace("'", ""));
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


            dtGroup = bll.GetDataPage("cmd.selectVersion", int.Parse(ViewState["CurrentPage"].ToString()), int.Parse(ViewState["PageSize"].ToString()), out totalCount, new DataParameter[] { new DataParameter("{0}", ViewState["filter"].ToString()) });
            pageCount = GetPageCount(totalCount, pageSize);
            if (ViewState["CurrentPage"].ToString() == "0" || int.Parse(ViewState["CurrentPage"].ToString()) > pageCount)
                ViewState["CurrentPage"] = pageCount;

            SaveCookie30Day("CurrentPage_" + FormID, ViewState["CurrentPage"].ToString());

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
        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortExpression = e.SortExpression;
            if (GridViewSortDirection == SortDirection.Ascending)
            {
                GridViewSortDirection = SortDirection.Descending;
                ViewState["OrderField"] = sortExpression + " Desc ";

                SetBtnEnabled("");
                this.GridView1.SortedAscendingCellStyle.BackColor = System.Drawing.Color.Red;
            }
            else if (GridViewSortDirection == SortDirection.Descending)
            {
                GridViewSortDirection = SortDirection.Ascending;
                ViewState["OrderField"] = sortExpression + " Asc ";
                //排序並重新綁定
                SetBtnEnabled("");
                this.GridView1.SortedAscendingCellStyle.BackColor = System.Drawing.Color.Red;
            }
        }
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
       
    }
}