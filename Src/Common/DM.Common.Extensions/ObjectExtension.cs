using System;

namespace DM.Common.Extensions
{
    /// <summary>
    ///   封装一些对象的扩展方法
    /// </summary>
    public static class ObjectExtension
    {
        /// <summary>
        ///   将一个对象转化为Int型数据，为空时时返回0
        /// </summary>
        /// <param name = "obj">要转化为Int型数据的对象</param>
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
        ///   将一个对象转化为日期型数据
        /// </summary>
        /// <param name = "obj">要进行转化的对象</param>
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
        ///   将一个对象转化为逻辑性数据
        /// </summary>
        /// <param name = "obj">要进行转化的对象</param>
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
        ///   将一个对象转化为实数类型
        /// </summary>
        /// <param name = "obj">要进行转化的对象</param>
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
        ///   转化为实数类型，发生异常时返回默认，而不报错
        /// </summary>
        /// <param name = "obj">要进行转化的对象</param>
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