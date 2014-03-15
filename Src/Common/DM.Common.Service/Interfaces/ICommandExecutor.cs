using DM.Common.Service.Entities;

namespace DM.Common.Service.Interfaces
{
    public interface ICommandExecutor
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        void Execute(RequestModel request, ResponseModel response);

        /// <summary>
        /// 取得命令名称
        /// </summary>
        /// <returns></returns>
        string GetCommandName();
    }
}