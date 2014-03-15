using System.Text.RegularExpressions;

namespace DM.Web.SL.Common.Utility.Data
{
    /// <summary>
    ///   数据验证
    /// </summary>
    public abstract class DataValidator
    {
        #region Fields

        private static Regex RegCHZN = new Regex("[\u4e00-\u9fa5]");
        private static Regex RegDecimal = new Regex("^[0-9]+[.]?[0-9]+$");
        private static Regex RegDecimalSign = new Regex("^[+-]?[0-9]+[.]?[0-9]+$"); //等价于^[+-]?\d+[.]?\d+$
        private static Regex RegEmail = new Regex("^[\\w-]+@[\\w-]+\\.(com|net|org|edu|mil|tv|biz|info)$"); //w 英文字母或数字的字符串，和 [a-zA-Z0-9] 语法一样
        private static Regex RegNumber = new Regex("^[0-9]+$");
        private static Regex RegNumberSign = new Regex("^[+-]?[0-9]+$");
        private static Regex RegIP = new Regex(@"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$");
        private static Regex RegURL = new Regex(@"^http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$");

        #endregion Fields

        #region Methods

        /// <summary>
        ///   是否是区域码
        /// </summary>
        /// <param name = "input"></param>
        /// <returns></returns>
        public static bool IsAreaCode(string input)
        {
            return ((IsNumber(input) && (input.Length >= 3)) && (input.Length <= 5));
        }

        /// <summary>
        ///   是否是Decimal
        /// </summary>
        /// <param name = "input"></param>
        /// <returns></returns>
        public static bool IsDecimal(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }
            Match m = RegDecimal.Match(input);
            return m.Success;
        }

        /// <summary>
        ///   是否是Decimal(包括正负)
        /// </summary>
        /// <param name = "input"></param>
        /// <returns></returns>
        public static bool IsDecimalSign(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }
            Match m = RegDecimalSign.Match(input);
            return m.Success;
        }

        /// <summary>
        ///   是否是Email地址
        /// </summary>
        /// <param name = "input"></param>
        /// <returns></returns>
        public static bool IsEmail(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }
            Match m = RegEmail.Match(input);
            return m.Success;
        }

        /// <summary>
        ///   检测是否有中文字符
        /// </summary>
        /// <param name = "inputData"></param>
        /// <returns></returns>
        public static bool IsHasChzn(string inputData)
        {
            if (string.IsNullOrEmpty(inputData))
            {
                return false;
            }
            Match m = RegCHZN.Match(inputData);
            return m.Success;
        }

        /// <summary>
        ///   是否是IP地址
        /// </summary>
        /// <param name = "input"></param>
        /// <returns></returns>
        public static bool IsIP(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }
            Match m = RegIP.Match(input);
            return m.Success;
        }

        /// <summary>
        ///   是否是数字
        /// </summary>
        /// <param name = "input"></param>
        /// <returns></returns>
        public static bool IsNumber(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }
            Match m = RegNumber.Match(input);
            return m.Success;
        }

        /// <summary>
        ///   是否是数字(包括正负)
        /// </summary>
        /// <param name = "input"></param>
        /// <returns></returns>
        public static bool IsNumberSign(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }
            Match m = RegNumberSign.Match(input);
            return m.Success;
        }

        /// <summary>
        ///   否是是邮政编码
        /// </summary>
        /// <param name = "input"></param>
        /// <returns></returns>
        public static bool IsPostCode(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }
            return (IsNumber(input) && (input.Length == 6));
        }

        /// <summary>
        ///   是否是URL连接
        /// </summary>
        /// <param name = "input"></param>
        /// <returns></returns>
        public static bool IsUrl(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }
            Match m = RegURL.Match(input);
            return m.Success;
        }

        /// <summary>
        ///   是否是合法的用户名
        ///   长度小于20，不包括\\/\"[]:|&lt;&gt;+=;,?*@
        /// </summary>
        /// <param name = "userName">用户名</param>
        /// <returns></returns>
        public static bool IsValidUserName(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return false;
            }
            if (userName.Length > 20)
            {
                return false;
            }
            if (userName.Trim().Length == 0)
            {
                return false;
            }
            if (userName.Trim(new[] {'.'}).Length == 0)
            {
                return false;
            }
            string str = "\\/\"[]:|<>+=;,?*@";
            for (int i = 0; i < userName.Length; i++)
            {
                if (str.IndexOf(userName[i]) >= 0)
                {
                    return false;
                }
            }
            return true;
        }

        #endregion Methods
    }
}