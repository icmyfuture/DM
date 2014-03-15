using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace DM.Web.SL.Common.Core.DragDrop.Primitives
{
    #region Imports

    

    #endregion

    public class DragOperation
    {
        #region Fields

        private Cursor _cursor;
        private double _deltaX;
        private double _deltaY;
        private bool _isActive;
        private TextBox _keyboardTracker;
        private Popup _mouseTracker;
        private Control _previousFocus;
        private MouseEventArgs _previousMouseArgs;
        private Point _previousPosition;

        #endregion

        public bool IsActive
        {
            get { return _isActive; }
        }

        public Cursor Cursor
        {
            get { return _cursor; }
            set
            {
                if (_cursor != value)
                {
                    _cursor = value;
                    if (_mouseTracker != null)
                    {
                        _mouseTracker.Cursor = value;
                    }
                }
            }
        }

        /// <summary>
        ///   Starts a drag operation from the specified <see cref = "MouseEventArgs" />
        /// </summary>
        /// <param name = "fromArgs"></param>
        /// <param name = "cursor"></param>
        /// <returns><c>true</c> if the drag operation was started; otherwise, <c>false</c>.</returns>
        /// <remarks>
        ///   The <see cref = "DragOperation" /> must capture the mouse to start a drag operation. If mouse capture 
        ///   is not granted the method will return <c>false</c>.
        /// </remarks>
        public bool Start(MouseEventArgs fromArgs, Cursor cursor)
        {
            if (_isActive)
            {
                throw new InvalidOperationException();
            }

            _cursor = cursor;
            try
            {
                //  bind mouse and keyboard trackers
                if (!InitializeTrackers())
                {
                    return false;
                }

                //  start
                StartInternal(fromArgs);

                //  done!!
                return true;
            }
            catch
            {
                CleanupTrackers();
                throw;
            }
        }

        public bool Start(MouseEventArgs fromArgs)
        {
            return Start(fromArgs, null);
        }

        private void StartInternal(MouseEventArgs fromArgs)
        {
            //	reset deltas
            _deltaX = 0.0;
            _deltaY = 0.0;

            //  save initial position (used to calculate deltas)
            _previousPosition = fromArgs.GetPosition(_mouseTracker);

            //  save MouseEventArgs, required when the operation is cancelled
            _previousMouseArgs = fromArgs;

            //  notify
            OnDragStarted(new DragOperationStartedEventArgs(_previousMouseArgs, _deltaX, _deltaY));

            //  done
            _isActive = true;
        }

        private bool InitializeTrackers()
        {
            // save current focus (we set focus to the keyboard tracker and must restore at the end)
            _previousFocus = FocusManager.GetFocusedElement() as Control;

            //	create mouse tracker (use popup to ensure tracking is not affected by changes in visual tree)
            var mouseTracker = new Popup();
            if (!mouseTracker.CaptureMouse())
            {
                return false;
            }

            // initialize keyboard tracker
            _keyboardTracker = new TextBox
                               {
                                   Width = 0.0,
                                   Height = 0.0,
                                   Opacity = 0.0
                               };
            _keyboardTracker.KeyDown += KeyDown;
            _keyboardTracker.LostFocus += LostFocus;

            // complete initialization of mouse tracker 
            _mouseTracker = mouseTracker;
            _mouseTracker.Child = _keyboardTracker;
            _mouseTracker.MouseMove += MouseMove;
            _mouseTracker.MouseLeftButtonUp += MouseLeftButtonUp;
            _mouseTracker.LostMouseCapture += LostMouseCapture;

            // open the popup
            _mouseTracker.IsOpen = true;

            //	sync cursor
            _mouseTracker.Cursor = _cursor;

            //  set focus to track keyboard events
            _keyboardTracker.Focus();

            //  done!!
            return true;
        }

        private void LostMouseCapture(object sender, MouseEventArgs e)
        {
            SetComplete(false);
        }

        private void LostFocus(object sender, RoutedEventArgs e)
        {
            SetComplete(false);
        }

        private void CleanupTrackers()
        {
            //  restore focus
            if (_previousFocus != null)
            {
                _previousFocus.Focus();
                _previousFocus = null;
            }

            //  clear keyboard tracker
            if (_keyboardTracker != null)
            {
                _keyboardTracker.KeyDown -= KeyDown;
                _keyboardTracker.LostFocus -= LostFocus;
                _keyboardTracker = null;
            }

            //  clear mouse tracker
            if (_mouseTracker != null)
            {
                _mouseTracker.MouseMove -= MouseMove;
                _mouseTracker.MouseLeftButtonUp -= MouseLeftButtonUp;
                _mouseTracker.LostMouseCapture -= LostMouseCapture;
                _mouseTracker.ReleaseMouseCapture();
                _mouseTracker.IsOpen = false;
                _mouseTracker = null;
            }
        }

        private void SetComplete(bool success)
        {
            if (!_isActive)
            {
                return;
            }

            //	de-activate
            _isActive = false;

            //	unbind from target
            CleanupTrackers();

            //	notify
            OnDragCompleted(new DragOperationCompletedEventArgs(_previousMouseArgs, _deltaX, _deltaY, !success));
        }

        private void KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                SetComplete(false);
            }
        }

        private void MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _previousMouseArgs = e;
            SetComplete(true);
        }

        private void MouseMove(object sender, MouseEventArgs e)
        {
            if (!_isActive)
            {
                return;
            }

            _previousMouseArgs = e;

            //	calculate delta
            var position = e.GetPosition((UIElement) sender);
            if (position != _previousPosition)
            {
                _deltaX += (position.X - _previousPosition.X);
                _deltaY += (position.Y - _previousPosition.Y);
                _previousPosition = position;

                OnDragDelta(new DragOperationDeltaEventArgs(e, _deltaX, _deltaY));
            }
        }

        protected virtual void OnDragStarted(DragOperationStartedEventArgs args)
        {
            var handler = DragStarted;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        protected virtual void OnDragDelta(DragOperationDeltaEventArgs args)
        {
            var handler = DragDelta;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        protected virtual void OnDragCompleted(DragOperationCompletedEventArgs args)
        {
            var handler = DragCompleted;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        public void Cancel()
        {
            if (!_isActive)
            {
                throw new InvalidOperationException();
            }
            SetComplete(false);
        }

        public event DragOperationStartedEventHandler DragStarted;

        public event DragOperationCompletedEventHandler DragCompleted;

        public event DragOperationDeltaEventHandler DragDelta;
    }
}