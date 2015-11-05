using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Xml;

namespace Util
{
    public class DBConfig
    {
        private DbConnectionStringBuilder builder = new DbConnectionStringBuilder();
        private DataTable dtDBConfig;
        private string fileName;
        private XmlDocument doc = new XmlDocument();

        public DBConfig()
        {

            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load("DB.xml");
                fileName = "DB.xml";
            }
            catch
            {
                doc.Load(Utility.BasePath + @"\DB.xml");
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

                builder.ConnectionString = dr["CnString"].ToString();
                dtDBConfig.Rows.Add(dr);
            }
        }

        public void Save()
        {
            this.doc.Load(this.fileName);
            foreach (XmlNode node in this.doc.GetElementsByTagName("Connection"))
            {
                node.Attributes["Value"].Value = DESEncrypt.Encrypt(this.builder.ConnectionString);
            }
            this.doc.Save(this.fileName);
        }
        public DbConnectionStringBuilder Parameters
        {
            get
            {
                return builder;
            }
        }
    }
}
