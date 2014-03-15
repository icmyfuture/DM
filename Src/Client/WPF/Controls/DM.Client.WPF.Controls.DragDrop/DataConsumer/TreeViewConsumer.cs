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
    /// The TreeViewItem is added as either a sibling or
    /// a child, depending on the state of the Shift key.
    /// </summary>
    /// <typeparam name="TContainer">Drag source and drop destination container type</typeparam>
    /// <typeparam name="TObject">Drag source and drop destination object type</typeparam>
    public class TreeViewConsumer<TContainer, TObject> : DataConsumerBase<TObject>
        where TContainer : ItemsControl
        where TObject : ItemsControl
    {
        public TreeViewConsumer(string[] dataFormats)
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
        /// Add the item as the drop target's child when Shift is not pressed,
        /// or insert the item before the drop target when Shift is pressed.
        /// When there is no drop target (dropped on empty space),
        /// add to the end of the items.
        /// 
        /// Note that the source object cannot be an ancestor of the drop target.
        /// </summary>
        /// <param name="bDrop">True to perform an actual drop, otherwise just return e.Effects</param>
        /// <param name="sender">DragDrop event <code>sender</code></param>
        /// <param name="e">DragDrop event arguments</param>
        protected override void DragOverOrDrop(bool bDrop, object sender, DragEventArgs e)
        {
            TreeViewProvider<TContainer, TObject> provider = GetData(e) as TreeViewProvider<TContainer, TObject>;
            if (provider != null)
            {
                TContainer dragSourceContainer = provider.SourceContainer as TContainer;
                TreeViewItem dragSourceObject = provider.SourceObject as TreeViewItem;
                Debug.Assert(dragSourceContainer != null);
                Debug.Assert(dragSourceObject != null);

                TContainer dropContainer = Utilities.FindParentControlIncludingMe<TContainer>(sender as DependencyObject);
                Debug.Assert(dropContainer != null);
                TObject dropTarget = e.Source as TObject;

                if (dropTarget == null)
                {
                    if (bDrop)
                    {
                        provider.Unparent();
                        dropContainer.Items.Add(dragSourceObject);
                    }
                    e.Effects = DragDropEffects.Move;
                    e.Handled = true;
#if PRINT2OUTPUT
                    Debug.WriteLine("  Move0");
#endif
                }
                else
                {
                    bool IsAncestor = dragSourceObject.IsAncestorOf(dropTarget);
                    if ((provider.KeyStates & DragDropKeyStates.ShiftKey) != 0)
                    {
                        ItemsControl shiftDropTarget = Utilities.FindParentControlExcludingMe<ItemsControl>(dropTarget);
                        Debug.Assert(shiftDropTarget != null);
                        if (!IsAncestor)
                        {
                            if (bDrop)
                            {
                                provider.Unparent();
                                Debug.Assert(shiftDropTarget != null);
                                shiftDropTarget.Items.Insert(shiftDropTarget.Items.IndexOf(dropTarget), dragSourceObject);
                            }
                            e.Effects = DragDropEffects.Link;
                            e.Handled = true;
#if PRINT2OUTPUT
                            Debug.WriteLine("  Link1");
#endif
                        }
                        else
                        {
                            e.Effects = DragDropEffects.None;
                            e.Handled = true;
#if PRINT2OUTPUT
                            Debug.WriteLine("  None1");
#endif
                        }
                    }
                    else
                    {
                        if (!IsAncestor && (dragSourceObject != dropTarget))
                        {
                            if (bDrop)
                            {
                                provider.Unparent();
                                dropTarget.Items.Add(dragSourceObject);
                            }
                            e.Effects = DragDropEffects.Move;
                            e.Handled = true;
#if PRINT2OUTPUT
                            Debug.WriteLine("  Move2");
#endif
                        }
                        else
                        {
                            e.Effects = DragDropEffects.None;
                            e.Handled = true;
#if PRINT2OUTPUT
                            Debug.WriteLine("  None2");
#endif
                        }
                    }
                }
                if (bDrop && e.Handled && (e.Effects != DragDropEffects.None))
                {
                    dragSourceObject.IsSelected = true;
                    dragSourceObject.BringIntoView();
                }
            }
        }
    }
}