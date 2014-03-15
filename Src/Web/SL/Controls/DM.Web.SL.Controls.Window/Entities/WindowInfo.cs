using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using DM.Web.SL.Common.Utility;

namespace DM.Web.SL.Controls.Window.Entities
{

    #region Delegates

    /// <summary>
    ///   关闭前事件
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    public delegate void WindowClosingEventHandler( object sender, WindowClosingEventArgs e );

    /// <summary>
    ///   关闭后事件
    /// </summary>
    /// <param name = "sender"></param>
    /// <param name = "e"></param>
    public delegate void WindowClosedEventHandler( object sender, WindowClosedEventArgs e );

    #endregion Delegates

    /// <summary>
    ///   窗口实体列
    /// </summary>
    [TemplatePart( Name = "PART_ContentPresenter", Type = typeof( ContentPresenter ) )]
    [TemplatePart( Name = "PART_ScrollContent", Type = typeof( ScrollViewer ) )]
    [TemplatePart( Name = "PART_CaptionBar", Type = typeof( Border ) )]
    [TemplatePart( Name = "PART_CaptionIco", Type = typeof( Image ) )]
    [TemplatePart( Name = "PART_CaptionText", Type = typeof( TextBlock ) )]
    [TemplatePart( Name = "PART_CloseButton", Type = typeof( Button ) )]
    [TemplatePart( Name = "PART_WindowInfo", Type = typeof( Grid ) )]
    [TemplatePart( Name = "PART_LoadingCover", Type = typeof( Border ) )]
    [TemplatePart( Name = "PART_LoadingMassage", Type = typeof( TextBlock ) )]
    public class WindowInfo : ContentControl, IDisposable
    {
        #region Fields

        //public static readonly DependencyProperty MaxImageSourceProperty = DependencyProperty.Register("MaxImageSource", typeof(ImageSource), typeof(Window), null);

        /// <summary>
        ///   HorizontalScrollBarVisibility
        /// </summary>
        public static readonly DependencyProperty HorizontalScrollBarVisibilityProperty = DependencyProperty.Register( "HorizontalScrollBarVisibility", typeof( ScrollBarVisibility ), typeof( WindowInfo ), new PropertyMetadata( new PropertyChangedCallback( OnHorizontalScrollBarVisibilityPropertyChanged ) ) );

        /// <summary>
        ///   滚动条是否可用属性
        /// </summary>
        public static readonly DependencyProperty VerticalScrollBarVisibilityProperty = DependencyProperty.Register( "VerticalScrollBarVisibility", typeof( ScrollBarVisibility ), typeof( WindowInfo ), new PropertyMetadata( new PropertyChangedCallback( OnVerticalScrollBarVisibilityPropertyChanged ) ) );

        private static int _currentZIndex = 1;
        private readonly double _innerContentPresenterOffset;
        private readonly bool _showMaxButton;
        private readonly bool _showMinButton;

        private bool _canResizeEnabled;
        private string _caption;
        private FrameworkElement _captionBar;
        private Image _captionIco;
        private TextBlock _captionText;
        private ContentPresenter _contentpresenter;
        private string _icoImagePath;
        private string _id;
        private Point _initialDragPoint;
        private Point _initialResizePoint;
        private Point _initialWindowLocation;
        private Point _initialWindowLocationM;
        private Size _initialWindowSize;
        private bool _isDragging;
        private bool _isMaximized;
        private bool _isResizing;
        private ResizeAnchor _resizeAnchor;
        private ScrollViewer _scrollcontent;
        private Grid _windowInfo;
        private Border _loadingCover;
        private TextBlock _loadingMassage;
        private Button _closeButton;
        private Button _helpButton;
        private Button _minButton;
        private Button _maxButton;

        #endregion Fields

        #region Constructors

