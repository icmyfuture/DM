using DM.Web.SL.Common.Core.DragDrop;
using DM.Web.SL.Common.Core.DragDrop.Primitives;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using DragEventArgs = DM.Web.SL.Common.Core.DragDrop.DragEventArgs;
using DragEventHandler = DM.Web.SL.Common.Core.DragDrop.DragEventHandler;

namespace DM.Web.SL.Common.Utility
{
    #region Imports

    

    #endregion

    /// <summary>
    ///   提供支持拖放和拖放操作，包括发起拖和拖放支持
    ///   添加/删除与拖放相关的事件处理程序。
    /// </summary>
    public class DragDropHelper
    {
        #region Fields

        ///<summary>
        /// AllowDrop属性
        ///</summary>
        public static readonly DependencyProperty AllowDropProperty = DependencyProperty.RegisterAttached(
            "AllowDrop", typeof (AllowDrop), typeof (DragDropHelper), new PropertyMetadata(AllowDrop.Inherited));

        private static DragDropHelper m_current;
        private readonly DragDropEffects m_allowedEffects;
        private readonly object m_data;

        private readonly DragCursor m_dragCursor;
        private readonly DragOperation m_dragOperation;
        private readonly DependencyObject m_dragSource;
        private UIElement m_currentDropTarget;
        private IMouseEventArgs m_currentMouseArgs;
        private UIElement m_rootVisual;

        #endregion

        private DragDropHelper(DependencyObject dragSource, object data, DragDropEffects allowedEffects, DragCursor dragCursor)
        {
            m_dragSource = dragSource;
            m_data = data;
            m_allowedEffects = allowedEffects;
            m_dragCursor = dragCursor;

            //	create drag operation and bind to events
            m_dragOperation = new DragOperation();
            m_dragOperation.DragStarted += OnDragStarted;
            m_dragOperation.DragDelta += OnDragDelta;
            m_dragOperation.DragCompleted += OnDragCompleted;
        }

        #region 注册和移除拖拽事件
        /// <summary>
        /// 注册拖过
        /// </summary>
        /// <param name="element"></param>
        /// <param name="handler"></param>
        public static void AddDragOverHandler(DependencyObject element, DragEventHandler handler)
        {
            if (handler != null)
            {
                DragDropBehaviour.GetOrCreateBehaviour(element).DragOver += handler;
            }
        }

        /// <summary>
        /// 注册拖入事件
        /// </summary>
        /// <param name="element"></param>
        /// <param name="handler"></param>
        public static void AddDragEnterHandler(DependencyObject element, DragEventHandler handler)
        {
            if (handler != null)
            {
                DragDropBehaviour.GetOrCreateBehaviour(element).DragEnter += handler;
            }
        }

        /// <summary>
        /// 注册拖出事件
        /// </summary>
        /// <param name="element"></param>
        /// <param name="handler"></param>
        public static void AddDragLeaveHandler(DependencyObject element, DragEventHandler handler)
        {
            if (handler != null)
            {
                DragDropBehaviour.GetOrCreateBehaviour(element).DragLeave += handler;
            }
        }

        /// <summary>
        /// 注册拖拽释放事件
        /// </summary>
        /// <param name="element"></param>
        /// <param name="handler"></param>
        public static void AddDropHandler(DependencyObject element, DragEventHandler handler)
        {
            if (handler != null)
            {
                DragDropBehaviour.GetOrCreateBehaviour(element).Drop += handler;
            }
        }

        /// <summary>
        /// 移除拖过事件
        /// </summary>
        /// <param name="element"></param>
        /// <param name="handler"></param>
        public static void RemoveDragOverHandler(DependencyObject element, DragEventHandler handler)
        {
            if (handler != null)
            {
                var adapter = DragDropBehaviour.GetBehaviour(element);
                if (adapter != null)
                {
                    adapter.DragOver -= handler;
                }
            }
        }

        /// <summary>
        /// 移除拖入事件
        /// </summary>
        /// <param name="element"></param>
        /// <param name="handler"></param>
        public static void RemoveDragEnterHandler(DependencyObject element, DragEventHandler handler)
        {
            if (handler != null)
            {
                var adapter = DragDropBehaviour.GetBehaviour(element);
                if (adapter != null)
                {
                    adapter.DragEnter -= handler;
                }
            }
        }

