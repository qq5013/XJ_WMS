using System;
using System.Collections.Generic;

using System.Text;

namespace Util
{
    public class Utility
    {
        /// <summary>
        /// 当前程序基目录
        /// </summary>
        public static string BasePath
        {
            get
            {
               

                string basePath = string.Empty;
                if (AppDomain.CurrentDomain.BaseDirectory.StartsWith(System.Environment.CurrentDirectory))//Windows应用程序则相等
                {
                    basePath = AppDomain.CurrentDomain.BaseDirectory;
                }
                else
                {
                    basePath = AppDomain.CurrentDomain.BaseDirectory + "Bin\\";
                }
                return basePath;
            }
        }

        /// <summary>
        /// 当前主机IP
        /// </summary>
        public static string IP
        {
            get
            {
                System.Net.IPHostEntry ipHost = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
                System.Net.IPAddress ipAddr = null;
                foreach (System.Net.IPAddress address in ipHost.AddressList)
                {
                    try
                    {
                        long l = address.Address;
                        ipAddr = address;
                        break;
                    }
                    catch
                    {
                    }
                }
                if (ipAddr != null)
                {
                    return ipAddr.ToString();
                }
                return string.Empty;
            }
        }

        //public static int GetCRCKey(byte[] data)
        //{
        //    int count = data.Length;
        //    byte[] buf = new byte[data.Length + 2];
        //    data.CopyTo(buf, 0);
        //    int ptr = 0;
        //    int i = 0;
        //    int crc = 0;
        //    byte crc1, crc2, crc3;
        //    crc1 = buf[ptr++];
        //    crc2 = buf[ptr++];
        //    buf[count] = 0;
        //    buf[count + 1] = 0;
        //    while (--count >= 0)
        //    {
        //        crc3 = buf[ptr++];
        //        for (i = 0; i < 8; i++)
        //        {
        //            if (((crc1 & 0x80) >> 7) == 1)//判断crc1高位是否为1 
        //            {
        //                crc1 = (byte)(crc1 << 1); //移出高位 
        //                if (((crc2 & 0x80) >> 7) == 1)//判断crc2高位是否为1 
        //                {
        //                    crc1 = (byte)(crc1 | 0x01);//crc1低位由0变1 
        //                }
        //                crc2 = (byte)(crc2 << 1);//crc2移出高位 
        //                if (((crc3 & 0x80) >> 7) == 1) //判断crc3高位是否为1 
        //                {
        //                    crc2 = (byte)(crc2 | 0x01); //crc2低位由0变1 
        //                }
        //                crc3 = (byte)(crc3 << 1);//crc3移出高位 
        //                crc1 = (byte)(crc1 ^ 0x10);
        //                crc2 = (byte)(crc2 ^ 0x21);
        //            }
        //            else
        //            {
        //                crc1 = (byte)(crc1 << 1); //移出高位 
        //                if (((crc2 & 0x80) >> 7) == 1)//判断crc2高位是否为1 
        //                {
        //                    crc1 = (byte)(crc1 | 0x01);//crc1低位由0变1 
        //                }
        //                crc2 = (byte)(crc2 << 1);//crc2移出高位 
        //                if (((crc3 & 0x80) >> 7) == 1) //判断crc3高位是否为1 
        //                {
        //                    crc2 = (byte)(crc2 | 0x01); //crc2低位由0变1 
        //                }
        //                crc3 = (byte)(crc3 << 1);//crc3移出高位 
        //            }
        //        }
        //    }
        //    crc = (int)((crc1 << 8) + crc2);
        //    return crc;
        //}



        public static int GetCRCKey(byte[] Array, uint Len)
        {
            uint IX, IY;
            uint CRC = 0xFFFF;//set all 1

            if (Len <= 0)
                CRC = 0;
            else
            {
                Len--;
                ;
                for (IX = 0; IX <= Len; IX++)
                {
                    CRC = CRC ^ (Array[IX]);
                    for (IY = 0; IY <= 7; IY++)
                    {
                        if ((CRC & 1) != 0)
                            CRC = (CRC >> 1) ^ 0xA001;
                        else
                            CRC = CRC >> 1;    //
                    }
                }

            }
            uint[] Rcvbuf = new uint[2];
            Rcvbuf[0] = (CRC & 0xff00) >> 8;//高位置
            Rcvbuf[1] = (CRC & 0x00ff);  //低位置

            CRC = Rcvbuf[0] << 8;
            CRC += Rcvbuf[1];
            return (int)CRC;
        }


    }
        
}
