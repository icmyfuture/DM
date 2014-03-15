using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace DM.Common.Config
{
    ///<summary>
    ///  配置文件管理类
    ///</summary>
    public static class Configuration
    {
        private const string ConfigPath = "Config";
        private static string _assemblyName;
        private static readonly string GloabalConfig = @"C:\DM\Configuration.xml";
        private static string _configFile;
        private static string _myConfigFile;

        static Configuration()
        {
            GloabalConfig = CommonSettings("GloabalConfigPath");
            if (string.IsNullOrEmpty(GloabalConfig))
            {
                GloabalConfig = Environment.CurrentDirectory + "\\Configuration.xml";
            }
            GetGlobalIp();
        }

        /// <summary>
        /// 全局IP配置
        /// </summary>
        public static readonly Dictionary<string, string> GloabalIpConfigs = new Dictionary<string, string>();

        /// <summary>
        /// 获取全局配置中的模块IP
        /// 如： GetGlobalIP("{Ms-ConfigServer}")
        /// </summary>
        /// <param name="servername">模块名称</param>
        /// <returns></returns>
        public static string GetGlobalIp(string servername)
        {
            if (GloabalIpConfigs.Count <= 0 || !GloabalIpConfigs.ContainsKey(servername))
            {
                GetGlobalIp();
            }

            if (GloabalIpConfigs.ContainsKey(servername))
            {
                return GloabalIpConfigs[servername];
            }
            return "127.0.0.1";
        }

        /// <summary>
        /// 公共配置值，按KEY取值(大小写敏感) 
        /// 获取执行目录的上级目录 Config/common.config 里面的配置项
        /// </summary>
        /// <param name = "key">配置项的KEY</param>
        /// <returns></returns>
        public static string CommonSettings(string key)
        {
            var config = CommonSettings();
            var setting = config.Settings[key];
            return setting != null ? ReplaceSetting(setting.Value) : string.Empty;
        }

        ///<summary>
        /// 获取设置项
        /// 如果调用 SetFileName("my.xml") 方法设置了配置文件名称，那么获取 执行目录的上级目录 Config/my.xml 里面的配置项
        /// 如果调用 SetAssemblyName("dm.exe") 方法设置了程序集名称，那么获取 执行目录的上级目录 Config/dm.exe.config 里面的配置项
        /// 如果 SetFile和SetAssemblyName 都被调用，则按照SetFileName逻辑
        /// 如果 SetFile和SetAssemblyName 都未调用，则获取 执行目录的上级目录 Config/myapp.exe.config 里面的配置项（执行文件为myapp.exe）
        ///</summary>
        ///<param name = "key">配置项的KEY</param>
        ///<returns></returns>
        public static string AppSettings(string key)
        {
            var config = AppSettings();
            var setting = config.Settings[key];
            return setting != null
                       ? ReplaceSetting(setting.Value)
                       : string.Empty;
        }

        ///<summary>
        /// 指定当前程序集名称
        ///</summary>
        ///<param name="assemblyName"></param>
        public static void SetAssemblyName(string assemblyName)
        {
            _assemblyName = assemblyName;
        }

        ///<summary>
        ///  指定配置文件
        ///</summary>
        ///<param name = "fileName">配置文件名，全路径</param>
        public static void SetFile(string fileName)
        {
            _myConfigFile = fileName;
        }

        /// <summary>
        ///   读取CONFIG下的文件
        /// </summary>
        /// <param name = "filename">文件名</param>
        /// <returns></returns>
        public static string ReadFileInConfig(string filename)
        {
            var file = BuildFile(filename);
            return File.Exists(file)
                       ? File.ReadAllText(file)
                       : string.Empty;
        }

        /// <summary>
        /// 用实际IP地址替换服务占位符(大小写敏感)
        /// </summary>
        private static string ReplaceSetting(string setting)
        {
            return GloabalIpConfigs.Keys.Aggregate(setting, (current, key) => current.Replace(key, GloabalIpConfigs[key]));
        }

        /// <summary>
        /// 获取全局IP设置
        /// </summary>
        private static void GetGlobalIp()
        {
            if (File.Exists(GloabalConfig))
            {
                var globalxml = XDocument.Parse(File.ReadAllText(GloabalConfig));
                foreach (var element in globalxml.Descendants())
                {
                    string key = string.Format("{{{0}}}", element.Name.LocalName);
                    if (!GloabalIpConfigs.ContainsKey(key))
                    {
                        GloabalIpConfigs.Add(key, element.Value);
                    }
                }
            }
        }

        ///<summary>
        ///  获取设置项
        ///</summary>
        ///<returns></returns>
        private static AppSettingsSection AppSettings()
        {
            _configFile = BuildFile(_myConfigFile);
            AppSettingsSection settings;
            if (File.Exists(_configFile))
            {
                var configmap = new ExeConfigurationFileMap
                {
                    ExeConfigFilename = _configFile
                };
                var config = ConfigurationManager.OpenMappedExeConfiguration(configmap, ConfigurationUserLevel.None);
                settings = config.AppSettings;
            }
            else
            {
                settings = new AppSettingsSection();
            }
            return settings;
        }

        /// <summary>
        /// BuildFile
        /// </summary>
        /// <param name="myConfigFile"></param>
        /// <returns></returns>
        private static string BuildFile(string myConfigFile)
        {
            string assemblyName = string.Empty;

            if (_assemblyName == null)
            {
                // 单元测试时“Assembly.GetEntryAssembly()”为空。
                if (Assembly.GetEntryAssembly() != null)
                    assemblyName = Assembly.GetEntryAssembly().ManifestModule.Name;
            }
            else
                assemblyName = _assemblyName;
            var parentpath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..."));
            var exepath = Path.Combine(parentpath, ConfigPath);
            return string.IsNullOrEmpty(myConfigFile)
                       ? Path.Combine(exepath, assemblyName + ".config")
                       : Path.Combine(exepath, myConfigFile);
        }

        ///<summary>
        ///  公共配置值
        ///</summary>
        ///<returns></returns>
        private static AppSettingsSection CommonSettings()
        {
            var configfile = BuildFile("common.config");
            var configmap = new ExeConfigurationFileMap
            {
                ExeConfigFilename = configfile
            };
            var config = ConfigurationManager.OpenMappedExeConfiguration(configmap, ConfigurationUserLevel.None);
            return config.AppSettings;
        }
    }
}