using System.Windows;
using DM.Client.WPF.Controls.DragDrop.DragDropFramework.Enum;

namespace DM.Client.WPF.Controls.DragDrop.DragDropFramework.Interface
{
    /// <summary>
    /// A declaration of actions that can be performed on dragged data
    /// </summary>
    public interface  IDataConsumer
    {
        DataConsumerActions DataConsumerActions { get; }


        void DropTarget_DragEnter(object sender, DragEventArgs e);
        void DropTarget_DragOver(object sender, DragEventArgs e);
        void DropTarget_Drop(object sender, DragEventArgs e);
        void DropTarget_DragLeave(object sender, DragEventArgs e);
    }
}