namespace DM.Web.SL.Controls.Guide
{
    /// <summary>
    /// 向导类
    /// </summary>
    public class Guide : GuideBase
    {
        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public Guide()
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// 后退一步
        /// </summary>
        public override void BackGuide()
        {
            if (base.SelectStepIndex <= 0)
            {
                return;
            }
            base.SelectStepIndex -= 1;
        }

        /// <summary>
        /// 获取当前步骤控件
        /// </summary>
        /// <returns></returns>
        public override GuideUIElement GetCurrentStepControl()
        {
            return base.Children.CurrentStepControl(this.SelectStepIndex);
        }

        /// <summary>
        /// 获取指定步骤
        /// </summary>
        /// <param name="stepNumber">步骤数</param>
        /// <returns></returns>
        public override GuideUIElement GetStepControl(int stepNumber)
        {
            return base.Children.CurrentStepControl(stepNumber);
        }

        /// <summary>
        /// 移至最后一个步骤
        /// </summary>
        /// <returns></returns>
        public override void MoveEndStep()
        {
            base.SelectStepIndex = base.Children.Count - 1;
        }

        /// <summary>
        /// 移至第一个步骤
        /// </summary>
        public override void MoveFirstStep()
        {
            base.SelectStepIndex = 0;
        }

        /// <summary>
        /// 移至指定步骤
        /// </summary>
        /// <param name="selectStepIndex">步骤数字</param>
        public override void MoveToStep(int selectStepIndex)
        {
            if (base.Children.Count > selectStepIndex && selectStepIndex >= 0)
            {
                base.SelectStepIndex = selectStepIndex;
                base.Children.ShowSelectIndexExpender(base.SelectStepIndex);
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// 移至指定步骤
        /// </summary>
        /// <param name="title">步骤标题</param>
        public override void MoveToStep(string title)
        {
            base.Children.ShowSelectIndexExpender(title);
        }

        /// <summary>
        /// 前进一步
        /// </summary>
        public override void NextGuide()
        {
            if (base.Children.Count - 1 == base.SelectStepIndex)
            {
                return;
            }
            base.SelectStepIndex += 1;
        }

        /// <summary>
        /// 步骤变化回调函数
        /// </summary>
        /// <param name="eventArgs">事件参数</param>
        protected override void OnSelectStepIndexChanged(System.Windows.DependencyPropertyChangedEventArgs eventArgs)
        {
            var stepNum = (int)eventArgs.NewValue;
            MoveToStep(stepNum);
        }

        #endregion Methods
    }
}