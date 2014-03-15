using System;
using System.Collections.Generic;
using DM.Client.WPF.Controls.NotifyMessage.Entitys;
using DM.Client.WPF.Controls.NotifyMessage.Extension.View;
using System.Threading;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace DM.Client.WPF.Controls.NotifyMessage
{
    /// <summary>
    /// Notify Message 管理者
    /// </summary>
    public static class NotifyMessageManager
    {
        /// <summary>
        /// Notify 消息队列
        /// </summary>
        public static Queue<NotifyWindowEntity> NotifyCacth = new Queue<NotifyWindowEntity>();

        static NotifyMessageManager()
        {
            ThreadPool.QueueUserWorkItem((s) => InvokeSendMessage());
        }

        private static TaskbarIcon m_notifyTaskBar;

        internal static TaskbarIcon NotifyTaskBar
        {
            get { return m_notifyTaskBar; }
            private set { m_notifyTaskBar = value; }
        }

        private static NotifyWindow m_notifyWindow;

        private static NotifyWindow NotifyWindow
        {
            get { return m_notifyWindow; }
            set { m_notifyWindow = value; }
        }

        private static void InvokeSendMessage()
        {

            while (true)
            {
                if (NotifyCacth.Count > 0)
                {
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        NotifyWindow = new NotifyWindow
                        {
                            MessageContent = { Content = new MessageContent(NotifyCacth.Dequeue()) }
                        };
                    }));
                    try
                    {
                        //最后一条显示5秒
                        NotifyTaskBar.ShowCustomBalloon(
                            NotifyWindow,
                            PopupAnimation.Slide,
                            2000);
                    }
                    catch (System.Exception exception)
                    {
                        // SystemHelper.Instance.Loger.Loger.WriteLog(exception);
                    }
                }
                Thread.Sleep(500);
            }
        }

        private static void SendToDeskTop(this NotifyWindowEntity notifyWindow)
        {
            NotifyCacth.Enqueue(notifyWindow);
        }

        /// <summary>
        /// send string content to notify ariespace with custorm window message
        /// </summary>
        /// <param name="messageContent">notify message content</param>
        public static void SendMessageToNotify(this string messageContent)
        {
            var notifyWindow = new NotifyWindowEntity(new NotifyWindowMessage(messageContent), NotifyWindowType.Message);
            notifyWindow.SendToDeskTop();
        }

        /// <summary>
        /// 注册NotifyIcon
        /// <param name="notifyImage">Notify image show with taskbar notify Airespace</param>
        /// <param name="toolTip">Notify tool tips show with taskbar  notify Airespace</param>
        /// </summary>
        public static void Register(ImageSource notifyImage, string toolTip)
        {
            IniteTooltipStyle();
            NotifyTaskBar = new TaskbarIcon
                            {
                                IconSource = notifyImage,
                                TrayToolTip = new CustermToolTip(toolTip),
                            };

            //var model = new ToolTipBind { ToolTipText = toolTip };
            //ToolTipService.SetToolTip(NotifyTaskBar, new ToolTip { Style = notifytipStyle, DataContext = model});
        }

        private static Style notifytipStyle = null;

        /// <summary>
        /// 初始化Tool Tip 样式
        /// </summary>
        private static void IniteTooltipStyle()
        {
            if (notifytipStyle == null)
                return;
            if (System.Windows.Application.Current == null)
            {
                return;
            }
            if (System.Windows.Application.Current.Resources["BaseToolTip"] == null)
            {
                System.Windows.Application.Current.Resources.MergedDictionaries.Add(System.Windows.Application.LoadComponent(new Uri("/DM.Client.WPF.Controls.NotifyMessage;component/Resource/Style/BaseToolTipStyle.xaml", UriKind.Relative)) as ResourceDictionary);
            }
            notifytipStyle = (Style)System.Windows.Application.Current.Resources["BaseToolTip"];
        }

        /// <summary>
        /// 反注册消息 通知
        /// </summary>
        public static void UnRegister()
        {
            NotifyTaskBar.Dispose();
        }
    }
}
