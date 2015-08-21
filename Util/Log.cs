using System;
using System.Collections.Generic;

using System.Text;
using System.IO;
namespace Util
{
    public class Log
    {
        /// <summary>
        /// 记录 信息级 日志
        /// </summary>
        /// <param name="message">消息</param>
        public static void Info(string message)
        {
            string msg = FormatMsg(message);
          
            OutPut(msg);
        }

        /// <summary>
        /// 记录 跟踪级 日志
        /// </summary>
        /// <param name="message">消息</param>
        public static void Trace(string message)
        {
            string msg = FormatMsg(message);
           
            OutPut(msg);
        }

        /// <summary>
        /// 记录 调试级 日志
        /// </summary>
        /// <param name="message">消息</param>
        public static void Debug(string message)
        {
            string msg = FormatMsg(message);
           
            OutPut(msg);
        }

        /// <summary>
        /// 记录 警告级 日志
        /// </summary>
        /// <param name="message">消息</param>
        public static void Warn(string message)
        {
            string msg = FormatMsg(message);
            
            OutPut(msg);
        }

        /// <summary>
        /// 记录 错误级 日志
        /// </summary>
        /// <param name="message">消息</param>
        public static void Error(string message)
        {
            string msg = FormatMsg(message);
           
            OutPut(msg);
        }

        /// <summary>
        /// 记录 致命级 日志
        /// </summary>
        /// <param name="message">消息</param>
        

        private static void OutPut(string message)
        {
#if DEBUG
            System.Diagnostics.Trace.Write(message);
#endif
        }

        private static string FormatMsg(string message)
        {
            string msg = message.Trim();
            if (msg.IndexOf("\r\n") != 0)
            {
                msg = "\r\n" + msg;
            }
            if (!msg.EndsWith("\r\n"))
            {
                msg = msg + "\r\n";
            }
            return msg;
        }

        //写入获取图纸的信息。
        public static void WriteFileLog(string Msg)
        {
            string fullPath = System.AppDomain.CurrentDomain.BaseDirectory + @"\Log\FileInfo.txt";

            if (!File.Exists(fullPath))
            {
                FileStream fs = new FileStream(fullPath, FileMode.OpenOrCreate);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " : " + Msg + "\r\n");
                sw.Close();
            }
            else
            {
                FileStream fout = new FileStream(fullPath, FileMode.Append, FileAccess.Write);
                StreamWriter brout = new StreamWriter(fout, Encoding.Default);
                brout.Write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " : " + Msg + "\r\n");
                brout.Close();
            }

        }
    }
}
