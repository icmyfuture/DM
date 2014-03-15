using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using DM.Web.SL.Common.Core.App;
using DM.Web.SL.Common.Extensions;
using DM.Web.SL.Controls.Window.Entities;

namespace DM.Web.SL.Controls.Window
{
    /// <summary>
    ///   操作类
    /// </summary>
    public class ApplicationManager
    {
        #region Fields

        /// <summary>
        ///   单一实例
        /// </summary>
        public static readonly ApplicationManager Instance = new ApplicationManager();

        [CompilerGenerated]
        private UserControl m_backingField;

        #endregion Fields

        #region Constructors

        #endregion Constructors

        #region Event

        /// <summary>
        ///   打开应用事件
        /// </summary>
        public EventHandler ApplicationShowingEventHandler { get; set; }

        #endregion

        #region Properties

        /// <summary>
        ///   获取所有应用程序
        /// </summary>
        public List<ApplicationInfo> AllApplication { get; set; }

        /// <summary>
        ///   获取开始菜单左边程序
        /// </summary>
        public List<ApplicationInfo> GetRightStartMenuPrograms
        {
            get
            {
                List<ApplicationInfo> list = new List<ApplicationInfo>();
                return list;
            }
        }

        /// <summary>
        /// </summary>
        public UserControl Wrapper
        {
            [CompilerGenerated]
            get { return m_backingField; }
            [CompilerGenerated]
            set { m_backingField = value; }
        }

        #endregion Properties

        #region Methods

        #region 关闭所有窗口
        /// <summary>
        ///   关闭所有窗口
        /// </summary>
        public void CloseAllWindows()
        {
            WindowsManager.Instance.CloseAllWindows();
        }
        #endregion

        #region 获取应用信息

        ///<summary>
        ///  根据应用id判断在否已经打开
        ///</summary>
        ///<param name = "applicationID">应用id</param>
        ///<returns>是否已打开</returns>
        public bool ExistWindowByApplicationID(string applicationID)
        {
            return WindowsManager.Instance.ExistWindowByApplicationID(applicationID);
        }

        /// <summary>
        ///   获取应用信息
        /// </summary>
        /// <param name = "applicationId">应用ID</param>
        /// <returns></returns>
        public ApplicationInfo GetApplicationInfo(string applicationId)
        {
            try
            {
                ApplicationInfo resultapp = null;
                //兼容ApplicationID的大小写
                ApplicationInfo app = AllApplication.First(x => x.ApplicationID.ToLower() == applicationId.ToLower());
                if (app != null)
                {
                    //拷贝一个新实例,防止参数影响应用的处理逻辑，防止应用中修改了应用参数
                    string appJsonstr = app.ToJson();
                    resultapp = appJsonstr.ToObject<ApplicationInfo>();
                }
                return resultapp;
            }
            catch (Exception ex)
            {
                //WindowsManager.Instance.ShowModal( "Tip", "No install the " + applicationID + "-application or application exception!", 260, 130 );
                return null;
            }
        }

        /// <summary>
        ///   获取应用信息
        /// </summary>
        /// <param name = "windowId">窗口ID</param>
        /// <returns>应用信息</returns>
        public ApplicationInfo GetApplicationInfoByWindowID(string windowId)
        {
            string applicationId = WindowsManager.Instance.GetApplicationID(windowId);
            if (!string.IsNullOrEmpty(applicationId))
            {
                return GetApplicationInfo(applicationId);
            }
            return null;
        }
        #endregion

        #region 获取应用的服务地址
        /// <summary>
        ///   获取应用的服务地址
        /// </summary>
        /// <param name = "windowId">窗口ID</param>
        /// <returns>应用服务地址</returns>
        public string GetApplicationServiceAddress(string windowId)
        {
            //string applicationId = WindowsManager.Instance.GetApplicationID(windowId);
            //if (!string.IsNullOrEmpty(applicationId))
            //{
            //    ApplicationInfo app = GetApplicationInfo(applicationId);

            //    return app != null ? app.ServiceAddress : RequestHelper.Instance.GetServiceURL();
            //}
            //return RequestHelper.Instance.GetServiceURL();
            return string.Empty;
        }
        #endregion

        #region 获取应用功能
        /// <summary>
        ///   获取应用功能
        /// </summary>
        /// <param name = "applicationId">应用ID</param>
        /// <param name = "canBackEvent">回调函数</param>
        public void GetFunctionList(string applicationId, EventHandler canBackEvent)
        {
            //RequestModel requestModel = new RequestModel
            //                                {
            //                                    CommandName = "UIFrameService/getFunctionList",
            //                                    Parameters = applicationId
            //                                };
            //RequestHelper.Instance.GetServiceRequest(requestModel, canBackEvent);
        }
        #endregion

        #region 是否拥有应用权限
        /// <summary>
        ///   是否拥有应用权限
        /// </summary>
        /// <param name = "applicationId">应用ID</param>
        /// <returns></returns>
        public bool HaveApplicationPermission(string applicationId)
        {
            //兼容ApplicationID的大小写
            return AllApplication.Count(x => x.ApplicationID.ToLower() == applicationId.ToLower()) > 0;
        }
        #endregion

