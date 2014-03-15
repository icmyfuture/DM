using System;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;

namespace DM.Common.Utility.Encrypt
{
    /// <summary>
    /// DES����/�����ࡣ
    /// </summary>
    public class DESEncrypt
    {
        private const string EncryptKey = "DM";

        #region ========����========

        /// <summary>
        /// Ĭ��Key����(Key=SobeyMAM)
        /// </summary>
        /// <param name="text">��Ҫ���ܵ�����</param>
        /// <returns>���ܵ�������</returns>
        public static string Encrypt(string text)
        {
            return Encrypt(text, EncryptKey);
        }

        /// <summary> 
        /// �������� 
        /// </summary> 
        /// <param name="text">��Ҫ���ܵ�����</param> 
        /// <param name="sKey">��Կ</param> 
        /// <returns>���ܵ�������</returns> 
        public static string Encrypt(string text, string sKey)
        {
            var des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.UTF8.GetBytes(text);
            var passwordForStoringInConfigFile = FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5");
            if (passwordForStoringInConfigFile != null)
            {
                des.Key = Encoding.UTF8.GetBytes(passwordForStoringInConfigFile.Substring(0, 8));
            }
            var hashPasswordForStoringInConfigFile = FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5");
            if (hashPasswordForStoringInConfigFile != null)
            {
                des.IV = Encoding.UTF8.GetBytes(hashPasswordForStoringInConfigFile.Substring(0, 8));
            }
            var ms = new System.IO.MemoryStream();
            var cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            var ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            return ret.ToString();
        }

        #endregion

        #region ========����========

        /// <summary>
        /// Ĭ��Key����(Key=SobeyMAM)
        /// </summary>
        /// <param name="text">Ҫ���ܵ�����</param>
        /// <returns>���ܵ�����</returns>
        public static string Decrypt(string text)
        {
            return Decrypt(text, EncryptKey);
        }

        /// <summary> 
        /// �������� 
        /// </summary> 
        /// <param name="text">Ҫ���ܵ�����</param> 
        /// <param name="sKey">��Կ</param> 
        /// <returns>���ܵ�����</returns> 
        public static string Decrypt(string text, string sKey)
        {
            var des = new DESCryptoServiceProvider();
            int len = text.Length / 2;
            var inputByteArray = new byte[len];
            int x;
            for (x = 0; x < len; x++)
            {
                int i = Convert.ToInt32(text.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }
            var hashPasswordForStoringInConfigFile = FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5");
            if (hashPasswordForStoringInConfigFile != null)
            {
                des.Key = Encoding.UTF8.GetBytes(hashPasswordForStoringInConfigFile.Substring(0, 8));
            }
            var passwordForStoringInConfigFile = FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5");
            if (passwordForStoringInConfigFile != null)
            {
                des.IV = Encoding.UTF8.GetBytes(passwordForStoringInConfigFile.Substring(0, 8));
            }
            var ms = new System.IO.MemoryStream();
            var cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Encoding.UTF8.GetString(ms.ToArray());
        }

        #endregion
    }
}