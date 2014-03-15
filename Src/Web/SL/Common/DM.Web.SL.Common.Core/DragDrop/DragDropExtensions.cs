using System.Windows;

namespace DM.Web.SL.Common.Core.DragDrop
{
    #region Imports

    

    #endregion

    public static class DragDropExtensions
    {
        public static IDropTarget GetDropTarget(this UIElement element)
        {
            return (element as IDropTarget) ?? DragDropBehaviour.GetBehaviour(element);
        }
    }
}