using System;
using System.Collections.Generic;

using System.Text;
using System.Data;


namespace IDAL
{
    /// <summary>
    /// 数据访问层接口
    /// </summary>
    public interface IDataAccess
    {

       

        #region 使用SQL命令执行增删改查操作

        /// <summary>
        /// 当前使用连接名
        /// </summary>
        string ConnectionName { get; set; }
        /// <summary>
        /// 当前使用连接字符串
        /// </summary>
        string ConnectionString { get; set; }

        /// <summary>
        /// 执行带参数的非查询命令
        /// </summary>
        /// <param name="commandID">命令ID</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="parameters">参数数组</param>
        /// <returns>影响的记录行数</returns>
        int ExecNonQuery(string commandID,  params object[] parameters);

        /// <summary>
        /// 执行带参数的非查询命令
        /// </summary>
        /// <param name="commandID">命令ID</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="parameters">参数数组</param>
        /// <returns>影响的记录行数</returns>
        int ExecNonQueryTran(string commandID, params object[] parameters);

        /// <summary>
        /// 将SQL命令数组和对应参数集作为一个事务提交至数据库
        /// </summary>
        /// <param name="commandIDs">SQL命令ID数组</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="parameters">参数数组集合,必需跟命令顺序一致</param>
        /// <returns>影响行数</returns>
        int ExecTran(string[] commandIDs, /*CommandType commandType,*/ List<object[]> parameters);

        /// <summary>
        /// 通过SQL命令(带参数)获取数据表
        /// </summary>
        /// <param name="commandID">命令ID</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="parameters">参数数组</param>
        /// <returns>查询数据表</returns>
        DataTable FillDataTable(string commandID,   params object[] parameters);

        /// <summary>
        /// 通过SQL命令(带参数)获取数据集合
        /// </summary>
        /// <param name="commandID">命令ID</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="parameters">参数数组</param>
        /// <returns>查询的数据集合</returns>
        DataSet FillDataSet(string commandID,   params object[] parameters);

        /// <summary>
        /// 通过SQL命令(带参数)获取数据读取对象
        /// </summary>
        /// <param name="commandID">命令ID</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="parameters">参数数组</param>
        /// <returns>数据读取对象</returns>
        IDataReader ExecDataReader(string commandID,  params object[] parameters);

        /// <summary>
        /// 通过SQL命令获取单个值
        /// </summary>
        /// <param name="commandID">命令ID</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="parameters">参数数组</param>
        /// <returns></returns>
        object ExecScalar(string commandID, params object[] parameters);

        /// <summary>
        /// 获取分页表数据
        /// </summary>
        /// <param name="commandID">命令ID</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="currentPage">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="recordCount">总记录条数</param>
        /// <param name="parameters">参数数组</param>
        /// <returns>表</returns>
        DataTable GetDataPage(string commandID, int currentPage, int pageSize,out int TotalCount, out int PageCount,params object[] parameters);

        /// <summary>
        /// 根据命令ID获取对应的SQL命令
        /// </summary>
        /// <param name="commandID">格式：命令所在XML文件名.命令ID</param>
        /// <returns>SQL命令</returns>
        string GetCommandText(string commandID);

        /// <summary>
        /// 根据命令ID获取命令的类型
        /// </summary>
        /// <param name="commandID"></param>
        /// <returns></returns>
        CommandType GetCommandType(string commandID);

        #endregion
    }
}
