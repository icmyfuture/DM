using System.Windows;
using System.Windows.Controls;

namespace DM.Client.WPF.Controls.DragDrop.DragDropFramework.DragProxy
{
    internal partial class DragProxy
    {
        #region Constructors

        public DragProxy(FrameworkElement element)
        {
            Image image = new Image();
            image.Source = Utilities.CreateImage(element, 1);
            image.Height = element.Height;
            image.Width = element.Width;

            InitializeComponent();
            Panel.SetZIndex(image, 0);
            _content.Content = image;
            Height = image.Height;
            Width = image.Width;
            LayoutRoot.Height = image.Height;
            LayoutRoot.Width = image.Width;
            txt.Text = "10";
            UpdateLayout();
        }

        #endregion Constructors
    }
}