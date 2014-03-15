using System;
using System.Windows;

namespace DM.Web.SL.Controls.MessageBox.Effects
{
    public class NoEffect : Effect
    {
        #region Constructors

        public NoEffect(IPopupBox target)
            : base(target)
        {
        }

        #endregion Constructors

        #region Methods

        public override void PerformInEffect()
        {
            Target.Element.Visibility = Visibility.Visible;
            OnComplete(EventArgs.Empty);
        }

        public override void PerformOutEffect()
        {
            Target.Element.Visibility = Visibility.Collapsed;
            OnComplete(EventArgs.Empty);
        }

        #endregion Methods
    }
}