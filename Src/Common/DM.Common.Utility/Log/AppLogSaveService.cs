using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web.Hosting;
using Timer = System.Timers.Timer;

namespace DM.Common.Utility.Log
{
    /// <summary>
    /// 应用程序日志保存线程
    /// </summary>
    internal class AppLogSaveService
    {
        #region 属性变量

        private static readonly object LockObj = new object();
        private readonly long _bufferSize;
        private readonly Timer _t;

        #endregion

        #region 构造函数

        public AppLogSaveService()
        {
            _bufferSize = 64 * 1024;

            #region 启动定时清理计时器

            _t = new Timer { Interval = 1000 };

            _t.Elapsed += SaveLogMsg;

            #endregion
        }

        #endregion

        #region 获取当前DLL所在路径

        /// <summary>
        /// 获取当前DLL所在路径（以\\结尾,支持BS和CS）
        /// </summary>
        public static string AppPhysicalPath
        {
            get
            {
                //优先考虑WEB应用程序
                string path = HostingEnvironment.ApplicationPhysicalPath ?? Thread.GetDomain().BaseDirectory;

                if (!path.EndsWith("\\")) path = path + "\\";

                return path;
            }
        }

        #endregion

        #region 启动日志定时保存服务

        /// <summary>
        /// 启动日志定时保存服务
        /// </summary>
        public void StartService()
        {
            _t.Start();
        }

        #endregion

        #region 保存日志信息

