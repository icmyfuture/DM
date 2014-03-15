#region Import

using System;

#endregion

namespace DM.Client.WPF.Controls.SimpleControls.DataPager
{
    /// <summary>
    /// 页索引更改事件
    /// </summary>
    public class PageIndexChangeEventArgs : EventArgs
    {

        #region Constructors

        /// <summary>
        /// 狗奥函数
        /// </summary>
        /// <param name="pageIndex"></param>
        public PageIndexChangeEventArgs(int pageIndex)
        {
            PageIndex = pageIndex;
        }

        #endregion

        #region Properties

        /// <summary>
        /// 页索引
        /// </summary>
        public int PageIndex
        {
            private set;
            get;
        }

        #endregion

    }
}
