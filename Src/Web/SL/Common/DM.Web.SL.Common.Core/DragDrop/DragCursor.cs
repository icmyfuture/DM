using System.Collections.Generic;
using System.Windows;
using DM.Web.SL.Common.Core.DragDrop.Primitives;

namespace DM.Web.SL.Common.Core.DragDrop
{
    #region Imports

    

    #endregion

    /// <summary>
    ///   Base class for cursors used within the drag drop system.
    /// </summary>
    public abstract class DragCursor : DependencyObject
    {
        #region Fields

        public static readonly DependencyProperty EffectsProperty = DependencyProperty.Register(
            "Effects", typeof (DragDropEffects), typeof (DragCursor), new PropertyMetadata(EffectsChanged));

        public static readonly DependencyProperty DataContextProperty = DependencyProperty.Register(
            "DataContext", typeof (object), typeof (DragCursor), new PropertyMetadata(DataContextChanged));

        #endregion

        /// <summary>
        ///   Gets or sets the target drag drop effects.
        /// </summary>
        public DragDropEffects Effects
        {
            get { return (DragDropEffects) GetValue(EffectsProperty); }
            set { SetValue(EffectsProperty, value); }
        }

        /// <summary>
        ///   Gets or sets the data context for the cursor.
        /// </summary>
        public object DataContext
        {
            get { return GetValue(DataContextProperty); }
            set { SetValue(DataContextProperty, value); }
        }

        /// <summary>
        ///   Gets the drag cursors root visual elements.
        /// </summary>
        /// <remarks>
        ///   When the drag drop system is determining the current drop target it must 
        ///   ignore any visual elements that form part of the drag cursor.
        /// </remarks>
        public virtual IEnumerable<UIElement> DragVisualRoots
        {
            get { yield break; }
        }

        private static void DataContextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DragCursor) d).OnDataContextChanged();
        }

        private static void EffectsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DragCursor) d).OnEffectsChanged();
        }

        public void BeginDrag(IMouseEventArgs args)
        {
            OnBeginDrag(args);
        }

        public void Drag(IMouseEventArgs args)
        {
            OnDrag(args);
        }

        public void EndDrag()
        {
            OnEndDrag();
        }

        /// <summary>
        ///   Invoked when the <see cref = "Effects" /> have beeen changed.
        /// </summary>
        protected virtual void OnEffectsChanged()
        {}

        /// <summary>
        ///   Invoked when the <see cref = "DataContext" /> has been changed.
        /// </summary>
        protected virtual void OnDataContextChanged()
        {}

        /// <summary>
        ///   Invoked when a drag drop operation begins.
        /// </summary>
        /// <param name = "args"></param>
        protected virtual void OnBeginDrag(IMouseEventArgs args)
        {}

        /// <summary>
        ///   Invoked when a drag drop operation is in progress and the pointer has moved or the content beneath the cursor has changed.
        /// </summary>
        /// <param name = "args"></param>
        protected virtual void OnDrag(IMouseEventArgs args)
        {}

        /// <summary>
        ///   Invoekd when a drop drop operation ends.
        /// </summary>
        protected virtual void OnEndDrag()
        {}
    }
}