using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using IDAL;
using System.Configuration;


namespace WMS.Index
{
    public partial class ShowMsg : App_Code.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int MsgInstockTime = int.Parse(ConfigurationManager.AppSettings["MsgInstockTime"]);
            int MsgOutstockTime = int.Parse(ConfigurationManager.AppSettings["MsgOutstockTime"]);
            int MsgCellCount = int.Parse(ConfigurationManager.AppSettings["MsgCellCount"]);

            BLL.BLLBase bll = new BLL.BLLBase();
            DataTable dtMsg = bll.FillDataTable("WMS.SelectMsg", new DataParameter[] { new DataParameter("@MsgInstockTime", MsgInstockTime), 
                                                                                       new DataParameter("@MsgOutstockTime", MsgOutstockTime),
                                                                                       new DataParameter("@MsgCellCount", MsgCellCount) });

            string strMsg = "";
            if (dtMsg.Rows.Count > 0)
                strMsg = "<table style=\"widht:100%\">";
            DataRow[] drs = dtMsg.Select("Flag=1");
            if (drs.Length > 0)
            {
                strMsg += "<tr><td colspan=\"4\"><b>未入库提示:</b></td></tr>";
                for (int i = 0; i < drs.Length; i++)
                {
                    strMsg += "<tr><td>&nbsp;&nbsp;&nbsp;&nbsp;采购订单单号:" + drs[i]["BillID"].ToString() + "</td><td>&nbsp;&nbsp;产品名称：" + drs[i]["ProductName"].ToString() + "(" + drs[i]["ProductModel"].ToString() + ")" + drs[i]["ColorName"].ToString() + "</td><td>&nbsp;&nbsp;预入库日：" + ToYMD(drs[i]["PlanDate"]) + "</td><td>&nbsp;&nbsp;未入库数量:" + drs[i]["NotCount"].ToString() + "</td></tr>";
                }
            }
            drs = dtMsg.Select("Flag=2");
            if (drs.Length > 0)
            {
                strMsg += "<tr><td colspan=\"4\"><b>未出库提示:</b></td></tr>";
                for (int i = 0; i < drs.Length; i++)
                {
                    strMsg += "<tr><td>&nbsp;&nbsp;&nbsp;&nbsp;销售订单单号：" + drs[i]["BillID"].ToString() + "</td><td>&nbsp;&nbsp;产品名称：" + drs[i]["ProductName"].ToString() + "(" + drs[i]["ProductModel"].ToString() + ")" + drs[i]["ColorName"].ToString() + "</td><td>&nbsp;&nbsp;预出库日：" + ToYMD(drs[i]["PlanDate"]) + "</td><td>&nbsp;&nbsp;未出库数量:" + drs[i]["NotCount"].ToString() + "</td></tr>";
                }
            }
            drs = dtMsg.Select("Flag=3");
            if(drs.Length>0)
            {
                strMsg += "<tr><td colspan=\"4\"><b>移库提示:</b></td></tr>";
                for (int i = 0; i < drs.Length; i++)
                {
                    strMsg += "<tr><td>&nbsp;&nbsp;&nbsp;&nbsp;货架：" + drs[i]["BillID"].ToString() + "</td><td>&nbsp;&nbsp;产品名称：" + drs[i]["ProductName"].ToString() + "(" + drs[i]["ProductModel"].ToString() + ")  " + drs[i]["ColorName"].ToString() + "</td><td colspan=\"2\"></td></tr>";
                }
            }
            if(dtMsg.Rows.Count>0)
            strMsg += "</table>";
            ShowMsgDiv.InnerHtml = strMsg;
        }
    }
}