        /// <summary>
        ///   构造函数
        /// </summary>
        public WindowInfo()
        {
            CacheMode = new BitmapCache
                        {
                            RenderAtScale = 1
                        };
            _captionBar = null;
            _windowInfo = null;
            _scrollcontent = null;
            _contentpresenter = null;
            _innerContentPresenterOffset = 1.0;
            _isMaximized = false;
            _showMinButton = true;
            _showMaxButton = false;
            DraggingEnabled = true;
            _caption = "";
            _isDragging = false;
            _isResizing = false;
            _resizeAnchor = ResizeAnchor.None;
            ResizeEnabled = true;
            DefaultStyleKey = typeof( WindowInfo );
            LayoutUpdated -= WindowLayoutUpdated;
            LayoutUpdated += WindowLayoutUpdated;

            Application.Current.Host.Content.FullScreenChanged -= ContentFullScreenChanged;
            Application.Current.Host.Content.FullScreenChanged += ContentFullScreenChanged;

            Loaded += WindowLoaded;
        }


        /// <summary>
        ///   构造函数
        /// </summary>
        /// <param name = "isMinButton">是否可最小化</param>
        /// <param name = "isMaxButton">是否可最大化</param>
        public WindowInfo( bool isMinButton, bool isMaxButton )
        {
            CacheMode = new BitmapCache
            {
                RenderAtScale = 1
            };
            _captionBar = null;
            _windowInfo = null;
            _scrollcontent = null;
            _contentpresenter = null;
            _innerContentPresenterOffset = 1.0;
            _isMaximized = false;
            DraggingEnabled = true;
            _caption = "";
            _isDragging = false;
            _isResizing = false;
            _resizeAnchor = ResizeAnchor.None;
            ResizeEnabled = true;
            _showMinButton = isMinButton;
            _showMaxButton = isMaxButton;
            DefaultStyleKey = typeof( WindowInfo );

            LayoutUpdated -= WindowLayoutUpdated;
            LayoutUpdated += WindowLayoutUpdated;

            Application.Current.Host.Content.FullScreenChanged -= ContentFullScreenChanged;
            Application.Current.Host.Content.FullScreenChanged += ContentFullScreenChanged;

            Loaded += WindowLoaded;
        }

        #endregion Constructors

        #region Enumerations

        private enum ResizeAnchor
        {
            None,
            Left,
            TopLeft,
            Top,
            TopRight,
            Right,
            BottomRight,
            Bottom,
            BottomLeft
        }

        #endregion Enumerations

        #region Events

        /// <summary>
        ///   关闭事件
        /// </summary>
        public event EventHandler CloseEventHandler;

        /// <summary>
        ///   关闭前事件
        /// </summary>
        public event WindowClosingEventHandler ClosingEventHandler;

        /// <summary>
        ///   关闭后事件
        /// </summary>
        public event WindowClosedEventHandler ClosedEventHandler;

        /// <summary>
        ///   拖拽
        /// </summary>
        public event EventHandler Dragged;

        /// <summary>
        /// 最大化
        /// </summary>
        public event EventHandler Maximized;

        /// <summary>
        ///   最小化
        /// </summary>
        public event EventHandler Minimized;

        /// <summary>
        ///   正常
        /// </summary>
        public event EventHandler Normalized;

        /// <summary>
        ///   改变显示层
        /// </summary>
        public event EventHandler SetZIndex;

        #endregion Events

        #region Properties

        /// <summary>
        ///   应用ID
        /// </summary>
        public string ApplicationId { get; set; }

        /// <summary>
        ///   标题
        /// </summary>
        public string Caption
        {
            get { return ( ( _captionText != null ) ? _captionText.Text : _caption ); }
            set
            {
                if ( _captionText != null )
                {
                    _captionText.Text = value;
                }
                else
                {
                    _caption = value;
                }
            }
        }

        /// <summary>
        ///   是否可拖动
        /// </summary>
        public bool DraggingEnabled { get; set; }

        /// <summary>
        ///   横向滚动条是否可用
        /// </summary>
        public ScrollBarVisibility HorizontalScrollBarVisibility
        {
            get { return (ScrollBarVisibility)GetValue( HorizontalScrollBarVisibilityProperty ); }
            set { SetValue( HorizontalScrollBarVisibilityProperty, value ); }
        }

