using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace DM.Common.Extensions
{
    /// <summary>
    ///   字节数组的扩展
    /// </summary>
    public static class ByteExtension
    {
        /// <summary>
        ///   将对象转为字节数组
        /// </summary>
        /// <typeparam name = "TObject">对象</typeparam>
        /// <param name = "obj"></param>
        /// <returns></returns>
        public static byte[] ToBytes<TObject>(this TObject obj)
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                stream.Flush();
                return stream.ToArray();
            }
        }

        /// <summary>
        ///   将字节数组转换为对象
        /// </summary>
        /// <typeparam name = "TObject">需要转换的对象类型</typeparam>
        /// <param name = "bytes"></param>
        /// <returns></returns>
        public static TObject ToObject<TObject>(this byte[] bytes) where TObject:class
        {
            using (var stream = new MemoryStream(bytes, 0, bytes.Length, false))
            {
                var formatter = new BinaryFormatter();
                var data = formatter.Deserialize(stream);
                stream.Flush();
                return data as TObject;
            }
        }

        /// <summary>
        ///   将字节数组转换为对象(struct)
        /// </summary>
        /// <typeparam name = "TObject">需要转换的对象类型</typeparam>
        /// <param name = "bytes"></param>
        /// <returns></returns>
        public static TObject ToObjectAsStruct<TObject>(this byte[] bytes) where TObject:struct
        {
            using (var stream = new MemoryStream(bytes, 0, bytes.Length, false))
            {
                var formatter = new BinaryFormatter();
                var data = formatter.Deserialize(stream);
                stream.Flush();
                return (TObject) data;
            }
        }
    }
}