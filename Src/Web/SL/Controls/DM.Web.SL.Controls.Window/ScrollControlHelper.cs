using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using DM.Web.SL.Controls.Window.Entities;

namespace DM.Web.SL.Controls.Window
{
    /// <summary>
    ///   操作类
    /// </summary>
    public class ScrollControlHelper
    {
        #region 单一实例

        /// <summary>
        ///   单一实例
        /// </summary>
        public static readonly ScrollControlHelper Instance = new ScrollControlHelper();

        #endregion

        #region  构造函数

        #endregion  构造函数        

        #region 公共方法

        /// <summary>
        ///   为DataGrid的滚动条绑定事件(默认为窗口的Focus事件)
        /// </summary>
        /// <param name = "dataGrid"></param>
        /// <param name = "windowID"></param>
        public void BindScrollEvent(Control container, string windowID)
        {
            if (container == null)
            {
                return;
            }

            container.UpdateLayout();
            WindowInfo window = WindowsManager.Instance.GetWindow(windowID);
            if (window == null)
            {
                return;
            }

            List<ScrollBar> scrollBars = new List<ScrollBar>();
            FindScrollBar(container, ref scrollBars);
            foreach (ScrollBar item in scrollBars)
            {
                item.Scroll += (obj, e) => { window.Focus(); };
            }
        }

        /// <summary>
        ///   为DataGrid的滚动条绑定自定义事件
        /// </summary>
        /// <param name = "container"></param>
        /// <param name = "handler"></param>
        public void BindScrollEvent(Control container, EventHandler handler)
        {
            List<ScrollBar> scrollBars = new List<ScrollBar>();
            FindScrollBar(container, ref scrollBars);
            foreach (ScrollBar item in scrollBars)
            {
                item.Scroll += (obj, e) =>
                                   {
                                       if (handler != null)
                                       {
                                           handler(scrollBars, null);
                                       }
                                   };
            }
        }

        #endregion

        #region 私有方法

        private void FindScrollBar(DependencyObject obj, ref List<ScrollBar> scrollBars)
        {
            for (int i = VisualTreeHelper.GetChildrenCount(obj) - 1; i >= 0; i--)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is ScrollBar)
                {
                    scrollBars.Add(child as ScrollBar);
                    if (scrollBars.Count > 1)
                    {
                        return;
                    }
                }
                else
                {
                    FindScrollBar(child, ref scrollBars);
                }
            }
        }

        #endregion
    }
}