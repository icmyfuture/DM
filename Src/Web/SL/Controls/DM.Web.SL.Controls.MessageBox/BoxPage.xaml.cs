using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace DM.Web.SL.Controls.MessageBox
{
    public partial class BoxPage : IPopupBox
    {
        #region Fields

        private readonly DragService m_DragService;

        #endregion Fields

        #region Constructors

        public BoxPage()
        {
            // 需要初始化变量
            InitializeComponent();
            m_DragService = new DragService(this);
        }

        #endregion Constructors

        #region Events

        public event EventHandler CloseComplete;

        public event EventHandler ShowComplete;

        #endregion Events

        #region Properties

        public Button CloseBoxElement
        {
            get
            {
                return CloseBox;
            }
        }

        public ImageSource CloseIcon
        {
            get
            {
                ImageBrush brush = CloseBox.Background as ImageBrush;
                return (brush == null ? null : brush.ImageSource);
            }
            set
            {
                if (value != null)
                {
                    ImageBrush brush = new ImageBrush();
                    brush.ImageSource = value;
                    CloseBox.Background = brush;
                }
            }
        }

        public FrameworkElement ContentElement
        {
            get;
            set;
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

        public bool IsModal
        {
            get;
            private set;
        }

        public Panel LayoutElement
        {
            get
            {
                return LayoutRoot;
            }
        }

        public LayoutMask Mask
        {
            get;
            set;
        }

        public bool ShowCloseBox
        {
            get
            {
                return (CloseBox.Visibility == Visibility.Visible);
            }
            set
            {
                if (value)
                {
                    CloseBox.Visibility = Visibility.Visible;
                }
                else
                {
                    CloseBox.Visibility = Visibility.Collapsed;
                }
            }
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

        protected void OnCloseComplete(EventArgs e)
        {
            EventHandler handler = CloseComplete;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected void OnShowComplete(EventArgs e)
        {
            EventHandler handler = ShowComplete;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void CloseBox_Click(object sender, RoutedEventArgs e)
        {
            Close();
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

                OnCloseComplete(EventArgs.Empty);
            }
        }

        private void Effect_CompleteOnShow(object sender, EventArgs e)
        {
            Effect.Complete -= Effect_CompleteOnShow;

            OnShowComplete(EventArgs.Empty);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (ContentElement != null)
            {
                LayoutRoot.Children.Add(ContentElement);
            }

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