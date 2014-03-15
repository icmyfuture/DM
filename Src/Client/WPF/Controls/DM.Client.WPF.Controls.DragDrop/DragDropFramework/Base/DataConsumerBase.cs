using System;
using System.Diagnostics;
using System.Windows;
using DM.Client.WPF.Controls.DragDrop.DragDropFramework.EntityAndEvent;
using DM.Client.WPF.Controls.DragDrop.DragDropFramework.Enum;
using DM.Client.WPF.Controls.DragDrop.DragDropFramework.Interface;

namespace DM.Client.WPF.Controls.DragDrop.DragDropFramework.Base
{
    /// <summary>
    /// This class provides some default implementations for
    /// IDataConsumer that can be used by derived classes.
    /// This class represents dragged data that can be consumed.
    /// </summary>
    public class DataConsumerBase<TDropedData> : IDataConsumer
        where TDropedData : class
    {
        /// <summary>
        /// A list of formats this data object consumer supports
        /// </summary>
        private readonly string[] _dataFormats;

        public event EventHandler<ConsumerDropedEventArgs<TDropedData>> Droped;

        public void OnDroped(object sender, ConsumerDropedEventArgs<TDropedData> e)
        {
            EventHandler<ConsumerDropedEventArgs<TDropedData>> handler = Droped;
            if (handler != null) handler(sender, e);
        }

        /// <summary>
        /// Create a Data Consumer that supports
        /// the specified data formats
        /// </summary>
        /// <param name="dataFormats">Data formats supported by this data consumer</param>
        public DataConsumerBase(params string[] dataFormats)
        {
            _dataFormats = dataFormats;
            Debug.Assert((dataFormats != null) && (dataFormats.Length > 0), "Must have at least one format string");
        }

        #region IDataConsumer Members

        /// <summary>
        /// Returns the actions supported by this data object consumer
        /// </summary>
        public virtual DataConsumerActions DataConsumerActions
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

        /// <summary>
        /// Occurs when mouse enters the area occupied
        /// by the dropTarget (specified in the constructor).
        /// Provide your own method if you wish; making sure
        /// to define DragEnter in DataConsumerActions.
        /// 
        /// See DropTarget_DragEnter in DropManager for additional comments.
        /// </summary>
        /// <param name="sender">Drag event <code>sender</code></param>
        /// <param name="e">DragEnter event arguments</param>
        public virtual void DropTarget_DragEnter(object sender, DragEventArgs e)
        {
            DragOverOrDrop(false, sender, e);
        }

        /// <summary>
        /// Occurs when mouse is over the area occupied
        /// by the dropTarget (specified in the constructor).
        /// You must likely will provide your own method; make sure
        /// to define DragOver in DataConsumerActions.
        /// </summary>
        /// <param name="sender">Drag event <code>sender</code></param>
        /// <param name="e">DragOver event arguments</param>
        public virtual void DropTarget_DragOver(object sender, DragEventArgs e)
        {
            DragOverOrDrop(false, sender, e);
        }

        /// <summary>
        /// Occurs when the left mouse button is released in the area
        /// occupied by the dropTarget (specified in the constructor).
        /// You must likely will provide your own method; make sure
        /// to define Drop in DataConsumerActions.
        /// 
        /// See DropTarget_DragEnter in DropManager for additional comments.
        /// </summary>
        /// <param name="sender">Drag event <code>sender</code></param>
        /// <param name="e">Drop event arguments</param>
        public virtual void DropTarget_Drop(object sender, DragEventArgs e)
        {
            DragOverOrDrop(true, sender, e);
        }

        /// <summary>
        /// Occurs when mouse leaves the area occupied
        /// by the dropTarget (specified in the constructor).
        /// Provide your own method if you wish; making sure
        /// to define DragEnter in DataConsumerActions.
        /// 
        /// See DropTarget_DragLeave in DropManager for additional comments.
        /// </summary>
        /// <param name="sender">Drag event <code>sender</code></param>
        /// <param name="e">DragLeave event arguments</param>
        public virtual void DropTarget_DragLeave(object sender, DragEventArgs e)
        {
            throw new NotImplementedException("DragLeave not implemented");
        }

        #endregion

        /// <summary>
        /// Search the available data formats for a
        /// supported data format and return the first match
        /// </summary>
        /// <param name="e">DragEventArgs from one of the four Drag events</param>
        /// <returns>Returns first available/supported data object match; null when no match is found</returns>
        public virtual object GetData(DragEventArgs e)
        {
            object data = null;
            string[] dataFormats = e.Data.GetFormats();
            foreach (string dataFormat in dataFormats)
            {
                foreach (string dataFormatString in _dataFormats)
                {
                    if (dataFormat.Equals(dataFormatString))
                    {
                        try
                        {
                            data = e.Data.GetData(dataFormat);
                        }
                        catch /*(COMException e2)*/
                        {
                        }
                    }
                    if (data != null)
                        return data;
                }
            }

            return null;
        }

        protected IDataProvider GetProvider(DragEventArgs e)
        {
            IDataProvider provider = null;
            if (GetData(e) is IDataProvider)
                provider = (IDataProvider)GetData(e);

            return provider;
        }

        protected virtual void DragOverOrDrop(bool bDrop, object sender, DragEventArgs e)
        {
            var provider = GetProvider(e);

            if (provider != null)
            {
                if (bDrop)
                {
                    TDropedData dragDropedData = provider.SourceObject as TDropedData;
                    if (dragDropedData == null)
                        return;

                    OnDroped(sender, new ConsumerDropedEventArgs<TDropedData>(dragDropedData, provider));
                }

                e.Effects = DragDropEffects.Move;
                e.Handled = true;
            }
        }
    }
}