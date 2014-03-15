using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DM.Web.SL.Common.Extensions.Serializers
{
    /// <summary>
    ///   Json字符串 序列化或反序列化
    /// </summary>
    internal class JsonSerializer : ISerializer
    {
        #region Fields

        private readonly JsonHelper _jss;

        #endregion Fields

        #region Constructors

        public JsonSerializer()
        {
            _jss = JsonHelper.Instance;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        ///   将字符串反序列化为对象
        /// </summary>
        /// <typeparam name = "T">对象类型</typeparam>
        /// <param name = "data">字符串</param>
        /// <param name = "encoder">编码</param>
        /// <returns></returns>
        public T Deserialize<T>(string data, Encoding encoder)
        {
            byte[] buffer = encoder.GetBytes(data);
            string dataStr = encoder.GetString(buffer, 0, buffer.Length);

            return Deserialize<T>(dataStr);
        }

        /// <summary>
        ///   将字符串反序列化为对象
        /// </summary>
        /// <typeparam name = "T">对象类型</typeparam>
        /// <param name = "data">字符串</param>
        /// <returns></returns>
        public T Deserialize<T>( string data )
        {
            byte[] buffer = Encoding.UTF8.GetBytes(data);
            string dataStr = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
            return _jss.Deserialize<T>(dataStr);
        }

        /// <summary>
        ///   将字符串反序列化为对象
        /// </summary>
        /// <param name = "data">字符串</param>
        /// <param name = "encoder">编码</param>
        /// <returns></returns>
        public object Deserialize(string data, Encoding encoder)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        ///   将字符串反序列化为对象
        /// </summary>
        /// <param name = "data">字符串</param>
        /// <returns></returns>
        public object Deserialize(string data)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        ///   将字节数组反序列化为对象
        /// </summary>
        /// <typeparam name = "T">对象类型</typeparam>
        /// <param name = "data">字节数组</param>
        /// <returns></returns>
        public T Deserialize<T>( byte[] data )
        {
            string dataStr = Encoding.UTF8.GetString(data, 0, data.Length);
            return Deserialize<T>(dataStr);
        }

        /// <summary>
        ///   将字节数组反序列化为对象
        /// </summary>
        /// <param name = "data">字节数组</param>
        /// <returns></returns>
        public object Deserialize(byte[] data)
        {
            throw new NotSupportedException();
        }

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
                return Deserialize<T>(ms.ToArray());
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

        /// <summary>
        ///   将内存流反序列化为对象
        /// </summary>
        /// <param name = "ms">System.IO.MemoryStream</param>
        /// <returns></returns>
        public object Deserialize(MemoryStream ms)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        ///   将对象序列化为字节数组
        /// </summary>
        /// <typeparam name = "T">对象类型</typeparam>
        /// <param name = "obj">对象值</param>
        /// <returns>字节数组</returns>
        public byte[] SerializeToByteArray<T>(T obj)
        {
            string x = SerializeToString(obj);

            if (!string.IsNullOrEmpty(x))
            {
                return Encoding.UTF8.GetBytes(x);
            }
            return new byte[0];
        }

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
                byte[] x = SerializeToByteArray(obj);
                ms = new MemoryStream(x, 0, x.Length, false);

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

        /// <summary>
        ///   将对象序列化为字符串
        /// </summary>
        /// <typeparam name = "T">对象类型</typeparam>
        /// <param name = "obj">对象值</param>
        /// <param name = "encoder">编码</param>
        /// <returns>字符串</returns>
        public string SerializeToString<T>( T obj, Encoding encoder )
        {
            byte[] buffer = encoder.GetBytes(SerializeToString(obj));
            string returnStr = encoder.GetString(buffer, 0, buffer.Length);

            return returnStr;

        }

        /// <summary>
        ///   将对象序列化为字符串
        /// </summary>
        /// <typeparam name = "T">对象类型</typeparam>
        /// <param name = "obj">对象值</param>
        /// <returns>字符串</returns>
        public string SerializeToString<T>( T obj )
        {
            string returnStr = _jss.Serialize(obj);

            return returnStr;
        }

        #endregion Methods
    }

    /// <summary>
    ///   JsonHelper
    /// </summary>
    internal class JsonHelper
    {
        #region Fields

        /// <summary>
        ///   单一实例
        /// </summary>
        public static JsonHelper Instance = new JsonHelper();

        #endregion Fields

        #region Methods

        /// <summary>
        ///   将字符串反序列化为对象
        /// </summary>
        /// <typeparam name = "T">类型</typeparam>
        /// <param name = "strData">源字符串</param>
        /// <returns>反序列化后的对象</returns>
        public T Deserialize<T>(string strData)
        {
            return JsonConvert.DeserializeObject<T>(strData);
        }

        /// <summary>
        ///   将对象序列化为字符串
        /// </summary>
        /// <param name = "obj">源对象</param>
        /// <returns>序列化后的字符串</returns>
        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.None, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }

        #endregion Methods
    }
}