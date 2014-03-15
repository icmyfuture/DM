using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using DM.Client.WPF.Controls.DragDrop.DragDropFramework;
using DM.Client.WPF.Controls.DragDrop.DragDropFramework.Base;
using DM.Client.WPF.Controls.DragDrop.DragDropFramework.Enum;

namespace DM.Client.WPF.Controls.DragDrop.DataProvider
{
    /// <summary>
    /// This Data Provider represents TreeViewItems.
    /// 
    /// Note that a TreeViewItem's container can be
    /// either a TreeView or another TreeViewItem.
    /// </summary>
    /// <typeparam name="TContainer">Drag source container type</typeparam>
    /// <typeparam name="TObject">Drag source object type</typeparam>
    public class TreeViewProvider<TContainer, TObject> : DataProviderBase<TContainer, TObject>
        where TContainer : ItemsControl
        where TObject : ItemsControl
    {
        public TreeViewProvider(string dataFormatString)
            : base(dataFormatString)
        {
        }

        #region IDataProvider Members

        public override DataProviderActions DataProviderActions
        {
            get
            {
                return
                    DataProviderActions.QueryContinueDrag | // Need Shift key info
                    DataProviderActions.GiveFeedback |
                    //DragDropDataProviderActions.DoDragDrop_Done |
                    DataProviderActions.None;
            }
        }

        public override void DragSource_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            if (e.Effects == DragDropEffects.Move)
            {
                e.UseDefaultCursors = true;
                e.Handled = true;
            }
            else if (e.Effects == DragDropEffects.Link)
            {
                e.UseDefaultCursors = true;
                e.Handled = true;
            }
        }

        public override void Unparent()
        {
            TObject item = SourceObject as TObject;
            // 'container' can be either TreeView or another TreeViewItem
            TContainer container = Utilities.FindParentControlExcludingMe<TContainer>(item);

            Debug.Assert(item != null, "Unparent expects a non-null item");
            Debug.Assert(container != null, "Unparent expects a non-null container");

            if ((container != null) && (item != null))
                container.Items.Remove(item);
        }

        #endregion
    }
}