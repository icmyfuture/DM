using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using Carrier.Entities;
using DM.Common.Utility.Log;

namespace Carrier.Utility
{
    internal static class Extension
    {
        private const string InterceptorFileName = "Carrier.Interceptor.exe";

        private static readonly string InterceptorFileFullPath = Path.Combine(Environment.CurrentDirectory, InterceptorFileName);

        ///<summary>
        ///  无返回值的迭代器遍历方法
        ///</summary>
        ///<param name = "source">集合</param>
        ///<param name = "action">对集合内对象做的操作</param>
        ///<typeparam name = "T">集合内对象的类型</typeparam>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T item in source)
            {
                action(item);
            }
        }

        /// <summary>
        ///   转换路径为应用实体
        /// </summary>
        /// <param name = "path">path</param>
        /// <param name = "app">应用</param>
        /// <returns></returns>
        public static ExecuteFile ToExe(this string path, ServiceUrl app)
        {
            if (File.Exists(path))
            {
                Icon icon = Icon.ExtractAssociatedIcon(path);
                string extension = Path.GetExtension(path);
                string filename = Path.GetFileNameWithoutExtension(path);
                string newAppPath;

                if (extension != null && extension.Equals(".dll"))
                {
                    //处理注入代理拷贝，分离依赖环境
                    newAppPath = CopyInterceptorExe(Path.GetDirectoryName(path));
                    icon = Icon.ExtractAssociatedIcon(newAppPath); //Interceptor;
                }
                else
                {
                    newAppPath = path;
                }

                if (icon != null)
                    return new ExecuteFile
                        {
                            FilePath = path,
                            FileName = filename,
                            Icon = GetIconSource(icon.ToBitmap()),
                            NeedLive = true,
                            DeadCount = 0,
                            Port = app.Port,
                            IsAlive = true,
                            ID = app.ID,
                            RunFileFullName = newAppPath,
                            StartIndex = app.StartIndex
                        };
            }
            LogHelper.Writelog(LogType.Error, "Carrier", string.Format("【carrier error】 -> {0}", "application path error."));
            return null;
        }

        private static BitmapSource GetIconSource(Bitmap ico)
        {
            IntPtr intPtr = ico.GetHbitmap();
            BitmapSource bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(intPtr, IntPtr.Zero, Int32Rect.Empty,
                                                                              BitmapSizeOptions.FromEmptyOptions());
            DeleteObject(intPtr);
            return bitmapSource;
        }

        /// <summary>
        /// copy注入代理
        /// </summary>
        /// <param name="toDirectory">DLL所在目录</param>
        /// <returns></returns>
        private static string CopyInterceptorExe(string toDirectory)
        {
            //采用DLL所在目录名作为进程别名，避免重复
            string appName = string.Format("{0}.{1}", Path.GetFileNameWithoutExtension(toDirectory),
                                           InterceptorFileName);
            string dirBase = Path.GetDirectoryName(InterceptorFileFullPath);

            if (Directory.Exists(toDirectory))
            {
                try
                {
                    //拷贝INTERCEPTOR需要的库和配置文件
                    File.Copy(dirBase + "\\DM.Server.Common.Config.dll", toDirectory + "\\DM.Server.Common.Config.dll",
                              true);
                    File.Copy(dirBase + "\\DM.Server.Common.MessageGateway.dll",
                              toDirectory + "\\DM.Server.Common.MessageGateway.dll", true);
                    File.Copy(dirBase + "\\DM.Server.Common.Utility.dll",
                              toDirectory + "\\DM.Server.Common.Utility.dll", true);
                    File.Copy(dirBase + "\\Carrier.Engine.dll", toDirectory + "\\Carrier.Engine.dll", true);
                    File.Copy(dirBase + "\\Carrier.Interceptor.exe.config", toDirectory + "\\" + appName + ".config",
                              true);
                    File.Copy(dirBase + "\\Carrier.Linker.DLL", toDirectory + "\\Carrier.Linker.DLL", true);
                    File.Copy(InterceptorFileFullPath, toDirectory + "\\" + appName, true);
                }
                catch (Exception)
                {
                    LogHelper.Writelog(LogType.Error, "Carrier", string.Format("【carrier error】 -> {0}", "CopyInterceptorExe error."));
                }
            }

            return toDirectory + "\\" + appName;
        }

        [DllImport("gdi32")]
        private static extern int DeleteObject(IntPtr o);
    }
}