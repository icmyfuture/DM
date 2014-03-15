using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web.Hosting;
using DM.Common.Utility.Log;

namespace DM.Common.Utility
{
    /// <summary>
    /// 文件系统IO服务
    /// </summary>
    public class FileSystemHelper
    {
        #region 属性变量

        private static readonly long BufferSize;
        private static readonly object ThisLock = new object();

        #endregion

        #region 构造函数

        static FileSystemHelper()
        {
            BufferSize = 64*1024;
        }

        #endregion

        #region 读取操作

        #region 返回流

        #region 读取文件并返回文件流(使用完毕一定要记得关闭,多线程注意控制)

        /// <summary>
        /// 读取文件并返回文件流(使用完毕一定要记得关闭,多线程注意控制)
        /// </summary>
        /// <param name="filePath">文件路劲</param>
        /// <returns></returns>
        public static FileStream ReadFileStream(string filePath)
        {
            FileStream fs = null;

            //确保文件存在 ，并且参数合法
            if (IsFileExist(filePath))
            {
                try
                {
                    fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                }
                catch (Exception ex)
                {
                    if (fs != null)
                    {
                        fs.Close();
                        fs.Dispose();
                    }

                    AppLog.WriteLog<FileSystemHelper>(ex);

                    throw;
                }
            }

            return fs;
        }

        #endregion

        #region 读取文件并返回具有起始位置的文件流(使用完毕一定要记得关闭,多线程注意控制)

        /// <summary>
        /// 读取文件并返回具有起始位置的文件流(使用完毕一定要记得关闭,多线程注意控制)
        /// </summary>
        /// <param name="filePath">文件路劲</param>
        /// <param name="startIndex">开始位置</param>
        /// <returns></returns>
        public static FileStream ReadFileStream(string filePath, long startIndex)
        {
            FileStream fs = null;

            //确保文件存在 ，并且参数合法
            if (IsFileExist(filePath))
            {
                try
                {
                    fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
                             {Position = startIndex};
                }
                catch (Exception ex)
                {
                    if (fs != null)
                    {
                        fs.Close();
                        fs.Dispose();
                    }

                    AppLog.WriteLog<FileSystemHelper>(ex);

                    throw;
                }
            }

            return fs;
        }

        #endregion

        #region 读取文件并返回文件流(使用完毕一定要记得关闭,多线程注意控制)

        /// <summary>
        /// 读取文件并返回文件流(使用完毕一定要记得关闭,多线程注意控制)
        /// </summary>
        /// <param name="filePath">文件路劲</param>
        /// <returns></returns>
        public static FileStream ReadFileStreamToWrite(string filePath)
        {
            return ReadFileStreamToWrite(filePath, FileMode.Open);
        }

        /// <summary>
        /// 读取文件并返回文件流(使用完毕一定要记得关闭,多线程注意控制)
        /// </summary>
        /// <param name="filePath">文件路劲</param>
        /// <param name="fm">文件模式集合</param>
        /// <returns></returns>
        public static FileStream ReadFileStreamToWrite(string filePath, FileMode fm)
        {
            FileStream fs = null;

            //确保文件存在 ，并且参数合法
            if (IsFileExist(filePath))
            {
                try
                {
                    fs = new FileStream(filePath, fm, FileAccess.ReadWrite, FileShare.ReadWrite);
                }
                catch (Exception ex)
                {
                    if (fs != null)
                    {
                        fs.Close();
                        fs.Dispose();
                    }

                    AppLog.WriteLog<FileSystemHelper>(ex);

                    throw;
                }
            }

            return fs;
        }

        #endregion

        #region 读取文件并返回具有起始位置的文件流(使用完毕一定要记得关闭,多线程注意控制)

        /// <summary>
        /// 读取文件并返回具有起始位置的文件流(使用完毕一定要记得关闭,多线程注意控制)
        /// </summary>
        /// <param name="filePath">文件路劲</param>
        /// <param name="startIndex">开始位置</param>
        /// <returns></returns>
        public static FileStream ReadFileStreamToWrite(string filePath, long startIndex)
        {
            FileStream fs = null;

            //确保文件存在 ，并且参数合法
            if (IsFileExist(filePath))
            {
                try
                {
                    fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite)
                             {Position = startIndex};
                }
                catch (Exception ex)
                {
                    if (fs != null)
                    {
                        fs.Close();
                        fs.Dispose();
                    }

                    AppLog.WriteLog<FileSystemHelper>(ex);

                    throw;
                }
            }

            return fs;
        }

        #endregion

        #endregion

        #region 返回字符串

        #region 读取文件并返回指定开始位置以后的所有字符串

        /// <summary>
        /// 读取文件并返回指定开始位置以后的所有字符串
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="startIndex">起始位置</param>
        /// <returns></returns>
        public static string ReadFileString(string filePath, long startIndex)
        {
            return ReadFileString(filePath, startIndex, Encoding.UTF8);
        }

        /// <summary>
        /// 读取文件并返回指定开始位置以后的所有字符串
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="encoder">Encoding</param>
        /// <returns></returns>
        public static string ReadFileString(string filePath, long startIndex, Encoding encoder)
        {
            byte[] b = ReadFileByte(filePath, startIndex);

            if (b != null && b.Length > 0)
            {
                return encoder.GetString(b);
            }
            return string.Empty;
        }

        #endregion

        #region 读取文件并返回字符串

