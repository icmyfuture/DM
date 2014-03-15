using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace DM.Web.SL.Common.Utility
{
    /// <summary>
    ///   Make partial UI elements full screen
    ///   Provider features:
    ///   make UI element full screen
    ///   exit full screen and restore UI element
    ///   support UIElement.ToggleElementFullScreen -- extension methods
    /// </summary>
    public static class ElementFullScreenHelper
    {
        #region Members

        private static readonly Popup PopupContainer;
        private static readonly Panel PopupContentContainer;
        private static ElementController m_lastElementController;

        #endregion

        #region ElementController

        /// <summary>
        ///   ElementController
        /// </summary>
        public class ElementController
        {
            private readonly DependencyObject m_parent;
            private double m_height = double.NaN;
            private int m_lastPanelPosition;
            private bool m_lastPopupIsOpen;
            private Thickness? m_margin;
            private double m_width = double.NaN;

            /// <summary>
            ///   ElementController
            /// </summary>
            /// <param name = "element"></param>
            public ElementController(UIElement element)
            {
                Element = element;

                var elem = element as FrameworkElement;
                if (elem != null && elem.Parent != null)
                {
                    m_parent = elem.Parent;
                }
            }

            /// <summary>
            ///   Element
            /// </summary>
            public UIElement Element { get; private set; }

            /// <summary>
            ///   BringElementToFullScreen
            /// </summary>
            public void BringElementToFullScreen()
            {
                TryAction<FrameworkElement>(Element, f =>
                                                     {
                                                         m_height = f.Height;
                                                         m_width = f.Width;

                                                         f.Height = double.NaN;
                                                         f.Width = double.NaN;
                                                     });

                TryAction<Control>(Element, f =>
                                            {
                                                m_margin = f.Margin;

                                                f.Margin = new Thickness(0);
                                            });

                if (m_parent != null)
                {
                    if (!TryAction<Panel>(m_parent, p =>
                                                    {
                                                        m_lastPanelPosition = p.Children.IndexOf(Element);
                                                        p.Children.RemoveAt(m_lastPanelPosition);
                                                    }))
                    {
                        if (!TryAction<ContentControl>(m_parent, c => c.Content = null))
                        {
                            if (!TryAction<UserControl>(m_parent, u => u.Content = null))
                            {
                                TryAction<Popup>(m_parent, p =>
                                                           {
                                                               m_lastPopupIsOpen = p.IsOpen;
                                                               p.Child = null;
                                                           });
                            }
                        }
                    }
                }
            }

            /// <summary>
            ///   ReturnElementFromFullScreen
            /// </summary>
            public void ReturnElementFromFullScreen()
            {
                TryAction<FrameworkElement>(Element, f =>
                                                     {
                                                         f.Height = m_height;
                                                         f.Width = m_width;
                                                     });

                TryAction<Control>(Element, f =>
                                            {
                                                if (m_margin.HasValue)
                                                {
                                                    f.Margin = m_margin.Value;
                                                }
                                            });

                if (m_parent != null)
                {
                    if (!TryAction<Panel>(m_parent, p => p.Children.Insert(m_lastPanelPosition, Element)))
                    {
                        if (!TryAction<ContentControl>(m_parent, c => c.Content = Element))
                        {
                            if (!TryAction<UserControl>(m_parent, u => u.Content = Element))
                            {
                                TryAction<Popup>(m_parent, p =>
                                                           {
                                                               p.Child = Element;
                                                               p.IsOpen = m_lastPopupIsOpen;
                                                           });
                            }
                        }
                    }
                }
            }

            private static bool TryAction<T>(object o, Action<T> action)
                where T:class
            {
                T val = o as T;

                if (val != null)
                {
                    action(val);
                    return true;
                }

                return false;
            }
        }

        #endregion

        #region FullscreenElementID Attached Property

        private static readonly DependencyProperty FullscreenElementIDProperty = DependencyProperty.RegisterAttached(
            "FullscreenElementID", typeof (Guid?), typeof (ElementFullScreenHelper), new PropertyMetadata(null));

        private static void SetFullscreenElementID(DependencyObject obj, Guid? value)
        {
            obj.SetValue(FullscreenElementIDProperty, value);
        }

        private static Guid? GetFullscreenElementID(DependencyObject obj)
        {
            return (Guid?) obj.GetValue(FullscreenElementIDProperty);
        }

        #endregion

        #region Initialization

        /// <summary>
        ///   Initializes the <see cref = "ElementsFullScreenProvider" /> class.
        /// </summary>
        static ElementFullScreenHelper()
        {
            PopupContentContainer = new Grid();

            PopupContainer = new Popup
                             {
                                 Child = PopupContentContainer
                             };

            Application.Current.Host.Content.FullScreenChanged += delegate
                                                                  {
                                                                      if (m_lastElementController == null)
                                                                      {
                                                                          return;
                                                                      }
                                                                      if (!Application.Current.Host.Content.IsFullScreen)
                                                                      {
                                                                          ReturnElementFromFullScreen();
                                                                      }
                                                                      else
                                                                      {
                                                                          UpdateContentSize();
                                                                      }
                                                                  };
        }

        #endregion

        #region Public Methods

        public static void BringElementToFullScreen(this UIElement element)
        {
            if (m_lastElementController == null)
            {
                m_lastElementController = new ElementController(element);

                m_lastElementController.BringElementToFullScreen();

                PopupContentContainer.Children.Add(element);
                PopupContainer.IsOpen = true;
            }
        }

        public static void ReturnElementFromFullScreen(this UIElement element)
        {
            ReturnElementFromFullScreen();
        }

        public static void ReturnElementFromFullScreen()
        {
            if (m_lastElementController != null)
            {
                PopupContentContainer.Children.Clear();

                m_lastElementController.ReturnElementFromFullScreen();

                PopupContainer.IsOpen = false;
                m_lastElementController = null;
            }
        }

        public static void ToggleElementFullScreen(this UIElement element)
        {
            bool newValue = !Application.Current.Host.Content.IsFullScreen;

            bool toggle = false;

            if (newValue)
            {
                if (m_lastElementController == null)
                {
                    element.BringElementToFullScreen();
                    toggle = true;
                }
            }
            else
            {
                if (m_lastElementController != null && ReferenceEquals(element, m_lastElementController.Element))
                {
                    element.ReturnElementFromFullScreen();
                    toggle = true;
                }
            }

            if (toggle)
            {
                ToggleFullScreen();
            }
        }

        public static void ToggleFullScreen()
        {
            Application.Current.Host.Content.IsFullScreen = !Application.Current.Host.Content.IsFullScreen;
        }

        #endregion

        #region Private Method

        private static void UpdateContentSize()
        {
            if (Application.Current != null && Application.Current.Host != null && Application.Current.Host.Content != null)
            {
                double height = Application.Current.Host.Content.ActualHeight;
                double width = Application.Current.Host.Content.ActualWidth;

                //if (Application.Current.Host.Settings.EnableAutoZoom)
                //{
                //    double zoomFactor = Application.Current.Host.Content.ZoomFactor;
                //    if (zoomFactor != 0.0)
                //    {
                //        height /= zoomFactor;
                //        width /= zoomFactor;
                //    }
                //}

                PopupContentContainer.Height = height;
                PopupContentContainer.Width = width;
            }
        }

        #endregion
    }
}