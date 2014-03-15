using System;
using DM.Common.Service.Entities;

namespace DM.Common.Service.Interfaces
{
    public interface IServiceProxy
    {
        /// <summary>
        /// 请求服务
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ResponseModel Invoke(string request);

        /// <summary>
        /// 异步请求服务
        /// </summary>
        /// <param name="reqeust"></param>
        /// <param name="callBack"></param>
        void InvokeAsync(string reqeust, EventHandler callBack);
    }
}