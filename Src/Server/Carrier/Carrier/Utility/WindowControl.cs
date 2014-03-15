using System;
using System.Runtime.InteropServices;

namespace Carrier.Utility
{
    ///<summary>
    ///  窗口控制
    ///</summary>
    internal static class WindowControl
    {
        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("gdi32")]
        private static extern int DeleteObject(IntPtr o);

        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        /// <summary>
        /// 查找窗口
        /// </summary>
        /// <param name="windowName">窗口标题</param>
        /// <returns></returns>
        public static IntPtr FindWindow(this string windowName)
        {
            return FindWindow(null, windowName);
        }


        /// <summary>
        ///   显示
        /// </summary>
        /// <param name = "processPtr"></param>
        public static void Show(this IntPtr processPtr)
        {
            ShowWindow(processPtr, 1);
            DeleteObject(processPtr);
        }

        /// <summary>
        ///   隐藏
        /// </summary>
        /// <param name = "processPtr"></param>
        public static void Hide(this IntPtr processPtr)
        {
            ShowWindow(processPtr, 0);
            DeleteObject(processPtr);
        }
    }
}