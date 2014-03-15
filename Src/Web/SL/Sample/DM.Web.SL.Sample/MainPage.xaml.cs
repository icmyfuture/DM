using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;
using DM.Web.SL.Common.Core.App;
using DM.Web.SL.Common.Extensions;
using DM.Web.SL.Controls.Window;
using DM.Web.SL.Controls.Window.Entities;

namespace DM.Web.SL.Sample
{
    public partial class MainPage
    {
        private const int TaskItemHeight = 36;
        /// <summary>
        ///   显示区域宽度与内容实际宽度
        /// </summary>
        private double _mChildFrom;
        /// <summary>
        ///   显示区域宽度与内容实际宽度
        /// </summary>
        private double _mChildHeight;
        /// <summary>
        ///   显示区域宽度与内容实际宽度
        /// </summary>
        private double _mChildNum;
        /// <summary>
        ///   显示区域宽度与内容实际宽度
        /// </summary>
        private double _mChildTo;
        /// <summary>
        ///   显示区域宽度与内容实际宽度
        /// </summary>
        private double _mParentHeight;
        /// <summary>
        ///   切换状态，0为不能切换，1为允许切屏，2为可以向上切屏，3为可以向下切换，4为可以左右切换
        /// </summary>
        private int _mTabState;

        public MainPage()
        {
            InitializeComponent();
            Loaded += MainPage_Loaded;
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            WindowsManager.Instance.Desktop = cnvDesktop;
            WindowsManager.Instance.TaskBar = cnvTaskBar;
            WindowsManager.Instance.TaskBarItemChengedEventHandler = TaskbarContorEdit;
            WindowsManager.Instance.WindowZIndexChengedEventHandler = null;
            ApplicationManager.Instance.Wrapper = this;
            ApplicationManager.Instance.ApplicationShowingEventHandler = null;

            string clientBinUri = string.Format("http://{0}:{1}/ClientBin", Application.Current.Host.Source.Host, Application.Current.Host.Source.Port);
            ApplicationManager.Instance.AllApplication = new List<ApplicationInfo>
            {
                new ApplicationInfo
                    {
                    AppIconPath = clientBinUri + "/Resources/Image/mytask.png",
                    ApplicationID = "App.Demo",
                    ApplicationName = "示例应用",
                    WindowIconPath = clientBinUri + "/Resources/Image/mytask.png",
                    XapName = "DM.Web.SL.Sample.App.Demo.xap",
                    TypeName = "DM.Web.SL.Sample.App.Demo.MainPage;DM.Web.SL.Sample.App.Demo",
                    CanMaxWindow = true,
                    IsReplaceOrgWindow = true,
                    IsResizable = false,
                    IsShowAppList = true,
                    OpenNumber = 1,
                    OrderNo = 0,
                    WindowWidth = 960,
                    WindowHeight = 540,
                }
            };
        }

        /// <summary>
        ///   切换箭头状态调整
        /// </summary>
        /// <param name = "obj"></param>
        /// <param name = "e"></param>
        private void TaskbarContorEdit(object obj, EventArgs e)
        {
            var getMessageDispatcherTimer = new DispatcherTimer {Interval = TimeSpan.FromMilliseconds(100)};
            getMessageDispatcherTimer.Tick += (tickobj, tickarg) =>
            {
                #region 延时加载

                _mParentHeight = stackpanelTaskBar.Height;
                _mChildHeight = cnvTaskBar.ActualHeight;
                //---按钮禁用
                TaskBar_Down.IsEnabled = false;
                TaskBar_Up.IsEnabled = false;
                //---按钮状态归0
                _mTabState = 0;
                //---获取移动变化
                _mChildFrom = TaskBar_Story_From.Value;
                _mChildTo = TaskBar_Story_To.Value;
                //---
                if (_mChildHeight > _mParentHeight)
                {
                    TaskBar_Down.Visibility = Visibility.Visible;
                    TaskBar_Up.Visibility = Visibility.Visible;
                    //-------------------------------------------
                    _mTabState = 1; //允许切屏
                }
                else
                {
                    TaskBar_Down.Visibility = Visibility.Collapsed;
                    TaskBar_Up.Visibility = Visibility.Collapsed;
                }
                if (Math.Abs(_mChildTo - 0) < 1e-6)
                {
                    //没有切屏，允许向上滚屏
                    _mTabState += 1;
                }
                else if (_mChildTo < 0)
                {
                    //已经操作过向上切屏
                    _mChildNum = _mChildHeight - _mParentHeight + _mChildTo; //可操作区域
                    if (_mChildNum > 0)
                    {
                        //依然还有未显示区域可供向上切屏
                        _mTabState += 1;
                    }
                    else if (_mChildNum < 0)
                    {
                        //当前向上切屏区域已经超出实际可显示区域
                        //--迁移到最下限向上切屏区域（执行动画）
                        _mChildFrom = _mChildTo;
                        _mChildTo = _mChildTo - _mChildNum;
                        TaskBar_Story_From.Value = _mChildFrom;
                        TaskBar_Story_To.Value = _mChildTo;
                        //TaskBar_Story_From.KeyTime =new KeyTime(00:00:00);
                        TaskBar_Story.Begin();
                    }
                    if (_mChildTo < 0)
                    {
                        //已有切屏空间可供向下切屏
                        _mTabState += 2;
                    }
                }
                if (_mTabState == 2 || _mTabState == 4)
                {
                    TaskBar_Down.IsEnabled = true; //可以进行向上切屏
                }
                if (_mTabState == 3 || _mTabState == 4)
                {
                    TaskBar_Up.IsEnabled = true; //可以进行向下切屏 
                }

                #endregion

                getMessageDispatcherTimer.Stop();
                getMessageDispatcherTimer = null;
            };
            getMessageDispatcherTimer.Start();
        }

        /// <summary>
        ///   向上切屏
        /// </summary>
        /// <param name = "sender"></param>
        /// <param name = "e"></param>
        private void TaskBarDownClick(object sender, RoutedEventArgs e)
        {
            TaskBar_Down.IsEnabled = false;
            TaskBar_Up.IsEnabled = false;
            //--向上移动--
            _mChildFrom = _mChildTo;
            _mChildTo = _mChildTo - TaskItemHeight;
            TaskBar_Story_From.Value = _mChildFrom;
            TaskBar_Story_To.Value = _mChildTo;
            TaskBar_Story.Begin();
            TaskbarContorEdit(null, EventArgs.Empty);
        }

        /// <summary>
        ///   向下切屏
        /// </summary>
        /// <param name = "sender"></param>
        /// <param name = "e"></param>
        private void TaskBarUpClick(object sender, RoutedEventArgs e)
        {
            TaskBar_Down.IsEnabled = false;
            TaskBar_Up.IsEnabled = false;
            //--向上移动--
            _mChildFrom = _mChildTo;
            _mChildTo = _mChildTo + TaskItemHeight;
            TaskBar_Story_From.Value = _mChildFrom;
            TaskBar_Story_To.Value = _mChildTo;
            TaskBar_Story.Begin();
            TaskbarContorEdit(null, EventArgs.Empty);
        }

        private void BtnShowDesktopClick(object sender, RoutedEventArgs e)
        {
            WindowsManager.Instance.ShowDesktop();
        }

        private void OpenAppButtonClick(object sender, RoutedEventArgs e)
        {
            var ooapp = new OpenOtherApplicationInfo
                {
                ApplicationID = "App.Demo",
                Param = (new AppInterfaceData
                    {
                    EntityTypeID = string.Empty
                }).ToJson()
            };
            ApplicationManager.Instance.OpenOtherApplication(ooapp);
        }
    }
}