        /// <summary>
        /// 读取文件并返回字符串
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static string ReadFileString(string filePath)
        {
            return ReadFileString(filePath, Encoding.UTF8);
        }

        /// <summary>
        /// 读取文件并返回字符串
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="encoder">Encoding</param>
        /// <returns></returns>
        public static string ReadFileString(string filePath, Encoding encoder)
        {
            byte[] b = ReadFileByte(filePath);

            if (b != null && b.Length > 0)
            {
                return encoder.GetString(b);
            }
            return string.Empty;
        }

        #endregion

        #region 读取文件并返回指定范围的字符串

        /// <summary>
        /// 读取文件并返回指定范围的字符串
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="length">读取长度</param>
        /// <returns></returns>
        public static string ReadFileString(string filePath, long startIndex, long length)
        {
            return ReadFileString(filePath, startIndex, length, Encoding.UTF8);
        }

        /// <summary>
        /// 读取文件并返回指定范围的字符串
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="length">读取长度</param>
        /// <param name="encoder">Encoding</param>
        /// <returns></returns>
        public static string ReadFileString(string filePath, long startIndex, long length, Encoding encoder)
        {
            byte[] b = ReadFileByte(filePath, startIndex, length);

            if (b != null && b.Length > 0)
            {
                return encoder.GetString(b);
            }
            return string.Empty;
        }

        #endregion

        #endregion

        #region 返回字节数组

        #region 读取文件并返回指定开始位置以后的所有字节数组

