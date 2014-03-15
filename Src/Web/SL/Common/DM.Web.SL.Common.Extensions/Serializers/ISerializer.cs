using System.IO;
using System.Text;

namespace DM.Web.SL.Common.Extensions.Serializers
{
    /// <summary>
    ///   序列化服务服务接口
    /// </summary>
    public interface ISerializer
    {
        #region Methods

        /// <summary>
        ///   将字符串反序列化为对象
        /// </summary>
        /// <typeparam name = "T">对象类型</typeparam>
        /// <param name = "data">字符串</param>
        /// <param name = "encoder">编码</param>
        /// <returns></returns>
        T Deserialize<T>(string data, Encoding encoder);

        /// <summary>
        ///   将字符串反序列化为对象
        /// </summary>
        /// <typeparam name = "T">对象类型</typeparam>
        /// <param name = "data">字符串</param>
        /// <returns></returns>
        T Deserialize<T>(string data);

        /// <summary>
        ///   将字符串反序列化为对象
        /// </summary>
        /// <param name = "data">字符串</param>
        /// <param name = "encoder">编码</param>
        /// <returns></returns>
        object Deserialize(string data, Encoding encoder);

        /// <summary>
        ///   将字符串反序列化为对象
        /// </summary>
        /// <param name = "data">字符串</param>
        /// <returns></returns>
        object Deserialize(string data);

        /// <summary>
        ///   将字节数组反序列化为对象
        /// </summary>
        /// <typeparam name = "T">对象类型</typeparam>
        /// <param name = "data">字节数组</param>
        /// <returns></returns>
        T Deserialize<T>(byte[] data);

        /// <summary>
        ///   将字节数组反序列化为对象
        /// </summary>
        /// <param name = "data">字节数组</param>
        /// <returns></returns>
        object Deserialize(byte[] data);

        /// <summary>
        ///   将内存流反序列化为对象
        /// </summary>
        /// <typeparam name = "T">对象类型</typeparam>
        /// <param name = "ms">System.IO.MemoryStream</param>
        /// <returns></returns>
        T Deserialize<T>(MemoryStream ms);

        /// <summary>
        ///   将内存流反序列化为对象
        /// </summary>
        /// <param name = "ms">System.IO.MemoryStream</param>
        /// <returns></returns>
        object Deserialize(MemoryStream ms);

        /// <summary>
        ///   将对象序列化为字节数组
        /// </summary>
        /// <typeparam name = "T">对象类型</typeparam>
        /// <param name = "obj">对象值</param>
        /// <returns>字节数组</returns>
        byte[] SerializeToByteArray<T>(T obj);

        /// <summary>
        ///   将对象序列化为内存流
        /// </summary>
        /// <typeparam name = "T">对象类型</typeparam>
        /// <param name = "obj">对象值</param>
        /// <returns>内存流</returns>
        MemoryStream SerializeToMemoryStream<T>(T obj);

        /// <summary>
        ///   将对象序列化为字符串
        /// </summary>
        /// <typeparam name = "T">对象类型</typeparam>
        /// <param name = "obj">对象值</param>
        /// <param name = "encoder">编码</param>
        /// <returns>字符串</returns>
        string SerializeToString<T>(T obj, Encoding encoder);

        /// <summary>
        ///   将对象序列化为字符串
        /// </summary>
        /// <typeparam name = "T">对象类型</typeparam>
        /// <param name = "obj">对象值</param>
        /// <returns>字符串</returns>
        string SerializeToString<T>(T obj);

        #endregion Methods
    }
}