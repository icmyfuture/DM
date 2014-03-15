using System;

namespace DM.Client.WPF.Controls.DragDrop.DragDropFramework.Enum
{
    /// <summary>
    /// Actions that can be defined by a data consumer
    /// </summary>
    [Flags]
    public enum DataConsumerActions
    {
        DragEnter = 0x01,
        DragOver = 0x02,
        Drop = 0x04,
        DragLeave = 0x08,

        None = 0x00,

        AllowDropMask = DragEnter | DragOver | Drop | DragLeave,
    }
}