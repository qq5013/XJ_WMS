using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using IDAL;
using System.Drawing;

namespace WMS.WebUI.Query
{
    public partial class BarCodeQuery : WMS.App_Code.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                btnSearch_Click(null, null);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string Barcode = this.txtSearch.Text.Replace("?", "_");
            BLL.BLLBase bll = new BLL.BLLBase();
            DataTable dtGroup = bll.FillDataTable("WMS.SelectBarCodeTable", new DataParameter[] { new DataParameter("@BarCode", Barcode) });

            if (dtGroup.Rows.Count == 0)
            {
                dtGroup.Rows.Add(dtGroup.NewRow());
                dgMain.DataSource = dtGroup;
                dgMain.DataBind();
                int columnCount = dgMain.Rows[0].Cells.Count;
                dgMain.Rows[0].Cells.Clear();
                dgMain.Rows[0].Cells.Add(new TableCell());
                dgMain.Rows[0].Cells[0].ColumnSpan = columnCount;
                dgMain.Rows[0].Cells[0].Text = "没有符合以上条件的数据,请重新查询 ";
                dgMain.Rows[0].Visible = true;
                BindDataSub("");

            }
            else
            {
                this.dgMain.DataSource = dtGroup;
                this.dgMain.DataBind();
                BindDataSub(dtGroup.Rows[0]["Barcode"].ToString());

            }
        }

        
        protected void dgMain_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.RowIndex == Convert.ToInt32(this.hdnRowIndex.Value))
                {
                    e.Row.BackColor = ColorTranslator.FromHtml("#60c0ff");
                }
                  DataRowView drv = e.Row.DataItem as DataRowView;
                  e.Row.Attributes.Add("onclick", string.Format("$('#hdnRowValue').val('{1}');selectRow({0});", e.Row.RowIndex, drv.Row.ItemArray[drv.DataView.Table.Columns.IndexOf("Barcode")].ToString()));
                e.Row.Attributes.Add("style", "cursor:pointer;");
            }
        }

        protected void btnReload_Click(object sender, EventArgs e)
        {
            int i = Convert.ToInt32(this.hdnRowIndex.Value);

            BindDataSub(this.hdnRowValue.Value);

            for (int j = 0; j < this.dgMain.Rows.Count; j++)
            {
                if (j % 2 == 0)
                    this.dgMain.Rows[j].BackColor = ColorTranslator.FromHtml("#ffffff");
                else
                    this.dgMain.Rows[j].BackColor = ColorTranslator.FromHtml("#E9F2FF");
                if (j == i)
                    this.dgMain.Rows[j].BackColor = ColorTranslator.FromHtml("#60c0ff");
            }
            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.UpdatePanel1.GetType(), "Resize", "content_resize();", true);
        }

        private void BindDataSub(string value)
        {
            BLL.BLLBase bll = new BLL.BLLBase();
            DataTable dtGroup = bll.FillDataTable("WMS.SelectBarCodeQuery", new DataParameter[] { new DataParameter("@BarCode", value) });

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
            }
            else
            {
                this.GridView1.DataSource = dtGroup;
                this.GridView1.DataBind();
            }
        }
    }
}