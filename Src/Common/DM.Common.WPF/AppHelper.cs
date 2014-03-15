using System;
using System.Diagnostics;
using System.Windows;
using DM.Common.Utility;
using DM.Common.Utility.Log;

namespace DM.Common.WPF
{
    public class AppHelper
    {
        private const string AppName = @"Application";
        private Action _loadAction;
        private bool _useHidedWindow;

        public static readonly AppHelper Instance = new AppHelper();

        private AppHelper()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;
        }

        /// <summary>
        /// 使得传入的Application为单一实例运行，并附加相关行为
        /// </summary>
        /// <param name="app"></param>
        /// <param name="loadAction">载入时的附加行为</param>
        /// <param name="useHidedWindow">是否使用影藏窗体（用于加载服务供Carrier调用）</param>
        public void Attach(Application app, Action loadAction, bool useHidedWindow = false)
        {
            _loadAction = loadAction;
            _useHidedWindow = useHidedWindow;
            app.Startup += ApplicationStartup;
            app.Exit += ApplicationExit;
        }

        private void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;

            if (ex != null)
                LogHelper.Fatal(AppName, "全局异常捕获到的错误信息！", ex);
            else
                LogHelper.Fatal(AppName, "全局异常捕获到的错误信息！" + e.ExceptionObject);

            //终止所有线程
            Environment.Exit(Environment.ExitCode);
        }

        private void ApplicationStartup(object sender, StartupEventArgs e)
        {
            //得到当前打开的窗口实例  
            var instance = MutexHelper.RunningInstance(Process.GetCurrentProcess());
            if (instance != null)
            {
                try
                {
                    //处理已经存在的窗口实例  
                    MutexHelper.HandleRunningInstance(instance);
                    ApplicationExit(null, null);
                }
                catch (Exception ex)
                {
                    LogHelper.Fatal(AppName, string.Empty, ex);
                    ApplicationExit(null, null);
                }
            }
            else
            {
                if (_useHidedWindow)
                {
                    var main = new HidedWindow(_loadAction, AppName);
                    main.Show();
                }
                else
                {
                    if (_loadAction != null)
                    {
                        _loadAction();
                    }
                }
            }
        }

        private void ApplicationExit(object sender, ExitEventArgs e)
        {
            //终止所有线程
            Environment.Exit(Environment.ExitCode);
        }
    }
}