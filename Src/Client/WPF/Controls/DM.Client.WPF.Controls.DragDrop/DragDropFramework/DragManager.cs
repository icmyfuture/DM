using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using DM.Client.WPF.Controls.DragDrop.DragDropFramework.EntityAndEvent;
using DM.Client.WPF.Controls.DragDrop.DragDropFramework.Enum;
using DM.Client.WPF.Controls.DragDrop.DragDropFramework.Interface;

namespace DM.Client.WPF.Controls.DragDrop.DragDropFramework
{
    /// <summary>
    /// Manage drag events for IDataProviders
    /// </summary>
    public class DragManager
    {
        private readonly IDataProvider[] _dataProviders;
        private readonly UIElement _dragSource;

        private IDataProvider _dataProvider;
        private bool _dragInProgress;
        private Point _startPosition;

        /// <summary>
        /// Manage dragging data object from <code>dragSource</code> FrameworkElement.
        /// Hook various PreviewMouse* events in order to determine when a drag starts.
        /// </summary>
        /// <param name="dragSource">The FrameworkElement which contains objects to be dragged</param>
        /// <param name="dataProviders">Array of objects to be dragged, implementing IDataProvider</param>
        public DragManager(FrameworkElement dragSource, params IDataProvider[] dataProviders)
        {
            _dragSource = dragSource;
            Debug.Assert(dragSource != null, "dragSource cannot be null");
            _dataProviders = dataProviders;

            _dragSource.PreviewMouseLeftButtonDown += DragSource_PreviewMouseLeftButtonDown;
            _dragSource.PreviewMouseMove += DragSource_PreviewMouseMove;
            _dragSource.PreviewMouseLeftButtonUp += DragSource_PreviewMouseLeftButtonUp;
        }

        /// <summary>
        /// Check for a supported SourceContainer/SourceObject.
        /// If found, get ready for a possible drag operation.
        /// </summary>
        private void DragSource_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            foreach (IDataProvider dragDropObject in _dataProviders)
            {
                if (dragDropObject.IsSupportedContainerAndObject(true, sender, e.Source, e.OriginalSource))
                {
                    Debug.Assert(sender.Equals(_dragSource));
                    _dataProvider = dragDropObject;
                    _startPosition = e.GetPosition(sender as IInputElement);

                    object src = Utilities.FindParentControlIncludingMe((DependencyObject)e.OriginalSource, _dataProvider.SourceObject.GetType());
                    _dataProvider.StartPosition = e.GetPosition(src as IInputElement);
                    if (_dataProvider.NeedsCaptureMouse)
                        _dragSource.CaptureMouse();

                    break;
                }
            }
        }

