﻿using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Drawing;
using IDAL;

namespace WMS.WebUI.SysInfo.SystemLog
{
    public partial class OperationLog :WMS.App_Code.BasePage
    {
        int pageIndex = 1;
        int pageSize = 15;
        int totalCount = 0;
        int pageCount = 0;
        string filter = "1=1";
        DataTable dtLog;
        string PrimaryKey = "OperatorLogID";
        string OrderByFields = "OperatorLogID desc";
        string TableName = "sys_OperatorLog";
        string strQueryFields = "OperatorLogID,LoginUser,LoginTime,LoginModule,ExecuteOperator";
        BLL.BLLBase bll = new BLL.BLLBase();

        #region 窗体加裁
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["sys_PageCount"] != null)
                {
                    pageSize = Convert.ToInt32(Session["sys_PageCount"].ToString());
                    pager.PageSize = pageSize;
                }
                if (Session["pager_ShowPageIndex"] != null)
                {
                    pager.ShowPageIndex = Convert.ToBoolean(Session["pager_ShowPageIndex"].ToString());
                }

                if (!IsPostBack)
                {
                    if (this.btnDelete.Enabled)
                    {
                        this.btnDeleteAll.Enabled = true;
                    }
                    totalCount = bll.GetRowCount(TableName, filter);
                    pager.RecordCount = totalCount;
                    GridDataBind();
                }
                else
                {
                    pageCount = Convert.ToInt32(ViewState["pageCount"]);
                    pageIndex = Convert.ToInt32(ViewState["pageIndex"]);
                    totalCount = Convert.ToInt32(ViewState["totalCount"]);
                    filter = ViewState["filter"].ToString();
                    OrderByFields = ViewState["OrderByFields"].ToString();
                    totalCount = bll.GetRowCount(TableName, filter);
                    GridDataBind();
                }

            }
            catch (Exception exp)
            {
              WMS.App_Code.JScript.Instance.ShowMessage(this.UpdatePanel1, exp.Message);
            }
        }
        #endregion

        #region 数据源绑定
        void GridDataBind()
        {
            dtLog = bll.GetDataPage("Security.SeleteOperatorLog", pageIndex, pageSize, out totalCount,out pageCount, new DataParameter[] { new DataParameter("{0}", filter) });
            if (dtLog.Rows.Count == 0)
            {
                dtLog.Rows.Add(dtLog.NewRow());
                gvMain.DataSource = dtLog;
                gvMain.DataBind();
                int columnCount = gvMain.Rows[0].Cells.Count;
                gvMain.Rows[0].Cells.Clear();
                gvMain.Rows[0].Cells.Add(new TableCell());
                gvMain.Rows[0].Cells[0].ColumnSpan = columnCount;
                gvMain.Rows[0].Cells[0].Text = "没有符合以上条件的数据,请重新查询 ";
                gvMain.Rows[0].Visible = true;

            }
            else
            {
                this.gvMain.DataSource = dtLog;
                this.gvMain.DataBind();
            }

            ViewState["pageIndex"] = pageIndex;
            ViewState["totalCount"] = totalCount;
            ViewState["pageCount"] = pageCount;
            ViewState["filter"] = filter;
            ViewState["OrderByFields"] = OrderByFields;
        }
        #endregion

        #region GridView绑定
        protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                CheckBox chk = new CheckBox();
                chk.Attributes.Add("style", " font-weight:bold; text-align:center;word-break:keep-all; white-space:nowrap");
                chk.ID = "checkAll";
                chk.Attributes.Add("onclick", "checkboxChange(this,'gvMain',0);");
                chk.Text = "";
                e.Row.Cells[0].Controls.Add(chk);
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.RowIndex % 2 == 0)
                {
                    e.Row.BackColor = Color.FromName(Session["grid_EvenRowColor"].ToString());
                }
                else
                {
                    e.Row.BackColor = Color.FromName(Session["grid_OddRowColor"].ToString());
                }
                e.Row.Cells[0].Attributes.Add("style", "word-break:keep-all; white-space:nowrap");

                CheckBox chk = new CheckBox();
                e.Row.Cells[0].Controls.Add(chk);
            }
        }
        #endregion

        #region 删除日志
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string strGroupID = "-1,";
                for (int i = 0; i < gvMain.Rows.Count; i++)
                {
                    CheckBox chk = (CheckBox)gvMain.Rows[i].Cells[0].Controls[0];
                    if (chk.Enabled && chk.Checked)
                    {
                        strGroupID += dtLog.Rows[i]["OperatorLogID"].ToString() + ",";
                    }
                }
                strGroupID += "-1";
                bll.ExecNonQuery("Security.DeleteOperatorLog", new DataParameter[] { new DataParameter("{0}", strGroupID) });
                totalCount = bll.GetRowCount(TableName, filter);
                pager.RecordCount = totalCount;
                if (pageIndex > pager.PageCount)
                {
                    pageIndex = pager.PageCount;
                }
                GridDataBind();
                AddOperateLog("操作日志管理", "删除操作日志");
            }
            catch (Exception exp)
            {
               WMS.App_Code.JScript.Instance.ShowMessage(this.UpdatePanel1, exp.Message);
            }
        }
        #endregion


        #region 清空日志
        protected void btnDeleteAll_Click(object sender, EventArgs e)
        {
            bll.ExecNonQuery("Security.DeleteAllOperatorLog", null);
            pageIndex = 1;
            pager.RecordCount = 0;
            GridDataBind();
            AddOperateLog("操作日志管理", "清空操作日志");
        }
        #endregion

        #region 查询
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            string start = "1900-01-01";
            string end = System.DateTime.Now.AddDays(1).ToString();
            try
            {
                if (this.txtDateStart.Text.Trim().Length > 0)
                {
                    start = Convert.ToDateTime(this.txtDateStart.Text.Trim()).ToString();
                }
                if (this.txtDateEnd.Text.Trim().Length > 0)
                {
                    end = Convert.ToDateTime(this.txtDateEnd.Text.Trim()).AddDays(1).ToString();
                }
            }
            catch
            {
               WMS.App_Code.JScript.Instance.ShowMessage(this.UpdatePanel1, "输入时间格式不正确！");
                return;
            }

            filter = string.Format("{0} like '{1}%' and (LoginTime>='{2}' and LoginTime<'{3}')", this.ddl_Field.SelectedValue, this.txtKeyWords.Text.Trim().Replace("'", ""), start, end);
            ViewState["filter"] = filter;
            if (rbASC.Checked)
            {
                OrderByFields = this.ddl_Field.SelectedValue + " asc ,LoginTime desc";
            }
            else
            {
                OrderByFields = this.ddl_Field.SelectedValue + " desc ,LoginTime desc";
            }

            totalCount = bll.GetRowCount(TableName,filter);
            pageIndex = 1;
            pager.CurrentPageIndex = 1;
            pager.RecordCount = totalCount;
            GridDataBind();
        }
        #endregion

        # region 分页控件 页码changing事件
        protected void pager_PageChanging(object src, PageChangingEventArgs e)
        {
            pager.CurrentPageIndex = e.NewPageIndex;
            pager.RecordCount = totalCount;
            pageIndex = pager.CurrentPageIndex;
            ViewState["pageIndex"] = pageIndex;
            GridDataBind();
        }
        #endregion
    }
}