using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Interop;
using DM.Common.Extensions;

namespace DM.Client.WPF.Controls.MessageBox
{
    /// <summary>
    ///   父对话框
    /// </summary>
    public class BaseMessageBox : Window, IMessageBox
    {
        #region Fields

        protected System.Windows.Controls.Button CloseButton;

        protected System.Windows.Controls.Button MinButton;

        private TextBlock _textTitle;
        private bool _isMined;

        private readonly WindowPosition _position = new WindowPosition();


        /// <summary>
        ///   返回结果
        /// </summary>
        private MessageBoxResult _result = MessageBoxResult.Cancel;

        #endregion

        #region Constructors

        /// <summary>
        ///   构造函数
        /// </summary>
        protected BaseMessageBox()
        {
            InitializeStyle();
            IsCollapsedMinButton = true;

            if (!(System.Windows.Application.Current.MainWindow is BaseMessageBox))
            {
                Owner = System.Windows.Application.Current.MainWindow;
            }

            Loaded += delegate
                          {
                              InitializeEvent();
                              SetCustomControl();
                              if (IsCollapsedCloseButton)
                                  CloseButton.Visibility = Visibility.Collapsed;
                              if (IsCollapsedMinButton)
                                  MinButton.Visibility = Visibility.Collapsed;
                              //将窗体推向最顶层 并处于激活状态
                              SetForegroundWindow(WindPtr);
                          };

            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Closed += BaseMessageBoxClosed;
            KeyDown += BaseMessageBoxKeyDown;
        }

        #endregion

        #region Methods

        /// <summary>
        ///   初始化样式
        /// </summary>
        private void InitializeStyle()
        {
            if (System.Windows.Application.Current == null)
            {
                return;
            }
            if (System.Windows.Application.Current.Resources["BaseMessageBoxWindowStyle"] == null)
            {
                System.Windows.Application.Current.Resources.MergedDictionaries.Add(System.Windows.Application.LoadComponent(new Uri("/DM.Client.WPF.Controls.MessageBox;component/Style/BaseMessageBoxStyle.xaml", UriKind.Relative)) as ResourceDictionary);
            }
            Style = (Style)System.Windows.Application.Current.Resources["BaseMessageBoxWindowStyle"];
        }

        /// <summary>
        ///   初始化事件
        /// </summary>
        private void InitializeEvent()
        {
            var baseWindowTemplate = (ControlTemplate)System.Windows.Application.Current.Resources["BaseWindowControlTemplate"];

            _textTitle = (TextBlock)baseWindowTemplate.FindName("tblTitle", this);
            if (_textTitle != null)
                SetWindowTitle();

            CloseButton = (System.Windows.Controls.Button)baseWindowTemplate.FindName("btnClose", this);
            if (CloseButton != null)
                CloseButton.Click += CloseDilalog;

            MinButton = (System.Windows.Controls.Button)baseWindowTemplate.FindName("btnMin", this);
            if (MinButton != null)
                MinButton.Click += (s, e) => MinWindow();

            var borderTitle = (Border)baseWindowTemplate.FindName("borderTitle", this);
            borderTitle.MouseLeftButtonDown += (s, e) => RestoreWindow();
            borderTitle.MouseMove += delegate(object sender, System.Windows.Input.MouseEventArgs e)
                                         {
                                             if (e.LeftButton == MouseButtonState.Pressed)
                                             {
                                                 DragMove();
                                                 _position.Left = Left;
                                                 _position.Top = Top;
                                             }
                                         };
        }

        protected virtual void CloseDilalog(object sender, RoutedEventArgs args)
        {
            Close();
        }

        private void RestoreWindow()
        {
            if (!_isMined)
                return;
            MoveWindow(WindPtr, (int)_position.Left, (int)_position.Top, _position.Width, _position.Height, true);
            _isMined = false;
            MinButton.Visibility = Visibility.Visible;
        }

