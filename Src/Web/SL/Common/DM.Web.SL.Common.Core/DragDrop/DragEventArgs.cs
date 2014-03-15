using System.Windows;
using DM.Web.SL.Common.Core.DragDrop.Primitives;

namespace DM.Web.SL.Common.Core.DragDrop
{
    #region Imports

    

    #endregion

    public class DragEventArgs : RoutedEventArgs, IMouseEventArgs
    {
        #region Fields

        private readonly DragDropEffects _allowedEffects;
        private readonly object _data;
        private readonly IMouseEventArgs _fromArgs;
        private readonly object _source;

        #endregion

        public DragEventArgs(IMouseEventArgs fromArgs, object source, object data, DragDropEffects allowedEffects)
        {
            _fromArgs = fromArgs;
            _source = source;
            _data = data;
            Effects = allowedEffects;
            _allowedEffects = allowedEffects;
        }

        /// <summary>
        ///   Gets the source of the event
        /// </summary>
        public object Source
        {
            get { return _source; }
        }

        /// <summary>
        ///   Gets the allowed <see cref = "DragDropEffects" /> as specified by the originator of the drag event.
        /// </summary>
        public DragDropEffects AllowedEffects
        {
            get { return _allowedEffects; }
        }

        /// <summary>
        ///   Gets or sets the <see cref = "DragDropEffects" /> supported by the current drop target.
        /// </summary>
        public DragDropEffects Effects { get; set; }

        /// <summary>
        ///   Gets the data associated with the drag-and-drop operation.
        /// </summary>
        public object Data
        {
            get { return _data; }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether the event has been handled.
        /// </summary>
        /// <remarks>
        ///   Setting <see cref = "Handled" /> to <c>true</c> will prevent the event 
        ///   from being bubbled up any further within the visual tree.
        /// </remarks>
        public bool Handled { get; set; }

        #region IMouseEventArgs Members

        /// <summary>
        ///   Gets the current position of the mouse relative to a specified <see cref = "UIElement" />.
        /// </summary>
        /// <param name = "element">An <see cref = "UIElement" /> from which to get a relative position.</param>
        /// <returns>A point that is relativie to the element specified in <paramref name = "element" /></returns>
        public Point GetPosition(UIElement element)
        {
            return _fromArgs.GetPosition(element);
        }

        #endregion
    }
}