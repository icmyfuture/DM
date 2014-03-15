#region Import

using System;
using System.Windows;
using System.Windows.Threading;

#endregion

namespace DM.Client.WPF.Controls.SimpleControls.ProgressBar
{
    /// <summary>
    /// BufferProgressBar.xaml 的交互逻辑
    /// </summary>
    public partial class BufferProgressBar : System.Windows.Controls.ProgressBar, IDisposable
    {

        #region Fields

        /// <summary>
        /// 是否启用进度缓冲效果
        /// </summary>
        private static readonly DependencyProperty IsEnabledBufferProperty = DependencyProperty.Register("IsEnabledBuffer", typeof(bool), typeof(BufferProgressBar),
            new PropertyMetadata(false));

        /// <summary>
        /// 缓冲进度条值
        /// </summary>
        private static readonly DependencyProperty BufferValueProperty = DependencyProperty.Register("BufferValue", typeof(double), typeof(BufferProgressBar),
            new PropertyMetadata(0d, BufferValuePropertyChangedCallBack));

        /// <summary>
        /// 缓冲进度线程
        /// </summary>
        private readonly DispatcherTimer _bufferTimer = new DispatcherTimer();

        /// <summary>
        /// 缓冲次数
        /// </summary>
        private const double BufferCount = 20;

        /// <summary>
        /// 缓冲次数变量
        /// </summary>
        private double _bufferCountValue = 0;

        /// <summary>
        /// 上半段进度缓冲比例
        /// </summary>
        private const double UpBufferDistance = 0.6;

        /// <summary>
        /// 上半段进度缓冲时间(秒)
        /// </summary>
        private const double UpBufferTimer = 0.2;

        /// <summary>
        /// 上半段进度缓冲时间
        /// </summary>
        private TimeSpan _upTime;

        /// <summary>
        /// 上半段进度缓冲值
        /// </summary>
        private double _upBufferValue = 0;

        /// <summary>
        /// 下半段进度缓冲比例
        /// </summary>
        private const double DownBufferDistance = 0.35;

        /// <summary>
        /// 下半段进度缓冲时间(秒)
        /// </summary>
        private const double DownBufferTimer = 0.5;

        /// <summary>
        /// 下半段进度缓冲时间
        /// </summary>
        private TimeSpan _downTime;

        /// <summary>
        /// 下半段进度缓冲值
        /// </summary>
        private double _downBufferValue = 0;

        #endregion

        #region Constructors

        ~BufferProgressBar()
        {
            Dispose();
        }

        public BufferProgressBar()
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

            #region 缓冲线程初始化

            _upTime = TimeSpan.FromSeconds(UpBufferTimer);
            _downTime = TimeSpan.FromSeconds(DownBufferTimer);

            _bufferTimer.Interval = _upTime;
            _bufferTimer.Tick += BufferTimerTick;

            #endregion

            #region 进度条相关属性、事件初始化

            ValueChanged += BufferProgressBarValueChanged;

            #endregion

        }

        /// <summary>
        /// 缓冲进度条值改变后触发
        /// </summary>
        /// <param name="e"></param>
        private void BufferValueChanged(DependencyPropertyChangedEventArgs e)
        {
            if (IsEnabledBuffer)
            {
                //缓冲线程停止
                _bufferTimer.Stop();
                //重新赋值
                _bufferCountValue = BufferCount;
                //获取进度值
                var bufferValue = (double)e.NewValue;
                //获取缓冲差
                var bufferRest = bufferValue - Value;
                //计算上半段进度缓冲值
                _upBufferValue = (bufferRest * UpBufferDistance) / BufferCount;
                //计算下半段进度缓冲值
                _downBufferValue = (bufferRest * DownBufferDistance) / BufferCount;
                //缓冲线程开始
                _bufferTimer.Interval = _upTime;
                _bufferTimer.Start();
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// 缓冲进度条值改变回调事件
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void BufferValuePropertyChangedCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((BufferProgressBar)d).BufferValueChanged(e);
        }

        /// <summary>
        /// 进度条值发生改变回调事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BufferProgressBarValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //停止缓冲线程
            if (BufferValue == e.NewValue)
                _bufferTimer.Stop();
        }

        /// <summary>
        /// 缓冲进度条循环事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BufferTimerTick(object sender, EventArgs e)
        {
            //上半段
            if (_bufferTimer.Interval == _upTime)
            {
                Value += _upBufferValue;
                _bufferCountValue--;
                if (_bufferCountValue == 0)
                {
                    _bufferTimer.Stop();
                    _bufferTimer.Interval = _downTime;
                    _bufferCountValue = BufferCount;
                    _bufferTimer.Start();
                }
            }
            //下半段
            else if (_bufferTimer.Interval == _downTime)
            {
                Value += _downBufferValue;
                _bufferCountValue--;
                if (_bufferCountValue == 0)
                {
                    _bufferTimer.Stop();
                }
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// 是否启用进度缓冲效果, 这是一个依赖属性。
        /// </summary>
        public bool IsEnabledBuffer
        {
            set { SetValue(IsEnabledBufferProperty, value); }
            get { return (bool)GetValue(IsEnabledBufferProperty); }
        }

        /// <summary>
        /// 缓冲进度条值, 这是一个依赖属性。
        /// </summary>
        public double BufferValue
        {
            set { SetValue(BufferValueProperty, value); }
            get { return (double)GetValue(BufferValueProperty); }
        }

        #endregion

        #region IDisposable 接口实现

        public void Dispose()
        {
            if (_bufferTimer != null)
                _bufferTimer.Stop();
        }

        #endregion

    }
}
