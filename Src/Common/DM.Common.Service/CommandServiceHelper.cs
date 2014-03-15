using System;
using System.Collections.Generic;
using DM.Common.Extensions;
using DM.Common.Service.Entities;
using DM.Common.Service.Factories;
using DM.Common.Service.Helpers;
using DM.Common.Service.Invokers;
using DM.Common.Service.Services;
using System.Linq;

namespace DM.Common.Service
{
    public static class CommandServiceHelper
    {
        private const string FailedToParseResponseModel = @"Failed to parse ResponseModel.";
        private static readonly Dictionary<string, CommandServiceProxy> ServiceProxyDictionary = new Dictionary<string, CommandServiceProxy>();

        /// <summary>
        /// 发布服务(CommandService)
        /// </summary>
        /// <typeparam name="TDispatcher"></typeparam>
        /// <param name="url">格式：http://172.16.136.8:9999/Service </param>
        public static void PublishService<TDispatcher>(string url) where TDispatcher : DispatcherFactory
        {
            new CommandServiceLauncherFactory<CommandService<TDispatcher>>(url).Launch();
        }

        #region 同步请求

        /// <summary>
        /// 请求：无参数，无返回值
        /// </summary>
        /// <param name="serviceUrl"> 格式: http://172.16.136.8:9999 </param>
        /// <param name="commandName"></param>
        public static void Request(string serviceUrl, string commandName)
        {
            Request<string, string>(serviceUrl, commandName, string.Empty);
        }

        /// <summary>
        /// 请求：有参数，无返回值
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <param name="serviceUrl">格式: http://172.16.136.8:9999 </param>
        /// <param name="commandName"></param>
        /// <param name="request"></param>
        public static void Request<TRequest>(string serviceUrl, string commandName, TRequest request)
        {
            Request<TRequest, string>(serviceUrl, commandName, request);
        }

        /// <summary>
        /// 请求：无参数，有返回值
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="serviceUrl">格式: http://172.16.136.8:9999  </param>
        /// <param name="commandName"></param>
        /// <returns></returns>
        public static ResponseEntity<TResponse> Reqeuest<TResponse>(string serviceUrl, string commandName) where TResponse : class
        {
            return Request<string, TResponse>(serviceUrl, commandName, string.Empty);
        }

        /// <summary>
        /// 请求：有参数，有返回值
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="serviceUrl">格式: http://172.16.136.8:9999  </param>
        /// <param name="commandName"></param>
        /// <param name="request"></param>
        public static ResponseEntity<TResponse> Request<TRequest, TResponse>(string serviceUrl, string commandName, TRequest request) where TResponse : class
        {
            var responseEntity = new ResponseEntity<TResponse>();
            var proxy = ServiceProxyDictionary.Keys.Contains(serviceUrl) ? ServiceProxyDictionary[serviceUrl] : new CommandServiceProxy(serviceUrl);
            var response = proxy.Invoke(InteractiveHelper.BuildRequestWithParams(commandName, request));
            ParseResponseModel(response, responseEntity);
            return responseEntity;
        }

        #endregion

        #region 异步请求

        /// <summary>
        /// 请求：无参数，无返回值
        /// </summary>
        /// <param name="serviceUrl">格式: http://172.16.136.8:9999  </param>
        /// <param name="command"></param>
        public static void RequestAsync(string serviceUrl, string command)
        {
            RequestAsync<string, string>(serviceUrl, command, string.Empty, e => { });
        }

        /// <summary>
        /// 请求：无参数，有返回值
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="serviceUrl">格式: http://172.16.136.8:9999  </param>
        /// <param name="command"></param>
        /// <param name="callBack"></param>
        public static void RequestAsync<TResponse>(string serviceUrl, string command, Action<ResponseEntity<TResponse>> callBack) where TResponse : class
        {
            RequestAsync(serviceUrl, command, string.Empty, callBack);
        }

        /// <summary>
        /// 请求：有参数，无返回值
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <param name="serviceUrl">格式: http://172.16.136.8:9999  </param>
        /// <param name="command"></param>
        /// <param name="request"></param>
        public static void RequestAsync<TRequest>(string serviceUrl, string command, TRequest request)
        {
            RequestAsync<TRequest, string>(serviceUrl, command, request, e => { });
        }

        /// <summary>
        /// 请求：有参数，有返回值
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="serviceUrl">格式: http://172.16.136.8:9999  </param>
        /// <param name="commandName"></param>
        /// <param name="request"></param>
        /// <param name="callBack"></param>
        public static void RequestAsync<TRequest, TResponse>(string serviceUrl, string commandName, TRequest request, Action<ResponseEntity<TResponse>> callBack) where TResponse : class
        {
            var proxy = ServiceProxyDictionary.Keys.Contains(serviceUrl) ? ServiceProxyDictionary[serviceUrl] : new CommandServiceProxy(serviceUrl);
            proxy.InvokeAsync(
                InteractiveHelper.BuildRequestWithParams(commandName, request),
                (s, e) =>
                {
                    var responseEntity = new ResponseEntity<TResponse>();
                    try
                    {
                        var response = s as ResponseModel;
                        ParseResponseModel(response, responseEntity);
                    }
                    catch (Exception ex)
                    {
                        responseEntity.ErrorMessage = ex.ToString();
                    }
                    finally
                    {
                        if (callBack != null)
                        {
                            callBack(responseEntity);
                        }
                    }
                });
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 解析ResponseModel
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="response"></param>
        /// <param name="responseEntity"></param>
        private static void ParseResponseModel<TResponse>(ResponseModel response, ResponseEntity<TResponse> responseEntity) where TResponse : class
        {
            if (response != null)
            {
                if (response.State == ResponseStateDefine.Success)
                {
                    if (!string.IsNullOrEmpty(response.ResultData))
                    {
                        responseEntity.Result = response.ResultData.ToObject<TResponse>();
                    }
                    responseEntity.IsSuccess = true;
                }
                else
                {
                    responseEntity.ErrorMessage = response.ResultData;
                }
            }
            else
            {
                responseEntity.ErrorMessage = FailedToParseResponseModel;
            }
        }

        #endregion
    }
}