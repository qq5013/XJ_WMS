﻿using System;
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
                    DataTable dt = bll.FillDataTable("Cmd.SelectBillType", new DataParameter[] { new DataParameter("{0}", string.Format("BillTypeCode='{0}'", strID)) });
                    BindData(dt);
                    
                    SetTextReadOnly(this.txtID);
                }
                else
                {
                    this.txtID.Text = bll.GetNewID("CMD_BillType", "BillTypeCode", "1=1");
                    this.txtCreator.Text = Session["EmployeeCode"].ToString();
                    this.txtUpdater.Text = Session["EmployeeCode"].ToString();
                    this.txtCreatDate.Text = ToYMD(DateTime.Now);
                    this.txtUpdateDate.Text = ToYMD(DateTime.Now);

                }

                writeJsvar(FormID, SqlCmd, strID);
                SetTextReadOnly(this.txtCreator, this.txtCreatDate, this.txtUpdater, this.txtUpdateDate);

            }
        }

        private void BindDropDownList()
        {
            
        }


        private void BindData(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                this.txtID.Text = dt.Rows[0]["BillTypeCode"].ToString();
                this.txtBillTypeName.Text = dt.Rows[0]["BillTypeName"].ToString();
              
                this.txtMemo.Text = dt.Rows[0]["Memo"].ToString();
                this.txtCreator.Text = dt.Rows[0]["Creator"].ToString();
                this.txtCreatDate.Text = ToYMD(dt.Rows[0]["CreateDate"]);
                this.txtUpdater.Text = dt.Rows[0]["Updater"].ToString();
                this.txtUpdateDate.Text = ToYMD(dt.Rows[0]["UpdateDate"]);

            }
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
                                                                                new DataParameter("@BillTypeName", this.txtBillTypeName.Text.Trim()),
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
                bll.ExecNonQuery("Cmd.UpdateTrainType", new DataParameter[] {  new DataParameter("@BillTypeName", this.txtBillTypeName.Text.Trim()),
                                                                                 new DataParameter("@Memo", this.txtMemo.Text.Trim()) ,
                                                                                 new DataParameter("@Updater", Session["EmployeeCode"].ToString()),
                                                                                 new DataParameter("@BillTypeCode", this.txtID.Text.Trim())
                                                                               });
            }

            Response.Redirect(FormID + "View.aspx?SubModuleCode=" + SubModuleCode + "&FormID=" + Server.UrlEncode(FormID) + "&SqlCmd=" + SqlCmd + "&ID=" + Server.UrlEncode(this.txtID.Text));
        }
    }
}