        /// <summary>
        /// If the mouse is moved (dragged) a minimum distance
        /// over a supported SourceContainer/SourceObject,
        /// initiate a drag operation.
        /// </summary>
        private void DragSource_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if ((_dataProvider != null) && !_dragInProgress &&
                _dataProvider.IsSupportedContainerAndObject(false, sender, e.Source, e.OriginalSource))
            {
                Point currentPosition = e.GetPosition(sender as IInputElement);
                if (((Math.Abs(currentPosition.X - _startPosition.X) > SystemParameters.MinimumHorizontalDragDistance) ||
                     (Math.Abs(currentPosition.Y - _startPosition.Y) > SystemParameters.MinimumVerticalDragDistance)))
                {
                    // NOTE:
                    //      While dragging a ListBoxItem, another one can be selected
                    //      This doesn't seem to happen with TreeView or TabControl
                    if (sender is ListBox)
                        _dataProvider.SourceObject = e.Source;

                    _dragInProgress = true;

                    DragDropEffects resultEffects = DoDragDrop_Start(e);

                    if (_dataProvider.NeedsCaptureMouse)
                        _dragSource.ReleaseMouseCapture();

                    DoDragDrop_Done(resultEffects);

                    if (_dataProvider.AddAdorner)
                    {
                        AdornerLayer.GetAdornerLayer((Visual) Application.Current.MainWindow.Content).Remove(_dataProvider.DragAdorner);
                    }

                    Mouse.OverrideCursor = null;

                    _dataProvider = null;
                    _dragInProgress = false;
                }
            }
        }

        /// <summary>
        /// When MouseLeftButtonUp event occurs, abandon
        /// any drag operation that may be in progress
        /// </summary>
        private void DragSource_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_dataProvider != null)
            {
                if (_dataProvider.NeedsCaptureMouse)
                    _dragSource.ReleaseMouseCapture();
                _dataProvider = null;
                _dragInProgress = false;
            }
        }

        /// <summary>
        /// Gather keyboard key state information
        /// and optionally abort a drag operation
        /// </summary>
        private void DragSource_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            if ((_dataProvider.DataProviderActions & DataProviderActions.QueryContinueDrag) != 0)
                _dataProvider.DragSource_QueryContinueDrag(sender, e);
        }

        /// <summary>
        /// Display the appropriate drag cursor based on
        /// DragDropEffects returned within the DropManager
        /// </summary>
        private void DragSource_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            if (_dataProvider.AddAdorner)
            {
                Point point = Utilities.Win32GetCursorPos();
                DefaultAdorner dragAdorner = _dataProvider.DragAdorner;
                dragAdorner.SetMousePosition(dragAdorner.AdornedElement.PointFromScreen(point));
            }

            if ((_dataProvider.DataProviderActions & DataProviderActions.GiveFeedback) != 0)
                _dataProvider.DragSource_GiveFeedback(sender, e);
        }

        /// <summary>
        /// Prepare for and begin a drag operation.
        /// Hook the events needed by the data provider.
        /// </summary>
        private DragDropEffects DoDragDrop_Start(MouseEventArgs e)
        {
            DragDropEffects resultEffects = DragDropEffects.None;

            DataObject data = new DataObject();
            _dataProvider.SetData(ref data);

            bool hookQueryContinueDrag = false;
            bool hookGiveFeedback = false;

            if ((_dataProvider.DataProviderActions & DataProviderActions.QueryContinueDrag) != 0)
                hookQueryContinueDrag = true;

            if ((_dataProvider.DataProviderActions & DataProviderActions.GiveFeedback) != 0)
                hookGiveFeedback = true;

            if (_dataProvider.AddAdorner)
                hookGiveFeedback = true;

            QueryContinueDragEventHandler queryContinueDrag = null;
            GiveFeedbackEventHandler giveFeedback = null;

            if (hookQueryContinueDrag)
            {
                queryContinueDrag = new QueryContinueDragEventHandler(DragSource_QueryContinueDrag);
                _dragSource.QueryContinueDrag += queryContinueDrag;
            }
            if (hookGiveFeedback)
            {
                giveFeedback = new GiveFeedbackEventHandler(DragSource_GiveFeedback);
                _dragSource.GiveFeedback += giveFeedback;
            }

            _dataProvider.DragSource_StartDrag(_dataProvider.SourceObject);

            int dragNo;
            _dataProvider.DragSource_GetDragNo(out dragNo);

            if (_dataProvider.AddAdorner)
            {
                _dataProvider.DragAdorner = new DefaultAdorner(
                    (UIElement)Application.Current.MainWindow.Content,
                    (UIElement)_dataProvider.SourceObject,
                    _dataProvider.StartPosition, dragNo);
                Visual visual = Application.Current.MainWindow.Content as Visual;
                AdornerLayer.GetAdornerLayer(visual).Add(_dataProvider.DragAdorner);
            }

            try
            {
                // NOTE:  Set 'dragSource' to desired value (dragSource or item being dragged)
                //		  'dragSource' is passed to QueryContinueDrag as Source and OriginalSource
                DependencyObject dragSource;
                dragSource = _dragSource;
                //dragSource = this._dataProvider.Item;
                resultEffects = System.Windows.DragDrop.DoDragDrop(dragSource, data, _dataProvider.AllowedEffects);
            }
            catch
            {
                Debug.WriteLine("DragDrop.DoDragDrop threw an exception");
            }

            if (queryContinueDrag != null)
                _dragSource.QueryContinueDrag -= queryContinueDrag;
            if (giveFeedback != null)
                _dragSource.GiveFeedback -= giveFeedback;

            return resultEffects;
        }

        /// <summary>
        /// Called after DragDrop.DoDragDrop() returns.
        /// Typically during a file move, for example, the file is deleted here.
        /// However, when moving a TabItem from one TabControl to another the
        /// source TabItem must be unparented from the source TabControl
        /// before it can be added to the destination TabControl.
        /// So most of the time when moving items between item controls,
        /// this method isn't used.
        /// </summary>
        /// <param name="resultEffects">The drop operation that was performed</param>
        private void DoDragDrop_Done(DragDropEffects resultEffects)
        {
            if ((_dataProvider.DataProviderActions & DataProviderActions.DoDragDrop_Done) != 0)
                _dataProvider.DoDragDrop_Done(resultEffects);
        }
    }
}