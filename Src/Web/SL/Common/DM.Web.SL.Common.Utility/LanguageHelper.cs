using DM.Web.SL.Common.Core;
using DM.Web.SL.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Windows;

namespace DM.Web.SL.Common.Utility
{
    /// <summary>
    ///   操作类
    /// </summary>
    public class LanguageHelper
    {
        #region Fields

        /// <summary>
        ///   单一实例
        /// </summary>
        public static readonly LanguageHelper Instance = new LanguageHelper();

        #region 私有属性

        /// <summary>
        ///   当前应用Web端地址
        /// </summary>
        private static string currentAppWebServerIPAddress = string.Empty;

        private static readonly Dictionary<string, Dictionary<string, string>> Landics = new Dictionary<string, Dictionary<string, string>>(); //格式为 Key=应用 Value为应用下当前语言的 所有翻译 格式为 Key=键值 Value = 翻译的值。

        //private static LanguageInfo _currentLanguage = new LanguageInfo
        //{
        //    ShowName = "English",
        //    LanguageKey = "EN_US",
        //    FontFamily = "Arial,SimSun,MS UI Gothic"
        //};

        /// <summary>
        ///   系统当前支持的语言
        /// </summary>
        private static List<LanguageInfo> m_languageList = new List<LanguageInfo>();

        #endregion

        #region 公共属性


        #region 默认语言修改
        /// <summary>
        /// 默认语言修改 MRC默认为中文
        /// </summary>
        public static LanguageInfo DefaultLanguageInfo = new LanguageInfo
        {
            LanguageKey = "ZH_CN",
            ShowName = "简体中文",
            FontFamily = "Arial,SimSun,MS UI Gothic"
        }; 
        #endregion

        #region  当前应用Web端地址

        /// <summary>
        ///   当前应用Web端地址(格式如：172.16.2.28:83)
        /// </summary>
        public static string CurrentAppWebServerIPAddress
        {
            get { return currentAppWebServerIPAddress; }
            set { currentAppWebServerIPAddress = value; }
        }

        #endregion

        #endregion

        #endregion Fields

        #region Methods

        #region 公共方法

        /// <summary>
        ///   根据字典Key获取相应的翻译值
        /// </summary>
        /// <param name = "appName">当前应用名称</param>
        /// <param name = "sourceKey">字典Key</param>
        /// <param name = "defaultValue">默认值</param>
        /// <returns>翻译值</returns>
        public static string GetDictionary(string appName, string sourceKey, string defaultValue)
        {
            //忽略应用名称大小写
            appName = appName.ToUpper();
            sourceKey = sourceKey.ToUpper();
            string dictionary = defaultValue;
            if (Landics.ContainsKey(appName))
            {
                Dictionary<string, string> appDics = Landics[appName];

                if (appDics.ContainsKey(sourceKey))
                {
                    dictionary = appDics[sourceKey];
                }
            }
            return dictionary;
        }

        /// <summary>
        ///   加载字典文件
        /// </summary>
        /// <param name = "appName">应用名称</param>
        /// <param name = "globalizationCompleted">回调函数</param>
        public static void Globalization(string appName, EventHandler globalizationCompleted)
        {
            //忽略应用名称大小写
            appName = appName.ToUpper();
            LanguageInfo lang = GetUserLanguageInfo();
            if (DefaultLanguageInfo.LanguageKey != lang.LanguageKey || !Landics.ContainsKey(appName))
            {
                if (DefaultLanguageInfo.LanguageKey != lang.LanguageKey)
                {
                    Landics.Clear();
                    DefaultLanguageInfo = lang;
                }

                string serverUrl = "http://" + CurrentAppWebServerIPAddress;
                var loadLanguageFile = new LoadLanguageFile(serverUrl);
                loadLanguageFile.LoadLanguages(appName, lang, globalizationCompleted, AddLanDics);
            }
            else
            {
                if (globalizationCompleted != null)
                {
                    globalizationCompleted(new object(), EventArgs.Empty);
                }
            }
        }

        /// <summary>
        ///   设置当前语言
        /// </summary>
        /// <param name = "lang">语言信息</param>
        public static void SetLanguage(LanguageInfo lang)
        {
            CurrentUserHelper.SetCurrentUserLangInfo(lang);
        }

        ///<summary>
        ///  GetUserLanguageInfo
        ///</summary>
        ///<returns></returns>
        public static LanguageInfo GetUserLanguageInfo()
        {
            UserInfo user = CurrentUserHelper.GetCurrentUserInfo();
            if (user != null && user.Language != null)
            {
                return user.Language;
            }
            return DefaultLanguageInfo;
        }

