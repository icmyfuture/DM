using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;

namespace DM.Web.SL.Common.Utility
{
    /// <summary>
    ///   存储帮助类
    /// </summary>
    public class IsolatedStorageHelper
    {
        #region 单一实例

        /// <summary>
        ///   单一实例
        /// </summary>
        public static IsolatedStorageHelper Instance = new IsolatedStorageHelper();

        #endregion

        #region 复制

        #region 复制目录

        /// <summary>
        ///   复制目录
        /// </summary>
        /// <param name = "oldDirFullPath">源目录</param>
        /// <param name = "newDirPath">目标目录</param>
        public void CopyDirectory(string oldDirFullPath, string newDirPath)
        {
            using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (file.DirectoryExists(oldDirFullPath))
                {
                    string dir = string.Format(@"{0}\{1}", newDirPath, GetItemName(oldDirFullPath));
                    file.CreateDirectory(dir);
                    foreach (string str2 in GetFiles(oldDirFullPath))
                    {
                        CopyFile(string.Format(@"{0}\{1}", oldDirFullPath, str2), dir);
                    }
                    foreach (string str3 in GetSubDirectories(oldDirFullPath))
                    {
                        CopyDirectory(string.Format(@"{0}\{1}", oldDirFullPath, str3), dir);
                    }
                }
            }
        }

        #endregion

        #region 复制文件

        /// <summary>
        ///   复制文件
        /// </summary>
        /// <param name = "oldFileFullPath">源文件</param>
        /// <param name = "newDirPath">目标文件</param>
        public void CopyFile(string oldFileFullPath, string newDirPath)
        {
            CreateFile(string.Format(@"{0}\{1}", newDirPath, GetItemName(oldFileFullPath)), GetFileContent(oldFileFullPath), true);
        }

        #endregion

        #endregion

        #region 创建

        #region 创建目录

        /// <summary>
        ///   创建目录
        /// </summary>
        /// <param name = "directoryPath">目录路径</param>
        public void CreateDirectory(string directoryPath)
        {
            CreateDirectory(directoryPath, false);
        }

