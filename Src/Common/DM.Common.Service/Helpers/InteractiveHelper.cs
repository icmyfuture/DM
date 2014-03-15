using System;
using DM.Common.Extensions;
using DM.Common.Service.Entities;
using DM.Common.Utility.Log;

namespace DM.Common.Service.Helpers
{
    public class InteractiveHelper
    {
        #region 构建请求协议

        public static string BuildRequest(string commandName)
        {
            return BuildRequest(commandName, string.Empty, null);
        }

        public static string BuildRequest(string commandName, string softType)
        {
            return BuildRequest(commandName, softType, null);
        }

        public static string BuildRequestWithParams(string commandName, object parameters)
        {
            return BuildRequest(commandName, string.Empty, parameters);
        }

        public static string BuildRequest(string commandName, string softType, object parameters)
        {
            return new RequestModel
                {
                CommandName = commandName,
                Parameters = (parameters == null ? string.Empty : parameters.ToJson()),
            }.ToCompressedStr();
        }

        #endregion

        #region 构建响应协议

        public static ResponseModel BuildResponse(string resM)
        {
            try
            {
                if (!string.IsNullOrEmpty(resM))
                {
                    //1.反序列化
                    return resM.ToDeCompressedObject<ResponseModel>();
                }
                return new ResponseModel { State = ResponseStateDefine.Failed };
            }
            catch (Exception ex)
            {
                LogHelper.Fatal(ConfigHelper.ModuleName, ex);
                return new ResponseModel { State = ResponseStateDefine.Failed };
            }
        }

        #endregion
    }
}