        #region CurrentCulture

        /// <summary>
        ///   获取当前语言的CultureInfo
        /// </summary>
        public static CultureInfo GetCurrentCultureInfo()
        {
            UserInfo user = CurrentUserHelper.GetCurrentUserInfo();
            if (user == null)
            {
                user = new UserInfo();
            }
            LanguageInfo lan = user.Language;
            if (lan == null)
            {
                lan = DefaultLanguageInfo;
            }

            return GetCultureInfo(lan);
        }

        /// <summary>
        ///   根据Language获取CultureInfo
        /// </summary>
        /// <param name = "lan">Language</param>
        /// <returns></returns>
        public static CultureInfo GetCultureInfo(LanguageInfo lan)
        {
            string lanCulture = lan.LanguageKey.Replace("_", "-");
            var culture = new CultureInfo(lanCulture);

            return culture;
        }

        #endregion

        #region GetLanguageInfo

        /// <summary>
        ///   获取系统当前支持的所有语言
        /// </summary>
        /// <param name = "callBackEvent">响应回调函数</param>
        public static void GetAllLanguageInfo(EventHandler callBackEvent)
        {
            GetAllLanguageInfo(false, callBackEvent);
        }

        /// <summary>
        ///   获取所有系统注册信息
        /// </summary>
        /// <param name = "isForceUpdate">是否强制更新为服务端数据</param>
        /// <param name = "callBackEvent">响应回调函数</param>
        public static void GetAllLanguageInfo(bool isForceUpdate, EventHandler callBackEvent)
        {
            if (m_languageList.Count <= 0 || isForceUpdate)
            {
                WebClient client = new WebClient();
                //打开文件
                var uri = new Uri("http://" + CurrentAppWebServerIPAddress + "/Resources/LANDIC/langConfig.xml");
                client.OpenReadCompleted += (obj, arg) =>
                                            {
                                                try
                                                {
                                                    using (StreamReader sr = new StreamReader(arg.Result))
                                                    {
                                                        #region 获取字典

                                                        string languageListXmlStr = sr.ReadToEnd();
                                                        m_languageList = languageListXmlStr.ToObjectFromXml<List<LanguageInfo>>();

                                                        #endregion
                                                    }
                                                    if (callBackEvent != null)
                                                    {
                                                        callBackEvent(m_languageList, EventArgs.Empty);
                                                    }
                                                }
                                                catch
                                                {
                                                    if (callBackEvent != null)
                                                    {
                                                        callBackEvent(m_languageList, EventArgs.Empty);
                                                    }
                                                }
                                                client = null;
                                            };
                client.OpenReadAsync(uri);

                #region MyRegion

                //RequestModel requestModel = new RequestModel()
                //{
                //    CommandName = "LanguageSupportControl/getAll",
                //    Parameters = "",
                //    UserID = "-1"
                //};
                //RequestHelper.Instance.GetServiceRequest( requestModel, ( obj, arg ) =>
                //{
                //    RequestResultModel resultModel = obj as RequestResultModel;
                //    if ( resultModel.State == ResultState.Success )
                //    {
                //        languageList = SerializeProxy.JsonSerialize.Deserialize<List<LanguageInfo>>( resultModel.ResultData );
                //        if ( callBackEvent != null )
                //        {
                //            callBackEvent( languageList, EventArgs.Empty );
                //        }
                //    }
                //    else
                //    {
                //    }
                //} );

                #endregion
            }
            else
            {
                if (callBackEvent != null)
                {
                    callBackEvent(m_languageList, null);
                }
            }
        }

        #endregion

        #endregion

        #region 私有方法

        /// <summary>
        ///   添加字典
        /// </summary>
        /// <param name = "obj"></param>
        /// <param name = "arg"></param>
        private static void AddLanDics(object obj, EventArgs arg)
        {
            List<object> landicList = obj as List<object>;
            if (landicList != null && landicList.Count == 2)
            {
                //防止重复
                if (!Landics.ContainsKey(landicList[0].ToString()))
                {
                    Landics.Add(landicList[0].ToString(), landicList[1] as Dictionary<string, string>);
                }
            }
        }

        #endregion

        #endregion Methods
    }

    /// <summary>
    ///   加载风格文件
    /// </summary>
    public class LoadLanguageFile : IDisposable
    {
        #region Fields

        private string _ServerUrl = string.Empty;
        private EventHandler _addLanDicsEventHandler;
        private string _appName = string.Empty;
        private WebClient _client;
        private LanguageInfo _lang = new LanguageInfo();
        private EventHandler _loadCompleted;

