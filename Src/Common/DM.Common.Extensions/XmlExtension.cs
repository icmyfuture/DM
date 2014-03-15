using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace DM.Common.Extensions
{
    /// <summary>
    ///   XML序列扩展
    /// </summary>
    public static class XmlExtension
    {
        /// <summary>
        ///   将XML字符串转为对象
        /// </summary>
        /// <typeparam name = "TObject"></typeparam>
        /// <param name = "str"></param>
        /// <returns></returns>
        public static TObject ToObjectFromXml<TObject>(this string str) where TObject:class
        {
            var temp = Encoding.UTF8.GetBytes(str);
            using (var mstream = new MemoryStream(temp))
            {
                var serializer = new XmlSerializer(typeof (TObject));
                return serializer.Deserialize(mstream) as TObject;
            }
        }

        /// <summary>
        ///   将XML字符串转为对象(struct)
        /// </summary>
        /// <typeparam name = "TObject"></typeparam>
        /// <param name = "str"></param>
        /// <returns></returns>
        public static TObject ToObjectAsStructFromXml<TObject>(this string str) where TObject:struct
        {
            var temp = Encoding.UTF8.GetBytes(str);
            using (var mstream = new MemoryStream(temp))
            {
                var serializer = new XmlSerializer(typeof (TObject));
                return (TObject) serializer.Deserialize(mstream);
            }
        }

        /// <summary>
        ///   将对象转为XML字符串
        /// </summary>
        /// <typeparam name = "TObject"></typeparam>
        /// <param name = "obj"></param>
        /// <returns></returns>
        public static string ToXml<TObject>(this TObject obj)
        {
            using (var mstream = new MemoryStream())
            {
                var serializer = new XmlSerializer(typeof (TObject));
                serializer.Serialize(mstream, obj);
                return Encoding.UTF8.GetString(mstream.ToArray());
            }
        }
    }
}