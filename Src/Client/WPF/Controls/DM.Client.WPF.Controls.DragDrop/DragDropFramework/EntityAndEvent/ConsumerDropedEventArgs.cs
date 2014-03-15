using System;
using DM.Client.WPF.Controls.DragDrop.DragDropFramework.Interface;

namespace DM.Client.WPF.Controls.DragDrop.DragDropFramework.EntityAndEvent
{
    public class ConsumerDropedEventArgs<TDropedData> : EventArgs where TDropedData : class 
    {
        public TDropedData DropedData { get; private set; }
        public IDataProvider Provider { get; private set; }

        public ConsumerDropedEventArgs(TDropedData dropedData, IDataProvider provider)
        {
            DropedData = dropedData;
            Provider = provider;
        }
    }
}