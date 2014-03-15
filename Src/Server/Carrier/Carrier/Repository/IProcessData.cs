using System.Collections.ObjectModel;
using Carrier.Entities;

namespace Carrier.Repository
{
    /// <summary>
    ///   应用服务存储接口
    /// </summary>
    public interface IProcessData
    {
        /// <summary>
        ///   获取服务列表
        /// </summary>
        /// <returns></returns>
        ObservableCollection<ExecuteFile> GetApplications();

        /// <summary>
        ///   添加应用服务
        /// </summary>
        /// <param name = "path">应用路径</param>
        void AddApplication(string path);

        /// <summary>
        ///   删除应用服务
        /// </summary>
        /// <param name = "exefile"></param>
        void RemoveApplication(ExecuteFile exefile);
    }
}