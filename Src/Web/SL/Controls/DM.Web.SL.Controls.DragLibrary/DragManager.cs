using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace DM.Web.SL.Controls.DragLibrary
{
    /// <summary>
    /// 实现拖放操作，并伴随一些动态效果。
    /// </summary>
    /// <typeparam name="T">拖放操作中拖放项的数据类型。</typeparam>
    public class DragManager<T>
    {
        #region 构造

        /// <summary>
        /// 构造。
        /// </summary>
        /// <param name="root">设置根Panel，一般是最高层的Panel对象。</param>
        /// <param name="gridRow">如果根的Panel有Grid控件并定义了行，则为最大高度的行索引，否则为null。</param>
        /// <param name="gridColumn">如果根的Panel有Grid控件并定义了列，则为最大宽度的列索引，否则为null。</param>
        /// <param name="dropTargetWidth">接受“放下”操作的目标的宽度，只有当root在子窗体时才设置该参数，否则设置为null。</param>
        /// <param name="dropTargetHeight">接受“放下”操作的目标的高度，只有当root在子窗体时才设置该参数，否则设置为null。</param>
        public DragManager( Panel root, int? gridRow, int? gridColumn, double? dropTargetWidth, double? dropTargetHeight )
        {
            Init( root, gridRow, gridColumn, dropTargetWidth, dropTargetHeight );
        }

        /// <summary>
        /// 构造。
        /// </summary>
        public DragManager()
        {

        }

        #endregion

        #region 事件

        /// <summary>
        /// Fired when a succesfull drop has occured.
        /// </summary>
        public event EventHandler<DragEventArgs<T>> Dropped;

        /// <summary>
        /// Fired when the mouse button was released while not over any drop target.
        /// </summary>
        public event EventHandler<DragEventArgs<T>> Ended;

        #endregion

        #region 私有字段

        private readonly SyncList<UIElement> _dropTargets = new SyncList<UIElement>();
        private ScaleTransform _currentScaleTransform;
        private TranslateTransform _currentTranslateTransform;
        private T _dragData;
        private FrameworkElement _dragInitiator;
        private DragProxy _dragProxy;
        private double _proxyAlpha;
        private Point _returnLocation;
        private Panel _root;
        private Point _startingMouseOffSet;
        private Point _startingPositionInRoot;
        private GeneralTransform _transform;
        private double? _dropTargetWidth;
        private double? _dropTargetHeight;
        private int? _gridRow;
        private int? _gridColumn;
        #endregion

        #region 属性

        /// <summary>
        /// 获取开始拖动的对象。
        /// </summary>
        public FrameworkElement DragInitiator
        {
            get { return _dragInitiator; }
        }

        /// <summary>
        /// 接受放下动作的UI对象。
        /// </summary>
        public List<UIElement> DropTargets
        {
            get { return _dropTargets; }
        }

        /// <summary>
        /// 获取拖放图像的透明度。
        /// </summary>
        public double ProxyAlpha
        {
            get { return _proxyAlpha; }
        }

        /// <summary>
        /// 获取拖放传送的数据对象。
        /// </summary>
        public T DragData
        {
            get { return _dragData; }
        }

        /// <summary>
        /// 获取是否处于拖放动作中。
        /// </summary>
        public bool IsDragging
        {
            get { return _dragInitiator != null; }
        }

        #endregion

        #region 公有方法

        /// <summary>
        /// 开始拖动，显示dragInitiator的图像，直接鼠标按钮释放，在这个操作前必须调用RegisterDropTarget()进行注册目标对象。
        /// </summary>
        /// <param name="dragInitiator">开始拖动，接受MouseLeftbuttonDown事件的对象。</param>
        /// <param name="eventArgs">关联到dragInitiator的 MouseLeftButtonDown事件。</param>
        /// <param name="dragData">拖放传送的数据对象。</param>
        /// <param name="proxyAlpha">拖放图像的透明度。</param>
        public void StartDrag( FrameworkElement dragInitiator, MouseEventArgs eventArgs, T dragData,
                              double proxyAlpha )
        {
            //Drag animation still playing, so return.
            if ( IsDragging )
            {
                return;
            }
            //Can't drag without a drop target!
            if ( _dropTargets.Count == 0 )
            {
                throw new InvalidOperationException(
                    "Can't start a drag without a droptarget. Register a droptarget first!" );
            }

            //Root needed, dragging must be on top level container
            if ( _root == null )
                _root = (Panel)VisualTreeHelper.GetChild( Application.Current.RootVisual, 0 );
            _dragInitiator = dragInitiator;

            //Creating the prox image, which is a snapshot of the draginitiator
            var proxyImage = new Image();
            proxyImage.Source = new WriteableBitmap( dragInitiator, dragInitiator.RenderTransform );
            proxyImage.Width = dragInitiator.ActualWidth;
            proxyImage.Height = dragInitiator.ActualHeight;
            _dragProxy = new DragProxy( proxyImage );
            _dragData = dragData;
            _proxyAlpha = proxyAlpha;
            _dragProxy.Background = new SolidColorBrush( Colors.Blue );
            _startingMouseOffSet = eventArgs.GetPosition( _dragInitiator );

            //Transforms, needed for drop animation and keeping the dragproxy in sync with the mouse
            _currentTranslateTransform = new TranslateTransform();
            _currentScaleTransform = new ScaleTransform();
            proxyImage.Opacity = _proxyAlpha;
            var tr = new TransformGroup();
            tr.Children.Add( _currentScaleTransform );
            tr.Children.Add( _currentTranslateTransform );
            _dragProxy.RenderTransform = tr;

            //The proxy gets translated, so ideal for Bitmap caching......
            _dragProxy.CacheMode = new BitmapCache();

            //That we're not showing the dragproxy right away, but on the first mousemovement.
            //This enables us to still receive ordinary mousebuttonup events on the draginitiator
            _root.MouseMove += root_FirstDrag;
            _root.MouseLeftButtonUp += root_MouseLeftButtonUpWithOutDragging;
        }

        /// <summary>
        /// 注册一个UIElement对象，表示该对象可以接受“放下”操作，之前必须调用StartDrag()方法。
        /// </summary>
        /// <param name="dropTarget">The drop target itself.</param>
        public void RegisterDropTarget( UIElement dropTarget )
        {
            foreach ( var item in DropTargets )
                if ( item == dropTarget )
                    return;

            _dropTargets.Add( dropTarget );
        }

        /// <summary>
        /// 清空全部目标。
        /// </summary>
        public void RemoveAllDropTargets()
        {
            _dropTargets.Clear();
        }

        /// <summary>
        /// 清空指定目标。
        /// </summary>
        public void RemoveDropTarget( UIElement dropTarget )
        {
            _dropTargets.Remove( dropTarget );
        }

        /// <summary>
        /// 初始化控件。
        /// </summary>
        /// <param name="root">设置根Panel，一般是最高层的Panel对象。</param>
        /// <param name="gridRow">如果根的Panel有Grid控件并定义了行，则为最大高度的行索引，否则为null。</param>
        /// <param name="gridColumn">如果根的Panel有Grid控件并定义了列，则为最大宽度的列索引，否则为null。</param>
        /// <param name="dropTargetWidth">接受“放下”操作的目标的宽度，只有当root在子窗体时才设置该参数，否则设置为null。</param>
        /// <param name="dropTargetHeight">接受“放下”操作的目标的高度，只有当root在子窗体时才设置该参数，否则设置为null。</param>
        public void Init( Panel root, int? gridRow, int? gridColumn, double? dropTargetWidth, double? dropTargetHeight )
        {
            _gridRow = gridRow;
            _gridColumn = gridColumn;
            _root = root;
            _dropTargetWidth = dropTargetWidth;
            _dropTargetHeight = dropTargetHeight;
        }

        #endregion

        #region 事件处理

        /// <summary>
        /// This eventHandler is when startDrag is called, but the user releases the left mouse button before making a movement.
        /// This handler cleans up resources.
        /// </summary>
        private void root_MouseLeftButtonUpWithOutDragging( object sender, MouseButtonEventArgs e )
        {
            if ( _root == null )
                return;

            _root.MouseMove -= root_FirstDrag;
            _root.MouseLeftButtonUp -= root_MouseLeftButtonUpWithOutDragging;
            CleanUp();
        }

        /// <summary>
        /// This eventHandler is called when a user drags for the first time, and the proxy must become visible.
        /// </summary>
        private void root_FirstDrag( object sender, MouseEventArgs e )
        {
            if ( _root == null )
                return;

            // Adding proxy first, so the starting position of the proxy that the root has given it,
            // can be determined.
            if ( _gridRow.HasValue )
                Grid.SetRow( _dragProxy, _gridRow.Value );
            if ( _gridColumn.HasValue )
                Grid.SetColumn( _dragProxy, _gridColumn.Value );
            _root.Children.Add( _dragProxy );

            //Forcing of layout updating needed, so the starting position can be determined.
            _root.UpdateLayout();
            _dragProxy.CaptureMouse();

            //General transforms needed to get the positions of the dragproxy and the draginitiator
            //in the root panel's coordinate space
            _transform = _dragProxy.TransformToVisual( _root );
            GeneralTransform initiatorTransform = _dragInitiator.TransformToVisual( _root );

            Point initiatorLocation = initiatorTransform.Transform( new Point( 0, 0 ) );
            Point positionInRoot = GetProxyLocation();
            //Saving location the proxy needs to return to, needed for animation when drag ended without dropping.
            _returnLocation = new Point( initiatorLocation.X - positionInRoot.X, initiatorLocation.Y - positionInRoot.Y );

            //Determining the starting position to which the dragproxy needs to be translated.
            Point mouse = e.GetPosition( _root );
            double xDifference = mouse.X - positionInRoot.X - _startingMouseOffSet.X;
            double yDifference = mouse.Y - positionInRoot.Y - _startingMouseOffSet.Y;
            _startingPositionInRoot = positionInRoot;

            _currentTranslateTransform.X = xDifference;
            _currentTranslateTransform.Y = yDifference;


            ////Adding new eventhandlers, to handle the drag and removing the old ones.
            _dragProxy.MouseMove += dragProxy_AfterFirstDrag;
            _dragProxy.MouseLeftButtonUp += dragProxy_MouseLeftButtonUpAfterDragging;
            _root.MouseMove -= root_FirstDrag;
            _root.MouseLeftButtonUp -= root_MouseLeftButtonUpWithOutDragging;
        }

        /// <summary>
        /// Gets called when user releases left mouse button while dragging.
        /// </summary>
        private void dragProxy_MouseLeftButtonUpAfterDragging( object sender, MouseButtonEventArgs e )
        {
            if ( _root == null )
                return;

            UIElement dropedTarget;
            if ( IsOverDropTarget( e.GetPosition( _root ), out dropedTarget ) )
            {
                //If released above the drop target, remove listeners and start the drop animation.
                _dragProxy.ReleaseMouseCapture();
                _dragProxy.MouseMove -= dragProxy_AfterFirstDrag;
                DoDrop( e.GetPosition( _dragProxy ), dropedTarget );
            }
            else
            {
                //If not released above the drop target, end the drag and animate the proxy, returning to it's
                //original position.
                EndDrag();
            }
        }

        /// <summary>
        ///  Constantly gets called while the user is dragging the drag proxy. Tests wether the proxy is over the
        ///  droptarget and gives visual feedback.
        /// </summary>
        private void dragProxy_AfterFirstDrag( object sender, MouseEventArgs e )
        {
            if ( _root == null )
                return;

            Point currentMousePosition = e.GetPosition( _root );
            _currentTranslateTransform.X = currentMousePosition.X - _startingPositionInRoot.X - _startingMouseOffSet.X;
            _currentTranslateTransform.Y = currentMousePosition.Y - _startingPositionInRoot.Y - _startingMouseOffSet.Y;

            UIElement dropedTarget;
            if ( IsOverDropTarget( currentMousePosition, out dropedTarget ) )
            {
                _dragProxy.Accept();
            }
            else
            {
                _dragProxy.Deny();
            }
        }

        #endregion

        #region 私有辅助方法

        /// <summary>
        /// 处理Dropped事件。
        /// </summary>
        /// <param name="args"></param>
        private void OnDropped( DragEventArgs<T> args )
        {
            if ( Dropped != null )
                Dropped( typeof( DragManager<T> ), args );
        }

        /// <summary>
        /// 处理Ended事件。
        /// </summary>
        private void OnEnded( DragEventArgs<T> args )
        {
            if ( Ended != null )
                Ended( typeof( DragManager<T> ), args );
        }

        /// <summary>
        /// Gets the absolute location of the proxy.
        /// </summary>
        /// <returns> a point with the location of the proxy, relative to the rootvisual</returns>
        private Point GetProxyLocation()
        {
            return _transform.Transform( new Point( 0, 0 ) );
        }

        /// <summary>
        ///  Helper method to determine wether a user is currently dragging above the drop target.
        /// </summary>
        /// <param name="intersectingPoint">The current point, which must be tested for intersecting with the droptarget.</param>
        /// <param name="dropedTarget">return dropedTarget。</param>
        /// <returns>true when over droptarget, false otherwise.</returns>
        private bool IsOverDropTarget( Point intersectingPoint, out UIElement dropedTarget )
        {
            dropedTarget = null;
            lock ( _dropTargets.SyncRoot )
            {
                foreach ( UIElement dropTarget in _dropTargets )
                {
                    if ( _dropTargetWidth.HasValue )
                    {
                        var transform = dropTarget.TransformToVisual( _root );
                        if ( _dropTargetHeight != null )
                        {
                            var rect = transform.TransformBounds( new Rect( 0, 0, _dropTargetWidth.Value, _dropTargetHeight.Value ) );
                            if ( intersectingPoint.X >= rect.Left && intersectingPoint.X <= rect.Right &&
                                intersectingPoint.Y >= rect.Top && intersectingPoint.Y <= rect.Bottom
                                )
                            {
                                dropedTarget = dropTarget;
                                return true;
                            }
                        }
                    }
                    else
                    {
                        IEnumerable<UIElement> collidingElements = VisualTreeHelper.FindElementsInHostCoordinates(
                            intersectingPoint, dropTarget );
                        dropedTarget = dropTarget;
                        if ( collidingElements.Any() )
                            return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Ends the drag, animating the proxy back to it's starting position.
        /// </summary>
        private void EndDrag()
        {
            var sb = new Storyboard();
            var an = new DoubleAnimation();
            an.Duration = new Duration( TimeSpan.FromSeconds( 0.5 ) );
            Storyboard.SetTarget( an, _currentTranslateTransform );
            Storyboard.SetTargetProperty( an, new PropertyPath( TranslateTransform.XProperty ) );
            an.To = _returnLocation.X;
            sb.Children.Add( an );
            an = new DoubleAnimation();
            an.Duration = new Duration( TimeSpan.FromSeconds( 0.5 ) );
            Storyboard.SetTarget( an, _currentTranslateTransform );
            Storyboard.SetTargetProperty( an, new PropertyPath( TranslateTransform.YProperty ) );
            an.To = _returnLocation.Y;
            sb.Children.Add( an );
            sb.Completed += ( o, e ) =>
            {
                OnEnded( new DragEventArgs<T>( _dragInitiator, null, _dragData ) );
                CleanUp();
            };
            sb.Begin();
        }

        /// <summary>
        /// Performs the actual drop and plays the dropping animation. After the animation,
        /// calls the Drop method on the drop target, passing the drop data to the drop target.
        /// </summary>
        private void DoDrop( Point scalePoint, UIElement dropedTarget )
        {
            var sb = new Storyboard();
            var an = new DoubleAnimation();
            an.Duration = new Duration( TimeSpan.FromSeconds( .5 ) );
            Storyboard.SetTarget( an, _currentScaleTransform );
            Storyboard.SetTargetProperty( an, new PropertyPath( ScaleTransform.ScaleXProperty ) );
            an.To = 0;
            sb.Children.Add( an );
            an = new DoubleAnimation();
            an.Duration = new Duration( TimeSpan.FromSeconds( .5 ) );
            Storyboard.SetTarget( an, _currentScaleTransform );
            Storyboard.SetTargetProperty( an, new PropertyPath( ScaleTransform.ScaleYProperty ) );
            _currentScaleTransform.CenterX = scalePoint.X;
            _currentScaleTransform.CenterY = scalePoint.Y;
            an.To = 0;
            sb.Children.Add( an );
            sb.Completed += ( o, e ) =>
            {
                OnDropped( new DragEventArgs<T>( _dragInitiator, dropedTarget, _dragData ) );
                CleanUp();
            };
            sb.Begin();
        }

        /// <summary>
        /// Frees resources, does not free the droptarget, so it can be reused for another drop operation.
        /// </summary>
        private void CleanUp()
        {
            if ( _dragProxy != null )
            {
                _dragProxy.ReleaseMouseCapture();
                if ( _dragProxy.Parent != null )
                {
                    ( (Panel)_dragProxy.Parent ).Children.Remove( _dragProxy );
                }
            }
            _dragInitiator = null;
            _dragProxy = null;
            _dragData = default( T );
            _proxyAlpha = default( double );
            _startingMouseOffSet = default( Point );
            _currentScaleTransform = null;
            _currentTranslateTransform = null;
            _startingPositionInRoot = default( Point );
            _returnLocation = default( Point );
            _transform = null;
        }

        #endregion
    }
}