        /// <summary>
        ///   图标
        /// </summary>
        public string IcoPath
        {
            get { return _icoImagePath; }
            set { _icoImagePath = value; }
        }

        /// <summary>
        ///   窗口标识
        /// </summary>
        public string Id
        {
            get
            {
                if ( string.IsNullOrEmpty( _id ) )
                {
                    _id = Guid.NewGuid().ToString();
                }
                return _id;
            }
            set { _id = value; }
        }

        /// <summary>
        ///   是否可改变大小
        /// </summary>
        public bool ResizeEnabled { get; set; }

        /// <summary>
        ///   纵向滚动条是否可用
        /// </summary>
        public ScrollBarVisibility VerticalScrollBarVisibility
        {
            get { return (ScrollBarVisibility)GetValue( VerticalScrollBarVisibilityProperty ); }
            set { SetValue( VerticalScrollBarVisibilityProperty, value ); }
        }

        private bool CanResize
        {
            get
            {
                //return (this.ResizeEnabled && (this.resizeAnchor != ResizeAnchor.None));
                return false;
            }
        }

        #endregion Properties

        #region Methods

        #region 公共方法

        /// <summary>
        ///   析构
        /// </summary>
        public void Dispose()
        {
            _captionIco = null;
            _captionText = null;
            _caption = null;
            _contentpresenter = null;
            _scrollcontent = null;
            _loadingCover = null;
            _loadingMassage = null;

            if ( _captionBar != null )
            {
                _captionBar.MouseLeftButtonDown -= CaptionBarMouseLeftButtonDown;
                _captionBar.MouseMove -= CaptionBarMouseMove;
                _captionBar.MouseLeftButtonUp -= CaptionBarMouseLeftButtonUp;
                _captionBar = null;
            }

            if ( _windowInfo != null )
            {
                _windowInfo.MouseLeftButtonDown -= WindowInfoMouseLeftButtonDown;
                _windowInfo.MouseMove -= WindowInfoMouseMove;
                _windowInfo.MouseLeftButtonUp -= WindowInfoMouseLeftButtonUp;

                _windowInfo = null;
            }

            if ( _closeButton != null )
            {
                _closeButton.Click -= CloseButtonClick;
                _closeButton = null;
            }

            if (_helpButton != null)
            {
                _helpButton.Click -= HelpButtonClick;
                _helpButton = null;
            }

            if (_minButton != null)
            {
                _minButton.Click -= MinButtonClick;
                _minButton = null;
            }

            if ( _maxButton != null )
            {
                _maxButton.Click -= MaxButtonClick;
                _maxButton = null;
            }

            Loaded -= WindowLoaded;
            LayoutUpdated -= WindowLayoutUpdated;
            Application.Current.Host.Content.FullScreenChanged -= ContentFullScreenChanged;
        }

        /// <summary>
        /// </summary>
        /// <param name = "sender"></param>
        /// <param name = "e"></param>
        public void CaptionTextMouseLeftButtonDown( object sender, MouseButtonEventArgs e )
        {
            Close();
        }

        /// <summary>
        ///   关闭窗口
        /// </summary>
        public bool Close()
        {
            Action act = () =>
                         {
                             Visibility = Visibility.Collapsed;
                             if (CloseEventHandler != null)
                             {
                                 CloseEventHandler(this, EventArgs.Empty);
                             }
                             if (ClosedEventHandler != null)
                             {
                                 WindowClosedEventArgs arg = new WindowClosedEventArgs(CloseReason.FormOwnerClosing, false);
                                 ClosedEventHandler(this, arg);
                             }

                             //释放内存
                             Dispose();
                         };

            if (ClosingEventHandler != null)
            {
                WindowClosingEventArgs arg = new WindowClosingEventArgs(CloseReason.FormOwnerClosing, false);
                ClosingEventHandler(this, arg);
                if (!arg.Cancel)
                {
                    act();
                }
                else
                {
                    return false;
                }
            }
            else
            {
                act();
            }
            return true;
        }

