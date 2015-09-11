﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util
{
    public class ConvertStringChar
    {
        public static string BytesToString(object[] obj)
        {
            byte[] b = new byte[obj.Length];
            for (int i = 0; i < obj.Length; i++)
            {
                if (obj[i].ToString() == "0")
                    b[i] = 32;
                else if (int.Parse(obj[i].ToString()) < 0)
                    b[i] = 32;
                else
                    b[i] = byte.Parse(obj[i].ToString());
            }

            return Encoding.ASCII.GetString(b).Trim();
        }
        public static sbyte[] stringToBytes(string strvalue, int length)
        {
            sbyte[] b = new sbyte[length];
            byte[] a = Encoding.ASCII.GetBytes(strvalue);
            for (int i = 0; i < length; i++)
            {
                if (i >= a.Length)
                    b[i] = 0;
                else
                    b[i] = sbyte.Parse(a[i].ToString());
            }
            return b;
        }

        public static byte[] stringToByte(string strvalue, int length)
        {
            byte[] b = new byte[length];
            byte[] a = Encoding.ASCII.GetBytes(strvalue);
            for (int i = 0; i < length; i++)
            {
                if (i >= a.Length)
                    b[i] = 32;
                else
                    b[i] = a[i];
            }
            return b;
        }
    }
}
