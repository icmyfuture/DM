#region Import

using System;
using System.Windows;
using System.Windows.Controls;

#endregion

namespace DM.Client.WPF.Controls.SimpleControls.DataPager
{
    /// <summary>
    /// 分页控件
    /// </summary>
    public class BaseDataPager : UserControl, IPager
    {

        #region Fields

        /// <summary>
        /// 每页条目数，这是一个依赖属性。
        /// </summary>
        private static readonly DependencyProperty PageSizeProperty = DependencyProperty.Register("PageSize", typeof(int), typeof(BaseDataPager),
            new PropertyMetadata(0, OnPageSizePropertyChangedCallback));

        /// <summary>
        /// 总共条目数，这是一个依赖属性。
        /// </summary>
        private static readonly DependencyProperty RecorderCountProperty = DependencyProperty.Register("RecorderCount", typeof(int), typeof(BaseDataPager),
            new PropertyMetadata(0, OnRecorderCountPropertyChangedCallback));

        /// <summary>
        /// 页索引
        /// </summary>
        private int _pageIndex;

        /// <summary>
        /// 总共页数目
        /// </summary>
        private int _pageCount;

        /// <summary>
        /// 锁
        /// </summary>
        private readonly object _lockObject = new object();

        #endregion

        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        protected BaseDataPager()
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// 触发索引更改事件
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnPageIndexChange(PageIndexChangeEventArgs e)
        {
            if (PageIndexChange != null)
                PageIndexChange(this, e);
        }

        /// <summary>
        /// 到指定页
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <exception cref="PageIndexNotFoundException">当页索引超出范围没有找到将抛出此异常</exception>
        public void ToPageIndex(int pageIndex)
        {
            lock (_lockObject)
            {
                if (pageIndex < 0 || pageIndex > PageCount - 1)
                    LogWriteExcption(new PageIndexNotFoundException("PageIndex out of range。"));
                _pageIndex = pageIndex;
                OnPageIndexChange(new PageIndexChangeEventArgs(_pageIndex));
            }
        }

        /// <summary>
        /// 写入异常信息
        /// </summary>
        /// <param name="ex"></param>
        protected virtual void LogWriteExcption(Exception ex)
        {

        }

        /// <summary>
        /// 跳转到第一页
        /// </summary>
        public void ToFirstPageIndex()
        {
            ToPageIndex(0);
        }

        /// <summary>
        /// 跳转到最后一页
        /// </summary>
        public void ToEndPageIndex()
        {
            ToPageIndex(_pageCount - 1);
        }

        /// <summary>
        /// 设置总数
        /// </summary>
        /// <param name="recorderCount">总条目数</param>
        protected virtual void OnRecorderCountChanged(int recorderCount)
        {
            lock (_lockObject)
            {
                _pageCount = recorderCount % PageSize == 0 ? recorderCount / PageSize : (recorderCount / PageSize) + 1;
            }
        }

        /// <summary>
        /// 设置每页条目数
        /// </summary>
        /// <param name="pageSize">分页记录数</param>
        protected virtual void OnPageSizeChanged(int pageSize)
        {
            if (pageSize > 0)
            {
                if (RecorderCount != 0)
                    OnRecorderCountChanged(RecorderCount);
            }
            else
                throw new ArgumentException("PageSize cannot be 0");
        }

        #endregion

        #region Events

        /// <summary>
        /// 页索引更改时触发
        /// </summary>
        public event EventHandler<PageIndexChangeEventArgs> PageIndexChange;

        /// <summary>
        /// 分页记录更改通知
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnPageSizePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((BaseDataPager)d).OnPageSizeChanged((int)e.NewValue);
        }

        /// <summary>
        /// 总条目数更改通知
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnRecorderCountPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((BaseDataPager)d).OnRecorderCountChanged((int)e.NewValue);
        }

        #endregion

        #region Properties

        /// <summary>
        /// 当前页索引
        /// </summary>
        public int PageIndex
        {
            get { return _pageIndex; }
        }

        /// <summary>
        /// 每页条目数，这是一个依赖属性。
        /// </summary>
        public int PageSize
        {
            get
            {
                return (int)GetValue(PageSizeProperty);
            }
            set
            {
                SetValue(PageSizeProperty, value);
            }
        }

        /// <summary>
        /// 总共页数目
        /// </summary>
        public int PageCount
        {
            get { return _pageCount; }
        }

        /// <summary>
        /// 总共条目数，这是一个依赖属性。
        /// </summary>
        public int RecorderCount
        {
            set { SetValue(RecorderCountProperty, value); }
            get { return (int)GetValue(RecorderCountProperty); }
        }

        #endregion

    }
}
