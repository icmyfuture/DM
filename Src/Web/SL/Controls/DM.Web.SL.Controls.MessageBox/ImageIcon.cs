using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace DM.Web.SL.Controls.MessageBox
{
    public class ImageIcon : MessageBoxIcon
    {
        #region Fields

        private readonly string m_SourceUri;

        #endregion Fields

        #region Constructors

        public ImageIcon(string sourceUri)
        {
            m_SourceUri = sourceUri;
        }

        #endregion Constructors

        #region Properties

        protected virtual string SourceUri
        {
            get
            {
                return m_SourceUri;
            }
        }

        #endregion Properties

        #region Methods

        public override FrameworkElement GetContentIcon()
        {
            Image image = new Image()
            {
                Source = new BitmapImage(new Uri(SourceUri, UriKind.Relative))
            };
            return image;
        }

        public override FrameworkElement GetTitleIcon()
        {
            Image image = new Image()
            {
                Source = new BitmapImage(new Uri(SourceUri, UriKind.Relative))
            };
            return image;
        }

        #endregion Methods
    }
}