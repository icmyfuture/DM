using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;

namespace DM.Common.Utility.Log
{
    /// <summary>
    /// 应用程序日志服务
    /// </summary>
    public class AppLog
    {
        #region 属性变量
        private static readonly Thread ThreadForAppLogSaveService;
        private static readonly string DefaultLogFile = string.Empty;
        private static readonly AppLogSaveService Alss;

        #endregion

        #region 构造函数
        static AppLog()
        {
            DefaultLogFile = "AppRunLog.txt";

            //启动日志定时保存服务
            Alss = new AppLogSaveService();
            var threadStartForAppLogSaveService = new ThreadStart(Alss.StartService);
            ThreadForAppLogSaveService = new Thread(threadStartForAppLogSaveService) {IsBackground = true};
            ThreadForAppLogSaveService.Start();
        }
        #endregion

        #region 拼接参数
        private static string GetFullString(IEnumerable<string> msg)
        {
            return msg.Aggregate(string.Empty, (current, t) => current + t);
        }

        #endregion

        #region 构建带时间毫秒数的日志
        /// <summary>
        /// 构建带时间毫秒数的日志
        /// </summary>
        /// <param name="msg">日志</param>
        public static string BuildLogWithTime(params string[] msg)
        {
            return string.Format("[{0}:{1}]->#{2}:{3}\r\n",
                DateTime.Now.ToString(CultureInfo.InvariantCulture),
                DateTime.Now.Millisecond,
                Thread.CurrentThread.ManagedThreadId,
                GetFullString(msg)
                );
        }
        #endregion

