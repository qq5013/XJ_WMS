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
using System.Data.SqlClient;
using BLL.Security;
using Util;


namespace WMS.WebUI.Start
{
    public partial class Start : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            
            if (Request.QueryString["Logout"] != null)
            {
                string strUserName;
                if (Session["G_user"] != null)
                {
                    strUserName = Session["G_user"].ToString();
                }
                else
                {
                    Response.Write("<script language=javascript>parent.parent.location.href='../../WebUI/Start/SessionTimeOut.aspx';</script>");
                    Response.End();
                    strUserName = "";
                }
                HttpContext.Current.Cache.Remove(strUserName);
                GC.Collect();
            }
            if (!Page.IsPostBack)
            {
                if (Session["G_user"] != null)
                {
                    Session["G_user"] = null;
                    Response.Write("<script language=javascript>parent.parent.location.href='../../WebUI/Start/SessionTimeOut.aspx';</script>");
                    Response.End();
                }
                 
            }
            

        }

        #region 控件事件
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            //DE22AF18AF9393F4EAD85D59E9C6F068ADA7DD8449FAFD8BDC64E20F9932365BD114260509047A22D0C0BE9B7C660AE329AFD9A79743F8AE
            //string text= Util.DESEncrypt.Encrypt("123456");
            Request.Cookies.Clear();
            if (txtUserName.Text.Trim() != "")
            {
                try
                {
                    string key = txtUserName.Text.ToLower();
                    string UserCache = Convert.ToString(Cache[key]);


                    UserBll userBll = new UserBll();

                    DataTable dtUserList = userBll.GetUserInfo(txtUserName.Text.Trim());
                    if (dtUserList != null && dtUserList.Rows.Count > 0)
                    {
                        if (dtUserList.Rows[0]["UserPassword"].ToString().Trim() == txtPassWord.Text.Trim())
                        {
                            FormsAuthentication.SetAuthCookie(this.txtUserName.Text, false);


                            Session["UserID"] = dtUserList.Rows[0]["UserID"].ToString();
                            Session["GroupID"] = dtUserList.Rows[0]["GroupID"].ToString();
                            Session["G_user"] = dtUserList.Rows[0]["UserName"].ToString();
                            Session["Client_IP"] = Page.Request.UserHostAddress;
                            string EmployeeCode = dtUserList.Rows[0]["EmployeeCode"].ToString();
                            App_Code.LoginUserInfo.UserName = dtUserList.Rows[0]["UserName"].ToString();
                            App_Code.LoginUserInfo.UserCode = EmployeeCode;

                            Session["EmployeeCode"] = dtUserList.Rows[0]["EmployeeCode"].ToString();
                            Session["EmployeeName"] = "";
                            Session["sys_PageCount"] = 15;


                            Session["grid_ColumnTitleFont"] = "楷体_GB2312,Coral,10,加粗";
                            Session["grid_ContentFont"] = "宋体,Black,10,正常";

                            Session["grid_ColumnTextAlign"] = "1";
                            Session["grid_ContentTextAlign"] = "1";
                            Session["grid_NumberColumnAlign"] = "1";
                            Session["grid_MoneyColumnAlign"] = "1";
                            Session["grid_SelectMode"] = "0";
                            Session["grid_IsRefreshBeforeAdd"] = "1";
                            Session["grid_IsRefreshBeforeUpdate"] = "1";
                            Session["grid_IsRefreshBeforeDelete"] = "1";

                            Session["grid_OddRowColor"] = "White";
                            Session["grid_EvenRowColor"] = "AliceBlue";
                            Session["sys_PrintForm"] = dtUserList.Rows[0]["sys_PrintForm"].ToString();
                            string pager_ShowPageIndex = dtUserList.Rows[0]["pager_ShowPageIndex"].ToString();
                            
                            
                            
                            Session.Timeout = int.Parse(dtUserList.Rows[0]["SessionTimeOut"].ToString());
                            #region 添加登录日志

                            BLL.BLLBase bll = new BLL.BLLBase();
                            bll.ExecNonQuery("Security.InsertOperatorLog", new IDAL.DataParameter[]{new IDAL.DataParameter("@LoginUser",App_Code.LoginUserInfo.UserName),new IDAL.DataParameter("@LoginTime",DateTime.Now),
                                                         new IDAL.DataParameter("@LoginModule","登录系统"),new IDAL.DataParameter("@ExecuteOperator","用户登录")});

                            #endregion
                            TimeSpan stLogin = new TimeSpan(0, 0, System.Web.HttpContext.Current.Session.Timeout, 0, 0);
                            HttpContext.Current.Cache.Insert(key, Page.Request.UserHostAddress, null, DateTime.MaxValue, stLogin, System.Web.Caching.CacheItemPriority.NotRemovable, null);

                            Response.Redirect("../../Default5.aspx", false);
                        }
                        else
                        {
                            BLL.BLLBase bll = new BLL.BLLBase();
                            bll.ExecNonQuery("Security.InsertOperatorLog", new IDAL.DataParameter[]{new IDAL.DataParameter("@LoginUser",this.txtUserName.Text.Trim()),new IDAL.DataParameter("@LoginTime",DateTime.Now),
                                                         new IDAL.DataParameter("@LoginModule","登录页面"),new IDAL.DataParameter("@ExecuteOperator","登录(用户密码有误)")});
                            labMessage.Text = "对不起,您输入的密码有误!";
                        }
                    }
                    else
                    {
                        labMessage.Text = "对不起,您输入的用户名不存在!";
                    }

                }
                catch (Exception exp)
                {
                    System.Diagnostics.StackFrame frame = new System.Diagnostics.StackFrame(0);
                    Session["ModuleName"] = this.Page.Title;
                    Session["FunctionName"] = frame.GetMethod().Name;
                    Session["ExceptionalType"] = exp.GetType().FullName;
                    Session["ExceptionalDescription"] = exp.Message;
                    Response.Redirect("../../Common/MistakesPage.aspx", false);
                }
            }
            else
            {
                labMessage.Text = "请输入用户名!";
            }
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            string strScript = @"<SCRIPT LANGUAGE='javascript'> " + "window.opener=null;window.close();" + "</SCRIPT>";
            Page.RegisterStartupScript("a2", strScript);
        }
        #endregion
    }
}