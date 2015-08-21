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
                    dtCell = bll.FillDataTable("Cmd.SelectCell", new DataParameter[] { new DataParameter("{0}", "CELL_CODE='" + Request.QueryString["CMD_CELL_ID"] + "'") });
                    this.txtCELLID.Text = dtCell.Rows[0]["CMD_CELL_ID"].ToString();
                    this.txtShelfCode.Text = dtCell.Rows[0]["SHELF_CODE"].ToString();
                    this.txtCellCode.Text = dtCell.Rows[0]["CELL_CODE"].ToString();
                    this.txtCellName.Text = dtCell.Rows[0]["CELL_NAME"].ToString();
                    this.ddlActive.SelectedValue = dtCell.Rows[0]["IS_ACTIVE"].ToString();
                    
                    this.txtCellRows.Text = dtCell.Rows[0]["CELL_ROW"].ToString();
                    this.txtCellCols.Text = dtCell.Rows[0]["CELL_COLUMN"].ToString();
                    this.ddlLock.SelectedValue = dtCell.Rows[0]["IS_LOCK"].ToString();
                    this.ddlActive.SelectedValue = dtCell.Rows[0]["IS_ACTIVE"].ToString();
                    this.txtMemo.Text = dtCell.Rows[0]["MEMO"].ToString();
                    this.txtAreaCode.Text = dtCell.Rows[0]["AREA_CODE"].ToString();
                }
                else if (Request.QueryString["SHELFCODE"] != null)
                {
                    this.txtShelfCode.Text = Request.QueryString["SHELFCODE"];
                    this.txtAreaCode.Text = Request.QueryString["SHELFCODE"].Substring(0, 3);

                    int RowCount = bll.GetRowCount("CMD_WH_CELL", string.Format("SHELF_CODE='{0}'", Request.QueryString["SHELFCODE"])) + 1;
                    this.txtCellCode.Text = Request.QueryString["SHELFCODE"] + RowCount.ToString().PadLeft(3, '0');
                    this.txtCellCols.Text = RowCount.ToString();

                    //this.txtEGroup.Text = "0";
                    //this.txtECom.Text = "0";
                    //this.txtEAddress.Text = "0";
                     
                }
                SetTextReadOnly(this.txtShelfCode, this.txtCellCode, this.txtAreaCode, this.txtCellCols, this.txtCellRows);
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
                    int count = bll.GetRowCount("CMD_WH_CELL", string.Format("CELL_CODE='{0}'", this.txtCellCode.Text));
                    if (count > 0)
                    {
                        WMS.App_Code.JScript.Instance.ShowMessage(this, "此货位编码已存在，不能新增！");
                        return;
                    }
              
                     

                
                    string[] comds = new string[2];
                    comds[0] = "Cmd.InsertCell";
                    comds[1] = "Cmd.UpdateShelfColumnsByShelfCode";
                    List<DataParameter[]> paras = new List<DataParameter[]>();
                    paras.Add(new DataParameter[] {    new DataParameter("@CELL_CODE", this.txtCellCode.Text),
                                                                                new DataParameter("@CELL_NAME", this.txtCellName.Text.Trim().Replace("\'", "\''")),
                                                                                new DataParameter("@AREA_CODE",  this.txtAreaCode.Text),
                                                                                new DataParameter("@SHELF_CODE", this.txtShelfCode.Text.Trim()),
                                                                                new DataParameter("@CELL_ROW",  this.txtCellRows.Text),
                                                                                new DataParameter("@CELL_COLUMN", this.txtCellCols.Text.Trim()),
                                                                                new DataParameter("@IS_ACTIVE",   this.ddlActive.SelectedValue),
                                                                                new DataParameter("@IS_LOCK", this.ddlLock.SelectedValue),
                                                                                new DataParameter("@MEMO",  this.txtMemo.Text)});
                    paras.Add(new DataParameter[] { new DataParameter("@ShelfCode",this.txtShelfCode.Text.Trim()) });
                    bll.ExecTran(comds, paras);


                    this.btnSave.Enabled = false;

                    WMS.App_Code.JScript.Instance.RegisterScript(this, "ReloadParent();");
                    AddOperateLog("货位管理", "添加货位信息");
                }
                else//修改
                {
                    bll.ExecNonQuery("Cmd.UpdateCell", new DataParameter[] { new DataParameter("@CELL_CODE", this.txtCellCode.Text),
                                                                             new DataParameter("@CELL_NAME", this.txtCellName.Text.Trim().Replace("\'", "\''")),
                                                                             new DataParameter("@AREA_CODE",  this.txtAreaCode.Text),
                                                                             new DataParameter("@SHELF_CODE", this.txtShelfCode.Text.Trim()),
                                                                             new DataParameter("@CELL_ROW",  this.txtCellRows.Text),
                                                                             new DataParameter("@CELL_COLUMN", this.txtCellCols.Text.Trim()),
                                                                             new DataParameter("@IS_ACTIVE",   this.ddlActive.SelectedValue),
                                                                             new DataParameter("@IS_LOCK", this.ddlLock.SelectedValue),
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
            this.txtAreaCode.Text = "";
           

            this.btnSave.Enabled = true;
          
            string path = whcode + "/" + areacode + "/" + shelfCode + "/" + cellcode;
            WMS.App_Code.JScript.Instance.RegisterScript(this, "RefreshParent('" + path + "');");
            AddOperateLog("货位管理", "删除货位信息");
        }
    }
}