        #region 启动其他应用程序
        /// <summary>
        ///   启动其他应用程序
        /// </summary>
        /// <param name = "ooApp">启动外部程序参数</param>
        public void OpenOtherApplication(OpenOtherApplicationInfo ooApp)
        {
            //兼容ApplicationID的大小写
            ApplicationInfo app = AllApplication.First(x => x.ApplicationID.ToLower() == ooApp.ApplicationID.ToLower());

            if (app != null)
            {
                OpenOtherApplication(app, ooApp.Param, ooApp.Width, ooApp.Height);
            }
        }

        /// <summary>
        ///   启动其他应用程序
        /// </summary>
        /// <param name = "appInfo">应用信息</param>
        /// <param name = "param">参数信息</param>
        public void OpenOtherApplication(ApplicationInfo appInfo, string param)
        {
            OpenOtherApplication(appInfo, param, 0, 0);
        }

        /// <summary>
        ///   启动其他应用程序
        /// </summary>
        /// <param name = "appInfo">应用信息</param>
        /// <param name = "param">参数信息</param>
        /// <param name="width">窗口高</param>
        /// <param name="height">窗口宽</param>
        public void OpenOtherApplication(ApplicationInfo appInfo, string param, int width, int height)
        {
            try
            {
                if (appInfo != null)
                {
                    //拷贝一个新实例,防止参数影响应用的处理逻辑
                    string appJsonstr = appInfo.ToJson();
                    ApplicationInfo otherApp = appJsonstr.ToObject<ApplicationInfo>();
                    if (width > 0 && height > 0)
                    {
                        otherApp.WindowWidth = width;
                        otherApp.WindowHeight = height;
                    }
                    otherApp.Param = param;

                    //加载XAP，加载完后打开
                    ApplicationXAPHelper.Instance.LoadXap(otherApp, (sender, arg) =>
                                                                        {
                                                                            ApplicationInfo app = sender as ApplicationInfo;

                                                                            Instance.ShowApplication(app);
                                                                            if (app != null) app.Instance = null;
                                                                        });
                }
            }
            catch (Exception ex)
            {
                if (appInfo != null) WindowsManager.Instance.ShowModal("Tip", "No install the " + appInfo.ApplicationID + "-application or application exception!", 260, 130);
            }
        }
        #endregion

        #region 显示程序
        /// <summary>
        ///   显示程序
        /// </summary>
        /// <param name = "app"></param>
        /// <param name = "param">程序参数</param>
        public void ShowApplication(ApplicationInfo app, string param)
        {
            app.Param = param;
            ShowApplication(app);
        }

        /// <summary>
        ///   显示程序
        /// </summary>
        /// <param name = "app"></param>
        public void ShowApplication(ApplicationInfo app)
        {
            WindowInfo window = new WindowInfo(true, app.CanMaxWindow);
            //如果窗口图标不为空采用窗口图标，为空则采用应用图标
            window.IcoPath = string.IsNullOrEmpty(app.WindowIconPath) ? app.AppIconPath : app.WindowIconPath;

            window.Name = app.ApplicationID;

            window.Id = Guid.NewGuid().ToString();
            window.ApplicationId = app.ApplicationID;
            //如果窗口标题不为空采用窗口标题，为空则采用应用名称
            window.Caption = string.IsNullOrEmpty(app.WindowCaption) ? app.ApplicationName : app.WindowCaption;

            object obj2 = app.Instance;
            if (obj2 is IApplicationInterface)
            {
                ((IApplicationInterface)obj2).Param = app.Param;
                ((IApplicationInterface)obj2).WindowID = window.Id;
                //((IApplicationInterface)obj2).OpenOtherApplicationStarted(OpenOtherApplicationStarted);
            }
            window.Content = obj2;

            //double minW = WindowsManager.Instance.Desktop.ActualWidth * 2 / 3;
            //double minH = WindowsManager.Instance.Desktop.ActualHeight * 2 / 3;
            //((Control)obj2).MinWidth = ((Control)obj2).MinWidth < minW ? minW : ((Control)obj2).MinWidth;
            //((Control)obj2).MinHeight = ((Control)obj2).MinHeight < minW ? minW : ((Control)obj2).MinHeight;

            window.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
            window.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;

            window.Tag = app;

            //double width = !double.IsNaN(((Control) obj2).Width) && ((Control) obj2).Width <= 960 ? ((Control) obj2).Width : 960;
            //double height = !double.IsNaN(((Control) obj2).Height) && ((Control) obj2).Height <= 540 ? ((Control) obj2).Height : 540;

            double width = !double.IsNaN(app.WindowWidth) && app.WindowWidth > 0 ? app.WindowWidth : 960;
            double height = !double.IsNaN(app.WindowHeight) && app.WindowHeight > 0 ? app.WindowHeight : 540;

            ((Control)obj2).Width = width - 4;
            ((Control)obj2).Height = height - 10;
            window.Width = width;
            window.Height = height;

            #region 打开前事件

            if (ApplicationShowingEventHandler != null)
            {
                ApplicationShowingEventHandler(app, null);
            }

            #endregion

            Point location = new Point(Math.Floor((WindowsManager.Instance.Desktop.ActualWidth - width) / 2.0), Math.Floor(((WindowsManager.Instance.Desktop.ActualHeight - height)) / 2.0));

            WindowsManager.Instance.ShowWindow(window, location, app.IsResizable);
        }
        #endregion

        #endregion Methods
    }
}