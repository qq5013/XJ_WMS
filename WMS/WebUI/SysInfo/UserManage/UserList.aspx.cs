using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Drawing;
using System.Collections.Generic;
using IDAL;

namespace WMS.WebUI.SysInfo.UserManage
{
    public partial class UserList : WMS.App_Code.BasePage
    {
        int pageIndex = 1;
        int pageSize = 15;
        int totalCount = 0;
        int pageCount = 0;
        string filter = "1=1";
       
        string PrimaryKey = "UserID";
        string OrderByFields = "UserName";
        string TableView = "sys_UserList";
        DataTable dtUser;
        BLL.BLLBase bll = new BLL.BLLBase();
        #region 窗体加载
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
                    totalCount = bll.GetRowCount(TableView, filter);
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

                    totalCount = bll.GetRowCount(TableView, filter);
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
            BLL.Security.UserBll Ubll = new BLL.Security.UserBll();
            dtUser = Ubll.GetUserList(pageIndex, pageSize, filter, OrderByFields);
            if (dtUser.Rows.Count == 0)
            {
                dtUser.Rows.Add(dtUser.NewRow());
                gvMain.DataSource = dtUser;
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
                this.gvMain.DataSource = dtUser;
                this.gvMain.DataBind();
            }

            ViewState["pageIndex"] = pageIndex;
            ViewState["totalCount"] = totalCount;
            ViewState["pageCount"] = pageCount;
            ViewState["filter"] = filter;
            ViewState["OrderByFields"] = OrderByFields;
        }
        #endregion

