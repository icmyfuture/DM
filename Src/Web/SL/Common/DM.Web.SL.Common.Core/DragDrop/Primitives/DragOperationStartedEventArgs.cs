using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace DM.Web.SL.Common.Core.DragDrop.Primitives
{
    #region Imports

    

    #endregion

    public class DragOperationStartedEventArgs : DragStartedEventArgs, IMouseEventArgs
    {
        private readonly MouseEventArgs _innerEvent;

        internal DragOperationStartedEventArgs(MouseEventArgs innerEvent, double horizontalOffset, double verticalOffset) : base(horizontalOffset, verticalOffset)
        {
            _innerEvent = innerEvent;
        }

        #region IMouseEventArgs Members

        public Point GetPosition(UIElement element)
        {
            return _innerEvent.GetPosition(element);
        }

        #endregion
    }
}