        #endregion Fields

        #region Constructors

        /// <summary>
        ///   构造函数
        /// </summary>
        public LoadLanguageFile()
        {}

        /// <summary>
        ///   构造函数
        /// </summary>
        /// <param name = "serverUrl">服务地址</param>
        public LoadLanguageFile(string serverUrl)
        {
            _ServerUrl = serverUrl;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        ///   加载风格资源
        /// </summary>
        /// <param name = "appName">应用名称</param>
        /// <param name = "lang">语言信息</param>
        /// <param name = "loadCompleted">完成事件</param>
        /// <param name = "addLanDicsEventHandler">添加语言字典实践</param>
        public void LoadLanguages(string appName, LanguageInfo lang, EventHandler loadCompleted, EventHandler addLanDicsEventHandler)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
                                                      {
                                                          try
                                                          {
                                                              _lang = lang;
                                                              _appName = appName;

                                                              _client = new WebClient();
                                                              _loadCompleted = loadCompleted;
                                                              _addLanDicsEventHandler = addLanDicsEventHandler;
                                                              //打开文件
                                                              Uri uri = new Uri(_ServerUrl + "/Resources/LANDIC/" + _lang.LanguageKey + "/" + appName + ".txt");
                                                              _client.OpenReadCompleted += OpenReadCompleted;
                                                              _client.OpenReadAsync(uri);
                                                          }
                                                          catch (Exception)
                                                          {
                                                              Dispose();
                                                          }
                                                      });
        }

        /// <summary>
        ///   解析每行字典内容
        /// </summary>
        /// <param name = "line">行内容</param>
        /// <param name = "key">key</param>
        /// <param name = "value">值</param>
        /// <returns>是否成功</returns>
        private static bool ParseLine(string line, out string key, out string value)
        {
            if (!string.IsNullOrEmpty(line))
            {
                string[] a = line.Split('=');

                if (a.Length <= 1)
                {
                    key = string.Empty;
                    value = string.Empty;
                    return false;
                }
                key = a[0];
                value = a[1];
                return true;
            }
            key = string.Empty;
            value = string.Empty;
            return false;
        }

        private void OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
                                                      {
                                                          try
                                                          {
                                                              using (StreamReader sr = new StreamReader(e.Result))
                                                              {
                                                                  #region 获取字典

                                                                  Dictionary<string, string> appDics = new Dictionary<string, string>();
                                                                  while (true)
                                                                  {
                                                                      string line = sr.ReadLine();

                                                                      if (line == null || string.IsNullOrEmpty(line))
                                                                      {
                                                                          break;
                                                                      }
                                                                      string sKey;
                                                                      string sValue;

                                                                      if (ParseLine(line, out sKey, out sValue))
                                                                      {
                                                                          sKey = sKey.ToUpper();
                                                                          //排除单个文件中重复的字典
                                                                          if (!appDics.ContainsKey(sKey))
                                                                          {
                                                                              appDics.Add(sKey, sValue);
                                                                          }
                                                                      }
                                                                  }
                                                                  if (_addLanDicsEventHandler != null && _lang != null && _lang.LanguageKey == GetUserLanguageInfo().LanguageKey)
                                                                  {
                                                                      List<object> landicList = new List<object>();
                                                                      landicList.Add(_appName);
                                                                      landicList.Add(appDics);
                                                                      _addLanDicsEventHandler(landicList, null);
                                                                      landicList.Clear();
                                                                  }

                                                                  #endregion

                                                                  //appDics.Clear();
                                                                  sr.Close();
                                                              }
                                                              if (_loadCompleted != null)
                                                              {
                                                                  _loadCompleted(new object(), EventArgs.Empty);
                                                              }
                                                          }
                                                          catch
                                                          {
                                                              if (_loadCompleted != null)
                                                              {
                                                                  _loadCompleted(new object(), EventArgs.Empty);
                                                              }
                                                              Dispose();
                                                          }

                                                          Dispose();
                                                      });
        }

        private LanguageInfo GetUserLanguageInfo()
        {
            UserInfo user = CurrentUserHelper.GetCurrentUserInfo();
            if (user != null && user.Language != null)
            {
                return user.Language;
            }
            return LanguageHelper.GetUserLanguageInfo();
        }

        #endregion Methods

        #region IDisposable Members

        public void Dispose()
        {
            _loadCompleted = null;
            _addLanDicsEventHandler = null;
            if ( _client != null )
            {
                _client.OpenReadCompleted -= OpenReadCompleted;
                _client = null;
            }
        }

        #endregion
    }
}