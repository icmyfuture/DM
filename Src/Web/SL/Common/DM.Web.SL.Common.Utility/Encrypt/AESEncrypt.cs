using System;
using System.Text;
using System.Security.Cryptography;

namespace DM.Web.SL.Common.Utility.Encrypt
{
    /// <summary>
    /// AES加解密
    /// AES作为新一代的数据加密标准汇聚了强安全性、高性能、高效率、易用和灵活等优点。
    /// AES设计有三个密钥长度:128,192,256位，相对而言，AES的128密钥比DES的56密钥强1021倍。
    /// </summary>
    public class AESEncrypt
    {
        /// <summary> 
        /// 获取向量 
        /// </summary> 
        private static string Iv
        {
            get { return "DM"; }
        }

        #region ========加密========

        /// <summary>
        /// 默认Key加密(Key=SobeyMAM)
        /// </summary>
        /// <param name="text">需要加密的明文</param>
        /// <returns>加密的明文文</returns>
        public static string Encrypt(string text)
        {
            return Encrypt(text, "DM");
        }

        /// <summary> 
        /// 加密数据 
        /// </summary> 
        /// <param name="text">需要加密的明文</param> 
        /// <param name="sKey">密钥</param> 
        /// <returns>加密的明文文</returns> 
        public static string Encrypt(string text, string sKey)
        {
            byte[] dataKey = Encoding.UTF8.GetBytes(text);
            byte[] encryptData = Encrypt(dataKey, sKey, Iv);
            return Convert.ToBase64String(encryptData);
        }

        /// <summary>
        /// 加密数据 
        /// </summary> 
        /// <param name="text">需要加密的明文</param> 
        /// <param name="sKey">密钥</param> 
        /// <param name="vector">向量</param>
        /// <returns>加密后的密文</returns>
        public static string Encrypt(string text, string sKey, string vector)
        {
            byte[] dataKey = Encoding.UTF8.GetBytes(text);
            byte[] encryptData = Encrypt(dataKey, sKey, vector);
            return Convert.ToBase64String(encryptData);
        }

        /// <summary>
        /// 加密数据 
        /// </summary> 
        /// <param name="data">需要加密的明文</param> 
        /// <param name="sKey">密钥</param> 
        /// <returns>加密后的密文</returns>
        public static string Encrypt(byte[] data, string sKey)
        {
            byte[] dataKey = data;
            byte[] encryptData = Encrypt(dataKey, sKey, Iv);
            return Convert.ToBase64String(encryptData);
        }

        #region AES加密byte[]

        /// <summary>
        /// AES加密byte[]
        /// </summary>
        /// <param name="data">被加密的明文</param>
        /// <param name="key">密钥</param>
        /// <param name="vector">向量</param>
        /// <returns>密文</returns>
        public static byte[] Encrypt(byte[] data, string key, string vector)
        {
            var bKey = new Byte[16];
            Array.Copy(Encoding.UTF8.GetBytes(key.PadRight(bKey.Length)), bKey, bKey.Length);
            var bVector = new Byte[16];
            Array.Copy(Encoding.UTF8.GetBytes(vector.PadRight(bVector.Length)), bVector, bVector.Length);

            byte[] cryptograph; // 加密后的密文

            var aes = new AesManaged();


            try
            {
                aes.Key = bKey;
                aes.IV = bVector;
                //aes.Mode = CipherMode.CBC;
                //aes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = aes.CreateEncryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(data, 0, data.Length);
                cryptograph = resultArray;
            }
            catch
            {
                cryptograph = null;
            }

            return cryptograph;
        }

        #endregion

        #endregion

        #region ========解密========

        /// <summary>
        /// 默认Key解密(Key=SobeyMAM)
        /// </summary>
        /// <param name="text">要解密的密文</param>
        /// <returns>解密的明文</returns>
        public static string Decrypt(string text)
        {
            return Decrypt(text, "DM");
        }

        /// <summary> 
        /// 解密数据 
        /// </summary> 
        /// <param name="text">要解密的密文</param> 
        /// <param name="sKey">密钥</param> 
        /// <returns>解密的明文</returns> 
        public static string Decrypt(string text, string sKey)
        {
            byte[] dataKey = Convert.FromBase64String(text);
            byte[] encryptData = Decrypt(dataKey, sKey, Iv);
            return Encoding.UTF8.GetString(encryptData, 0, encryptData.Length);
        }

        /// <summary>
        /// 解密数据 
        /// </summary> 
        /// <param name="text">要解密的密文</param> 
        /// <param name="sKey">密钥</param> 
        /// <param name="vector">向量</param>
        /// <returns>解密的明文</returns>
        public static string Decrypt(string text, string sKey, string vector)
        {
            byte[] dataKey = Convert.FromBase64String(text);
            byte[] encryptData = Decrypt(dataKey, sKey, vector);
            return Encoding.UTF8.GetString(encryptData, 0, encryptData.Length);
        }

        /// <summary>
        /// 解密数据 
        /// </summary> 
        /// <param name="data">要解密的密文</param> 
        /// <param name="sKey">密钥</param>
        /// <returns>解密的明文</returns>
        public static string Decrypt(byte[] data, string sKey)
        {
            byte[] dataKey = data;
            byte[] encryptData = Decrypt(dataKey, sKey, Iv);
            return Encoding.UTF8.GetString(encryptData, 0, encryptData.Length);
        }

        #region AES解密byte[]

        /// <summary>
        /// AES解密byte[]
        /// </summary>
        /// <param name="data">被解密的密文</param>
        /// <param name="key">密钥</param>
        /// <param name="vector">向量</param>
        /// <returns>明文</returns>
        public static byte[] Decrypt(byte[] data, string key, string vector)
        {
            var bKey = new Byte[16];
            Array.Copy(Encoding.UTF8.GetBytes(key.PadRight(bKey.Length)), bKey, bKey.Length);
            var bVector = new Byte[16];
            Array.Copy(Encoding.UTF8.GetBytes(vector.PadRight(bVector.Length)), bVector, bVector.Length);

            byte[] original; // 解密后的明文

            var aes = new AesManaged();

            try
            {
                aes.Key = bKey;
                aes.IV = bVector;
                //aes.Mode = CipherMode.CBC;
                //aes.Padding = PaddingMode.PKCS7;
                ICryptoTransform cTransform = aes.CreateDecryptor();
                byte[] resultArray  = cTransform.TransformFinalBlock(data, 0, data.Length);

                original = resultArray;
            }
            catch
            {
                original = null;
            }

            return original;
        }
        #endregion

        #endregion
    }
}