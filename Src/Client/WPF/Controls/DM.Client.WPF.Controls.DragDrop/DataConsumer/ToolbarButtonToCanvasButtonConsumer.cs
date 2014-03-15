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
    /// When dropped, the button is moved from the ToolBar
    /// to the Canvas.
    /// </summary>
    /// <typeparam name="TContainer">Drag data source container type</typeparam>
    /// <typeparam name="TObject">Drag data source object type</typeparam>
    public class ToolbarButtonToCanvasButtonConsumer<TContainer, TObject> : DataConsumerBase<TObject>
        where TContainer : ItemsControl
        where TObject : Button
    {
        public ToolbarButtonToCanvasButtonConsumer(string[] dataFormats)
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
        /// Second determine whether or not a Move can be done.
        /// And finally handle the actual drop when <code>bDrop</code> is true.
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
                if (dragSourceObject == null)
                    dragSourceObject = Utilities.FindParentControlIncludingMe<TObject>(provider.SourceObject as DependencyObject);
                Debug.Assert(dragSourceObject != null);
                Debug.Assert(dragSourceContainer != null);

                Panel dropContainer = sender as Panel;

                if (dropContainer != null)
                {
                    if (bDrop)
                    {
                        provider.Unparent();
                        Point containerPoint = e.GetPosition(dropContainer);
                        Point objectPoint = provider.StartPosition;

                        Button oldButton = dragSourceObject;
                        Button newButton = new Button();
                        newButton.Content = Utilities.CloneElement(oldButton.Content);
                        newButton.ToolTip = oldButton.ToolTip;
                        dropContainer.Children.Add(newButton);
                        Canvas.SetLeft(newButton, containerPoint.X - objectPoint.X);
                        Canvas.SetTop(newButton, containerPoint.Y - objectPoint.Y);
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