        /// <summary>
        ///   最大化
        /// </summary>
        public void Maximize()
        {
            if ( !_isMaximized )
            {
                _canResizeEnabled = false;
                _initialWindowSize.Width = !double.IsNaN( Width ) ? Width : ActualWidth;
                _initialWindowSize.Height = !double.IsNaN( Height ) ? Height : ActualHeight;
                _initialWindowLocationM.X = Canvas.GetLeft( this );
                _initialWindowLocationM.Y = Canvas.GetTop( this );

                
                Canvas.SetLeft(this, 0);
                Canvas.SetTop(this, 0);
                Width = WindowsManager.Instance.Desktop.ActualWidth;
                Height = WindowsManager.Instance.Desktop.ActualHeight;
                DraggingEnabled = false;

                if (this.Maximized != null)
                {
                    this.Maximized(this, EventArgs.Empty);
                }
            }
            else
            {
                _canResizeEnabled = ResizeEnabled;

                Canvas.SetLeft( this, _initialWindowLocationM.X );
                Canvas.SetTop( this, _initialWindowLocationM.Y );
                Width = _initialWindowSize.Width;
                Height = _initialWindowSize.Height;
                DraggingEnabled = true;

                if ( Normalized != null )
                {
                    Normalized( this, EventArgs.Empty );
                }
            }
            _isMaximized = !_isMaximized;
            SetContentPresenterWidth();
        }

        /// <summary>
        ///   应用样式
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _windowInfo = GetTemplateChild( "PART_Window" ) as Grid;
            _scrollcontent = GetTemplateChild( "PART_ScrollContent" ) as ScrollViewer;
            if ( _scrollcontent != null )
            {
                _scrollcontent.HorizontalScrollBarVisibility = HorizontalScrollBarVisibility;
                _scrollcontent.VerticalScrollBarVisibility = VerticalScrollBarVisibility;
            }
            _contentpresenter = GetTemplateChild( "PART_ContentPresenter" ) as ContentPresenter;
            _captionBar = GetTemplateChild( "PART_CaptionBar" ) as FrameworkElement;
            _captionText = GetTemplateChild( "PART_CaptionText" ) as TextBlock;
            if ( _captionText != null )
            {
                _captionText.Text = _caption;
                //_captionText.MouseLeftButtonDown += new MouseButtonEventHandler(CaptionTextMouseLeftButtonDown);
            }
            _captionIco = GetTemplateChild( "PART_CaptionIco" ) as Image;
            if ( _captionIco != null && !string.IsNullOrEmpty( _icoImagePath ) )
            {
                BitmapImage image = new BitmapImage();
                image.UriSource = new Uri( _icoImagePath );
                _captionIco.Source = image;
            }

            _helpButton = GetTemplateChild( "PART_HelpButton" ) as Button;
            if ( _helpButton != null )
            {
                _helpButton.Click += HelpButtonClick;
            }

            _minButton = GetTemplateChild( "PART_MinButton" ) as Button;
            if ( _minButton != null )
            {
                _minButton.Click += MinButtonClick;
                if ( !_showMinButton )
                {
                    _minButton.Visibility = Visibility.Collapsed;
                }
            }
            _maxButton = GetTemplateChild( "PART_MaxButton" ) as Button;
            if ( _maxButton != null )
            {
                _maxButton.Click += MaxButtonClick;
                if ( !_showMaxButton )
                {
                    _maxButton.Visibility = Visibility.Collapsed;
                }
            }
            _closeButton = GetTemplateChild( "PART_CloseButton" ) as Button;
            if ( _closeButton != null )
            {
                _closeButton.Click += CloseButtonClick;
            }
            DefineDragEvents();
            DefineResizeEvents();
            Canvas.SetZIndex( this, _currentZIndex );
            if ( SetZIndex != null )
            {
                SetZIndex( this, EventArgs.Empty );
            }

