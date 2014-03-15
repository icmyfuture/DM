using DM.Common.Service.Factories;
using DM.Common.Service.Helpers;
using DM.Server.GlobalServer.Executors;

namespace DM.Server.GlobalServer
{
    public class GlobalDispatcher : DispatcherFactory
    {
        public GlobalDispatcher()
        {
            DispatcherHelper.AddCommandExecutor(AllExecutors, new GetClientIP());
            DispatcherHelper.AddCommandExecutor(AllExecutors, new GetServerIP());
        }
    }
}