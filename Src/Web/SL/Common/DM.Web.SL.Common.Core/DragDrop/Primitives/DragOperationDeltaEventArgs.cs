using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace DM.Web.SL.Common.Core.DragDrop.Primitives
{
    #region Imports

    

    #endregion

    public class DragOperationDeltaEventArgs : DragDeltaEventArgs, IMouseEventArgs
    {
        private readonly MouseEventArgs _innerEvent;

        public DragOperationDeltaEventArgs(MouseEventArgs innerEvent, double horizontalChange, double verticalChange) : base(horizontalChange, verticalChange)
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