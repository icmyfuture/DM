using DM.Common.Service.Interfaces;

namespace DM.Common.Service.Factories
{
    /// <summary>
    /// 命令服务启动工厂
    /// </summary>
    /// <typeparam name="TCommandService">命令服务</typeparam>
    public class CommandServiceLauncherFactory<TCommandService> : ServiceLauncherFactory<TCommandService, ICommandService> where TCommandService : ICommandService
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="toPublishServiceUrl">待发布地址</param>
        public CommandServiceLauncherFactory(string toPublishServiceUrl)
            : base(toPublishServiceUrl)
        {
        }
    }
}