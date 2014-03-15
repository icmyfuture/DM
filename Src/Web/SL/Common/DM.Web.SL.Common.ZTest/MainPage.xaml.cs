using System.Windows;
using DM.Web.SL.Common.Service;

namespace DM.Web.SL.Common.ZTest
{
    public partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void TestClick1(object sender, RoutedEventArgs e)
        {
            ServiceUrls.GlobalServerUrl = "http://172.16.136.8:9999";

            CommandServiceHelper.Request<string>(ServiceUrls.GlobalServerUrl, "GlobalService/GetClientIP", entity =>
            {
                if (entity.IsSuccess)
                {
                    var value = entity.Result;
                    if (value != null)
                    {
                        Test.Content = value;
                    }
                }
            });
        }
    }

    public static class ServiceUrls
    {
        public static string GlobalServerUrl { get; set; }
    }
}
