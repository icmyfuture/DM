using DM.Common.Service.Entities;

namespace DM.Common.Service.Interfaces
{
    public interface IRequestDispatcher
    {
        /// <summary>
        /// 分发请求
        /// </summary>
        /// <param name="request">请求模型</param>
        /// <returns>请求结果</returns>
        ResponseModel Dispatch(RequestModel request); 
    }
}