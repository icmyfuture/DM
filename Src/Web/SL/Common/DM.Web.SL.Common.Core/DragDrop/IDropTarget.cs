namespace DM.Web.SL.Common.Core.DragDrop
{
    /// <summary>
    ///   Represents an element that can act as a target of drag-and-drop operations.
    /// </summary>
    public interface IDropTarget
    {
        /// <summary>
        ///   Gets the underlying object the <see cref = "IDropTarget" /> represents.
        /// </summary>
        object Target { get; }

        /// <summary>
        ///   Called when an object is dragged out of the targets's bounds.
        /// </summary>
        /// <param name = "e">The <see cref = "DragEventArgs" /> instance containing the event data.</param>
        void DragLeave(DragEventArgs e);

        /// <summary>
        ///   Called when an object is dragged into the targets's bounds.
        /// </summary>
        /// <param name = "e">The <see cref = "DragEventArgs" /> instance containing the event data.</param>
        void DragEnter(DragEventArgs e);

        /// <summary>
        ///   Called when an object is dragged over the targets's bounds.
        /// </summary>
        /// <param name = "e">The <see cref = "DragEventArgs" /> instance containing the event data.</param>
        void DragOver(DragEventArgs e);

        /// <summary>
        ///   Called when a drag-and-drop operation is completed.
        /// </summary>
        /// <param name = "e">The <see cref = "DragEventArgs" /> instance containing the event data.</param>
        void Drop(DragEventArgs e);
    }
}