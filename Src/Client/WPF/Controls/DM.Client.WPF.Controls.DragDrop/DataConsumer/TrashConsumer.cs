using System.Windows;
using DM.Client.WPF.Controls.DragDrop.DragDropFramework.Base;
using DM.Client.WPF.Controls.DragDrop.DragDropFramework.Enum;
using DM.Client.WPF.Controls.DragDrop.DragDropFramework.Interface;


namespace DM.Client.WPF.Controls.DragDrop.DataConsumer
{
    /// <summary>
    /// This data consumer looks for all data formats specified in the constructor.
    /// When dropped, erase (Unparent) the source object.
    /// </summary>
    public class TrashConsumer : DataConsumerBase<IDataProvider>
    {
        public TrashConsumer(string[] dataFormats)
            : base(dataFormats)
        {
        }

        #region IDataConsumer Members

        public override DataConsumerActions DataConsumerActions
        {
            get
            {
                return
                    //DragDropDataConsumerActions.DragEnter |
                    DataConsumerActions.DragOver |
                    DataConsumerActions.Drop |
                    //DragDropDataConsumerActions.DragLeave |
                    DataConsumerActions.None;
            }
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
        /// Finally erase (Unparent) the source object when <code>bDrop</code> is true.
        /// </summary>
        /// <param name="bDrop">True to perform an actual drop, otherwise just return e.Effects</param>
        /// <param name="sender">DragDrop event <code>sender</code></param>
        /// <param name="e">DragDrop event arguments</param>
        protected override void DragOverOrDrop(bool bDrop, object sender, DragEventArgs e)
        {
            IDataProvider dataProvider = GetData(e) as IDataProvider;
            if (dataProvider != null)
            {
                if (bDrop)
                {
                    dataProvider.Unparent();
                }
                e.Effects = DragDropEffects.Move;
                e.Handled = true;
            }
        }
    }
}