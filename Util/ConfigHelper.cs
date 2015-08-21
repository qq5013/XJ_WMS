using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Xml;

namespace Util
{
    public sealed class ConfigHelper
    {
        // Methods
        public ConfigHelper()
        {

        }

        public static bool GetConfigBool(string key)
        {
            bool flag1 = false;
            string text1 = ConfigHelper.GetConfigString(key);
            if ((text1 == null) || (string.Empty == text1))
            {
                return flag1;
            }
            try
            {
                return bool.Parse(text1);
            }
            catch (FormatException)
            {
                return false;
            }
        }
        public static decimal GetConfigDecimal(string key)
        {
            decimal num1 = new decimal(0);
            string text1 = ConfigHelper.GetConfigString(key);
            if ((text1 == null) || (string.Empty == text1))
            {
                return num1;
            }
            try
            {
                return decimal.Parse(text1);
            }
            catch (FormatException)
            {
                return num1;
            }
        }
        public static int GetConfigInt(string key)
        {
            int num1 = 0;
            string text1 = ConfigHelper.GetConfigString(key);
            if ((text1 == null) || (string.Empty == text1))
            {
                return num1;
            }
            try
            {
                return int.Parse(text1);
            }
            catch (FormatException)
            {
                return num1;
            }
        }

        public static string GetConfigString(string key)
        {
            return  ConfigurationManager.AppSettings[key];
        }
        public static void SetKeyValue(string path,string AppKey, string AppValue)
        {
            XmlDocument xDoc = new XmlDocument();
            if (!System.IO.File.Exists(path + ".config")) 
            {
                
               
               XmlNode xmlnode = xDoc.CreateNode(XmlNodeType.XmlDeclaration, "", "");
                xDoc.AppendChild(xmlnode);
                XmlElement xmlelem = xDoc.CreateElement("", "configuration", "");
                xDoc.AppendChild(xmlelem);
                XmlNode root = xDoc.SelectSingleNode("configuration"); 
                XmlElement xe1 = xDoc.CreateElement("appSettings"); 
                root.AppendChild(xe1);
                xDoc.Save(path + ".config");
            }
            xDoc.Load(path+ ".config");

            XmlNode xNode;
            XmlElement xElem1;
            XmlElement xElem2;
            xNode = xDoc.SelectSingleNode("//appSettings");

            xElem1 = (XmlElement)xNode.SelectSingleNode("//add[@key='" + AppKey + "']");
            if (xElem1 != null) 
                xElem1.SetAttribute("value", AppValue);
            else
            {
                xElem2 = xDoc.CreateElement("add");
                xElem2.SetAttribute("key", AppKey);
                xElem2.SetAttribute("value", AppValue);
                xNode.AppendChild(xElem2);
            }
            xDoc.Save(path + ".config");
        }
        public static string GetAppConfigValue(string path,string appKey)
        {
            System.Xml.XmlDocument xDoc = new System.Xml.XmlDocument();
            try
            {
                xDoc.Load(path + ".config");

                System.Xml.XmlNode xNode;
                System.Xml.XmlElement xElem;
                xNode = xDoc.SelectSingleNode("//appSettings");
                xElem = (System.Xml.XmlElement)xNode.SelectSingleNode("//add[@key='" + appKey + "']");
                if (xElem != null)
                    return xElem.GetAttribute("value");
                else
                    return "";
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}

