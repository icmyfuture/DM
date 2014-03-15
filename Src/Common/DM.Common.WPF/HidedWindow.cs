using System;
using System.Windows;
using DM.Common.Utility.Log;

namespace DM.Common.WPF
{
    internal class HidedWindow : Window
    {
        private readonly Action _loadAction;
        private readonly string _logName;

        public HidedWindow(Action loadAction, string logName)
        {
            _loadAction = loadAction;
            _logName = logName;
            Loaded += CommonWindowLoaded;
        }

        private void CommonWindowLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_loadAction != null)
                {
                    _loadAction();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Fatal(_logName, string.Empty, ex);
            }
            finally
            {
                Hide();
            }
        }
    }
}