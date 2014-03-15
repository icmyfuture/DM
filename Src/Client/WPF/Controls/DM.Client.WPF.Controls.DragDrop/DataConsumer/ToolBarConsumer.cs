#define PRINT2BUFFER
#define PRINT2OUTPUT

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
    /// This data consumer looks for Buttons coming from a ToolBar.
    /// When dropped, it either inserts the button (if drop target
    /// is a button) or moves the button to the end of the ToolBar.
    /// </summary>
    /// <typeparam name="TContainer">Drag source and drop destination container type</typeparam>
    /// <typeparam name="TObject">Drag source and drop destination object type</typeparam>
    public class ToolBarConsumer<TContainer, TObject> : DataConsumerBase<TObject>
        where TContainer : ItemsControl
        where TObject : UIElement
    {
        public ToolBarConsumer(string[] dataFormats)
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
        /// Second determine what operation to do (move, link).
        /// And finally handle the actual drop when <code>bDrop</code> is true.
        /// Insert the button if the target is another button, otherwise
        /// just add it to the end of the list.
        /// </summary>
        /// <param name="bDrop">True to perform an actual drop, otherwise just return e.Effects</param>
        /// <param name="sender">DragDrop event <code>sender</code></param>
        /// <param name="e">DragDrop event arguments</param>
        protected override void DragOverOrDrop(bool bDrop, object sender, DragEventArgs e)
        {
            ToolBarProvider<TContainer, TObject> provider = GetData(e) as ToolBarProvider<TContainer, TObject>;
            if (provider != null)
            {
                TContainer dragSourceContainer = provider.SourceContainer as TContainer;
                TObject dragSourceObject = provider.SourceObject as TObject;
                Debug.Assert(dragSourceObject != null);
                Debug.Assert(dragSourceContainer != null);

                TContainer dropContainer = sender as TContainer;
                TObject dropTarget = e.Source as TObject;
                if (dropTarget == null)
                    dropTarget = Utilities.FindParentControlExcludingMe<TObject>(e.Source as DependencyObject);

                if (dropContainer != null)
                {
                    if (bDrop)
                    {
                        provider.Unparent();
                        if (dropTarget == null)
                            dropContainer.Items.Add(dragSourceObject);
                        else
                            dropContainer.Items.Insert(dropContainer.Items.IndexOf(dropTarget), dragSourceObject);
                    }
                    e.Effects = (dropTarget == null) ? DragDropEffects.Move : DragDropEffects.Link;
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