        /// <summary>
        /// 读取文件并返回指定开始位置以后的所有字节数组
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="startIndex">起始位置</param>
        /// <returns></returns>
        public static byte[] ReadFileByte(string filePath, long startIndex)
        {
            var returnBytes = new byte[0];
            FileStream fs = null;

            //确保文件存在 ，并且参数合法
            if (IsFileExist(filePath))
            {
                try
                {
                    fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);

                    //确保起始位置在允许范围内
                    if (fs.Length >= startIndex)
                    {
                        returnBytes = ReadBuffer(fs, startIndex);
                    }
                }
                catch (Exception ex)
                {
                    AppLog.WriteLog<FileSystemHelper>(ex);

                    throw;
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

            return returnBytes;
        }

        #endregion

        #region 读取文件并返回字节数组

        /// <summary>
        /// 读取文件并返回字节数组
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static byte[] ReadFileByte(string filePath)
        {
            var returnBytes = new byte[0];
            FileStream fs = null;

            //确保文件存在 ，并且参数合法
            if (IsFileExist(filePath))
            {
                try
                {
                    fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);

                    //确保起始位置在允许范围内
                    if (fs.Length >= 0)
                    {
                        returnBytes = ReadBuffer(fs, 0);
                    }
                }
                catch (Exception ex)
                {
                    AppLog.WriteLog<FileSystemHelper>(ex);

                    throw;
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

            return returnBytes;
        }

        #endregion

        #region 读取文件并返回指定范围的字节数组

        /// <summary>
        /// 读取文件并返回指定范围的字节数组
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="length">读取长度</param>
        /// <returns></returns>
        public static byte[] ReadFileByte(string filePath, long startIndex, long length)
        {
            var returnBytes = new byte[0];
            FileStream fs = null;

            //确保文件存在 ，并且参数合法
            if (IsFileExist(filePath) && length > 0 && startIndex > -1)
            {
                try
                {
                    fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);

                    //确保起始位置在允许范围内
                    if (fs.Length >= startIndex)
                    {
                        //确保从起始位置开始到结束不小于请求长度,如果小于，则返回剩余部分
                        if ((fs.Length - (startIndex - 1)) < length)
                        {
                        }

                        returnBytes = ReadBuffer(fs, startIndex);
                    }
                }
                catch (Exception ex)
                {
                    AppLog.WriteLog<FileSystemHelper>(ex);

                    throw;
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

            return returnBytes;
        }

        private static byte[] ReadBuffer(MemoryStream fs, long startIndex)
        {
            //读取字节数
            int bytesRead;
            var buffer = new byte[BufferSize];
            var returnBytes = new byte[0];

            do
            {
                fs.Position = startIndex;
                bytesRead = fs.Read(buffer, 0, buffer.Length);

                if (bytesRead < BufferSize)
                {
//当读取字节数小于缓冲区大小，则截取掉多于部分
                    buffer = ByteConverter.CutByteArray(buffer, 0, bytesRead);
                }

                returnBytes = ByteConverter.MergeByteArray(returnBytes, buffer);
                startIndex += buffer.Length;
            } while (bytesRead >= BufferSize);

            return returnBytes;
        }

        private static byte[] ReadBuffer(FileStream fs, long startIndex)
        {
            //读取字节数
            int bytesRead;
            var buffer = new byte[BufferSize];
            var returnBytes = new byte[0];

            do
            {
                fs.Position = startIndex;
                bytesRead = fs.Read(buffer, 0, buffer.Length);

                if (bytesRead < BufferSize)
                {
//当读取字节数小于缓冲区大小，则截取掉多于部分
                    buffer = ByteConverter.CutByteArray(buffer, 0, bytesRead);
                }

                returnBytes = ByteConverter.MergeByteArray(returnBytes, buffer);
                startIndex += buffer.Length;
            } while (bytesRead >= BufferSize);

            return returnBytes;
        }

        #endregion

        #endregion

        #region 从指定流返回字节数组

        #region 读取文件并返回指定开始位置以后的所有字节数组

        /// <summary>
        /// 读取文件流并返回指定开始位置以后的所有字节数组
        /// </summary>
        /// <param name="fs">文件流</param>
        /// <param name="startIndex">起始位置</param>
        /// <returns></returns>
        public static byte[] ReadFileByte(FileStream fs, long startIndex)
        {
            var returnBytes = new byte[0];

            //确保文件存在 ，并且参数合法
            if (fs != null)
            {
                try
                {
                    fs.Position = 0;

                    //确保起始位置在允许范围内
                    if (fs.Length >= startIndex)
                    {
                        returnBytes = ReadBuffer(fs, startIndex);
                    }
                }
                catch (Exception ex)
                {
                    AppLog.WriteLog<FileSystemHelper>(ex);

                    throw;
                }
                finally
                {
                    fs.Position = 0;
                }
            }

            return returnBytes;
        }

        #endregion

        #region 读取文件并返回字节数组

        /// <summary>
        /// 读取文件并返回字节数组
        /// </summary>
        /// <returns></returns>
        public static byte[] ReadFileByte(FileStream fs)
        {
            var returnBytes = new byte[0];

            //确保文件存在 ，并且参数合法
            if (fs != null)
            {
                try
                {
                    fs.Position = 0;

                    //确保起始位置在允许范围内
                    if (fs.Length >= 0)
                    {
                        returnBytes = ReadBuffer(fs, 0);
                    }
                }
                catch (Exception ex)
                {
                    AppLog.WriteLog<FileSystemHelper>(ex);

                    throw;
                }
                finally
                {
                    fs.Position = 0;
                }
            }

            return returnBytes;
        }

        #endregion

        #region 读取文件并返回指定范围的字节数组

        /// <summary>
        /// 读取文件并返回指定范围的字节数组
        /// </summary>
        /// <param name="fs">文件流</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="length">读取长度</param>
        /// <returns></returns>
        public static byte[] ReadFileByte(FileStream fs, long startIndex, long length)
        {
            var returnBytes = new byte[0];

            //确保文件存在 ，并且参数合法
            if (fs != null && length > 0 && startIndex > -1)
            {
                try
                {
                    fs.Position = 0;

                    //确保起始位置在允许范围内
                    if (fs.Length >= startIndex)
                    {
                        //确保从起始位置开始到结束不小于请求长度,如果小于，则返回剩余部分
                        if ((fs.Length - (startIndex - 1)) < length)
                        {
                        }

                        returnBytes = ReadBuffer(fs, startIndex);
                    }
                }
                catch (Exception ex)
                {
                    AppLog.WriteLog<FileSystemHelper>(ex);

                    throw;
                }
                finally
                {
                    fs.Position = 0;
                }
            }

            return returnBytes;
        }

        #endregion

        #endregion

        #region 从指定流返回字节数组

        #region 读取文件并返回指定开始位置以后的所有字节数组

        /// <summary>
        /// 读取文件流并返回指定开始位置以后的所有字节数组
        /// </summary>
        /// <param name="fs">文件流</param>
        /// <param name="startIndex">起始位置</param>
        /// <returns></returns>
        public static byte[] ReadFileByte(MemoryStream fs, long startIndex)
        {
            var returnBytes = new byte[0];

            //确保文件存在 ，并且参数合法
            if (fs != null)
            {
                try
                {
                    fs.Position = 0;

                    //确保起始位置在允许范围内
                    if (fs.Length >= startIndex)
                    {
                        returnBytes = ReadBuffer(fs, startIndex);
                    }
                }
                catch (Exception ex)
                {
                    AppLog.WriteLog<FileSystemHelper>(ex);

                    throw;
                }
                finally
                {
                    fs.Position = 0;
                }
            }

            return returnBytes;
        }

        #endregion

        #region 读取文件并返回字节数组

        /// <summary>
        /// 读取文件并返回字节数组
        /// </summary>
        /// <returns></returns>
        public static byte[] ReadFileByte(MemoryStream fs)
        {
            var returnBytes = new byte[0];

            //确保文件存在 ，并且参数合法
            if (fs != null)
            {
                try
                {
                    fs.Position = 0;

                    //确保起始位置在允许范围内
                    if (fs.Length >= 0)
                    {
                        returnBytes = ReadBuffer(fs, 0);
                    }
                }
                catch (Exception ex)
                {
                    AppLog.WriteLog<FileSystemHelper>(ex);

                    throw;
                }
                finally
                {
                    fs.Position = 0;
                }
            }

            return returnBytes;
        }

        #endregion

        #region 读取文件并返回指定范围的字节数组

        /// <summary>
        /// 读取文件并返回指定范围的字节数组
        /// </summary>
        /// <param name="fs">文件流</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="length">读取长度</param>
        /// <returns></returns>
        public static byte[] ReadFileByte(MemoryStream fs, long startIndex, long length)
        {
            var returnBytes = new byte[0];

            //确保文件存在 ，并且参数合法
            if (fs != null && length > 0 && startIndex > -1)
            {
                try
                {
                    fs.Position = 0;

                    //确保起始位置在允许范围内
                    if (fs.Length >= startIndex)
                    {
                        //确保从起始位置开始到结束不小于请求长度,如果小于，则返回剩余部分
                        if ((fs.Length - (startIndex - 1)) < length)
                        {
                        }

                        returnBytes = ReadBuffer(fs, startIndex);
                    }
                }
                catch (Exception ex)
                {
                    AppLog.WriteLog<FileSystemHelper>(ex);

                    throw;
                }
                finally
                {
                    fs.Position = 0;
                }
            }

            return returnBytes;
        }

        #endregion

        #endregion

        #endregion

        #region 写入操作

        #region 写入字符串

        #region 向文件指定开始位置写入字符串

        /// <summary>
        /// 向文件指定开始位置写入字符串
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="data">字符串</param>
        /// <returns></returns>
        public static void WriteFileString(string filePath, long startIndex, string data)
        {
            WriteFileString(filePath, startIndex, Encoding.UTF8, data);
        }

        /// <summary>
        /// 向文件指定开始位置写入字符串
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="encoder">Encoding</param>
        /// <param name="data">字符串</param>
        /// <returns></returns>
        public static void WriteFileString(string filePath, long startIndex, Encoding encoder, string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                WriteFileByte(filePath, startIndex, encoder.GetBytes(data));
            }
        }

        #endregion

        #region 向文件附加字符串

        /// <summary>
        /// 向文件附加字符串
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="data">字符串</param>
        /// <returns></returns>
        public static void AppendFileString(string filePath, string data)
        {
            AppendFileString(filePath, Encoding.UTF8, data);
        }

        /// <summary>
        /// 向文件附加字符串
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="encoder">Encoding</param>
        /// <param name="data">字符串</param>
        /// <returns></returns>
        public static void AppendFileString(string filePath, Encoding encoder, string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                AppendFileByte(filePath, encoder.GetBytes(data));
            }
        }

        #endregion

        #region 向文件指定范围附加字符串

        /// <summary>
        /// 向文件指定范围附加字符串
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="length">写入长度</param>
        /// <param name="data">字符串</param>
        /// <returns></returns>
        public static void AppendFileString(string filePath, long length, string data)
        {
            AppendFileString(filePath, length, Encoding.UTF8, data);
        }

        /// <summary>
        /// 向文件指定范围附加字符串
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="length">写入长度</param>
        /// <param name="encoder">Encoding</param>
        /// <param name="data">字符串</param>
        /// <returns></returns>
        public static void AppendFileString(string filePath, long length, Encoding encoder, string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                AppendFileByte(filePath, length, encoder.GetBytes(data));
            }
        }

