using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace IDAL
{
    /// <summary>
    /// 数据访问对象工厂
    /// </summary>
    public class DALFactory
    {
        protected static Dictionary<string, IDataAccess> DataAccessStroge = new Dictionary<string, IDataAccess>();

        /// <summary>
        /// 获取默认数据访问对象（根据配置文件中appSettings节中配置的第一个有效连接字符串构建）
        /// </summary>
        /// <returns></returns>
        public static IDataAccess GetDataAccess()
        {
            return GetConfigDataAccess(null);
        }

        /// <summary>
        /// 根据连接字符串名获取数据访问对象
        /// </summary>
        /// <param name="connectionName">连接字符串名，值为null时表示默认连接字符串名</param>
        /// <returns></returns>
        public static IDataAccess GetDataAccess(string connectionName)
        {
            return GetConfigDataAccess(connectionName);
        }

        /// <summary>
        /// 根据连接字符串名创建数据访问层对象，并将对应的命令加载到内存
        /// </summary>
        /// <param name="connectionName">连接字符串名</param>
        /// <returns></returns>
        private static IDataAccess GetConfigDataAccess(string CnKey)
        {
            if (string.IsNullOrEmpty(CnKey))
            {
                if (DataAccessStroge.Count > 0)
                {
                    return DataAccessStroge.First().Value;
                }
            }
            else if (DataAccessStroge.ContainsKey(CnKey))
            {
                return DataAccessStroge[CnKey];
            }

            
            DataTable dt = Util.DBConfigUtil.DtDBConfig;
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }

            string key = null;
            string value = null;
            string DbType = null;
            if (CnKey == null)
            {
                key = dt.Rows[0]["Name"].ToString();
                DbType = dt.Rows[0]["DBType"].ToString();
                value = dt.Rows[0]["CnString"].ToString();
            }
            else
            {
                DataRow[] drs = dt.Select(string.Format("Name='{0}'", CnKey));
                if (drs.Length > 0)
                {
                    key = drs[0]["Name"].ToString();
                    DbType = drs[0]["DBType"].ToString();
                    value = drs[0]["CnString"].ToString();
                }
            }
            //反射获取类型
            string basePath = Util.Utility.BasePath;
            string dllPath = basePath + Util.DBConfigUtil.DBDllByName(DbType);
            string cmdPath = basePath + Util.DBConfigUtil.CmdByName(DbType);
            System.Reflection.Assembly assembly = System.Reflection.Assembly.LoadFile(dllPath);
            if (assembly == null) return null;
            Type type = assembly.GetType(Util.DBConfigUtil.AccessByName(DbType), false);
            if (type == null) return null;

            IDataAccess da = null;
            try
            {
                da = (IDataAccess)Activator.CreateInstance(type);
                da.ConnectionName = key;
                da.ConnectionString = value;
                CommandStorage.LoadCommand(cmdPath);//加载命令到内存
                if (!DataAccessStroge.ContainsKey(key))
                {
                    DataAccessStroge.Add(key, da);
                }
                return da;
            }
            catch (Exception e)
            {
                da = null;
                throw e;
            }
        }
    }
}
