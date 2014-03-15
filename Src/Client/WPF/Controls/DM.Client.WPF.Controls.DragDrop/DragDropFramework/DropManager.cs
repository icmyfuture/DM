using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using DM.Client.WPF.Controls.DragDrop.DragDropFramework.Enum;
using DM.Client.WPF.Controls.DragDrop.DragDropFramework.Interface;

namespace DM.Client.WPF.Controls.DragDrop.DragDropFramework
{
    /// <summary>
    /// Manage drop events for IDataConsumers
    /// </summary>
    public class DropManager
    {
        private readonly IDataConsumer[] _dragDropConsumers;
        private readonly FrameworkElement _dropTarget;

        /// <summary>
        /// Manage data that is dragged over and dropped on the <code>dropTarget</code>.
        /// Supported data is defined as one or more classes that implement IDataConsumer.
        /// </summary>
        /// <param name="dropTarget">FrameworkElement monitored for drag events</param>
        /// <param name="dragDropConsumers">Array of supported data objects</param>
        public DropManager(FrameworkElement dropTarget, params IDataConsumer[] dragDropConsumers)
        {
            _dropTarget = dropTarget;
            Debug.Assert(dropTarget != null);

            _dragDropConsumers = dragDropConsumers;
            Debug.Assert(dragDropConsumers != null);

            bool hookDragEnter = false;
            bool hookDragOver = false;
            bool hookDrop = false;
            bool hookDragLeave = false;

            // Determine which events to hook
            foreach (IDataConsumer dragDropConsumer in _dragDropConsumers)
            {
                if ((dragDropConsumer.DataConsumerActions & DataConsumerActions.DragEnter) != 0)
                    hookDragEnter = true;
                if ((dragDropConsumer.DataConsumerActions & DataConsumerActions.DragOver) != 0)
                    hookDragOver = true;
                if ((dragDropConsumer.DataConsumerActions & DataConsumerActions.Drop) != 0)
                    hookDrop = true;
                if ((dragDropConsumer.DataConsumerActions & DataConsumerActions.DragLeave) != 0)
                    hookDragLeave = true;
            }

            if (hookDragEnter || hookDragOver || hookDrop || hookDragLeave)
                _dropTarget.AllowDrop = true;

            // Hook only the events needed
            if (hookDragEnter)
            {
                if (_dropTarget is TextBox)
                    _dropTarget.PreviewDragEnter += DropTarget_DragEnter;
                else
                    _dropTarget.DragEnter += DropTarget_DragEnter;
            }

            if (hookDragOver)
            {
                if (_dropTarget is TextBox)
                    _dropTarget.PreviewDragOver += DropTarget_DragOver;
                else
                    _dropTarget.DragOver += DropTarget_DragOver;
            }
            if (hookDrop)
            {
                if (_dropTarget is TextBox)
                    _dropTarget.PreviewDrop += DropTarget_Drop;
                else
                    _dropTarget.Drop += DropTarget_Drop;

            }
            if (hookDragLeave)
            {
                if (_dropTarget is TextBox)
                    _dropTarget.PreviewDragLeave += DropTarget_DragLeave;
                else
                    _dropTarget.DragLeave += DropTarget_DragLeave;
            }
        }

        /// <summary>
        /// Initial call, after DoDragDrop is called, has Effects and AllowedEffects set to
        /// allowedEffects as passed to DoDragDrop.  Subsequent Effects and AllowedEffects
        /// are set to the Effects returned by DragLeave.  Note that DragLeave can return
        /// effects that are not defined in allowedEffects (as passed to DoDragDrop).
        /// Source and Original source are set to dragSource as passed to DoDragDrop.
        /// </summary>
        private void DropTarget_DragEnter(object sender, DragEventArgs e)
        {
            foreach (IDataConsumer dragDropConsumer in _dragDropConsumers)
            {
                if ((dragDropConsumer.DataConsumerActions & DataConsumerActions.DragEnter) != 0)
                {
                    dragDropConsumer.DropTarget_DragEnter(sender, e);
                    if (e.Handled)
                        break;
                }
            }

            if (!e.Handled)
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
            }
        }

        /// <summary>
        /// Occurs when mouse is over the area occupied
        /// by the dropTarget (specified in the constructor).
        /// You must likely will provide your own method; make sure
        /// to define DragOver in DataConsumerActions.
        /// </summary>
        private void DropTarget_DragOver(object sender, DragEventArgs e)
        {
            foreach (IDataConsumer dragDropConsumer in _dragDropConsumers)
            {
                if ((dragDropConsumer.DataConsumerActions & DataConsumerActions.DragOver) != 0)
                {
                    dragDropConsumer.DropTarget_DragOver(sender, e);
                    if (e.Handled)
                        break;
                }
            }

            if (!e.Handled)
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
            }
        }

        /// <summary>
        /// Occurs when the left mouse button is released in the area
        /// occupied by the dropTarget (specified in the constructor).
        /// You must likely will provide your own method; make sure
        /// to define Drop in DataConsumerActions.
        /// 
        /// See DropTarget_DragEnter in DropManager for additional comments.
        /// </summary>
        private void DropTarget_Drop(object sender, DragEventArgs e)
        {
            foreach (IDataConsumer dragDropConsumer in _dragDropConsumers)
            {
                if ((dragDropConsumer.DataConsumerActions & DataConsumerActions.Drop) != 0)
                {
                    dragDropConsumer.DropTarget_Drop(sender, e);
                    if (e.Handled)
                        break;
                }
            }

            if (!e.Handled)
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
            }
        }

        /// <summary>
        /// Retured effects are passed to *_DragEnter in both Effects and AllowedEffects;
        /// even effects not included in DoDragDrop's allowedEffects can be used.
        /// </summary>
        private void DropTarget_DragLeave(object sender, DragEventArgs e)
        {
            foreach (IDataConsumer dragDropConsumer in _dragDropConsumers)
            {
                if ((dragDropConsumer.DataConsumerActions & DataConsumerActions.DragLeave) != 0)
                {
                    dragDropConsumer.DropTarget_DragLeave(sender, e);
                    if (e.Handled)
                        break;
                }
            }

            if (!e.Handled)
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
            }
        }
    }
}