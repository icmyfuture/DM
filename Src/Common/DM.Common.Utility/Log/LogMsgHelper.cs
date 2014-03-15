using System.Linq;
using System.Threading;
using System.Collections.Generic;

namespace DM.Common.Utility.Log
{
    /// <summary>
    /// 日志信息帮助类
    /// </summary>
    internal class LogMsgHelper
    {
        #region 属性变量
        public static readonly LogMsgHelper Instance = new LogMsgHelper();
        public Dictionary<string, string> LogList;
        //在添加日志的时候一定不是正在写日志的时候
        //在写日志的时候一定不是正在添加日志的时候
        private readonly Mutex _mu = new Mutex(false);

        #endregion

        #region 方法

        #region 构造函数
        public LogMsgHelper()
        {
            LogList = new Dictionary<string, string>();
        }
        #endregion

        #region 添加待保存日志信息

        /// <summary>
        /// 添加待保存日志信息
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="logMsg">日志消息</param>
        /// <param name="su">日志大小</param>
        /// <returns></returns>
        public string AddLogMsg(string filePath, string logMsg, SizeWithUnitInfo su)
        {

            //先添加日志文件大小限制信息
            LogFileHelper.Instance.AddLogFile(filePath, su);

            //再添加日志信息
            return AddLogMsg(filePath, logMsg);
        }
        #endregion

        #region 添加待保存日志信息
        /// <summary>
        /// 添加待保存日志信息
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="logMsg"></param>
        public string AddLogMsg(string filePath, string logMsg)
        {
            try
            {
                _mu.WaitOne();

                if (LogList != null && LogList.Count > 0)
                {
                    if (LogList.ContainsKey(filePath))
                    {
                        LogList[filePath] += logMsg;
                    }
                    else
                    {
                        LogList.Add(filePath, logMsg);
                    }
                }
                else
                {
                    if (LogList != null) LogList.Add(filePath, logMsg);
                }
            }
            catch
            {
            }
            finally
            {
                _mu.ReleaseMutex();
            }

            return logMsg;
        }
        #endregion

        #region 获取缓冲中所有待保存日志信息
        /// <summary>
        /// 获取缓冲中所有待保存日志信息
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetAllLogMsgAndClean()
        {
            Dictionary<string, string> list = null;

            try
            {
                _mu.WaitOne();

                //将当前日志信息转存到返回变量

                list = LogList.Keys.ToDictionary(key => key, key => LogList[key]);

                //再清空当前日志信息
                LogList.Clear();
            }
            catch
            {
            }
            finally
            {
                _mu.ReleaseMutex();
            }

            return list;
        }
        #endregion

        #endregion
    }
}
