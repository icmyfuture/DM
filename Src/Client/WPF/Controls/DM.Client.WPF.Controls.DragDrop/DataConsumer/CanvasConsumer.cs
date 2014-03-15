using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using DM.Client.WPF.Controls.DragDrop.DataProvider;
using DM.Client.WPF.Controls.DragDrop.DragDropFramework.Base;
using DM.Client.WPF.Controls.DragDrop.DragDropFramework.Enum;

namespace DM.Client.WPF.Controls.DragDrop.DataConsumer
{
    /// <summary>
    /// This data consumer looks for drag data coming from
    /// a canvas (of type TContainer) and
    /// a drag source data object of type TObject.
    /// When dropped, it moves the data object to the
    /// mouse drop location.
    /// </summary>
    /// <typeparam name="TContainer">Drag source and drop destination container type</typeparam>
    /// <typeparam name="TObject">Drag source and drop destination object type</typeparam>
    public class CanvasConsumer<TContainer, TObject> : DataConsumerBase<TObject>
        where TContainer : Canvas
        where TObject : UIElement
    {
        public CanvasConsumer(string[] dataFormats)
            : base(dataFormats)
        {
        }

        #region IDataConsumer Members

        public override DataConsumerActions DataConsumerActions
        {
            get
            {
                return
                    DataConsumerActions.DragEnter |
                    DataConsumerActions.DragOver |
                    DataConsumerActions.Drop |
                    //DragDropDataConsumerActions.DragLeave |
                    DataConsumerActions.None;
            }
        }

        public override void DropTarget_DragEnter(object sender, DragEventArgs e)
        {
            DragOverOrDrop(false, sender, e);
        }

        public override void DropTarget_DragOver(object sender, DragEventArgs e)
        {
            DragOverOrDrop(false, sender, e);
        }

        public override void DropTarget_Drop(object sender, DragEventArgs e)
        {
            DragOverOrDrop(true, sender, e);
        }

        #endregion

        /// <summary>
        /// First determine whether the drag data is supported.
        /// Second determine what operation to do (copy, move, link).
        /// And finally handle the actual drop when <code>bDrop</code> is true.
        /// </summary>
        /// <param name="bDrop">True to perform an actual drop, otherwise just return e.Effects</param>
        /// <param name="sender">DragDrop event <code>sender</code></param>
        /// <param name="e">DragDrop event arguments</param>
        protected override void DragOverOrDrop(bool bDrop, object sender, DragEventArgs e)
        {
            PanelProvider<TContainer, TObject> provider = GetData(e) as PanelProvider<TContainer, TObject>;
            if (provider != null)
            {
                TObject dragSourceObject = provider.SourceObject as TObject;
                Debug.Assert(dragSourceObject != null);

                TContainer dropContainer = sender as TContainer;

                if (dropContainer != null)
                {
                    if (bDrop)
                    {
                        provider.Unparent();
                        dropContainer.Children.Add(dragSourceObject);

                        Point dropPosition = e.GetPosition(dropContainer);
                        Point objectOrigin = provider.StartPosition;
                        Canvas.SetLeft(dragSourceObject, dropPosition.X - objectOrigin.X);
                        Canvas.SetTop(dragSourceObject, dropPosition.Y - objectOrigin.Y);
                    }
                    e.Effects = DragDropEffects.Move;
                    e.Handled = true;
                }
                else
                {
                    e.Effects = DragDropEffects.None;
                    e.Handled = true;
                }
            }
        }
    }
}