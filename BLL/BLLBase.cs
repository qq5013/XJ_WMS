using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDAL;
using System.Data;
using System.Text.RegularExpressions;


namespace BLL
{
    public class BLLBase
    {
          IDataAccess da;

        public BLLBase()
        {
            da = IDAL.DALFactory.GetDataAccess();
        }
        public BLLBase(string CnKey)
        {
            da = DALFactory.GetDataAccess(CnKey);
        }

        /// <summary>
        /// 使用指定连接名，执行指定命令ID的非查询SQL语句
        /// </summary>
        /// <param name="connectionName">连接名</param>
        /// <param name="commandID">命令ID</param>
        /// <param name="parameters">参数</param>
        /// <returns>影响行数</returns>
        public int ExecNonQuery(string commandID, params DataParameter[] parameters)
        {
            try
            {
                return da.ExecNonQuery(commandID, parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 使用指定连接名，执行指定命令ID的非查询SQL语句
        /// </summary>
        /// <param name="connectionName">连接名</param>
        /// <param name="commandID">命令ID</param>
        /// <param name="parameters">参数</param>
        /// <returns>影响行数</returns>
        public int ExecNonQueryTran(string commandID, params DataParameter[] parameters)
        {
            try
            {
                return da.ExecNonQueryTran(commandID, parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 使用指定连接名，开启事务执行一组SQL命令
        /// </summary>
        /// <param name="connectionName">连接名</param>
        /// <param name="commandIDs">命令ID数组</param>
        /// <param name="parameters">参数列表，与命令ID对应</param>
        /// <returns>影响行数</returns>
        public int ExecTran(  string[] commandIDs, List<DataParameter[]> parameters)
        {
            return da.ExecTran(commandIDs, parameters.ToList<object[]>());
        }
        /// <summary>
        /// 使用指定连接名，执行指定命令ID的查询SQL语句
        /// </summary>
        /// <param name="connectionName">连接名</param>
        /// <param name="commandID">命令ID</param>
        /// <param name="parameters">参数</param>
        /// <returns>结果集</returns>
        public DataTable FillDataTable(string commandID, params DataParameter[] parameters)
        {
            return da.FillDataTable(commandID, parameters);
        }
        public DataSet FillDataSet(string commandID,  params object[] parameters)
        {
            return da.FillDataSet(commandID, parameters);
        }

        /// <summary>
        /// 使用指定连接名，执行指定命令ID的查询SQL语句，并返回第一行第一列
        /// </summary>
        /// <param name="connectionName">连接名</param>
        /// <param name="commandID">命令ID</param>
        /// <param name="parameters">参数</param>
        /// <returns>第一行第一列</returns>
        public object ExecScalar(string commandID, params DataParameter[] parameters)
        {
            return da.ExecScalar(commandID, parameters);
        }
        /// <summary>
        /// 使用指定连接名，执行指定命令ID的分页查询SQL语句
        /// </summary>
        /// <param name="connectionName">连接名</param>
        /// <param name="commandID">命令ID</param>
        /// <param name="currentPage">当前第几页</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="recordCount">总记录条数</param>
        /// <param name="parameters">参数</param>
        /// <returns>结果集</returns>
        public DataTable GetDataPage(int pageIndex, int pageSize, string filter,string orderField, string strPrimaryKey, string strTableView, string strQueryFields)
        {
            DataParameter[] param = new DataParameter[] { 
                    new DataParameter("@tbname",strTableView),
                    new DataParameter("@FieldKey",strPrimaryKey),
                    new DataParameter("@PageCurrent",pageIndex),
                    new DataParameter("@pageSize",pageSize),
                    new DataParameter("@FieldShow",strQueryFields),
                    new DataParameter("@FieldOrder",orderField),
                    new DataParameter("@Where",filter),
                    new DataParameter("@PageCount",0),
                    new DataParameter("@RecordCount",0)
            };

            DataTable dtResult = da.FillDataTable("Security.SpDataQuery", param);
            return dtResult;
        }
        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="commandID"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="TotalCount"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataTable GetDataPage(string commandID, int currentPage, int pageSize,out int TotalCount,params DataParameter[] parameters)
        {
            return da.GetDataPage(commandID, currentPage, pageSize,out TotalCount, parameters);
        }
        /// <summary>
        /// 获取表TableName的行数
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public int GetRowCount(string TableName, string filter)
        {
            return (int)da.ExecScalar("Security.SelectRowCount", new DataParameter[] { new DataParameter("{0}", TableName), new DataParameter("{1}", filter) });
        }

        public string GetAutoCode(string PreName, DateTime dtime, string Filter)
        {
            string strNew = "";
            if (Filter == "")
                Filter = "1=1";
            DataTable dt = da.FillDataTable("Security.SelectTmpAutoCode", new DataParameter[] { new DataParameter("{0}", string.Format("PreName='{0}'", PreName)) });
            if (dt.Rows.Count > 0)
            {
                string TableName = dt.Rows[0]["TableName"].ToString();
                string FieldName = dt.Rows[0]["FieldName"].ToString();
                string dateFormat = dt.Rows[0]["DateFormat"].ToString();
               
                int SeqLen = int.Parse(dt.Rows[0]["SeqLen"].ToString());

                string PreCode = PreName + dtime.ToString(dateFormat);

                dt = da.FillDataTable("Security.SelectMaxValue", new DataParameter[] { new DataParameter("{0}", TableName),
                                                                 new DataParameter("{1}", FieldName),
                                                                 new DataParameter("{2}", Filter+ string.Format(" and {0} like '{1}%'",FieldName,PreCode)) });
                if (dt.Rows.Count > 0)
                {
                    strNew = dt.Rows[0][0].ToString();
                }
                if (strNew == "")
                    strNew = PreCode + "1".PadLeft(SeqLen, '0');
                else
                {
                    strNew = NewID(strNew);
                }
            }
            return strNew;
        }

        public string GetAutoCodeByTableName(string PreName, string TableName, DateTime dtime, string Filter)
        {
            string strNew = "";
            if (Filter == "")
                Filter = "1=1";
            DataTable dt = da.FillDataTable("Security.SelectTmpAutoCode", new DataParameter[] { new DataParameter("{0}", string.Format("PreName='{0}' and TableName='{1}'", PreName,TableName)) });
            if (dt.Rows.Count > 0)
            {
               
                string FieldName = dt.Rows[0]["FieldName"].ToString();
                string dateFormat = dt.Rows[0]["DateFormat"].ToString();

                int SeqLen = int.Parse(dt.Rows[0]["SeqLen"].ToString());

                string PreCode = PreName + dtime.ToString(dateFormat);

                dt = da.FillDataTable("Security.SelectMaxValue", new DataParameter[] { new DataParameter("{0}", TableName),
                                                                 new DataParameter("{1}", FieldName),
                                                                 new DataParameter("{2}", Filter+ string.Format(" and {0} like '{1}%'",FieldName,PreCode)) });
                if (dt.Rows.Count > 0)
                {
                    strNew = dt.Rows[0][0].ToString();
                }
                if (strNew == "")
                    strNew = PreCode + "1".PadLeft(SeqLen, '0');
                else
                {
                    strNew = NewID(strNew);
                }
            }
            return strNew;
        }



        //动态生成ID
        public string GetNewID(string TableName, string ColumnName,string Filter)
        {
            string strNew = "";
            DataTable dt = da.FillDataTable("Security.SelectPrimaryType", new DataParameter[] { new DataParameter("{0}", ColumnName), new DataParameter("{1}", TableName) });
            if (dt.Rows.Count > 0)
            {
                int DataLength = int.Parse(dt.Rows[0]["max_length"].ToString());
                if (dt.Rows[0]["user_type_id"].ToString() == "231")
                {
                    DataLength = DataLength / 2;
                }
                dt = da.FillDataTable("Security.SelectMaxValue", new DataParameter[] { new DataParameter("{0}", TableName), new DataParameter("{1}", ColumnName), new DataParameter("{2}", Filter) });
                if (dt.Rows.Count > 0)
                {
                    strNew = dt.Rows[0][0].ToString();
                }
                if (strNew == "")
                    strNew = "1".PadLeft(DataLength, '0');
                else
                {
                    strNew = NewID(strNew);
                }
            }
            return strNew;
        }
        public string GetFieldValue(string TableName, string FieldName, string Filter)
        {
            string strValue = "";
            DataTable dt = da.FillDataTable("Security.SelectFieldValue", new DataParameter[] { new DataParameter("{0}", TableName), new DataParameter("{1}", FieldName), new DataParameter("{2}", Filter) });
            if (dt.Rows.Count > 0)
            {
                strValue = dt.Rows[0][FieldName].ToString();
            }
            return strValue;

        }

        public DataTable GetRecord(string move, string TableName, string Filter, string PrimaryKey, string ID)
        {
            if (Filter.Trim() == "")
            {
                Filter = "1=1";
            }
            string strOrderBy = string.Format(" Order by {0}", PrimaryKey);
            string strWhere = Filter;
            switch (move)
            {
                case "F":
                    strOrderBy = string.Format(" Order by {0}", PrimaryKey);
                    strWhere = Filter;
                    break;
                case "P":
                    strOrderBy = string.Format(" Order by {0} desc ", PrimaryKey);
                    strWhere = Filter + string.Format("and {0}<'{1}'", PrimaryKey, ID);
                    break;
                case "N":
                    strOrderBy = string.Format("  Order by {0}", PrimaryKey);
                    strWhere = Filter + string.Format(" and {0}>'{1}'", PrimaryKey, ID);
                    break;
                case "L":
                    strOrderBy = string.Format(" Order by {0} Desc", PrimaryKey);
                    strWhere = Filter;
                    break;

            }
            DataTable dt = da.FillDataTable("Security.SelectViewTable", new DataParameter[] { new DataParameter("{0}", TableName), new DataParameter("{1}", strWhere), new DataParameter("{2}", strOrderBy) });

            return dt;
        }

        /// <summary>
        /// 主从表新增修改，一个主表，对应多个从表。
        /// </summary>
        /// <param name="commandIDs">一个主表commandID，一个从表对应两个commandID: delete,insert 
        /// 0：表示主表，1-2：表示第一个从表，3-4：表示第二个从表
        /// </param>
        /// <param name="parameter">主表参数</param>
        /// <param name="PrimarySubKey">从表关键字 0：表示第一个从表关键字，1：表示第二个从表关键字...</param>
        /// <param name="dtSub">从表DataTable，需与PrimarySubKey对应 0：表示第一个从表Datatable，1：表示第二个从表Datatable</param>
        /// <returns></returns>
        public int ExecTran(string[] commandIDs, DataParameter[] parameter, string PrimaryKey, DataTable[] dtSub)
        {
            List<string> Comd = new List<string>();
            Comd.Insert(0, commandIDs[0]);
            List<DataParameter[]> paras = new List<DataParameter[]>();
            paras.Insert(0, parameter);
            int InsertID = 1;
            for (int i = 0; i < dtSub.Length; i++)
            {
                string[] SubKey = PrimaryKey.Split(',');
                string strFormater = "";
                for (int j = 0; j < SubKey.Length; j++)
                {
                    if (j == SubKey.Length - 1)
                        strFormater += (SubKey[j] + "='{" + j + "}'");
                    else
                        strFormater += (SubKey[j] + "='{" + j + "}' and ");
                }

                object[] args = new object[SubKey.Length];
                for (int K = 0; K < SubKey.Length; K++)
                {
                    if (dtSub[i].Rows.Count > 0)
                        args[K] = dtSub[i].Rows[0][SubKey[K]];
                }
                DataParameter[] delPara = new DataParameter[] { new DataParameter("{0}", string.Format(strFormater, args)) };
                Comd.Insert(InsertID, commandIDs[i * 2 + 1]);

                paras.Insert(InsertID, delPara);

                InsertID++;

                

                 
                for (int j = 0; j < dtSub[i].Rows.Count; j++)
                {
                    DataParameter[] AddPara = new DataParameter[dtSub[i].Columns.Count];

                    for (int K = 0; K < dtSub[i].Columns.Count; K++)
                    {
                        AddPara[K] = new DataParameter("@" + dtSub[i].Columns[K].ColumnName, dtSub[i].Rows[j][K]);
                    }

                    Comd.Insert(InsertID, commandIDs[i * 2 + 2]);
                    paras.Insert(InsertID, AddPara);

                    InsertID++;
                }


            }
            return ExecTran(Comd.ToArray(), paras);
        }


        public int ExecTran(string[] commandIDs, string PrimaryKey, DataTable[] dtSub)
        {
            List<string> Comd = new List<string>();
            
            List<DataParameter[]> paras = new List<DataParameter[]>();
            
            int InsertID = 0;
            for (int i = 0; i < dtSub.Length; i++)
            {
                string[] SubKey = PrimaryKey.Split(',');
                string strFormater = "";
                for (int j = 0; j < SubKey.Length; j++)
                {
                    if (j == SubKey.Length - 1)
                        strFormater += (SubKey[j] + "='{" + j + "}'");
                    else
                        strFormater += (SubKey[j] + "='{" + j + "}' and ");
                }

                object[] args = new object[SubKey.Length];
                for (int K = 0; K < SubKey.Length; K++)
                {
                    if (dtSub[i].Rows.Count > 0)
                        args[K] = dtSub[i].Rows[0][SubKey[K]];
                }
                DataParameter[] delPara = new DataParameter[] { new DataParameter("{0}", string.Format(strFormater, args)) };
                Comd.Insert(InsertID, commandIDs[i * 2 ]);

                paras.Insert(InsertID, delPara);

                InsertID++;




                for (int j = 0; j < dtSub[i].Rows.Count; j++)
                {
                    DataParameter[] AddPara = new DataParameter[dtSub[i].Columns.Count];

                    for (int K = 0; K < dtSub[i].Columns.Count; K++)
                    {
                        AddPara[K] = new DataParameter("@" + dtSub[i].Columns[K].ColumnName, dtSub[i].Rows[j][K]);
                    }

                    Comd.Insert(InsertID, commandIDs[i * 2 + 1]);
                    paras.Insert(InsertID, AddPara);

                    InsertID++;
                }


            }
            return ExecTran(Comd.ToArray(), paras);
        }

        #region 私有方法
        private string NewID(string No)
        {
            if (No == "")
                return No;
            int id = No.Length - 1;
            char[] arr = No.ToCharArray();
            string strReturn = "";

            while (id > -1)
            {
                switch (arr[id].ToString())
                {
                    case "9":
                        strReturn = "0" + strReturn;
                        if (No.Length == 1)
                            No = "10";
                        id--;
                        break;

                    case "Z":
                        strReturn = "A" + strReturn;
                        if (No.Length == 1)
                            No = "A0";
                        id--;
                        break;
                    default:
                        if (IsNumber(arr[id].ToString()))
                            No = No.Substring(0, id) + (int.Parse(arr[id].ToString()) + 1) + strReturn;
                        else
                            No = No.Substring(0, id) + (char)(arr[id] + 1) + strReturn;

                        id = -1;
                        break;
                }
            }
            return No;
        }
        private static Regex RegNumber = new Regex("^[0-9]+$");
        private bool IsNumber(string inputData)
        {
            Match match1 = RegNumber.Match(inputData);
            return match1.Success;
        }
        #endregion
        
    }
}
