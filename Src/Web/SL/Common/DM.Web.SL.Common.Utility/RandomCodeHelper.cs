using System;
using System.Text;

namespace DM.Web.SL.Common.Utility
{
    /// <summary>
    ///   随机数操作类
    ///   Liujianming 2010-3-21
    /// </summary>
    public class RandomCodeHelper
    {
        #region Fields

        private static readonly Random Rnd = new Random();

        #endregion Fields


        #region Methods

        /// <summary>
        ///   从字符串里随机得到，规定个数的字符串.
        /// </summary>
        /// <param name = "allChar"></param>
        /// <param name = "codeCount"></param>
        /// <returns></returns>
        public static string GetRandomCode(string allChar, int codeCount)
        {
            //string allChar = "1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,i,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            string[] allCharArray = allChar.Split(',');
            string randomCode = "";
            int temp = -1;
            var rand = new Random();
            for (int i = 0; i < codeCount; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(temp * i * ((int) DateTime.Now.Ticks));
                }

                int t = rand.Next(allCharArray.Length - 1);

                while (temp == t)
                {
                    t = rand.Next(allCharArray.Length - 1);
                }

                temp = t;
                randomCode += allCharArray[t];
            }

            return randomCode;
        }

        /// <summary>
        ///   生成随机字符串
        /// </summary>
        /// <param name = "pwdlen">长度</param>
        /// <returns></returns>
        public static string MakeRandomString(int pwdlen)
        {
            return MakeRandomString("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", pwdlen);
        }

        /// <summary>
        ///   生成随机字符串
        /// </summary>
        /// <param name = "pwdchars">生成的串中包含的字符</param>
        /// <param name = "pwdlen">长度</param>
        /// <returns></returns>
        public static string MakeRandomString(string pwdchars, int pwdlen)
        {
            var builder = new StringBuilder();
            for (int i = 0; i < pwdlen; i++)
            {
                int num = Rnd.Next(pwdchars.Length);
                builder.Append(pwdchars[num]);
            }
            return builder.ToString();
        }

        /// <summary>
        ///   生成随机数
        /// </summary>
        /// <param name = "intlong"></param>
        /// <returns></returns>
        public static string RandomNum(int intlong)
        {
            var builder = new StringBuilder(string.Empty);
            for (int i = 0; i < intlong; i++)
            {
                builder.Append(Rnd.Next(10));
            }
            return builder.ToString();
        }

        #endregion Methods
    }
}