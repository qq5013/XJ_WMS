using System;
using System.Collections.Generic;

using System.Text;
using System.Data;
using System.IO;

namespace IDAL
{
    /// <summary>
    /// 命令仓库，存放SQL语句
    /// </summary>
    public class CommandStorage
    {
        private static DataSet dsCommand = new DataSet();

        private CommandStorage() { }

        /// <summary>
        /// 将命令从指定目录（文件类型为xml）读到内存中
        /// </summary>
        /// <param name="directoryPath">命令所在目录</param>
        public static void LoadCommand(string directoryPath)
        {
            //获取下面所有文件
            string[] files = Directory.GetFiles(directoryPath, "*.xml", SearchOption.AllDirectories);
            //添加命令数据
            foreach (string xmlFilePath in files)
            {
                if (string.IsNullOrEmpty(xmlFilePath))
                {
                    continue;
                }
                string tableName = new FileInfo(xmlFilePath).Name.Split('.')[0];
                //已加载的命令不再加载
                if (dsCommand.Tables.Contains(tableName)) continue;

                DataSet ds = new DataSet();
                ds.ReadXml(xmlFilePath);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables.Contains("Command"))
                    {
                        DataTable dt = ds.Tables["Command"].Copy();
                        dt.TableName = tableName;

                        //前缀
                        if (ds.Tables.Contains("SqlCommand") && ds.Tables["SqlCommand"].Rows.Count > 0 && ds.Tables["SqlCommand"].Columns.Contains("Pre"))
                        {
                            object pre = ds.Tables["SqlCommand"].Rows[0]["Pre"];
                            dt.ExtendedProperties["Pre"] = pre.ToString();
                        }

                        if (!dsCommand.Tables.Contains(tableName))
                        {
                            dsCommand.Tables.Add(dt);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 根据命令ID获取命令
        /// </summary>
        /// <param name="commandID">命令ID，格式：命令所在文件名.命令ID</param>
        /// <returns></returns>
        public static string GetCommandText(string commandID)
        {
            if (string.IsNullOrEmpty(commandID))
            {
                return string.Empty;
            }

            string[] spItem = commandID.Split('.');
            if (spItem.Length != 2 || !dsCommand.Tables.Contains(spItem[0]))
            {
                return string.Empty;
            }

            DataTable dtCommand = dsCommand.Tables[spItem[0]];
            DataRow[] dr = dtCommand.Select("ID = '" + spItem[1] + "'");
            if (dr != null && dr.Length > 0)
            {
                string cmd = dr[0]["CommandString"].ToString();
                return cmd;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