        /// <summary>
        /// 移除拖出事件
        /// </summary>
        /// <param name="element"></param>
        /// <param name="handler"></param>
        public static void RemoveDragLeaveHandler(DependencyObject element, DragEventHandler handler)
        {
            if (handler != null)
            {
                var adapter = DragDropBehaviour.GetBehaviour(element);
                if (adapter != null)
                {
                    adapter.DragLeave -= handler;
                }
            }
        }

        /// <summary>
        /// 移除拖拽释放事件
        /// </summary>
        /// <param name="element"></param>
        /// <param name="handler"></param>
        public static void RemoveDropHandler(DependencyObject element, DragEventHandler handler)
        {
            if (handler != null)
            {
                var adapter = DragDropBehaviour.GetBehaviour(element);
                if (adapter != null)
                {
                    adapter.Drop -= handler;
                }
            }
        }

        #endregion

        #region 属性设置

        ///<summary>
        /// 设置AllowDrop
        ///</summary>
        ///<param name="dependencyObject"></param>
        ///<param name="value"></param>
        public static void SetAllowDrop(DependencyObject dependencyObject, AllowDrop value)
        {
            dependencyObject.SetValue(AllowDropProperty, AllowDropBoxes.Box(value));
        }

        /// <summary>
        /// 获取AllowDrop
        /// </summary>
        /// <param name="dependencyObject"></param>
        /// <returns></returns>
        public static AllowDrop GetAllowDrop(DependencyObject dependencyObject)
        {
            return (AllowDrop) dependencyObject.GetValue(AllowDropProperty);
        }

        #endregion

        #region Visual States

        

        #endregion

        /// <summary>
        ///   Gets a value indicating whether a drag operation is currently in progress.
        /// </summary>
        /// <value>
        ///   <c>true</c> if a drag operation is in progress; otherwise, <c>false</c>.
        /// </value>
        public static bool IsDragging
        {
            get { return m_current != null; }
        }

        private UIElement GetDropTargetAt(Point intersectionPoint)
        {
            var elements = VisualTreeHelper.FindElementsInHostCoordinates(intersectionPoint, m_rootVisual);

            //	exclude drop targets that form part of the cursor
            if (m_dragCursor.DragVisualRoots != null)
            {
                foreach (var dragElement in m_dragCursor.DragVisualRoots)
                {
                    elements = elements.Except(VisualTreeHelper.FindElementsInHostCoordinates(intersectionPoint, dragElement));
                }
            }

            //	retrieve top most target
            var element = elements.FirstOrDefault();
            if (element != null)
            {
                if (AllowsDrop(element))
                {
                    return element;
                }
            }
            return null;
        }

        private static bool AllowsDrop(DependencyObject element)
        {
            while (element != null)
            {
                var result = GetAllowDrop(element);
                if (result != AllowDrop.Inherited)
                {
                    return result == AllowDrop.True;
                }
                element = VisualTreeHelper.GetParent(element);
            }
            return false;
        }

        private void UpdateCurrentDropTarget()
        {
            var value = GetDropTargetAt(m_currentMouseArgs.GetPosition(null));
            if (m_currentDropTarget != value)
            {
                if (m_currentDropTarget != null)
                {
                    OnDragLeave();
                }
                m_currentDropTarget = value;
                if (m_currentDropTarget != null)
                {
                    OnDragEnter();
                }
            }
            if (m_currentDropTarget != null)
            {
                OnDragOver();
            }
            else
            {
                m_dragCursor.Effects = DragDropEffects.None;
            }
        }

        private void BubbleDragEvent(DragEventArgs args, Action<IDropTarget, DragEventArgs> handler)
        {
            var source = m_currentDropTarget;
            while (source != null && GetAllowDrop(source) != AllowDrop.False)
            {
                var dropTarget = source.GetDropTarget();
                if (dropTarget != null)
                {
                    handler(dropTarget, args);
                    if (args.Handled)
                    {
                        return;
                    }
                }
                source = VisualTreeHelper.GetParent(source) as UIElement;
            }
        }

