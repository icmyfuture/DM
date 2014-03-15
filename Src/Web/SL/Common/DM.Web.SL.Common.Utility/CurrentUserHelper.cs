using DM.Web.SL.Common.Core;
using DM.Web.SL.Common.Extensions;
using DM.Web.SL.Common.Utility.Encrypt;
using System.Globalization;
using System.Threading;

namespace DM.Web.SL.Common.Utility
{
    /// <summary>
    ///   操作类
    /// </summary>
    public class CurrentUserHelper
    {
        #region Fields

        private static string cookieKey = @"System_CurrentUser";

        #endregion Fields

        #region Methods

        /// <summary>
        ///   获取当前用户信息
        /// </summary>
        /// <returns></returns>
        public static UserInfo GetCurrentUserInfo()
        {
            string fileContent = CookieHelper.GetCookie(cookieKey);
            if (!string.IsNullOrEmpty(fileContent))
            {
                try
                {
                    fileContent = AESEncrypt.Decrypt(fileContent); //DES解密
                    return fileContent.ToObject<UserInfo>();
                }
                catch
                {
                    CookieHelper.DeleteCookie(cookieKey);
                    return null;
                }
            }
            return null;
        }

        /// <summary>
        ///   设置当前用户信息
        /// </summary>
        /// <param name = "user">用户信息</param>
        public static void SetCurrentUserInfo(UserInfo user)
        {
            string fileContent = AESEncrypt.Encrypt(user.ToJson()); //DES加密
            CookieHelper.SetCookie(cookieKey, fileContent);
        }

        /// <summary>
        ///   设置当前用户语言信息
        /// </summary>
        /// <param name = "lang">语言信息</param>
        public static void SetCurrentUserLangInfo(LanguageInfo lang)
        {
            UserInfo user = GetCurrentUserInfo();
            if (user == null)
            {
                user = new UserInfo();
            }

            CultureInfo culture = LanguageHelper.GetCultureInfo(lang);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            //时间格式全采用yyyy-MM-dd HH:mm:ss格式 
            Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
            Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortTimePattern = "HH:mm:ss";

            user.Language = lang;
            SetCurrentUserInfo(user);
        }

        #endregion Methods
    }
}