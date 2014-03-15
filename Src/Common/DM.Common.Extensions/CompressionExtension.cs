using System;
using System.Text;
using System.IO;
using ComponentAce.Compression.Libs.zlib;

namespace DM.Common.Extensions
{
    /// <summary>
    /// 使用 SharpZipLib 进行压缩的辅助类，简化对字节数组和字符串进行压缩的操作。
    /// </summary>
    public static class CompressionExtension
    {
        /// <summary>
        /// 压缩字符串(对象->xml->UTF8二进制->压缩二进制->Base64字符串)
        /// </summary>
        /// <returns></returns>
        public static string ToCompressedStr<T>(this T obj)
        {
            return obj.ToXml().ToCompressedStr();
        }

        /// <summary>
        /// 压缩字符串(旧有方法，对象->二进制->压缩二进制->Base64字符串）
        /// </summary>
        /// <returns></returns>
        public static string ToCompressedStrOld<T>(this T obj)
        {
            return Convert.ToBase64String(obj.ToBytes().Compress());
        }

        /// <summary>
        /// 压缩字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToCompressedStr(this string str)
        {
            return Compress(str);
        }

        /// <summary>
        /// 解压缩字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToDeCompressedStr(this string str)
        {
            return DeCompress(str);
        }

        /// <summary>
        /// 解压缩字符串(旧有方法，Base64字符串->压缩二进制->普通二进制->对象）
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static T ToDecompressedObjectOld<T>(this string str) where T:class
        {
            return Convert.FromBase64String(str).DeCompress().ToObject<T>();
        }

        /// <summary>
        /// 解压缩字符串(Base64字符串->压缩二进制->UTF8二进制->xml->对象)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static T ToDeCompressedObject<T>(this string str) where T : class
        {
            return str.ToDeCompressedStr().ToObjectFromXml<T>();
        }

        /// <summary>
        /// 压缩供应者，默认为 GZip。
        /// </summary>
        public static CompressionType CompressionProvider = CompressionType.ZLib;

        #region public methods

        /// <summary>
        /// 从原始字节数组生成已压缩的字节数组。
        /// </summary>
        /// <param name="bytesToCompress">原始字节数组。</param>
        /// <param name="level">压缩等级</param>
        /// <returns>返回已压缩的字节数组</returns>
        public static byte[] Compress(this byte[] bytesToCompress, int level = 9)
        {
            using (var ms = new MemoryStream())
            {
                using (var zOut = new ZOutputStream(ms, level))
                {
                    zOut.Write(bytesToCompress, 0, bytesToCompress.Length);
                    zOut.finish();
                    return ms.ToArray();
                }
            }

            #region 以前的方法

            //MemoryStream ms = new MemoryStream();
            //Stream s = OutputStream(ms);
            //s.Write(bytesToCompress, 0, bytesToCompress.Length);
            //s.Close();
            //return ms.ToArray(); 

            #endregion
        }

        /// <summary>
        /// 从原始字符串生成已压缩的字符串。
        /// </summary>
        /// <param name="stringToCompress">原始字符串。</param>
        /// <returns>返回已压缩的字符串。</returns>
        public static string Compress(string stringToCompress)
        {
            byte[] compressedData = CompressToByte(stringToCompress);
            string strOut = Convert.ToBase64String(compressedData);
            return strOut;
        }
        /// <summary>
        /// 从原始字符串生成已压缩的字节数组。
        /// </summary>
        /// <param name="stringToCompress">原始字符串。</param>
        /// <returns>返回已压缩的字节数组。</returns>
        public static byte[] CompressToByte(string stringToCompress)
        {
            byte[] bytData = Encoding.UTF8.GetBytes(stringToCompress);
            return Compress(bytData);
        }

        /// <summary>
        /// 从已压缩的字符串生成原始字符串。
        /// </summary>
        /// <param name="stringToDecompress">已压缩的字符串。</param>
        /// <returns>返回原始字符串。</returns>
        public static byte[] DeCompressToByte(string stringToDecompress)
        {
            return DeCompress(Encoding.UTF8.GetBytes(stringToDecompress));
        }

        /// <summary>
        /// 从已压缩的字符串生成原始字符串。
        /// </summary>
        /// <param name="stringToDecompress">已压缩的字符串。</param>
        /// <returns>返回原始字符串。</returns>
        public static string DeCompress(string stringToDecompress)
        {
            string outString;
            if (stringToDecompress == null)
            {
                throw new ArgumentNullException("stringToDecompress", "You tried to use an empty string");
            }
            try
            {
                byte[] inArr = Convert.FromBase64String(stringToDecompress.Trim());
                outString = Encoding.UTF8.GetString(DeCompress(inArr));
            }
            catch (NullReferenceException nEx)
            {
                return nEx.Message;
            }
            return outString;
        }
        /// <summary>
        /// 从已压缩的字节数组生成原始字节数组。
        /// </summary>
        /// <param name="bytesToDecompress">已压缩的字节数组。</param>
        /// <returns>返回原始字节数组。</returns>
        public static byte[] DeCompress(this byte[] bytesToDecompress)
        {
            using (var ms = new MemoryStream())
            {
                using (var zOut = new ZOutputStream(ms))
                {
                    zOut.Write(bytesToDecompress, 0, bytesToDecompress.Length);
                    //zOut.finish();
                    return ms.ToArray();
                }
            }
        }
        #endregion
    }
}
