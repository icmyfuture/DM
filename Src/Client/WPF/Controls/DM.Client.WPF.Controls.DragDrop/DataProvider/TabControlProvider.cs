using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DM.Client.WPF.Controls.DragDrop.DragDropFramework.Base;
using DM.Client.WPF.Controls.DragDrop.DragDropFramework.Interface;


namespace DM.Client.WPF.Controls.DragDrop.DataProvider
{
    /// <summary>
    /// This Data Provider represents TabItems.
    /// Note that custom cursors are used.
    /// When a TabItem is dragged within its
    /// original container, the cursor is an arrow,
    /// otherwise its a custom cursor with an
    /// arrow and a page.
    /// </summary>
    /// <typeparam name="TContainer">Drag source container type</typeparam>
    /// <typeparam name="TObject">Drag source object type</typeparam>
    public class TabControlProvider<TContainer, TObject> : DataProviderBase<TContainer, TObject>, IDataProvider
        where TContainer : ItemsControl
        where TObject : TabItem
    {
        private static readonly Cursor MovePageCursor =
            new Cursor(Assembly.GetExecutingAssembly().GetManifestResourceStream("DM.Client.WPF.Controls.DragDrop.Images.MovePage.cur"));

        private static readonly Cursor MovePageNotCursor =
            new Cursor(Assembly.GetExecutingAssembly().GetManifestResourceStream("DM.Client.WPF.Controls.DragDrop.Images.MovePageNot.cur"));

        public TabControlProvider(string dataFormatString)
            : base(dataFormatString)
        {
        }

        #region IDataProvider Members

        public override void DragSource_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            if (e.Effects == DragDropEffects.Move)
            {
                // Move the tab to be the first in the list
                e.UseDefaultCursors = false;
                Mouse.OverrideCursor = MovePageCursor;
                e.Handled = true;
            }
            else if (e.Effects == DragDropEffects.Link)
            {
                // Drag tabs around
                e.UseDefaultCursors = false;
                Mouse.OverrideCursor = Cursors.Arrow;
                e.Handled = true;
            }
            else
            {
                e.UseDefaultCursors = false;
                Mouse.OverrideCursor = MovePageNotCursor;
                e.Handled = true;
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