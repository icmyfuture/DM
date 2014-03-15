using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DM.Web.SL.Controls.LoadingControl
{
    public partial class LoadingControl : UserControl,IDisposable
    {
        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        public LoadingControl()
        {
            InitializeComponent();
            Loaded += ( sender, e ) =>
            {
                loading.Begin();
            };
            //txtloadMsg.Text = LanguageHelper.GetDictionary( "DM.Common.Controls", "txtLoading", "数据加载中..." );
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="msg">显示消息</param>
        public LoadingControl( string msg )
            : this()
        {
            txtloadMsg.Text = msg;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="bgColor">背景色</param>
        public LoadingControl( Color bgColor )
            : this()
        {
            mainGrid.Background = new SolidColorBrush( bgColor );
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// 背景色
        /// </summary>
        public Color BgColor
        {
            get { return ( (SolidColorBrush)mainGrid.Background ).Color; }
            set { mainGrid.Background = new SolidColorBrush( value ); }
        }

        /// <summary>
        /// 显示信息
        /// </summary>
        public string ShowMsg
        {
            get { return (string)GetValue(ShowMsgProperty); }
            set { SetValue(ShowMsgProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowMsgProperty =
            DependencyProperty.Register("ShowMsg", typeof(string), typeof(LoadingControl), new PropertyMetadata(ShowMsgPropertyCallBack));
        private static void ShowMsgPropertyCallBack(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            LoadingControl ctr = obj as LoadingControl;
            ctr.txtloadMsg.Text = (string)e.NewValue;
        }

        #endregion Properties

        #region IDisposable Members

        public void Dispose()
        {
            Loaded -= ( sender, e ) =>
            {
                loading.Begin();
            };
            loading.Stop();
            loading = null;
        }

        #endregion
    }
}