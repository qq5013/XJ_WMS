using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Xml;

namespace Util
{
    public class DBConfigUtil
    {
        private static DataTable dtDBConfig;
        public static DataTable DtDBConfig
        {
            get
            {
                //若已经读取配置，则不再读取
                if (dtDBConfig != null)
                {
                    return dtDBConfig;
                }
                XmlDocument doc = new XmlDocument();
                string fileName;
                try
                {
                    doc.Load("DB.xml");
                    fileName = "DB.xml";
                }
                catch
                {
                    doc.Load(Utility.BasePath+ @"\DB.xml");
                    fileName = Utility.BasePath + @"\DB.xml";
                }

                dtDBConfig = new DataTable();
                DataColumn dc1 = new DataColumn("Name", Type.GetType("System.String"));
                DataColumn dc2 = new DataColumn("DBType", Type.GetType("System.String"));
                DataColumn dc3 = new DataColumn("CnString", Type.GetType("System.String"));
                dtDBConfig.Columns.Add(dc1);
                dtDBConfig.Columns.Add(dc2);
                dtDBConfig.Columns.Add(dc3);

                XmlNodeList nodeList = doc.SelectSingleNode("Connections").ChildNodes;



                foreach (XmlNode xn in nodeList)//遍历所有子节点 
                {
                    XmlElement xel = (XmlElement)xn;//将子节点类型转换为XmlElement类型 
                    DataRow dr = dtDBConfig.NewRow();
                    dr["Name"] = xel.GetAttribute("Name");
                    dr["DBType"] = xel.GetAttribute("DbType");
                    dr["CnString"] = DESEncrypt.Decrypt(xel.GetAttribute("Value"));

                    dtDBConfig.Rows.Add(dr);
                }
                
                return dtDBConfig;
            }

        }

        public static string DBDllByName(string Name)
        {
            string dll = string.Empty;
            switch (Name)
            {
                case "SQL":
                    dll = "SQLDAL.dll";
                    break;
                case "ORACLE":
                    dll = "ORACLEDAL.dll";
                    break;
            }
            return dll;
        }

        public static string CmdByName(string Name)
        {

            string cmd = string.Empty;
            switch (Name)
            {
                case "SQL":
                    cmd = "SQLCommand";
                    break;
                case "ORACLE":
                    cmd = "ORACLECommand";
                    break;
            }
            return cmd;
        }

        public static string AccessByName(string Name)
        {

            string Access = string.Empty;
            switch (Name)
            {
                case "SQL":
                    Access = "SQLDAL.SqlDataAccess";
                    break;
                case "ORACLE":
                    Access = "ORACLEDAL.ORACLEDataAccess";
                    break;
            }
            return Access;
        }
       
    }
}
