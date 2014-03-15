using DM.Common.Service.Factories;
using DM.Common.Service.Helpers;
using DM.Common.Service.Services;
using DM.Common.WPF;

namespace DM.Server.GlobalServer
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App
    {
        public App()
        {
            AppHelper.Instance.Attach(this, LoadAction, true);
        }

        private void LoadAction()
        {
            var serviceUrl = string.Format("http://{0}:{1}/Service", ConfigHelper.GetServerIp(), ConfigHelper.GetAppSetting("Port"));
            new CommandServiceLauncherFactory<CommandService<GlobalDispatcher>>(serviceUrl).Launch();
        }
    }
}
