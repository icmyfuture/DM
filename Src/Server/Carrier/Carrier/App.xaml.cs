using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using Carrier.Entities;
using Carrier.Utility;
using Carrier.ViewModel;
using DM.Common.Utility.Log;

namespace Carrier
{
    /// <summary>
    ///   Interaction logic for App.xaml
    /// </summary>
    public sealed partial class App
    {
        static App()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;

            if (ex != null)
                LogHelper.Debug("Carrier", "全局异常捕获到的错误信息！", ex);
            else
                LogHelper.Debug("Carrier", "全局异常捕获到的错误信息！" + e.ExceptionObject);

            //终止所有线程
            Environment.Exit(Environment.ExitCode);
        }

        ///<summary>
        ///  构造
        ///</summary>
        public App()
        {
            string pname = Process.GetCurrentProcess().ProcessName;
            int pcount = Process.GetProcesses().Count(process => process.ProcessName.Equals(pname));
            if (pcount > 1)
            {
                MessageBox.Show("already running.");
                Current.Shutdown();
                return;
            }
            InitWindow();
        }

        private void InitWindow()
        {
            new MainWindow
                {
                    DataContext = MoniterViewModel.Instance,
                }.Show();

            CarrierTimer.
                SetMethod(() =>
                    {
                        CarrierTimer.Stop();
                        //FindErrorWindow();
                        foreach (ExecuteFile executeFile in MoniterViewModel.Instance.FileNames)
                        {
                            Dispatcher.BeginInvoke(Watcher(), executeFile);
                        }
                        CarrierTimer.Start();
                    });
        }

        /// <summary>
        ///   进程监视委托
        /// </summary>
        /// <returns></returns>
        private static Action<ExecuteFile> Watcher()
        {
            return file =>
                {
                    try
                    {
                        if (file.SysProcess != null)
                        {
                            file.SysProcess.Refresh();
                            Moniter.OverMemory += (s, e) =>
                                {
                                    MoniterViewModel.Instance.StopApplication(file.SysProcess);
                                    Thread.Sleep(2000);
                                    MoniterViewModel.Instance.StartApplication(file.ID);
                                };
                            Moniter.AddWatch(file.SysProcess);
                            if (file.Process != null)
                            {
                                file.Process.Memory = file.SysProcess.WorkingSet64;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MoniterViewModel.Instance.Info += ex.HandlException();
                    }
                };
        }

        /// <summary>
        ///   启动
        /// </summary>
        /// <param name = "e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            CarrierTimer.Start();
            base.OnStartup(e);
        }
    }
}