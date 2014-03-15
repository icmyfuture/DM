using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Carrier.Entities;
using Carrier.Repository;
using Carrier.Utility;
using Carrier.ViewModel;

namespace Carrier
{
    /// <summary>
    ///   Interaction logic for MainWindow.xaml
    /// </summary>
    public sealed partial class MainWindow
    {
        private readonly IProcessData _dataProcessor;
        private bool _ishide;
        private ProcessWindowStyle _state = ProcessWindowStyle.Hidden;

        ///<summary>
        ///</summary>
        ///<param name = "processdata"></param>
        public MainWindow(IProcessData processdata)
        {
            _dataProcessor = processdata;
            var processFile = new FileInfo(Process.GetCurrentProcess().MainModule.FileName);
            Title = processFile.Name.Replace(processFile.Extension, "");
        }

        ///<summary>
        ///</summary>
        public MainWindow()
            : this(LocalStorage.ProcessData)
        {
            InitializeComponent();
            Notifier.ShowTip("Launched at " + DateTime.Now);
            Notifier.TrayClick(() =>
                {
                    if (_ishide)
                    {
                        Hide();
                        _ishide = !_ishide;
                    }
                    else
                    {
                        Show();
                        _ishide = !_ishide;
                    }
                });
        }

        private void RunApplication(object sender, RoutedEventArgs e)
        {
            try
            {
                var btn = sender as Button;
                if (btn == null)
                {
                    return;
                }
                var executer = btn.Tag as ExecuteFile;
                if (executer != null && executer.IsAlive)
                {
                    MoniterViewModel.Instance.StartApplication(executer.ID);
                }
            }
            catch (Exception exception)
            {
                MoniterViewModel.Instance.Info += exception.HandlException();
            }
        }

        private void ShowWindow(object sender, RoutedEventArgs e)
        {
            try
            {
                var btn = sender as Button;
                if (btn == null)
                {
                    return;
                }
                var process = (btn.Tag) as Process;
                if (process == null)
                {
                    MoniterViewModel.Instance.Info += "no process in task. \r\n";
                    return;
                }
                ExecutingCache pcache =
                    MoniterViewModel.Instance.ExecutingCaches.FirstOrDefault(p => p.Process.Id == process.Id);
                if (_state == ProcessWindowStyle.Normal)
                {
                    pcache.Handle.Hide();
                    _state = ProcessWindowStyle.Hidden;
                }
                else
                {
                    pcache.Handle.Show();
                    _state = ProcessWindowStyle.Normal;
                }
            }
            catch (Exception exception)
            {
                MoniterViewModel.Instance.Info = exception.HandlException();
            }
        }

        private void ShutDown(object sender, RoutedEventArgs e)
        {
            try
            {
                var btn = sender as Button;
                if (btn == null)
                {
                    return;
                }
                var process = (btn.Tag) as Process;
                if (process == null)
                {
                    MoniterViewModel.Instance.Info += "no process in task. \r\n";
                    return;
                }
                MoniterViewModel.Instance.StopApplication(process);
            }
            catch (Exception exception)
            {
                MoniterViewModel.Instance.Info += exception.HandlException();
            }
        }

        private void HyperlinkClick(object sender, RoutedEventArgs e)
        {
            var link = sender as Hyperlink;
            if (link != null)
            {
                Process.Start(link.NavigateUri.AbsoluteUri);
            }
        }
    }
}