using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace DM.Web.SL.Common.Extensions.Serializers
{
    #region Imports

    

    #endregion

    /// <summary>
    ///   XML序列化、反序列化服务
    /// </summary>
    internal class XmlSerializer : ISerializer
    {
        #region 属性

        #endregion

        #region  序列化

        #region 将指定类型序列化为字符串

        #region 将对象序列化为字符串

        /// <summary>
        ///   将对象序列化为字符串
        /// </summary>
        /// <typeparam name = "T">对象类型</typeparam>
        /// <param name = "obj">对象值</param>
        /// <param name = "encoder">编码</param>
        /// <returns>字符串</returns>
        public string SerializeToString<T>( T obj, Encoding encoder )
        {
            byte[] x = SerializeToByteArray(obj);

            string returnStr = encoder.GetString(x, 0, x.Length);

            return returnStr;
        }

        #endregion

        #region 将对象序列化为字符串

        /// <summary>
        ///   将对象序列化为字符串
        /// </summary>
        /// <typeparam name = "T">对象类型</typeparam>
        /// <param name = "obj">对象值</param>
        /// <returns>字符串</returns>
        public string SerializeToString<T>(T obj)
        {
            MemoryStream ms = null;

            try
            {
                var xs = new System.Xml.Serialization.XmlSerializer(typeof(T));
                ms = new MemoryStream();
                //Create our own namespaces for the output   
                var ns = new XmlSerializerNamespaces();

                //Add an empty namespace and empty value   
                ns.Add("", "");

                xs.Serialize(ms, obj, ns);
                ms.Seek(0, SeekOrigin.Begin);
                TextReader tr = new StreamReader(ms);
                string returnStr = tr.ReadToEnd();

                return returnStr;
            }
            finally
            {
                if ( ms != null )
                {
                    ms.Close();
                    ms.Dispose();
                }
            }
        }

        #endregion

        #endregion

        #region 将指定类型对象序列化为字节数组

        /// <summary>
        ///   将对象序列化为字节数组
        /// </summary>
        /// <typeparam name = "T">对象类型</typeparam>
        /// <param name = "obj">对象值</param>
        /// <returns>字节数组</returns>
        public byte[] SerializeToByteArray<T>(T obj)
        {
            MemoryStream ms = null;

            try
            {
                ms = SerializeToMemoryStream(obj);

                byte[] returnBytes = ms != null ? ms.ToArray() : new byte[0];

                return returnBytes;
            }
            finally
            {
                if (ms != null)
                {
                    ms.Close();
                    ms.Dispose();
                }
            }
        }

        #endregion

        #region 将指定类型对象序列化为内存流

        /// <summary>
        ///   将对象序列化为内存流
        /// </summary>
        /// <typeparam name = "T">对象类型</typeparam>
        /// <param name = "obj">对象值</param>
        /// <returns>内存流</returns>
        public MemoryStream SerializeToMemoryStream<T>(T obj)
        {
            MemoryStream ms = null;

            try
            {
                var xs = new System.Xml.Serialization.XmlSerializer(typeof(T));
                ms = new MemoryStream();
                //Create our own namespaces for the output   
                var ns = new XmlSerializerNamespaces();

                //Add an empty namespace and empty value   
                ns.Add( "", "" );

                xs.Serialize( ms, obj, ns );

                return ms;
            }
            finally
            {
                if (ms != null)
                {
                    ms.Close();
                    ms.Dispose();
                }
            }
        }

        #endregion

        #endregion

        #region 反序列化

        #region 将字符串反序列化为指定类型对象

        #region 将字符串反序列化为对象

        /// <summary>
        ///   将字符串反序列化为对象
        /// </summary>
        /// <typeparam name = "T">对象类型</typeparam>
        /// <param name = "data">字符串</param>
        /// <param name = "encoder">编码</param>
        /// <returns></returns>
        public T Deserialize<T>(string data, Encoding encoder)
        {
            return Deserialize<T>(encoder.GetBytes(data));
        }

        #endregion

        #region 将字符串反序列化为对象

        /// <summary>
        ///   将字符串反序列化为对象
        /// </summary>
        /// <typeparam name = "T">对象类型</typeparam>
        /// <param name = "data">字符串</param>
        /// <returns></returns>
        public T Deserialize<T>(string data)
        {
            return Deserialize<T>(Encoding.UTF8.GetBytes(data));
        }

        #endregion

        #endregion

        #region 将字符串反序列化为对象

        #region 将字符串反序列化为对象

        /// <summary>
        ///   将字符串反序列化为对象
        /// </summary>
        /// <param name = "data">字符串</param>
        /// <param name = "encoder">编码</param>
        /// <returns></returns>
        public object Deserialize(string data, Encoding encoder)
        {
            return Deserialize(encoder.GetBytes(data));
        }

        #endregion

        #region 将字符串反序列化为对象

        /// <summary>
        ///   将字符串反序列化为对象
        /// </summary>
        /// <param name = "data">字符串</param>
        /// <returns></returns>
        public object Deserialize(string data)
        {
            return Deserialize(Encoding.UTF8.GetBytes(data));
        }

        #endregion

        #endregion

        #region 将字节数组反序列化为指定类型对象

        /// <summary>
        ///   将字节数组反序列化为对象
        /// </summary>
        /// <typeparam name = "T">对象类型</typeparam>
        /// <param name = "data">字节数组</param>
        /// <returns></returns>
        public T Deserialize<T>(byte[] data)
        {
            MemoryStream ms = null;

            try
            {
                ms = new MemoryStream(data, 0, data.Length, false);

                return Deserialize<T>(ms);
            }
            finally
            {
                if (ms != null)
                {
                    ms.Close();
                    ms.Dispose();
                }
            }
        }

        #endregion

        #region 将字节数组反序列化为对象

        /// <summary>
        ///   将字节数组反序列化为对象
        /// </summary>
        /// <param name = "data">字节数组</param>
        /// <returns></returns>
        public object Deserialize(byte[] data)
        {
            throw new NotSupportedException();
        }

        #endregion

        #region 将内存流反序列化为指定类型对象

        /// <summary>
        ///   将内存流反序列化为对象
        /// </summary>
        /// <typeparam name = "T">对象类型</typeparam>
        /// <param name = "ms">System.IO.MemoryStream</param>
        /// <returns></returns>
        public T Deserialize<T>(MemoryStream ms)
        {
            try
            {
                var xs = new System.Xml.Serialization.XmlSerializer(typeof(T));

                return (T) xs.Deserialize(ms);
            }
            finally
            {
                if (ms != null)
                {
                    ms.Close();
                    ms.Dispose();
                }
            }
        }

        #endregion

        #region 将内存流反序列化为对象

        /// <summary>
        ///   将内存流反序列化为对象
        /// </summary>
        /// <param name = "ms">System.IO.MemoryStream</param>
        /// <returns></returns>
        public object Deserialize(MemoryStream ms)
        {
            throw new NotSupportedException();
        }

        #endregion

        #endregion
    }
}