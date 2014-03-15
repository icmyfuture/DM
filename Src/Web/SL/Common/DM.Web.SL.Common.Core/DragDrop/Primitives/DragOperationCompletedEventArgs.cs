using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace DM.Web.SL.Common.Core.DragDrop.Primitives
{
    #region Imports

    

    #endregion

    public class DragOperationCompletedEventArgs : DragCompletedEventArgs, IMouseEventArgs
    {
        private readonly MouseEventArgs _innerEvent;

        public DragOperationCompletedEventArgs(MouseEventArgs innerEvent, double horizontalChange, double verticalChange, bool canceled) : base(horizontalChange, verticalChange, canceled)
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