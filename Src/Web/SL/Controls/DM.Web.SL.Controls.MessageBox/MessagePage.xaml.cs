using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DM.Web.SL.Controls.MessageBox
{
    public partial class MessagePage : UserControl, IPopupBox
    {
        #region Fields

        private readonly DragService m_DragService;

        #endregion Fields

        #region Constructors

        public MessagePage()
        {
            // 需要初始化变量
            InitializeComponent();
            m_DragService = new DragService(this);
        }

        #endregion Constructors

        #region Events

        public event EventHandler ButtonClick;

        public event EventHandler ShowComplete;

        #endregion Events

        #region Properties

        public Border Border
        {
            get
            {
                return LayoutRoot;
            }
        }

        public Panel ButtonElement
        {
            get
            {
                return ButtonPanel;
            }
        }

        public MessageBoxButtonType ButtonType
        {
            get;
            set;
        }

        public Panel ContentElement
        {
            get
            {
                return ContentPanel;
            }
        }

        public UIElement DragMouseCaptureArea
        {
            get
            {
                return TitlePanel;
            }
        }

        public DragService DragService
        {
            get
            {
                return m_DragService;
            }
        }

        public Effect Effect
        {
            get;
            set;
        }

        public FrameworkElement Element
        {
            get
            {
                return this;
            }
        }

        public MessageBoxIcon Icon
        {
            get;
            set;
        }

        public bool IsModal
        {
            get;
            private set;
        }

        public LayoutMask Mask
        {
            get;
            set;
        }

        public string Message
        {
            get
            {
                return MessageText.Text;
            }
            set
            {
                MessageText.Text = value;
            }
        }

        public MessageBoxButtonResult Result
        {
            get;
            private set;
        }

        public string Title
        {
            get
            {
                return TitleText.Text;
            }
            set
            {
                TitleText.Text = value;
            }
        }

        public Panel TitleElement
        {
            get
            {
                return TitlePanel;
            }
        }

        #endregion Properties

        #region Methods

        public void Close()
        {
            //进行退出动画
            Effect.Complete += new EventHandler(Effect_CompleteOnOut);
            Effect.PerformOutEffect();
        }

        public void Show()
        {
            //设定ZIndex
            Canvas.SetZIndex(this, Mask.MaxZIndex + 1);

            //先将控件隐藏以显示动画效果
            Visibility = Visibility.Collapsed;

            Mask.AddBox(this);
        }

        public void ShowAsModal()
        {
            IsModal = true;
            Show();
        }

        protected virtual void OnButtonClick(EventArgs e)
        {
            EventHandler handler = ButtonClick;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnShowComplete(EventArgs e)
        {
            EventHandler handler = ShowComplete;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void AddButton(string text)
        {
            Button button = new Button();
            button.Content = text;
            button.Width = 60;
            button.HorizontalAlignment = HorizontalAlignment.Center;
            button.Margin = new Thickness(5, 3, 5, 3);
            button.Click += new RoutedEventHandler(Button_Click);
            ButtonPanel.Children.Add(button);
        }

        private void BuildButton()
        {
            string typeEnum = ButtonType.ToString();
            typeEnum = Regex.Replace(
                typeEnum,
                "[A-Z]",
                match => ":" + match.Value
            );
            string[] types = typeEnum.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string type in types)
            {
                AddButton(type);
            }
        }

        private void BuildContent(FrameworkElement icon)
        {
            if (icon != null)
            {
                icon.Width = 64;
                icon.Height = 64;
                icon.HorizontalAlignment = HorizontalAlignment.Left;
                icon.VerticalAlignment = VerticalAlignment.Top;
                icon.Margin = new Thickness(5, 10, 5, 10);
                ContentPanel.Children.Insert(0, icon);
            }
        }

        private void BuildTitle(FrameworkElement icon)
        {
            if (icon != null)
            {
                icon.Width = 18;
                icon.Height = 18;
                icon.HorizontalAlignment = HorizontalAlignment.Left;
                icon.VerticalAlignment = VerticalAlignment.Center;
                icon.Margin = new Thickness(2, 0, 0, 0);
                TitlePanel.Children.Insert(0, icon);
            }
            TitleText.Text = Title;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                Result = (MessageBoxButtonResult)Enum.Parse(
                    typeof(MessageBoxButtonResult),
                    (string)button.Content,
                    true
                );
                Close();
            }
        }

        private void Effect_CompleteOnOut(object sender, EventArgs e)
        {
            //有可能由于多次关闭导致此事件执行多次
            //这种情况下第一次执行是有效的
            //以后的事件会因为Mask.RemoveBox方法的调用使Mask为null
            //因此需要进行判断
            if (Mask != null)
            {
                //从Mask中移除
                Mask.RemoveBox(this);

                Effect.Complete -= Effect_CompleteOnOut;

                OnButtonClick(EventArgs.Empty);
            }
        }

        private void Effect_CompleteOnShow(object sender, EventArgs e)
        {
            Effect.Complete -= Effect_CompleteOnShow;

            OnShowComplete(EventArgs.Empty);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            FrameworkElement titleIcon = null;
            FrameworkElement contentIcon = null;

            if (Icon != null)
            {
                titleIcon = Icon.GetTitleIcon();
                contentIcon = Icon.GetContentIcon();
            }
            BuildTitle(titleIcon);
            BuildContent(contentIcon);
            BuildButton();

            Effect.Complete += new EventHandler(Effect_CompleteOnShow);
            Visibility = Visibility.Visible;
            UpdateLayout();
            //计算位置并显示
            Mask.PositionBox(this);
            Effect.PerformInEffect();
        }

        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Canvas.SetZIndex(this, Mask.MaxZIndex + 1);
        }

        #endregion Methods
    }
}