using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace DM.Client.WPF.Controls.MarqueeBar
{
    /// <summary>
    /// Interaction logic for MarqueeBar.xaml
    /// </summary>
    public partial class MarqueeBar
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MarqueeBar()
        {
            InitializeComponent();

            DependencyPropertyDescriptor textDescr = DependencyPropertyDescriptor.
    FromProperty(MarqueeContentProperty, typeof(MarqueeBar));
            textDescr.AddValueChanged(this, (s, e) => Start());

        }

        /// <summary>
        /// 用于显示的内容
        /// </summary>
        public string MarqueeContent
        {
            get { return (string)GetValue(MarqueeContentProperty); }
            set { SetValue(MarqueeContentProperty, value); }
        }

        /// <summary>
        /// 用于显示的内容
        /// </summary>
        public static readonly DependencyProperty MarqueeContentProperty =
            DependencyProperty.Register("MarqueeContent", typeof(string), typeof(MarqueeBar), new UIPropertyMetadata(String.Empty));


        /// <summary>
        /// 初始化显示区域
        /// </summary>
        private void InitializingShowRange()
        {
            var geometryGroup = new GeometryGroup();
            var combinedGeometry = new RectangleGeometry
                {Rect = new Rect(5, 5, ActualWidth - 5, ActualHeight), RadiusX = 0, RadiusY = 0};
            geometryGroup.Children.Add(combinedGeometry);
            Clip = geometryGroup;
            
        }



        private void Start()
        {
            InitializingShowRange();
            tbCotent.Text = MarqueeContent;
            CeaterAnimation(tbCotent);
        }

        /// <summary>
        /// 创建动画
        /// </summary>
        /// <param name="text"></param>
        private void CeaterAnimation(TextBlock text)
        {
            //创建动画资源
            var storyboard = new Storyboard();
            double lenth = StringLengthHelper.MeasureTextWidth(text);
            if (lenth <= ActualWidth - 5)
                /*
                 * 2011年11月23日 11:32:56 xiangmaojun 如果内容长度小于
                 */
                return;
            //移动动画
            {
                var widthMove = new DoubleAnimationUsingKeyFrames();
                Storyboard.SetTarget(widthMove, text);
                var propertyChain = new object[]
                {
                    RenderTransformProperty,
                    TransformGroup.ChildrenProperty,
                    TranslateTransform.XProperty
                };
                Storyboard.SetTargetProperty(widthMove, new PropertyPath("(0).(1)[3].(2)", propertyChain));//设置动画类型
                widthMove.KeyFrames.Add(new EasingDoubleKeyFrame(Width, KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0))));//添加时间线
                widthMove.KeyFrames.Add(new EasingDoubleKeyFrame(-lenth, KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0, (int)(lenth / 60)))));
                storyboard.Children.Add(widthMove);
            }
            storyboard.RepeatBehavior = RepeatBehavior.Forever;
            storyboard.Begin();
        }
    }
}
