using System;

namespace DM.Client.WPF.Controls.DragDrop.DragDropFramework.Enum
{
    [Flags]
    public enum DataProviderActions
    {
        /// <summary>
        /// Call IDataProvider.DragSource_QueryContinueDrag
        /// </summary>
        QueryContinueDrag = 0x01, 
        
        /// <summary>
        /// Call IDataProvider.DragSource_GiveFeedback
        /// </summary>
        GiveFeedback = 0x02, 

        /// <summary>
        /// Call IDataProvider.DoDragDrop_Done
        /// </summary>
        DoDragDrop_Done = 0x04, 

        None = 0x00,
    }
}