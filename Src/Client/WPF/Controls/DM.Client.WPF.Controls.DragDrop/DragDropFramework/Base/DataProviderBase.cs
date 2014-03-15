using System;
using System.Diagnostics;
using System.Windows;
using DM.Client.WPF.Controls.DragDrop.DragDropFramework.EntityAndEvent;
using DM.Client.WPF.Controls.DragDrop.DragDropFramework.Enum;
using DM.Client.WPF.Controls.DragDrop.DragDropFramework.Interface;

namespace DM.Client.WPF.Controls.DragDrop.DragDropFramework.Base
{
    /// <summary>
    /// This class provides some default implementations for
    /// IDataProvider that can be used by most derived classes.
    /// This class defines a data object that can be dragged.
    /// </summary>
    /// <typeparam name="TSourceContainer">Type of the source container, e.g. TabControl</typeparam>
    /// <typeparam name="TSourceObject">Type of the source object, e.g. TabItem</typeparam>
    public class DataProviderBase<TSourceContainer, TSourceObject> : IDataProvider
        where TSourceContainer : UIElement
        where TSourceObject : UIElement
    {
        /// <summary>
        /// EscapePressed saved from QueryContinueDrag
        /// </summary>
        private bool? _escapePressed;

        /// <summary>
        /// KeyStates saved from QueryContinueDrag
        /// </summary>
        private DragDropKeyStates? _keyStates;

        /// <summary>
        /// Create a Data Provider for specified SourceContainer/SourceObject
        /// identified by the specified data format string
        /// </summary>
        /// <param name="dataFormatString">Identifies the data object</param>
        public DataProviderBase(string dataFormatString)
        {
            SourceDataFormat = dataFormatString;
        }

        /// <summary>
        /// Name of the dragged data object
        /// </summary>
        public string SourceDataFormat { get; private set; }

        public DragDropKeyStates KeyStates
        {
            get
            {
                if (_keyStates != null)
                    return (DragDropKeyStates) _keyStates;
                throw new NotImplementedException("No KeyState value to return");
            }
        }

        public bool EscapePressed
        {
            get
            {
                if (_escapePressed != null)
                    return (bool) _escapePressed;
                throw new NotImplementedException("No EscapePressed value to return");
            }
        }

        #region IDataProvider Members

        /// <summary>
        /// Called by drag-and-drop framework to initialize the class
        /// </summary>
        public void Init()
        {
            _keyStates = null;
            _escapePressed = null;
        }

        /// <summary>
        /// Return true to add an adorner to the dragged object
        /// </summary>
        public virtual bool AddAdorner
        {
            get { return true; }
        }

        /// <summary>
        /// Return true to capture the mouse while dragging
        /// </summary>
        public virtual bool NeedsCaptureMouse
        {
            get { return false; }
        }

        /// <summary>
        /// Returns the drag operations supported by this data object provider
        /// </summary>
        public virtual DragDropEffects AllowedEffects
        {
            get
            {
                return
                    //DragDropEffects.Copy |
                    //DragDropEffects.Scroll |
                    DragDropEffects.Move | // Move tab from one TabControl to another
                    DragDropEffects.Link | // Move tabs within the same TabControl
                    DragDropEffects.None;
            }
        }

        /// <summary>
        /// Returns the actions used by this data object provider
        /// </summary>
        public virtual DataProviderActions DataProviderActions
        {
            get
            {
                return
                    //DataProviderActions.QueryContinueDrag |
                    DataProviderActions.GiveFeedback |
                    //DragDropDataProviderActions.DoDragDrop_Done |
                    DataProviderActions.None;
            }
        }

        /// <summary>
        /// Returns true when the specified source container, source object
        /// and original source object are supported by this data object provider.
        /// Saves the parameters in SourceContainer, SourceObject and
        /// OriginalSourceObject, respectively, when initFlag is true.
        /// </summary>
        /// <param name="initFlag">When true, initialize the class and source/container values</param>
        /// <param name="dragSourceContainer">Mouse event <code>sender</code></param>
        /// <param name="dragSourceObject">Mouse event args <code>Source</code></param>
        /// <param name="dragOriginalSourceObject">Mouse event args <code>Source</code></param>
        /// <returns>True for a supported container and object; false otherwise</returns>
        public virtual bool IsSupportedContainerAndObject(bool initFlag, object dragSourceContainer, object dragSourceObject,
                                                          object dragOriginalSourceObject)
        {
            var sourceObject = dragSourceObject as TSourceObject;
            if (sourceObject == null)
                sourceObject = Utilities.FindParentControlIncludingMe<TSourceObject, TSourceContainer>(dragOriginalSourceObject as DependencyObject);
            if (sourceObject == null)
                return false;

            var sourceContainer = dragSourceContainer as TSourceContainer;
            if (sourceContainer == null)
                return false;

            if (initFlag)
            {
                Init();
                SourceContainer = sourceContainer;
                SourceObject = sourceObject;
                OriginalSourceObject = dragOriginalSourceObject;
            }

            return true;
        }

        /// <summary>
        /// The adorner (when used)
        /// </summary>
        public DefaultAdorner DragAdorner { get; set; }

        /// <summary>
        /// Point where LeftMouseDown occurred,
        /// relative to the drag source object's origin
        /// </summary>
        public Point StartPosition { get; set; }

        /// <summary>
        /// Drag source container, e.g. TabControl
        /// </summary>
        public object SourceContainer { get; set; }

        /// <summary>
        /// Drag source object, e.g. TabItem
        /// </summary>
        public object SourceObject { get; set; }

        /// <summary>
        /// OriginalSource from MouseButtonEventArgs
        /// </summary>
        public object OriginalSourceObject { get; set; }

        /// <summary>
        /// Sets the data passed to WPF
        /// DragDrop.DoDragDrop()
        /// </summary>
        /// <param name="data"></param>
        public virtual void SetData(ref DataObject data)
        {
            Debug.Assert(data.GetDataPresent(SourceDataFormat) == false, "Shouldn't set data more than once");
            data.SetData(SourceDataFormat, this);
        }

        /// <summary>
        /// Saves EscapePressed and KeyStates when
        /// QueryContinueDrag is defined in DataProviderActions.
        /// Provide your own method if you wish; making sure
        /// to define QueryContinueDrag in DataProviderActions.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void DragSource_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            _escapePressed = e.EscapePressed;
            _keyStates = e.KeyStates;
        }

