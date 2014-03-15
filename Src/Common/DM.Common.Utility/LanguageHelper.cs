using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DM.Common.Utility
{
    /// <summary>
    ///   操作类
    /// </summary>
    public class LanguageHelper
    {

        /// <summary>
        /// 语言
        /// </summary>
        public static string LanguageKey = string.Empty;

        /// <summary>
        /// 格式为 Key=应用 Value为应用下当前语言的 所有翻译 格式为 Key=键值 Value = 翻译的值。
        /// </summary>
        private static readonly Dictionary<string, Dictionary<string, string>> Landics = new Dictionary<string, Dictionary<string, string>>();

        /// <summary>
        /// 读取翻译数据
        /// </summary>
        /// <param name="appName">应用程序名字</param>
        /// <param name="reReadFile">重新读取文件(否则判断缓存)</param>
        public static void ReadLanguageData(string appName, bool reReadFile)
        {
            if (reReadFile && Landics.ContainsKey(appName))
            {
                Landics.Remove(appName);
            }
            ReadLanguageData_Inner(appName, reReadFile);
        }

        /// <summary>
        /// 读取翻译数据
        /// </summary>
        /// <param name="appName">应用程序名字</param>
        public static void ReadLanguageData(string appName)
        {
            ReadLanguageData_Inner(appName, false);
        }

        /// <summary>
        /// 读取翻译数据
        /// </summary>
        /// <param name="appName">应用程序名字</param>
        /// <param name="reReadFile">重新读取文件</param>
        private static void ReadLanguageData_Inner(string appName, bool reReadFile)
        {
            //如果不存在，则查找并添加到集合中
            if (reReadFile || !Landics.ContainsKey(appName))
            {
              //  2011年10月11日18:09:41 xmj 更新路径的组成方式
                var searchFilePath = AppDomain.CurrentDomain.BaseDirectory + @"Resource\Language\" + LanguageKey + @"\" + appName + ".txt";
                if (File.Exists(searchFilePath))
                {
                    using (var sr = new StreamReader(searchFilePath, Encoding.UTF8))
                    {
                        #region 获取字典

                        var appDics = new Dictionary<string, string>();
                        while (true)
                        {
                            var line = sr.ReadLine();
                            if (line == null || string.IsNullOrEmpty(line))
                            {
                                break;
                            }
                            string sKey;
                            string sValue;
                            if (ParseLine(line, out sKey, out sValue))
                            {
                                //排除单个文件中重复的字典
                                if (!appDics.ContainsKey(sKey))
                                    appDics.Add(sKey, sValue);
                            }
                        }
                        Landics.Add(appName, appDics);

                        #endregion
                    }
                }
            }
        }

        /// <summary>
        /// 根据字典Key获取相应的翻译值
        /// </summary>
        /// <param name="sourceKey">字典Key</param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="appName">当前应用名称</param>
        /// <returns>翻译值</returns>
        public static string GetDictionary(string sourceKey, string defaultValue, string appName = "DM.Client.WPF")
        {
            var dictionary = defaultValue;
            if (Landics.ContainsKey(appName))
            {
                var appDics = Landics[appName];
                if (appDics.ContainsKey(sourceKey))
                    dictionary = appDics[sourceKey];
            }
            return dictionary;
        }

        /// <summary>
        /// 解析每行字典内容
        /// </summary>
        /// <param name="line">行内容</param>
        /// <param name="key">key</param>
        /// <param name="value">值</param>
        /// <returns>是否成功</returns>
        private static bool ParseLine(string line, out string key, out string value)
        {
            if (!string.IsNullOrEmpty(line))
            {
                var a = line.Split('=');
                if (a.Length <= 1)
                {
                    key = string.Empty;
                    value = string.Empty;
                    return false;
                }
                key = a[0].Trim();
                value = a[1].Trim();
                return true;
            }
            key = string.Empty;
            value = string.Empty;
            return false;
        }
    }
}