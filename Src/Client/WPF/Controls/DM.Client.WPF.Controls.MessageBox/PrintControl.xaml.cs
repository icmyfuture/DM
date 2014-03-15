using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;
using DM.Common.Utility;

namespace DM.Client.WPF.Controls.MessageBox
{
    /// <summary>
    /// PrintControl.xaml 的交互逻辑
    /// </summary>
    public partial class PrintControl
    {

        #region Fields

        /// <summary>
        /// 动态构造的UI
        /// </summary>
        private readonly object _printUi;

        /// <summary>
        /// 标题
        /// </summary>
        private readonly string _title;

        #endregion

        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="xaml">xaml字符串</param>
        public PrintControl(string title, string xaml)
        {
            InitializeComponent();
            Loaded += PrintControlLoaded;
            btnPrint.Content = LanguageHelper.GetDictionary("PrintTitle", "Print");
            Topmost = false;
            _title = title;
            var xamlReader = new XamlReader();
            xamlReader.LoadCompleted += XamlReaderLoadCompleted;
            _printUi = xamlReader.LoadAsync(new XmlTextReader(new StringReader(xaml)));
        }

        #endregion

        #region Methods

        /// <summary>  
        /// UI 打印  
        /// </summary>  
        /// <param name="description">打印的文档名称</param>  
        /// <param name="ele">要打印的UI</param>  
        public static void ChartPrint(FrameworkElement ele, string description)
        {
            //打印函数
            var pd = new PrintDialog();
            //弹出打印选项对话框
            bool result = pd.ShowDialog().GetValueOrDefault();
            if (result)
            {
                var renderBitmap = new RenderTargetBitmap((int)ele.ActualWidth * 4,
                                                                         (int)ele.ActualHeight * 4, 96 * 4, 96 * 4,
                                                                         PixelFormats.Pbgra32);
                //使用 RenderTargetBitmap 保存图片
                renderBitmap.Render(ele);
                //使用图片作为载体显示UI
                var image = new Image
                {
                    Width = pd.PrintableAreaWidth + 45,
                    Height = ele.ActualHeight,
                    StretchDirection = StretchDirection.DownOnly,
                    Stretch = Stretch.Uniform,
                    Source = renderBitmap
                };

                // 设置图片位置并显示.  
                var canvas = new Canvas();
                Canvas.SetLeft(image, 0);
                Canvas.SetTop(image, 10);
                canvas.Children.Add(image);
                var w = new Window
                    {
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Content = canvas,
                        Left = -10000,
                        Top = -10000,
                        WindowStartupLocation = WindowStartupLocation.CenterScreen,
                        Owner = (Window) ele,
                    };

                w.Show();
                w.Close();

                //打印图片
                pd.PrintVisual(image, description);
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrintControlLoaded(object sender, RoutedEventArgs e)
        {
            Height = Application.Current.MainWindow.ActualHeight - 100;
            UpdateLayout();
        }

        /// <summary>
        /// 异步加载完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void XamlReaderLoadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                PreviewControl.Content = _printUi;
                PreviewControl.UpdateLayout();
            }
        }

        /// <summary>
        /// 打印事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPrintClick(object sender, RoutedEventArgs e)
        {
            ChartPrint(PreviewControl, _title);
        }

        #endregion

    }
}
