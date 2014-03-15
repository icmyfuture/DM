using DM.Client.WPF.Controls.MessageBox;
using DM.Client.WPF.Controls.Styles;
using DM.Common.WPF;
using MvvmLight.Threading;

namespace DM.Client.WPF.Sample
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App
    {
        public App()
        {
            AppHelper.Instance.Attach(this, LoadAction);
        }

        private void LoadAction()
        {
            StyleManager.AddStyles();

            //UI线程初始化
            DispatcherHelper.Initialize();
            //注册对话框消息
            MessageBoxManage.Instance.Register();
        }
    }
}
