using DM.Common.Config;
using DM.Common.Extensions;
using DM.Common.Service.Entities;
using DM.Common.Service.Interfaces;

namespace DM.Server.GlobalServer.Executors
{
    public class GetServerIP : ICommandExecutor
    {
        public void Execute(RequestModel rm, ResponseModel resM)
        {
            var key = rm.Parameters.ToObject<string>();
            var ipDictionary = Configuration.GloabalIpConfigs;
            resM.State = ResponseStateDefine.Success;
            if (ipDictionary != null)
            {
                resM.ResultData = ipDictionary[string.Format(@"{{{0}}}", key)].ToJson();
            }
        }

        public string GetCommandName()
        {
            return "GlobalService/GetServerIP";
        }
    }
}