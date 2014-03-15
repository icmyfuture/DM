using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;

namespace DM.Web.SL.Controls.Guide
{
    /// <summary>
    /// 用户向导控件类
    /// </summary>
    public class GuideChild : GuideChildBase, IProjectable
    {
        #region Fields

        /// <summary>
        /// 用户向导控件集合
        /// </summary>
        private readonly List<GuideUIElement> elmentList = null;

        /// <summary>
        /// 向导控件
        /// </summary>
        private readonly GuideControl guideControl;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        public GuideChild(GuideControl guideControl)
        {
            this.guideControl = guideControl;
            elmentList = new List<GuideUIElement>();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// 数量
        /// </summary>
        public int Count
        {
            get
            {
                return elmentList.Count;
            }
        }

        /// <summary>
        /// 父对象
        /// </summary>
        public GuideBase ParentMap
        {
            get
            {
                IProjectable parent = guideControl.Parent as IProjectable;
                if (parent != null)
                {
                    return parent.ParentMap;
                }
                GuideBase base2 = guideControl.Parent as GuideBase;
                if (base2 != null)
                {
                    return base2;
                }
                ItemsPresenter reference = VisualTreeHelper.GetParent(guideControl) as ItemsPresenter;
                if (reference != null)
                {
                    IProjectable projectable2 = VisualTreeHelper.GetParent(reference) as IProjectable;
                    if (projectable2 != null)
                    {
                        return projectable2.ParentMap;
                    }
                }
                throw new InvalidOperationException(Resources.ExceptionStrings.IProjectable);
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 添加步骤控件
        /// </summary>
        /// <param name="guideUIElement">步骤控件</param>
        public override void AddChild(GuideUIElement guideUIElement)
        {
            SetChildControlToGuide(guideUIElement,true);
        }

        /// <summary>
        /// 添加步骤控件
        /// </summary>
        /// <param name="guideUIElement">步骤控件</param>
        /// <param name="isShowTitleNum">控制是否显示标题标号</param>
        public override void AddChild(GuideUIElement guideUIElement, bool isShowTitleNum)
        {
            SetChildControlToGuide(guideUIElement, isShowTitleNum);
        }

        /// <summary>
        /// 添加步骤控件集合
        /// </summary>
        /// <param name="guideUIElementList">步骤控件集合</param>
        public override void AddChild(List<GuideUIElement> guideUIElementList)
        {
            foreach (var guideUIElement in guideUIElementList)
            {
                SetChildControlToGuide(guideUIElement,true);
            }
        }

        /// <summary>
        /// 获取当前步骤控件
        /// </summary>
        /// <param name="selectStepIndex">当前步骤索引</param>
        /// <returns></returns>
        internal override GuideUIElement CurrentStepControl(int selectStepIndex)
        {
            if (elmentList.Count < 1)
            {
                throw new StepIndexInvalidException(Resources.ExceptionStrings.StepIndexInvaild_Null, new IndexOutOfRangeException());
            }
            return elmentList[selectStepIndex];
        }

        /// <summary>
        /// 展开指定步骤
        /// </summary>
        /// <param name="selectIndex">步骤索引</param>
        internal override void ShowSelectIndexExpender(int selectIndex)
        {
            foreach (var info in elmentList)
            {
                if (info.StepIndex == selectIndex)
                {
                    info.IsEnabled = true;
                    info.OpenExpanded();
                }
                else
                {
                    info.IsEnabled = false;
                }
            }
        }

        /// <summary>
        /// 展开指定步骤
        /// </summary>
        /// <param name="title">步骤标题</param>
        internal override void ShowSelectIndexExpender(string title)
        {
            int setpIndex = 0;
            foreach (var info in elmentList)
            {
                if (info.StepTitle == title)
                {
                    this.ParentMap.SelectStepIndex = setpIndex;
                }
                setpIndex++;
            }
        }

        /// <summary>
        /// 设置控件到向导上
        /// </summary>
        private void SetChildControlToGuide(GuideUIElement guideUIElement,bool isShowTitleNum)
        {
            //设置IGuide接口
            var iguide = guideUIElement.UIElementControl as IGuide;
            if (iguide != null)
                iguide.ParentGuide = ParentMap;
            //添加到List集合中
            elmentList.Add(guideUIElement);
            //设置步骤属性
            guideUIElement.stepIndex = elmentList.Count - 1;
            //设置标题
            guideUIElement.Header = new DefaultStepTitle(guideUIElement.StepIndex, guideUIElement.StepTitle).GetStepTitle(isShowTitleNum);
            //设置控件到向导上
            guideControl.contentControl.Items.Add(guideUIElement);
        }

        /// <summary>
        /// 清空所有步骤
        /// </summary>
        public override void Clear()
        {
            if (guideControl != null)
                guideControl.contentControl.Items.Clear();
            if (elmentList != null)
                elmentList.Clear();
            if (ParentMap != null)
                ParentMap.SaveValue = null;
        }

        #endregion Methods
        
    }
}