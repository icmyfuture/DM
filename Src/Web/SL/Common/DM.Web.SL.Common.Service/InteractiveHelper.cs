using System;
using DM.Web.SL.Common.Extensions;
using DM.Web.SL.Common.Service.Entities;

namespace DM.Web.SL.Common.Service
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
                return new ResponseModel { ResultData = "resM is null or empty.", State = ResponseStateDefine.Failed };
            }
            catch (Exception ex)
            {
                return new ResponseModel { ResultData = ex.ToString(), State = ResponseStateDefine.Failed };
            }
        }

        #endregion
    }
}