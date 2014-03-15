#define PRINT2BUFFER
#define PRINT2OUTPUT

using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using DM.Client.WPF.Controls.DragDrop.DragDropFramework;
using DM.Client.WPF.Controls.DragDrop.DragDropFramework.Base;


namespace DM.Client.WPF.Controls.DragDrop.DataProvider
{
    /// <summary>
    /// This Data Provider represents items found on a ToolBar.
    /// Note that text specific to the object is also added to the drag data;
    /// this allows canvas items to be dragged onto the Rich Text Box.
    /// </summary>
    /// <typeparam name="TContainer">Drag source container type</typeparam>
    /// <typeparam name="TObject">Drag source object type</typeparam>
    public class ToolBarProvider<TContainer, TObject> : DataProviderBase<TContainer, TObject>
        where TContainer : ItemsControl
        where TObject : UIElement
    {
        public ToolBarProvider(string dataFormatString) :
            base(dataFormatString)
        {
        }

        #region IDataProvider Members

        public override bool IsSupportedContainerAndObject(bool initFlag, object dragSourceContainer, object dragSourceObject,
                                                           object dragOriginalSourceObject)
        {
            TObject sourceObject = dragSourceObject as TObject;
            // When an image button is clicked,
            // most of the time the image is the <code>e.Source</code>.
            // So when _SourceObject is null, search for a TObject parent.
            if (sourceObject == null)
                sourceObject = Utilities.FindParentControlExcludingMe<TObject>(dragSourceObject as DependencyObject);

            if (initFlag)
            {
                // Init DataProvider variables
                Init();
                SourceContainer = dragSourceContainer;
                SourceObject = sourceObject;
                OriginalSourceObject = dragOriginalSourceObject;
            }

            return
                (dragSourceContainer is TContainer) &&
                (sourceObject != null)
                ;
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
            TContainer container = (TContainer) SourceContainer;

            Debug.Assert(item != null, "Unparent expects a non-null item");
            Debug.Assert(container != null, "Unparent expects a non-null container");

            if ((container != null) && (item != null))
                container.Items.Remove(item);
        }

        #endregion
    }
}