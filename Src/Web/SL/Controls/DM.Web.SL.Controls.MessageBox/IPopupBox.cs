using System.Windows;

namespace DM.Web.SL.Controls.MessageBox
{
    public interface IPopupBox
    {
        #region Properties

        UIElement DragMouseCaptureArea
        {
            get;
        }

        DragService DragService
        {
            get;
        }

        Effect Effect
        {
            get;
            set;
        }

        FrameworkElement Element
        {
            get;
        }

        bool IsModal
        {
            get;
        }

        LayoutMask Mask
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        void Close();

        void Show();

        void ShowAsModal();

        #endregion Methods
    }
}