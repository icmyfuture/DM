using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DM.Common.Utility
{
    #region Imports
    #endregion

    /// <summary>
    ///   字符串处理帮助类
    ///   2009-6-23
    /// </summary>
    public class StringHelper
    {
        #region Methods

        /// <summary>
        ///   删除最后结尾的指定字符后的字符
        /// </summary>
        /// <param name = "str">源字符串</param>
        /// <param name = "strchar">分隔符</param>
        /// <returns>去掉结尾字符后的字符串</returns>
        public static string DelLastChar(string str, string strchar)
        {
            return str.Substring(0, str.LastIndexOf(strchar, StringComparison.Ordinal));
        }

        /// <summary>
        ///   删除最后结尾的一个逗号
        /// </summary>
        /// <param name = "str">源字符串</param>
        /// <returns>去掉结尾一个逗号后的字符串</returns>
        public static string DelLastComma(string str)
        {
            return str.Substring(0, str.LastIndexOf(",", StringComparison.Ordinal));
        }

        /// <summary>
        ///   字符串数组转换为字符串
        /// </summary>
        /// <param name = "list">字符数组</param>
        /// <param name = "speater">分隔符</param>
        /// <returns>字符串</returns>
        public static string GetArrayStr(List<string> list, string speater)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                if (i == list.Count - 1)
                {
                    sb.Append(list[i]);
                }
                else
                {
                    sb.Append(list[i]);
                    sb.Append(speater);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        ///   获取一个汉字的拼音声母
        /// </summary>
        /// <param name = "chinese">Unicode格式的一个汉字</param>
        /// <returns>汉字的声母</returns>
        public static String GetChineseOneIndex(String chinese)
        {
            var buffer = new char[chinese.Length];
            for (int i = 0; i < chinese.Length; i++)
            {
                buffer[i] = GetChineseOneIndex(chinese[i]);
            }
            return new String(buffer);
        }

        /// <summary>
        ///   获取一个汉字的拼音声母
        /// </summary>
        /// <param name = "chinese">Unicode格式的一个汉字</param>
        /// <returns>汉字的声母</returns>
        public static char GetChineseOneIndex(Char chinese)
        {
            Encoding gb2312 = Encoding.GetEncoding("GB2312");
            Encoding unicode = Encoding.Unicode;

            //   Convert   the   string   into   a   byte[].
            byte[] unicodeBytes = unicode.GetBytes(new[] { chinese });
            //   Perform   the   conversion   from   one   encoding   to   the   other.
            byte[] asciiBytes = Encoding.Convert(unicode, gb2312, unicodeBytes);

            //   计算该汉字的GB-2312编码
            int n = asciiBytes[0] << 8;
            n += asciiBytes[1];

            //   根据汉字区域码获取拼音声母
            if (In(0xB0A1, 0xB0C4, n))
            {
                return 'a';
            }
            if (In(0XB0C5, 0XB2C0, n))
            {
                return 'b';
            }
            if (In(0xB2C1, 0xB4ED, n))
            {
                return 'c';
            }
            if (In(0xB4EE, 0xB6E9, n))
            {
                return 'd';
            }
            if (In(0xB6EA, 0xB7A1, n))
            {
                return 'e';
            }
            if (In(0xB7A2, 0xB8c0, n))
            {
                return 'f';
            }
            if (In(0xB8C1, 0xB9FD, n))
            {
                return 'g';
            }
            if (In(0xB9FE, 0xBBF6, n))
            {
                return 'h';
            }
            if (In(0xBBF7, 0xBFA5, n))
            {
                return 'j';
            }
            if (In(0xBFA6, 0xC0AB, n))
            {
                return 'k';
            }
            if (In(0xC0AC, 0xC2E7, n))
            {
                return 'l';
            }
            if (In(0xC2E8, 0xC4C2, n))
            {
                return 'm';
            }
            if (In(0xC4C3, 0xC5B5, n))
            {
                return 'n';
            }
            if (In(0xC5B6, 0xC5BD, n))
            {
                return 'o';
            }
            if (In(0xC5BE, 0xC6D9, n))
            {
                return 'p';
            }
            if (In(0xC6DA, 0xC8BA, n))
            {
                return 'q';
            }
            if (In(0xC8BB, 0xC8F5, n))
            {
                return 'r';
            }
            if (In(0xC8F6, 0xCBF0, n))
            {
                return 's';
            }
            if (In(0xCBFA, 0xCDD9, n))
            {
                return 't';
            }
            if (In(0xCDDA, 0xCEF3, n))
            {
                return 'w';
            }
            if (In(0xCEF4, 0xD188, n))
            {
                return 'x';
            }
            if (In(0xD1B9, 0xD4D0, n))
            {
                return 'y';
            }
            if (In(0xD4D1, 0xD7F9, n))
            {
                return 'z';
            }
            return '\0';
        }

        /// <summary>
        ///   获取一串汉字的拼音声母
        /// </summary>
        /// <param name = "chinese">Unicode格式的汉字字符串</param>
        /// <returns>拼音声母字符串</returns>
        /// <example>
        ///   “新桥软件”转换为“xqrj”
        /// </example>
        public static String GetChineseStringIndex(String chinese)
        {
            var buffer = new char[chinese.Length];
            for (int i = 0; i < chinese.Length; i++)
            {
                buffer[i] = GetChineseOneIndex(chinese[i]);
            }
            return new String(buffer);
        }

        /// <summary>
        ///   掉分隔符
        /// </summary>
        /// <param name = "strList"></param>
        /// <param name = "splitString">分割字符</param>
        /// <returns></returns>
        public static string GetCleanStyle(string strList, string splitString)
        {
            string retrunValue;
            //如果为空，返回空值
            if (strList == null)
            {
                retrunValue = "";
            }
            else
            {
                //返回去掉分隔符
                string newString = strList.Replace(splitString, "");
                retrunValue = newString;
            }
            return retrunValue;
        }

        /// <summary>
        ///   将字符串转换为新样式
        /// </summary>
        /// <param name = "strList">源字符串</param>
        /// <param name = "newStyle">新样式</param>
        /// <param name = "splitString">分隔符</param>
        /// <param name = "error">错误信息</param>
        /// <returns></returns>
        public static string GetNewStyle(string strList, string newStyle, string splitString, out string error)
        {
            string returnValue;
            //如果输入空值，返回空，并给出错误提示
            if (strList == null)
            {
                returnValue = "";
                error = "请输入需要划分格式的字符串";
            }
            else
            {
                //检查传入的字符串长度和样式是否匹配,如果不匹配，则说明使用错误。给出错误信息并返回空值
                int strListLength = strList.Length;
                int newStyleLength = GetCleanStyle(newStyle, splitString).Length;
                if (strListLength != newStyleLength)
                {
                    returnValue = "";
                    error = "样式格式的长度与输入的字符长度不符，请重新输入";
                }
                else
                {
                    //检查新样式中分隔符的位置
                    string lengstr = "";
                    for (int i = 0; i < newStyle.Length; i++)
                    {
                        if (newStyle.Substring(i, 1) == splitString)
                        {
                            lengstr = lengstr + "," + i;
                        }
                    }
                    if (lengstr != "")
                    {
                        lengstr = lengstr.Substring(1);
                    }
                    //将分隔符放在新样式中的位置
                    string[] str = lengstr.Split(',');
                    strList = str.Aggregate(strList, (current, bb) => current.Insert(int.Parse(bb), splitString));
                    //给出最后的结果
                    returnValue = strList;
                    //因为是正常的输出，没有错误
                    error = "";
                }
            }
            return returnValue;
        }

        /// <summary>
        ///   分割字符串为字符串数组(分隔符为逗号)
        /// </summary>
        /// <param name = "str">源字符串</param>
        /// <returns>字符串数组</returns>
        public static string[] GetStrArray(string str)
        {
            return str.Split(new char[',']);
        }

        /// <summary>
        ///   得到字符串真实长度, 1个汉字长度为2,一个大写算1.5个
        /// </summary>
        /// <returns>字符长度</returns>
        public static int GetStringLength(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return 0;
            }

            double num = 0;
            var arr = str.ToCharArray();
            var r = new Regex("[^\x00-\xff]"); //判断双字节及大写
            var r2 = new Regex("[A-H]|[M-Z]");

            for (int i = 0; i < arr.Length; i++)
            {
                Match m = r.Match(arr[i].ToString(CultureInfo.InvariantCulture));
                if (m.Success)
                {
                    num += 2;
                }
                else
                {
                    Match m2 = r2.Match(arr[i].ToString(CultureInfo.InvariantCulture));
                    if (m2.Success)
                    {
                        num += 1.5;
                    }
                    else
                    {
                        num++;
                    }
                }
            }
            return (int)num;
        }

        /// <summary>
        ///   分割字符串为字符串数组(分隔符为逗号)
        /// </summary>
        /// <param name = "oStr"></param>
        /// <param name = "sepeater"></param>
        /// <returns></returns>
        public static List<string> GetSubStringList(string oStr, char sepeater)
        {
            string[] ss = oStr.Split(sepeater);
            return ss.Where(s => !string.IsNullOrEmpty(s) && s != sepeater.ToString(CultureInfo.InvariantCulture)).ToList();
        }

        /// <summary>
        ///   分割字符串为字符串数组(分隔符为逗号)
        /// </summary>
        /// <param name = "str">源字符串</param>
        /// <param name = "speater">分隔符</param>
        /// <param name = "toLower">是否转行为小写</param>
        /// <returns></returns>
        public static List<string> GetSubStringList(string str, char speater, bool toLower)
        {
            var list = new List<string>();
            string[] ss = str.Split(speater);
            foreach (string s in ss)
            {
                if (!string.IsNullOrEmpty(s) && s != speater.ToString(CultureInfo.InvariantCulture))
                {
                    string strVal = s;
                    if (toLower)
                    {
                        strVal = s.ToLower();
                    }
                    list.Add(strVal);
                }
            }
            return list;
        }

        /// <summary>
        ///   字符串截断
        /// </summary>
        /// <param name = "input">源字符串</param>
        /// <param name = "length">长度</param>
        /// <param name = "substitute">超过长度的标示，如“...”，不填就不会再后面打点</param>
        /// <returns></returns>
        public static string SubString(string input, int length, string substitute)
        {
            if (GetStringLength(input) <= length)
            {
                return input;
            }
            input = DataFilter.HtmlDecode(input);
            length -= (int)Math.Floor(Encoding.UTF8.GetBytes(substitute).Length / 1.5);
            double num = 0;
            var builder = new StringBuilder();
            var arr = input.ToCharArray();
            var r = new Regex("[^\x00-\xff]"); //判断双字节及大写
            var r2 = new Regex("[A-H]|[M-Z]");

            for (int i = 0; i < arr.Length; i++)
            {
                Match m = r.Match(arr[i].ToString(CultureInfo.InvariantCulture));
                if (m.Success)
                {
                    num += 2;
                }
                else
                {
                    Match m2 = r2.Match(arr[i].ToString(CultureInfo.InvariantCulture));
                    if (m2.Success)
                    {
                        num += 1.5;
                    }
                    else
                    {
                        num++;
                    }
                }
                if (num > length)
                {
                    break;
                }
                builder.Append(arr[i]);
            }
            builder.Append(substitute);
            return builder.ToString();
        }

        /// <summary>
        ///   转半角的函数(SBC case)
        /// </summary>
        /// <param name = "input">输入</param>
        /// <returns></returns>
        public static string ToDbc(string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                {
                    c[i] = (char)(c[i] - 65248);
                }
            }
            return new string(c);
        }

        /// <summary>
        ///   转全角的函数(SBC case)
        /// </summary>
        /// <param name = "input"></param>
        /// <returns></returns>
        public static string ToSbc(string input)
        {
            //半角转全角：
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                if (c[i] < 127)
                {
                    c[i] = (char)(c[i] + 65248);
                }
            }
            return new string(c);
        }

        private static bool In(int lp, int hp, int value)
        {
            return ((value <= hp) && (value >= lp));
        }

        #endregion Methods
    }
}