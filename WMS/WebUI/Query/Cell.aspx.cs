using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace WMS.WebUI.Query
{

    public partial class Cell : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            string ShelfCode = Request.QueryString["ShelfCode"].ToString();
            string AreaCode = Request.QueryString["AreaCode"].ToString();
            string ShelfWhere = Request.QueryString["ShelfWhere"].ToString();
            BLL.BLLQuery bll = new BLL.BLLQuery();

            DataTable tableCell = bll.GetWareHouseQuery(ShelfCode, AreaCode, ShelfWhere);
            ShowCellChart(tableCell);
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Resize", "content_resize();", true);
        }

        #region 显示货位图表
        protected void ShowCellChart(DataTable tableCell)
        {
            this.pnlCell.Controls.Clear();
            if (tableCell.Rows.Count == 0)
                return;
           
            Table tableShelf = new Table();
            tableShelf.Attributes.Add("style", "width:1400px;");
            TableRow trShelf = new TableRow();
            string oldAreaCode = tableCell.Rows[0]["SHELF_CODE"].ToString();
            string newAreaCode = tableCell.Rows[0]["SHELF_CODE"].ToString();
            for (int i = 0; i < tableCell.Rows.Count; i++)
            {
                TableCell tc = new TableCell();
                DataRow dr = tableCell.Rows[i];
                int ColumnCount=int.Parse(dr["COLUMN_COUNT"].ToString());
                #region 插入货架信息
                if (i == 0)
                {
                    TableCell tcNew = new TableCell();
                    tcNew.Text = dr["SHELF_NAME"].ToString()  +"   存放产品:"+ dr["PRODUCT_PartsName"].ToString() + " (" + dr["PRODUCT_MODEL"].ToString() + ") " + dr["COLOR_NAME"].ToString();
                    tcNew.Attributes.Add("style", "font-size: 12px");
                    tcNew.ColumnSpan = ColumnCount;
                    tcNew.Height = 6;
                    tcNew.Font.Bold = true;
                    oldAreaCode = newAreaCode;
                    trShelf.Controls.Add(tcNew);
                    tableShelf.Controls.Add(trShelf);
                    trShelf = new TableRow();
                }
                #endregion
                #region 层数换行
                if (i % ColumnCount == 0 && i > 0)
                {
                    tableShelf.Controls.Add(trShelf);
                    trShelf = new TableRow();
                }
                #endregion
                #region 货架判断
                newAreaCode = dr["SHELF_CODE"].ToString();
                if (oldAreaCode != newAreaCode)
                {
                    TableCell tcNew = new TableCell();
                    tcNew.Text = dr["SHELF_NAME"].ToString() +"  存放产品:"+ dr["PRODUCT_PartsName"].ToString() + " (" + dr["PRODUCT_MODEL"].ToString() + ") " + dr["COLOR_NAME"].ToString();
                    tcNew.Attributes.Add("style", "font-size: 12px");
                    tcNew.ColumnSpan = ColumnCount;
                    tcNew.Height = 6;
                    tcNew.Font.Bold = true;
                    oldAreaCode = newAreaCode;
                    trShelf.Controls.Add(tcNew);
                    tableShelf.Controls.Add(trShelf);
                    trShelf = new TableRow();
                }
                #endregion
              

                #region 插入货位信息
                if (dr["IS_ACTIVE"].ToString() == "0")
                {
                    tc.Text = string.Format("{0}<br/>未启用", dr["CELL_Name"].ToString());
                    tc.Attributes.Add("style", "background-color: #C0C0C0;cursor:default;font-size: 12px ;text-align:center");
                }
                else
                {
                    if (dr["IS_LOCK"].ToString() == "1")
                    {
                        tc.Text = string.Format("{0}<br/>" + (dr["IN_DATE"].ToString().Length > 0 ? "已入锁定" : "未入锁定"), dr["CELL_Name"].ToString());
                        tc.Attributes.Add("style", "background-color: #FF0000;cursor:hand;font-size: 12px ;text-align:center");
                        tc.ToolTip = string.Format("产品数量：{0}\r\n箱 条 码 ：{1}", dr["QUANTITY"].ToString(), dr["PALLET_CODE"].ToString());
                    }
                    else
                    {
                        if (dr["IN_DATE"].ToString().Length > 0)
                        {
                            tc.Text = string.Format("{0}<br/>已装货", dr["CELL_Name"].ToString());
                            tc.Attributes.Add("style", "background-color: #0066FF;cursor:hand;font-size: 12px ;text-align:center");
                            tc.ToolTip = string.Format("产品数量：{0}\r\n箱 条 码 ：{1}", dr["QUANTITY"].ToString(), dr["PALLET_CODE"].ToString());
                        }
                        else
                        {
                            tc.Text = string.Format("{0}<br/>空货位", dr["CELL_Name"].ToString());
                            tc.Attributes.Add("style", "background-color: #00ffc0;cursor:hand;font-size: 12px ;text-align:center");
                        }
                    }
                }
                #endregion
                trShelf.Controls.Add(tc);
            }
            tableShelf.Controls.Add(trShelf);
            this.pnlCell.Controls.Add(tableShelf);
        }
        #endregion
    }
}



 