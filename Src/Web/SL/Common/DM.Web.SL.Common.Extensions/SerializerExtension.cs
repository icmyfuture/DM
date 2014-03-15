using System;
using DM.Web.SL.Common.Extensions.Serializers;

namespace DM.Web.SL.Common.Extensions
{
    /// <summary>
    ///   扩展方法
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        ///   从JSON字符串获取对象
        /// </summary>
        /// <typeparam name = "TObject">需要转换的对象类型</typeparam>
        /// <param name = "str">json字符串</param>
        /// <returns></returns>
        public static TObject ToObject<TObject>(this string str) where TObject : class
        {
            return SerializerProxy.JsonSerialize.Deserialize<TObject>(str);
        }

        ///<summary>
        ///  将对象转为JSON字符串
        ///</summary>
        ///<param name = "obj">需要转换的对象</param>
        ///<typeparam name = "TObject">对象类型</typeparam>
        ///<returns></returns>
        public static string ToJson<TObject>(this TObject obj)
        {
            return SerializerProxy.JsonSerialize.SerializeToString(obj);
        }

        /// <summary>
        ///   从XML字符串获取对象
        /// </summary>
        /// <typeparam name = "TObject">需要转换的对象类型</typeparam>
        /// <param name = "str">json字符串</param>
        /// <returns></returns>
        public static TObject ToObjectFromXml<TObject>(this string str) where TObject : class
        {
            return SerializerProxy.XmlSerialize.Deserialize<TObject>(str);
        }

        ///<summary>
        ///  将对象转为XML字符串
        ///</summary>
        ///<param name = "obj">需要转换的对象</param>
        ///<typeparam name = "TObject">对象类型</typeparam>
        ///<returns></returns>
        public static string ToXml<TObject>(this TObject obj)
        {
            return SerializerProxy.XmlSerialize.SerializeToString(obj);
        }

        /// <summary>
        ///   安全对象
        /// </summary>
        /// <typeparam name = "TObject">对象类型</typeparam>
        /// <param name = "obj">对象自己</param>
        /// <returns>永远不会未将对象XXXXXX的对象</returns>
        public static TObject SafeObject<TObject>(this TObject obj) where TObject : class, new()
        {
            return obj ?? new TObject();
        }

        /// <summary>
        /// 将一个对象转化为Int型数据，为空时时返回0
        /// </summary>
        /// <param name="obj">要转化为Int型数据的对象</param>
        /// <returns>Int型数据，若转化失败返回0</returns>
        public static int ToInt32<T>(this T obj)
        {
            try
            {
                return Convert.ToInt32(obj);
            }
            catch
            {
                return default(int);
            }
        }


        /// <summary>
        /// 将一个对象转化为日期型数据
        /// </summary>
        /// <param name="obj">要进行转化的对象</param>
        /// <returns>返回时间型数据,若转化失败,则返回DateTime的默认值(1900-1-1)</returns>
        public static DateTime ToDateTime<T>(this T obj)
        {
            try
            {
                return Convert.ToDateTime(obj);
            }
            catch
            {
                return "1900-1-1".ToDateTime();
            }
        }


        /// <summary>
        /// 将一个对象转化为逻辑性数据
        /// </summary>
        /// <param name="obj">要进行转化的对象</param>
        /// <returns>返回布尔值,若转化失败,返回布尔型的默认值</returns>
        public static bool ToBoolean<T>(this T obj)
        {
            try
            {
                return Convert.ToBoolean(obj);
            }
            catch
            {
                return default(bool);
            }
        }

        /// <summary>
        /// 将一个对象转化为实数类型
        /// </summary>
        /// <param name="obj">要进行转化的对象</param>
        /// <returns>返回实数类型,若转化失败,返回实数的默认值</returns>
        public static decimal ToDecimal<T>(this T obj)
        {
            try
            {
                return Convert.ToDecimal(obj);
            }
            catch
            {
                return default(decimal);
            }
        }

        /// <summary>
        /// 转化为实数类型，发生异常时返回默认，而不报错
        /// </summary>
        /// <param name="obj">要进行转化的对象</param>
        /// <returns>返回实数类型,若转化失败,返回实数的默认值</returns>
        public static double ToDouble<T>(this T obj)
        {

            try
            {
                return Convert.ToDouble(obj);
            }
            catch
            {
                return default(double);
            }
        }

    }
}