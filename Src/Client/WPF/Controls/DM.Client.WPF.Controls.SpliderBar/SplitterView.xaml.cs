using DM.Client.WPF.Controls.SpliderBar.Enum;
using DM.Client.WPF.Controls.SpliderBar.EventArgs;
using System;
using System.Windows;
using System.Windows.Controls;

namespace DM.Client.WPF.Controls.SpliderBar
{
    /// <summary>
    /// SplitterView.xaml 的交互逻辑
    /// </summary>
    public partial class SplitterView : Grid
    {

        #region Fields

        /// <summary>
        /// 容器，这是个依赖属性
        /// </summary>
        private static readonly DependencyProperty ChildProperty;

        /// <summary>
        /// 收缩方向,这是个依赖属性
        /// </summary>
        private static readonly DependencyProperty ChildDirectionProperty;

        public event EventHandler<UpdateInfoArgs> UpdateLayOut;

        public void OnUpdateLayOut(UpdateInfoArgs e)
        {
            EventHandler<UpdateInfoArgs> handler = UpdateLayOut;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion

        #region Constructors

        static SplitterView()
        {
            ChildProperty = DependencyProperty.Register("Child", typeof(UIElement), typeof(SplitterView),
                                                        new PropertyMetadata(new PropertyChangedCallback(OnChildPropertyChangedCallback)));
            ChildDirectionProperty = DependencyProperty.Register("ChildDirection", typeof(DirectionType), typeof(SplitterView),
                                                            new PropertyMetadata(new PropertyChangedCallback(OnChildDirectionPropertyChangedCallback)));
        }

        public SplitterView()
        {
            InitializeComponent();
            Init();
        }

        #endregion

        #region Methods

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            _splitterHorizontalButton.Click += SplitterButtonClick;
            _splitterVerticalButton.Click += SplitterButtonClick;
            _splitterHorizontalButton.Loaded += SplitterHorizontalButtonLoaded;
            _splitterVerticalButton.Loaded += SplitterVerticalButtonLoaded;
        }

