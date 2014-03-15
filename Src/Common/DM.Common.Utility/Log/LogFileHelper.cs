using System.Collections.Generic;

namespace DM.Common.Utility.Log
{
    /// <summary>
    /// 日志文件帮助类
    /// </summary>
    internal class LogFileHelper
    {
        #region 属性变量
        public static readonly LogFileHelper Instance = new LogFileHelper();
        public static Dictionary<string, SizeWithUnitInfo> LogFileList;
        private readonly SizeWithUnitInfo _sizeUnit;
        #endregion

        #region 方法

        #region 构造函数
        public LogFileHelper()
        {
            LogFileList = new Dictionary<string, SizeWithUnitInfo>();

            //默认日志文件为3M
            _sizeUnit = new SizeWithUnitInfo {Size = 3, Unit = ByteUnit.MB};
        }
        #endregion

        #region 获取日志文件信息
        /// <summary>
        /// 获取日志文件信息
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>获取日志文件信息</returns>
        public SizeWithUnitInfo GetLogFile(string filePath)
        {
            if (LogFileList != null && LogFileList.Count > 0)
            {
                if (LogFileList.ContainsKey(filePath))
                {
                    return LogFileList[filePath];
                }
                return _sizeUnit;
            }
            return _sizeUnit;
        }

        #endregion

        #region 添加日志文件信息

        /// <summary>
        /// 添加日志文件信息
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="su">带单位的大小信息</param>
        public void AddLogFile(string filePath, SizeWithUnitInfo su)
        {
            if (LogFileList != null && LogFileList.Count > 0)
            {
                if (LogFileList.ContainsKey(filePath))
                {
                    LogFileList[filePath] = su;
                }
                else
                {
                    LogFileList.Add(filePath, su);
                }
            }
            else
            {
                if (LogFileList != null) LogFileList.Add(filePath, su);
            }
        }
        #endregion

        #endregion
    }
}
