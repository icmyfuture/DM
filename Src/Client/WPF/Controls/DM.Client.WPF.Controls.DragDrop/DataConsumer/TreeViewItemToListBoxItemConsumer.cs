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
    /// This data consumer looks for TreeViewItems.
    /// The item is inserted before the
    /// target ListBoxItem or at the end of the
    /// list if dropped on empty space.
    /// 
    /// Note that only TreeViewItems with no children can be moved.
    /// </summary>
    /// <typeparam name="TSourceContainer">Drag data source container type</typeparam>
    /// <typeparam name="TSourceObject">Drag data source object type</typeparam>
    public class TreeViewItemToListBoxItemConsumer<TSourceContainer, TSourceObject> : DataConsumerBase<TSourceObject>
        where TSourceContainer : ItemsControl
        where TSourceObject : TreeViewItem
    {
        public TreeViewItemToListBoxItemConsumer(string[] dataFormats)
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
        /// Finally handle the actual drop when <code>bDrop</code> is true.
        /// Insert the item before the drop target.  When there is no drop
        /// target (dropped on empty space), add to the end of the items.
        /// 
        /// Note that only TreeViewItems with no children can be moved.
        /// </summary>
        /// <param name="bDrop">True to perform an actual drop, otherwise just return e.Effects</param>
        /// <param name="sender">DragDrop event <code>sender</code></param>
        /// <param name="e">DragDrop event arguments</param>
        protected override void DragOverOrDrop(bool bDrop, object sender, DragEventArgs e)
        {
            TreeViewProvider<TSourceContainer, TSourceObject> provider = GetData(e) as TreeViewProvider<TSourceContainer, TSourceObject>;
            if (provider != null)
            {
                TSourceObject dragSourceObject = provider.SourceObject as TSourceObject;
                TSourceContainer dragSourceContainer = provider.SourceContainer as TSourceContainer;
                Debug.Assert(dragSourceObject != null);
                Debug.Assert(dragSourceContainer != null);

                ListBox dropContainer = Utilities.FindParentControlIncludingMe<ListBox>(e.Source as DependencyObject);
                ListBoxItem dropTarget = Utilities.FindParentControlIncludingMe<ListBoxItem>(e.Source as DependencyObject);

                if (!dragSourceObject.HasItems)
                {
                    // TreeViewItem must be a leaf
                    if (bDrop)
                    {
                        provider.Unparent();

                        ListBoxItem item = new ListBoxItem();
                        item.Content = dragSourceObject.Header;
                        item.ToolTip = dragSourceObject.ToolTip;
                        if (dropTarget == null)
                            dropContainer.Items.Add(item);
                        else
                            dropContainer.Items.Insert(dropContainer.Items.IndexOf(dropTarget), item);

                        item.IsSelected = true;
                        item.BringIntoView();
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