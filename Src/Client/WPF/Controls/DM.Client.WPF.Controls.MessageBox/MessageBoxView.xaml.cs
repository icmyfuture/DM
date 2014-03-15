using System;
using System.Windows;
using System.Windows.Media.Imaging;
using DM.Common.Utility;

namespace DM.Client.WPF.Controls.MessageBox
{
    /// <summary>
    ///   MessageBoxView.xaml 的交互逻辑
    /// </summary>
    public partial class MessageBoxView
    {
        #region Fields

        /// <summary>
        ///   回调结果
        /// </summary>
        private readonly Action<MessageBoxResult> _excute;

        #endregion

        #region Constructors

        /// <summary>
        /// </summary>
        /// <param name = "content">提示内容</param>
        /// <param name = "caption">提示标题</param>
        /// <param name = "icon">提示图标</param>
        /// <param name = "button">操作按钮</param>
        /// <param name = "okCaption">OK按钮翻译提示</param>
        /// <param name = "cancelCaption">Cancel按钮翻译提示</param>
        /// <param name = "excute">返回结果</param>
        public MessageBoxView(string content, string caption, MessageBoxImage icon, MessageBoxButton button, string okCaption, string cancelCaption, Action<MessageBoxResult> excute)
        {
            if (Application.Current == null)
            {
                return;
            }

            InitializeComponent();
            Topmost = false;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            ShowInTaskbar = false;
            txtContent.Text = content;
            SetMessageBoxIcon(icon);
            SetMessageBoxButton(button);
            IsCollapsedCloseButton = false;
            IsCollapsedMinButton = true;
            _excute = excute;
            if (!string.IsNullOrEmpty(caption))
            {
                Caption = caption;
            }

            btnOK.Content = string.IsNullOrEmpty(okCaption) ? LanguageHelper.GetDictionary("Controls_MessageBox_OK", "OK") : okCaption;
            btnYes.Content = LanguageHelper.GetDictionary("Controls_MessageBox_YES", "Yes");
            btnNo.Content = LanguageHelper.GetDictionary("Controls_MessageBox_NO", "No");
            btnCancel.Content = string.IsNullOrEmpty(cancelCaption) ? LanguageHelper.GetDictionary("Controls_MessageBox_Cancel", "Cancel") : cancelCaption;
            btnOK.Focus();
        }

        #endregion

        #region Methods

        /// <summary>
        ///   XX关闭按钮事件
        /// </summary>
        /// <param name = "result">返回结果</param>
        protected override void OnCloseButton(MessageBoxResult result)
        {
            if (_excute != null)
                _excute(result);
        }

        /// <summary>
        ///   设置弹出对话框图标
        /// </summary>
        /// <param name = "icon">图标</param>
        private void SetMessageBoxIcon(MessageBoxImage icon)
        {
            switch (icon)
            {
                case MessageBoxImage.None:
                    {
                        msgIcon.Visibility = Visibility.Collapsed;
                    }
                    break;
                case MessageBoxImage.Error:
                    {
                        msgIcon.Visibility = Visibility.Visible;
                        msgIcon.Source = new BitmapImage(new Uri("/DM.Client.WPF.Controls.MessageBox;component/Image/Error.png", UriKind.Relative));
                        Caption = LanguageHelper.GetDictionary("Controls_MessageBox_Error", "Error");
                    }
                    break;
                case MessageBoxImage.Warning:
                    {
                        msgIcon.Visibility = Visibility.Visible;
                        msgIcon.Source = new BitmapImage(new Uri("/DM.Client.WPF.Controls.MessageBox;component/Image/Warning.png", UriKind.Relative));
                        Caption = LanguageHelper.GetDictionary("Controls_MessageBox_Warning", "Warning");
                    }
                    break;
                case MessageBoxImage.Information:
                    {
                        msgIcon.Visibility = Visibility.Visible;
                        msgIcon.Source = new BitmapImage(new Uri("/DM.Client.WPF.Controls.MessageBox;component/Image/Information.png", UriKind.Relative));
                        Caption = LanguageHelper.GetDictionary("Controls_MessageBox_Information", "Information");
                    }
                    break;
                default:
                    {
                        msgIcon.Visibility = Visibility.Visible;
                        msgIcon.Source = new BitmapImage(new Uri("/DM.Client.WPF.Controls.MessageBox;component/Image/Information.png", UriKind.Relative));
                    }
                    break;
            }
        }

        /// <summary>
        ///   设置弹出对话框操作按钮
        /// </summary>
        /// <param name = "button"></param>
        private void SetMessageBoxButton(MessageBoxButton button)
        {
            btnOK.Visibility = Visibility.Collapsed;
            btnYes.Visibility = Visibility.Collapsed;
            btnNo.Visibility = Visibility.Collapsed;
            btnCancel.Visibility = Visibility.Collapsed;
            switch (button)
            {
                case MessageBoxButton.OK:
                    btnOK.Visibility = Visibility.Visible;
                    break;
                case MessageBoxButton.OKCancel:
                    btnOK.Visibility = Visibility.Visible;
                    btnCancel.Visibility = Visibility.Visible;
                    break;
                case MessageBoxButton.YesNo:
                    btnYes.Visibility = Visibility.Visible;
                    btnNo.Visibility = Visibility.Visible;
                    break;
                case MessageBoxButton.YesNoCancel:
                    btnYes.Visibility = Visibility.Visible;
                    btnNo.Visibility = Visibility.Visible;
                    btnCancel.Visibility = Visibility.Visible;
                    break;
            }
        }

        #endregion

        #region Events

        //确定事件
        private void BtnOkClick(object sender, RoutedEventArgs e)
        {
            BaseClose(MessageBoxResult.OK);
        }

        //是 事件
        private void BtnYesClick(object sender, RoutedEventArgs e)
        {
            BaseClose(MessageBoxResult.Yes);
        }

        //否 事件
        private void BtnNoClick(object sender, RoutedEventArgs e)
        {
            BaseClose(MessageBoxResult.No);
        }

        //取消事件
        private void BtnCancelClick(object sender, RoutedEventArgs e)
        {
            BaseClose();
        }

        #endregion

        #region Properties

        /// <summary>
        ///   返回窗体内容
        /// </summary>
        public override object MsgContent
        {
            get { return txtContent.Text; }
        }

        #endregion
    }
}