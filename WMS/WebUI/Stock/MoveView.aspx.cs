using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using IDAL;

namespace WMS.WebUI.Stock
{
    public partial class MoveView : WMS.App_Code.BasePage
    {
        protected string FormID;
        protected string ID;
        protected string TableName = "WMS_Move";
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
                DataTable dt = bll.FillDataTable("WMS.SelectMove", new DataParameter[] { new DataParameter("{0}", string.Format("{0}='{1}' and Flag={2}", PrimaryKey, ID, Flag)) });
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
                 
                this.txtMemo.Text = dt.Rows[0]["Memo"].ToString();
                this.txtCreator.Text = dt.Rows[0]["Creator"].ToString();
                this.txtCreateDate.Text = ToYMD(dt.Rows[0]["CreateDate"]);
                hdnState.Value = dt.Rows[0]["State"].ToString();
                this.txtChecker.Text = dt.Rows[0]["Checker"].ToString();
                this.txtCheckDate.Text = ToYMD(dt.Rows[0]["CheckDate"]);

                if (this.txtChecker.Text != "")
                    this.btnCheck.Text = "反审";
                else
                    this.btnCheck.Text = "审核";
                
                
               
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

            if (int.Parse(hdnState.Value) >= 3)
            {
                this.btnDelete.Enabled = false;
                this.btnEdit.Enabled = false;
                this.btnCheck.Enabled = false;
                this.btnTask.Enabled = false;
            }
            else
            {
                if (int.Parse(hdnState.Value) == 2) //出库作业
                {
                    this.btnTask.Text = "取消作业";
                    this.btnCheck.Text = "反审";
                    this.btnCheck.Enabled = false;
                    this.btnTask.Enabled = blnReCheck;
                    this.btnDelete.Enabled = false;
                    this.btnEdit.Enabled = false;

                }
                else if (int.Parse(hdnState.Value) == 1) //审核
                {
                    this.btnTask.Text = "移库作业";
                    this.btnCheck.Text = "反审";
                    this.btnCheck.Enabled = blnCheck;
                    this.btnTask.Enabled = blnReCheck;
                    this.btnDelete.Enabled = false;
                    this.btnEdit.Enabled = false;
                }
                else
                {
                    this.btnTask.Text = "移库作业";
                    this.btnCheck.Text = "审核";
                    this.btnTask.Enabled = false;
                    this.btnCheck.Enabled = blnCheck;

                    this.btnDelete.Enabled = blnDelete;
                    this.btnEdit.Enabled = blnEdit;
                }
            }

        }



        private void BindDataSub()
        {

            DataTable dt = bll.FillDataTable("WMS.SelectMoveSub", new DataParameter[] { new DataParameter("{0}", string.Format("BillID='{0}' and Flag={1}", this.txtID.Text, Flag)) });
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
            string strID = this.txtID.Text;
            string[] comds = new string[2];
            comds[0] = "WMS.DeleteMove";
            comds[1] = "WMS.DeleteMoveSub";
            List<DataParameter[]> paras = new List<DataParameter[]>();
            paras.Add(new DataParameter[] { new DataParameter("{0}", string.Format("BillID='{0}' and Flag={1}", strID, Flag)) });
            paras.Add(new DataParameter[] { new DataParameter("{0}", string.Format("BillID='{0}' and Flag={1}", strID, Flag)) });
            bll.ExecTran(comds, paras);
            AddOperateLog("管理移库单", "删除单号：" + strID);
            btnNext_Click(sender, e);
            if (this.txtID.Text == strID)
                btnPre_Click(sender, e);
        }

        #region ButtonClick
        protected void btnCheck_Click(object sender, EventArgs e)
        {

            DataParameter[] paras = new DataParameter[4];
            if (this.btnCheck.Text == "审核")
            {
                paras[0] = new DataParameter("@Checker", Session["EmployeeCode"].ToString());
                paras[1] = new DataParameter("{1}", "getdate()");
                paras[2] = new DataParameter("{2}", "1");
            }
            else
            {
                paras[0] = new DataParameter("@Checker", "");
                paras[1] = new DataParameter("{1}", null);
                paras[2] = new DataParameter("{2}", "0");
            }
            paras[3] = new DataParameter("{0}", string.Format("BillID='{0}' and Flag={1}", this.txtID.Text, Flag));


            bll.ExecNonQuery("WMS.UpdateMoveCheck", paras);
            AddOperateLog("移出单作业审核", btnCheck.Text+":"+this.txtID.Text);
            DataTable dt = bll.FillDataTable("WMS.SelectMove", new DataParameter[] { new DataParameter("{0}", string.Format("{0}='{1}' and Flag={2}", PrimaryKey, this.txtID.Text, Flag)) });
            BindData(dt);
        }
        //移库作业
        protected void btnTask_Click(object sender, EventArgs e)
        {

            try
            {
                DataParameter[] paras = new DataParameter[2];
                string commands = "";
                if (this.btnTask.Text == "移库作业")
                {
                    paras[0] = new DataParameter("@BillID", this.txtID.Text);
                    paras[1] = new DataParameter("@UserName", Session["EmployeeCode"].ToString());
                    commands = "WMS.SPMoveTask";
                }
                else
                {
                    paras[0] = new DataParameter("@BillID", this.txtID.Text);
                    paras[1] = null;
                    commands = "WMS.SPMoveUnTask";
                }



                bll.ExecNonQueryTran(commands, paras);
                if (this.btnTask.Text == "移库作业")
                {
                    WMS.App_Code.JScript.Instance.ShowMessage(this.updatePanel, "移库作业完成，请查按照移库通知单进行操作。");
                }
                AddOperateLog("移入移出单作业", btnTask.Text + ":" + this.txtID.Text);
                DataTable dt = bll.FillDataTable("WMS.SelectMove", new DataParameter[] { new DataParameter("{0}", string.Format("{0}='{1}' and Flag={2}", PrimaryKey, this.txtID.Text, Flag)) });
                BindData(dt);
            }
            catch (Exception ex)
            {
                WMS.App_Code.JScript.Instance.ShowMessage(this.updatePanel,ex.Message);
                DataTable dt = bll.FillDataTable("WMS.SelectMove", new DataParameter[] { new DataParameter("{0}", string.Format("{0}='{1}' and Flag={2}", PrimaryKey, this.txtID.Text, Flag)) });
                BindData(dt);
                return;
            }



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

       

       

       
    }
}