using System;
using System.Windows;
using MvvmLight.Messaging;
using MvvmLight.Threading;

namespace DM.Client.WPF.Sample
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Test_Click(object sender, RoutedEventArgs e)
        {
            ShowDialogMessage("Sample");
        }

        /// <summary>
        /// 显示操作结果对话框
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="caption"></param>
        /// <param name="action"></param>
        /// <param name="msgButton"></param>
        /// <param name="msgBoxImage"></param>
        public static void ShowDialogMessage(string msg, string caption = "操作结果", Action<MessageBoxResult> action = null, MessageBoxButton msgButton = MessageBoxButton.OK, MessageBoxImage msgBoxImage = MessageBoxImage.Information)
        {
            var message = new DialogMessage(msg, action)
            {
                Button = msgButton,
                Caption = caption,
                Icon = msgBoxImage,
                IsTop = true,
                OKCaption = "确定",
                CancelCaption = "取消"
            };
            DispatcherHelper.CheckBeginInvokeOnUI(() => Messenger.Default.Send(message));
        }
    }
}
