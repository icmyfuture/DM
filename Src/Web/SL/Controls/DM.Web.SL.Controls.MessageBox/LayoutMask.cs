using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DM.Web.SL.Controls.MessageBox
{
    public class LayoutMask
    {
        #region Fields

        private readonly Canvas m_Mask = new Canvas();
        private readonly Panel m_Owner;

        #endregion Fields

        #region Constructors

        public LayoutMask(Panel owner)
        {
            m_Owner = owner;
        }

        #endregion Constructors

        #region Properties

        public int BoxCount
        {
            get
            {
                return Boxes.Count();
            }
        }

        public virtual bool IsRendered
        {
            get
            {
                return (MaskPanel.Parent != null);
            }
        }

        public virtual Canvas MaskPanel
        {
            get
            {
                return m_Mask;
            }
        }

        public int MaxZIndex
        {
            get
            {
                int zIndex = 0;
                if (m_Mask.Children.Count > 0)
                {
                    zIndex = m_Mask.Children.Select(element => Canvas.GetZIndex(element)).Max();
                }
                if (zIndex >= 30000)
                {
                    //ZIndex的最大值是30000多一些
                    //因此如果最大的ZIndex已经超过了30000，则未来很可能超过最大值
                    //处理方案是超过30000后重新排序ZIndex值
                    ReorderZIndex();
                }
                return zIndex;
            }
        }

        protected virtual IEnumerable<IPopupBox> Boxes
        {
            get
            {
                return m_Mask.Children.OfType<IPopupBox>();
            }
        }

        protected bool IsModal
        {
            get
            {
                if (MaskPanel.Children.Count == 0)
                {
                    return false;
                }
                //只要有一个弹出窗口是Modal的，则此层应该是Modal的
                return Boxes.Any(box => box.IsModal);
            }
        }

        protected virtual Panel Owner
        {
            get
            {
                return m_Owner;
            }
        }

        #endregion Properties

        #region Methods

        public void AddBox(IPopupBox box)
        {
            box.Mask = this;
            MaskPanel.Children.Add(box.Element);

            CheckModal();

            //如果Mask中没有其他的弹出窗，则此时Mask并没有显示
            //要将Mask显示出来
            if (IsRendered == false)
            {
                RenderMask();
            }
        }

        public void PositionBox(IPopupBox box)
        {
            //计算左右距离
            double left = (MaskPanel.ActualWidth - box.Element.ActualWidth) / 2;
            double top = (MaskPanel.ActualHeight - box.Element.ActualHeight) / 2;

            PositionBox(box, left, top);
        }

        public void PositionBox(IPopupBox box, double left, double top)
        {
            //位置回归会最终造成死递归
            //因此暂时取消此功能
            //double right = left + box.Element.ActualWidth;
            //double bottom = left + box.Element.ActualHeight;
            //bool isOutSideMask =
            //    ((right > MaskPanel.ActualWidth) || (top > MaskPanel.ActualHeight));
            //if (isOutSideMask)
            //{
            //    PositionBox(box, 0, 0);
            //    return;
            //}

            bool hasBoxOnThisPosition = MaskPanel.Children.Any(
                element => (Canvas.GetLeft(element) == left && Canvas.GetTop(element) == top));
            if (hasBoxOnThisPosition)
            {
                //在这个位置已经有弹出窗口
                //所以要移动当前需要加上去的窗口
                //策略是向右下角移动以便原来的窗口的标题栏可以显示出来
                //窗口的标题栏高度一般是24
                PositionBox(box, left + 24, top + 24);
            }
            else
            {
                Canvas.SetLeft(box.Element, left);
                Canvas.SetTop(box.Element, top);
            }
        }

        public void RemoveBox(IPopupBox box)
        {
            box.Mask = null;
            MaskPanel.Children.Remove(box.Element);

            CheckModal();

            //如果没有子元素则从上级中移除
            if (MaskPanel.Children.Count == 0)
            {
                Owner.Children.Remove(MaskPanel);
            }
        }

        protected virtual void CheckModal()
        {
            if (IsModal)
            {
                SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(100, 255, 255, 255));
                MaskPanel.Background = brush;
            }
            else
            {
                MaskPanel.Background = null;
            }
        }

        protected virtual void RemoveMask()
        {
            Owner.Children.Remove(MaskPanel);
        }

        protected virtual void RenderMask()
        {
            CheckModal();

            Owner.UpdateLayout();
            MaskPanel.Width = Owner.ActualWidth;
            MaskPanel.Height = Owner.ActualHeight;
            MaskPanel.UpdateLayout();
            Owner.Children.Add(MaskPanel);
            Canvas.SetZIndex(MaskPanel, (Int16.MaxValue - 1));
        }

        protected virtual void ReorderZIndex()
        {
            //先将所有控件按ZIndex从小到大排序
            IEnumerable<UIElement> elements =
                m_Mask.Children.OrderBy(element => Canvas.GetZIndex(element));
            //从1开始将控件的ZIndex改变为递增序列
            int currentZIndex = 1;
            int zIndexOfLastElement = 0;
            foreach (UIElement element in elements)
            {
                //如果当前控件与上一控件的ZIndex相同，说明他们在同一层
                //则不需要递增地设置ZIndex
                //相反则需要设置ZIndex递增
                if (zIndexOfLastElement != Canvas.GetZIndex(element))
                {
                    currentZIndex++;
                }
                Canvas.SetZIndex(element, currentZIndex);
            }
        }

        #endregion Methods
    }
}