        #region 显示切换
        private void SwitchView(int index)
        {
            if (index == 0)
            {
                this.pnlList.Visible = true;
                this.pnlEdit.Visible = false;
            }
            else
            {
                this.pnlList.Visible = false;
                this.pnlEdit.Visible = true;
            }
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
                chk.Text = "操作";
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
                //chk.Attributes.Add("onclick","");
                LinkButton lkbtn = new LinkButton();
                lkbtn.CommandName = "Edit";
                lkbtn.ID = e.Row.ID;
                lkbtn.Text = " 编辑 ";
                if (this.hdnXGQX.Value == "0")
                {
                    lkbtn.Enabled = false;
                }
                e.Row.Cells[0].Controls.Add(chk);
                e.Row.Cells[0].Controls.Add(lkbtn);
                if (e.Row.Cells[1].Text.Trim() == "admin")
                {
                    chk.Enabled = false;
                    lkbtn.Enabled = false;
                }
            }
        }
        #endregion

        #region 数据编辑
        protected void gvMain_RowEditing(object sender, GridViewEditEventArgs e)
        {
            hdnOpFlag.Value = "1";
            ViewState["OpFlag"] = "1";
            SwitchView(1);
            this.txtUserID.Text = dtUser.Rows[e.NewEditIndex]["UserID"].ToString();
            this.txtUserName.Text = dtUser.Rows[e.NewEditIndex]["UserName"].ToString();
            this.txtEmployeeCode.Text = dtUser.Rows[e.NewEditIndex]["EmployeeCode"].ToString();
            //this.txtEmployeeName.Text = dsUser.Tables[0].Rows[e.NewEditIndex]["EmployeeName"].ToString(); 
            this.txtMemo.Text = dtUser.Rows[e.NewEditIndex]["Memo"].ToString();
        }
        #endregion

        #region 按字段查询
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                filter = string.Format("{0} like '%{1}%'", this.ddl_Field.SelectedValue, this.txtKeyWords.Text.Trim().Replace("'", ""));
                ViewState["filter"] = filter;
                if (rbASC.Checked)
                {
                    OrderByFields = this.ddl_Field.SelectedValue + " asc ";
                }
                else
                {
                    OrderByFields = this.ddl_Field.SelectedValue + " desc ";
                }

                totalCount = (int)bll.GetRowCount(TableView, filter);
                pageIndex = 1;
                pager.CurrentPageIndex = 1;
                pager.RecordCount = totalCount;
                GridDataBind();
            }
            catch (Exception exp)
            {
              WMS.App_Code.JScript.Instance.ShowMessage(this.UpdatePanel1, exp.Message);
            }
        }
        #endregion

        #region 新增用户
        protected void btnCreate_Click(object sender, EventArgs e)
        {
            ClearData();
            this.hdnOpFlag.Value = "0";
            ViewState["OpFlag"] = "0";
            SwitchView(1);
        }
        #endregion

        #region 删除用户
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {

                string strUserID = "-1,";
                for (int i = 0; i < gvMain.Rows.Count; i++)
                {
                    CheckBox chk = (CheckBox)gvMain.Rows[i].Cells[0].Controls[0];
                    if (gvMain.Rows[i].Cells[1].Text == "admin" && chk.Checked)
                    {
                      WMS.App_Code.JScript.Instance.ShowMessage(this.UpdatePanel1, "管理员帐号不能删除！");
                      continue;
                    }
                    if (chk.Enabled && chk.Checked)
                    {
                        strUserID += dtUser.Rows[i]["UserID"].ToString() + ",";                        
                    }
                }
                strUserID += "-1";


                bll.ExecNonQuery("Security.DeleteUser", new DataParameter[] { new DataParameter("{0}", strUserID) });
                AddOperateLog("用户管理", "删除用户信息");
                //for (int i = 0; i < empIdList.Count; i++)
                //{
                //    remoteDal.SaveDWV_IORG_PERSON(empIdList[i]);
                //}

                totalCount = bll.GetRowCount(TableView, filter);
                pager.RecordCount = totalCount;
                if (pageIndex > pager.PageCount)
                {
                    pageIndex = pager.PageCount;
                }
                GridDataBind();
            }
            catch (Exception exp)
            {
               WMS.App_Code.JScript.Instance.ShowMessage(this.UpdatePanel1, exp.Message);
                GridDataBind();
            }
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

        #region 数据保存
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                BLL.Security.UserBll ubll = new BLL.Security.UserBll();
                if (ViewState["OpFlag"].ToString() == "0")//新增
                {
                    DataTable dtTemp = ubll.GetUserInfo(this.txtUserName.Text.Trim());
                    if (dtTemp.Rows.Count > 0)
                    {
                        WMS.App_Code.JScript.Instance.ShowMessage(this.UpdatePanel1, "该用户名已经存在！");
                        return;
                    }
                    if (this.txtEmployeeCode.Text.Trim() == "")
                    {
                      txtEmployeeCode.Text = txtUserName.Text.Trim();
                    }
                    ubll.InsertUser(this.txtUserName.Text.Trim(), this.txtEmployeeCode.Text, this.txtMemo.Text);
                    pager.RecordCount = bll.GetRowCount(TableView, filter);
                    GridDataBind();
                    SwitchView(0);
                   WMS.App_Code.JScript.Instance.ShowMessage(this.UpdatePanel1, "数据添加成功！");
                    AddOperateLog("用户管理", "添加用户信息");
                }
                else//修改
                {
                    foreach (DataRow dr in dtUser.Rows)
                    {
                        if (dr["UserID"].ToString() == this.txtUserID.Text.Trim())
                        {
                            DataTable dtTemp =ubll. GetUserList(1, 10, string.Format("UserID<>{0} and UserName='{1}'", this.txtUserID.Text, this.txtUserName.Text.Trim()), OrderByFields);
                            if (dtTemp.Rows.Count > 0)
                            {
                                WMS.App_Code.JScript.Instance.ShowMessage(this.UpdatePanel1, "该用户名已经存在！");
                                return;
                            }

                            ubll.UpdateUser(this.txtUserName.Text.Trim(), this.txtEmployeeCode.Text.Trim(), this.txtMemo.Text.Trim(), int.Parse(this.txtUserID.Text));
                            break;
                        }
                    }
                    this.gvMain.EditIndex = -1;
                    GridDataBind();
                    WMS.App_Code.JScript.Instance.ShowMessage(this.UpdatePanel1, "数据修改成功！");
                    SwitchView(0);
                    AddOperateLog("用户管理", "修改用户信息");
                }

                
            }
            catch (Exception exp)
            {
                WMS.App_Code.JScript.Instance.ShowMessage(this.UpdatePanel1, exp.Message);
            }
        }
        #endregion

        #region 取消
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            this.gvMain.EditIndex = -1;
            ClearData();
            SwitchView(0);
            GridDataBind();
        }

        protected void ClearData()
        {
            this.txtUserID.Text = "";
            this.txtUserName.Text = "";
            this.txtEmployeeCode.Text = "";
            //this.txtEmployeeName.Text = "";
            this.txtMemo.Text = "";
        }
        #endregion
    }
}