        private void SaveLogMsg(object sender, EventArgs e)
        {
            try
            {
                _t.Stop();

                Dictionary<string, string> list;
                lock (LockObj)
                {
                    //首先获取所有待保存日志,并且清空缓冲区
                    list = LogMsgHelper.Instance.GetAllLogMsgAndClean();
                }

                if (list != null && list.Count > 0)
                {
                    //循环保存每个日志信息
                    foreach (string file in list.Keys)
                    {
                        //保存信息到指定日志文件
                        WriteLog(file, list[file],
                                 ConvertToByteCountFromSizeAndUnit(LogFileHelper.Instance.GetLogFile(file)));
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                _t.Start();
            }
        }

        #endregion

        #region 记录日志，内部方法

        /// <summary>
        /// 记录指定文件、指定大小的日志信息
        /// </summary>
        /// <param name="filePath">文件完整路劲</param>
        /// <param name="msg">日志</param>
        /// <param name="fileSize">日志文件大小</param>
        /// <returns></returns>
        private void WriteLog(string filePath, string msg, long fileSize)
        {
            lock (LockObj)
            {
                //zj 2012-4-18 消息在写文件之前就要判断，否则内存中的消息与文件中的消息的总和可能远远大于3M
                long oldFileSize = GetFileLength(filePath);
                long strSize = Encoding.UTF8.GetByteCount(msg);

                //预先判断文件大小
                while (oldFileSize + strSize > fileSize)
                {
                    //写入
                    WriteLogFile(filePath, msg.Substring(0, (int)(fileSize - oldFileSize)));

                    //处理文件大小限制
                    DealWithFileSizeLimit(filePath, fileSize);

                    //截断字符串
                    msg = msg.Substring((int)(fileSize - oldFileSize));

                    //重新计算
                    oldFileSize = GetFileLength(filePath);
                    strSize = Encoding.UTF8.GetByteCount(msg);
                }

                //写入
                WriteLogFile(filePath, msg);
            }
        }

        private void DealWithFileSizeLimit(string filePath, long fileSize)
        {
            //调用日志大小处理
            if (GetFileLength(filePath) >= fileSize)
            {
                var di = new DirectoryInfo(Path.GetDirectoryName(filePath));
                FileInfo[] fis = di.GetFiles(Path.GetFileName(filePath) + "_*", SearchOption.TopDirectoryOnly);

                var fisList = new List<FileInfo>();

                #region 过滤文件

                foreach (FileInfo t in fis)
                {
                    //删除一星期之前的日志
                    if (DateTime.Now.Subtract(t.LastWriteTime).Days >= 7)
                    {
                        try
                        {
                            t.Delete();
                        }
                        catch
                        {
                            // 删除失败不处理
                        }
                    }
                    else
                    {
                        fisList.Add(t);
                    }
                }

                #endregion

                #region 重命名文件

                fisList = fisList.OrderByDescending(s => s.LastWriteTime).ToList();
                fisList.Insert(0, new FileInfo(filePath));

                //重命名老文件
                for (int i = fisList.Count - 1; i >= 0; i--)
                {
                    try
                    {
                        if (i == 0)
                        {
                            fisList[i].MoveTo(Path.GetDirectoryName(filePath) + @"\" +
                                              Path.GetFileName(fisList[i].FullName) + "_" + (i + 1));
                        }
                        else
                        {
                            fisList[i].MoveTo(Path.GetDirectoryName(filePath) + @"\" +
                                              Path.GetFileName(fisList[i].FullName).Split(new[] { ".txt_" },
                                                                                          StringSplitOptions.
                                                                                              RemoveEmptyEntries)[0] +
                                              ".txt_" + (i + 1));
                        }
                    }
                    catch
                    {
                    }
                }

                #endregion
            }
        }

        #endregion

        #region 写日志文件

        private void WriteLogFile(string filePath, string msg)
        {
            //将字符串转换为字节
            byte[] data = Encoding.UTF8.GetBytes(msg);

            FileStream fs = null;

            if (!string.IsNullOrEmpty(filePath) && data.Length > 0)
            {
                try
                {
                    lock (LockObj)
                    {
                        //确保文件存在 ，并且参数合法
                        if (!File.Exists(filePath))
                        {
                            CreateFile(filePath, 0);
                        }


                        fs = new FileStream(filePath, FileMode.Open, FileAccess.Write, FileShare.Write);

                        //确保起始位置在允许范围内
                        if (fs.Length >= 0)
                        {
                            WriteBuffer(fs, fs.Length, data.Length, data);
                        }
                    }
                }
                finally
                {
                    if (fs != null)
                    {
                        fs.Close();
                        fs.Dispose();
                    }
                }
            }
        }

        private void WriteBuffer(FileStream fs, long startIndex, long length, byte[] data)
        {
            //写入字节数
            int bytesWrite = 0;

            if (length > data.Length)
            {
            }
            else if (length < data.Length)
            {
                data = CutByteArray(data, 0, (int)length);
            }

            while (bytesWrite < data.Length)
            {
                fs.Position = startIndex + bytesWrite;

                if (data.Length - bytesWrite < _bufferSize)
                {
                    //当数据已经写完，但是还没到达指定结束边界，则结束
                    fs.Write(data, bytesWrite, data.Length - bytesWrite);
                    bytesWrite += data.Length - bytesWrite;
                }
                else
                {
                    fs.Write(data, bytesWrite, (int)_bufferSize);
                    bytesWrite += (int)_bufferSize;
                }
            }
        }

        private long GetFileLength(string filePath)
        {
            if (IsFileExist(filePath))
            {
                var fi = new FileInfo(filePath);

                return fi.Length;
            }
            return 0;
        }

        private bool IsFileExist(string filePath)
        {
            return File.Exists(Path.GetFullPath(filePath));
        }

        private void CreateFile(string filePath, long fileSize)
        {
            FileStream fs = null;

            if (!IsFileExist(filePath))
            {
                try
                {
                    string directory = Path.GetDirectoryName(Path.GetFullPath(filePath));

                    if (!Directory.Exists(Path.GetFullPath(directory)))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    //由于调用Create方法后会返回一个文件流，这个流导致文件被占用，因此要释放
                    fs = File.Create(filePath);
                    fs.SetLength(fileSize);
                }
                finally
                {
                    if (fs != null)
                    {
                        fs.Close();
                        fs.Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// 截取字节数组
        /// </summary>
        /// <param name="input">待截取字节数组</param>
        /// <param name="offset">起始位置</param>
        /// <param name="length">截取长度</param>
        /// <returns></returns>
        public byte[] CutByteArray(byte[] input, int offset, int length)
        {
            var returnByteArray = new byte[length];

            for (int i = offset; i < length; i++)
            {
                returnByteArray[i] = input[i];
            }

            return returnByteArray;
        }

        /// <summary>
        /// 将带单位的大小转换为字节大小
        /// </summary>
        /// <param name="swui">带单位的大小信息</param>
        /// <returns></returns>
        public long ConvertToByteCountFromSizeAndUnit(SizeWithUnitInfo swui)
        {
            switch (swui.Unit)
            {
                case ByteUnit.B:
                    {
                        return (long)swui.Size;
                    }
                case ByteUnit.KB:
                    {
                        return (long)swui.Size * 1024;
                    }
                case ByteUnit.MB:
                    {
                        return (long)swui.Size * 1048576;
                    }
                case ByteUnit.GB:
                    {
                        return (long)swui.Size * 1073741824;
                    }
                default:
                    {
                        throw new Exception("不支持的单位换算!");
                    }
            }
        }

        #endregion
    }
}