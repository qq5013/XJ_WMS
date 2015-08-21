using System;
using System.Collections.Generic;

using System.Text;
using IDAL;
using System.Data;

namespace BLL.Security
{

    public class UserBll
    {
        private string strTableView = "sys_UserList";
        private string strPrimaryKey = "UserID";
        //private string strOrderByFields = "UserName ASC";
        private string strQueryFields = "UserID,UserName,UserPassword,Memo,EmployeeCode,EmployeeCode as EmployeeName";
        IDataAccess da;

        public UserBll()
        {
            da = IDAL.DALFactory.GetDataAccess();
        }
        public UserBll(string CnKey)
        {
            da = DALFactory.GetDataAccess(CnKey);
        }

        public DataTable GetUserInfo(string UserName)
        {
            DataTable dt = da.FillDataTable("Security.SelectUserInfoByUserName", new DataParameter[] { new DataParameter("@UserName", UserName) });
            if (dt.Rows.Count > 0)
            {
                dt.Rows[0].BeginEdit();
                dt.Rows[0]["UserPassword"] = Util.DESEncrypt.Decrypt(dt.Rows[0]["UserPassword"].ToString());
                dt.Rows[0].EndEdit();
            }
            return dt;
        }
        /// <summary>
        /// 更新用户密码
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="PWD"></param>
        public void UpdateUserPWD(string UserName, string PWD)
        {
            string strPwd = Util.DESEncrypt.Encrypt(PWD);
            da.ExecNonQuery("Security.UpdateUserPWD", new DataParameter[] { new DataParameter("@UserName", UserName), new DataParameter("@PWD", strPwd) });

        }
        public DataTable GetUserList(int pageIndex, int pageSize, string filter, string OrderByFields)
        {
            DataParameter[] param = new DataParameter[] { 
            new DataParameter("@tbname",strTableView), 
            new DataParameter("@FieldKey",strPrimaryKey),
           new DataParameter("@PageCurrent",pageIndex),
           new DataParameter("@pageSize",pageSize),
            new DataParameter("@FieldShow",strQueryFields),
           new DataParameter("@Where",filter),
           new DataParameter("@FieldOrder",OrderByFields),
           new DataParameter("@PageCount",0),
           new DataParameter("@RecordCount",0),
           };

            DataTable dtResult = da.FillDataTable("Security.SpDataQuery", param);
            return dtResult;

        }
        public void InsertUser(string UserName, string EmployeeCode, string Memo)
        {
            string strPwd = Util.DESEncrypt.Encrypt("123456");

            da.ExecNonQuery("Security.InsertUser", new DataParameter[]{new DataParameter("@UserName",UserName),new DataParameter("@UserPassword",strPwd),
                new DataParameter("@EmployeeCode",EmployeeCode),new DataParameter("@Memo",Memo)});

        }
        public void UpdateUser(string UserName, string EmployeeCode, string Memo, int UserID)
        {
            da.ExecNonQuery("Security.UpdateUserInfo", new DataParameter[]{new DataParameter("@UserName",@UserName),new DataParameter("@UserID",UserID),
                new DataParameter("@EmployeeCode",EmployeeCode),new DataParameter("@Memo",Memo)});

        }

        public bool Login(string UserName,string Pwd)
        {
            bool blnvalue=false;
            DataTable dt = da.FillDataTable("Security.SelectUserInfoByUserName", new DataParameter[] { new DataParameter("@UserName", UserName) });
            string PWD = "";
            if (dt.Rows.Count > 0)
            {
               PWD= Util.DESEncrypt.Decrypt(dt.Rows[0]["UserPassword"].ToString());
                
            }
            if (Pwd == PWD)
                blnvalue = true;
            return blnvalue;
            
        }

    }
}
