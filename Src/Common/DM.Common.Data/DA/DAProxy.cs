using System;

namespace DM.Common.Data.DA
{
    /// <summary>
    /// 数据库访问服务代理，提供数据库访问控制类的获取
    /// 数据库访问控制类，请察看 DAControl 的注释
    /// </summary>
    public class DAProxy
    {
        private static string _hbmCfgPath;
        private static string _connStr;
        private static string _mainDirectory;

        /// <summary>
        /// 初始化数据库连接
        /// 同 【GetDAControl()】配合使用
        /// </summary>
        /// <param name="hbmCfgPath">Nhibernate配置文件</param>
        /// <param name="connStr">连接字符串，如果不为空，用这个连接字符串替换Nhibernate配置文件中的连接字符串</param>
        /// <param name="mainDirectory">映射配置的程序集目录，如果为空，表示映射配置的程序集文件和hbmCfgPath在同一文件夹下</param>
        public static void Init(string hbmCfgPath, string connStr, string mainDirectory)
        {
            _hbmCfgPath = hbmCfgPath;
            _connStr = connStr;
            _mainDirectory = mainDirectory;
        }

        /// <summary>
        /// 获取数据库访问控制类
        /// 调用此方法前必须调用【Init】初始化
        /// </summary>
        /// <returns>数据库访问控制类</returns>
        public static DAControl GetDAControl()
        {
            if (string.IsNullOrEmpty(_hbmCfgPath))
                throw new Exception("call Init first.");
            return new DAControl(_hbmCfgPath, _mainDirectory, _connStr);
        }

        /// <summary>
        /// 获取数据库访问控制类
        /// 【注：映射配置的程序集文件和hbmCfgPath在同一文件夹下】
        /// </summary>
        /// <param name="hbmCfgPath"> Nhibernate配置文件</param>
        /// <returns>数据库访问控制类</returns>
        public static DAControl GetDAControl(string hbmCfgPath)
        {
            return new DAControl(hbmCfgPath, "");
        }
        /// <summary>
        /// 获取数据库访问控制类
        /// 【注：映射配置的程序集文件和hbmCfgPath在同一文件夹下】
        /// </summary>
        /// <param name="hbmCfgPath">Nhibernate配置文件</param>
        /// <param name="connStr">连接字符串，如果不为空，用这个连接字符串替换Nhibernate配置文件中的连接字符串</param>
        /// <returns>数据库访问控制类</returns>
        public static DAControl GetDAControl(string hbmCfgPath, string connStr)
        {
            return new DAControl(hbmCfgPath, connStr);
        }

        /// <summary>
        /// 获取数据库访问控制类
        /// </summary>
        /// <param name="hbmCfgPath">Nhibernate配置文件</param>
        /// <param name="mainDirectory">映射文件程序集目录</param>
        /// <param name="connStr">连接字符串，如果不为空，用这个连接字符串替换Nhibernate配置文件中的连接字符串</param>
        /// <returns>数据库访问控制类</returns>
        public static DAControl GetDAControl(string hbmCfgPath, string mainDirectory, string connStr)
        {
            return new DAControl(hbmCfgPath, mainDirectory, connStr);
        }
    }
}
