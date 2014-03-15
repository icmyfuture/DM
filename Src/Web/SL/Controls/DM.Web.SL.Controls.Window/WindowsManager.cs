using DM.Web.SL.Common.Core.App;
using DM.Web.SL.Common.Utility;
using DM.Web.SL.Controls.MessageBox;
using DM.Web.SL.Controls.Window.Controls;
using DM.Web.SL.Controls.Window.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DM.Web.SL.Controls.Window
{
    #region Imports

    

    #endregion

    /// <summary>
    ///   窗口管理类
    /// </summary>
    public class WindowsManager
    {
        #region Fields

        /// <summary>
        ///   单一实例
        /// </summary>
        public static readonly WindowsManager Instance = new WindowsManager();

        private double m_activeWindowLeft;
        private double m_activeWindowTop;
        private List<WindowInfo> m_childrenWindows = new List<WindowInfo>();
        private Canvas m_desktop = new Canvas();
        private WrapPanel m_taskBar;
        private double m_windowHeight;
        private double m_windowWidth;

        #endregion Fields

        #region Properties

        /// <summary>
        ///   系统桌面Canvas
        /// </summary>
        public Canvas Desktop
        {
            get { return m_desktop; }
            set { m_desktop = value; }
        }

        /// <summary>
        ///   系统任务栏WrapPanel
        /// </summary>
        public WrapPanel TaskBar
        {
            get { return m_taskBar; }
            set { m_taskBar = value; }
        }

        /// <summary>
        ///   任务栏改变事件
        /// </summary>
        public EventHandler TaskBarItemChengedEventHandler { get; set; }

        /// <summary>
        ///   窗口ZIndex改变事件
        /// </summary>
        public EventHandler WindowZIndexChengedEventHandler { get; set; }

        #endregion Properties

        #region Methods

        #region 公共方法

        /// <summary>
        ///   窗口大小改变事件
        /// </summary>
        public void AllWindowsSizeChanged()
        {
            for (int i = 0; i < m_childrenWindows.Count; i++)
            {
                m_childrenWindows[i].ResizeEnabled = true;
                m_childrenWindows[i].Maximize();
                m_childrenWindows[i].ResizeEnabled = false;
            }
        }

        /// <summary>
        ///   启用活动窗口
        /// </summary>
        public void ArrangeActiveWindow()
        {
            WindowInfo activeWindow = GetActiveWindow();
            TaskBarSetStatus(activeWindow);
        }

        /// <summary>
        ///   关闭所有窗口
        /// </summary>
        public void CloseAllWindows()
        {
            for (int i = 0; i < m_childrenWindows.Count; i++)
            {
                m_childrenWindows[i].Close();
                m_childrenWindows[i] = null; //内存释放
            }
            //taskBar.Children.Clear();

            m_childrenWindows.Clear();
            m_childrenWindows = null;
            m_childrenWindows = new List<WindowInfo>();
        }

        /// <summary>
        ///   关闭窗口
        /// </summary>
        /// <param name = "windowId">窗口ID</param>
        public void CloseWindow(string windowId)
        {
            WindowInfo w = GetWindow(windowId);
            if (w != null)
            {
                w.Close();
            }
        }

        ///<summary>
        ///  根据应用id判断在否已经打开
        ///</summary>
        ///<param name = "applicationID"></param>
        ///<returns></returns>
        public bool ExistWindowByApplicationID(string applicationID)
        {
            WindowInfo w = (WindowInfo) (from i in m_desktop.Children
                                         where (i is WindowInfo) && (((WindowInfo) i).ApplicationId == applicationID)
                                         select i).FirstOrDefault();

            return w == null ? false : true;
        }

        /// <summary>
        ///   获取应用ID
        /// </summary>
        /// <param name = "windowId">窗口ID</param>
        /// <returns>应用ID</returns>
        public string GetApplicationID(string windowId)
        {
            WindowInfo activeWindow = GetWindow(windowId);

            if (activeWindow != null)
            {
                return activeWindow.ApplicationId;
            }
            return "";
        }

        /// <summary>
        ///   获取窗口
        /// </summary>
        /// <param name = "windowId">窗口ID</param>
        /// <returns></returns>
        public WindowInfo GetWindow(string windowId)
        {
            WindowInfo w = (WindowInfo) (from i in m_desktop.Children
                                         where (i is WindowInfo) && (((WindowInfo) i).Id == windowId)
                                         select i).FirstOrDefault();
            return w;
        }

        /// <summary>
        ///   获取窗口位置
        /// </summary>
        /// <param name = "windowId">窗口ID</param>
        /// <returns>窗口位置</returns>
        public Point GetWindowLocation(string windowId)
        {
            Point location = new Point();
            WindowInfo activeWindow = GetWindow(windowId);

            if (activeWindow != null)
            {
                location.X = Canvas.GetLeft(activeWindow);
                location.Y = Canvas.GetTop(activeWindow);
            }
            return location;
        }

        /// <summary>
        ///   设置窗口大小
        /// </summary>
        /// <param name = "windowId">窗口ID</param>
        /// <returns>窗口大小</returns>
        public Size GetWindowSize(string windowId)
        {
            Size size = new Size();
            WindowInfo activeWindow = GetWindow(windowId);

            if (activeWindow != null)
            {
                size.Width = activeWindow.ActualWidth;
                size.Height = activeWindow.ActualHeight;
            }
            return size;
        }

        /// <summary>
        ///   获取窗口标题
        /// </summary>
        /// <param name = "windowId">窗口ID</param>
        /// <returns>窗口标题</returns>
        public string GetWindowTitle(string windowId)
        {
            WindowInfo activeWindow = GetWindow(windowId);
            if (activeWindow != null)
            {
                return activeWindow.Caption;
            }
            return "";
        }

        /// <summary>
        ///   注册窗口关闭后事件
        /// </summary>
        /// <param name = "windowId">窗口ID</param>
        /// <param name = "closedEventHandler">事件</param>
        public void RegistrationClosedEvent(string windowId, WindowClosedEventHandler closedEventHandler)
        {
            WindowInfo w = GetWindow(windowId);
            if (w != null)
            {
                w.ClosedEventHandler -= closedEventHandler;
                w.ClosedEventHandler += closedEventHandler;
            }
        }

        /// <summary>
        ///   注册窗口关闭前事件
        /// </summary>
        /// <param name = "windowId">窗口ID</param>
        /// <param name = "closingEventHandler">事件</param>
        public void RegistrationClosingEvent(string windowId, WindowClosingEventHandler closingEventHandler)
        {
            WindowInfo w = GetWindow(windowId);
            if (w != null)
            {
                w.ClosingEventHandler -= closingEventHandler;
                w.ClosingEventHandler += closingEventHandler;
            }
        }

        /// <summary>
        ///   设置窗口位置
        /// </summary>
        /// <param name = "windowId">窗口ID</param>
        /// <param name = "left">靠左</param>
        /// <param name = "top">据顶</param>
        public void SetWindowLocation(string windowId, double left, double top)
        {
            WindowInfo activeWindow = GetWindow(windowId);

            if (activeWindow != null)
            {
                #region 位置处理

                left = left > 0 ? Math.Floor(left) : 0;
                top = top > 0 ? Math.Floor(top) : 0;

                #endregion

                Canvas.SetLeft(activeWindow, left);
                Canvas.SetTop(activeWindow, top);
            }
        }

        /// <summary>
        ///   设置窗口位置居中
        /// </summary>
        /// <param name = "windowId">窗口ID</param>
        public void SetWindowLocationCenter(string windowId)
        {
            WindowInfo activeWindow = GetWindow(windowId);

            if (activeWindow != null)
            {
                Point location = new Point(Math.Floor((Instance.Desktop.ActualWidth - activeWindow.Width) / 2.0), Math.Floor(((Instance.Desktop.ActualHeight - activeWindow.Height)) / 2.0));
                SetWindowLocation(windowId, location.X, location.Y);
            }
        }

        /// <summary>
        ///   设置窗口大小
        /// </summary>
        /// <param name = "windowId">窗口ID</param>
        /// <param name = "width">宽度</param>
        /// <param name = "height">高度</param>
        public void SetWindowSize(string windowId, double width, double height)
        {
            WindowInfo activeWindow = GetWindow(windowId);

            if (activeWindow != null)
            {
                activeWindow.Width = width;
                activeWindow.Height = height;
                activeWindow.UpdateLayout();
            }
        }

        /// <summary>
        ///   设置窗口大小
        /// </summary>
        /// <param name = "windowId">窗口ID</param>
        /// <param name = "width">宽度</param>
        /// <param name = "height">高度</param>
        /// <param name = "location">窗口位置</param>
        public void SetWindowSize(string windowId, double width, double height, Point location)
        {
            WindowInfo activeWindow = GetWindow(windowId);

            if (activeWindow != null)
            {
                activeWindow.Width = width;
                activeWindow.Height = height;
                activeWindow.UpdateLayout();
                SetWindowLocation(windowId, location.X, location.Y);
            }
        }

        /// <summary>
        ///   设置窗口标题
        /// </summary>
        /// <param name = "windowId">窗口ID</param>
        /// <param name = "title">标题</param>
        public void SetWindowTitle(string windowId, string title)
        {
            WindowInfo activeWindow = GetWindow(windowId);

            if (activeWindow != null)
            {
                activeWindow.Caption = title;
            }
        }

        /// <summary>
        ///   显示自定义窗口
        /// </summary>
        /// <param name = "content">内容</param>
        /// <param name = "caption">标题</param>
        /// <returns></returns>
        public WindowInfo ShowCustom(FrameworkElement content, string caption)
        {
            WindowInfo element = new WindowInfo(false, true)
                                 {
                                     Caption = caption,
                                     Content = content,
                                     ResizeEnabled = false,
                                     DraggingEnabled = false,
                                     Name = Guid.NewGuid().ToString()
                                 };
            Canvas.SetLeft(element, (m_desktop.ActualWidth - content.Width) / 2.0);
            Canvas.SetTop(element, 120.0);
            m_desktop.Children.Add(element);
            return element;
        }

        /// <summary>
        ///   显示桌面
        /// </summary>
        public void ShowDesktop()
        {
            foreach (WindowInfo window in from i in m_desktop.Children
                                          where (i is WindowInfo) && ((i).Visibility == Visibility.Visible)
                                          select i)
            {
                window.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        ///   显示模态窗口
        /// </summary>
        /// <param name = "caption">标题</param>
        /// <param name = "text">内容</param>
        /// <param name = "width">宽度</param>
        /// <param name = "height">高度</param>
        public void ShowModal(string caption, string text, double width, double height)
        {
            if (string.IsNullOrEmpty(caption))
            {
                caption = LanguageHelper.GetDictionary("DM.Common.Controls", "txtsTips", "Tip");
            }
            Ai3MessageBox.Show(text, caption, MessageBoxButtonType.OK, width, height);
        }

        /// <summary>
        ///   显示模态窗口
        ///   Author:cj
        /// </summary>
        /// <param name = "text">内容</param>
        /// <param name = "width">宽度</param>
        /// <param name = "height">高度</param>
        public void ShowModal(string text, double width, double height)
        {
            string caption = LanguageHelper.GetDictionary("DM.Common.Controls", "txtsTips", "Tip");
            Ai3MessageBox.Show(text, caption, MessageBoxButtonType.OK, width, height);
        }

        /// <summary>
        ///   显示窗口
        /// </summary>
        /// <param name = "window"></param>
        /// <param name = "location"></param>
        public void ShowWindow(WindowInfo window, Point location)
        {
            ShowWindow(window, location, false);
        }

        /// <summary>
        ///   显示窗口
        /// </summary>
        /// <param name = "content"></param>
        /// <param name = "caption"></param>
        /// <param name = "location"></param>
        /// <returns></returns>
        public WindowInfo ShowWindow(FrameworkElement content, string caption, Point location)
        {
            WindowInfo window = new WindowInfo
                                {
                                    Caption = caption,
                                    Content = content,
                                };
            ShowWindow(window, location);
            return window;
        }

        /// <summary>
        ///   显示窗口
        /// </summary>
        /// <param name = "window"></param>
        /// <param name = "location">位置</param>
        /// <param name = "isResizable">是否可投</param>
        public void ShowWindow(WindowInfo window, Point location, bool isResizable)
        {
            try
            {
                #region 位置处理

                location.X = location.X > 0 ? Math.Floor(location.X) : 0;
                location.Y = location.Y > 0 ? Math.Floor(location.Y) : 0;

                #endregion

                #region 处理应用数目

                GetNewAppName(window);

                #endregion

                if (ExistWindowProcess(window))
                {
                    #region 应用窗口样式

                    if (Application.Current.Resources.Contains("windowStyle"))
                    {
                        window.Style = Application.Current.Resources["windowStyle"] as Style;
                    }

                    #endregion

                    #region 事件注册

                    window.CloseEventHandler -= Instance.WindowClosed;
                    //window.Maximized += new System.EventHandler(WindowsManager.Instance.window_Maximized);
                    window.Normalized -= Instance.window_Normalized;
                    window.Minimized -= Instance.WindowMinimized;
                    window.Dragged -= Instance.window_Dragged;
                    window.SetZIndex -= Instance.WindowSetZIndex;
                    window.MouseLeftButtonDown -= window_MouseLeftButtonDown;

                    window.CloseEventHandler += Instance.WindowClosed;
                    //window.Maximized += new System.EventHandler(WindowsManager.Instance.window_Maximized);
                    window.Normalized += Instance.window_Normalized;
                    window.Minimized += Instance.WindowMinimized;
                    window.Dragged += Instance.window_Dragged;
                    window.SetZIndex += Instance.WindowSetZIndex;
                    window.MouseLeftButtonDown += window_MouseLeftButtonDown;

                    #endregion

                    #region 保存窗口大小

                    m_windowHeight = window.Height;
                    m_windowWidth = window.Width;

                    m_activeWindowLeft = location.X;
                    m_activeWindowTop = location.Y;

                    #endregion

                    //window.MaxHeight = desktop.ActualHeight;
                    //window.MaxWidth = desktop.ActualWidth;
                    //window.ResizeEnabled = isResizable;
                    window.ResizeEnabled = true;
                    Canvas.SetLeft(window, location.X);
                    Canvas.SetTop(window, location.Y);
                    //Canvas.SetLeft(window, 0.0);
                    //Canvas.SetTop(window, 0.0);

                    //window.Width = desktop.ActualWidth;
                    //window.Height = desktop.ActualHeight;

                    m_childrenWindows.Add(window);
                    m_desktop.Children.Add(window);
                    AddTaskBarItem(window);
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        ///   全屏事件
        /// </summary>
        public void WindowFullScreen()
        {
            //WindowInfo activeWindow = GetActiveWindow();

            //if (Application.Current.Host.Content.IsFullScreen)
            //{
            //    double actualWidth = Application.Current.Host.Content.ActualWidth + 10;
            //    double actualHeight = Application.Current.Host.Content.ActualHeight + 50;

            //    activeWindow.Height = actualHeight;
            //    activeWindow.Width = actualWidth;

            //    ((Control) activeWindow.Content).Width = actualWidth - 4;
            //    ((Control) activeWindow.Content).MinWidth = actualWidth - 4;
            //    ((Control) activeWindow.Content).Height = actualHeight - 10;
            //    ((Control) activeWindow.Content).MinHeight = actualHeight - 10;

            //    Canvas.SetLeft(activeWindow, -5);
            //    Canvas.SetTop(activeWindow, -25);
            //}
            //else
            //{
            //    activeWindow.Height = m_windowHeight;
            //    activeWindow.Width = m_windowWidth;

            //    ((Control) activeWindow.Content).Width = m_windowWidth - 4;
            //    ((Control) activeWindow.Content).Height = m_windowHeight - 10;

            //    Canvas.SetLeft(activeWindow, m_activeWindowLeft);
            //    Canvas.SetTop(activeWindow, m_activeWindowTop);
            //}
        }

        #endregion

        #region 私有方法

        /// <summary>
        ///   激活已打开的窗口
        /// </summary>
        /// <param name = "window"></param>
        private void ActivateExistWindow(WindowInfo window)
        {
            //是否存在相同窗口
            if (IsExistWindow(window))
            {
                Func<UIElement, bool> predicate = i => (i is TaskBarItem) && (((TaskBarItem) i).ID == window.Id);

                #region 激活原窗口

                var item = (TaskBarItem) m_taskBar.Children.Where(predicate).FirstOrDefault();
                if (item != null)
                {
                    WindowInfo w = (WindowInfo) (from i in m_desktop.Children
                                                 where (i is WindowInfo) && (((WindowInfo) i).Id == item.ID)
                                                 select i).FirstOrDefault();
                    if (w != null)
                    {
                        w.Visibility = Visibility.Visible;
                        w.Focus();
                        //taskBar_setStatus(w, true);
                    }
                }
                item = null;

                #endregion
            }
        }

        /// <summary>
        ///   已打开的窗口处理
        /// </summary>
        /// <param name = "window"></param>
        private bool ExistWindowProcess(WindowInfo window)
        {
            bool isOpenNewWindow;

            //是否存在相同窗口
            if (IsExistWindow(window))
            {
                Func<UIElement, bool> predicate = i => (i is TaskBarItem) && (((TaskBarItem) i).ID == window.Id);

                ApplicationInfo app = window.Tag as ApplicationInfo;
                if (app != null && !app.IsReplaceOrgWindow)
                {
                    #region 激活原窗口

                    var item = (TaskBarItem) m_taskBar.Children.Where(predicate).FirstOrDefault();
                    if (item != null)
                    {
                        WindowInfo w = (WindowInfo) (from i in m_desktop.Children
                                                     where (i is WindowInfo) && (((WindowInfo) i).Id == item.ID)
                                                     select i).FirstOrDefault();
                        if (w != null)
                        {
                            w.Visibility = Visibility.Visible;
                            w.Focus();
                            //taskBar_setStatus(w, true);
                        }
                    }
                    item = null;

                    #endregion

                    isOpenNewWindow = false;
                }
                else
                {
                    #region 清除原窗口

                    TaskBarItem item = (TaskBarItem) m_taskBar.Children.Where(predicate).FirstOrDefault();

                    if (item != null)
                    {
                        var w = (WindowInfo) (from i in m_desktop.Children
                                              where (i is WindowInfo) && (((WindowInfo) i).Id == item.ID)
                                              select i).FirstOrDefault();
                        if (w != null)
                        {
                            if (w.Close())
                            {
                                m_desktop.Children.Remove(w);
                                w.Content = null;
                                w.Dispose();

                                m_taskBar.Children.Remove(item);

                                item.Dispose();
                                item = null;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }

                    #endregion

                    window.Id = ((IApplicationInterface) window.Content).WindowID;
                    isOpenNewWindow = true;
                }
            }
            else
            {
                isOpenNewWindow = true;
            }
            return isOpenNewWindow;
        }

        /// <summary>
        ///   添加任务栏
        /// </summary>
        /// <param name = "w"></param>
        private void AddTaskBarItem(WindowInfo w)
        {
            if (m_taskBar != null)
            {
                TaskBarItem item = new TaskBarItem
                                   {
                                       ID = w.Id,
                                       Name = "tb" + w.Name,
                                       Caption = w.Caption,
                                       IcoPath = w.IcoPath,
                                       ApplicationId = w.ApplicationId
                                   };
                item.Clicked += Instance.TaskBarItemClicked;
                m_taskBar.Children.Add(item);

                if (TaskBarItemChengedEventHandler != null)
                {
                    TaskBarItemChengedEventHandler(null, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        ///   获取当前活动窗口
        /// </summary>
        /// <returns></returns>
        private WindowInfo GetActiveWindow()
        {
            WindowInfo activeWindow = null;
            int num = 0;
            foreach (WindowInfo window in from i in m_desktop.Children
                                          where (i is WindowInfo) && ((i).Visibility == Visibility.Visible)
                                          select i)
            {
                int zIndex = Canvas.GetZIndex(window);
                if (zIndex >= num)
                {
                    num = zIndex;
                    activeWindow = window;
                }
            }
            return activeWindow;
        }

        /// <summary>
        ///   获取新添加的应用Name
        /// </summary>
        /// <param name = "window"></param>
        /// <returns></returns>
        private void GetNewAppName(WindowInfo window)
        {
            var app = (ApplicationInfo) window.Tag;
            string mSAppName = window.ApplicationId;
            Func<UIElement, bool> predicate;

            //兼容ApplicationID的大小写
            predicate = i => (i is TaskBarItem) && (((TaskBarItem) i).ApplicationId.ToLower() == app.ApplicationID.ToLower());
            List<UIElement> items = m_taskBar.Children.Where(predicate).ToList();
            if (items.Count > 0)
            {
                if (items.Count < app.OpenNumber)
                {
                    mSAppName += "_" + items.Count + 1;
                }
                else
                {
                    mSAppName += "_1";

                    window.Id = ((TaskBarItem) items[items.Count - 1]).ID;
                    //Ai3MessageBox.Show( "This application has reached the number set to start, please close the application has nothing to do!", "Tip", MessageBoxButtonType.OK, 300, 130 );
                }
            }
            else
            {
                mSAppName += "_1";
            }
            window.Name = mSAppName;
        }

        /// <summary>
        ///   判断窗口是否已经打开或已经达到了打开的个数
        /// </summary>
        /// <param name = "window"></param>
        /// <returns></returns>
        private bool IsExistWindow(WindowInfo window)
        {
            Func<UIElement, bool> predicate;
            if (m_taskBar != null)
            {
                predicate = i => (i is TaskBarItem) && (((TaskBarItem) i).ID == window.Id);

                TaskBarItem item = (TaskBarItem) m_taskBar.Children.Where(predicate).FirstOrDefault();
                if (item != null)
                {
                    return true;
                }

                return false;
            }
            return false;
        }

        /// <summary>
        ///   任务栏点击事件
        /// </summary>
        /// <param name = "sender"></param>
        /// <param name = "e"></param>
        private void TaskBarItemClicked(object sender, EventArgs e)
        {
            TaskBarItem taskBarItem = (TaskBarItem) sender;
            WindowInfo window = (WindowInfo) (from i in m_desktop.Children
                                              where (i is WindowInfo) && (((WindowInfo) i).Id == taskBarItem.ID)
                                              select i).FirstOrDefault();
            if (window != null)
            {
                if (window.Visibility == Visibility.Collapsed)
                {
                    window.Visibility = Visibility.Visible;
                }
                else
                {
                    //window.Visibility = Visibility.Collapsed;
                    //HideIEFrame(window);
                    //ArrangeActiveWindow();
                    WindowInfo activeWindow = GetActiveWindow();
                    if (activeWindow.Equals(window))
                    {
                        window.Visibility = Visibility.Collapsed;
                        ArrangeActiveWindow();
                    }
                    else
                    {
                        ActivateExistWindow(window);
                    }
                }
                window.Focus();
            }
        }

        /// <summary>
        ///   改变任务栏按钮状态
        /// </summary>
        /// <param name = "w">活动窗体</param>
        private void TaskBarSetStatus(WindowInfo w)
        {
            foreach (UIElement item in m_taskBar.Children)
            {
                if (item.GetType().Equals(typeof (TaskBarItem)))
                {
                    TaskBarItem taskItem = item as TaskBarItem;
                    if (taskItem != null && w != null)
                    {
                        if (taskItem.ID != w.Id)
                        {
                            taskItem.IsActived = false;
                            VisualStateManager.GoToState(taskItem.btnTitle, "Normal", false);
                        }
                        else
                        {
                            taskItem.IsActived = true;
                            VisualStateManager.GoToState(taskItem.btnTitle, "MouseOver", false);
                        }
                    }
                    else if (taskItem != null)
                    {
                        taskItem.IsActived = false;
                        VisualStateManager.GoToState(taskItem.btnTitle, "Normal", false);
                    }
                }
            }
        }

        /// <summary>
        ///   窗口关闭事件
        /// </summary>
        /// <param name = "sender"></param>
        /// <param name = "e"></param>
        private void WindowClosed(object sender, EventArgs e)
        {
            Func<UIElement, bool> predicate = null;
            WindowInfo w = (WindowInfo) sender;

            m_childrenWindows.Remove(w);
            m_desktop.Children.Remove(w);
            if (m_taskBar != null)
            {
                predicate = i => (i is TaskBarItem) && (((TaskBarItem) i).ID == w.Id);

                TaskBarItem item = (TaskBarItem) m_taskBar.Children.Where(predicate).FirstOrDefault();
                if (item != null)
                {
                    m_taskBar.Children.Remove(item);
                }
                if (item != null)
                {
                    item.Dispose();
                }
            }

            if (TaskBarItemChengedEventHandler != null)
            {
                TaskBarItemChengedEventHandler(null, EventArgs.Empty);
            }

            #region 释放内存

            #region 事件注册

            w.CloseEventHandler -= Instance.WindowClosed;
            //window.Maximized += new System.EventHandler(WindowsManager.Instance.window_Maximized);
            w.Normalized -= Instance.window_Normalized;
            w.Minimized -= Instance.WindowMinimized;
            w.Dragged -= Instance.window_Dragged;
            w.SetZIndex -= Instance.WindowSetZIndex;
            w.MouseLeftButtonDown -= window_MouseLeftButtonDown;

            #endregion

            //释放内存
            predicate = null;

            ApplicationInfo app = (w.Tag as ApplicationInfo);
            if (app != null)
            {
                app.Instance = null;
                app = null;
            }
            w.Style = null;
            w.Tag = null;
            UserControl content = (w.Content as UserControl);
            if (content != null)
            {
                ((IApplicationInterface) content).Dispose();
                content.Clip = null;
            }
            w.Dispose();
            w = null;

            #endregion

            ArrangeActiveWindow();
        }

        /// <summary>
        ///   窗口拖拽事件
        /// </summary>
        /// <param name = "sender"></param>
        /// <param name = "e"></param>
        private void window_Dragged(object sender, EventArgs e)
        {
            WindowInfo window = (WindowInfo) sender;
        }

        /// <summary>
        ///   窗口最大化
        /// </summary>
        /// <param name = "sender"></param>
        /// <param name = "e"></param>
        private void WindowMaximized(object sender, EventArgs e)
        {
            WindowInfo element = (WindowInfo) sender;
            Canvas.SetLeft(element, 0.0);
            Canvas.SetTop(element, 0.0);
            element.Width = m_desktop.ActualWidth;
            element.Height = m_desktop.ActualHeight;
        }

        /// <summary>
        ///   窗口最小化
        /// </summary>
        /// <param name = "sender"></param>
        /// <param name = "e"></param>
        private void WindowMinimized(object sender, EventArgs e)
        {
            ArrangeActiveWindow();
        }

        /// <summary>
        ///   点击窗口事件
        /// </summary>
        /// <param name = "sender"></param>
        /// <param name = "e"></param>
        private void window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WindowInfo windows = (WindowInfo) sender;
            windows.Focus();
        }

        /// <summary>
        ///   普通大小
        /// </summary>
        /// <param name = "sender"></param>
        /// <param name = "e"></param>
        private void window_Normalized(object sender, EventArgs e)
        {}

        /// <summary>
        ///   窗口改变显示层次
        /// </summary>
        /// <param name = "sender"></param>
        /// <param name = "e"></param>
        private void WindowSetZIndex(object sender, EventArgs e)
        {
            WindowInfo w = (WindowInfo) sender;
            if (w.Visibility == Visibility.Visible)
            {
                if (WindowZIndexChengedEventHandler != null)
                {
                    WindowZIndexChengedEventHandler(null, null);
                }
                TaskBarSetStatus(w);
            }
        }

        #endregion

        #endregion Methods
    }
}