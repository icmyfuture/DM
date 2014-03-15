using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Input;
using Carrier.Entities;
using Carrier.Repository;
using Carrier.Utility;
using DM.Common.Extensions;
using Microsoft.VisualBasic.Devices;

namespace Carrier.ViewModel
{
    /// <summary>
    ///   监控视图模型
    /// </summary>
    public sealed partial class MoniterViewModel : ViewModelBase
    {
        //private static readonly string Interceptor = Path.Combine(Environment.CurrentDirectory, "Carrier.Interceptor.exe");

        ///<summary>
        ///  视图单件实例
        ///</summary>
        public static readonly MoniterViewModel Instance = new MoniterViewModel();

        private readonly IProcessData _dataProcessor;
        private readonly IUrlData _urlData;
        private bool _restartAllEnable = true;

        private ICommand _delApp;
        private ObservableCollection<ExecuteFile> _filenames;
        private string _info;
        private ICommand _option;
        private ICommand _startApps;

        //避免用户重复点击按钮导致服务启动失败

        private MoniterViewModel(IUrlData urldata, IProcessData processdata)
        {
            _urlData = urldata;
            _dataProcessor = processdata;
        }

        private MoniterViewModel()
            : this(LocalStorage.ProcessData, LocalStorage.ProcessData)
        {
            _filenames = _dataProcessor.GetApplications();
            Process[] processes = Process.GetProcesses();
            processes.ForEach(p => _filenames.ForEach(f =>
                {
                    if (f == null)
                    {
                        return;
                    }
                    if (f.FileName.StartsWith(p.ProcessName))
                    {
                        f.Process = new AppProcess
                            {
                                Pid = p.Id,
                                Memory = p.WorkingSet64
                            };
                        f.SysProcess = p;
                    }
                }));
            ExecutingCaches = new List<ExecutingCache>();

            var autostart = DM.Common.Config.Configuration.CommonSettings("AutoStart");
            if (autostart.Equals("1"))
            {
                StartApplications();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private string Interceptor { get; set; }

        private bool RestartAllEnable
        {
            get { return _restartAllEnable; }
            set { _restartAllEnable = value; }
        }

        ///<summary>
        ///  执行信息
        ///</summary>
        public string Info
        {
            get { return _info; }
            set
            {
                _info = value;
                OnPropertyChanged("Info");
            }
        }

        ///<summary>
        ///  删除一个应用
        ///</summary>
        public ICommand DelApp
        {
            get { return _delApp ?? (_delApp = new RelayCommand(act => DelApplication(), allow => true)); }
        }

        ///<summary>
        ///  启动所有应用
        ///</summary>
        public ICommand StartApps
        {
            get { return _startApps ?? (_startApps = new RelayCommand(obj => StartApplications(), allow => true)); }
        }

        /// <summary>
        ///   打开设置项
        /// </summary>
        public ICommand Option
        {
            get { return _option ?? (_option = new RelayCommand(act => OpenOptions(), allow => true)); }
        }

        ///<summary>
        ///  运行时状态集合
        ///</summary>
        public IList<ExecutingCache> ExecutingCaches { get; set; }

        ///<summary>
        ///  文件列表
        ///</summary>
        public ObservableCollection<ExecuteFile> FileNames
        {
            get { return _filenames; }
            set
            {
                _filenames = value;
                OnPropertyChanged("FileNames");
            }
        }

        ///<summary>
        ///  启动一个应用
        ///</summary>
        ///<param name = "id">程序标识</param>
        public void StartApplication(string id)
        {
            try
            {
                ExecuteFile app = _filenames.FirstOrDefault(a => !string.IsNullOrEmpty(a.ID) && a.ID.Equals(id));
                Debug.Assert(app != null, "app != null");
                Interceptor = Path.GetDirectoryName(app.FilePath) + "\\Carrier.Interceptor.exe";
                string extension = Path.GetExtension(app.FilePath);

                #region 这段代码会导致 关闭程序的时候有重启在自动启动 写如配置文件中的列表为空

                //_urlData.SaveServiceUrl(new ServiceUrl
                //{
                //    FilePath = app.FilePath,
                //    Port = app.Port,
                //    Path = "Service",
                //    ID = app.ID
                //}); 

                #endregion

                ServiceUrl urlentity = _urlData.GetServiceUrl(app.ID);
                var firstOrDefault = Dns.GetHostEntry(new Computer().Name).AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
                if (firstOrDefault != null)
                {
                    string computername =
                        firstOrDefault.ToString();
                    string url = string.Format("http://{0}:{1}/{2}", computername, urlentity.Port, urlentity.Path);
                    string arg;
                    if (extension != null && extension.Equals(".dll"))
                    {
                        arg = app.FilePath.ToCompressedStr() + " " + url.ToCompressedStr();
                    }
                    else
                    {
                        arg = url.ToCompressedStr();
                    }

                    //检测目标进程是否存在，如果存在则先杀死
                    if (!TryToKillExistProcess(Path.GetFileNameWithoutExtension(app.RunFileFullName)))
                    {
                        return;
                    }

                    var process = new Process
                        {
                            EnableRaisingEvents = true,
                            StartInfo =
                                {
                                    CreateNoWindow = false,
                                    FileName = app.RunFileFullName,
                                    Arguments = arg,
                                    ErrorDialog = true,
                                    UseShellExecute = true,
                                    WindowStyle = ProcessWindowStyle.Minimized
                                }
                        };

                    string appname = Path.GetFileNameWithoutExtension(app.FilePath);
                    process.Exited += ProcessExited;
                    Process[] running = Process.GetProcessesByName(appname);
                    Info += string.Format("searching process -> {0},find running -> {1} \r\n", appname, running.Length);
                    process.Start();
                    if (!process.WaitForInputIdle())
                    {
                        return;
                    }

                    while (process.MainWindowHandle.ToInt32() == 0)
                    {
                        process.Refresh();
                    }

                    Notifier.ShowTip(string.Format("{0} on,\r\nthe url is {1}", app.FileName, url));
                    app.IsAlive = false;
                    app.Url = url;
                    app.StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    app.Process = new AppProcess
                        {
                            Pid = process.Id,
                            Memory = process.WorkingSet64
                        };
                    app.SysProcess = process;
                    UpdateCache(process, appname, app.ID);
                    Thread.Sleep(100);
                    process.MainWindowHandle.Hide();
                }
            }
            catch (Exception e)
            {
                Info += e.HandlException();
            }
        }

        /// <summary>
        /// 检测目标进程是否存在，如果存在则先杀死
        /// </summary>
        /// <param name="processName"></param>
        public bool TryToKillExistProcess(string processName)
        {
            try
            {
                while (true)
                {
//不允许重复进程存在
                    Process[] pros = Process.GetProcessesByName(processName);

                    if (pros.Length > 0)
                    {
                        foreach (Process t in pros)
                        {
                            t.Kill();
                        }
                    }
                    else
                    {
                        return true;
                    }

                    Thread.Sleep(500);
                }
            }
            catch (Exception ex)
            {
                Info += ex.HandlException();
                return false;
            }
        }

        /// <summary>
        ///   停止一个应用
        /// </summary>
        /// <param name = "process"></param>
        public void StopApplication(Process process)
        {
            if (process == null || process.HasExited)
            {
                return;
            }
            ExecutingCache pcache = ExecutingCaches.FirstOrDefault(p => p.Process.Id == process.Id);

            process.Exited -= ProcessExited;

            process.Kill();
            process.Close();
            ExecuteFile fcache = FileNames.FirstOrDefault(f => !string.IsNullOrEmpty(f.ID) && f.ID.Equals(pcache.ID));
            if (fcache != null)
            {
                fcache.IsAlive = true;
                fcache.Url = string.Empty;
                fcache.Process = null;
                fcache.SysProcess = null;
            }
            ExecutingCaches.Remove(pcache);
            if (fcache != null)
            {
                Info += fcache.FileName + " down.\r\n";
                Notifier.ShowTip(fcache.FileName + " down");
            }
        }

        /// <summary>
        ///   释放方法
        /// </summary>
        protected override void OnDispose()
        {
            Dispose();
        }
    }
}