        /// <summary>
        ///   创建目录
        /// </summary>
        /// <param name = "directoryPath">目录路径</param>
        /// <param name = "checkedQuota"></param>
        public void CreateDirectory(string directoryPath, bool checkedQuota)
        {
            try
            {
                using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    int num = 0;
                    for (string str = directoryPath; file.FileExists(str); str = string.Format("{0}{1}", directoryPath, num))
                    {
                        num++;
                    }

                    if (!file.DirectoryExists(directoryPath))
                    {
                        file.CreateDirectory(directoryPath);
                    }
                }
            }
            catch (IsolatedStorageException exception)
            {
                if (!(checkedQuota || exception.Message != "There is not enough free space to perform the operation."))
                {
                    CreateDirectory(directoryPath, true);
                }
            }
        }

        #endregion

        #region 创建文件

        /// <summary>
        ///   创建文件
        /// </summary>
        /// <param name = "fullFileName">文件路径</param>
        /// <param name = "content">内容</param>
        /// <param name = "rewriteFile">是否重写</param>
        public void CreateFile(string fullFileName, string content, bool rewriteFile)
        {
            CreateFile(fullFileName.Substring(0, fullFileName.LastIndexOf(".", System.StringComparison.Ordinal)), fullFileName.Substring(fullFileName.LastIndexOf(".", System.StringComparison.Ordinal) + 1, (fullFileName.Length - fullFileName.LastIndexOf(".", System.StringComparison.Ordinal)) - 1), content, rewriteFile);
        }

        /// <summary>
        ///   创建文件
        /// </summary>
        /// <param name = "fileName">文件名</param>
        /// <param name = "fileExtension">扩展名</param>
        /// <param name = "content">内容</param>
        /// <param name = "rewriteFile">是否重写</param>
        public void CreateFile(string fileName, string fileExtension, string content, bool rewriteFile)
        {
            CreateFile(fileName, fileExtension, content, rewriteFile, false);
        }

        /// <summary>
        ///   创建文件
        /// </summary>
        /// <param name = "fileName">文件名</param>
        /// <param name = "fileExtension">扩展名</param>
        /// <param name = "content">内容</param>
        /// <param name = "rewriteFile">是否重写</param>
        /// <param name = "checkedQuota"></param>
        public void CreateFile(string fileName, string fileExtension, string content, bool rewriteFile, bool checkedQuota)
        {
            try
            {
                using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    int num = 0;
                    string path = string.Format("{0}.{1}", fileName, fileExtension);
                    if (!rewriteFile)
                    {
                        while (file.FileExists(path))
                        {
                            num++;
                            path = string.Format("{0}{1}.{2}", fileName, num, fileExtension);
                        }
                    }
                    using (IsolatedStorageFileStream stream = file.CreateFile(path))
                    {
                        using (var writer = new StreamWriter(stream))
                        {
                            writer.Write(content);
                        }
                    }
                }
            }
            catch (IsolatedStorageException exception)
            {
                if (!(checkedQuota || exception.Message != "There is not enough free space to perform the operation."))
                {
                    CreateFile(fileName, fileExtension, content, rewriteFile, true);
                }
            }
        }

        #endregion

        #endregion

        #region 删除

        #region 删除目录

        /// <summary>
        ///   删除目录
        /// </summary>
        /// <param name = "directoryName"></param>
        public void DeleteDirectory(string directoryName)
        {
            using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (file.DirectoryExists(directoryName))
                {
                    file.DeleteDirectory(directoryName);
                }
            }
        }

        #endregion

        #region 删除文件

        /// <summary>
        ///   删除文件
        /// </summary>
        /// <param name = "fileName">文件路径</param>
        public void DeleteFile(string fileName)
        {
            try
            {
                using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (file.FileExists(fileName))
                    {
                        file.DeleteFile(fileName);
                    }
                }
            }
            catch (IsolatedStorageException)
            {}
        }

        #endregion

        #endregion

        #region 获取

        /// <summary>
        ///   获取文件内容
        /// </summary>
        /// <param name = "fileName">文件路径</param>
        /// <returns></returns>
        public string GetFileContent(string fileName)
        {
            try
            {
                using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (file.FileExists(fileName))
                    {
                        using (IsolatedStorageFileStream stream = file.OpenFile(fileName, FileMode.Open))
                        {
                            using (var reader = new StreamReader(stream))
                            {
                                return reader.ReadToEnd();
                            }
                        }
                    }
                    return null;
                }
            }
            catch (IsolatedStorageException)
            {}
            return string.Empty;
        }

        /// <summary>
        ///   获取文件列表
        /// </summary>
        /// <param name = "directory">目录</param>
        /// <returns>文件列表</returns>
        public string[] GetFiles(string directory)
        {
            string[] fileNames = null;
            try
            {
                if (!directory.EndsWith(@"\"))
                {
                    directory = string.Format(@"{0}\", directory);
                }
                using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    fileNames = file.GetFileNames(string.Format("{0}*", directory));
                }
            }
            catch (IsolatedStorageException)
            {}
            return fileNames;
        }

        /// <summary>
        ///   获取路径的目录
        /// </summary>
        /// <param name = "fullPath"></param>
        /// <returns></returns>
        public string GetItemDir(string fullPath)
        {
            return fullPath.Substring(0, fullPath.LastIndexOf(@"\", System.StringComparison.Ordinal));
        }

        /// <summary>
        ///   获取项名称
        /// </summary>
        /// <param name = "fullPath"></param>
        /// <returns></returns>
        public string GetItemName(string fullPath)
        {
            return fullPath.Substring(fullPath.LastIndexOf(@"\", System.StringComparison.Ordinal) + 1, (fullPath.Length - fullPath.LastIndexOf(@"\", System.StringComparison.Ordinal)) - 1);
        }

        /// <summary>
        ///   获取跟目录子目录
        /// </summary>
        /// <returns></returns>
        public string[] GetRootDirectories()
        {
            string[] strArray = null;
            try
            {
                using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    strArray = (from d in file.GetDirectoryNames()
                                where d != "System"
                                select d).ToArray();
                }
            }
            catch (IsolatedStorageException)
            {}
            return strArray;
        }

        /// <summary>
        ///   获取跟目录文件
        /// </summary>
        /// <returns></returns>
        public string[] GetRootFiles()
        {
            string[] fileNames = null;
            try
            {
                using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    fileNames = file.GetFileNames();
                }
            }
            catch (IsolatedStorageException)
            {}
            return fileNames;
        }

        /// <summary>
        ///   获取子目录
        /// </summary>
        /// <param name = "directory">目录路径</param>
        /// <returns>子目录</returns>
        public string[] GetSubDirectories(string directory)
        {
            string[] directoryNames = null;
            try
            {
                if (!directory.EndsWith(@"\"))
                {
                    directory = string.Format(@"{0}\", directory);
                }
                using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    directoryNames = file.GetDirectoryNames(string.Format("{0}*", directory));
                }
            }
            catch (IsolatedStorageException)
            {}
            return directoryNames;
        }

        #endregion

        #region 初始化

        /// <summary>
        ///   初始化
        /// </summary>
        /// <param name = "size">大小</param>
        /// <returns></returns>
        public IsolatedStorageHelper(long size)
        {
            using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication())
            {
                long num = 0x100000L * size;
                if (file.AvailableFreeSpace < num)
                {
                    file.IncreaseQuotaTo(file.Quota + num);
                }
            }
        }

        /// <summary>
        ///   初始化
        /// </summary>
        public IsolatedStorageHelper()
        {
            try
            {
                using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (!file.DirectoryExists("System"))
                    {
                        file.CreateDirectory("System");
                    }
                }
            }
            catch (IsolatedStorageException)
            {}
        }

        #endregion
    }
}