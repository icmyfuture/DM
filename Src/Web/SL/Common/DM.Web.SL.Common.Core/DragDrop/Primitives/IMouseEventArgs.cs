using System.Windows;

namespace DM.Web.SL.Common.Core.DragDrop.Primitives
{
    #region Imports

    

    #endregion

    public interface IMouseEventArgs
    {
        Point GetPosition(UIElement element);
    }
}