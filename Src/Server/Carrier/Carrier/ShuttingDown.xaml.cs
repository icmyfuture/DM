using System;
using System.Threading;
using Carrier.Utility;
using Carrier.ViewModel;

namespace Carrier
{
    /// <summary>
    ///   Interaction logic for ShuttingDown.xaml
    /// </summary>
    public sealed partial class ShuttingDown
    {
        ///<summary>
        ///</summary>
        public ShuttingDown(int processcount)
        {
            int i = 0;
            InitializeComponent();
            _shutting.Maximum = processcount;
            new Thread(() =>
                {
                    try
                    {
                        CarrierTimer.End();
                        MoniterViewModel.
                            Instance.
                            ExecutingCaches.
                            ForEach(p =>
                                {
                                    MoniterViewModel.Instance.FileNames.ForEach(f => f.NeedLive = false);
                                    if (p.Process == null)
                                    {
                                        i++;
                                        return;
                                    }
                                    p.Process.Kill();
                                    i++;
                                    Dispatcher.BeginInvoke(new Action(() => _shutting.Value++), null);
                                    if (processcount != i)
                                    {
                                        return;
                                    }
                                    Thread.Sleep(1000);

                                    Dispatcher.BeginInvoke(new Action(Close));
                                });
                        if (MoniterViewModel.Instance.ExecutingCaches.Count == 0)
                        {
                            Dispatcher.BeginInvoke(new Action(Close));
                        }
                    }
                    catch (Exception ex)
                    {
                        MoniterViewModel.Instance.Info += ex.HandlException();
                        Dispatcher.BeginInvoke(new Action(Close));
                    }
                }).Start();
        }
    }
}