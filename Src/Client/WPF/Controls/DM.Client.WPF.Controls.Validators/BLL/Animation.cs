using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DM.Client.WPF.Controls.Validators.BLL
{
    /// <summary>
    /// ʵ��һ��UIԪ���ƶ��Ķ�����
    /// </summary>
    internal static class Animation
    {
        /// <summary>
        /// �ƶ�һ��Ԫ�ص�ָ��Ԫ�ص�λ�á�
        /// </summary>
        /// <param name="fromElement">���ƶ���Ԫ�ء�</param>
        /// <param name="toElement">ָ����Ԫ�ء�</param>
        /// <param name="offsetX">X����ƫ������</param>
        /// <param name="offsetY">Y����ƫ������</param>
        /// <param name="root">��Panel��һ������߼���Panel����</param>
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