            _loadingCover = GetTemplateChild( "PART_LoadingCover" ) as Border;
            _loadingMassage = GetTemplateChild( "PART_LoadingMassage" ) as TextBlock;
        }

        /// <summary>
        /// 显示加载信息
        /// </summary>
        /// <param name="massage">信息内容, 请自行全球化</param>
        public void ShowLoadingMassage( string massage )
        {
            _loadingMassage.Text = massage;
            _loadingCover.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 显示加载信息
        /// </summary>
        /// <param name="massage">信息内容, 请自行全球化</param>
        /// <param name="background">背景颜色 如Color.FromArgb(0xE5, 0x00, 0x00, 0x00)</param>
        public void ShowLoadingMassage( string massage, Color background )
        {
            _loadingMassage.Text = massage;
            _loadingCover.Background = new SolidColorBrush(background);
            _loadingCover.Visibility = Visibility.Visible;
        }


        /// <summary>
        /// 关闭加载消息
        /// </summary>
        public void HideLoadingMassage()
        {
            _loadingMassage.Text = "";
            _loadingCover.Visibility = Visibility.Collapsed;
        }

        #endregion

        #region 私有方法
        /// <summary>
        ///   得到焦点
        /// </summary>
        /// <param name = "e"></param>
        protected override void OnGotFocus( RoutedEventArgs e )
        {
            var win = e.OriginalSource as WindowInfo;
            if (win != null && win.Caption == "内容管理")
            { 
                
            }
            base.OnGotFocus( e );
            _currentZIndex = _currentZIndex >= 999000 ? 1 : _currentZIndex;
            Canvas.SetZIndex( this, ++_currentZIndex );
            if ( SetZIndex != null )
            {
                SetZIndex( this, EventArgs.Empty );
            }
        }

        private static void OnHorizontalScrollBarVisibilityPropertyChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            WindowInfo window = d as WindowInfo;
            if ( ( window != null ) && ( window._scrollcontent != null ) )
            {
                window._scrollcontent.HorizontalScrollBarVisibility = (ScrollBarVisibility)e.NewValue;
            }
        }

