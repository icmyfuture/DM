using System;
using System.Collections.Generic;
using DM.Common.Service.Entities;
using DM.Common.Service.Helpers;
using DM.Common.Service.Interfaces;
using DM.Common.Utility.Log;

namespace DM.Common.Service.Factories
{
    public class DispatcherFactory : IRequestDispatcher
    {
        protected Dictionary<string, ICommandExecutor> AllExecutors = new Dictionary<string, ICommandExecutor>();

        public ResponseModel Dispatch(RequestModel request)
        {
            var responseModel = new ResponseModel();
            try
            {
                //1. 找出命令执行器
                if (AllExecutors.ContainsKey(request.CommandName))
                {
                    //3. 执行命令
                    AllExecutors[request.CommandName].Execute(request, responseModel);
                }
                else
                {
                    responseModel.ResultData = string.Format("没有找到请求命令{0}所对应的执行器!", request.CommandName);
                    responseModel.State = ResponseStateDefine.Failed;//失败
                }

                return responseModel;
            }
            catch (Exception ex)
            {
                LogHelper.Fatal(ConfigHelper.ModuleName, ex);

                responseModel.State = ResponseStateDefine.Failed;//失败
                responseModel.ResultData = "处理异常，详情见服务端日志!";

                return responseModel;
            }
        }
    }
}