using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace DM.Web.SL.Controls.Validator.DefaultIndicator
{
    /// <summary>
    /// 缺省的Indicator。
    /// </summary>
    public partial class DefaultIndicator : IIndicator, IDisposable
    {
        #region Constructors

        /// <summary>
        /// 构造。
        /// </summary>
        public DefaultIndicator()
        {
            InitializeComponent();
            _toolTip = (ToolTip) ToolTipService.GetToolTip(this);
            _tb = (TextBlock) _toolTip.Content;
            _moveMe = (TranslateTransform) _toolTip.RenderTransform;

            EventTrigger et = (EventTrigger) _toolTip.Triggers[0];
            BeginStoryboard bsb = (BeginStoryboard) et.Actions[0];
            DoubleAnimation da = (DoubleAnimation) bsb.Storyboard.Children[0];
            DoubleAnimationUsingKeyFrames daf = (DoubleAnimationUsingKeyFrames) bsb.Storyboard.Children[1];
            Storyboard.SetTarget(da, _tb);
            Storyboard.SetTarget(daf, _moveMe);
            Loaded += Indicator1Loaded;
        }

        #endregion Constructors

        #region Fields

        private readonly TranslateTransform _moveMe;
        private readonly TextBlock _tb;
        private readonly ToolTip _toolTip;

        #endregion Fields

        #region Methods Public

        #region IDisposable Members

        public void Dispose()
        {
            Loaded -= Indicator1Loaded;
        }

        #endregion

        #region IIndicator Members

        public void SetErrMessage(string errMessage)
        {
            _tb.Text = errMessage;
        }

        #endregion

        #endregion Methods Public

        #region Methods Private

        private void Indicator1Loaded(object sender, RoutedEventArgs e)
        {
            UpdateLayout();
            ApplyTemplate();
        }

        #endregion Methods Private
    }
}