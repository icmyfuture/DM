using System;
using System.Collections.Generic;
using System.Linq;
using DM.Web.SL.Common.Core.App;
using DM.Web.SL.Common.Extensions;
using System.IO;
using System.Net;

namespace DM.Web.SL.Common.Utility
{
    /// <summary>
    /// 业务逻辑操作类
    /// </summary>
    public class AppHelpHelper
    {
        #region 单一实例
        /// <summary>
        /// 单一实例
        /// </summary>
        public static readonly AppHelpHelper Instance = new AppHelpHelper();
        #endregion

        #region  构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public AppHelpHelper()
        {
        }
        #endregion  构造函数

        #region 私有变量
        private List<AppHelpeInfo> m_appHelpeList = new List<AppHelpeInfo>();
        #endregion

        #region 属性

        #endregion

        #region 公共方法
        /// <summary>
        /// 获取帮助配置
        /// </summary>
        /// <param name="callBack">回调函数</param>
        public void GetAllAppHelpConfig(EventHandler callBack)
        {
            WebClient client = new WebClient();
            //打开文件
            var uri = new Uri("http://" + LanguageHelper.CurrentAppWebServerIPAddress + "/Resources/appHelpConfig.xml?" + DateTime.Now.GetHashCode());
            client.OpenReadCompleted += (obj, arg) =>
                                        {
                                            try
                                            {
                                                using (StreamReader sr = new StreamReader(arg.Result))
                                                {
                                                    #region 获取帮助配置

                                                    string languageListXmlStr = sr.ReadToEnd();
                                                    m_appHelpeList = languageListXmlStr.ToObjectFromXml<List<AppHelpeInfo>>();

                                                    #endregion
                                                }
                                                if (callBack != null)
                                                {
                                                    callBack(m_appHelpeList, EventArgs.Empty);
                                                }
                                            }
                                            catch
                                            {
                                                if (callBack != null)
                                                {
                                                    callBack(m_appHelpeList, EventArgs.Empty);
                                                }
                                            }
                                            client = null;
                                        };
            client.OpenReadAsync(uri);
        }


        ///<summary>
        /// 获取帮助地址
        ///</summary>
        ///<param name="applicationId">应用名称</param>
        ///<returns></returns>
        public string GetAppHelpUrl(string applicationId)
        {
            string appWebServerurl = "http://" + LanguageHelper.CurrentAppWebServerIPAddress;
            string lang = LanguageHelper.GetUserLanguageInfo().LanguageKey.ToLower();
            switch (lang)
            {
                case "zh_cn":
                case "zh_tw":
                    lang = "cn";
                    break;
                case "ja_jp":
                    lang = "jp";
                    break;
                default:
                    lang = "en";
                    break;
            }
            string url = string.Format("{0}/help/source/WebHelp_{1}/index.htm#{2}_operations/overview.htm", appWebServerurl, lang, applicationId);

            if (m_appHelpeList != null && m_appHelpeList.Count > 0 && m_appHelpeList.Any(x => x.ApplicationId == applicationId))
            {
                AppHelpeInfo appHelpe = m_appHelpeList.FirstOrDefault(x => x.ApplicationId == applicationId);
                if (appHelpe != null)
                {
                    //{$lang$} 语言
                    //{$appid$} 应用名
                    string appHelpUrl = appHelpe.AppHelpUrl.Replace("{$lang$}", lang).Replace("{$appid$}", applicationId);
                    url = string.Format("{0}{1}", appWebServerurl, appHelpUrl);
                }
            }

            return url;
        }

        #endregion

        #region 私有方法

        #endregion
    }
}
