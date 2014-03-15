using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using DM.Common.Utility.Log;

namespace DM.Common.Utility
{
    /// <summary>
    /// 单进程实例辅助类
    /// </summary>
    public class MutexHelper
    {
        #region 处理已经存在的窗口实例
        /// <summary>
        /// 处理已经存在的窗口实例
        /// </summary>
        /// <returns></returns>
        public static Process RunningInstance(Process current)
        {
            Process[] processes = Process.GetProcessesByName(current.ProcessName);

            //遍历具有相同名字的窗体实例
            foreach (Process process in processes)
            {
                //排除当前实例
                if (process.Id != current.Id)
                {
                    LogHelper.Fatal(typeof(MutexHelper).Name, string.Format("发现同一进程名字的不同进程实例,当前进程位置{0},已有进程位置{1}", process.MainModule.FileName, current.MainModule.FileName));

                    //肯定来自同一个EXE  
                    if (process.MainModule.FileName == current.MainModule.FileName)
                    {
                        //返回另外一个相同实例  
                        return process;
                    }
                }
            }

            //没有找到返回空
            return null;
        }

        public static void HandleRunningInstance(Process instance)
        {
            /*0——隐藏窗口，活动状态给令一个窗口。

            1或9——用原来的大小和位置显示一个窗口，同时令其进入活动状态。

            2——最小化窗口，并将其激活。

            3——最大化窗口，并将其激活。

            4——用最近的大小和位置显示一个窗口，同时不改变活动窗口。

            5——用当前的大小和位置显示一个窗口，同时令其进入活动状态。

            6——最小化窗口，活动状态给令一个窗口。

            7——最小化一个窗口，同时不改变活动窗口。

            8——用当前的大小和位置显示一个窗口，不改变活动窗口。
            */

            //让窗口在最大化与最小化间切换   
            ShowWindowAsync(instance.MainWindowHandle, 1);

            //让发现的实例在窗口最前端显示
            SetForegroundWindow(instance.MainWindowHandle);
        }
        #endregion

        #region 引用WINDOW底层API
        [DllImport("User32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);
        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        #endregion

        #region 异常处理
        public static void Application_Error(object obj, ThreadExceptionEventArgs args)
        {
            //记录下日志
            LogHelper.Fatal(typeof(MutexHelper).Name, string.Empty, args.Exception);
        }
        #endregion
    }
}