using System.Windows;
using System.Windows.Controls;

namespace DM.Web.SL.Controls.DragLibrary
{
    internal partial class DragProxy
    {
        #region Constructors

        public DragProxy(FrameworkElement element)
        {
            InitializeComponent();
            Canvas.SetZIndex(element, 0);
            _content.Content = element;
            Height = element.Height;
            Width = element.Width;
            LayoutRoot.Height = element.Height;
            LayoutRoot.Width = element.Width;
            UpdateLayout();
        }

        #endregion Constructors

        #region Methods

        public void Accept()
        {
            imgOk.Visibility = Visibility.Visible;
            imgErr.Visibility = Visibility.Collapsed;
        }

        public void Deny()
        {
            imgOk.Visibility = Visibility.Collapsed;
            imgErr.Visibility = Visibility.Visible;
        }

        #endregion Methods
    }
}