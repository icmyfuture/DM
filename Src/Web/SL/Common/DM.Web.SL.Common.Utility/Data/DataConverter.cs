using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Windows.Media;

namespace DM.Web.SL.Common.Utility.Data
{
    /// <summary>
    ///   数据转换类
    /// </summary>
    public static class DataConverter
    {
        #region Bool

        /// <summary>
        ///   string 转换为 Bool
        /// </summary>
        /// <param name = "input"></param>
        /// <returns></returns>
        public static bool CBoolean(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }
            input = input.Trim();
            if (((string.Compare(input, "true", StringComparison.OrdinalIgnoreCase) != 0) && (string.Compare(input, "yes", StringComparison.OrdinalIgnoreCase) != 0)) && (string.Compare(input, "1", StringComparison.OrdinalIgnoreCase) != 0))
            {
                return false;
            }
            return true;
        }

        #endregion

        #region 时间

        /// <summary>
        ///   转换为时间
        /// </summary>
        /// <param name = "input"></param>
        /// <returns></returns>
        public static DateTime CDate(object input)
        {
            if (!Convert.IsDBNull(input) && !Equals(input, null))
            {
                return CDate(input.ToString());
            }
            return DateTime.Now;
        }

        /// <summary>
        ///   转换为时间
        /// </summary>
        /// <param name = "input">输入字符串</param>
        /// <returns></returns>
        public static DateTime CDate(string input)
        {
            DateTime now;
            if (!DateTime.TryParse(input, out now))
            {
                now = DateTime.Now;
            }
            return now;
        }

        /// <summary>
        ///   转换为时间
        /// </summary>
        /// <param name = "input">输入字符串</param>
        /// <param name = "outTime">返回</param>
        /// <returns></returns>
        public static DateTime? CDate(string input, DateTime? outTime)
        {
            DateTime time;
            if (!DateTime.TryParse(input, out time))
            {
                return outTime;
            }
            return time;
        }

        /// <summary>
        ///   转换为时间
        /// </summary>
        /// <param name = "input">输入字符串</param>
        /// <param name = "outTime">返回</param>
        /// <returns></returns>
        public static DateTime CDate(string input, DateTime outTime)
        {
            DateTime time;
            if (!DateTime.TryParse(input, out time))
            {
                return outTime;
            }
            return time;
        }

        /// <summary>
        ///   时间格式化
        /// </summary>
        /// <param name = "input"></param>
        /// <returns>yyyy-MM-dd格式</returns>
        public static string CDateString(string input)
        {
            DateTime time;
            if (!DateTime.TryParse(input, out time))
            {
                return string.Empty;
            }
            return time.ToString("yyyy-MM-dd");
        }

        #endregion

        #region Decimal

        /// <summary>
        ///   ToDecimal
        /// </summary>
        /// <param name = "input"></param>
        /// <returns></returns>
        public static decimal CDecimal(object input)
        {
            return CDecimal(input, 0M);
        }

        /// <summary>
        ///   ToDecimal
        /// </summary>
        /// <param name = "input"></param>
        /// <returns></returns>
        public static decimal CDecimal(string input)
        {
            return CDecimal(input, 0M);
        }

        /// <summary>
        ///   ToDecimal
        /// </summary>
        /// <param name = "input">要转换的值</param>
        /// <param name = "defaultValue">默认值</param>
        /// <returns></returns>
        public static decimal CDecimal(object input, decimal defaultValue)
        {
            if (!Convert.IsDBNull(input) && !Equals(input, null))
            {
                return CDecimal(input.ToString(), defaultValue);
            }
            return 0M;
        }

        /// <summary>
        ///   ToDecimal
        /// </summary>
        /// <param name = "input">要转换的值</param>
        /// <param name = "defaultValue">默认值</param>
        /// <returns></returns>
        public static decimal CDecimal(string input, decimal defaultValue)
        {
            decimal num;
            if (!decimal.TryParse(input, out num))
            {
                num = defaultValue;
            }
            return num;
        }

        #endregion

        #region Double

        /// <summary>
        ///   ToDouble
        /// </summary>
        /// <param name = "input"></param>
        /// <returns></returns>
        public static double CDouble(object input)
        {
            return CDouble(input, 0.0);
        }

        /// <summary>
        ///   ToDouble
        /// </summary>
        /// <param name = "input"></param>
        /// <returns></returns>
        public static double CDouble(string input)
        {
            return CDouble(input, 0.0);
        }

        /// <summary>
        ///   ToDouble
        /// </summary>
        /// <param name = "input"></param>
        /// <param name = "defaultValue">默认值</param>
        /// <returns></returns>
        public static double CDouble(object input, double defaultValue)
        {
            if (!Convert.IsDBNull(input) && !Equals(input, null))
            {
                return CDouble(input.ToString(), defaultValue);
            }
            return 0.0;
        }

        /// <summary>
        ///   ToDouble
        /// </summary>
        /// <param name = "input"></param>
        /// <param name = "defaultValue"></param>
        /// <returns></returns>
        public static double CDouble(string input, double defaultValue)
        {
            double num;
            if (!double.TryParse(input, out num))
            {
                return defaultValue;
            }
            return num;
        }

        #endregion

        #region Int

        /// <summary>
        ///   ToInt
        /// </summary>
        /// <param name = "input"></param>
        /// <returns></returns>
        public static int CInt(object input)
        {
            return CInt(input, 0);
        }

        /// <summary>
        ///   ToInt
        /// </summary>
        /// <param name = "input"></param>
        /// <returns></returns>
        public static int CInt(string input)
        {
            return CInt(input, 0);
        }

