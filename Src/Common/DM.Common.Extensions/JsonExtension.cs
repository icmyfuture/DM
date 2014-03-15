using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Web.Script.Serialization;

namespace DM.Common.Extensions
{
    ///<summary>
    ///  解析JSON的扩展方法
    ///</summary>
    public static class JsonExtension
    {
        private static readonly JavaScriptSerializer Jss;

        static JsonExtension()
        {
            Jss = new JavaScriptSerializer();
        }

        /// <summary>
        ///   从JSON字符串获取对象
        /// </summary>
        /// <typeparam name = "TObject">需要转换的对象类型</typeparam>
        /// <param name = "str">json字符串</param>
        /// <returns></returns>
        public static TObject ToObject<TObject>(this string str) where TObject:class
        {
            return JsonConvert.DeserializeObject<TObject>(str);
        }

        /// <summary>
        ///   从JSON字符串获取对象
        /// </summary>
        /// <typeparam name = "TObject">需要转换的对象类型</typeparam>
        /// <param name = "str">json字符串</param>
        /// <returns></returns>
        public static TObject ToObject2<TObject>(this string str) where TObject : class
        {
            return Jss.Deserialize<TObject>(str);
        }

        /// <summary>
        ///   从JSON字符串获取对象(struct)
        /// </summary>
        /// <typeparam name = "TObject">需要转换的对象类型</typeparam>
        /// <param name = "str">json字符串</param>
        /// <returns></returns>
        public static TObject ToObjectAsStruct<TObject>(this string str) where TObject:struct
        {
            return JsonConvert.DeserializeObject<TObject>(str);
        }

        /// <summary>
        ///   从JSON字符串获取对象(Anonymous)
        /// </summary>
        /// <typeparam name = "TObject">需要转换的对象类型</typeparam>
        /// <param name = "str">json字符串</param>
        /// <param name = "obj"></param>
        /// <returns></returns>
        public static TObject ToObjectAsAnonymous<TObject>(this string str, TObject obj)
        {
            return JsonConvert.DeserializeAnonymousType(str, obj);
        }

        ///<summary>
        ///  将对象转为JSON字符串
        ///</summary>
        ///<param name = "obj">需要转换的对象</param>
        ///<typeparam name = "TObject">对象类型</typeparam>
        ///<returns></returns>
        public static string ToJson2<TObject>(this TObject obj)
        {
            return Jss.Serialize(obj);
        }

        ///<summary>
        ///  将对象转为JSON字符串
        ///</summary>
        ///<param name = "obj">需要转换的对象</param>
        ///<typeparam name = "TObject">对象类型</typeparam>
        ///<returns></returns>
        public static string ToJson<TObject>(this TObject obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.None, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }

        ///<summary>
        ///  将对象转为JSON字符串
        ///</summary>
        ///<param name = "obj">需要转换的对象</param>
        ///<typeparam name = "TObject">对象类型</typeparam>
        ///<returns></returns>
        public static string ToJsonPascalType<TObject>(this TObject obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.None);
        }
    }
}