        /// <summary>
        /// Provide your own method for displaying
        /// the correct cursor during a drag.
        /// Make sure to define GiveFeedback in DataProviderActions.
        /// </summary>
        /// <param name="sender">GiveFeedback event sender</param>
        /// <param name="e">GiveFeedback event arguments</param>
        public virtual void DragSource_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            if (e.Effects == DragDropEffects.Move)
            {
                e.UseDefaultCursors = true;
                e.Handled = true;
            }
            else if (e.Effects == DragDropEffects.Link)
            {
                // ... when Shift key is pressed
                e.UseDefaultCursors = true;
                e.Handled = true;
            }
        }

        /// <summary>
        /// Start Drag.
        /// </summary>
        public void DragSource_StartDrag(object sourceObject)
        {
            OnStartDrag(new DragingDataEventArgs<TSourceObject>((TSourceObject) sourceObject));
        }

        /// <summary>
        /// Get DragNo.
        /// </summary>
        public void DragSource_GetDragNo(out int dragNo)
        {
            var dragNoEventArgs = new DragNoEventArgs();
            OnGetDragNo(dragNoEventArgs);
            dragNo = dragNoEventArgs.Vaule;
        }

        /// <summary>
        /// Called after DragDrop.DoDragDrop() returns.
        /// Typically during a file move, for example, the file is deleted here.
        /// However, when moving a TabItem from one TabControl to another the
        /// source TabItem must be unparented from the source TabControl
        /// before it can be added to the destination TabControl.
        /// So most of the time when moving items between item controls,
        /// this method isn't used.
        /// Provide your own method if you wish; making sure
        /// to define DoDragDrop_Done in DataProviderActions.
        /// </summary>
        /// <param name="resultEffects">The drop operation that was performed</param>
        public virtual void DoDragDrop_Done(DragDropEffects resultEffects)
        {
            OnDragDropDone(new DragingDataEventArgs<TSourceObject>((TSourceObject) SourceObject));
        }

        /// <summary>
        /// Provide your own method to remove the source object from its container.
        /// This method is typically called when the source object is dropped and
        /// must be removed from its container.
        /// </summary>
        public virtual void Unparent()
        {
            throw new NotImplementedException("Unparent not implemented");
        }

        #endregion

        public event EventHandler<DragingDataEventArgs<TSourceObject>> StartDrag;

        public event EventHandler<DragingDataEventArgs<TSourceObject>> DragDropDone;

        public event EventHandler<DragNoEventArgs> GetDragNo;

        public void OnGetDragNo(DragNoEventArgs e)
        {
            EventHandler<DragNoEventArgs> handler = GetDragNo;
            if (handler != null) handler(this, e);
        }

        public void OnDragDropDone(DragingDataEventArgs<TSourceObject> e)
        {
            EventHandler<DragingDataEventArgs<TSourceObject>> handler = DragDropDone;
            if (handler != null) handler(this, e);
        }

        public void OnStartDrag(DragingDataEventArgs<TSourceObject> e)
        {
            EventHandler<DragingDataEventArgs<TSourceObject>> handler = StartDrag;
            if (handler != null) handler(this, e);
        }
    }
}