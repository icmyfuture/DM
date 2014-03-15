using DM.Web.SL.Controls.MessageBox.Effects;
using System;

namespace DM.Web.SL.Controls.MessageBox
{
    public abstract class Effect
    {
        #region Fields

        private readonly IPopupBox m_Target;

        #endregion Fields

        #region Constructors

        public Effect(IPopupBox target)
        {
            m_Target = target;
        }

        #endregion Constructors

        #region Events

        public event EventHandler Complete;

        #endregion Events

        #region Properties

        protected virtual IPopupBox Target
        {
            get
            {
                return m_Target;
            }
        }

        #endregion Properties

        #region Methods

        public static Effect Fade(IPopupBox target)
        {
            return new FadeEffect(target);
        }

        public static Effect NoEffect(IPopupBox target)
        {
            return new NoEffect(target);
        }

        public static Effect Zoom(IPopupBox target)
        {
            return new ZoomEffect(target);
        }

        public abstract void PerformInEffect();

        public abstract void PerformOutEffect();

        protected virtual void OnComplete(EventArgs e)
        {
            EventHandler handler = Complete;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion Methods
    }
}