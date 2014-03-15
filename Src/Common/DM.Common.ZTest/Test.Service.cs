using System;
using DM.Common.Extensions;
using DM.Common.Service;
using DM.Common.Service.Entities;
using DM.Common.Service.Factories;
using DM.Common.Service.Helpers;
using DM.Common.Service.Interfaces;
using DM.Common.Utility.Log;

namespace DM.Common.ZTest
{
    public class ServiceTest : ITest
    {
        private readonly string _url;

        public ServiceTest()
        {
            _url = string.Format("http://{0}:{1}", ConfigHelper.GetServerIp(), "7777");
            string serviceUrl = string.Format("{0}/Service", _url);
            CommandServiceHelper.PublishService<CommandDispatcher>(serviceUrl);
        }

        public void Test()
        {
            try
            {
                var responseEntity = CommandServiceHelper.Request<UserInfo, AddressInfo>(_url,"CommandService/GetAddress", new UserInfo { Name = "XLZ" });
                if (responseEntity.IsSuccess)
                {
                    if (responseEntity.Result != null)
                    {
                    }
                }
                for (var i = 0; i < 100; i++)
                {
                    CommandServiceHelper.RequestAsync<UserInfo, AddressInfo>(_url,"CommandService/GetAddress",
                    new UserInfo { Name = "XLZ" + i }, e =>
                    {
                        if (e.IsSuccess)
                        {
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("Error", ex.Message);
            }
        }
    }

    public class CommandDispatcher : DispatcherFactory
    {
        public CommandDispatcher()
        {
            DispatcherHelper.AddCommandExecutor(AllExecutors, new GetAddress());
        }
    }

    public class GetAddress : ICommandExecutor
    {
        public void Execute(RequestModel rm, ResponseModel resM)
        {
            //1.得到参数
            var userInfo = rm.Parameters.ToObject<UserInfo>();

            var addressInfo = new AddressInfo { Address = "Fuck you, " + userInfo.Name };

            //3.返回结果
            resM.State = ResponseStateDefine.Success;
            resM.ResultData = addressInfo.ToJson();
        }

        public string GetCommandName()
        {
            return "CommandService/GetAddress";
        }
    }

    public class UserInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class AddressInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }
}