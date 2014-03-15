using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace DM.Client.WPF.Controls.Loading
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class LoadingControl
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public LoadingControl()
        {
            InitializeComponent();
            Loaded += (sender, e) => BeginStory();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="msg">显示消息</param>
        public LoadingControl(string msg): this()
        {
            txtloadMsg.Text = msg;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="bgColor">背景色</param>
        public LoadingControl(Color bgColor): this()
        {
            mainGrid.Background = new SolidColorBrush(bgColor);
        }

        /// <summary>
        /// 背景色
        /// </summary>
        public Color BgColor
        {
            get { return ((SolidColorBrush)mainGrid.Background).Color; }
            set { mainGrid.Background = new SolidColorBrush(value); }
        }

        private bool _isShowMsg;
        /// <summary>
        /// 是否显示消息
        /// </summary>
        public bool IsShowMsg
        {
            get { return _isShowMsg; }
            set
            {
                _isShowMsg = value;
                txtloadMsg.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        /// <summary>
        /// 显示信息
        /// </summary>
        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(LoadingControl), new PropertyMetadata(MessagePropertyCallBack));
        private static void MessagePropertyCallBack(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var ctr = obj as LoadingControl;
            if (ctr != null) ctr.txtloadMsg.Text = (string)e.NewValue;
        }

        /// <summary>
        /// 停止story
        /// </summary>
        private void StopStory()
        {
            var loading = (Storyboard)FindResource("loading");
            if (loading != null)
            {
                loading.Stop();
            }
        }
        
        /// <summary>
        /// 开始story
        /// </summary>
        private void BeginStory()
        {
            var loading = (Storyboard)FindResource("loading");
            if (loading != null)
            {
                loading.Begin();
            }
        }

        public void Dispose()
        {
            StopStory();
        }
    }
}
