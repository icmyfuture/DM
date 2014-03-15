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
    /// This data consumer looks for drag data coming from
    /// a drag source container of type TContainer and
    /// a drag source data object of type TObject.
    /// It creates a new button using the contents of the
    /// old button and adds the new button to the
    /// drop target's container.
    /// </summary>
    /// <typeparam name="TContainer">Drag data source container type</typeparam>
    /// <typeparam name="TObject">Drag data source object type</typeparam>
    public class CanvasButtonToToolbarButtonConsumer<TContainer, TObject> : DataConsumerBase<TObject>
        where TContainer : Canvas
        where TObject : Button
    {
        public CanvasButtonToToolbarButtonConsumer(string[] dataFormats)
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
                    //Ddf.DragDropDataConsumerActions.DragLeave |
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
        /// 
        /// Note that a new button needs to be created for the toolbar.
        /// </summary>
        /// <param name="bDrop">True to perform an actual drop, otherwise just return e.Effects</param>
        /// <param name="sender">DragDrop event <code>sender</code></param>
        /// <param name="e">DragDrop event arguments</param>
        protected override void DragOverOrDrop(bool bDrop, object sender, DragEventArgs e)
        {
            PanelProvider<TContainer, TObject> provider = GetData(e) as PanelProvider<TContainer, TObject>;
            if (provider != null)
            {
                TContainer dragSourceContainer = provider.SourceContainer as TContainer;
                TObject dragSourceObject = provider.SourceObject as TObject;
                Debug.Assert(dragSourceObject != null);
                Debug.Assert(dragSourceContainer != null);

                ItemsControl dropContainer = sender as ItemsControl;
                TObject dropTarget = e.Source as TObject;
                if (dropTarget == null)
                    dropTarget = Utilities.FindParentControlExcludingMe<TObject>(e.Source as DependencyObject);

                if (dropContainer != null)
                {
                    if (bDrop)
                    {
                        provider.Unparent();
                        Button button;
                        Button oldButton = dragSourceObject;
                        button = new Button();
                        button.Content = Utilities.CloneElement(oldButton.Content);
                        button.ToolTip = oldButton.ToolTip;
                        if (dropTarget == null)
                            dropContainer.Items.Add(button);
                        else
                            dropContainer.Items.Insert(dropContainer.Items.IndexOf(dropTarget), button);
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