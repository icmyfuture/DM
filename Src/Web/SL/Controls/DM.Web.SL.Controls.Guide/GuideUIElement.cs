using System.Windows.Controls;

namespace DM.Web.SL.Controls.Guide
{
    /// <summary>
    /// 向导步骤控件类
    /// </summary>
    public class GuideUIElement : GuideUIElementBase
    {
        #region Fields

        /// <summary>
        /// 步骤索引
        /// </summary>
        internal int stepIndex;

        /// <summary>
        /// 步骤标题
        /// </summary>
        private string stepTitle;

        #endregion Fields

        #region Constructors

        public GuideUIElement()
        {
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// 当前步骤
        /// </summary>
        public int StepIndex
        {
            get
            {
                return this.stepIndex;
            }
        }

        /// <summary>
        /// 步骤标题
        /// </summary>
        public override string StepTitle
        {
            set { this.stepTitle = value; }
            get { return this.stepTitle; }
        }

        /// <summary>
        /// 控件
        /// </summary>
        public override UserControl UIElementControl
        {
            get
            {
                return base.Content as UserControl;
            }
            set
            {
                base.Content = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 收缩当前步骤
        /// </summary>
        internal override void CloseExpanded()
        {
            base.IsSelected = false;
        }

        /// <summary>
        /// 展开当前步骤
        /// </summary>
        internal override void OpenExpanded()
        {
            base.IsSelected = true;
            UIElementControl.Focus();
        }

        #endregion Methods
    }
}