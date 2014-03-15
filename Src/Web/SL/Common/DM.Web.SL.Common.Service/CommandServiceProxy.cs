using System;
using System.ServiceModel;
using DM.Web.SL.Common.Service.Entities;

namespace DM.Web.SL.Common.Service
{
    /// <summary>
    /// 命令服务客户端代理基类
    /// </summary>
    public class CommandServiceProxy
    {
        #region 属性变量

        private readonly string _serviceUrl;

        #endregion

        /// <summary>
        /// 构造函数，设置ServiceUrl
        /// </summary>
        /// <param name="serviceUrl"></param>
        public CommandServiceProxy(string serviceUrl)
        {
            _serviceUrl = serviceUrl;
        }

        #region 调用

        public void InvokeAsync(string request, EventHandler callBack)
        {
            try
            {
                if (string.IsNullOrEmpty(_serviceUrl)) throw new Exception("Set the ServiceUrl before invoking.");
                var proxy = CreateCommandServiceClient();
                proxy.InvokeAsync(request);
                proxy.InvokeCompleted += (s, e) =>
                    {
                        if (callBack != null)
                        {
                            try
                            {
                                callBack(InteractiveHelper.BuildResponse(e.Result), e);
                            }
                            catch (Exception ex)
                            {
                                callBack(new ResponseModel { ResultData = ex.ToString(), State = ResponseStateDefine.Failed }, e);
                            }
                            finally
                            {
                                proxy.CloseAsync();
                            }
                        }
                    };
            }
            catch (Exception ex)
            {
                if (callBack != null)
                {
                    callBack(new ResponseModel { ResultData = ex.ToString(), State = ResponseStateDefine.Failed }, null);
                }
            }
        }

        #endregion

        protected CommandServiceClient CreateCommandServiceClient()
        {
            CommandServiceClient proxy = null;
            if (!string.IsNullOrEmpty(_serviceUrl))
            {
                proxy = new CommandServiceClient(new BasicHttpBinding
                {
                    MaxReceivedMessageSize = 1024 * 1024 * 10,
                    ReceiveTimeout = new TimeSpan(0, 10, 0),
                    SendTimeout = new TimeSpan(0, 10, 0)
                }, new EndpointAddress(string.Format("{0}/Service", _serviceUrl)));
            }
            return proxy;
        }
    }
}