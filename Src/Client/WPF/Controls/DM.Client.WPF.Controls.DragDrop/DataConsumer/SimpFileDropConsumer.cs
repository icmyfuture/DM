using System.Windows;
using DM.Client.WPF.Controls.DragDrop.DragDropFramework.Base;
using DM.Client.WPF.Controls.DragDrop.DragDropFramework.EntityAndEvent;
using DM.Client.WPF.Controls.DragDrop.DragDropFramework.Enum;

namespace DM.Client.WPF.Controls.DragDrop.DataConsumer
{
    public class SimpFileDropConsumer : DataConsumerBase<string[]>
    {
        public SimpFileDropConsumer() : base(new[]{"FileDrop","FileNameW",}){}

        public SimpFileDropConsumer(params string[] dataFormats) : base(dataFormats) { }

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

        protected override void DragOverOrDrop(bool bDrop, object sender, DragEventArgs e)
        {
            string[] files = GetData(e) as string[];
            if (files != null)
            {
                if (bDrop)
                {
                    OnDroped(sender, new ConsumerDropedEventArgs<string[]>(files, null));
                }

                e.Effects = DragDropEffects.Move;
                e.Handled = true;
            }
        }
    }
}