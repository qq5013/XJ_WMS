using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using IDAL;


namespace WMS.WebUI.InStock
{
    public partial class InStockEdit :App_Code.BasePage
    {
        protected string strID;
        BLL.BLLBase bll = new BLL.BLLBase();
        protected void Page_Load(object sender, EventArgs e)
        {
            strID = Request.QueryString["ID"] + "";

            if (!IsPostBack)
            {
                BindDropDownList();
                if (strID != "")
                {
                    DataTable dt = bll.FillDataTable("WMS.SelectBillMaster", new DataParameter[] { new DataParameter("{0}", string.Format("BillID='{0}'", strID)) });
                    BindData(dt);

                    SetTextReadOnly(this.txtID);
                }
                else
                {
                    BindDataSub();
                    this.txtID.Text = bll.GetNewID("CMD_BillType", "BillTypeCode", "1=1");
                    this.txtCreator.Text = Session["EmployeeCode"].ToString();
                    this.txtUpdater.Text = Session["EmployeeCode"].ToString();
                    this.txtCreatDate.Text = ToYMD(DateTime.Now);
                    this.txtUpdateDate.Text = ToYMD(DateTime.Now);
                }
            }
            ScriptManager.RegisterStartupScript(this.updatePanel1, this.updatePanel1.GetType(), "Resize", "resize();", true);
            writeJsvar(FormID, SqlCmd, strID);
            SetTextReadOnly(this.txtCreator, this.txtCreatDate, this.txtUpdater, this.txtUpdateDate);


        }

        private void BindDropDownList()
        {
            
        }


        private void BindData(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                this.txtID.Text = dt.Rows[0]["BillTypeCode"].ToString();
                 
              
                this.txtMemo.Text = dt.Rows[0]["Memo"].ToString();
                this.txtCreator.Text = dt.Rows[0]["Creator"].ToString();
                this.txtCreatDate.Text = ToYMD(dt.Rows[0]["CreateDate"]);
                this.txtUpdater.Text = dt.Rows[0]["Updater"].ToString();
                this.txtUpdateDate.Text = ToYMD(dt.Rows[0]["UpdateDate"]);
            }
            BindDataSub();
        }

        private void BindDataSub()
        {
            DataTable dt = bll.FillDataTable("WMS.SelectBillDetail", new DataParameter[] { new DataParameter("{0}", "1=1") });
            Session[FormID + "_Edit_dgViewSub1"] = dt;
            MovePage("Edit", this.dgViewSub1, 0, btnFirstSub1, btnPreSub1, btnNextSub1, btnLastSub1, btnToPageSub1, lblCurrentPageSub1);

        }
        protected void btnSave_Click(object sender, EventArgs e)
        {


            if (strID == "") //新增
            {
                int Count = bll.GetRowCount("CMD_BillType", string.Format("BillTypeCode='{0}'", this.txtID.Text));
                if (Count > 0)
                {
                    WMS.App_Code.JScript.Instance.ShowMessage(this.Page, "该类型编码已经存在！");
                    return;
                }

                bll.ExecNonQuery("Cmd.InsertBillType", new DataParameter[] { 
                                                                                new DataParameter("@BillTypeCode", this.txtID.Text.Trim()),
                                                                              
                                                                                new DataParameter("@Flag", "1"),
                                                                                new DataParameter("@TaskType", "11"),
                                                                                new DataParameter("@TaskLevel", 1),
                                                                                new DataParameter("@Memo", this.txtMemo.Text.Trim()),
                                                                                new DataParameter("@Creator", Session["EmployeeCode"].ToString()),
                                                                                new DataParameter("@Updater", Session["EmployeeCode"].ToString())
                                                                              });
            }
            else //修改
            {
                bll.ExecNonQuery("Cmd.UpdateTrainType", new DataParameter[] {   
                                                                                 new DataParameter("@Memo", this.txtMemo.Text.Trim()) ,
                                                                                 new DataParameter("@Updater", Session["EmployeeCode"].ToString()),
                                                                                 new DataParameter("@BillTypeCode", this.txtID.Text.Trim())
                                                                               });
            }

            Response.Redirect(FormID + "View.aspx?SubModuleCode=" + SubModuleCode + "&FormID=" + Server.UrlEncode(FormID) + "&SqlCmd=" + SqlCmd + "&ID=" + Server.UrlEncode(this.txtID.Text));
        }
        
        protected void btnAddDetail_Click(object sender, EventArgs e)
        {
            
        }
        protected void btnDelDetail_Click(object sender, EventArgs e)
        { 
        }

        protected void btnProduct_Click(object sender, EventArgs e)
        {

        }



        public override void UpdateTempSub(GridView dgv)
        {
            DataTable dt1 = (DataTable)Session[FormID + "_Edit_" + dgv.ID];
            if (dt1.Rows.Count == 0)
                return;
            DataRow dr;
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                dr = dt1.Rows[i + dgv.PageSize * dgv.PageIndex];
                dr.BeginEdit();


                dr.EndEdit();
            }
            dt1.AcceptChanges();
            Session[FormID + "_Edit_" + dgv.ID] = dt1;
        }

        protected void dgViewSub1_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
        #region 子表绑定

        protected void btnFirstSub1_Click(object sender, EventArgs e)
        {
            MovePage("Edit", this.dgViewSub1, 0, btnFirstSub1, btnPreSub1, btnNextSub1, btnLastSub1, btnToPageSub1, lblCurrentPageSub1);
        }

        protected void btnPreSub1_Click(object sender, EventArgs e)
        {
            MovePage("Edit", this.dgViewSub1, this.dgViewSub1.PageIndex - 1, btnFirstSub1, btnPreSub1, btnNextSub1, btnLastSub1, btnToPageSub1, lblCurrentPageSub1);
        }

        protected void btnNextSub1_Click(object sender, EventArgs e)
        {
            MovePage("Edit", this.dgViewSub1, this.dgViewSub1.PageIndex + 1, btnFirstSub1, btnPreSub1, btnNextSub1, btnLastSub1, btnToPageSub1, lblCurrentPageSub1);
        }

        protected void btnLastSub1_Click(object sender, EventArgs e)
        {
            MovePage("Edit", this.dgViewSub1, this.dgViewSub1.PageCount - 1, btnFirstSub1, btnPreSub1, btnNextSub1, btnLastSub1, btnToPageSub1, lblCurrentPageSub1);
        }

        protected void btnToPageSub1_Click(object sender, EventArgs e)
        {
            MovePage("Edit", this.dgViewSub1, int.Parse(this.txtPageNoSub1.Text) - 1, btnFirstSub1, btnPreSub1, btnNextSub1, btnLastSub1, btnToPageSub1, lblCurrentPageSub1);
        }

      
       
        #endregion

    }
}