        /// <summary>
        ///   关闭窗体
        /// </summary>
        /// <param name = "result">结果值</param>
        protected void BaseClose(MessageBoxResult result = MessageBoxResult.Cancel)
        {
            _result = result;
            CloseButton.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent));
        }

        /// <summary>
        ///   设置自定义弹出控件的属性
        /// </summary>
        private void SetCustomControl()
        {
            var contentPresenter = FindVisualChild<ContentPresenter>(this);
            if (contentPresenter != null)
            {
                if (contentPresenter.Content is ICustomMessageBox)
                {
                    var iCustomMessageBox = contentPresenter.Content as ICustomMessageBox;
                    iCustomMessageBox.Close += (s, e) => BaseClose();
                }
            }
        }

        /// <summary>
        ///   XX关闭按钮事件点击之后继承的子控件方法有需要的就调用
        /// </summary>
        /// <param name = "result">返回结果</param>
        protected virtual void OnCloseButton(MessageBoxResult result)
        {
        }

        /// <summary>
        ///   关闭
        /// </summary>
        protected virtual void KeyEsc()
        {
            //BaseClose();
        }

        /// <summary>
        /// 通过模板元素，找到父元素
        /// </summary>
        /// <typeparam name="TParentItem">查找类型</typeparam>
        /// <param name="obj">源</param>
        /// <returns></returns>
        public static TParentItem FindVisualParent<TParentItem>(DependencyObject obj) where TParentItem : DependencyObject
        {
            if (obj != null)
            {
                var parent = VisualTreeHelper.GetParent(obj);
                return parent as TParentItem ?? FindVisualParent<TParentItem>(parent);
            }
            return null;
        }

        /// <summary>
        /// 通过模板元素，找到指定子元素
        /// </summary>
        /// <typeparam name="TChildItem">查找类型</typeparam>
        /// <param name="obj">源</param>
        /// <returns></returns>
        public static TChildItem FindVisualChild<TChildItem>(DependencyObject obj) where TChildItem : DependencyObject
        {
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is TChildItem)
                    return (TChildItem)child;
                var childOfChild = FindVisualChild<TChildItem>(child);
                if (childOfChild != null)
                    return childOfChild;
            }
            return null;
        }


        #endregion

        #region Events


        [DllImport("user32.dll")]
        public static extern bool MoveWindow(IntPtr hWnd, int x, int y, int nWidth, int nHeight, bool bRepaint);


        [DllImport("user32.dll", EntryPoint = "SetForegroundWindow")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        private IntPtr WindPtr
        {
            get
            {
                return new WindowInteropHelper(this).Handle;
            }
        }

        /// <summary>
        /// 右下角移动窗体.
        /// </summary>
        private void MinWindow()
        {
            if (_isMined)
                return;
            _position.Left = Left;
            _position.Top = Top;
            _position.Height = (int)Height;
            _position.Width = (int)Width;
            var size = SystemInformation.WorkingArea;
            int posionX = size.Width - 250;
            int posionY = size.Height - 25;
            MoveWindow(WindPtr, posionX, posionY, 250, 25, true);

            _isMined = true;
            MinButton.Visibility = Visibility.Collapsed;
        }

        //窗体关闭事件
        private void BaseMessageBoxClosed(object sender, EventArgs e)
        {
            if (Owner != null)
            {
                //将Owner推向前方窗体
                SetForegroundWindow(new WindowInteropHelper(Owner).Handle);
            }

            //继承的用
            OnCloseButton(_result);


        }

        //键盘按下事件
        private void BaseMessageBoxKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                KeyEsc();
            }
        }

        #endregion

        #region Properties

        private string _caption;

        /// <summary>
        ///   设置弹出对话框提示信息
        /// </summary>
        public string Caption
        {
            set
            {
                _caption = value;
                if (_textTitle != null)
                {
                    SetWindowTitle();

                }
            }
            get { return _caption; }
        }

        private void SetWindowTitle()
        {
            _textTitle.Text = Caption.SubCutString(60);
        }

        #region 2012-6-18 14:57:45 JL 导入导出工具执行时，不能关闭窗口

        public void SetCloseBtnDisabled()
        {
            CloseButton.IsEnabled = false;
        }

        public void SetCloseBtnEnabled()
        {
            CloseButton.IsEnabled = true;
        }

        #endregion

        private bool _isCollapsedCloseButton;

        /// <summary>
        ///   是否隐藏关闭按钮
        /// </summary>
        protected bool IsCollapsedCloseButton
        {
            set
            {
                _isCollapsedCloseButton = value;
                if (value && CloseButton != null)
                {
                    CloseButton.Visibility = Visibility.Collapsed;
                }
                else if (!value && CloseButton != null)
                {
                    CloseButton.Visibility = Visibility.Visible;
                }
            }
            get { return _isCollapsedCloseButton; }
        }

        private bool _mIsCollapsedMinButton;

        protected bool IsCollapsedMinButton
        {
            get { return _mIsCollapsedMinButton; }
            set
            {
                _mIsCollapsedMinButton = value;
                if (value && CloseButton != null)
                {
                    CloseButton.Visibility = Visibility.Collapsed;
                }
                else if (!value && CloseButton != null)
                {
                    CloseButton.Visibility = Visibility.Visible;
                }
            }
        }

        /// <summary>
        ///   返回窗体内容
        /// </summary>
        public virtual object MsgContent
        {
            get { return string.Empty; }
        }

        #endregion

        #region IMessageBox 接口类型

        /// <summary>
        ///   关闭窗体
        /// </summary>
        public void CloseMessageBox()
        {
            BaseClose();
        }
        #endregion
    }
}