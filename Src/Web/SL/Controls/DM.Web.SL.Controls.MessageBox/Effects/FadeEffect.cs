using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace DM.Web.SL.Controls.MessageBox.Effects
{
    public class FadeEffect : Effect
    {
        #region Constructors

        public FadeEffect(IPopupBox target)
            : base(target)
        {
        }

        #endregion Constructors

        #region Methods

        public override void PerformInEffect()
        {
            DoubleAnimation animationForOpacity = new DoubleAnimation();
            Storyboard.SetTarget(animationForOpacity, Target.Element);
            Storyboard.SetTargetProperty(
                animationForOpacity, new PropertyPath(FrameworkElement.OpacityProperty));
            animationForOpacity.From = 0;
            animationForOpacity.To = 1;

            Storyboard storyBoard = new Storyboard();
            storyBoard.RepeatBehavior = new RepeatBehavior(1);
            storyBoard.Children.Add(animationForOpacity);

            storyBoard.Completed += new EventHandler(StoryBoard_Completed);

            storyBoard.Begin();
        }

        public override void PerformOutEffect()
        {
            DoubleAnimation animationForOpacity = new DoubleAnimation();
            Storyboard.SetTarget(animationForOpacity, Target.Element);
            Storyboard.SetTargetProperty(
                animationForOpacity, new PropertyPath(FrameworkElement.OpacityProperty));
            animationForOpacity.From = 1;
            animationForOpacity.To = 0;

            Storyboard storyBoard = new Storyboard();
            storyBoard.RepeatBehavior = new RepeatBehavior(1);
            storyBoard.Children.Add(animationForOpacity);

            storyBoard.Completed += new EventHandler(StoryBoard_Completed);

            storyBoard.Begin();
        }

        private void StoryBoard_Completed(object sender, EventArgs e)
        {
            OnComplete(EventArgs.Empty);
        }

        #endregion Methods
    }
}