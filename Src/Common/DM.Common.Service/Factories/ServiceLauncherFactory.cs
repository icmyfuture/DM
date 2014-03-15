using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Xml;
using DM.Common.Service.Helpers;
using DM.Common.Service.Interfaces;
using DM.Common.Service.Services;
using DM.Common.Utility.Log;

namespace DM.Common.Service.Factories
{
    /// <summary>
    /// 服务启动工厂
    /// </summary>
    /// <typeparam name="TServiceProvider">服务提供类</typeparam>
    /// <typeparam name="TServiceInterface">服务接口类</typeparam>
    public class ServiceLauncherFactory<TServiceProvider, TServiceInterface> where TServiceProvider : TServiceInterface
    {
        private readonly string _toPublishServiceUrl;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="toPublishServiceUrl">待发布地址</param>
        public ServiceLauncherFactory(string toPublishServiceUrl)
        {
            _toPublishServiceUrl = toPublishServiceUrl;
        }

        /// <summary>
        ///   启动服务。
        /// </summary>
        public void Launch()
        {
            LogHelper.Info(ConfigHelper.ModuleName, string.Format("正在启动服务:{0}", _toPublishServiceUrl));

            var wsUrl = new Uri(_toPublishServiceUrl);

            //发布SILVERLIGHT授权服务
            LaunchPolicy("http://" + wsUrl.Authority);

            //发布服务
            LaunchService(_toPublishServiceUrl);

            LogHelper.Info(ConfigHelper.ModuleName, string.Format("启动服务成功"));
        }

        /// <summary>
        /// 发布服务
        /// </summary>
        /// <param name="serviceUrl"></param>
        private void LaunchService(string serviceUrl)
        {
            var hostUrl = new Uri(serviceUrl);

            var binding = new BasicHttpBinding
                {
                    MaxBufferSize = 2147483647,
                    MaxReceivedMessageSize = 2147483647,
                    MaxBufferPoolSize = 2147483647,
                    ReaderQuotas = new XmlDictionaryReaderQuotas
                        {
                            MaxDepth = 2147483647,
                            MaxStringContentLength = 2147483647,
                            MaxArrayLength = 2147483647,
                            MaxBytesPerRead = 2147483647,
                            MaxNameTableCharCount = 2147483647
                        }
                };
            var servicehost = new ServiceHost(typeof(TServiceProvider), hostUrl);
            servicehost.AddServiceEndpoint(typeof(TServiceInterface), binding, string.Empty);
            var behavior = new ServiceMetadataBehavior
                {
                    HttpGetEnabled = true,
                    HttpGetUrl = hostUrl
                };

            #region 限流控制

            var throttlingBehavior = servicehost.Description.Behaviors.Find<ServiceThrottlingBehavior>();

            if (null == throttlingBehavior)
            {
                throttlingBehavior = new ServiceThrottlingBehavior();
                servicehost.Description.Behaviors.Add(throttlingBehavior);
            }

            throttlingBehavior.MaxConcurrentCalls = 500;
            throttlingBehavior.MaxConcurrentInstances = 500;
            throttlingBehavior.MaxConcurrentSessions = 500;

            #endregion

            servicehost.Description.Behaviors.Add(behavior);
            servicehost.Open();
        }

        /// <summary>
        /// 发布SILVERLIGHT授权服务
        /// </summary>
        /// <param name="serviceUrl"></param>
        private void LaunchPolicy(string serviceUrl)
        {
            var policyhost = new ServiceHost(typeof(SilverlightService), new Uri(serviceUrl));
            var endpoint = policyhost.AddServiceEndpoint(typeof(ISilverlightService), new WebHttpBinding(), string.Empty);
            endpoint.Behaviors.Add(new WebHttpBehavior());
            policyhost.Open();
        }
    }
}