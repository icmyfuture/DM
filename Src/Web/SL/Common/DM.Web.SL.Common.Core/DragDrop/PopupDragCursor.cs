using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using DM.Web.SL.Common.Core.DragDrop.Primitives;

namespace DM.Web.SL.Common.Core.DragDrop
{
    #region Imports

    

    #endregion

    /// <summary>
    ///   <see cref = "DragCursor" /> implementation that uses a <see cref = "Popup" /> control to host its content.
    /// </summary>
    public class PopupDragCursor : DragCursor
    {
        #region Fields

        #region Dependency Properties

        public static readonly DependencyProperty AllowDropTemplateProperty = DependencyProperty.Register(
            "AllowDropTemplate", typeof (DataTemplate), typeof (PopupDragCursor), new PropertyMetadata(AllowDropTemplateChanged));

        public static readonly DependencyProperty ProhibitDropTemplateProperty = DependencyProperty.Register(
            "ProhibitDropTemplate", typeof (DataTemplate), typeof (PopupDragCursor), new PropertyMetadata(ProhibitDropTemplateChanged));

        #endregion

        private Popup _popup;
        private ContentPresenter _popupChildPresenter;

        #endregion

        /// <summary>
        ///   Gets or sets the <see cref = "DataTemplate" /> to use when the current drag-and-drop target prohibits dropping.
        /// </summary>
        public DataTemplate ProhibitDropTemplate
        {
            get { return (DataTemplate) GetValue(ProhibitDropTemplateProperty); }
            set { SetValue(ProhibitDropTemplateProperty, value); }
        }

        /// <summary>
        ///   Gets or sets the <see cref = "DataTemplate" /> to use when the current drag-and-drop target allows dropping.
        /// </summary>
        public DataTemplate AllowDropTemplate
        {
            get { return (DataTemplate) GetValue(AllowDropTemplateProperty); }
            set { SetValue(AllowDropTemplateProperty, value); }
        }

        private static void ProhibitDropTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((PopupDragCursor) d).OnProhibitDropTemplateChanged();
        }

        private static void AllowDropTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((PopupDragCursor) d).OnAllowDropTemplateChanged();
        }

        protected virtual void OnAllowDropTemplateChanged()
        {
            UpdateTemplate();
        }

        protected virtual void OnProhibitDropTemplateChanged()
        {
            UpdateTemplate();
        }

        protected override void OnBeginDrag(IMouseEventArgs args)
        {
            var position = args.GetPosition(null);

            // define presenter for child content
            _popupChildPresenter = new ContentPresenter();
            _popupChildPresenter.Content = DataContext;

            // define and open popup
            _popup = new Popup();
            _popup.Child = _popupChildPresenter;
            _popup.VerticalOffset = position.Y;
            _popup.HorizontalOffset = position.X;
            _popup.IsOpen = true;
        }

        protected override void OnDataContextChanged()
        {
            if (_popupChildPresenter != null)
            {
                _popupChildPresenter.Content = DataContext;
            }
        }

        protected override void OnEffectsChanged()
        {
            base.OnEffectsChanged();
            UpdateTemplate();
        }

        private void UpdateTemplate()
        {
            // update the template based on the current value of Effects
            if (_popupChildPresenter != null)
            {
                switch (Effects)
                {
                    case DragDropEffects.None:
                        _popupChildPresenter.ContentTemplate = ProhibitDropTemplate;
                        break;
                    default:
                        _popupChildPresenter.ContentTemplate = AllowDropTemplate;
                        break;
                }
            }
        }

        protected override void OnDrag(IMouseEventArgs args)
        {
            // adjust the position of the popup
            var position = args.GetPosition(null);
            _popup.VerticalOffset = position.Y;
            _popup.HorizontalOffset = position.X;
        }

        protected override void OnEndDrag()
        {
            if (_popup != null)
            {
                // detach data context (have seen EngineExecutionException raised on PropertyChanges on the DataContext)
                _popupChildPresenter.Content = null;

                // close and clear popup
                _popup.IsOpen = false;
                _popup = null;
            }
        }
    }
}