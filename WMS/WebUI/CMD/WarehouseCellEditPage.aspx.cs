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
using IDAL;
using System.Collections.Generic;

namespace WMS.WebUI.CMD
{
    public partial class WarehouseCellEditPage :WMS.App_Code.BasePage
    {
       
        DataTable dtCell;
        BLL.BLLBase bll = new BLL.BLLBase();
        private string TableName;
        private string ColumnName;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["CMD_CELL_ID"] != null)
                {
                    dtCell = bll.FillDataTable("Cmd.SelectCell", new DataParameter[] { new DataParameter("{0}", "CellCode='" + Request.QueryString["CMD_CELL_ID"] + "'") });
                    this.txtCELLID.Text = dtCell.Rows[0]["CellCode"].ToString();
                    this.txtShelfID.Text = dtCell.Rows[0]["ShelfCode"].ToString();
                    this.txtAreaID.Text = dtCell.Rows[0]["AreaCode"].ToString();

                    this.txtShelfName.Text = bll.GetFieldValue("CMD_Shelf", "ShelfName", "ShelfCode='" + this.txtShelfID.Text + "'");
                    this.txtAreaName.Text = bll.GetFieldValue("CMD_Area", "AreaName", "AreaCode='" + this.txtAreaID.Text + "'"); 

                    this.txtCellCode.Text = dtCell.Rows[0]["CellCode"].ToString();
                    this.txtCellName.Text = dtCell.Rows[0]["CellName"].ToString();
                    this.ddlActive.SelectedValue = dtCell.Rows[0]["IsActive"].ToString();

                    this.txtCellRows.Text = dtCell.Rows[0]["CellRow"].ToString();
                    this.txtCellCols.Text = dtCell.Rows[0]["CellColumn"].ToString();
                    this.ddlLock.SelectedValue = dtCell.Rows[0]["IsLock"].ToString();
              
                    this.txtMemo.Text = dtCell.Rows[0]["MEMO"].ToString();
                   
                }
                else if (Request.QueryString["SHELFCODE"] != null)
                {
                    this.txtShelfID.Text = Request.QueryString["SHELFCODE"];
                    this.txtAreaID.Text = bll.GetFieldValue("CMD_Shelf", "AreaCode", "ShelfCode='" + this.txtShelfID.Text + "'");

                    this.txtShelfName.Text = bll.GetFieldValue("CMD_Shelf", "ShelfName", "ShelfCode='" + this.txtShelfID.Text + "'");
                    this.txtAreaName.Text = bll.GetFieldValue("CMD_Area", "AreaName", "AreaCode='" + this.txtAreaID.Text + "'"); 

                    int RowCount = bll.GetRowCount("CMD_WH_CELL", string.Format("SHELF_CODE='{0}'", Request.QueryString["SHELFCODE"])) + 1;
                    this.txtCellCode.Text = Request.QueryString["SHELFCODE"] + RowCount.ToString().PadLeft(3, '0');
                    this.txtCellCols.Text = RowCount.ToString();
                    this.txtCELLID.Text = "";
                    //this.txtEGroup.Text = "0";
                    //this.txtECom.Text = "0";
                    //this.txtEAddress.Text = "0";
                     
                }
                SetTextReadOnly(this.txtAreaName, this.txtCellCode, this.txtShelfName, this.txtCellCols, this.txtCellRows);
            }
            else
            {

            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.txtCELLID.Text.Trim().Length == 0)//新增
                {
                    int count = bll.GetRowCount("CMD_WH_CELL", string.Format("CellCode='{0}'", this.txtCellCode.Text));
                    if (count > 0)
                    {
                        WMS.App_Code.JScript.Instance.ShowMessage(this, "此货位编码已存在，不能新增！");
                        return;
                    }
                    string comds = "Cmd.InsertCell";


                    DataParameter[] paras = new DataParameter[] {    new DataParameter("@CellCode", this.txtCellCode.Text),
                                                       new DataParameter("@CellName", this.txtCellName.Text.Trim().Replace("\'", "\''")),
                                                       new DataParameter("@AreaCode",  this.txtAreaID.Text),
                                                       new DataParameter("@ShelfCode", this.txtShelfID.Text.Trim()),
                                                       new DataParameter("@CellRow",  this.txtCellRows.Text),
                                                       new DataParameter("@CellColumn", this.txtCellCols.Text.Trim()),
                                                       new DataParameter("@IsActive",   this.ddlActive.SelectedValue),
                                                       new DataParameter("@IsLock", this.ddlLock.SelectedValue),
                                                       new DataParameter("@Memo",  this.txtMemo.Text)};
                    
                    bll.ExecNonQuery(comds, paras);


                    this.btnSave.Enabled = false;

                    WMS.App_Code.JScript.Instance.RegisterScript(this, "ReloadParent();");
                    AddOperateLog("货位管理", "添加货位信息");
                }
                else//修改
                {
                    bll.ExecNonQuery("Cmd.UpdateCell", new DataParameter[] { 
                                                                             new DataParameter("@CellName", this.txtCellName.Text.Trim().Replace("\'", "\''")),
                                                                             new DataParameter("@AreaCode",  this.txtAreaID.Text),
                                                                             new DataParameter("@ShelfCode", this.txtShelfID.Text.Trim()),
                                                                             new DataParameter("@CellRow",  this.txtCellRows.Text),
                                                                             new DataParameter("@CellColumn", this.txtCellCols.Text.Trim()),
                                                                             new DataParameter("@IsActive",   this.ddlActive.SelectedValue),
                                                                             new DataParameter("@IsLock", this.ddlLock.SelectedValue),
                                                                             new DataParameter("@MEMO",  this.txtMemo.Text),new DataParameter("{0}",this.txtCELLID.Text)});

                    WMS.App_Code.JScript.Instance.RegisterScript(this, "UpdateParent();");
                    AddOperateLog("货位管理", "修改货位信息");
                }
               
            }
            catch (Exception exp)
            {
               WMS.App_Code.JScript.Instance.ShowMessage(this, exp.Message);
            }
        }


        protected void btnCancel_Click(object sender, EventArgs e)
        {
            WMS.App_Code.JScript.Instance.RegisterScript(this, "window.close();");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            dtCell = bll.FillDataTable("Cmd.SelectCell", new DataParameter[] { new DataParameter("{0}", "CMD_CELL_ID='" + txtCELLID.Text + "'") });

            if (dtCell.Rows[0]["QUANTITY"].ToString() != "")
            {
                decimal qty = Convert.ToDecimal(dtCell.Rows[0]["QUANTITY"].ToString());
                if (qty > 0)
                {
                   WMS.App_Code.JScript.Instance.ShowMessage(this, "该货位正在使用，不能删除！");
                    return;
                }
            }

            string whcode = dtCell.Rows[0]["WAREHOUSE_CODE"].ToString();
            string areacode = dtCell.Rows[0]["AREA_CODE"].ToString();
            string shelfCode = dtCell.Rows[0]["SHELF_CODE"].ToString();
            string cellcode = dtCell.Rows[0]["CELL_CODE"].ToString();
            string cellid = this.txtCELLID.Text;
            
            dtCell = bll.FillDataTable("Cmd.DeleteCell", new DataParameter[] { new DataParameter("{0}", this.txtCELLID.Text) });
            this.txtCELLID.Text = "";
             
            this.txtCellCode.Text = "";
            this.txtCellName.Text = "";
            this.ddlActive.SelectedIndex = 0;
            this.txtAreaID.Text = "";
           

            this.btnSave.Enabled = true;
          
            string path = whcode + "/" + areacode + "/" + shelfCode + "/" + cellcode;
            WMS.App_Code.JScript.Instance.RegisterScript(this, "RefreshParent('" + path + "');");
            AddOperateLog("货位管理", "删除货位信息");
        }
    }
}