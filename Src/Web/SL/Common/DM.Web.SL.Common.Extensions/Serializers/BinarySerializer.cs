using System.IO;
using System.Text;

namespace DM.Web.SL.Common.Extensions.Serializers
{
    /// <summary>
    ///   Binary序列化、反序列化服务
    /// </summary>
    internal class BinarySerializer : ISerializer
    {
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
            return Deserialize<T>(encoder.GetBytes(data));
        }

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

        /// <summary>
        ///   将字符串反序列化为对象
        /// </summary>
        /// <param name = "data">字符串</param>
        /// <returns></returns>
        public object Deserialize(string data)
        {
            return Deserialize(Encoding.UTF8.GetBytes(data));
        }

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

                {
                    return Deserialize<T>(ms);
                }
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
        ///   将字节数组反序列化为对象
        /// </summary>
        /// <param name = "data">字节数组</param>
        /// <returns></returns>
        public object Deserialize(byte[] data)
        {
            MemoryStream ms = null;

            try
            {
                ms = new MemoryStream(data, 0, data.Length, false);

                {
                    return Deserialize(ms);
                }
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
        /// <typeparam name = "T">对象类型</typeparam>
        /// <param name = "ms">System.IO.MemoryStream</param>
        /// <returns></returns>
        public T Deserialize<T>(MemoryStream ms)
        {
                if (ms != null)
                {
                    return (T) BinaryHelper.Instance.Deserialize(ms);
                }
                return default(T);
        }

        /// <summary>
        ///   将内存流反序列化为对象
        /// </summary>
        /// <param name = "ms">System.IO.MemoryStream</param>
        /// <returns></returns>
        public object Deserialize( MemoryStream ms )
        {
            if (ms != null)
            {
                return BinaryHelper.Instance.Deserialize(ms);
            }
            return new object();
        }

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

        /// <summary>
        ///   将对象序列化为内存流
        /// </summary>
        /// <typeparam name = "T">对象类型</typeparam>
        /// <param name = "obj">对象值</param>
        /// <returns>内存流</returns>
        public MemoryStream SerializeToMemoryStream<T>( T obj )
        {
            MemoryStream ms = BinaryHelper.Instance.SerializeToMemoryStream(obj);
            return ms;
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
            byte[] x = SerializeToByteArray(obj);

            string returnStr = encoder.GetString(x, 0, x.Length);

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
            byte[] x = SerializeToByteArray(obj);

            string returnStr = Encoding.UTF8.GetString(x, 0, x.Length);

            return returnStr;
        }

        #endregion Methods
    }

    /// <summary>
    ///   BinaryHelper
    /// </summary>
    internal class BinaryHelper
    {
        #region Fields

        /// <summary>
        ///   单一实例
        /// </summary>
        public static BinaryHelper Instance = new BinaryHelper();

        private readonly BinarySerializer _bs = new BinarySerializer();

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
            return _bs.Deserialize<T>(strData);
        }

        /// <summary>
        ///   将字符流反序列化为对象
        /// </summary>
        /// <param name = "ms">源字符流</param>
        /// <returns>反序列化后的对象</returns>
        public object Deserialize(MemoryStream ms)
        {
            return _bs.Deserialize(ms);
        }

        /// <summary>
        ///   将对象序列化为字符串
        /// </summary>
        /// <param name = "obj">源对象</param>
        /// <returns>序列化后的字符串</returns>
        public string Serialize<T>(T obj)
        {
            return _bs.SerializeToString(obj);
        }

        /// <summary>
        ///   将对象序列化为字符流
        /// </summary>
        /// <typeparam name = "T">类型</typeparam>
        /// <param name = "obj">源对象</param>
        /// <returns>序列化后的字符流</returns>
        public MemoryStream SerializeToMemoryStream<T>(T obj)
        {
            return _bs.SerializeToMemoryStream(obj);
        }

        #endregion Methods
    }
}