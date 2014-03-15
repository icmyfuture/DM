using Carrier.Entities;

namespace Carrier.Repository
{
    /// <summary>
    ///   路径缓存接口
    /// </summary>
    public interface IUrlData
    {
        /// <summary>
        ///   获取路径
        /// </summary>
        /// <param name = "filepath">程序名，做KEY用</param>
        /// <returns></returns>
        ServiceUrl GetServiceUrl(string filepath);

        /// <summary>
        ///   保存路径
        /// </summary>
        /// <param name = "url"></param>
        void SaveServiceUrl(ServiceUrl url);

        /// <summary>
        ///   删除路径
        /// </summary>
        /// <param name = "url"></param>
        void RemoveServiceUrl(ServiceUrl url);
    }
}