using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace DM.Common.Extensions
{
    /// <summary>
    ///   内存流的扩展
    /// </summary>
    public static class StreamExtension
    {
        /// <summary>
        ///   从二进制流获取对象
        /// </summary>
        /// <typeparam name = "TObject">需要转换的对象类型</typeparam>
        /// <param name = "stream">流</param>
        /// <returns></returns>
        public static TObject ToData<TObject>(this MemoryStream stream) where TObject:class
        {
            using (stream)
            {
                stream.Position = 0;
                var deserializer = new BinaryFormatter();
                var temp = deserializer.Deserialize(stream);
                return temp as TObject;
            }
        }

        /// <summary>
        ///   从二进制流获取对象
        /// </summary>
        /// <typeparam name = "TObject">需要转换的对象类型</typeparam>
        /// <param name = "stream">流</param>
        /// <returns></returns>
        public static TObject ToDataAsStruct<TObject>(this MemoryStream stream) where TObject : struct
        {
            using (stream)
            {
                stream.Position = 0;
                var deserializer = new BinaryFormatter();
                var temp = deserializer.Deserialize(stream);
                return (TObject)temp;
            }
        }

        ///<summary>
        ///  将对象转为二进制流
        ///</summary>
        ///<param name = "obj">需要转换的对象</param>
        ///<typeparam name = "TObject">对象类型</typeparam>
        ///<returns></returns>
        public static MemoryStream ToStream<TObject>(this TObject obj)
        {
            var serializer = new BinaryFormatter();
            var stream = new MemoryStream();
            serializer.Serialize(stream, obj);
            return stream;
        }

        /// <summary>
        /// 以内存流的方式实现的深拷贝
        /// </summary>
        /// <typeparam name="TObject">对象类型</typeparam>
        /// <param name="obj">需要拷贝的对象</param>
        /// <returns></returns>
        public static TObject DeepClone<TObject>(this TObject obj) where TObject : class
        {
            return obj.ToStream().ToData<TObject>();
        }
    }
}