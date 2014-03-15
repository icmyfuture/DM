using System;
using System.Collections.Generic;
using DM.Web.SL.Common.Extensions;

namespace DM.Web.SL.Common.Utility
{
    /// <summary>
    ///   Cookie帮助类
    /// </summary>
    public static class CookieHelper
    {
        private const string CookiePath = @"System\cookie.txt";
        private static Dictionary<string, string> _cookiesCach = new Dictionary<string, string>();

        #region Methods

        /// <summary>
        ///   删除Cookie
        /// </summary>
        /// <param name = "key"></param>
        public static void DeleteCookie(String key)
        {
            //key = HttpUtility.UrlEncode(key);
            //DateTime expir = DateTime.UtcNow - TimeSpan.FromDays(1);
            //string cookie = String.Format("{0}=;expires={1}",
            //    key, expir.ToString("R"));
            //HtmlPage.Document.SetProperty("cookie", cookie);

            Dictionary<string, string> cookies = GetAllCookies();
            if (cookies.ContainsKey(key))
            {
                cookies.Remove(key);
                string content = cookies.ToJson();
                IsolatedStorageHelper.Instance.CreateFile(CookiePath, content, true);
            }
        }

        /// <summary>
        ///   判断是否存在
        /// </summary>
        /// <param name = "key"></param>
        /// <returns></returns>
        public static bool Exists(String key)
        {
            //string result = GetCookie( key );
            //return !string.IsNullOrEmpty( result );
            Dictionary<string, string> cookies = GetAllCookies();
            //如果为空或不存在就取IsolatedStorage中的值
            if (cookies.Count <= 0 || !cookies.ContainsKey(key))
            {
                cookies = GetAllCookiesFromIsolatedStorage();
            }

            return cookies.ContainsKey(key);
        }

        /// <summary>
        ///   获取Cookie
        /// </summary>
        /// <param name = "key">键值</param>
        /// <returns></returns>
        public static string GetCookie(String key)
        {
            //string result = null;
            //key = HttpUtility.UrlEncode(key);
            //String[] cookies = HtmlPage.Document.Cookies.Split(';');

            //foreach (string cookie in cookies)
            //{
            //    string[] keyValues=cookie.Split('=');
            //    if (keyValues.Length == 2 && keyValues[0].Trim() == key.Trim())
            //    {
            //        result = keyValues[1];
            //        result = HttpUtility.UrlDecode( result );
            //        break;
            //    }
            //}
            //return result;

            Dictionary<string, string> cookies = GetAllCookies();
            //如果为空或不存在就取IsolatedStorage中的值
            if (cookies.Count <= 0 || !cookies.ContainsKey(key))
            {
                cookies = GetAllCookiesFromIsolatedStorage();
            }

            if (cookies.Count > 0)
            {
                if (cookies.ContainsKey(key))
                {
                    return cookies[key];
                }
                return null;
            }
            return null;
        }

        /// <summary>
        ///   设置Cookie
        /// </summary>
        /// <param name = "key"></param>
        /// <param name = "value"></param>
        public static void SetCookie(String key, String value)
        {
            //SetCookie(key, value, null, null, null, false);
            Dictionary<string, string> cookies = GetAllCookiesFromIsolatedStorage();
            if (!cookies.ContainsKey(key))
            {
                cookies.Add(key, value);
            }
            else
            {
                cookies[key] = value;
            }
            string content = cookies.ToJson();
            IsolatedStorageHelper.Instance.CreateFile(CookiePath, content, true);
        }

        /// <summary>
        ///   获取所有Cookie
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetAllCookies()
        {
            return _cookiesCach;
        }

        ///<summary>
        /// GetAllCookiesFromIsolatedStorage
        ///</summary>
        ///<returns></returns>
        public static Dictionary<string, string> GetAllCookiesFromIsolatedStorage()
        {
            var cookies = new Dictionary<string, string>();
            string content = IsolatedStorageHelper.Instance.GetFileContent(CookiePath);
            if (!string.IsNullOrEmpty(content))
            {
                try
                {
                    cookies = content.ToObject<Dictionary<string, string>>();
                }
                catch
                {
                    IsolatedStorageHelper.Instance.DeleteFile(CookiePath);
                }
            }
            _cookiesCach = cookies;

            return cookies;
        }

        #region 注释

        //public static void SetCookie(String key, String value, TimeSpan expires)
        //{
        //    SetCookie(key, value, expires, null, null, false);
        //}

        //public static void SetCookie(String key, String value, TimeSpan? expires, String path, String domain, bool secure)
        //{

        //    #region URL转码
        //    key = HttpUtility.UrlEncode(key);
        //    value = HttpUtility.UrlEncode(value);
        //    path = HttpUtility.UrlEncode(path);
        //    domain = HttpUtility.UrlEncode(domain);
        //    #endregion

        //    StringBuilder cookie = new StringBuilder();
        //    cookie.Append(String.Concat(key, "=", value));
        //    if (expires.HasValue)
        //    {
        //        DateTime expire = DateTime.UtcNow + expires.Value;
        //        cookie.Append(String.Concat(";expires=", expire.ToString("R")));
        //    }
        //    if (!String.IsNullOrEmpty(path))
        //    {
        //        cookie.Append(String.Concat(";path=", path));
        //    }
        //    if (!String.IsNullOrEmpty(domain))
        //    {
        //        cookie.Append(String.Concat(";domain=", domain));
        //    }
        //    if (secure)
        //    {
        //        cookie.Append(";secure");
        //    }
        //    HtmlPage.Document.SetProperty("cookie", cookie.ToString());
        //} 

        #endregion

        #endregion Methods
    }
}