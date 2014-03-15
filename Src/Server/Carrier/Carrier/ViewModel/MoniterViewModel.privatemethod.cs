using System;
using System.Diagnostics;
using System.Linq;
using Carrier.Entities;
using Carrier.Utility;
using DM.Common.Utility.Log;

namespace Carrier.ViewModel
{
    public sealed partial class MoniterViewModel
    {
        /// <summary>
        ///   删除一个应用
        /// </summary>
        private void DelApplication()
        {
        }

        /// <summary>
        ///   更新缓存
        /// </summary>
        /// <param name = "process"></param>
        /// <param name = "appname"></param>
        /// <param name = "appid"></param>
        private void UpdateCache(Process process, string appname, string appid)
        {
            RomoveCache(appid);

            ExecutingCaches.Add(new ExecutingCache
                {
                    AppName = appname,
                    Process = process,
                    Handle = process.MainWindowHandle,
                    ID = appid
                });
        }

        /// <summary>
        ///   更新缓存
        /// </summary>
        /// <param name = "appid"></param>
        private void RomoveCache(string appid)
        {
            if (ExecutingCaches.Any(c => c.ID.Equals(appid)))
            {
                ExecutingCache cache = ExecutingCaches.FirstOrDefault(c => c.ID.Equals(appid));
                ExecutingCaches.Remove(cache);
            }
        }

        /// <summary>
        ///   进程退出
        /// </summary>
        /// <param name = "sender"></param>
        /// <param name = "e"></param>
        private void ProcessExited(object sender, EventArgs e)
        {
            try
            {
                var process = sender as Process;
                if (process == null)
                {
                    return;
                }

                if (ExecutingCaches.Any(p => p.Process != null && p.Process.Id == process.Id))
                {
                    ExecutingCache app =
                        ExecutingCaches.FirstOrDefault(p => p.Process != null && p.Process.Id == process.Id);

                    ExecuteFile executefile =
                        _filenames.FirstOrDefault(u => !string.IsNullOrEmpty(u.ID) && u.ID.Equals(app.ID));

                    if (executefile != null)
                    {
                        executefile.DeadCount++;
                        Notifier.ShowTip(string.Format("{0} down,it's the {1} times, \r\nnow restart",
                                                       executefile.FileName, executefile.DeadCount));
                        executefile.IsAlive = true;
                        executefile.Url = string.Empty;
                        executefile.Process = null;
                        ExecutingCaches.Remove(app);

                        if (executefile.NeedLive)
                        {
                            StartApplication(executefile.ID);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Info += exception.HandlException();
            }
        }

        /// <summary>
        ///   启动所有应用
        /// </summary>
        private void StartApplications()
        {
            if (RestartAllEnable)
            {
                //去掉事件绑定
                _filenames.ForEach(p => UnSubscibeProcessExitEvent(p.SysProcess));

                //重启服务
                Action launch = () =>
                    {
                        RestartAllEnable = false;

                        //启动所有服务
                        _filenames.OrderBy(p => p.StartIndex).ForEach(p => StartApplication(p.ID));

                        RestartAllEnable = true;
                    };

                launch.BeginInvoke(null, null);
            }
        }

        private void UnSubscibeProcessExitEvent(Process pro)
        {
            try
            {
                if (pro != null && pro.Id > 0)
                {
                    pro.Exited -= ProcessExited;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Writelog(LogType.Debug, "Carrier", string.Format("【carrier error】 -> {0}", ex));
            }
        }

        /// <summary>
        ///   打开设置项
        /// </summary>
        private void OpenOptions()
        {
            new Options
                {
                    DataContext = new Configuration()
                }.ShowDialog();
        }
    }
}