        #region 构建完整路径
        /// <summary>
        /// 构建完整路径
        /// </summary>
        /// <param name="fileName">文件名(不含路径信息)</param>
        /// <returns></returns>
        private static string BuildFilePath(string fileName)
        {
            return Path.Combine(AppLogSaveService.AppPhysicalPath, @"Log\" + (fileName.ToLower().EndsWith(".txt") ? fileName : fileName + ".txt"));
        }
        #endregion

        #region 记录日志、日志文件名由类型决定
        /// <summary>
        /// 记录日志
        /// </summary>
        /// <typeparam name="T">类型(从此产生日志文件名)</typeparam>
        /// <param name="msg">日志信息</param>
        /// <returns></returns>
        public static string WriteLog<T>(params string[] msg) where T : class
        {
            return LogMsgHelper.Instance.AddLogMsg(BuildFilePath(typeof(T).ToString()), BuildLogWithTime(GetFullString(msg)));
        }

        public static string WriteLog<T>(string msg) where T : class
        {
            return LogMsgHelper.Instance.AddLogMsg(BuildFilePath(typeof(T).ToString()), BuildLogWithTime(msg));
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <typeparam name="T">类型(从此产生日志文件名)</typeparam>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常信息</param>
        /// <returns></returns>
        public static string WriteLog<T>(string msg, Exception ex) where T : class
        {
            return LogMsgHelper.Instance.AddLogMsg(BuildFilePath(typeof(T).ToString()), BuildLogWithTime(msg + "\r\n" + ex));
        }

        /// <summary>
        /// 记录日志(带文件大小限制)
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="msg">消息</param>
        /// <param name="su">日志大小</param>
        /// <returns></returns>
        public string WriteLog<T>(string msg, SizeWithUnitInfo su) where T : class
        {
            return LogMsgHelper.Instance.AddLogMsg(BuildFilePath(typeof(T).ToString()), BuildLogWithTime(msg), su);
        }

        /// <summary>
        /// 记录日志(带文件大小限制)
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="msg">消息</param>
        /// <param name="ex">异常信息</param>
        /// <param name="su">带单位的大小信息</param>
        /// <returns></returns>
        public static string WriteLog<T>(string msg, Exception ex, SizeWithUnitInfo su) where T : class
        {
            return LogMsgHelper.Instance.AddLogMsg(BuildFilePath(typeof(T).ToString()), BuildLogWithTime(msg + "\r\n" + ex), su);
        }


        /// <summary>
        /// 记录日志
        /// </summary>
        /// <typeparam name="T">类型(从此产生日志文件名)</typeparam>
        /// <param name="ex">异常</param>
        /// <returns></returns>
        public static string WriteLog<T>(Exception ex) where T : class
        {
            return LogMsgHelper.Instance.AddLogMsg(BuildFilePath(typeof(T).ToString()), BuildLogWithTime(ex.ToString()));
        }

        /// <summary>
        /// 记录日志(带文件大小限制)
        /// </summary>
        /// <param name="ex">异常</param>
        /// <param name="su">文件大小限制</param>
        /// <returns></returns>
        public static string WriteLog<T>(Exception ex, SizeWithUnitInfo su) where T : class
        {
            return LogMsgHelper.Instance.AddLogMsg(BuildFilePath(typeof(T).ToString()), BuildLogWithTime(ex.ToString()), su);
        }

        /// <summary>
        /// 记录日志(带文件大小限制)
        /// </summary>
        /// <param name="su">文件大小限制</param>
        /// <returns></returns>
        public static string WriteLog<T>(SizeWithUnitInfo su) where T : class
        {
            return LogMsgHelper.Instance.AddLogMsg(BuildFilePath(typeof(T).ToString()), string.Empty, su);
        }
        #endregion

        #region 记录日志、日志文件名由传入参数决定
        /// <summary>
        /// 记录日志(将记录在指定文件中)
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="msg">日志内容</param>
        /// <returns></returns>
        public static string WriteLog(string fileName, string msg)
        {
            return LogMsgHelper.Instance.AddLogMsg(BuildFilePath(fileName), BuildLogWithTime(msg));
        }

        /// <summary>
        /// 记录日志(将记录在指定文件中)
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="msg">日志内容</param>
        /// <param name="ex">异常对象</param>
        /// <returns></returns>
        public static string WriteLog(string fileName, string msg, Exception ex)
        {
            return LogMsgHelper.Instance.AddLogMsg(BuildFilePath(fileName), BuildLogWithTime(msg + "\r\n" + ex));
        }

        /// <summary>
        /// 记录日志(带文件大小限制)
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="msg">日志</param>
        /// <param name="su">文件大小限制</param>
        /// <returns></returns>
        public static string WriteLog(string fileName, string msg, SizeWithUnitInfo su)
        {
            return LogMsgHelper.Instance.AddLogMsg(BuildFilePath(fileName), BuildLogWithTime(msg), su);
        }

        /// <summary>
        /// 记录日志(带文件大小限制)
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="msg">日志</param>
        /// <param name="ex">异常对象</param>
        /// <param name="su">文件大小限制</param>
        /// <returns></returns>
        public static string WriteLog(string fileName, string msg, Exception ex, SizeWithUnitInfo su)
        {
            return LogMsgHelper.Instance.AddLogMsg(BuildFilePath(fileName), BuildLogWithTime(string.Format("{0}\r\n{1}", msg, ex)), su);
        }

        /// <summary>
        /// 记录日志(将记录在指定文件中)
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="ex">异常</param>
        /// <returns></returns>
        public static string WriteLog(string fileName, Exception ex)
        {
            return LogMsgHelper.Instance.AddLogMsg(BuildFilePath(fileName), BuildLogWithTime(ex.ToString()));
        }

        /// <summary>
        /// 记录日志(带文件大小限制)
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="ex">异常</param>
        /// <param name="su">文件大小限制</param>
        /// <returns></returns>
        public static string WriteLog(string fileName, Exception ex, SizeWithUnitInfo su)
        {
            return LogMsgHelper.Instance.AddLogMsg(BuildFilePath(fileName), BuildLogWithTime(ex.ToString()), su);
        }

        /// <summary>
        /// 记录日志(带文件大小限制)
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="su">文件大小限制</param>
        /// <returns></returns>
        public static string WriteLog(string fileName, SizeWithUnitInfo su)
        {
            return LogMsgHelper.Instance.AddLogMsg(BuildFilePath(fileName), string.Empty, su);
        }

        #endregion

        #region 记录日志、日志文件名为默认
        /// <summary>
        /// 记录日志(将记录在默认文件AppRunLog.txt中)
        /// </summary>
        /// <param name="msg">日志内容</param>
        /// <param name="ex">异常</param>
        /// <returns></returns>
        public static string WriteDefaultLog(string msg, Exception ex)
        {
            return LogMsgHelper.Instance.AddLogMsg(BuildFilePath(DefaultLogFile), BuildLogWithTime(msg + "\r\n" + ex));
        }

        /// <summary>
        /// 记录日志(将记录在默认文件AppRunLog.txt中)
        /// </summary>
        /// <param name="msg">日志内容</param>
        /// <returns></returns>
        public static string WriteDefaultLog(params string[] msg)
        {
            return LogMsgHelper.Instance.AddLogMsg(BuildFilePath(DefaultLogFile), BuildLogWithTime(GetFullString(msg)));
        }

        public static string WriteDefaultLog(string msg)
        {
            return LogMsgHelper.Instance.AddLogMsg(BuildFilePath(DefaultLogFile), BuildLogWithTime(msg));
        }

        /// <summary>
        /// 记录日志(将记录在默认文件AppRunLog.txt中)
        /// </summary>
        /// <param name="ex">异常对象</param>
        /// <returns></returns>
        public static string WriteDefaultLog(Exception ex)
        {
            return LogMsgHelper.Instance.AddLogMsg(BuildFilePath(DefaultLogFile), BuildLogWithTime(ex.ToString()));
        }
        #endregion
    }
}