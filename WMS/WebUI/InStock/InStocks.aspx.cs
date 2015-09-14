using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using IDAL;
using System.Drawing;

namespace WMS.WebUI.InStock
{

    public partial class InStocks : App_Code.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["filter"] = "BillID like 'IS%'";
                ViewState["CurrentPage"] = 1;

                try
                {
                   DataTable dt= SetBtnEnabled(int.Parse(ViewState["CurrentPage"].ToString()),SqlCmd, ViewState["filter"].ToString(), pageSize, GridView1, btnFirst, btnPre, btnNext, btnLast, btnToPage, lblCurrentPage, this.UpdatePanel1);
                   SetBindDataSub(dt);
                }
                catch (Exception exp)
                {
                    WMS.App_Code.JScript.Instance.ShowMessage(this.UpdatePanel1, exp.Message);
                }

               
            }
            writeJsvar(FormID, SqlCmd, "");
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.UpdatePanel1.GetType(), "Resize", "resize();", true);


        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.RowIndex == Convert.ToInt32(this.hdnRowIndex.Value))
                {
                    e.Row.BackColor = ColorTranslator.FromHtml("#60c0ff");
                }
                e.Row.Attributes.Add("onclick", string.Format("$('#hdnRowValue').val('{1}');selectRow({0});", e.Row.RowIndex, ((HyperLink)e.Row.Cells[2].FindControl("HyperLink1")).Text));
                e.Row.Attributes.Add("style", "cursor:pointer;");
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {

            try
            {
                ViewState["filter"] = " BillID like 'IS%' " + " and " + string.Format("{0} like '%{1}%'", this.ddlField.SelectedValue, this.txtSearch.Text.Trim().Replace("'", ""));
                ViewState["CurrentPage"] = 1;
                SetBtnEnabled(int.Parse(ViewState["CurrentPage"].ToString()), SqlCmd, ViewState["filter"].ToString(), pageSize, GridView1, btnFirst, btnPre, btnNext, btnLast, btnToPage, lblCurrentPage, this.UpdatePanel1);

            }
            catch (Exception exp)
            {
                WMS.App_Code.JScript.Instance.ShowMessage(this.UpdatePanel1, exp.Message);
            }
        }

        protected void btnDeletet_Click(object sender, EventArgs e)
        {
            string strColorCode = "'-1',";
            BLL.BLLBase bll = new BLL.BLLBase();
            for (int i = 0; i < this.GridView1.Rows.Count; i++)
            {
                CheckBox cb = (CheckBox)(this.GridView1.Rows[i].FindControl("cbSelect"));
                if (cb != null && cb.Checked)
                {
                    HyperLink hk = (HyperLink)(this.GridView1.Rows[i].FindControl("HyperLink1"));
                    //判断能否删除
                    int Count = bll.GetRowCount("VUsed_CMD_BillType", string.Format("BillTypeCode='{0}'", hk.Text));
                    if (Count > 0)
                    {
                        WMS.App_Code.JScript.Instance.ShowMessage(this.UpdatePanel1, GridView1.Rows[i].Cells[2].Text + "入库单号被其它单据使用，请调整后再删除！");
                        return;
                    }

                    strColorCode += "'" + hk.Text + "',";
                }
            }
            strColorCode += "'-1'";


            string[] comds = new string[2];
            comds[0] = "WMS.DeleteBillMaster";
            comds[1] = "WMS.DeleteBillDetail";
            List<DataParameter[]> paras = new List<DataParameter[]>();
            paras.Add(new DataParameter[] { new DataParameter("{0}", strColorCode) });
            paras.Add(new DataParameter[] { new DataParameter("{0}", string.Format("BillID in ({0})", strColorCode)) });
            bll.ExecTran(comds, paras);
 
            AddOperateLog("入库单", "删除单号：" + strColorCode.Replace("'-1',", "").Replace(",'-1'", ""));
            DataTable dt = SetBtnEnabled(int.Parse(ViewState["CurrentPage"].ToString()), SqlCmd, ViewState["filter"].ToString(), pageSize, GridView1, btnFirst, btnPre, btnNext, btnLast, btnToPage, lblCurrentPage, this.UpdatePanel1);
            SetBindDataSub(dt);
        }
        #region 主档事件
        protected void btnFirst_Click(object sender, EventArgs e)
        {
            ViewState["CurrentPage"] = 1;
           DataTable dt= SetBtnEnabled(int.Parse(ViewState["CurrentPage"].ToString()), SqlCmd, ViewState["filter"].ToString(), pageSize, GridView1, btnFirst, btnPre, btnNext, btnLast, btnToPage, lblCurrentPage, this.UpdatePanel1);
           SetBindDataSub(dt);
        }
      

        protected void btnPre_Click(object sender, EventArgs e)
        {
            ViewState["CurrentPage"] = int.Parse(ViewState["CurrentPage"].ToString()) - 1;
           DataTable dt= SetBtnEnabled(int.Parse(ViewState["CurrentPage"].ToString()), SqlCmd, ViewState["filter"].ToString(), pageSize, GridView1, btnFirst, btnPre, btnNext, btnLast, btnToPage, lblCurrentPage, this.UpdatePanel1);
           SetBindDataSub(dt);
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            ViewState["CurrentPage"] = int.Parse(ViewState["CurrentPage"].ToString()) + 1;
           DataTable dt= SetBtnEnabled(int.Parse(ViewState["CurrentPage"].ToString()), SqlCmd, ViewState["filter"].ToString(), pageSize, GridView1, btnFirst, btnPre, btnNext, btnLast, btnToPage, lblCurrentPage, this.UpdatePanel1);
           SetBindDataSub(dt);
        }

        protected void btnLast_Click(object sender, EventArgs e)
        {
            ViewState["CurrentPage"] = 0;
            DataTable dt= SetBtnEnabled(int.Parse(ViewState["CurrentPage"].ToString()), SqlCmd, ViewState["filter"].ToString(), pageSize, GridView1, btnFirst, btnPre, btnNext, btnLast, btnToPage, lblCurrentPage, this.UpdatePanel1);
            SetBindDataSub(dt);
        }

        protected void btnToPage_Click(object sender, EventArgs e)
        {
            int PageIndex = 0;
            int.TryParse(this.txtPageNo.Text, out PageIndex);
            if (PageIndex == 0)
                PageIndex = 1;

            ViewState["CurrentPage"] = PageIndex;
            DataTable dt = SetBtnEnabled(int.Parse(ViewState["CurrentPage"].ToString()), SqlCmd, ViewState["filter"].ToString(), pageSize, GridView1, btnFirst, btnPre, btnNext, btnLast, btnToPage, lblCurrentPage, this.UpdatePanel1);
            SetBindDataSub(dt);
        }

        private void SetBindDataSub(DataTable dt)
        {
            string BillID = "";
            if (dt.Rows.Count > 0)
            {
                BillID = dt.Rows[0]["BillID"].ToString(); 
            }
            BLL.BLLBase bll = new BLL.BLLBase();
            DataTable dtSub = bll.FillDataTable("WMS.SelectBillDetail", new DataParameter[] { new DataParameter("{0}", string.Format("BillID='{0}'", BillID)) });
            Session[FormID + "_S_GridView2"] = dtSub;
            this.GridView2.DataSource = dtSub;
            this.GridView2.DataBind();
            MovePage("S", this.GridView2, 0, btnFirstSub, btnPreSub, btnNextSub, btnLastSub, btnToPageSub, lblPageSub);
        }
        private void BindDataSub(string BillID)
        {
            BLL.BLLBase bll = new BLL.BLLBase();
            DataTable dtSub = bll.FillDataTable("WMS.SelectBillDetail", new DataParameter[] { new DataParameter("{0}", string.Format("BillID='{0}'", BillID)) });
            Session[FormID + "_S_GridView2"] = dtSub;
            this.GridView2.DataSource = dtSub;
            this.GridView2.DataBind();
            MovePage("S", this.GridView2, 0, btnFirstSub, btnPreSub, btnNextSub, btnLastSub, btnToPageSub, lblPageSub);
        }
       
        #endregion


        protected void btnReload_Click(object sender, EventArgs e)
        {
            int i = Convert.ToInt32(this.hdnRowIndex.Value);
            BindDataSub(this.hdnRowValue.Value);
            for (int j = 0; j < this.GridView1.Rows.Count; j++)
            {
                if (j % 2 == 0)
                    this.GridView1.Rows[j].BackColor = ColorTranslator.FromHtml("#ffffff");
                else
                    this.GridView1.Rows[j].BackColor = ColorTranslator.FromHtml("#E9F2FF");
                if (j == i)
                    this.GridView1.Rows[j].BackColor = ColorTranslator.FromHtml("#60c0ff");
            }
         
        }

       
    }
}