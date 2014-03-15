using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DM.Common.Utility;
using DM.Common.WPF;

namespace DM.Client.WPF.Controls.Paging
{
    /// <summary>
    /// Interaction logic for PagerControl.xaml
    /// </summary>
    public partial class PagerControl : UserControl
    {
        #region Fields

        private long curpage = 1;
        private bool isShowFirstPage;
        private bool isShowLastPage;
        private bool isShowNextPage;
        private bool isShowPrePage;
        private int pageSize = 10;
        private long totalCount = 0;

        /// <summary>
        /// 总页数
        /// </summary>
        public long TotalPagesCount
        {
            get;
            set;
        }
        #endregion Fields

        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        public PagerControl()
        {
            InitializeComponent();
            LanguageHelper.ReadLanguageData("DM.Client.WPF");
            ToolTipServiceHelper.SetToolTip(Refresh, LanguageHelper.GetDictionary("Refresh", "Refresh"));
            ToolTipServiceHelper.SetToolTip(FristPage, LanguageHelper.GetDictionary("FristPage", "First Page"));
            ToolTipServiceHelper.SetToolTip(PrePage, LanguageHelper.GetDictionary("PrePage", "Previous"));
            ToolTipServiceHelper.SetToolTip(NextPage, LanguageHelper.GetDictionary("NextPage", "Next"));
            ToolTipServiceHelper.SetToolTip(LastPage, LanguageHelper.GetDictionary("LastPage", "Last Page"));
            Txt_Total.Text = LanguageHelper.GetDictionary("txtTotal", "Total");
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// 当前页
        /// </summary>
        public long CurrentPageIndex
        {
            get
            {
                return this.curpage;
            }
            set
            {
                this.CurrentPage.Text = value.ToString();
                SetCurrentPageToolTip(this.CurrentPage.Text);
                if (curpage != value)
                {
                    curpage = value;
                    if (this.PageCountChangeExternalHandler != null)
                    {
                        this.PageCountChangeExternalHandler(this, null);
                    }
                }
            }
        }

        /// <summary>
        /// 页数更改外部处理事件
        /// </summary>
        public EventHandler PageCountChangeExternalHandler
        {
            get;
            set;
        }

        /// <summary>
        /// PageSize
        /// </summary>
        public int PageSize
        {
            get
            {
                return pageSize;
            }
            set
            {
                if (pageSize != value)
                {
                    pageSize = value;
                    ComputeTotalPageCount();

                    //CurrentPageIndex = 1;
                    //if (PageCountChangeExternalHandler != null)
                    //{
                    //    PageCountChangeExternalHandler(this, null);
                    //}
                }
            }
        }

        /// <summary>
        /// 是否显示第一页按钮
        /// </summary>
        public bool ShowFirstPage
        {
            get
            {
                return isShowFirstPage;
            }
            set
            {
                isShowFirstPage = value;
                if (isShowFirstPage)
                {
                    this.FristPage.Visibility = Visibility.Visible;
                }
                else
                {
                    this.FristPage.Visibility = Visibility.Collapsed;
                }
            }
        }

        /// <summary>
        /// 是否显示最后一页按钮
        /// </summary>
        public bool ShowLastPage
        {
            get
            {
                return isShowLastPage;
            }
            set
            {
                isShowLastPage = value;
                if (isShowLastPage)
                {
                    this.LastPage.Visibility = Visibility.Visible;
                }
                else
                {
                    this.LastPage.Visibility = Visibility.Collapsed;
                }
            }
        }

        /// <summary>
        /// 是否显示下一页按钮
        /// </summary>
        public bool ShowNextPage
        {
            get
            {
                return isShowNextPage;
            }
            set
            {
                isShowNextPage = value;
                if (isShowNextPage)
                {
                    this.NextPage.Visibility = Visibility.Visible;
                }
                else
                {
                    this.NextPage.Visibility = Visibility.Collapsed;
                }
            }
        }

        /// <summary>
        /// 是否显示上一页按钮
        /// </summary>
        public bool ShowPrePage
        {
            get
            {
                return isShowPrePage;
            }
            set
            {
                isShowPrePage = value;
                if (isShowPrePage)
                {
                    this.PrePage.Visibility = Visibility.Visible;
                }
                else
                {
                    this.PrePage.Visibility = Visibility.Collapsed;
                }
            }
        }

        /// <summary>
        /// 总数
        /// </summary>
        public long TotalItemsCount
        {
            get
            {
                return totalCount;
            }
            set
            {
                if (value != totalCount)
                {
                    totalCount = value;
                    lb_totalCount.Text = totalCount.ToString();
                    ComputeTotalPageCount();
                }
            }
        }

        /// <summary>
        /// 刷新按钮是否可见
        /// </summary>
        public bool RefreshVisibility
        {
            get
            {
                return Refresh.Visibility == System.Windows.Visibility.Visible ? true : false;
            }
            set
            {
                Refresh.Visibility = value ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            }
        }

        /// <summary>
        /// 刷新按钮是否可用
        /// </summary>
        public bool RefreshEnlable
        {
            get
            {
                return Refresh.IsEnabled;
            }
            set
            {
                Refresh.IsEnabled = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 计算总的页数
        /// </summary>
        private void ComputeTotalPageCount()
        {
            long temp = TotalItemsCount % PageSize;
            TotalPagesCount = TotalItemsCount / PageSize;
            if (temp > 0)
            {
                TotalPagesCount += 1;
            }

            if (TotalPagesCount == 0)
            {
                TotalPagesCount = 1;
            }

            ////当前页大于总页数时,跳转到上一页
            //if (CurrentPageIndex > this.TotalPagesCount)
            //{
            //    //CurrentPageIndex = 1;
            //    if (CurrentPageIndex > 1)
            //    {
            //        CurrentPageIndex = CurrentPageIndex - 1;
            //    }
            //}

            ////2011-09-05 JiangLei 修改查询后，不能返回第一页的问题
            //CurrentPageIndex = 1;

            //2011-09-09 JiangLei 因为审片后要加载上一条任务，所以不返回第一页，而留在当前页；超出时返回最后一页
            if (CurrentPageIndex > this.TotalPagesCount)
            {
                CurrentPageIndex = this.TotalPagesCount;
            }

            if (this.totalpage != null)
            {
                this.totalpage.Text = TotalPagesCount.ToString();
            }
        }

        /// <summary>
        /// 用户输入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CurrentPage_KeyUp(object sender, KeyEventArgs e)
        {
            if (string.IsNullOrEmpty(this.CurrentPage.Text.Trim()))
            {
                return;
            }

            string str = "^[1-9][0-9]*$";
            if (!Regex.IsMatch(this.CurrentPage.Text.Trim(), str, RegexOptions.IgnoreCase))
            {
                this.CurrentPage.Text = CurrentPageIndex.ToString();
                return;
            }

            long pages = long.Parse(this.CurrentPage.Text.Trim());
            if (pages > TotalPagesCount)
            {
                CurrentPageIndex = TotalPagesCount;
            }
            else if (pages < 1)
            {
                CurrentPageIndex = 1;
            }
            else
            {
                CurrentPageIndex = pages;
            }

            SetCurrentPageToolTip(this.CurrentPage.Text);
        }

        private void SetCurrentPageToolTip(string toolTipValue)
        {
            ToolTipServiceHelper.SetToolTip(this.CurrentPage, toolTipValue);
        }

        /// <summary>
        /// 第一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FristPage_Click(object sender, RoutedEventArgs e)
        {
            CurrentPageIndex = 1;
        }

        /// <summary>
        /// 最后一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LastPage_Click(object sender, RoutedEventArgs e)
        {
            CurrentPageIndex = TotalPagesCount;
        }

        /// <summary>
        /// 下一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            long tempPage = CurrentPageIndex + 1;
            if (tempPage > TotalPagesCount)
            {
                return;
            }

            CurrentPageIndex = tempPage;
        }

        /// <summary>
        /// 上一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrePage_Click(object sender, RoutedEventArgs e)
        {
            long tempPage = CurrentPageIndex - 1;
            if (tempPage < 1)
            {
                return;
            }

            CurrentPageIndex = tempPage;
        }

        /// <summary>
        /// 刷新事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            if (PageCountChangeExternalHandler != null)
            {
                //CurrentPageIndex = 1;
                PageCountChangeExternalHandler(this, null);
            }
        }

        /// <summary>
        /// 重置当前页为1
        /// </summary>
        public void ResetPageIndex()
        {
            this.curpage = 1;
            this.CurrentPage.Text = curpage.ToString();
        }

        #endregion Methods

        #region IDisposable Members

        public void Dispose()
        {
            CurrentPage.KeyUp -= new KeyEventHandler(CurrentPage_KeyUp);
            Refresh.Click -= new RoutedEventHandler(Refresh_Click);
            FristPage.Click -= new RoutedEventHandler(FristPage_Click);
            LastPage.Click -= new RoutedEventHandler(LastPage_Click);
            NextPage.Click -= new RoutedEventHandler(NextPage_Click);
            PrePage.Click -= new RoutedEventHandler(PrePage_Click);
        }

        #endregion
    }
}
