using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DM.Web.SL.Common.Core.DragDrop.Primitives;

namespace DM.Web.SL.Common.Core.DragDrop
{
    #region Imports

    

    #endregion

    /// <summary>
    ///   A simple <see cref = "DragCursor" /> that drags an element around the screen using 
    ///   a <see cref = "Transform" />.
    /// </summary>
    public class ElementTransformDragCursor : DragCursor
    {
        #region Fields

        private readonly UIElement m_dragElement;
        private readonly bool m_useEffectVisualStates;
        private Point m_dragOffset;

        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dragElement"></param>
        public ElementTransformDragCursor( UIElement dragElement )
            : this( dragElement, false )
        { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dragElement"></param>
        /// <param name="enableVisualStates"></param>
        public ElementTransformDragCursor( UIElement dragElement, bool enableVisualStates )
        {
            #region Validate Arguments

            Guard.ArgumentNull( "dragElement", dragElement );

            #endregion

            m_dragElement = dragElement;
            m_useEffectVisualStates = enableVisualStates;
        } 
        #endregion

        /// <summary>
        /// DragVisualRoots
        /// </summary>
        public override IEnumerable<UIElement> DragVisualRoots
        {
            get { yield return DragElement; }
        }

        protected UIElement DragElement
        {
            get { return m_dragElement; }
        }

        protected Point DragOffset
        {
            get { return m_dragOffset; }
        }

        protected bool UseEffectVisualStates
        {
            get { return m_useEffectVisualStates; }
        }

        private bool IsEffectSet(DragDropEffects effect)
        {
            return ((Effects & effect) == effect);
        }

        protected override void OnBeginDrag(IMouseEventArgs args)
        {
            m_dragOffset = args.GetPosition(m_dragElement);
        }

        protected override void OnDrag(IMouseEventArgs args)
        {
            //	get the position based on the drag cursor
            var movePoint = args.GetPosition(m_dragElement);

            //	get translate transform
            var translateTransform = m_dragElement.RenderTransform as TranslateTransform ?? new TranslateTransform();

            //	adjust coordinates 
            movePoint = translateTransform.Transform(movePoint);
            movePoint.X -= m_dragOffset.X;
            movePoint.Y -= m_dragOffset.Y;

            //	update transform
            translateTransform.X = movePoint.X;
            translateTransform.Y = movePoint.Y;
            m_dragElement.RenderTransform = translateTransform;
        }

        protected override void OnEndDrag()
        {
            m_dragElement.RenderTransform = null;
        }

        protected override void OnEffectsChanged()
        {
            base.OnEffectsChanged();

            // use visual states?
            if (!m_useEffectVisualStates)
            {
                return;
            }

            // drag element must be a control
            var control = DragElement as Control;
            if (control == null)
            {
                return;
            }

            var visualState = DragVisualStates.DragNone;
            if (Effects != DragDropEffects.None)
            {
                visualState = DragVisualStates.DragAll;
                if (m_useEffectVisualStates)
                {
                    //	set effects based on logical priority of effects
                    if (IsEffectSet(DragDropEffects.Copy))
                    {
                        visualState = DragVisualStates.DragCopy;
                    }
                    else if (IsEffectSet(DragDropEffects.Move))
                    {
                        visualState = DragVisualStates.DragMove;
                    }
                    else if (IsEffectSet(DragDropEffects.Link))
                    {
                        visualState = DragVisualStates.DragLink;
                    }
                }
            }
            VisualStateManager.GoToState(control, visualState, true);
        }
    }

    #region Nested type: 拖拽视觉状态
    /// <summary>
    /// 拖拽视觉状态
    /// </summary>
    public static class DragVisualStates
    {
        ///<summary>
        /// DragAll
        ///</summary>
        public const string DragAll = "Drag All";
        ///<summary>
        /// DragNone
        ///</summary>
        public const string DragNone = "Drag None";
        ///<summary>
        /// DragCopy
        ///</summary>
        public const string DragCopy = "Drag Copy";
        ///<summary>
        /// DragMove
        ///</summary>
        public const string DragMove = "Drag Move";
        ///<summary>
        /// DragLink
        ///</summary>
        public const string DragLink = "Drag Link";
    }

    #endregion

    #region Nested type: 拖放视觉状态
    /// <summary>
    /// 拖放视觉状态
    /// </summary>
    public static class DropVisualStates
    {
        /// <summary>
        /// DropTarget
        /// </summary>
        public const string DropTarget = "Drop Target";
        /// <summary>
        /// DropNormal
        /// </summary>
        public const string DropNormal = "Drop Normal";
    }

    #endregion
}