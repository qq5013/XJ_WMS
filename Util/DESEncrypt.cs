using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web.Security;
using System.Security.Cryptography;
namespace Util
{
    public class DESEncrypt
    {
        // Methods
        public DESEncrypt()
        {
        }

        public static string Decrypt(string Text)
        {
            return DESEncrypt.Decrypt(Text, "Js");
        }

        public static string Decrypt(string Text, string sKey)
        {
            DESCryptoServiceProvider provider1 = new DESCryptoServiceProvider();
            int num1 = Text.Length / 2;
            if (num1 == 0)
                return "";
            byte[] buffer1 = new byte[num1];

            for (int num2 = 0; num2 < num1; num2++)
            {
                int num3 = Convert.ToInt32(Text.Substring(num2 * 2, 2), 0x10);
                buffer1[num2] = (byte)num3;
            }
            provider1.Key = Encoding.ASCII.GetBytes(FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            provider1.IV = Encoding.ASCII.GetBytes(FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            MemoryStream stream1 = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream1, provider1.CreateDecryptor(), CryptoStreamMode.Write);
            stream2.Write(buffer1, 0, buffer1.Length);
            stream2.FlushFinalBlock();
            return Encoding.Default.GetString(stream1.ToArray());
        }

        public static string Encrypt(string Text)
        {
            return DESEncrypt.Encrypt(Text, "Js");
        }

        public static string Encrypt(string Text, string sKey)
        {
            DESCryptoServiceProvider provider1 = new DESCryptoServiceProvider();
            byte[] buffer1 = Encoding.Default.GetBytes(Text);
            provider1.Key = Encoding.ASCII.GetBytes(FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            provider1.IV = Encoding.ASCII.GetBytes(FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            MemoryStream stream1 = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream1, provider1.CreateEncryptor(), CryptoStreamMode.Write);
            stream2.Write(buffer1, 0, buffer1.Length);
            stream2.FlushFinalBlock();
            StringBuilder builder1 = new StringBuilder();
            foreach (byte num1 in stream1.ToArray())
            {
                builder1.AppendFormat("{0:X2}", num1);
            }
            return builder1.ToString();
        }
    }
}
