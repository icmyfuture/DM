#region Region

using System.Windows;
using System.Windows.Controls;

#endregion

namespace DM.Client.WPF.Controls.SimpleControls.Logins
{
    /// <summary>
    /// RotationalEllipse.xaml 的交互逻辑
    /// </summary>
    public partial class RotationalEllipse : UserControl
    {

        #region Fields

        /// <summary>
        /// 缩放X坐标系比例
        /// </summary>
        private static readonly DependencyProperty ZoomXProperty = DependencyProperty.Register("ZoomX", typeof(double), typeof(RotationalEllipse),
            new PropertyMetadata(1d, ZoomXPropertyChangedCallBack));

        private static readonly DependencyProperty ZoomYProperty = DependencyProperty.Register("ZoomY", typeof(double), typeof(RotationalEllipse),
            new PropertyMetadata(1d, ZoomYPropertyChangedCallBack));

        #endregion

        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        public RotationalEllipse()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        /// <summary>
        /// 缩放比例X变换触发
        /// </summary>
        /// <param name="e"></param>
        private void ZoomXChanged(DependencyPropertyChangedEventArgs e)
        {
            SpinnerScale.ScaleX = double.Parse(e.NewValue.ToString());
        }

        /// <summary>
        /// 缩放比例Y变换触发
        /// </summary>
        /// <param name="e"></param>
        private void ZoomYChanged(DependencyPropertyChangedEventArgs e)
        {
            SpinnerScale.ScaleY = double.Parse(e.NewValue.ToString());
        }

        #endregion

        #region Events

        /// <summary>
        /// 缩放比例X变换回调事件
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void ZoomXPropertyChangedCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((RotationalEllipse)d).ZoomXChanged(e);
        }

        /// <summary>
        /// 缩放比例Y变换回调事件
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void ZoomYPropertyChangedCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((RotationalEllipse)d).ZoomYChanged(e);
        }

        #endregion

        #region Properties

        /// <summary>
        /// 缩放X坐标系比例, 这是一个依赖属性
        /// </summary>
        public double ZoomX
        {
            set { SetValue(ZoomXProperty, value); }
            get { return (double)GetValue(ZoomXProperty); }
        }

        /// <summary>
        /// 缩放Y坐标系比例, 这是一个依赖属性
        /// </summary>
        public double ZoomY
        {
            set { SetValue(ZoomYProperty, value); }
            get { return (double)GetValue(ZoomYProperty); }
        }

        #endregion

    }
}
