using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DM.Client.WPF.Controls.DragDrop.DragDropFramework.EntityAndEvent
{
    public class DefaultAdorner : Adorner
    {
        private readonly UIElement _child;
        private Point _adornerOffset;
        private Point _adornerOrigin;
        private readonly double _tipLeft;
        private readonly double _tipTop;
        private const double _tipHeight = 17;
        private static readonly BitmapSource _tipImage9 = ToBitmapSource(ResourceDic.t9);
        private static readonly BitmapSource _tipImage99 = ToBitmapSource(ResourceDic.t99);
        private static readonly BitmapSource _tipImage999 = ToBitmapSource(ResourceDic.t999);
        private static readonly BitmapSource _tipImage9999 = ToBitmapSource(ResourceDic.t9999);
        private readonly int _dragNo;
        private const int _tipWidth9 = 17;
        private const int _tipWidth99 = 25;
        private const int _tipWidth999 = 33;
        private const int _tipWidth9999 = 41;

        /// <summary>
        /// Create an adorner.
        /// The created adorner must then be added to the AdornerLayer.
        /// </summary>
        /// <param name="adornedElement">Element whose AdornerLayer will be use for displaying the adorner</param>
        /// <param name="adornerElement">Element used as adorner</param>
        /// <param name="adornerOrigin">Origin offset within the adorner</param>
        /// <param name="opacity">Adorner's opacity</param>
        public DefaultAdorner(UIElement adornedElement, UIElement adornerElement, Point adornerOrigin, int dragNo = 1,  double opacity = 0.3)
            : base(adornedElement)
        {
            Rectangle rect = new Rectangle();
            rect.Width = adornerElement.RenderSize.Width;
            rect.Height = adornerElement.RenderSize.Height;

            //DragProxy dp = new DragProxy((FrameworkElement)adornerElement);

            VisualBrush visualBrush = new VisualBrush(adornerElement);
            visualBrush.Opacity = opacity;
            visualBrush.Stretch = Stretch.None;
            rect.Fill = visualBrush;

            _child = rect;
            _adornerOrigin = adornerOrigin;

            _tipLeft = (adornerElement.RenderSize.Width - 22)/2;
            _tipTop = adornerElement.RenderSize.Height + 3;

            _dragNo = dragNo;
            if (_dragNo <= 0)
                _dragNo = 1;
            if (_dragNo >= 10000 )
               _dragNo = 9999;
        }

        protected override int VisualChildrenCount
        {
            get { return 1; }
        }

        /// <summary>
        /// Set the position of and redraw the adorner.
        /// Call when the mouse cursor position changes.
        /// </summary>
        /// <param name="position">Adorner's new position relative to AdornerLayer origin</param>
        public void SetMousePosition(Point position)
        {
            _adornerOffset.X = position.X - _adornerOrigin.X;
            _adornerOffset.Y = position.Y - _adornerOrigin.Y;
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            AdornerLayer adornerLayer = (AdornerLayer) Parent;
            if (adornerLayer != null)
            {
                adornerLayer.Update(AdornedElement);
            }
        }

        protected override Visual GetVisualChild(int index)
        {
            Debug.Assert(index == 0, "Index must be 0, there's only one child");
            return _child;
        }

        protected override Size MeasureOverride(Size finalSize)
        {
            _child.Measure(finalSize);
            return _child.DesiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            _child.Arrange(new Rect(finalSize));
            return finalSize;
        }

        public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
        {
            GeneralTransformGroup newTransform = new GeneralTransformGroup();
            newTransform.Children.Add(base.GetDesiredTransform(transform));
            newTransform.Children.Add(new TranslateTransform(_adornerOffset.X, _adornerOffset.Y));
            return newTransform;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (_dragNo == 1 || _dragNo == 0)
                return;
            
            if (_dragNo >= 2 && _dragNo <= 9)
                OnRender_9(drawingContext);
            else if (_dragNo >= 10 && _dragNo <= 99)
                OnRender_99(drawingContext);
            else if (_dragNo >= 100 && _dragNo <= 999)
                OnRender_999(drawingContext);
            else if (_dragNo >= 1000 && _dragNo <= 9999)
                OnRender_9999(drawingContext);
            else
                throw new ArgumentOutOfRangeException();
        }

        private void OnRender_9(DrawingContext drawingContext)
        {
            drawingContext.DrawImage(_tipImage9, new Rect(_tipLeft, _tipTop, _tipWidth9, _tipHeight));

            string testString = _dragNo.ToString();
            FormattedText formattedText = new FormattedText(testString, CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight,
                                                            new Typeface("Verdana"), 32, Brushes.Black);
            formattedText.SetFontSize(11, 0, 1);
            //formattedText.SetFontWeight(FontWeights.Bold, 0, 1);
            formattedText.SetForegroundBrush(new SolidColorBrush(Colors.White));
            drawingContext.DrawText(formattedText, new Point(_tipLeft + 5, _tipTop + 1));
        }

        private void OnRender_99(DrawingContext drawingContext)
        {
            drawingContext.DrawImage(_tipImage99, new Rect(_tipLeft, _tipTop, _tipWidth99, _tipHeight));

            string testString = _dragNo.ToString();
            FormattedText formattedText = new FormattedText(testString, CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight,
                                                            new Typeface("Verdana"), 32, Brushes.Black);
            formattedText.SetFontSize(11, 0, 2);
            //formattedText.SetFontWeight(FontWeights.Bold, 0, 2);
            formattedText.SetForegroundBrush(new SolidColorBrush(Colors.White));
            drawingContext.DrawText(formattedText, new Point(_tipLeft + 5, _tipTop + 1));
        }

        private void OnRender_999(DrawingContext drawingContext)
        {
            drawingContext.DrawImage(_tipImage999, new Rect(_tipLeft, _tipTop, _tipWidth999, _tipHeight));

            string testString = _dragNo.ToString();
            FormattedText formattedText = new FormattedText(testString, CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight,
                                                            new Typeface("Verdana"), 32, Brushes.Black);
            formattedText.SetFontSize(11, 0, 3);
            //formattedText.SetFontWeight(FontWeights.Bold, 0, 3);
            formattedText.SetForegroundBrush(new SolidColorBrush(Colors.White));
            drawingContext.DrawText(formattedText, new Point(_tipLeft + 5, _tipTop + 1));
        }

        private void OnRender_9999(DrawingContext drawingContext)
        {
            drawingContext.DrawImage(_tipImage9999, new Rect(_tipLeft, _tipTop, _tipWidth9999, _tipHeight));

            string testString = _dragNo.ToString();
            FormattedText formattedText = new FormattedText(testString, CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight,
                                                            new Typeface("Verdana"), 32, Brushes.Black);
            formattedText.SetFontSize(11, 0, 4);
            //formattedText.SetFontWeight(FontWeights.Bold, 0, 3);
            formattedText.SetForegroundBrush(new SolidColorBrush(Colors.White));
            drawingContext.DrawText(formattedText, new Point(_tipLeft + 5, _tipTop + 1));
        }

        private static BitmapSource ToBitmapSource(System.Drawing.Bitmap source)
        {
            BitmapSource bitSrc = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                source.GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            return bitSrc;
        }
    }
}