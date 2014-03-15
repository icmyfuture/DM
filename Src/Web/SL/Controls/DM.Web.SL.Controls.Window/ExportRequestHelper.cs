#region Import

using System;
using System.Collections.Generic;
using System.Linq;
using AI3.Common.Controls.Window.Entities;
using AI3.Common.Core;
using AI3.Common.Core.Utility.SerializeService;
using AI3.Common.Core.WSRequest;

#endregion

namespace AI3.Common.Controls.Window
{
    /// <summary>
    ///   操作类
    /// </summary>
    public class ExportRequestHelper
    {
        #region 单一实例

        /// <summary>
        ///   单一实例
        /// </summary>
        public static readonly ExportRequestHelper Instance = new ExportRequestHelper();

        #endregion

        #region  构造函数

        #endregion  构造函数

        #region 私有变量

        /// <summary>
        ///   系统接口缓存
        /// </summary>
        private List<SystemInterfactInfo> AllSystemInterfactInfo;

        #endregion

        #region 属性

        #endregion

        #region 公共方法

        /// <summary>
        ///   获取所有系统注册信息
        /// </summary>
        /// <param name = "CallBackEvent">响应回调函数</param>
        public void GetAllSystemInteract(EventHandler CallBackEvent)
        {
            GetAllSystemInteract(false, CallBackEvent);
        }

        /// <summary>
        ///   获取所有系统注册信息
        /// </summary>
        /// <param name = "IsForceUpdate">是否强制更新为服务端数据</param>
        /// <param name = "CallBackEvent">响应回调函数</param>
        public void GetAllSystemInteract(bool IsForceUpdate, EventHandler CallBackEvent)
        {
            if (AllSystemInterfactInfo == null || IsForceUpdate)
            {
                RequestModel requestModel = new RequestModel
                                                {
                                                    CommandName = "SystemInteractControl/getAllSystemInteract",
                                                    Parameters = "",
                                                    UserID = CurrentUserHelper.GetCurrentUserInfo().UserID
                                                };
                RequestHelper.Instance.GetServiceRequest(requestModel, (obj, arg) =>
                                                                           {
                                                                               RequestResultModel resultModel = obj as RequestResultModel;
                                                                               if (resultModel.State == ResultState.Success)
                                                                               {
                                                                                   AllSystemInterfactInfo = SerializeProxy.JsonSerialize.Deserialize<List<SystemInterfactInfo>>(resultModel.ResultData);
                                                                                   if (CallBackEvent != null)
                                                                                   {
                                                                                       CallBackEvent(AllSystemInterfactInfo, EventArgs.Empty);
                                                                                   }
                                                                               }
                                                                           });
            }
            else
            {
                if (CallBackEvent != null)
                {
                    CallBackEvent(AllSystemInterfactInfo, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        ///   获取接口信息
        /// </summary>
        /// <param name = "systemID">系统ID</param>
        /// <returns></returns>
        public SystemInterfactInfo GetSystemInterfactInfo( string systemID )
        {
            if (AllSystemInterfactInfo.Count(x => x.SystemID.ToUpper() == systemID.ToUpper()) > 0)
            {
                return AllSystemInterfactInfo.First(x => x.SystemID.ToUpper() == systemID.ToUpper());
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        ///   出库请求
        /// </summary>
        /// <param name = "exportRequest">出库请求对象</param>
        /// <param name = "CallBackEvent">响应回调函数</param>
        public void Export(ExportRequestWrapperInfo exportRequest, EventHandler CallBackEvent)
        {
            RequestModel requestModel = new RequestModel
                                            {
                                                CommandName = "SystemInteractControl/export",
                                                Parameters = SerializeProxy.JsonSerialize.SerializeToString(exportRequest),
                                                UserID = CurrentUserHelper.GetCurrentUserInfo().UserID
                                            };
            RequestHelper.Instance.GetServiceRequest(requestModel, CallBackEvent);
        }

        #endregion

        #region 私有方法

        #endregion
    }
}