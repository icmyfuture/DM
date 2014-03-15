using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace DM.Web.SL.Controls.MessageBox.Effects
{
    public class ZoomEffect : Effect
    {
        #region Constructors

        public ZoomEffect(IPopupBox target)
            : base(target)
        {
        }

        #endregion Constructors

        #region Methods

        public override void PerformInEffect()
        {
            DoubleAnimation animationForWidth = new DoubleAnimation();
            Storyboard.SetTarget(animationForWidth, Target.Element);
            Storyboard.SetTargetProperty(
                animationForWidth, new PropertyPath(FrameworkElement.WidthProperty));
            animationForWidth.From = 0;
            animationForWidth.To = Target.Element.ActualWidth;

            DoubleAnimation animationForHeight = new DoubleAnimation();
            Storyboard.SetTarget(animationForHeight, Target.Element);
            Storyboard.SetTargetProperty(
                animationForHeight, new PropertyPath(FrameworkElement.HeightProperty));
            animationForHeight.From = 0;
            animationForHeight.To = Target.Element.ActualHeight;

            Storyboard storyBoard = new Storyboard();
            storyBoard.RepeatBehavior = new RepeatBehavior(1);
            storyBoard.Children.Add(animationForWidth);
            storyBoard.Children.Add(animationForHeight);

            storyBoard.Completed += new EventHandler(StoryBoard_Completed);

            storyBoard.Begin();
        }

        public override void PerformOutEffect()
        {
            DoubleAnimation animationForWidth = new DoubleAnimation();
            Storyboard.SetTarget(animationForWidth, Target.Element);
            Storyboard.SetTargetProperty(
                animationForWidth, new PropertyPath(FrameworkElement.WidthProperty));
            animationForWidth.From = Target.Element.ActualWidth;
            animationForWidth.To = 0;

            DoubleAnimation animationForHeight = new DoubleAnimation();
            Storyboard.SetTarget(animationForHeight, Target.Element);
            Storyboard.SetTargetProperty(
                animationForHeight, new PropertyPath(FrameworkElement.HeightProperty));
            animationForHeight.From = Target.Element.ActualHeight;
            animationForHeight.To = 0;

            Storyboard storyBoard = new Storyboard();
            storyBoard.RepeatBehavior = new RepeatBehavior(1);
            storyBoard.Children.Add(animationForWidth);
            storyBoard.Children.Add(animationForHeight);

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