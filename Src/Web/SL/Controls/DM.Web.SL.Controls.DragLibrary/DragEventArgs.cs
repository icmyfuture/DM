using System;
using System.Windows;

namespace DM.Web.SL.Controls.DragLibrary
{
    /// <summary>
    /// 放下动作事件数据。
    /// </summary>
    /// <typeparam name="T">拖放的数据类型。</typeparam>
    public class DragEventArgs<T> : EventArgs
    {
        #region Fields

        private readonly T _data;
        private readonly FrameworkElement _fromTarget;
        private readonly UIElement _toTarget;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// 构造。
        /// </summary>
        /// <param name="fromTarget">获取拖放开始的目标对象。</param>
        /// <param name="dropedTarget">放下的目标对象</param>
        /// <param name="data">拖放过来的数据。</param>
        public DragEventArgs(FrameworkElement fromTarget, UIElement dropedTarget, T data)
        {
            _toTarget = dropedTarget;
            _fromTarget = fromTarget;
            _data = data;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// 获取拖放过来的数据。
        /// </summary>
        public T Data
        {
            get { return _data; }
        }

        /// <summary>
        /// 获取拖放开始的目标对象。
        /// </summary>
        public FrameworkElement FromTarget
        {
            get { return _fromTarget; }
        }

        /// <summary>
        /// 获取拖放结束的目标对象。
        /// </summary>
        public UIElement ToTarget
        {
            get { return _toTarget; }
        }

        #endregion Properties
    }
}