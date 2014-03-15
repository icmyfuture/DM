using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Carrier.Entities;
using Carrier.Utility;
using Carrier.ViewModel;
using DM.Common.Extensions;

namespace Carrier.Repository
{
    ///<summary>
    ///  应用缓存
    ///</summary>
    internal sealed class LocalStorage : IProcessData, IUrlData
    {
        private const string StorageFile = "ProcessData.config";

        ///<summary>
        ///  应用缓存实例
        ///</summary>
        public static readonly LocalStorage ProcessData = new LocalStorage();

        #region IProcessData Members

        /// <summary>
        ///   获取服务列表
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<ExecuteFile> GetApplications()
        {
            var cache = new List<ServiceUrl>();
            var result = new ObservableCollection<ExecuteFile>();
            if (File.Exists(StorageFile))
            {
                string resultstr = File.ReadAllText(StorageFile);
                cache = resultstr.ToObjectFromXml<List<ServiceUrl>>().
                    OrderBy(u => Path.GetFileNameWithoutExtension(u.FilePath)).
                    ToList();
            }
            cache.OrderBy(s => s.StartIndex).ToList().ForEach(u =>
                {
                    if (!string.IsNullOrEmpty(u.FilePath))
                    {
                        ExecuteFile ef = u.FilePath.ToExe(u);

                        if (ef != null)
                        {
                            result.Add(ef);
                        }
                    }
                });
            return result;
        }

        /// <summary>
        ///   添加应用服务
        /// </summary>
        /// <param name = "path">应用路径</param>
        public void AddApplication(string path)
        {
            string guid = Guid.NewGuid().ToString();
            var serviceurl = new ServiceUrl
                {
                    FilePath = path,
                    ID = guid,
                    Port = "0000",
                    Path = "Service"
                };
            ExecuteFile exefile = path.ToExe(serviceurl);
            if (File.Exists(StorageFile))
            {
                string resultstr = File.ReadAllText(StorageFile);
                var result = resultstr.ToObjectFromXml<List<ServiceUrl>>();
                result.Add(serviceurl);
                MoniterViewModel.Instance.FileNames.Add(exefile);
                File.WriteAllText(StorageFile, result.ToXml());
            }
            else
            {
                var newpath = new List<ServiceUrl>
                    {
                        new ServiceUrl()
                    };
                File.WriteAllText(StorageFile, newpath.ToXml());
                MoniterViewModel.Instance.FileNames.Add(exefile);
            }
        }

        /// <summary>
        ///   删除应用服务
        /// </summary>
        /// <param name = "exefile"></param>
        public void RemoveApplication(ExecuteFile exefile)
        {
            if (File.Exists(StorageFile))
            {
                string resultstr = File.ReadAllText(StorageFile);
                var result = resultstr.ToObjectFromXml<List<ServiceUrl>>();
                result.Remove(result.FirstOrDefault(s => !string.IsNullOrEmpty(s.ID) && s.ID.Equals(exefile.ID)));
                File.WriteAllText(StorageFile, result.ToXml());
                MoniterViewModel.Instance.FileNames.Remove(exefile);
            }
            else
            {
                var newpath = new List<string>
                    {
                        exefile.FilePath
                    };
                File.WriteAllText(StorageFile, newpath.ToXml());
            }
        }

        #endregion

        #region IUrlData Members

        /// <summary>
        ///   获取路径
        /// </summary>
        /// <param name = "appid">程序标识</param>
        /// <returns></returns>
        public ServiceUrl GetServiceUrl(string appid)
        {
            if (File.Exists(StorageFile))
            {
                string resultstr = File.ReadAllText(StorageFile);
                var result = resultstr.ToObjectFromXml<List<ServiceUrl>>();
                return result.FirstOrDefault(u => !string.IsNullOrEmpty(u.ID) && u.ID.Equals(appid));
            }
            return new ServiceUrl();
        }

        /// <summary>
        ///   保存路径
        /// </summary>
        /// <param name = "url"></param>
        public void SaveServiceUrl(ServiceUrl url)
        {
            if (File.Exists(StorageFile))
            {
                string resultstr = File.ReadAllText(StorageFile);
                var result = resultstr.ToObjectFromXml<List<ServiceUrl>>();
                var resultindb = result.FirstOrDefault(u => !string.IsNullOrEmpty(u.ID) && u.ID.Equals(url.ID));
                result.Remove(resultindb);
                result.Add(url);
                File.WriteAllText(StorageFile, result.ToXml());
            }
            else
            {
                var newpath = new List<ServiceUrl>
                    {
                        url
                    };
                File.WriteAllText(StorageFile, newpath.ToXml());
            }
        }

        /// <summary>
        ///   删除路径
        /// </summary>
        /// <param name = "url"></param>
        public void RemoveServiceUrl(ServiceUrl url)
        {
            if (!File.Exists(StorageFile))
            {
                return;
            }
            string resultstr = File.ReadAllText(StorageFile);
            var result = resultstr.ToObjectFromXml<List<ServiceUrl>>();
            if (result.Contains(url))
            {
                result.Remove(url);
            }
            File.WriteAllText(StorageFile, result.ToXml());
        }

        #endregion
    }
}