        /// <summary>
        ///   ToInt
        /// </summary>
        /// <param name = "input"></param>
        /// <param name = "defaultValue"></param>
        /// <returns></returns>
        public static int CInt(object input, int defaultValue)
        {
            if (!Convert.IsDBNull(input) && !Equals(input, null))
            {
                return CInt(input.ToString(), defaultValue);
            }
            return defaultValue;
        }

        /// <summary>
        ///   ToInt
        /// </summary>
        /// <param name = "input"></param>
        /// <param name = "defaultValue"></param>
        /// <returns></returns>
        public static int CInt(string input, int defaultValue)
        {
            int num;
            if (!int.TryParse(input, out num))
            {
                num = defaultValue;
            }
            return num;
        }

        #endregion

        #region Long

        /// <summary>
        ///   ToLong
        /// </summary>
        /// <param name = "input"></param>
        /// <returns></returns>
        public static long CLong(object input)
        {
            return CLong(input, 0);
        }

        /// <summary>
        ///   ToLong
        /// </summary>
        /// <param name = "input"></param>
        /// <returns></returns>
        public static long CLong(string input)
        {
            return CLong(input, 0);
        }

        /// <summary>
        ///   ToLong
        /// </summary>
        /// <param name = "input"></param>
        /// <param name = "defaultValue"></param>
        /// <returns></returns>
        public static long CLong(object input, int defaultValue)
        {
            if (!Convert.IsDBNull(input) && !Equals(input, null))
            {
                return CLongByString(input.ToString(), defaultValue);
            }
            return defaultValue;
        }

        /// <summary>
        ///   ToLong
        /// </summary>
        /// <param name = "input"></param>
        /// <param name = "defaultValue"></param>
        /// <returns></returns>
        public static long CLongByString(string input, long defaultValue)
        {
            double num = CDouble(input, defaultValue);
            //以下算法不适用于有小数位的转换
            //if ( !Int64.TryParse( input, out num ) )
            //{
            //    num = defaultValue;
            //}
            return (long) num;
        }

        #endregion

        #region Float

        /// <summary>
        ///   ToFloat
        /// </summary>
        /// <param name = "input"></param>
        /// <returns></returns>
        public static float CFloat(object input)
        {
            return CFloat(input, 0f);
        }

        /// <summary>
        ///   ToFloat
        /// </summary>
        /// <param name = "input"></param>
        /// <returns></returns>
        public static float CFloat(string input)
        {
            return CFloat(input, 0f);
        }

        /// <summary>
        ///   ToFloat
        /// </summary>
        /// <param name = "input"></param>
        /// <param name = "defaultValue">默认值</param>
        /// <returns></returns>
        public static float CFloat(object input, float defaultValue)
        {
            if (!Convert.IsDBNull(input) && !Equals(input, null))
            {
                return CFloat(input.ToString(), defaultValue);
            }
            return 0f;
        }

        /// <summary>
        ///   ToFloat
        /// </summary>
        /// <param name = "input"></param>
        /// <param name = "defaultValue">默认值</param>
        /// <returns></returns>
        public static float CFloat(string input, float defaultValue)
        {
            float num;
            if (!float.TryParse(input, out num))
            {
                num = defaultValue;
            }
            return num;
        }

        #endregion

        #region 转换IList<T>为List<T>

        /// <summary>
        ///   转换IList为指定类型的List
        /// </summary>
        /// <typeparam name = "T">指定的集合中泛型的类型</typeparam>
        /// <param name = "gbList">需要转换的IList</param>
        /// <returns></returns>
        public static List<T> ConvertIListToList<T>(IList gbList) where T:class
        {
            List<T> list = new List<T>();
            if (gbList != null && gbList.Count > 0)
            {
                for (int i = 0; i < gbList.Count; i++)
                {
                    T temp = gbList[i] as T;
                    if (temp != null)
                    {
                        list.Add(temp);
                    }
                }
            }
            return list;
        }

        #endregion 转换IList<T>为List<T>

        #region 将字符串转换为Color

        /// <summary>
        ///   将字符串转换为Color
        /// </summary>
        /// <param name = "colorString">颜色值</param>
        /// <returns></returns>
        public static Color ToColor(string colorString)
        {
            Color myColor = new Color();
            if (colorString.StartsWith("#"))
            {
                colorString = colorString.Replace("#", string.Empty);

                switch (colorString.Length)
                {
                    case 6:
                        if (colorString.StartsWith("#"))
                        {
                            colorString = colorString.Insert(1, "FF");
                        }
                        else
                        {
                            colorString = "FF" + colorString;
                        }
                        myColor = ToColor(colorString);
                        break;
                    case 8:
                        try
                        {
                            int v = int.Parse(colorString, NumberStyles.HexNumber);
                            myColor = new Color()
                            {
                                A = Convert.ToByte((v >> 24) & 255),
                                R = Convert.ToByte((v >> 16) & 255),
                                G = Convert.ToByte((v >> 8) & 255),
                                B = Convert.ToByte((v >> 0) & 255)
                            };
                        }
                        catch
                        {}
                        break;
                }
            }
            else
            {
                Type colorType = (typeof (Colors));
                if (colorType.GetProperty(colorString) != null)
                {
                    object color = colorType.InvokeMember(colorString, BindingFlags.GetProperty, null, null, null);
                    if (color != null)
                    {
                        myColor = (Color) color;
                    }
                }
            }
            return myColor;
        }

        #endregion
    }
}