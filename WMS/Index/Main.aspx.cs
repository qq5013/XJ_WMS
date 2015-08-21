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
    public partial class Main : System.Web.UI.Page
    {

        #region 变量

        DataTable dtDestopItem;

        Table tb;
        Panel pl;
        Hashtable GlobalMenuTitle;//菜单标题
        Hashtable GlobalMenuLink;//菜单链接地址
        Hashtable GlobalMenuParent;//菜单父标题

        Hashtable GlobalModuleID;//主键

        int iTableCount;
        int iCount = 0;
        int iCountColor = 0;
        #endregion
        #region 页面加载事件
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Session["IsFirstLogin"] != null)
            //{
            //    if (Session["IsFirstLogin"].ToString() == "1")
            //    {
            //    }
            //    else
            //    {
            if (!IsPostBack)
            {
                GC.Collect();
                Response.ExpiresAbsolute = DateTime.Now;
                try
                {
                    string str = "<script> " +
                                 "try " +
                                 "{ " +
                                 " var nav=window.parent.frames.Navigation.document.getElementById('labNavigation');" +
                                 " nav.innerText='快速通道';" +
                                 "} " +
                                 "catch(e) " +
                                 "{ " +
                                 "} " +
                                 "</script>";
                    this.ClientScript.RegisterClientScriptBlock(this.GetType(), DateTime.Now.ToLongTimeString(), str);


                    #region 提醒
                    //bool show = false;
                    //Alarm objAlarm = new Alarm();
                    //DataSet dsRemind = objAlarm.GetRemindList();
                    //if (dsRemind.Tables[0].Rows.Count > 0)
                    //{
                    //    show = true;
                    //    TableRow tr = new TableRow();
                    //    TableCell tc = new TableCell();
                    //    tc.Text = string.Format("<a href='code/StorageManagement/StorageRemindPage.aspx'>有:<font color='red'>{0}条</font>库存预警信息</a>", dsRemind.Tables[0].Rows.Count);
                    //    tr.Controls.Add(tc);
                    //    this.tblRemind.Controls.Add(tr);
                    //}

                    //if (show)
                    //{
                    //    this.pnlRemind.Visible = true;
                    //}
                    //else
                    //{
                    //    this.pnlRemind.Visible = false;
                    //}
                    #endregion
                }
                catch (Exception ex)
                {
                    //Session["ModuleName"] = "MainPage.aspx";
                    //Session["FunctionName"] = "Page_Load事件";
                    //Session["ExceptionalType"] = ex.GetType().FullName;
                    //Session["ExceptionalDescription"] = ex.Message;
                    //Response.Redirect("Common/MistakesPage.aspx");
                }

                int MsgInstockTime = int.Parse(ConfigurationManager.AppSettings["MsgInstockTime"]);
                int MsgOutstockTime = int.Parse(ConfigurationManager.AppSettings["MsgOutstockTime"]);
                int MsgCellCount = int.Parse(ConfigurationManager.AppSettings["MsgCellCount"]);

                BLL.BLLBase bll = new BLL.BLLBase();

                if (Session["DT_UserOperation"] == null)
                {
                    int iGroupID = int.Parse(Session["GroupID"].ToString());
                    DataTable dt = bll.FillDataTable("Security.SelectGroupRole", new DataParameter[] { new DataParameter("@GroupID", iGroupID), new DataParameter("@SystemName", "WMS") });
                    Session["DT_UserOperation"] = dt;
                }

                hdnMsg.Value = "";
                DataTable dtOP = (DataTable)(Session["DT_UserOperation"]);


                DataTable dtMsg = bll.FillDataTable("WMS.SelectMsg", new DataParameter[] { new DataParameter("@MsgInstockTime", MsgInstockTime), 
                                                                                       new DataParameter("@MsgOutstockTime", MsgOutstockTime),
                                                                                       new DataParameter("@MsgCellCount", MsgCellCount) });

                string strMsg = "";
                DataRow[] drs = dtMsg.Select("Flag=1");
                if (drs.Length > 0)
                {
                    strMsg = "";
                    //for (int i = 0; i < drs.Length; i++)
                    //{
                    //    strMsg += "采购单号:" + drs[i]["BillID"].ToString() + " " + drs[i]["ProductName"].ToString() + "(" + drs[i]["ProductModel"].ToString() + ")" + drs[i]["ColorName"].ToString() + " 未入库数量:" + drs[i]["NotCount"].ToString() + Environment.NewLine;
                    //}
                    if (hdnMsg.Value != "")
                        hdnMsg.Value += Environment.NewLine;
                    hdnMsg.Value += "您有" + drs.Length.ToString() + "笔采购订单未入库." + Environment.NewLine + strMsg;
                }

                drs = dtMsg.Select("Flag=3");
                if (drs.Length > 0)
                {
                    DataRow[] drops = dtOP.Select("SubModuleCode='MNU_M00C_00B'");
                    if (drops.Length > 0)
                    {

                        strMsg = "";
                        //for (int i = 0; i < drs.Length; i++)
                        //{
                        //    strMsg += "货架：" + drs[i]["BillID"].ToString() + "  产品名称：" + drs[i]["ProductName"].ToString() + "(" + drs[i]["ProductModel"].ToString() + ")  " + drs[i]["ColorName"].ToString();
                        //}
                        if (hdnMsg.Value != "")
                            hdnMsg.Value += Environment.NewLine;
                        hdnMsg.Value += "您有" + drs.Length.ToString() + "个货架需移库." + Environment.NewLine + strMsg;
                    }
                }

                //产品对照表
                drs = dtOP.Select("SubModuleCode='MNU_M00A_00F'");
                if (drs.Length > 0)
                {
                    dtMsg = bll.FillDataTable("WMS.SelectBomNotProduct", null);
                    if (dtMsg.Rows.Count > 0)
                    {
                        strMsg = "";
                        //for (int i = 0; i < dtMsg.Rows.Count; i++)
                        //{
                        //    strMsg += dtMsg.Rows[i]["ProductNo"].ToString() + Environment.NewLine;
                        //}
                        if (hdnMsg.Value != "")
                            hdnMsg.Value += Environment.NewLine;
                        hdnMsg.Value += "您有" + dtMsg.Rows.Count.ToString() + "笔K3物料编码未匹配条形码." + Environment.NewLine + strMsg;

                    }
                }
                //销售订单转出库单
                drs = dtOP.Select("SubModuleCode='MNU_M00D_00B' and OperatorCode=0");
                if (drs.Length > 0)
                {
                    dtMsg = bll.FillDataTable("WMS.SelectOutStockMsg", null);
                    if (dtMsg.Rows.Count > 0)
                    {
                        strMsg = "";
                        //for (int i = 0; i < dtMsg.Rows.Count; i++)
                        //{
                        //    strMsg += dtMsg.Rows[i]["ScheduleNo"].ToString() + Environment.NewLine;
                        //}
                        if (hdnMsg.Value != "")
                            hdnMsg.Value += Environment.NewLine;
                        hdnMsg.Value += "您有" + dtMsg.Rows.Count.ToString() + "笔销售订单未完成排单。" + Environment.NewLine + strMsg;

                    }
                }

                //出库作业
                drs = dtOP.Select("SubModuleCode='MNU_M00B_00C' or SubModuleCode='MNU_M00D_00C'");
                if (drs.Length > 0)
                {
                    dtMsg = bll.FillDataTable("WMS.SelectOutStockNotTask", null);
                    if (dtMsg.Rows.Count > 0)
                    {
                        strMsg = "";
                        for (int i = 0; i < dtMsg.Rows.Count; i++)
                        {
                            strMsg += dtMsg.Rows[i]["DriverType"].ToString() + ":" + dtMsg.Rows[i]["DriverCount"].ToString() + "笔" + Environment.NewLine;
                        }
                        if (hdnMsg.Value != "")
                            hdnMsg.Value += Environment.NewLine;
                        hdnMsg.Value += "您有发货通知单需要进行出库作业:" + Environment.NewLine + strMsg;

                    }
                }

                //销售退货单--审核
                drs = dtOP.Select("SubModuleCode='MNU_M00D_00G'");
                if (drs.Length > 0)
                {
                    dtMsg = bll.FillDataTable("WMS.SelectReturn", new DataParameter[] { new DataParameter("{0}", "Flag=2 and Checker=''") });
                    if (dtMsg.Rows.Count > 0)
                    {
                        strMsg = "";
                        
                        if (hdnMsg.Value != "")
                            hdnMsg.Value += Environment.NewLine;
                        hdnMsg.Value += "您有 " + dtMsg.Rows.Count + " 笔销售退货单需要进行审核:" + Environment.NewLine;

                    }
                }



                if (hdnMsg.Value.Length != 0)
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "", "ShowFormMsg('hdnMsg');", true);
            }
            //}
            if (Session["UserID"] != null)
            {
                GetDestopItemByUserID(Session["UserID"].ToString());
            }
            else
            {
                Response.Write("<script language=javascript>parent.parent.parent.location.href='../../WebUI/Start/SessionTimeOut.aspx';</script>");
                Response.End();
                return;

                //Response.Redirect("../../WebUI/Start/SessionTimeOut.aspx");
            }
            CreatePage();
            //判断消息提醒
          
            //}
            //else
            //{
            //    Response.Redirect("~/SessionTimeOut.aspx");
            //}
        }
        #endregion
        #region 创建页面
        private void CreatePage()
        {
            ////////
            GlobalMenuTitle = new Hashtable();
            GlobalMenuLink = new Hashtable();
            GlobalMenuParent = new Hashtable();
            GlobalModuleID = new Hashtable();
            tb = new Table();
            //Session["IsFirstLogin"] = "2";
            tb = CreateTable(iTableCount);
            tb.Attributes.Add("border", "0");
            tb.Attributes.Add("bordercolor", "#ffffff");
            tb.Attributes.Add("frame", "void");
            tb.Attributes.Add("cellpadding", "0");
            tb.Attributes.Add("cellspacing", "0");
            tb.Attributes.Add("align", "center");
            pl = new Panel();

            int i = 0, j = 0;
            if (dtDestopItem.Rows.Count > 0)
            {
                foreach (DataRow dr in dtDestopItem.Rows)
                {
                    ImageButton im = new ImageButton();
                    im.ID = "im" + i.ToString() + i.ToString() + j.ToString();
                    im.ImageUrl = "../images/" + dr["DestopImage"].ToString();
                    im.Click += new ImageClickEventHandler(im_Click);
                    GlobalMenuTitle.Add(im.ID, dr["MenuTitle"].ToString());
                    GlobalMenuLink.Add(im.ID, dr["MenuUrl"].ToString());
                    GlobalMenuParent.Add(im.ID, dr["MenuParent"].ToString());
                    GlobalModuleID.Add(im.ID, dr["ModuleID"].ToString());
                    tb.Rows[i].Cells[j].Controls.Add(im);
                    tb.Rows[i].Cells[j].Controls.Add(new LiteralControl("<br>" + dr["MenuTitle"].ToString()));
                    j++;
                    if (j > 4)
                    {
                        j = 0; i++;
                    }
                }
            }
            pl.Controls.Add(tb);
            form1.Controls.Add(pl);
        }
        #endregion
        #region 链接事件
        protected void im_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton ImageButtonID = (ImageButton)sender;
            string strScript = "<script> ";
            //strScript += "var nav=window.parent.document.getElementById(\'labNavigation\');";
            //strScript += " nav.innerText=\'" + GlobalMenuParent[ImageButtonID.ID].ToString() + ">>" + GlobalMenuTitle[ImageButtonID.ID].ToString() + "\';";
            strScript += string.Format("window.parent.addTab(\"{0}&tabId=tab_{2}\",\"{1}\",\"tab_{2}\");", GlobalMenuLink[ImageButtonID.ID].ToString(), GlobalMenuTitle[ImageButtonID.ID].ToString(), GlobalModuleID[ImageButtonID.ID].ToString());
            //strScript += "window.open(\'" + GlobalMenuLink[ImageButtonID.ID].ToString() + "\',\'_self\')";
            strScript += "</script>";
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), DateTime.Now.ToLongTimeString(), strScript);
        }
        private void GetDestopItemByUserID(string UserID)
        {
            BLL.BLLBase bll = new BLL.BLLBase();
            dtDestopItem = bll.FillDataTable("Security.SelectUserQuickDesktop", new DataParameter[] { new DataParameter("@UserID", UserID) });
            iTableCount = dtDestopItem.Rows.Count;
        }
        #endregion
        #region 创建表格
        /// <summary>
        /// 创建表格
        /// </summary>
        /// <param name="iRowCount"></param>
        /// <returns></returns>
        private Table CreateTable(int iTableCount)
        {
            for (int i = 0; i < iTableCount / 5 + 1; i++)
            {
                TableRow row = CreateTableRow();
                row.Attributes.Add("align", "center");
                for (int j = 0; j < 5; j++)
                {
                    row.Cells.Add(CreateTableCell(i, j));
                }
                tb.Rows.Add(row);
            }
            return tb;
        }
        #endregion 创建表格
        #region 创建Table的Row
        /// <summary>
        /// 创建Table的Row
        /// </summary>
        /// <returns></returns>
        private TableRow CreateTableRow()
        {
            TableRow CreateTableRow = new TableRow();
            //CreateTableRow.Attributes.Add("style","height:200px;width:200px");
            return CreateTableRow;
        }
        #endregion 创建Table的Row
        #region 创建Table的cell
        /// <summary>
        /// 创建Table的Cell
        /// </summary>
        /// <returns></returns>
        private TableCell CreateTableCell(int i, int j)
        {
            TableCell CreateTableCell = new TableCell();
            CreateTableCell.Attributes.Add("style", "height:120px;width:150px");
            CreateTableCell.ID = i.ToString() + j.ToString();
            int iID = Convert.ToInt32(CreateTableCell.ID);
            if (iCount < iTableCount)
            {
                //switch (iCountColor)
                //{
                //    case 0:
                //        CreateTableCell.Attributes.Add("bgcolor ", "");
                //        break;
                //    case 1:
                //        CreateTableCell.Attributes.Add("bgcolor ", "");
                //        break;
                //    case 2:
                //        CreateTableCell.Attributes.Add("bgcolor ", "");
                //        break;
                //    case 3:
                //        CreateTableCell.Attributes.Add("bgcolor ", "");
                //        break;
                //    default:
                //        CreateTableCell.Attributes.Add("bgcolor ", "");
                //        iCountColor = 0;
                //        break;
                //}
                CreateTableCell.Attributes.Add("onMouseOver", "SetNewColor(this);");
                CreateTableCell.Attributes.Add("onMouseOut", "SetOldColor(this);");
                iCountColor++;
            }
            iCount++;
            return CreateTableCell;
        }
        #endregion
    }
}