using System.Windows;
using DM.Client.WPF.Controls.DragDrop.DragDropFramework.Interface;

namespace DM.Client.WPF.Controls.DragDrop.DragDropFramework
{
    public static class DragDropLib
    {
        public static void Reg(FrameworkElement dragSource, params IDataProvider[] dragDropObjects)
        {
            new DragManager(dragSource, dragDropObjects);
        }

        public static void Reg(FrameworkElement dropTarget, params IDataConsumer[] dragDropConsumers)
        {
            new DropManager(dropTarget, dragDropConsumers);
        }
    }
}