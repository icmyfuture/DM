using System.Windows;
using System.Windows.Controls;

namespace DM.Web.SL.Controls.Guide
{
    /// <summary>
    /// 向导步骤控件父类
    /// </summary>
    public abstract class GuideUIElementBase : AccordionItem
    {
        #region Constructors

        protected GuideUIElementBase()
        {
            base.IsEnabled = false;
            base.Margin = new Thickness(5, 5, 5, 5);
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// 步骤标题
        /// </summary>
        public abstract string StepTitle
        {
            set; get;
        }

        /// <summary>
        /// 用户控件
        /// </summary>
        public abstract UserControl UIElementControl
        {
            set; get;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 收缩当前步骤
        /// </summary>
        internal abstract void CloseExpanded();

        /// <summary>
        /// 展开当前步骤
        /// </summary>
        internal abstract void OpenExpanded();

        #endregion Methods
    }
}