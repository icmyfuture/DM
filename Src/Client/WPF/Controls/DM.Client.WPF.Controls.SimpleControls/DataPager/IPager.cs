#region Import

using System;

#endregion

namespace DM.Client.WPF.Controls.SimpleControls.DataPager
{
    /// <summary>
    /// 分页接口
    /// </summary>
    public interface IPager
    {

        /// <summary>
        /// 当前页索引
        /// </summary>
        int PageIndex
        {
            get;
        }

        /// <summary>
        /// 每页条目数
        /// </summary>
        int PageSize
        {
            get;
        }

        /// <summary>
        /// 总共页数目
        /// </summary>
        int PageCount
        {
            get;
        }

        /// <summary>
        /// 总共条目数
        /// </summary>
        int RecorderCount
        {
            get;
        }

        /// <summary>
        /// 到指定页
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <exception cref="PageIndexNotFoundException">当页索引超出范围没有找到将抛出此异常</exception>
        void ToPageIndex(int pageIndex);

        /// <summary>
        /// 也索引更改时触发
        /// </summary>
        event EventHandler<PageIndexChangeEventArgs> PageIndexChange;

    }

}
