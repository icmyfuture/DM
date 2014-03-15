using System.Collections.Generic;

namespace Carrier.Repository
{
    /// <summary>
    ///   通用存储
    /// </summary>
    /// <typeparam name = "T"></typeparam>
    internal interface ILocalStorage<T>
    {
        /// <summary>
        ///   读取列表
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> GetList();

        /// <summary>
        ///   保存项
        /// </summary>
        /// <param name = "item"></param>
        void Save(T item);

        /// <summary>
        ///   删除项
        /// </summary>
        /// <param name = "item"></param>
        void Remove(T item);
    }
}