using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DM.Web.SL.Controls.MessageBox
{
    public class DragService
    {
        #region Fields

        private readonly IPopupBox m_Box;

        private bool m_IsDraggable = false;

        #endregion Fields

        #region Constructors

        public DragService(IPopupBox popupBox)
        {
            m_Box = popupBox;
        }

        #endregion Constructors

        #region Properties

        public bool IsDraggable
        {
            get
            {
                return m_IsDraggable;
            }
            set
            {
                if (m_IsDraggable == value)
                {
                    return;
                }
                if (value)
                {
                    EnableDrag();
                }
                else
                {
                    DisableDrag();
                }
            }
        }

        public bool IsDragging
        {
            get;
            set;
        }

        public Point LastDragPosition
        {
            get;
            set;
        }

        protected virtual IPopupBox PopupBox
        {
            get
            {
                return m_Box;
            }
        }

        #endregion Properties

        #region Methods

        private static void MoveElement(FrameworkElement element, Point lastPosition, Point currentPosition)
        {
            //计算移动距离
            double xDelta = currentPosition.X - lastPosition.X;
            double yDelta = currentPosition.Y - lastPosition.Y;

            //移动
            double left = Canvas.GetLeft(element) + xDelta;
            double top = Canvas.GetTop(element) + yDelta;
            Canvas.SetLeft(element, left);
            Canvas.SetTop(element, top);
        }

        private void DisableDrag()
        {
            PopupBox.DragMouseCaptureArea.MouseLeftButtonDown -= Drag_MouseLeftButtonDown;
            PopupBox.DragMouseCaptureArea.MouseMove -= Drag_MouseMove;
            PopupBox.DragMouseCaptureArea.MouseLeftButtonUp -= Drag_MouseLeftButtonUp;
        }

        private void Drag_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //开始拖动
            IsDragging = true;

            //设定鼠标位置
            LastDragPosition = e.GetPosition(PopupBox.Mask.MaskPanel);

            //锁定鼠标
            PopupBox.DragMouseCaptureArea.CaptureMouse();
        }

        private void Drag_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (IsDragging)
            {
                //停止拖动
                IsDragging = false;

                //释放鼠标
                PopupBox.DragMouseCaptureArea.ReleaseMouseCapture();

                //移动
                MoveElement(
                    PopupBox.Element,
                    LastDragPosition,
                    e.GetPosition(PopupBox.Mask.MaskPanel)
                );
            }
        }

        private void Drag_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsDragging)
            {
                Point currentPosition = e.GetPosition(PopupBox.Mask.MaskPanel);
                //移动
                MoveElement(
                    PopupBox.Element,
                    LastDragPosition,
                    currentPosition
                );
                LastDragPosition = currentPosition;
            }
        }

        private void EnableDrag()
        {
            PopupBox.DragMouseCaptureArea.MouseLeftButtonDown +=
                new MouseButtonEventHandler(Drag_MouseLeftButtonDown);
            PopupBox.DragMouseCaptureArea.MouseMove +=
                new MouseEventHandler(Drag_MouseMove);
            PopupBox.DragMouseCaptureArea.MouseLeftButtonUp +=
                new MouseButtonEventHandler(Drag_MouseLeftButtonUp);
        }

        #endregion Methods
    }
}