#define PRINT2BUFFER
#define PRINT2OUTPUT

using System.Windows;
using System.Windows.Controls;
using DM.Client.WPF.Controls.DragDrop.DragDropFramework.Base;
using DM.Client.WPF.Controls.DragDrop.DragDropFramework.Enum;

namespace DM.Client.WPF.Controls.DragDrop.DataConsumer
{
    /// <summary>
    /// This data consumer looks for an object of type string.
    /// When the item is dropped, a TextBlock is created with
    /// its text initialized to the contents of the data string.
    /// The TextBlock's origin is placed on the canvas at the
    /// point where the string was dropped.
    /// </summary>
    public class StringToCanvasTextBlockConsumer : DataConsumerBase<string>
    {
        /// <summary>
        /// Create a string data consumer for a canvas
        /// </summary>
        /// <param name="dataFormats">A data format whose data is of type string</param>
        public StringToCanvasTextBlockConsumer(string[] dataFormats)
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
        /// Finally handle the actual drop when <code>bDrop</code> is true
        /// by creating a new TextBlock and initializing its Text property
        /// to the value of the string.  The TextBlock is placed on the
        /// canvas such that its origin is at the point when the string
        /// was dropped.
        /// </summary>
        /// <param name="bDrop">True to perform an actual drop, otherwise just return e.Effects</param>
        /// <param name="sender">DragDrop event <code>sender</code></param>
        /// <param name="e">DragDrop event arguments</param>
        protected override void DragOverOrDrop(bool bDrop, object sender, DragEventArgs e)
        {
            object dropObject = GetData(e);
            if ((dropObject is string) && (sender is Canvas))
            {
                string data = dropObject as string;
                Canvas dropContainer = sender as Canvas;

                if (dropContainer != null)
                {
                    if (bDrop)
                    {
                        Point containerPoint = e.GetPosition(dropContainer);
                        TextBlock textBlock = new TextBlock();
                        textBlock.Text = dropObject.ToString();
                        dropContainer.Children.Add(textBlock);
                        Canvas.SetLeft(textBlock, containerPoint.X);
                        Canvas.SetTop(textBlock, containerPoint.Y);
                    }
                    e.Effects = DragDropEffects.Copy;
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