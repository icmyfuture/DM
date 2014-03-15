using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DM.Web.SL.Controls.MessageBox
{
    public class PopupService
    {
        #region Fields

        private static readonly IDictionary<Panel, PopupService> m_Cache = new Dictionary<Panel, PopupService>();

        private readonly LayoutMask m_Mask;
        private readonly Panel m_Owner;

        #endregion Fields

        #region Constructors

        private PopupService(Panel owner)
        {
            m_Owner = owner;
            m_Mask = new LayoutMask(owner);
        }

        #endregion Constructors

        #region Properties

        protected static IDictionary<Panel, PopupService> Cache
        {
            get
            {
                return m_Cache;
            }
        }

        protected virtual LayoutMask LayoutMask
        {
            get
            {
                return m_Mask;
            }
        }

        protected virtual Panel Owner
        {
            get
            {
                return m_Owner;
            }
        }

        #endregion Properties

        #region Methods

        public static PopupService GetServiceFor(Panel owner)
        {
            if (!Cache.ContainsKey(owner))
            {
                PopupService service = new PopupService(owner);
                Cache[owner] = service;
            }
            return Cache[owner];
        }

        public BoxPage GetBoxPage(FrameworkElement content, string title, bool draggable, bool showCloseBox, ImageSource closeIcon)
        {
            BoxPage box = new BoxPage()
            {
                ContentElement = content,
                Title = title,
                ShowCloseBox = showCloseBox,
                CloseIcon = closeIcon,
            };
            box.DragService.IsDraggable = draggable;

            RegisterPopupBox(box);

            return box;
        }

        public BoxPage GetBoxPage(FrameworkElement content, string title, bool draggable, bool showCloseBox)
        {
            return GetBoxPage(content, title, draggable, showCloseBox, null);
        }

        public BoxPage GetBoxPage(FrameworkElement content, string title, bool draggable)
        {
            return GetBoxPage(content, title, draggable, true, null);
        }

        public BoxPage GetBoxPage(FrameworkElement content, string title)
        {
            return GetBoxPage(content, title, true, true, null);
        }

        public BoxPage GetBoxPage(FrameworkElement content)
        {
            return GetBoxPage(content, String.Empty, true, true, null);
        }

        public MessagePage GetMessagePage(string message, string title, bool draggable, MessageBoxButtonType buttonType, MessageBoxIcon icon)
        {
            MessagePage box = new MessagePage()
            {
                Message = message,
                Title = title,
                ButtonType = buttonType,
                Icon = icon
            };
            box.DragService.IsDraggable = draggable;

            RegisterPopupBox(box);

            return box;
        }

        public MessagePage GetMessagePage(string message, string title, bool draggable, MessageBoxButtonType buttonType)
        {
            return GetMessagePage(message, title, draggable, buttonType, null);
        }

        public MessagePage GetMessagePage(string message, string title, bool draggable)
        {
            return GetMessagePage(message, title, draggable, MessageBoxButtonType.YesNo, null);
        }

        public MessagePage GetMessagePage(string message, string title)
        {
            return GetMessagePage(message, title, true, MessageBoxButtonType.YesNo, null);
        }

        public MessagePage GetMessagePage(string message)
        {
            return GetMessagePage(message, String.Empty, true, MessageBoxButtonType.YesNo, null);
        }

        public void RegisterPopupBox(IPopupBox box)
        {
            box.Mask = LayoutMask;
            if (box.Effect == null)
            {
                box.Effect = Effect.NoEffect(box);
            }
        }

        #endregion Methods
    }
}