using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DM.Common.Utility
{
    /// <summary>
    ///   数据安全以及过滤
    /// </summary>
    public abstract class DataFilter
    {
        #region Methods

        /// <summary>
        ///   转换为JavaScript
        /// </summary>
        /// <param name = "str">要转换的字符串</param>
        /// <returns></returns>
        public static string ConvertToJavaScript(string str)
        {
            str = str.Replace(@"\", @"\\");
            str = str.Replace("\n", @"\n");
            str = str.Replace("\r", @"\r");
            str = str.Replace("\"", "\\\"");
            str = str.Replace("'", @"\'");
            return str;
        }

        /// <summary>
        ///   过滤脏字符串
        /// </summary>
        /// <param name = "strchar">要过滤的字符串</param>
        /// <returns></returns>
        public static string FilterBadChar(string strchar)
        {
            string input = string.Empty;
            if (string.IsNullOrEmpty(strchar))
            {
                return string.Empty;
            }
            string str = strchar;
            var strArray = new[]
            {
                "+", "'", "%", "^", "&", "?", "(", ")", "<", ">", "[", "]", "{", "}", "/", "\"",
                ";", ":", "Chr(34)", "Chr(0)", "--"
            };
            var builder = new StringBuilder(str);
            foreach (string t in strArray)
            {
                input = builder.Replace(t, string.Empty).ToString();
            }
            return Regex.Replace(input, "@+", "@");
        }

        /// <summary>
        ///   过滤SQL关键字
        /// </summary>
        /// <param name = "strchar">sql语句</param>
        /// <returns>过滤后的sql</returns>
        public static string FilterSqlKeyword(string strchar)
        {
            bool flag = false;
            if (string.IsNullOrEmpty(strchar))
            {
                return string.Empty;
            }
            strchar = strchar.ToUpper();
            var strArray = new[]
            {
                "SELECT", "UPDATE", "INSERT", "DELETE", "DECLARE", "@", "EXEC", "DBCC", "ALTER", "DROP", "CREATE", "BACKUP", "IF", "ELSE", "END", "AND",
                "OR", "ADD", "SET", "OPEN", "CLOSE", "USE", "BEGIN", "RETUN", "AS", "GO", "EXISTS", "KILL"
            };
            foreach (string t in strArray)
            {
                if (strchar.Contains(t))
                {
                    strchar = strchar.Replace(t, string.Empty);
                    flag = true;
                }
            }
            if (flag)
            {
                return FilterSqlKeyword(strchar);
            }
            return strchar;
        }

        /// <summary>
        ///   HTML解码
        /// </summary>
        /// <param name = "value"></param>
        /// <returns></returns>
        public static string HtmlDecode(object value)
        {
            if (value == null)
            {
                return null;
            }
            return HtmlDecode(value.ToString());
        }

        /// <summary>
        ///   HTML解码
        /// </summary>
        /// <param name = "value"></param>
        /// <returns></returns>
        public static string HtmlDecode(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                value = value.Replace("<br>", "\n");
                value = value.Replace("<br/>", "\n");
                value = value.Replace("<br />", "\n");
                value = value.Replace("&gt;", ">");
                value = value.Replace("&lt;", "<");
                value = value.Replace("&nbsp;", " ");
                value = value.Replace("&#39;", "'");
                value = value.Replace("&quot;", "\"");
            }
            return value;
        }

        /// <summary>
        ///   HTML编码
        /// </summary>
        /// <param name = "value"></param>
        /// <returns></returns>
        public static string HtmlEncode(object value)
        {
            if (value == null)
            {
                return null;
            }
            return HtmlEncode(value.ToString());
        }

        /// <summary>
        ///   HTML编码
        /// </summary>
        /// <param name = "str"></param>
        /// <returns></returns>
        public static string HtmlEncode(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                str = str.Replace("<", "&lt;");
                str = str.Replace(">", "&gt;");
                str = str.Replace(" ", "&nbsp;");
                str = str.Replace("'", "&#39;");
                str = str.Replace("\"", "&quot;");
                str = str.Replace("\r\n", "<br />");
                str = str.Replace("\n", "<br />");
            }
            return str;
        }

        /// <summary>
        ///   生成随机文件名(yyyyMMddHHmmss+4位随机)
        /// </summary>
        /// <returns></returns>
        public static string MakeFileRndName()
        {
            return (DateTime.Now.ToString("yyyyMMddHHmmss", CultureInfo.CurrentCulture) + RandomCodeHelper.MakeRandomString("0123456789", 4));
        }

        /// <summary>
        ///   生成文件名(yyyyMM)
        /// </summary>
        /// <returns></returns>
        public static string MakeFolderName()
        {
            return DateTime.Now.ToString("yyyyMM", CultureInfo.CurrentCulture);
        }

        /// <summary>
        ///   检查SQL注入
        /// </summary>
        /// <param name = "sqlstr">sql语句</param>
        /// <returns>有注入返回true/无注入返回false</returns>
        public static bool SqlCheck(string sqlstr)
        {
            const string sqlKeys = "exec↓select↓drop↓alter↓exists↓union↓and↓or↓xor↓order↓mid↓asc↓execute↓xp_cmdshell↓insert↓update↓delete↓join↓declare↓char↓sp_oacreate↓wscript.shell↓xp_regwrite↓'↓;↓--";
            string[] sqlArrs = sqlKeys.Split('↓');
            return sqlArrs.Any(sql => sqlstr.ToLower().Contains(sql));
        }

        /// <summary>
        ///   过滤SQL注入
        /// </summary>
        /// <param name = "sqlstr">原SQL</param>
        /// <returns>过滤后的SQL</returns>
        public static string SqlFilter(string sqlstr)
        {
            const string sqlKeys = "exec↓select↓drop↓alter↓exists↓union↓and↓or↓xor↓order↓mid↓asc↓execute↓xp_cmdshell↓insert↓update↓delete↓join↓declare↓char↓sp_oacreate↓wscript.shell↓xp_regwrite↓'↓;↓--";
            string[] sqlArrs = sqlKeys.Split('↓');
            return sqlArrs.Aggregate(sqlstr, (current, sqlkey) => current.Replace(sqlkey, ""));
        }

        /// <summary>
        ///   Xml编码
        /// </summary>
        /// <param name = "str"></param>
        /// <returns></returns>
        public static string XmlEncode(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                str = str.Replace("&", "&amp;");
                str = str.Replace("<", "&lt;");
                str = str.Replace(">", "&gt;");
                str = str.Replace("'", "&apos;");
                str = str.Replace("\"", "&quot;");
            }
            return str;
        }

        #endregion Methods
    }
}