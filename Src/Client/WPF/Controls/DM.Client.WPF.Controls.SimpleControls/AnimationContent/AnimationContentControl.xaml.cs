using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace DM.Client.WPF.Controls.SimpleControls.AnimationContent
{
    /// <summary>
    /// AnimationContentControl.xaml 的交互逻辑
    /// </summary>
    public partial class AnimationContentControl : ContentControl
    {

        #region Fields

        /// <summary>
        /// 具有动画切换的功能，这是个依赖属性
        /// </summary>
        private static readonly DependencyProperty AnimationContentProperty = DependencyProperty.Register("AnimationContent", typeof(FrameworkElement),
            typeof(AnimationContentControl), new PropertyMetadata(OnAnimationContentPropertyChangedCallBack));

        /// <summary>
        /// 动画类型，这是个依赖属性
        /// </summary>
        private static readonly DependencyProperty AnimationTypeProperty = DependencyProperty.Register("AnimationType", typeof(AnimationContentType),
            typeof(AnimationContentControl), new PropertyMetadata(AnimationContentType.Shadow));
        
        /// <summary>
        /// 动画集合
        /// </summary>
        private readonly Dictionary<AnimationContentType, Storyboard> _storys = new Dictionary<AnimationContentType, Storyboard>();

        /// <summary>
        /// 当前动画
        /// </summary>
        private Storyboard _currentStory;

        /// <summary>
        /// 向左移动最大值（源对象）
        /// </summary>
        private EasingDoubleKeyFrame _moveLeftSourceX;

        /// <summary>
        /// 向左移动最大值（目标对象）
        /// </summary>
        private EasingDoubleKeyFrame _moveLeftTargetX;

        /// <summary>
        /// 向右移动最大值（源对象）
        /// </summary>
        private EasingDoubleKeyFrame _moveRightSourceX;

        /// <summary>
        /// 向右移动最大值（目标对象）
        /// </summary>
        private EasingDoubleKeyFrame _moveRightTargetX;

        /// <summary>
        /// 动画是否完成(false代表动画没有启动或已经完成, true代表动画已经启动)
        /// </summary>
        private bool _isStartAnimation;

        #endregion

        #region Constructors

        ///<summary>
        /// 构造函数
        ///</summary>
        public AnimationContentControl()
        {
            InitializeComponent();
            _currentStory = (Storyboard)Resources["storyShadow"];
            _storys.Add(AnimationType, _currentStory);
            _currentStory.Completed += CurrentStoryCompleted;
            SizeChanged += AnimationContentControlSizeChanged;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 动画切换属性改变事件
        /// </summary>
        /// <param name="eventArgs"></param>
        private void OnAnimationContentChanged(DependencyPropertyChangedEventArgs eventArgs)
        {
            var newUserControl = (FrameworkElement)eventArgs.NewValue;
            var oldUserControl = (FrameworkElement)eventArgs.OldValue;
            if (oldUserControl == null)
            {
                MainContent.Content = eventArgs.NewValue;
            }
            else
            {
                var visualBrushSource = new VisualBrush(oldUserControl) { Stretch = Stretch.None };
                MainRectSource.Fill = visualBrushSource;
                MainRectSource.Visibility = Visibility.Visible;

                var visualBrushTarget = new VisualBrush(newUserControl) { Stretch = Stretch.None };
                MainRectTarget.Fill = visualBrushTarget;
                MainRectTarget.Visibility = Visibility.Visible;

                MainContent.Content = eventArgs.NewValue;
                MainContent.Visibility = Visibility.Collapsed;

                //启动动画
                StartBoard();
            }
        }

        /// <summary>
        /// 启动动画
        /// </summary>
        private void StartBoard()
        {
            if (_isStartAnimation)
            {
                _currentStory.Stop();
                Reset();
            }
            _isStartAnimation = true;
            if (_storys.ContainsKey(AnimationType))
                _currentStory = _storys[AnimationType];
            else
                AddStory();
            _currentStory.Begin();
        }

        /// <summary>
        /// 添加动画
        /// </summary>
        private void AddStory()
        {
            switch (AnimationType)
            {
                case AnimationContentType.Shadow:
                    _currentStory = (Storyboard)Resources["storyShadow"];
                    break;
                case AnimationContentType.MoveLeft:
                    _currentStory = (Storyboard)Resources["storyMoveLeft"];
                    var moveLeftSX = _currentStory.Children[0] as DoubleAnimationUsingKeyFrames;
                    if (moveLeftSX != null)
                    {
                        _moveLeftSourceX = moveLeftSX.KeyFrames[1] as EasingDoubleKeyFrame;
                        if (_moveLeftSourceX != null)
                            _moveLeftSourceX.Value = -RenderSize.Width;
                    }
                    var moveLeftTX = _currentStory.Children[1] as DoubleAnimationUsingKeyFrames;
                    if (moveLeftTX != null)
                    {
                        _moveLeftTargetX = moveLeftTX.KeyFrames[0] as EasingDoubleKeyFrame;
                        if (_moveLeftTargetX != null)
                            _moveLeftTargetX.Value = RenderSize.Width;
                    }
                    break;
                case AnimationContentType.MoveRight:
                    _currentStory = (Storyboard)Resources["storyMoveRight"];
                    var moveRightSX = _currentStory.Children[0] as DoubleAnimationUsingKeyFrames;
                    if (moveRightSX != null)
                    {
                        _moveRightSourceX = moveRightSX.KeyFrames[1] as EasingDoubleKeyFrame;
                        if (_moveRightSourceX != null)
                            _moveRightSourceX.Value = RenderSize.Width;
                    }
                    var moveRightTX = _currentStory.Children[1] as DoubleAnimationUsingKeyFrames;
                    if (moveRightTX != null)
                    {
                        _moveRightTargetX = moveRightTX.KeyFrames[0] as EasingDoubleKeyFrame;
                        if (_moveRightTargetX != null)
                            _moveRightTargetX.Value = -RenderSize.Width;
                    }
                    break;
            }
            _currentStory.Completed += CurrentStoryCompleted;
            _storys.Add(AnimationType, _currentStory);
        }

        /// <summary>
        /// 重置
        /// </summary>
        private void Reset()
        {
            MainRectSource.Visibility = Visibility.Collapsed;
            MainRectSource.Opacity = 1;
            MainRectTarget.Visibility = Visibility.Collapsed;
            MainContent.Visibility = Visibility.Visible;
        }

        #endregion

        #region Events

        /// <summary>
        /// 动画完成时触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CurrentStoryCompleted(object sender, EventArgs e)
        {
            _isStartAnimation = false;
            Reset();
        }

        /// <summary>
        /// 大小改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AnimationContentControlSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.WidthChanged)
            {
                MainGrid.Clip = new RectangleGeometry { Rect = new Rect(0, 0, RenderSize.Width, RenderSize.Height) };
                if (_moveLeftSourceX != null)
                    _moveLeftSourceX.Value = -RenderSize.Width;
                if (_moveLeftTargetX != null)
                    _moveLeftTargetX.Value = RenderSize.Width;
                if (_moveRightSourceX != null)
                    _moveRightSourceX.Value = RenderSize.Width;
                if (_moveRightTargetX != null)
                    _moveRightTargetX.Value = -RenderSize.Width;
            }
        }

        private static void OnAnimationContentPropertyChangedCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AnimationContentControl)d).OnAnimationContentChanged(e);
        }

        #endregion

        #region Properties

        /// <summary>
        /// 动画切换功能，这是个依赖属性
        /// </summary>
        public FrameworkElement AnimationContent
        {
            set { SetValue(AnimationContentProperty, value); }
            get { return (FrameworkElement)GetValue(AnimationContentProperty); }
        }

        /// <summary>
        /// 动画类型，这是个依赖属性
        /// </summary>
        public AnimationContentType AnimationType
        {
            set { SetValue(AnimationTypeProperty, value); }
            get { return (AnimationContentType)GetValue(AnimationTypeProperty); }
        }

        #endregion

    }
}
