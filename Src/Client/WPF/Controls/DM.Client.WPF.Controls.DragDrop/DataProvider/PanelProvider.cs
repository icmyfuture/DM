using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using DM.Client.WPF.Controls.DragDrop.DragDropFramework;
using DM.Client.WPF.Controls.DragDrop.DragDropFramework.Base;


namespace DM.Client.WPF.Controls.DragDrop.DataProvider
{
    /// <summary>
    /// This Data Provider represents items found on a Canvas,
    /// allowing them to be drag data.
    /// Note that text specific to the object is also added to the drag data;
    /// this allows canvas items to be dragged onto the Rich Text Box.
    /// </summary>
    /// <typeparam name="TContainer">Drag source container type</typeparam>
    /// <typeparam name="TObject">Drag source object type</typeparam>
    public class PanelProvider<TContainer, TObject> : DataProviderBase<TContainer, TObject>
        where TContainer : Panel
        where TObject : UIElement
    {
        public PanelProvider(string dataFormatString) :
            base(dataFormatString)
        {
        }

        #region IDataProvider Members

        /// <summary>
        /// Return true so an addorner is added when an item is dragged
        /// </summary>
        public override bool AddAdorner
        {
            get { return true; }
        }

        public override bool IsSupportedContainerAndObject(bool initFlag, object dragSourceContainer, object dragSourceObject,
                                                           object dragOriginalSourceObject)
        {
            TObject sourceObject = dragSourceObject as TObject;
            // When an image button is clicked,
            // most of the time the image is the <code>e.Source</code>.
            // So when _SourceObject is null, search for a TObject parent.
            if (sourceObject == null)
            {
                // Image buttons can return the image as the source, so look for the button
                sourceObject = Utilities.FindParentControlExcludingMe<TObject>(dragOriginalSourceObject as DependencyObject);
            }

            if (initFlag)
            {
                // Init DataProvider variables
                Init();
                SourceContainer = dragSourceContainer;
                SourceObject = sourceObject;
                OriginalSourceObject = dragOriginalSourceObject;
            }

            return (dragSourceContainer is TContainer) && (sourceObject != null);
        }

        /// <summary>
        /// Not only add the DataProvider class, also add a string
        /// </summary>
        public override void SetData(ref DataObject data)
        {
            // Set default data
            Debug.Assert(data.GetDataPresent(SourceDataFormat) == false, "Shouldn't set data more than once");
            data.SetData(SourceDataFormat, this);

            // Look for a System.String
            string textString = null;

            if (SourceObject is Rectangle)
            {
                Rectangle rect = (Rectangle) SourceObject;
                if (rect.Fill != null)
                    textString = rect.Fill.ToString();
            }
            else if (SourceObject is TextBlock)
            {
                TextBlock textBlock = (TextBlock) SourceObject;
                textString = textBlock.Text;
            }
            else if (SourceObject is Button)
            {
                Button button = (Button) SourceObject;
                if (button.ToolTip != null)
                    textString = button.ToolTip.ToString();
            }

            if (textString != null)
                data.SetData(textString);
        }

        public override void Unparent()
        {
            TObject item = SourceObject as TObject;
            TContainer panel = SourceContainer as TContainer;

            if ((panel != null) && (item != null))
                panel.Children.Remove(item);
        }

        #endregion
    }
}