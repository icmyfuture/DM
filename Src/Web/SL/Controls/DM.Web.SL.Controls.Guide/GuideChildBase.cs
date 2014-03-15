using System.Collections.Generic;

namespace DM.Web.SL.Controls.Guide
{
    /// <summary>
    /// 用户向导控件父类
    /// </summary>
    public abstract class GuideChildBase
    {
        #region Constructors

        protected GuideChildBase()
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// 添加步骤控件
        /// </summary>
        /// <param name="guideUIElement">步骤控件</param>
        public abstract void AddChild(GuideUIElement guideUIElement);

        /// <summary>
        /// 添加步骤控件
        /// </summary>
        /// <param name="guideUIElement">步骤控件</param>
        public abstract void AddChild(GuideUIElement guideUIElement,bool isShowTitleNum);

        /// <summary>
        /// 添加步骤控件集合
        /// </summary>
        /// <param name="guideUIElementList">步骤控件集合</param>
        public abstract void AddChild(List<GuideUIElement> guideUIElementList);

        /// <summary>
        /// 获取当前步骤控件
        /// </summary>
        /// <param name="selectStepIndex">当前步骤索引</param>
        /// <returns></returns>
        internal abstract GuideUIElement CurrentStepControl(int selectStepIndex);

        /// <summary>
        /// 展开指定步骤
        /// </summary>
        /// <param name="selectIndex">步骤索引</param>
        internal abstract void ShowSelectIndexExpender(int selectIndex);

        /// <summary>
        /// 展开指定步骤
        /// </summary>
        /// <param name="title">步骤标题</param>
        internal abstract void ShowSelectIndexExpender(string title);

        /// <summary>
        /// 清空所有步骤
        /// </summary>
        public abstract void Clear();

        #endregion Methods
    }
}