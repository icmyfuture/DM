using System;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;

namespace DM.Common.Extensions
{
    /// <summary>
    /// 字符串处理帮助类
    /// </summary>
    public static class StringExtension
    {
        #region 字符串过长截取

        /// <summary>
        /// 双字节判断
        /// </summary>
        private static readonly Regex DoubleByteRegex = new Regex("[^\x00-\xff]");

        /// <summary>
        /// 大小写判断
        /// </summary>
        private static readonly Regex CaseRegex = new Regex("[A-H]|[M-Z]");

        /// <summary>
        /// 得到字符串真实长度, 1个汉字长度为2,一个大写算1.5个
        /// </summary>
        /// <param name="input">源字符串</param>
        /// <param name="nums">字符数组</param>
        /// <returns>字符长度</returns>
        public static int GetStringLength(this string input, out Char[] nums)
        {
            double num = 0;
            nums = input.ToCharArray();
            for (var i = 0; i < nums.Length; i++)
            {
                var m = DoubleByteRegex.Match(nums[i].ToString(CultureInfo.InvariantCulture));
                if (m.Success)
                {
                    num += 2;
                }
                else
                {
                    var m2 = CaseRegex.Match(nums[i].ToString(CultureInfo.InvariantCulture));
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
        /// 字符串截断
        /// </summary>
        /// <param name="input">源字符串</param>
        /// <param name="length">长度</param>
        /// <param name="substitute">超过长度的标示，如“...”，不填就不会再后面打点</param>
        /// <returns></returns>
        public static string SubCutString(this string input, int length, string substitute = "...")
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }
            //初始化字符串的字符数组
            Char[] arr;
            //如果字符串的真实长度小于或者等于要截取的字符串长度就直接返回源字符串
            if (GetStringLength(input, out arr) <= length)
            {
                return input;
            }
            //获取不包括substitute的长度
            if (string.IsNullOrEmpty(substitute))
            {
                length -= Encoding.UTF8.GetBytes("...").Length;
            }
            else
            {
                length -= Encoding.UTF8.GetBytes(substitute).Length;
            }
            //分析出来的字符字节长度
            double byteLength = 0;
            //拼装字符串类
            var subBuilder = new StringBuilder();
            //循环判断每个字符是属于双字节还是单字节(大写英文字母为1.5个字节)
            for (var i = 0; i < arr.Length; i++)
            {
                //判断双字节
                var doubleByteMatch = DoubleByteRegex.Match(arr[i].ToString(CultureInfo.InvariantCulture));
                if (doubleByteMatch.Success)
                {
                    byteLength += 2;
                }
                else
                {
                    //判断是否为大写
                    var caseMatch = CaseRegex.Match(arr[i].ToString(CultureInfo.InvariantCulture));
                    if (caseMatch.Success)
                    {
                        byteLength += 1.5;
                    }
                    else
                    {
                        byteLength++;
                    }
                }
                if (byteLength > length)
                {
                    break;
                }
                subBuilder.Append(arr[i]);
            }
            subBuilder.Append(substitute);
            return subBuilder.ToString();
        }

        /// <summary>
        /// 字符串截断
        /// </summary>
        /// <param name="input">源字符串</param>
        /// <param name="length">长度</param>
        /// <param name="substitute">超过长度的标示，如“...”，不填就不会再后面打点</param>
        /// <returns></returns>
        public static string SubCutString2(this string input, int length, string substitute = "...")
        {
            var ascii = new ASCIIEncoding();
            var tempLen = 0;
            var tempString = "";
            var s = ascii.GetBytes(input);
            for (var i = 0; i < s.Length; i++)
            {
                if (s[i] == 63)
                {
                    tempLen += 2;
                }
                else
                {
                    tempLen += 1;
                }
                try
                {
                    tempString += input.Substring(i, 1);
                }
                catch
                {
                    break;
                }
                if (tempLen > length)
                    break;
            }
            var mybyte = Encoding.Default.GetBytes(input);
            if (mybyte.Length > length)
                tempString += substitute;
            return tempString;
        }

        #endregion

        #region 半角全角转换

        /// <summary>
        ///  转半角的函数(SBC case)
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns></returns>
        public static string ToDBC(this string input)
        {
            var c = input.ToCharArray();
            for (var i = 0; i < c.Length; i++)
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
        /// 转全角的函数(SBC case)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToSBC(this string input)
        {
            //半角转全角：
            var c = input.ToCharArray();
            for (var i = 0; i < c.Length; i++)
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

        #endregion

        #region 获取汉字拼音首字母

        /// <summary> 
        /// 在指定的字符串列表cnStr中检索符合拼音索引字符串 
        /// </summary> 
        /// <param name="cnStr">汉字字符串</param> 
        /// <returns>相对应的汉语拼音首字母串</returns> 
        public static string GetSpellCode(this string cnStr)
        {
            var strTemp = string.Empty;
            var iLen = cnStr.Length;
            for (var i = 0; i <= iLen - 1; i++)
            {
                strTemp += GetCharSpellCode(cnStr.Substring(i, 1));
            }
            return strTemp;
        }

        /// <summary> 
        /// 得到一个汉字的拼音第一个字母，如果是一个英文字母则直接返回大写字母 
        /// </summary> 
        /// <param name="cnChar">单个汉字</param> 
        /// <returns>单个大写字母</returns> 
        private static string GetCharSpellCode(this string cnChar)
        {
            var zw = Encoding.Default.GetBytes(cnChar);
            //如果是字母，则直接返回 
            if (zw.Length == 1)
            {
                return cnChar.ToUpper();
            }
            //获取单一字符的字节数组
            int i1 = zw[0];
            int i2 = zw[1];
            long iCnChar = i1 * 256 + i2;
            //expresstion 
            //table of the constant list 
            // 'A'; //45217..45252 
            // 'B'; //45253..45760 
            // 'C'; //45761..46317 
            // 'D'; //46318..46825 
            // 'E'; //46826..47009 
            // 'F'; //47010..47296 
            // 'G'; //47297..47613 

            // 'H'; //47614..48118 
            // 'J'; //48119..49061 
            // 'K'; //49062..49323 
            // 'L'; //49324..49895 
            // 'M'; //49896..50370 
            // 'N'; //50371..50613 
            // 'O'; //50614..50621 
            // 'P'; //50622..50905 
            // 'Q'; //50906..51386 

            // 'R'; //51387..51445 
            // 'S'; //51446..52217 
            // 'T'; //52218..52697 
            //没有U,V 
            // 'W'; //52698..52979 
            // 'X'; //52980..53640 
            // 'Y'; //53689..54480 
            // 'Z'; //54481..55289 

            // iCnChar match the constant 
            if ((iCnChar >= 45217) && (iCnChar <= 45252))
            {
                return "A";
            }
            if ((iCnChar >= 45253) && (iCnChar <= 45760))
            {
                return "B";
            }
            if ((iCnChar >= 45761) && (iCnChar <= 46317))
            {
                return "C";
            }
            if ((iCnChar >= 46318) && (iCnChar <= 46825))
            {
                return "D";
            }
            if ((iCnChar >= 46826) && (iCnChar <= 47009))
            {
                return "E";
            }
            if ((iCnChar >= 47010) && (iCnChar <= 47296))
            {
                return "F";
            }
            if ((iCnChar >= 47297) && (iCnChar <= 47613))
            {
                return "G";
            }
            if ((iCnChar >= 47614) && (iCnChar <= 48118))
            {
                return "H";
            }
            if ((iCnChar >= 48119) && (iCnChar <= 49061))
            {
                return "J";
            }
            if ((iCnChar >= 49062) && (iCnChar <= 49323))
            {
                return "K";
            }
            if ((iCnChar >= 49324) && (iCnChar <= 49895))
            {
                return "L";
            }
            if ((iCnChar >= 49896) && (iCnChar <= 50370))
            {
                return "M";
            }
            if ((iCnChar >= 50371) && (iCnChar <= 50613))
            {
                return "N";
            }
            if ((iCnChar >= 50614) && (iCnChar <= 50621))
            {
                return "O";
            }
            if ((iCnChar >= 50622) && (iCnChar <= 50905))
            {
                return "P";
            }
            if ((iCnChar >= 50906) && (iCnChar <= 51386))
            {
                return "Q";
            }
            if ((iCnChar >= 51387) && (iCnChar <= 51445))
            {
                return "R";
            }
            if ((iCnChar >= 51446) && (iCnChar <= 52217))
            {
                return "S";
            }
            if ((iCnChar >= 52218) && (iCnChar <= 52697))
            {
                return "T";
            }
            if ((iCnChar >= 52698) && (iCnChar <= 52979))
            {
                return "W";
            }
            if ((iCnChar >= 52980) && (iCnChar <= 53640))
            {
                return "X";
            }
            if ((iCnChar >= 53689) && (iCnChar <= 54480))
            {
                return "Y";
            }
            if ((iCnChar >= 54481) && (iCnChar <= 55289))
            {
                return "Z";
            }
            return ("?");
        }

        #endregion

        #region 简繁体转换

        /// <summary>
        /// 将字符串转换为简体中文
        /// </summary>
        /// <param name="traditionalInput">繁体</param>
        /// <returns>简体</returns>
        public static string ToSimplifiedChinese(this string traditionalInput)
        {
            return Strings.StrConv(traditionalInput, VbStrConv.SimplifiedChinese);
        }

        /// <summary>
        /// 将字符串转换为繁体中文
        /// </summary>
        /// <param name="simplifiedInput">简体</param>
        /// <returns>繁体</returns>
        public static string ToTraditionalChinese(this string simplifiedInput)
        {
            return Strings.StrConv(simplifiedInput, VbStrConv.TraditionalChinese);
        }

        /// <summary>
        /// 将文件转换为简体中文
        /// </summary>
        /// <param name="filename">源文件名</param>
        /// <param name="outFilename">目标文件名</param>
        public static void ToSimplifiedChinese(this string filename, string outFilename)
        {
            using (var r = new StreamReader(filename, Encoding.GetEncoding("gb2312")))
            {
                using (var w = new StreamWriter(outFilename, false, Encoding.GetEncoding("gb2312")))
                {
                    w.Write(Strings.StrConv(r.ReadToEnd(), VbStrConv.SimplifiedChinese));
                    w.Flush();
                }
            }
        }

        /// <summary>
        /// 将文件转换为繁体中文
        /// </summary>
        /// <param name="filename">源文件名</param>
        /// <param name="outFilename">目标文件名</param>
        public static void ToTraditionalChinese(this string filename, string outFilename)
        {
            using (var r = new StreamReader(filename, Encoding.GetEncoding("gb2312")))
            {
                using (var w = new StreamWriter(outFilename, false, Encoding.GetEncoding("gb2312")))
                {
                    w.Write(Strings.StrConv(r.ReadToEnd(), VbStrConv.TraditionalChinese));
                    w.Flush();
                }
            }
        }

        #endregion

        #region 正则表达式

        /// <summary>
        /// 指示正则表达式使用pattern参数中制定的正则表达式是否在输入字符串中找到匹配项
        /// </summary>
        /// <param name="input">要搜索匹配项的字符串</param>
        /// <param name="pattern">要匹配的正则表达式模式</param>
        /// <returns></returns>
        public static bool IsMatch(this string input, string pattern)
        {
            return !input.IsNullOrEmpty() && Regex.IsMatch(input, pattern);
        }

        /// <summary>
        /// 在指定的输入字符串中搜索pattern参数中提供的正则表达式的匹配项
        /// </summary>
        /// <param name="input">要搜索匹配项的字符串</param>
        /// <param name="pattern">要匹配的正则表达式模式</param>
        /// <returns></returns>
        public static string Match(this string input, string pattern)
        {
            return input.IsNullOrEmpty()
                       ? string.Empty
                       : Regex.Match(input, pattern).Value;
        }

        /// <summary>
        /// 在指定的输入字符串内，使用指定的替换字符串替换于指定正则表达式匹配的所有字符串
        /// </summary>
        /// <param name="input">要搜索匹配项的字符串</param>
        /// <param name="pattern">要匹配的正则表达式模式</param>
        /// <param name="replacement">替换字符串</param>
        /// <returns></returns>
        public static string Replace(this string input, string pattern, string replacement)
        {
            return input.IsNullOrEmpty()
                       ? string.Empty
                       : Regex.Replace(input, pattern, replacement);
        }

        /// <summary>
        /// 在指定的输入字符串内，使用指定的替换字符串替换于指定正则表达式匹配的所有字符串
        /// </summary>
        /// <param name="input">要搜索匹配项的字符串</param>
        /// <param name="pattern">要匹配的正则表达式模式</param>
        /// <param name="replacement">替换字符串</param>
        /// <param name="options">提供用于设置正则表达式选项的枚举值</param>
        /// <returns></returns>
        public static string Replace(this string input, string pattern, string replacement, RegexOptions options)
        {
            return input.IsNullOrEmpty()
                       ? string.Empty
                       : Regex.Replace(input, pattern, replacement, options);
        }

        /// <summary>
        /// 在指定的输入字符串内，替换开始和结束字符串中间的值
        /// </summary>
        /// <param name="input"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="replacement">替换字符串</param>
        /// <returns></returns>
        public static string ReplaceStartToEnd(this string input, string start, string end, string replacement)
        {
            return input.Replace("(?<=(" + start + "))[.\\s\\S]*?(?=(" + end + "))", replacement, RegexOptions.Multiline | RegexOptions.Singleline);
        }

        #endregion

        #region 数据类型转换和验证

        /// <summary>
        /// 指示指定的对象是否是Int32
        /// </summary>
        /// <param name="input">字符串</param>
        /// <returns></returns>
        public static bool IsInt32(this string input)
        {
            Int32 i;
            return Int32.TryParse(input, out i);
        }

        /// <summary>
        /// 指示指定的对象是否是Int64（long）
        /// </summary>
        /// <param name="input">字符串</param>
        /// <returns></returns>
        public static bool IsInt64(this string input)
        {
            Int64 i;
            return Int64.TryParse(input, out i);
        }

        /// <summary>
        /// 指示指定的对象是否是DateTime
        /// </summary>
        /// <param name="input">字符串</param>
        /// <returns></returns>
        public static bool IsDateTime(this string input)
        {
            DateTime dateTime;
            return DateTime.TryParse(input, out dateTime);
        }

        /// <summary>
        /// 指示指定的对象是否是Boolean
        /// </summary>
        /// <param name="input">字符串</param>
        /// <returns></returns>
        public static bool IsBoolean(this string input)
        {
            Boolean b;
            return Boolean.TryParse(input, out b);
        }

        /// <summary>
        /// 指示指定的对象是否是Decimal
        /// </summary>
        /// <param name="input">字符串</param>
        /// <returns></returns>
        public static bool IsDecimal(this string input)
        {
            decimal de;
            return decimal.TryParse(input, out de);
        }

        /// <summary>
        /// 指示指定的对象是否是Double
        /// </summary>
        /// <param name="input">字符串</param>
        /// <returns></returns>
        public static bool IsDouble(this string input)
        {
            double d;
            return double.TryParse(input, out d);
        }

        /// <summary>
        /// 将指定的Base64字符串转化为字节数组
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte[] FromBase64String(this string input)
        {
            return Convert.FromBase64String(input);
        }

        /// <summary>
        /// 验证Email格式
        /// </summary>
        /// <param name="email">邮件地址</param>
        /// <returns></returns>
        public static bool IsValidEmailAddress(this string email)
        {
            return email.IsMatch(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
        }

        #endregion

        #region 帕斯卡和骆驼命名法

        /// <summary>
        /// 转换为骆驼命名法（第一个单词首字母小写。）
        /// </summary>
        /// <param name="input">源字符串</param>
        /// <returns></returns>
        public static string ToCamel(this string input)
        {
            if (input.IsNullOrEmpty())
            {
                return input;
            }
            return input[0].ToString(CultureInfo.InvariantCulture).ToLower() + input.Substring(1);
        }

        /// <summary>
        /// 转换为帕斯卡命名法（第一个单词首字母大写。）
        /// </summary>
        /// <param name="input">源字符串</param>
        /// <returns></returns>
        public static string ToPascal(this string input)
        {
            if (input.IsNullOrEmpty())
            {
                return input;
            }
            return input[0].ToString(CultureInfo.InvariantCulture).ToUpper() + input.Substring(1);
        }

        #endregion

        #region 算法

        /// <summary>
        /// 计算字符串的 MD5 哈希。若字符串为空，则返回空，否则返回计算结果。
        /// </summary>
        public static string ComputeMd5Hash(this string str)
        {
            var hash = str;
            if (str != null)
            {
                var md5 = new MD5CryptoServiceProvider();
                byte[] data = Encoding.ASCII.GetBytes(str);
                data = md5.ComputeHash(data);
                hash = "";
                for (var i = 0; i < data.Length; i++)
                {
                    hash += data[i].ToString("x2");
                }
            }
            return hash;
        }

        #endregion

        #region 其余扩展方法

        /// <summary>
        /// 指示指定的对象是Null还是String.Empty字符串
        /// </summary>
        /// <param name="input">字符串</param>
        /// <returns>如果为Null反True，否则反False</returns>
        public static bool IsNullOrEmpty(this string input)
        {
            return string.IsNullOrEmpty(input);
        }

        /// <summary>
        /// 将指定字符串中的格式项替换为指定数组中相应参数实例的值的文本等效项
        /// </summary>
        /// <param name="format">符合格式字符串</param>
        /// <param name="args">要格式化的参数</param>
        /// <returns></returns>
        public static string FormatWith(this string format, params object[] args)
        {
            return string.Format(format, args);
        }

        /// <summary>
        /// 删除最后结尾的指定字符后的字符
        /// </summary>
        /// <param name="input">源字符串</param>
        /// <param name="strchar">分隔符</param>
        /// <returns>去掉结尾字符后的字符串</returns>
        public static string DelLastChar(this string input, string strchar)
        {
            return input.Substring(0, input.LastIndexOf(strchar, StringComparison.Ordinal));
        }

        /// <summary>
        /// 字符串比较，忽略大小写
        /// </summary>
        /// <param name="strA">字符串A</param>
        /// <param name="strB">字符串B</param>
        /// <returns>比较结果</returns>
        public static bool CompareWith(this string strA, string strB)
        {
            return string.Compare(strA, strB, StringComparison.OrdinalIgnoreCase) == 1;
        }
        #endregion
    }
}