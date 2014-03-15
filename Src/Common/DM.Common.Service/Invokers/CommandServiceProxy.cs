using System;
using System.ServiceModel;
using System.Xml;
using DM.Common.Service.Entities;
using DM.Common.Service.Helpers;
using DM.Common.Service.Interfaces;
using DM.Common.Utility.Log;

namespace DM.Common.Service.Invokers
{
    /// <summary>
    /// 命令服务客户端代理基类
    /// </summary>
    public class CommandServiceProxy : IServiceProxy
    {
        #region 属性变量

        private readonly CommandServiceClient _proxy;
        private readonly string _serviceUrl;

        #endregion

        public CommandServiceProxy(string serviceUrl)
        {
            try
            {
                _serviceUrl = serviceUrl;
                _proxy = CreateCommandServiceClient();
            }
            catch (Exception ex)
            {
                LogHelper.Fatal(ConfigHelper.ModuleName, "CommandServiceProxy Init failed!", ex);
            }
        }

        #region 初始化通信信道

        private CommandServiceClient CreateCommandServiceClient()
        {
            CommandServiceClient proxy = null;
            if (!string.IsNullOrEmpty(_serviceUrl))
            {
                proxy = new CommandServiceClient(new BasicHttpBinding
                    {
                        ReaderQuotas = new XmlDictionaryReaderQuotas { MaxStringContentLength = 1024 * 1024 * 10 },
                        MaxReceivedMessageSize = 1024 * 1024 * 10,
                        ReceiveTimeout = new TimeSpan(0, 10, 0),
                        SendTimeout = new TimeSpan(0, 10, 0)
                    }, new EndpointAddress(string.Format("{0}/Service", _serviceUrl)));
            }
            else
            {
                LogHelper.Fatal(ConfigHelper.ModuleName, "服务地址不能为空!");
            }
            return proxy;
        }

        #endregion

        #region 调用

        public ResponseModel Invoke(string request)
        {
            return InteractiveHelper.BuildResponse(_proxy.Invoke(request));
        }

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
                            proxy.Close();
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
    }
}