using System.Windows;
using System.Windows.Controls;

namespace DM.Web.SL.Controls.Guide
{
    /// <summary>
    /// 向导父类
    /// </summary>
    public abstract class GuideBase : UserControl
    {
        #region Fields

        /// <summary>
        /// 当前选中步骤更改依赖属性
        /// </summary>
        private static readonly DependencyProperty SelectStepIndexProperty;

        /// <summary>
        /// 向导页面
        /// </summary>
        private readonly GuideControl guideControl;

        /// <summary>
        /// 用户向导控件类
        /// </summary>
        private readonly GuideChild userGuideChild;

        /// <summary>
        /// 全局保存的值
        /// </summary>
        private object saveValue;

        #endregion Fields

        #region Constructors

        static GuideBase()
        {
            SelectStepIndexProperty = DependencyProperty.Register("SelectStepIndex", typeof(int), typeof(GuideBase), new PropertyMetadata(new PropertyChangedCallback(GuideBase.OnSelectStepIndexPropertyChangedCallback)));
        }

        protected GuideBase()
        {
            guideControl = new GuideControl();
            userGuideChild = new GuideChild(guideControl);
            base.Content = guideControl;
            this.SelectStepIndex = -1;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// 向导控件
        /// </summary>
        public Accordion AccordionControl
        {
            get
            {
                return guideControl.contentControl;
            }
        }

        /// <summary>
        /// 子控件集合
        /// </summary>
        public GuideChild Children
        {
            get { return userGuideChild; }
        }

        /// <summary>
        /// 全局保存的值
        /// </summary>
        public object SaveValue
        {
            set
            {
                this.saveValue = value;
            }
            get
            {
                return this.saveValue;
            }
        }

        /// <summary>
        /// 当前选中步骤
        /// </summary>
        public int SelectStepIndex
        {
            get
            {
                return (int)base.GetValue(SelectStepIndexProperty);
            }
            set
            {
                base.SetValue(SelectStepIndexProperty, value);
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 后退一步
        /// </summary>
        public abstract void BackGuide();

        /// <summary>
        /// 获取当前步骤控件
        /// </summary>
        /// <returns></returns>
        public abstract GuideUIElement GetCurrentStepControl();

        /// <summary>
        /// 获取指定步骤控件
        /// </summary>
        /// <param name="stepNumber">步骤数</param>
        /// <returns></returns>
        public abstract GuideUIElement GetStepControl(int stepNumber);

        /// <summary>
        /// 移至最后一个步骤
        /// </summary>
        /// <returns></returns>
        public abstract void MoveEndStep();

        /// <summary>
        /// 移至第一个步骤
        /// </summary>
        public abstract void MoveFirstStep();

        /// <summary>
        /// 移至指定步骤
        /// </summary>
        /// <param name="selectStepIndex">步骤数字</param>
        public abstract void MoveToStep(int selectStepIndex);

        /// <summary>
        /// 移至指定步骤
        /// </summary>
        /// <param name="title">步骤标题</param>
        public abstract void MoveToStep(string title);

        /// <summary>
        /// 前进一步
        /// </summary>
        public abstract void NextGuide();

        /// <summary>
        /// 回调选中步骤更改依赖属性方法
        /// </summary>
        /// <param name="eventArgs">更改对象事件参数</param>
        protected abstract void OnSelectStepIndexChanged(DependencyPropertyChangedEventArgs eventArgs);

        private static void OnSelectStepIndexPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs eventArgs)
        {
            ((GuideBase)d).OnSelectStepIndexChanged(eventArgs);
        }

        #endregion Methods
    }
}