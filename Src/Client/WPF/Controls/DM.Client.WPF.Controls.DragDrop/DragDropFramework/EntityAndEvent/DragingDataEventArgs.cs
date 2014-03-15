using System;

namespace DM.Client.WPF.Controls.DragDrop.DragDropFramework.EntityAndEvent
{
    public class DragingDataEventArgs<TDragingData> : EventArgs 
        where TDragingData : class 
    {
        public TDragingData DragingData { get; private set; }

        public DragingDataEventArgs(TDragingData dragingData)
        {
            DragingData = dragingData;
        }
    }
}