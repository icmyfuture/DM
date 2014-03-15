using System;
using System.Collections.Generic;
using DM.Web.SL.Common.Extensions;
using DM.Web.SL.Common.Service.Entities;
using System.Linq;

namespace DM.Web.SL.Common.Service
{
    public static class CommandServiceHelper
    {
        private static readonly Dictionary<string, CommandServiceProxy> ServiceProxyDictionary = new Dictionary<string, CommandServiceProxy>();

        /// <summary>
        /// 请求：无参数，有返回值
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="serviceUrl">格式: http://172.16.136.8:9999 </param>
        /// <param name="command"></param>
        /// <param name="callBack"></param>
        public static void Request<TResponse>(string serviceUrl, string command, Action<ResponseEntity<TResponse>> callBack) where TResponse : class
        {
            Request(serviceUrl, command, string.Empty, callBack);
        }

        /// <summary>
        /// 请求：有参数，无返回值
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <param name="serviceUrl"> 格式: http://172.16.136.8:9999 </param>
        /// <param name="command"></param>
        /// <param name="request"></param>
        public static void Request<TRequest>(string serviceUrl, string command, TRequest request)
        {
            Request<TRequest, string>(serviceUrl, command, request, e => { });
        }

        /// <summary>
        /// 请求：无参数，无返回值
        /// </summary>
        /// <param name="serviceUrl">格式: http://172.16.136.8:9999  </param>
        /// <param name="command"></param>
        public static void Request(string serviceUrl, string command)
        {
            Request<string, string>(serviceUrl, command, string.Empty, e => { });
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
        public static void Request<TRequest, TResponse>(string serviceUrl, string commandName, TRequest request, Action<ResponseEntity<TResponse>> callBack) where TResponse : class
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
                            responseEntity.ErrorMessage = "Failed to parse ResponseModel";
                        }
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
    }
}