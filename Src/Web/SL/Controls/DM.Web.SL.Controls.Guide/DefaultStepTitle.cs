namespace DM.Web.SL.Controls.Guide
{
    /// <summary>
    /// 默认标题样式实现
    /// </summary>
    public class DefaultStepTitle
    {
        #region Constructors

        /// <summary>
        /// 构造函数(带参)
        /// </summary>
        /// <param name="stepIndex">步骤索引</param>
        /// <param name="title">标题</param>
        public DefaultStepTitle(int stepIndex, string title)
        {
            this.StepIndex = stepIndex;
            this.Title = title;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// 步骤索引
        /// </summary>
        public int StepIndex
        {
            set;
            get;
        }

        /// <summary>
        /// 步骤标题
        /// </summary>
        public string Title
        {
            set;
            get;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 获取步骤标题
        /// </summary>
        /// <returns></returns>
        public string GetStepTitle(bool isShowTitleNum)
        {
            return isShowTitleNum == true ? (this.StepIndex + 1) + "." + Title : Title;
        }

        #endregion Methods
    }
}