using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using DM.Client.WPF.Controls.DragDrop.DragDropFramework.Base;
using DM.Client.WPF.Controls.DragDrop.DragDropFramework.Enum;

namespace DM.Client.WPF.Controls.DragDrop.DataProvider
{
    /// <summary>
    /// This Data Provider represents ListBoxItems.
    /// </summary>
    /// <typeparam name="TContainer">Drag source container type</typeparam>
    /// <typeparam name="TObject">Drag source object type</typeparam>
    public class ListBoxProvider<TContainer, TObject> : DataProviderBase<TContainer, TObject>
        where TContainer : ItemsControl
        where TObject : FrameworkElement
    {
        public ListBoxProvider(string dataFormatString) :
            base(dataFormatString)
        {
        }

        #region IDataProvider Members

        public override DataProviderActions DataProviderActions
        {
            get
            {
                return
                    DataProviderActions.QueryContinueDrag | // Need Shift key info (for TreeView)
                    DataProviderActions.GiveFeedback |
                    //DragDropDataProviderActions.DoDragDrop_Done |
                    DataProviderActions.None;
            }
        }

        public override void Unparent()
        {
            TObject item = SourceObject as TObject;
            TContainer container = SourceContainer as TContainer;

            Debug.Assert(item != null, "Unparent expects a non-null item");
            Debug.Assert(container != null, "Unparent expects a non-null container");

            if ((container != null) && (item != null))
                container.Items.Remove(item);
        }

        #endregion
    }
}