        #endregion

        #region 向文件写入字符串

        /// <summary>
        /// 向文件写入字符串
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="data">字符串</param>
        /// <returns></returns>
        public static void WriteFileString(string filePath, string data)
        {
            WriteFileString(filePath, Encoding.UTF8, data);
        }

        /// <summary>
        /// 读取文件并返回字符串
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="encoder">Encoding</param>
        /// <param name="data">字符串</param>
        /// <returns></returns>
        public static void WriteFileString(string filePath, Encoding encoder, string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                WriteFileByte(filePath, encoder.GetBytes(data));
            }
        }

        #endregion

        #region 向文件指定范围写入字符串

        /// <summary>
        /// 向文件指定范围写入字符串
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="length">写入长度</param>
        /// <param name="data">字符串</param>
        /// <returns></returns>
        public static void WriteFileString(string filePath, long startIndex, long length, string data)
        {
            WriteFileString(filePath, startIndex, length, Encoding.UTF8, data);
        }

        /// <summary>
        /// 向文件指定范围写入字符串
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="length">写入长度</param>
        /// <param name="encoder">Encoding</param>
        /// <param name="data">字符串</param>
        /// <returns></returns>
        public static void WriteFileString(string filePath, long startIndex, long length, Encoding encoder, string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                WriteFileByte(filePath, startIndex, length, encoder.GetBytes(data));
            }
        }

        #endregion

        #endregion

        #region 写入字节数组

        #region 将指定字节数组附加进文件中

        /// <summary>
        /// 将指定字节数组附加进文件中
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="data">待写数据</param>
        /// <returns></returns>
        public static void AppendFileByte(string filePath, byte[] data)
        {
            FileStream fs = null;

            if (!string.IsNullOrEmpty(filePath) && data != null && data.Length > 0)
            {
                //确保文件存在 ，并且参数合法
                if (!IsFileExist(filePath))
                {
                    CreateFile(filePath);
                }

                try
                {
                    lock (ThisLock)
                    {
                        fs = ReadFileStreamToWrite(filePath);

                        AppendFileByte(fs, data);
                    }
                }
                catch (Exception ex)
                {
                    AppLog.WriteLog<FileSystemHelper>(ex);
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

        #endregion

        #region 将指定字节数组附加入文件中指定范围

        /// <summary>
        /// 将指定字节数组写入文件中指定范围
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="length">写入长度</param>
        /// <param name="data">待写数据</param>
        /// <returns></returns>
        public static void AppendFileByte(string filePath, long length, byte[] data)
        {
            FileStream fs = null;

            if (!string.IsNullOrEmpty(filePath) && length > 0 && data != null && data.Length > 0)
            {
                //确保文件存在 ，并且参数合法
                if (!IsFileExist(filePath))
                {
                    CreateFile(filePath);
                }

                try
                {
                    lock (ThisLock)
                    {
                        fs = ReadFileStreamToWrite(filePath);

                        AppendFileByte(fs, length, data);
                    }
                }
                catch (Exception ex)
                {
                    AppLog.WriteLog<FileSystemHelper>(ex);
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

        #endregion

        #region 将指定字节数组写入文件中指定开始位置

        /// <summary>
        /// 将指定字节数组写入文件中指定开始位置
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="data">待写数据</param>
        /// <returns></returns>
        public static void WriteFileByte(string filePath, long startIndex, byte[] data)
        {
            FileStream fs = null;

            if (!string.IsNullOrEmpty(filePath) && startIndex > -1 && data != null && data.Length > 0)
            {
                //确保文件存在 ，并且参数合法
                if (!IsFileExist(filePath))
                {
                    CreateFile(filePath);
                }

                try
                {
                    lock (ThisLock)
                    {
                        fs = ReadFileStreamToWrite(filePath);

                        WriteFileByte(fs, startIndex, data);
                    }
                }
                catch (Exception ex)
                {
                    AppLog.WriteLog<FileSystemHelper>(ex);
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

        #endregion

        #region 将指定字节数组写入文件中

        /// <summary>
        /// 将指定字节数组写入文件中
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="data">待写数据</param>
        /// <returns></returns>
        public static void WriteFileByte(string filePath, byte[] data)
        {
            FileStream fs = null;

            if (!string.IsNullOrEmpty(filePath) && data != null && data.Length > 0)
            {
                //确保文件存在 ，并且参数合法
                if (!IsFileExist(filePath))
                {
                    CreateFile(filePath);
                }

                try
                {
                    lock (ThisLock)
                    {
                        fs = ReadFileStreamToWrite(filePath, FileMode.Truncate);

                        WriteFileByte(fs, data);
                    }
                }
                catch (Exception ex)
                {
                    AppLog.WriteLog<FileSystemHelper>(ex);
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

        #endregion

        #region 将指定字节数组写入文件中指定范围

        /// <summary>
        /// 将指定字节数组写入文件中指定范围
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="length">写入长度</param>
        /// <param name="data">待写数据</param>
        /// <returns></returns>
        public static void WriteFileByte(string filePath, long startIndex, long length, byte[] data)
        {
            FileStream fs = null;

            if (!string.IsNullOrEmpty(filePath) && length > 0 && startIndex > -1 && data != null &&
                data.Length > 0)
            {
                //确保文件存在 ，并且参数合法
                if (!IsFileExist(filePath))
                {
                    CreateFile(filePath);
                }

                try
                {
                    lock (ThisLock)
                    {
                        fs = ReadFileStreamToWrite(filePath);

                        WriteFileByte(fs, startIndex, length, data);
                    }
                }
                catch (Exception ex)
                {
                    AppLog.WriteLog<FileSystemHelper>(ex);
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

        #endregion

        #endregion

        #region 写入字节数组到指定流

        #region 将指定字节数组附加进文件流中

        /// <summary>
        /// 将指定字节数组附加进文件流中
        /// </summary>
        /// <param name="fs">文件流</param>
        /// <param name="data">待写数据</param>
        /// <returns></returns>
        public static void AppendFileByte(FileStream fs, byte[] data)
        {
            if (fs != null && data != null && data.Length > 0)
            {
                try
                {
                    lock (ThisLock)
                    {
                        //确保起始位置在允许范围内
                        if (fs.Length >= 0)
                        {
                            WriteBuffer(fs, fs.Length, data.Length, data);
                        }
                    }
                }
                catch (Exception ex)
                {
                    AppLog.WriteLog<FileSystemHelper>(ex);
                }
                finally
                {
                    fs.Position = 0;
                }
            }
        }

        #endregion

        #region 将指定字节数组附加入文件中指定范围

        /// <summary>
        /// 将指定字节数组写入文件中指定范围
        /// </summary>
        /// <param name="fs">文件流</param>
        /// <param name="length">写入长度</param>
        /// <param name="data">待写数据</param>
        /// <returns></returns>
        public static void AppendFileByte(FileStream fs, long length, byte[] data)
        {
            if (fs != null && length > 0 && data != null && data.Length > 0)
            {
                try
                {
                    lock (ThisLock)
                    {
                        //确保起始位置在允许范围内
                        if (fs.Length >= 0)
                        {
                            WriteBuffer(fs, fs.Length, length, data);
                        }
                    }
                }
                catch (Exception ex)
                {
                    AppLog.WriteLog<FileSystemHelper>(ex);
                }
                finally
                {
                    fs.Position = 0;
                }
            }
        }

        #endregion

        #region 将指定字节数组写入文件中指定开始位置

        /// <summary>
        /// 将指定字节数组写入文件中指定开始位置
        /// </summary>
        /// <param name="fs">文件流</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="data">待写数据</param>
        /// <returns></returns>
        public static void WriteFileByte(FileStream fs, long startIndex, byte[] data)
        {
            if (fs != null && startIndex > -1 && data != null && data.Length > 0)
            {
                try
                {
                    lock (ThisLock)
                    {
                        //确保起始位置在允许范围内
                        if (fs.Length >= startIndex)
                        {
                            WriteBuffer(fs, startIndex, data.Length, data);
                        }
                    }
                }
                catch (Exception ex)
                {
                    AppLog.WriteLog<FileSystemHelper>(ex);

                    throw;
                }
                finally
                {
                    fs.Position = 0;
                }
            }
        }

        #endregion

        #region 将指定字节数组写入文件中

        /// <summary>
        /// 将指定字节数组写入文件中
        /// </summary>
        /// <param name="fs">文件流</param>
        /// <param name="data">待写数据</param>
        /// <returns></returns>
        public static void WriteFileByte(FileStream fs, byte[] data)
        {
            if (fs != null && data != null && data.Length > 0)
            {
                try
                {
                    lock (ThisLock)
                    {
                        //确保起始位置在允许范围内
                        if (fs.Length >= 0)
                        {
                            WriteBuffer(fs, 0, data.Length, data);
                        }
                    }
                }
                catch (Exception ex)
                {
                    AppLog.WriteLog<FileSystemHelper>(ex);

                    throw;
                }
                finally
                {
                    fs.Position = 0;
                }
            }
        }

        #endregion

        #region 将指定字节数组写入文件中指定范围

        /// <summary>
        /// 将指定字节数组写入文件中指定范围
        /// </summary>
        /// <param name="fs">文件流</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="length">写入长度</param>
        /// <param name="data">待写数据</param>
        /// <returns></returns>
        public static void WriteFileByte(FileStream fs, long startIndex, long length, byte[] data)
        {
            if (fs != null && length > 0 && startIndex > -1 && data != null && data.Length > 0)
            {
                try
                {
                    lock (ThisLock)
                    {
                        //确保起始位置在允许范围内
                        if (fs.Length >= startIndex)
                        {
                            //确保从起始位置开始到结束不小于请求长度,如果小于，则返回剩余部分
                            if ((fs.Length - (startIndex - 1)) < length)
                            {
                                length = fs.Length - (startIndex - 1);
                            }

                            WriteBuffer(fs, startIndex, length, data);
                        }
                    }
                }
                catch (Exception ex)
                {
                    AppLog.WriteLog<FileSystemHelper>(ex);

                    throw;
                }
                finally
                {
                    fs.Position = 0;
                }
            }
        }

        #endregion

        #endregion

        #region 写入字节数组到指定流

        #region 将指定字节数组附加进文件流中

        /// <summary>
        /// 将指定字节数组附加进文件流中
        /// </summary>
        /// <param name="fs">文件流</param>
        /// <param name="data">待写数据</param>
        /// <returns></returns>
        public static void AppendFileByte(MemoryStream fs, byte[] data)
        {
            if (fs != null && data != null && data.Length > 0)
            {
                try
                {
                    lock (ThisLock)
                    {
                        //确保起始位置在允许范围内
                        if (fs.Length >= 0)
                        {
                            WriteBuffer(fs, fs.Length, data.Length, data);
                        }
                    }
                }
                catch (Exception ex)
                {
                    AppLog.WriteLog<FileSystemHelper>(ex);
                }
                finally
                {
                    fs.Position = 0;
                }
            }
        }

        #endregion

        #region 将指定字节数组附加入文件中指定范围

        /// <summary>
        /// 将指定字节数组写入文件中指定范围
        /// </summary>
        /// <param name="fs">文件流</param>
        /// <param name="length">写入长度</param>
        /// <param name="data">待写数据</param>
        /// <returns></returns>
        public static void AppendFileByte(MemoryStream fs, long length, byte[] data)
        {
            if (fs != null && length > 0 && data != null && data.Length > 0)
            {
                try
                {
                    lock (ThisLock)
                    {
                        //确保起始位置在允许范围内
                        if (fs.Length >= 0)
                        {
                            WriteBuffer(fs, fs.Length, length, data);
                        }
                    }
                }
                catch (Exception ex)
                {
                    AppLog.WriteLog<FileSystemHelper>(ex);
                }
                finally
                {
                    fs.Position = 0;
                }
            }
        }

        #endregion

        #region 将指定字节数组写入文件中指定开始位置

        /// <summary>
        /// 将指定字节数组写入文件中指定开始位置
        /// </summary>
        /// <param name="fs">文件流</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="data">待写数据</param>
        /// <returns></returns>
        public static void WriteFileByte(MemoryStream fs, long startIndex, byte[] data)
        {
            if (fs != null && startIndex > -1 && data != null && data.Length > 0)
            {
                try
                {
                    lock (ThisLock)
                    {
                        //确保起始位置在允许范围内
                        if (fs.Length >= startIndex)
                        {
                            WriteBuffer(fs, startIndex, data.Length, data);
                        }
                    }
                }
                catch (Exception ex)
                {
                    AppLog.WriteLog<FileSystemHelper>(ex);

                    throw;
                }
                finally
                {
                    fs.Position = 0;
                }
            }
        }

        #endregion

        #region 将指定字节数组写入文件中

        /// <summary>
        /// 将指定字节数组写入文件中
        /// </summary>
        /// <param name="fs">文件流</param>
        /// <param name="data">待写数据</param>
        /// <returns></returns>
        public static void WriteFileByte(MemoryStream fs, byte[] data)
        {
            if (fs != null && data != null && data.Length > 0)
            {
                try
                {
                    lock (ThisLock)
                    {
                        //确保起始位置在允许范围内
                        if (fs.Length >= 0)
                        {
                            WriteBuffer(fs, 0, data.Length, data);
                        }
                    }
                }
                catch (Exception ex)
                {
                    AppLog.WriteLog<FileSystemHelper>(ex);

                    throw;
                }
                finally
                {
                    fs.Position = 0;
                }
            }
        }

        #endregion

        #region 将指定字节数组写入文件中指定范围

        /// <summary>
        /// 将指定字节数组写入文件中指定范围
        /// </summary>
        /// <param name="fs">文件流</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="length">写入长度</param>
        /// <param name="data">待写数据</param>
        /// <returns></returns>
        public static void WriteFileByte(MemoryStream fs, long startIndex, long length, byte[] data)
        {
            if (fs != null && length > 0 && startIndex > -1 && data != null && data.Length > 0)
            {
                try
                {
                    lock (ThisLock)
                    {
                        //确保起始位置在允许范围内
                        if (fs.Length >= startIndex)
                        {
                            //确保从起始位置开始到结束不小于请求长度,如果小于，则返回剩余部分
                            if ((fs.Length - (startIndex - 1)) < length)
                            {
                                length = fs.Length - (startIndex - 1);
                            }

                            WriteBuffer(fs, startIndex, length, data);
                        }
                    }
                }
                catch (Exception ex)
                {
                    AppLog.WriteLog<FileSystemHelper>(ex);

                    throw;
                }
                finally
                {
                    fs.Position = 0;
                }
            }
        }

        #endregion

        #endregion

        #endregion

        #region 清空操作

        /// <summary>
        /// 清空文件内容
        /// </summary>
        /// <param name="filePath"></param>
        public static void ClearFile(string filePath)
        {
            StreamWriter sw = null;

            if (!string.IsNullOrEmpty(filePath))
            {
                //确保文件存在 ，并且参数合法
                if (!IsFileExist(filePath))
                {
                    CreateFile(filePath);
                }
                else
                {
                    try
                    {
                        lock (ThisLock)
                        {
                            sw = new StreamWriter(filePath, false);
                            sw.Write("");
                        }
                    }
                    catch (Exception ex)
                    {
                        AppLog.WriteLog<FileSystemHelper>(ex);

                        throw;
                    }
                    finally
                    {
                        if (sw != null)
                        {
                            sw.Close();
                            sw.Dispose();
                        }
                    }
                }
            }
        }

        #endregion

        #region 判断文件是否存在

        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static bool IsFileExist(string filePath)
        {
            return File.Exists(filePath);
        }

        #endregion

        #region 判断目录是否存在

        /// <summary>
        /// 判断目录是否存在
        /// </summary>
        /// <param name="fileDirectory">文件目录</param>
        /// <returns></returns>
        public static bool IsDirectoryExist(string fileDirectory)
        {
            return Directory.Exists(fileDirectory);
        }

        #endregion

        #region 获取文件名后缀

        /// <summary>
        /// 获取文件名后缀
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static string GetFileExtension(string filePath)
        {
            return Path.GetExtension(filePath);
        }

        #endregion

        #region 获取文件名(带后缀)

        /// <summary>
        /// 获取文件名(带后缀)
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static string GetFileNameWithExtension(string filePath)
        {
            return Path.GetFileName(filePath);
        }

        #endregion

        #region 获取文件名(不带后缀)

        /// <summary>
        /// 获取文件名(不带后缀)
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static string GetFileNameWithOutExtension(string filePath)
        {
            return Path.GetFileNameWithoutExtension(filePath);
        }

        #endregion

        #region 截取文件

        /// <summary>
        /// 截取文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="fileLength">保留长度（从起始位置开始）</param>
        /// <returns></returns>
        public static void SetLength(string filePath, long fileLength)
        {
            FileStream fs = null;

            try
            {
                fs = ReadFileStream(filePath);

                fs.SetLength(fileLength);
            }
            catch (Exception ex)
            {
                AppLog.WriteLog<FileSystemHelper>(ex);

                throw;
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

        #endregion

        #region 获取文件长度

        /// <summary>
        /// 获取文件长度
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static long GetFileLength(string filePath)
        {
            if (IsFileExist(filePath))
            {
                var fi = new FileInfo(filePath);

                return fi.Length;
            }
            return 0;
        }

        #endregion

        #region 获取目录

        /// <summary>
        /// 获取目录
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static string GetFileDirectory(string filePath)
        {
            return Path.GetDirectoryName(filePath);
        }

        #endregion

        #region 创建文件

        public static void CreateFile(string filePath)
        {
            CreateFile(filePath, 0);
        }

        public static void CreateFile(string filePath, long fileSize)
        {
            FileStream fs = null;

            if (!IsFileExist(filePath))
            {
                string directory = GetFileDirectory(filePath);

                if (!IsDirectoryExist(directory))
                {
                    CreateDirectory(directory);
                }

                try
                {
                    //由于调用Create方法后会返回一个文件流，这个流导致文件被占用，因此要释放
                    fs = File.Create(filePath);
                    fs.SetLength(fileSize);
                }
                catch (Exception ex)
                {
                    AppLog.WriteLog<FileSystemHelper>(ex);

                    throw;
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

        #endregion

        #region 创建目录

        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="fileDirectory">目录</param>
        public static void CreateDirectory(string fileDirectory)
        {
            if (!IsDirectoryExist(fileDirectory))
            {
                try
                {
                    Directory.CreateDirectory(fileDirectory);
                }
                catch (Exception ex)
                {
                    AppLog.WriteLog<FileSystemHelper>(ex);

                    throw;
                }
            }
        }

        #endregion

        #region 删除文件

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        public static void DeleteFile(string filePath)
        {
            if (IsFileExist(filePath))
            {
                try
                {
                    File.Delete(filePath);
                }
                catch (Exception ex)
                {
                    AppLog.WriteLog<FileSystemHelper>(ex);

                    throw;
                }
            }
        }

        #endregion

        #region 删除目录

        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="fileDirectory">目录</param>
        public static void DeleteDirecotry(string fileDirectory)
        {
            if (IsDirectoryExist(fileDirectory))
            {
                try
                {
                    Directory.Delete(fileDirectory, true);
                }
                catch (Exception ex)
                {
                    AppLog.WriteLog<FileSystemHelper>(ex);

                    throw;
                }
            }
        }

        #endregion

        #region 改文件名字

        /// <summary>
        /// 改文件名字
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="newName">新文件名</param>
        public static void ReNameFile(string filePath, string newName)
        {
            if (IsFileExist(filePath))
            {
                try
                {
                    var fi = new FileInfo(filePath);

                    fi.MoveTo(Path.Combine(fi.Directory.FullName, newName));
                }
                catch (Exception ex)
                {
                    AppLog.WriteLog<FileSystemHelper>(ex);

                    throw;
                }
            }
        }

        #endregion

        #region 改目录名字

        /// <summary>
        /// 改目录名字
        /// </summary>
        /// <param name="fileDirectory">目录</param>
        /// <param name="newDirectoryName">新目录名</param>
        public static void ReNameDirectory(string fileDirectory, string newDirectoryName)
        {
            if (IsDirectoryExist(fileDirectory))
            {
                try
                {
                    var di = new DirectoryInfo(fileDirectory);

                    di.MoveTo(Path.Combine(di.Parent.FullName, newDirectoryName));
                }
                catch (Exception ex)
                {
                    AppLog.WriteLog<FileSystemHelper>(ex);

                    throw;
                }
            }
        }

        #endregion

        #region 移动文件

        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="oldPath">旧位置</param>
        /// <param name="newPath">新位置</param>
        public static void MoveFile(string oldPath, string newPath)
        {
            if (IsFileExist(oldPath))
            {
                if (!IsDirectoryExist(GetFileDirectory(newPath)))
                {
                    CreateDirectory(GetFileDirectory(newPath));
                }

                try
                {
                    var fi = new FileInfo(oldPath);

                    fi.MoveTo(newPath);
                }
                catch (Exception ex)
                {
                    AppLog.WriteLog<FileSystemHelper>(ex);

                    throw;
                }
            }
        }

        #endregion

        #region 移动目录

        /// <summary>
        /// 移动目录
        /// </summary>
        /// <param name="oldDirectory">旧位置</param>
        /// <param name="newDirectory">新位置</param>
        public static void MoveDirecotry(string oldDirectory, string newDirectory)
        {
            if (IsDirectoryExist(oldDirectory))
            {
                if (!IsDirectoryExist(newDirectory))
                {
                    CreateDirectory(newDirectory);
                }

                try
                {
                    var di = new DirectoryInfo(oldDirectory);

                    di.MoveTo(newDirectory);
                }
                catch (Exception ex)
                {
                    AppLog.WriteLog<FileSystemHelper>(ex);

                    throw;
                }
            }
        }

        #endregion

        #region 合并路径

        /// <summary>
        /// 合并路径
        /// </summary>
        /// <param name="path1">路径1</param>
        /// <param name="path2">路径2</param>
        /// <returns></returns>
        public static string CombinePath(string path1, string path2)
        {
            return Path.Combine(path1, path2);
        }

        /// <summary>
        /// 合并路径
        /// </summary>
        /// <param name="paths">路径集合</param>
        /// <returns></returns>
        public static string CombinePath(List<string> paths)
        {
            string tempPath = string.Empty;

            if (paths != null && paths.Count > 0)
            {
                tempPath = paths.Aggregate(tempPath, CombinePath);
            }

            return tempPath;
        }

        #endregion

        #region 内部辅助方法

        private static void WriteBuffer(FileStream fs, long startIndex, long length, byte[] data)
        {
            //写入字节数
            int bytesWrite = 0;

            if (length > data.Length)
            {
            }
            else if (length < data.Length)
            {
                data = ByteConverter.CutByteArray(data, 0, (int) length);
            }

            while (bytesWrite < data.Length)
            {
                fs.Position = startIndex + bytesWrite;

                if (data.Length - bytesWrite < BufferSize)
                {
//当数据已经写完，但是还没到达指定结束边界，则结束
                    fs.Write(data, bytesWrite, data.Length - bytesWrite);
                    bytesWrite += data.Length - bytesWrite;
                }
                else
                {
                    fs.Write(data, bytesWrite, (int) BufferSize);
                    bytesWrite += (int) BufferSize;
                }
            }
        }

        private static void WriteBuffer(MemoryStream fs, long startIndex, long length, byte[] data)
        {
            //写入字节数
            int bytesWrite = 0;

            if (length > data.Length)
            {
            }
            else if (length < data.Length)
            {
                data = ByteConverter.CutByteArray(data, 0, (int) length);
            }

            while (bytesWrite < data.Length)
            {
                fs.Position = startIndex + bytesWrite;

                if (data.Length - bytesWrite < BufferSize)
                {
//当数据已经写完，但是还没到达指定结束边界，则结束
                    fs.Write(data, bytesWrite, data.Length - bytesWrite);
                    bytesWrite += data.Length - bytesWrite;
                }
                else
                {
                    fs.Write(data, bytesWrite, (int) BufferSize);
                    bytesWrite += (int) BufferSize;
                }
            }
        }

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

        #endregion
    }
}