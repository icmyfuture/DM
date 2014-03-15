using System;

namespace DM.Common.Utility.Log
{
    public class LogHelper
    {
        private static string BuildMsgContent(LogType type, string moduleName, string msg, Exception ee)
        {
            string t;

            switch (type)
            {
                case Log.LogType.Fatal:
                    t = "Fatal";
                    break;
                case Log.LogType.Error:
                    t = "Error";
                    break;
                case Log.LogType.Info:
                    t = "Info";
                    break;
                case Log.LogType.Debug:
                    t = "Debug";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("type");
            }

            string msgContent = IsUseModuleName ? string.Format("[{0}]->{1}\r\n", t, msg) : string.Format("[{0} {1}]->{2}\r\n", moduleName, t, msg);

            if (ee != null)
            {
                msgContent += "\r\n[Exception]\r\n";
                msgContent += GetMsgContent(ee);
                if (ee.InnerException != null)
                {
                    msgContent += "\r\n[InnerException]\r\n";
                    msgContent += GetMsgContent(ee.InnerException);
                    if (ee.InnerException.InnerException != null)
                    {
                        msgContent += "\r\n[InnerException]\r\n";
                        msgContent += GetMsgContent(ee.InnerException.InnerException);
                        if (ee.InnerException.InnerException.InnerException != null)
                        {
                            msgContent += "\r\n[InnerException]\r\n";
                            msgContent += GetMsgContent(ee.InnerException.InnerException.InnerException);
                            if (ee.InnerException.InnerException.InnerException.InnerException != null)
                            {
                                msgContent += "\r\n[InnerException]\r\n";
                                msgContent += GetMsgContent(ee.InnerException.InnerException.InnerException.InnerException);
                            }
                        }
                    }
                }
            }

            return msgContent;
        }

        private static string GetMsgContent(Exception ee)
        {
            return ee.Message + "\r\n" + ee.StackTrace + "\r\n\r\n";
        }

        /// <summary>
        /// 0 Fatal, 1 Error, 2 Info, 3 Debug, 小于或等于该属性的日志会写，默认为Debug。
        /// </summary>
        public static LogType? LogType = Log.LogType.Debug;

        #region【属性】[公共] >>>> ServerName >>>> 服务名称 >>>>

        /// <summary>
        ///   服务名称
        /// </summary>
        public static string ServerName { get; set; }

        #endregion

        #region【属性】[公共] >>>> IsUseModuleName >>>> 是否使用模块名作为文件名 >>>>

        /// <summary>
        ///   是否使用模块名作为文件名
        /// </summary>
        public static bool IsUseModuleName { get; set; }

        #endregion

        #region【方法】[静态] [公共] >>>> Writelog >>>> 写日志

        /// <summary>
        ///   写日志
        /// </summary>
        /// <param name = "logType">日志级别</param>
        /// <param name = "moduleName">模块名，作为文件名</param>
        /// <param name = "logMsg">日志字符串</param>
        public static void Writelog(LogType logType, string moduleName, string logMsg)
        {
            Writelog(logType, moduleName, logMsg, null);
        }

        #endregion

        #region【方法】[静态] [公共] >>>> Writelog >>>> 写日志

        /// <summary>
        ///   写日志
        /// </summary>
        /// <param name = "logType">日志级别</param>
        /// <param name = "moduleName">模块名，作为文件名</param>
        /// <param name = "logMsg">日志字符串</param>
        /// <param name = "ee">异常信息</param>
        public static void Writelog(LogType logType, string moduleName, string logMsg, Exception ee)
        {
            var msgContent = BuildMsgContent(logType, moduleName, logMsg, ee);

            var fileName = IsUseModuleName
                               ? moduleName
                               : ServerName;
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = "Log";
            }

            AppLog.WriteLog(fileName, msgContent);
        }

        #endregion

        #region【方法】[静态] [公共] >>>> Writelog >>>> 写日志(扩展)
        /// <summary>
        ///   写日志，Info级别。
        /// </summary>
        /// <param name = "moduleName">模块名，作为文件名</param>
        /// <param name = "logMsg">日志字符串</param>
        public static void Info(string moduleName, string logMsg)
        {
            Writelog(Log.LogType.Info, moduleName, logMsg);
        }