        private static void OnVerticalScrollBarVisibilityPropertyChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            WindowInfo window = d as WindowInfo;
            if ( ( window != null ) && ( window._scrollcontent != null ) )
            {
                window._scrollcontent.VerticalScrollBarVisibility = (ScrollBarVisibility)e.NewValue;
            }
        }

        private void CaptionBarMouseLeftButtonDown( object sender, MouseButtonEventArgs e )
        {
            if ( DraggingEnabled )
            {
                Canvas.SetZIndex( this, ++_currentZIndex );
                if ( SetZIndex != null )
                {
                    SetZIndex( this, EventArgs.Empty );
                }
                ( (FrameworkElement)sender ).CaptureMouse();
                _initialDragPoint = e.GetPosition( Parent as UIElement );
                _initialWindowLocation.X = Canvas.GetLeft( this );
                _initialWindowLocation.Y = Canvas.GetTop( this );
                _isDragging = true;
            }
        }

        private void CaptionBarMouseLeftButtonUp( object sender, MouseButtonEventArgs e )
        {
            if ( DraggingEnabled )
            {
                ( (FrameworkElement)sender ).ReleaseMouseCapture();
                _isDragging = false;
            }
        }

        private void CaptionBarMouseMove( object sender, MouseEventArgs e )
        {
            if ( _isDragging )
            {
                Dispatcher.BeginInvoke( () =>
                                           {
                                               Point position = e.GetPosition( Parent as UIElement );
                                               Canvas parent = Parent as Canvas;
                                               double length = ( _initialWindowLocation.X + position.X ) - _initialDragPoint.X;
                                               //2010-6-9 16:25:11 ljm 左右拖拽限定为可以脱出
                                               //if ( ( length >= 0.0 ) && ( ( length + this.captionBar.ActualWidth ) <= parent.ActualWidth ) )
                                               if ( parent != null )
                                                   if ( ( length >= -_captionBar.ActualWidth + 2.0 ) && ( length <= parent.ActualWidth ) )
                                                   {
                                                       Canvas.SetLeft( this, length );
                                                   }
                                               double num2 = ( _initialWindowLocation.Y + position.Y ) - _initialDragPoint.Y;
                                               if ( parent != null )
                                                   if ( ( num2 >= 0.0 ) && ( ( num2 + _captionBar.ActualHeight ) <= parent.ActualHeight ) )
                                                   {
                                                       Canvas.SetTop( this, num2 );
                                                   }
                                               if ( Dragged != null )
                                               {
                                                   Dragged( this, EventArgs.Empty );
                                               }
                                           } );
            }
        }

        private void CloseButtonClick( object sender, RoutedEventArgs e )
        {
            Close();
        }

        private void ContentFullScreenChanged( object sender, EventArgs e )
        {
            //SetContentPresenterWidth();
        }

        private void DefineDragEvents()
        {
            if ( _captionBar != null )
            {
                _captionBar.MouseLeftButtonDown += CaptionBarMouseLeftButtonDown;
                _captionBar.MouseMove += CaptionBarMouseMove;
                _captionBar.MouseLeftButtonUp += CaptionBarMouseLeftButtonUp;
            }
        }

        private void DefineResizeEvents()
        {
            if ( _windowInfo != null )
            {
                _windowInfo.MouseLeftButtonDown += WindowInfoMouseLeftButtonDown;
                _windowInfo.MouseMove += WindowInfoMouseMove;
                _windowInfo.MouseLeftButtonUp += WindowInfoMouseLeftButtonUp;
            }
        }

        private void HelpButtonClick( object sender, RoutedEventArgs e )
        {
            string url = AppHelpHelper.Instance.GetAppHelpUrl(ApplicationId);

            System.Windows.Browser.HtmlPage.Window.Navigate(new Uri(url), "_blank");
        }

        private void MaxButtonClick( object sender, RoutedEventArgs e )
        {
            Maximize();
        }

        private void MinButtonClick( object sender, RoutedEventArgs e )
        {
            Visibility = Visibility.Collapsed;
            if ( Minimized != null )
            {
                Minimized( this, EventArgs.Empty );
            }
        }

        private void ResizeBottom( double deltaY )
        {
            Height = Math.Max( _initialWindowSize.Height + deltaY, _captionBar.ActualHeight );
        }

        private void ResizeLeft( double deltaX )
        {
            double num = ( _initialWindowLocation.X + _initialWindowSize.Width ) - 60.0;
            Canvas.SetLeft( this, Math.Min( _initialResizePoint.X + deltaX, num ) );
            Width = Math.Max( ( _initialWindowSize.Width - deltaX ), 60.0 );
        }

        private void ResizeRight( double deltaX )
        {
            Width = Math.Max( ( _initialWindowSize.Width + deltaX ), 60.0 );
        }

        private void ResizeTop( double deltaY )
        {
            double num = ( _initialWindowLocation.Y + _initialWindowSize.Height ) - _captionBar.ActualHeight;
            Canvas.SetTop( this, Math.Min( _initialResizePoint.Y + deltaY, num ) );
            Height = Math.Max( _initialWindowSize.Height - deltaY, _captionBar.ActualHeight );
        }

        private void SetContentPresenterSizeAndMinSize()
        {
            _contentpresenter.Width = Width - _innerContentPresenterOffset;
            _contentpresenter.Height = Height - _innerContentPresenterOffset;
            if ( _contentpresenter.Content != null )
            {
                if ( HorizontalScrollBarVisibility != ScrollBarVisibility.Hidden )
                {
                    _contentpresenter.MinWidth = ( (FrameworkElement)_contentpresenter.Content ).MinWidth;
                }
                if ( VerticalScrollBarVisibility != ScrollBarVisibility.Hidden )
                {
                    _contentpresenter.MinHeight = ( (FrameworkElement)_contentpresenter.Content ).MinHeight;
                }
                ( (FrameworkElement)_contentpresenter.Content ).Width = _contentpresenter.Width;
                ( (FrameworkElement)_contentpresenter.Content ).Height = _contentpresenter.Height;
            }
        }

        private void SetContentPresenterWidth()
        {
            _scrollcontent.Margin = new Thickness( 0, -2, 0, 0 );
            _scrollcontent.Padding = new Thickness( 0 );

            double num = Width - _innerContentPresenterOffset;
            double num2 = ( Height - 30.0 ) - _innerContentPresenterOffset;
            _contentpresenter.Width = ( num > 0.0 ) ? num : 0.0;
            _contentpresenter.Height = ( num2 > 0.0 ) ? num2 : 0.0;
            _contentpresenter.Margin = new Thickness( 0 );

            Canvas.SetLeft( _scrollcontent, 0 );
            Canvas.SetLeft( _contentpresenter, 0 );
        }

        private void WindowLayoutUpdated( object sender, EventArgs e )
        {
            if ( _scrollcontent != null )
            {
                //this.innerContentPresenterOffset = ActualWidth - ( ( ( ( ( ( this.scrollcontent.ActualWidth - this.scrollcontent.Margin.Left ) - this.scrollcontent.Margin.Right ) - this.scrollcontent.Padding.Left ) - this.scrollcontent.Padding.Right ) - this.scrollcontent.BorderThickness.Left ) - this.scrollcontent.BorderThickness.Right );
                //this.innerContentPresenterOffset = Math.Max( this.innerContentPresenterOffset, 1.0 );
                SetContentPresenterSizeAndMinSize();
            }
        }

        private void WindowInfoMouseLeftButtonDown( object sender, MouseButtonEventArgs e )
        {
            if ( CanResize )
            {
                ( (FrameworkElement)sender ).CaptureMouse();
                _initialResizePoint = e.GetPosition( Parent as UIElement );
                _initialWindowSize.Width = !double.IsNaN( Width ) ? Width : ActualWidth;
                _initialWindowSize.Height = !double.IsNaN( Height ) ? Height : ActualHeight;
                _initialWindowLocation.X = Canvas.GetLeft( this );
                _initialWindowLocation.Y = Canvas.GetTop( this );
                _isResizing = true;
            }
        }

        private void WindowInfoMouseLeftButtonUp( object sender, MouseButtonEventArgs e )
        {
            if ( _canResizeEnabled && _isResizing )
            {
                ( (FrameworkElement)sender ).ReleaseMouseCapture();
                _isResizing = false;
            }
        }

        private void WindowInfoMouseMove( object sender, MouseEventArgs e )
        {
            if ( _canResizeEnabled )
            {
                Dispatcher.BeginInvoke( () =>
                                           {
                                               if ( !_isResizing )
                                               {
                                                   #region 非拖拽

                                                   Point position = e.GetPosition( _windowInfo );
                                                   if ( ( position.Y <= 3.0 ) && ( position.X <= 3.0 ) )
                                                   {
                                                       _windowInfo.Cursor = Cursors.Hand;
                                                       _resizeAnchor = ResizeAnchor.TopLeft;
                                                   }
                                                   else if ( ( position.Y <= 3.0 ) && ( position.X >= ( _windowInfo.ActualWidth - 3.0 ) ) )
                                                   {
                                                       _windowInfo.Cursor = Cursors.Hand;
                                                       _resizeAnchor = ResizeAnchor.TopRight;
                                                   }
                                                   else if ( position.Y <= 3.0 )
                                                   {
                                                       _windowInfo.Cursor = Cursors.SizeNS;
                                                       _resizeAnchor = ResizeAnchor.Top;
                                                   }
                                                   else if ( ( position.Y >= ( _windowInfo.ActualHeight - 3.0 ) ) && ( position.X <= 3.0 ) )
                                                   {
                                                       _windowInfo.Cursor = Cursors.Hand;
                                                       _resizeAnchor = ResizeAnchor.BottomLeft;
                                                   }
                                                   else if ( ( position.Y >= ( _windowInfo.ActualHeight - 3.0 ) ) && ( position.X >= ( _windowInfo.ActualWidth - 3.0 ) ) )
                                                   {
                                                       _windowInfo.Cursor = Cursors.Hand;
                                                       _resizeAnchor = ResizeAnchor.BottomRight;
                                                   }
                                                   else if ( position.Y >= ( _windowInfo.ActualHeight - 3.0 ) )
                                                   {
                                                       _windowInfo.Cursor = Cursors.SizeNS;
                                                       _resizeAnchor = ResizeAnchor.Bottom;
                                                   }
                                                   else if ( position.X <= 3.0 )
                                                   {
                                                       _windowInfo.Cursor = Cursors.SizeWE;
                                                       _resizeAnchor = ResizeAnchor.Left;
                                                   }
                                                   else if ( position.X >= ( _windowInfo.ActualWidth - 3.0 ) )
                                                   {
                                                       _windowInfo.Cursor = Cursors.SizeWE;
                                                       _resizeAnchor = ResizeAnchor.Right;
                                                   }
                                                   else
                                                   {
                                                       _windowInfo.Cursor = null;
                                                       _resizeAnchor = ResizeAnchor.None;
                                                   }

                                                   #endregion
                                               }
                                               else
                                               {
                                                   #region 拖拽

                                                   Point point2 = e.GetPosition( Parent as UIElement );
                                                   double deltaX = point2.X - _initialResizePoint.X;
                                                   double deltaY = point2.Y - _initialResizePoint.Y;
                                                   Dispatcher.BeginInvoke( () =>
                                                                              {
                                                                                  switch ( _resizeAnchor )
                                                                                  {
                                                                                      case ResizeAnchor.Left:
                                                                                          ResizeLeft( deltaX );
                                                                                          break;

                                                                                      case ResizeAnchor.TopLeft:
                                                                                          ResizeLeft( deltaX );
                                                                                          ResizeTop( deltaY );
                                                                                          break;

                                                                                      case ResizeAnchor.Top:
                                                                                          ResizeTop( deltaY );
                                                                                          break;

                                                                                      case ResizeAnchor.TopRight:
                                                                                          ResizeRight( deltaX );
                                                                                          ResizeTop( deltaY );
                                                                                          break;

                                                                                      case ResizeAnchor.Right:
                                                                                          ResizeRight( deltaX );
                                                                                          break;

                                                                                      case ResizeAnchor.BottomRight:
                                                                                          ResizeRight( deltaX );
                                                                                          ResizeBottom( deltaY );
                                                                                          break;

                                                                                      case ResizeAnchor.Bottom:
                                                                                          ResizeBottom( deltaY );
                                                                                          break;

                                                                                      case ResizeAnchor.BottomLeft:
                                                                                          ResizeLeft( deltaX );
                                                                                          ResizeBottom( deltaY );
                                                                                          break;
                                                                                  }

                                                                                  if ( CanResize )
                                                                                  {
                                                                                      ( (FrameworkElement)sender ).CaptureMouse();
                                                                                      _initialResizePoint = e.GetPosition( Parent as UIElement );
                                                                                      _initialWindowSize.Width = !double.IsNaN( Width ) ? Width : ActualWidth;
                                                                                      _initialWindowSize.Height = !double.IsNaN( Height ) ? Height : ActualHeight;
                                                                                      _initialWindowLocation.X = Canvas.GetLeft( this );
                                                                                      _initialWindowLocation.Y = Canvas.GetTop( this );
                                                                                      _isResizing = true;
                                                                                  }
                                                                              } );
                                                   //this.SetContentPresenterWidth(); 

                                                   #endregion
                                               }
                                           } );
            }
        }

        private void WindowLoaded( object sender, RoutedEventArgs e )
        {
            Focus();
        }
        #endregion

        #endregion Methods
    }
}