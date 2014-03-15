using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DM.Client.WPF.Controls.Validators.BLL
{
    /// <summary>
    /// 实现一个UI元素移动的动画。
    /// </summary>
    internal static class Animation
    {
        /// <summary>
        /// 移动一个元素到指定元素的位置。
        /// </summary>
        /// <param name="fromElement">被移动的元素。</param>
        /// <param name="toElement">指定的元素。</param>
        /// <param name="offsetX">X坐标偏移量。</param>
        /// <param name="offsetY">Y坐标偏移量。</param>
        /// <param name="root">根Panel，一般是最高级的Panel对象。</param>
        public static void Move(FrameworkElement fromElement, FrameworkElement toElement, double offsetX, double offsetY, Panel root)
        {
            //positionInRoot
            GeneralTransform transform = fromElement.TransformToVisual(root);
            Point positionInRoot = transform.Transform(new Point(0, 0));

            //endPoint
            GeneralTransform initiatorTransform = toElement.TransformToVisual(root);
            Point initiatorLocation = initiatorTransform.Transform(new Point(0, 0));
            Point endPoint = new Point(initiatorLocation.X - positionInRoot.X, initiatorLocation.Y - positionInRoot.Y);

            //currentTranslateTransform
            TranslateTransform currentTranslateTransform = new TranslateTransform();
            currentTranslateTransform.X = endPoint.X + offsetX;
            currentTranslateTransform.Y = endPoint.Y + offsetY;
            fromElement.RenderTransform = currentTranslateTransform;
        }
    }
}