        /// <summary>
        ///   写日志，Info级别。
        /// </summary>
        /// <param name = "moduleName">模块名，作为文件名</param>
        /// <param name = "logMsg">日志字符串</param>
        /// <param name = "ee">异常信息</param>
        public static void Info(string moduleName, string logMsg, Exception ee)
        {
            Writelog(Log.LogType.Info, moduleName, logMsg, ee);
        }

        /// <summary>
        ///   写日志，Debug级别。
        /// </summary>
        /// <param name = "moduleName">模块名，作为文件名</param>
        /// <param name = "logMsg">日志字符串</param>
        public static void Debug(string moduleName, string logMsg)
        {
            Writelog(Log.LogType.Debug, moduleName, logMsg);
        }

        /// <summary>
        ///   写日志，Debug级别。
        /// </summary>
        /// <param name = "moduleName">模块名，作为文件名</param>
        /// <param name = "logMsg">日志字符串</param>
        /// <param name = "ee">异常信息</param>
        public static void Debug(string moduleName, string logMsg, Exception ee)
        {
            Writelog(Log.LogType.Debug, moduleName, logMsg, ee);
        }

        /// <summary>
        ///   写日志，Error级别。
        /// </summary>
        /// <param name = "moduleName">模块名，作为文件名</param>
        /// <param name = "logMsg">日志字符串</param>
        public static void Error(string moduleName, string logMsg)
        {
            Writelog(Log.LogType.Error, moduleName, logMsg);
        }

        /// <summary>
        ///   写日志，Error级别。
        /// </summary>
        /// <param name = "moduleName">模块名，作为文件名</param>
        /// <param name = "logMsg">日志字符串</param>
        /// <param name = "ee">异常信息</param>
        public static void Error(string moduleName, string logMsg, Exception ee)
        {
            Writelog(Log.LogType.Error, moduleName, logMsg, ee);
        }

        /// <summary>
        ///   写日志，Fatal级别。
        /// </summary>
        /// <param name = "moduleName">模块名，作为文件名</param>
        /// <param name = "logMsg">日志字符串</param>
        public static void Fatal(string moduleName, string logMsg)
        {
            Writelog(Log.LogType.Fatal, moduleName, logMsg);
        }

        /// <summary>
        ///   写日志，Fatal级别。
        /// </summary>
        /// <param name = "moduleName">模块名，作为文件名</param>
        /// <param name = "logMsg">日志字符串</param>
        /// <param name = "ee">异常信息</param>
        public static void Fatal(string moduleName, string logMsg, Exception ee)
        {
            Writelog(Log.LogType.Fatal, moduleName, logMsg, ee);
        }

        /// <summary>
        ///   写日志，Fatal级别。
        /// </summary>
        /// <param name = "moduleName">模块名，作为文件名</param>
        /// <param name = "ee">异常信息</param>
        public static void Fatal(string moduleName, Exception ee)
        {
            Writelog(Log.LogType.Fatal, moduleName, string.Empty, ee);
        }

        #endregion

        #region【方法】[静态] [公共] >>>> Writelog >>>> 写日志

        ///// <summary>
        ///// 写日志
        ///// </summary>
        ///// <param name="fileName">日志文件名</param>
        ///// <param name="logType">日志级别</param>
        ///// <param name="moduleName">模块名</param>
        ///// <param name="logMsg">日志字符串</param>
        //public static void Writelog( string fileName, LogType logType, string moduleName, string logMsg )
        //{
        //    AppLogProxy.AppLog.WriteLog( fileName, logMsg );
        //    if ( writeLogToDCMP != null )
        //        writeLogToDCMP( logType, moduleName, logMsg, null );
        //}

        #endregion

        #region【方法】[静态] [公共] >>>> Writelog >>>> 写日志

        ///// <summary>
        ///// 写日志
        ///// </summary>
        ///// <param name="fileName">日志文件名</param>
        ///// <param name="logType">日志级别</param>
        ///// <param name="moduleName">模块名</param>
        ///// <param name="logMsg">日志字符串</param>
        ///// <param name="ee">异常信息</param>
        //public static void Writelog( string fileName, LogType logType, string moduleName, string logMsg, Exception ee )
        //{
        //    AppLogProxy.AppLog.WriteLog( fileName, logMsg, ee );

        //    if ( writeLogToDCMP != null )
        //        writeLogToDCMP( logType, moduleName, logMsg, null );
        //}

        #endregion

    }
}