using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Carrier.Utility
{
    internal static class Notifier
    {
        private static readonly NotifyIcon TrayIcon;

        static Notifier()
        {
            string applocation = Assembly.GetExecutingAssembly().Location;
            var processFile = new FileInfo(Process.GetCurrentProcess().MainModule.FileName);
            string processFileName = processFile.Name.Replace(processFile.Extension, "");
            TrayIcon = new NotifyIcon
                {
                    BalloonTipTitle = processFileName,
                    BalloonTipIcon = ToolTipIcon.Info,
                    Text = processFileName,
                    Visible = true,
                    Icon = Icon.ExtractAssociatedIcon(applocation),
                };
        }

        public static void ShutDown()
        {
            TrayIcon.Dispose();
        }

        public static void TrayClick(Action clickaction)
        {
            TrayIcon.Click += ((sender, e) => clickaction());
        }

        public static void ShowTip(string tipmessage)
        {
            TrayIcon.BalloonTipText = tipmessage;
            TrayIcon.ShowBalloonTip(1000);
        }
    }
}