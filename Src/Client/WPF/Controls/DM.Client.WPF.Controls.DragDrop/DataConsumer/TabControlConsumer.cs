using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using DM.Client.WPF.Controls.DragDrop.DataProvider;
using DM.Client.WPF.Controls.DragDrop.DragDropFramework;
using DM.Client.WPF.Controls.DragDrop.DragDropFramework.Base;
using DM.Client.WPF.Controls.DragDrop.DragDropFramework.Enum;


namespace DM.Client.WPF.Controls.DragDrop.DataConsumer
{
    /// <summary>
    /// This data consumer looks for TabItems.
    /// When the TabItem is dragged within its original
    /// control, the TabItems are rearranged accordingly.
    /// When dropped, it is inserted as the first TabItem.
    /// </summary>
    /// <typeparam name="TContainer">Drag source and drop destination container type</typeparam>
    /// <typeparam name="TObject">Drag source and drop destination object type</typeparam>
    public class TabControlConsumer<TContainer, TObject> : DataConsumerBase<TObject>
        where TContainer : ItemsControl
        where TObject : TabItem
    {
        public TabControlConsumer(string[] dataFormats)
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
        /// Next check if it's a move within the same TabControl,
        /// and rearrange the TabItems.
        /// Finally handle the actual drop when <code>bDrop</code> is true.
        /// </summary>
        /// <param name="bDrop">True to perform an actual drop, otherwise just return e.Effects</param>
        /// <param name="sender">DragDrop event <code>sender</code></param>
        /// <param name="e">DragDrop event arguments</param>
        protected override void DragOverOrDrop(bool bDrop, object sender, DragEventArgs e)
        {
            TabControlProvider<TContainer, TObject> provider = GetData(e) as TabControlProvider<TContainer, TObject>;
            if (provider != null)
            {
                TContainer dragSourceContainer = provider.SourceContainer as TContainer;
                TObject dragSourceObject = provider.SourceObject as TObject;
                Debug.Assert(dragSourceObject != null);
                Debug.Assert(dragSourceContainer != null);

                TContainer dropContainer = Utilities.FindParentControlIncludingMe<TContainer>(e.Source as DependencyObject);
                TObject dropTarget = e.Source as TObject;

                if ((dragSourceContainer == dropContainer) && (dropTarget != null))
                {
                    // Reorder within same container
                    int srcIndex = dragSourceContainer.Items.IndexOf(dragSourceObject);
                    int dstIndex = dropContainer.Items.IndexOf(dropTarget);
                    if (srcIndex != dstIndex)
                    {
                        // Only move when there's no chance of oscillation
                        bool doMove = true;
                        if (dragSourceObject.ActualWidth < (dropTarget.ActualWidth))
                        {
                            Point point = e.GetPosition(dropTarget);
                            if (srcIndex < dstIndex)
                            {
                                doMove = point.X > ((dropTarget.ActualWidth - dragSourceObject.ActualWidth));
                            }
                            else
                            {
                                doMove = point.X < dragSourceObject.ActualWidth;
                            }
                        }
                        if (doMove)
                        {
                            provider.Unparent();
                            dropContainer.Items.Insert(dstIndex, dragSourceObject);
                            dragSourceObject.IsSelected = true;
                            //Debug.WriteLine("DragOverOrDrop doMove=True srcIndex=" + srcIndex.ToString() + " dstIndex=" + dstIndex.ToString());
                        }
                    }
                    e.Effects = DragDropEffects.Link;
                    e.Handled = true;
#if PRINT2OUTPUT
                    Debug.WriteLine("   Link0");
#endif
                }
                else if (dropContainer != null)
                {
                    // Move to destination container as 1st TabItem
                    if (bDrop)
                    {
                        //srcTabControl.Items.Remove(srcTabItem);
                        provider.Unparent();
                        dropContainer.Items.Insert(0, dragSourceObject);
                        dragSourceObject.IsSelected = true;
                    }
                    e.Effects = DragDropEffects.Move;
                    e.Handled = true;
#if PRINT2OUTPUT
                    Debug.WriteLine("  Move0");
#endif
                }
                else
                {
                    e.Effects = DragDropEffects.None;
                    e.Handled = true;
#if PRINT2OUTPUT
                    Debug.WriteLine("  None0");
#endif
                }
            }
        }
    }
}