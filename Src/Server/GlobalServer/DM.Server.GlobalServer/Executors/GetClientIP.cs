using DM.Common.Extensions;
using DM.Common.Service.Entities;
using DM.Common.Service.Interfaces;

namespace DM.Server.GlobalServer.Executors
{
    public class GetClientIP : ICommandExecutor
    {
        public void Execute(RequestModel rm, ResponseModel resM)
        {
            resM.State = ResponseStateDefine.Success;
            resM.ResultData = rm.UserIp.ToJson();
        }

        public string GetCommandName()
        {
            return "GlobalService/GetClientIP";
        }
    }
}