        private DragEventArgs BubbleDragEvent(Action<IDropTarget, DragEventArgs> handler)
        {
            //	create event args
            var args = new DragEventArgs(m_currentMouseArgs, m_currentDropTarget,
                                         m_data, m_allowedEffects);

            //	bubble event
            BubbleDragEvent(args, handler);

            //	return
            return args;
        }

        private void UpdateEffects(DragDropEffects effects)
        {
            m_dragCursor.Effects = effects & m_allowedEffects;
        }

        private void OnDragEnter()
        {
            var args = BubbleDragEvent((x, y) => x.DragEnter(y));
            UpdateEffects(args.Effects);
        }

        private void OnDragLeave()
        {
            BubbleDragEvent((x, y) => x.DragLeave(y));
            UpdateEffects(DragDropEffects.None);
        }

        private void OnDragOver()
        {
            var args = BubbleDragEvent((x, y) => x.DragOver(y));
            UpdateEffects(args.Effects);
        }

        private void OnDrop()
        {
            BubbleDragEvent((x, y) => x.Drop(y));
        }

        private bool UpdateRootVisual()
        {
            m_rootVisual = Application.Current != null ? Application.Current.RootVisual : null;
            if (m_rootVisual == null)
            {
                Debug.WriteLine("Cannot perform drag drop operations without a root visual.");
                return false;
            }
            return true;
        }

        private bool BeginDrag(MouseEventArgs startArgs) // todo: should be IMouseEventArgs?
        {
            //	must have a root visual for calls to VisualTreeHelper.FindElementsInHostCoordinates
            if (!UpdateRootVisual())
            {
                return false;
            }

            //	start dragging
            return m_dragOperation.Start(startArgs);
        }

        private void Dragging(IMouseEventArgs e)
        {
            m_currentMouseArgs = e;
            UpdateCurrentDropTarget();
            m_dragCursor.Drag(e);
        }

        private void EndDrag(bool drop)
        {
            try
            {
                //	notify the drag cursor
                m_dragCursor.EndDrag();

                //	drop or cancel?
                if (drop)
                {
                    if (m_currentDropTarget != null)
                    {
                        OnDrop();
                    }
                }
                else if (m_currentDropTarget != null)
                {
                    OnDragLeave();
                }
            }
            finally
            {
                m_current = null;
                m_rootVisual = null;
            }
        }

        private static void VerifyAccess()
        {
            if (!ApplicationDispatcher.Current.CheckAccess())
            {
                throw new InvalidOperationException("Wrong thread."); // todo: externalise
            }
        }

        /// <summary>
        ///   Updates the state of the current drag drop operation.
        /// </summary>
        public static void Update()
        {
            VerifyAccess();
            if (m_current != null)
            {
                m_current.UpdateCurrentDropTarget();
            }
        }

        public static bool DoDragDrop(MouseEventArgs fromArgs, DependencyObject dragSource, object data, DragCursor dragCursor, DragDropEffects allowedEffects)
        {
            #region Validate Arguments

            Guard.ArgumentNull("fromArgs", fromArgs);

            #endregion

            //	verify call has been made on UI thread.
            VerifyAccess();

            //	cancel current (if any)
            Cancel();

            //	begin!!
            var dragDrop = new DragDropHelper(dragSource, data, allowedEffects, dragCursor);
            if (dragDrop.BeginDrag(fromArgs))
            {
                m_current = dragDrop;
                return true;
            }
            return false;
        }

        /// <summary>
        ///   Cancels the current drag operation
        /// </summary>
        public static void Cancel()
        {
            VerifyAccess();
            if (m_current != null)
            {
                m_current.m_dragOperation.Cancel();
            }
        }

        #region DragOperation Event Handlers

        private void OnDragStarted(object sender, DragOperationStartedEventArgs e)
        {
            //	notify drag cursor
            m_dragCursor.BeginDrag(e);
            Dragging(e);
        }

        private void OnDragCompleted(object sender, DragOperationCompletedEventArgs e)
        {
            if (!e.Canceled)
            {
                Dragging(e);
            }
            EndDrag(!e.Canceled);
        }

        private void OnDragDelta(object sender, DragOperationDeltaEventArgs e)
        {
            Dragging(e);
        }

        #endregion
    }
}