        /// <summary>
        /// 更新布局
        /// </summary>
        private void UpdateSplitter()
        {
            grid.Children.Clear();
            grid.ColumnDefinitions.Clear();
            grid.RowDefinitions.Clear();
            if (ChildDirection == DirectionType.Down || ChildDirection == DirectionType.Up)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                grid.Children.Add(SplitterVerticalButton);
                grid.Children.Add(Child);
                if (ChildDirection == DirectionType.Down)
                {
                    SetRow(_splitterVerticalButton, 0);
                    SetRow(Child, 1);
                }
                else if (ChildDirection == DirectionType.Up)
                {
                    SetRow(Child, 0);
                    SetRow(_splitterVerticalButton, 1);
                }
            }
            else if (ChildDirection == DirectionType.Left || ChildDirection == DirectionType.Right)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                grid.Children.Add(SplitterHorizontalButton);
                grid.Children.Add(Child);
                if (ChildDirection == DirectionType.Left)
                {
                    SetColumn(Child, 0);
                    SetColumn(SplitterHorizontalButton, 1);
                }
                else if (ChildDirection == DirectionType.Right)
                {
                    SetColumn(SplitterHorizontalButton, 0);
                    SetColumn(Child, 1);
                }
            }
        }

        /// <summary>
        /// 是否显示
        /// </summary>
        /// <param name="isVisible"></param>
        public void UpdateVisible(bool isVisible)
        {
            //更新父窗体的大小
            OnUpdateLayOut(new UpdateInfoArgs(isVisible));
            if (isVisible)
            {
                if (ChildDirection == DirectionType.Left || ChildDirection == DirectionType.Right)
                    ShowChild(SplitterHorizontalButton);
                else if (ChildDirection == DirectionType.Up || ChildDirection == DirectionType.Down)
                    ShowChild(SplitterVerticalButton);
            }
            else
            {
                if (ChildDirection == DirectionType.Left || ChildDirection == DirectionType.Right)
                    HideChild(SplitterHorizontalButton);
                else if (ChildDirection == DirectionType.Up || ChildDirection == DirectionType.Down)
                    HideChild(SplitterVerticalButton);
            }
        }

        /// <summary>
        /// 容器属性更改通知
        /// </summary>
        /// <param name="eventArgs"></param>
        private void OnChildChanged(DependencyPropertyChangedEventArgs eventArgs)
        {
            if (eventArgs.NewValue == null)
                return;
            UpdateSplitter();
        }

        /// <summary>
        /// 收缩方向属性更改通知
        /// </summary>
        /// <param name="eventArgs"></param>
        private void OnChildDirectionChanged(DependencyPropertyChangedEventArgs eventArgs)
        {
            var directionType = (DirectionType)eventArgs.NewValue;
            if (directionType == DirectionType.None)
                return;
            if (Child == null)
                return;
            UpdateSplitter();
        }

        /// <summary>
        /// 显示
        /// </summary>
        /// <param name="sender"></param>
        private void HideChild(object sender)
        {
            Child.Visibility = Visibility.Collapsed;
            if (ChildDirection == DirectionType.Right || ChildDirection == DirectionType.Down)
                VisualStateManager.GoToState(sender as Button, "clickb", true);
            else if (ChildDirection == DirectionType.Left || ChildDirection == DirectionType.Up)
                VisualStateManager.GoToState(sender as Button, "clicka", true);
        }

        /// <summary>
        /// 隐藏
        /// </summary>
        /// <param name="sender"></param>
        private void ShowChild(object sender)
        {
            Child.Visibility = Visibility.Visible;
            if (ChildDirection == DirectionType.Left || ChildDirection == DirectionType.Up)
                VisualStateManager.GoToState(sender as Button, "clickb", true);
            else if (ChildDirection == DirectionType.Right || ChildDirection == DirectionType.Down)
                VisualStateManager.GoToState(sender as Button, "clicka", true);
        }

        #endregion

        #region Events

        private static void OnChildPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs eventArgs)
        {
            ((SplitterView)d).OnChildChanged(eventArgs);
        }

        private static void OnChildDirectionPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs eventArgs)
        {
            ((SplitterView)d).OnChildDirectionChanged(eventArgs);
        }

        /// <summary>
        /// 收缩按钮鼠标点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SplitterButtonClick(object sender, RoutedEventArgs e)
        {
            bool isExpand = true;
            if (Child.Visibility == Visibility.Visible)
            {
                isExpand = false;
            }
            OnUpdateLayOut(new UpdateInfoArgs(isExpand));
            if (isExpand)
            {
                ShowChild(sender);
            }
            else
            {
                HideChild(sender);
            }
        }

        private bool _horizontalFirst;
        /// <summary>
        /// 水平收缩按钮加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SplitterHorizontalButtonLoaded(object sender, RoutedEventArgs e)
        {
            if (!_horizontalFirst)
            {
                if (ChildDirection == DirectionType.Left)
                    VisualStateManager.GoToState(_splitterHorizontalButton, "clickb", true);
                else if (ChildDirection == DirectionType.Right)
                    VisualStateManager.GoToState(_splitterHorizontalButton, "clicka", true);
                _horizontalFirst = true;
            }
        }

        private bool _verticalFirst;
        /// <summary>
        /// 垂直收缩按钮加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SplitterVerticalButtonLoaded(object sender, RoutedEventArgs e)
        {
            if (!_verticalFirst)
            {
                if (ChildDirection == DirectionType.Up)
                    VisualStateManager.GoToState(_splitterVerticalButton, "clickb", true);
                else if (ChildDirection == DirectionType.Down)
                    VisualStateManager.GoToState(_splitterVerticalButton, "clicka", true);
                _verticalFirst = true;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// 容器，这是个依赖属性
        /// </summary>
        public UIElement Child
        {
            get
            {
                lock (this)
                {
                    return (UIElement)GetValue(ChildProperty);
                }
            }
            set
            {
                lock (this)
                {
                    SetValue(ChildProperty, value);
                }
            }
        }

        /// <summary>
        /// 收缩方向，这是个依赖属性
        /// </summary>
        public DirectionType ChildDirection
        {
            get
            {
                lock (this)
                {
                    return (DirectionType)GetValue(ChildDirectionProperty);
                }
            }
            set
            {
                lock (this)
                {
                    SetValue(ChildDirectionProperty, value);
                }
            }
        }

        /// <summary>
        /// 收缩按钮
        /// </summary>
        private readonly SplitterHorizontalButton _splitterHorizontalButton = new SplitterHorizontalButton();
        public SplitterHorizontalButton SplitterHorizontalButton
        {
            get { return _splitterHorizontalButton; }
        }

        /// <summary>
        /// 收缩按钮
        /// </summary>
        private readonly SplitterVerticalButton _splitterVerticalButton = new SplitterVerticalButton();
        public SplitterVerticalButton SplitterVerticalButton
        {
            get { return _splitterVerticalButton; }
        }

        #endregion

    }
}
