using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using DM.Web.SL.Controls.Validator.DefaultIndicator;

namespace DM.Web.SL.Controls.Validator.BLL
{
    /// <summary>
    /// 表示一个验证器的基类。
    /// </summary>
    public abstract class ValidatorBase : DependencyObject, IDisposable
    {
        #region Fields

        private bool _loaded;
        private Brush _origBackground;
        private SolidColorBrush _origBorder;
        private Thickness? _origBorderThickness = new Thickness( 1 );
        private object _origTooltip;
        private bool _validataWhenKeyUp;
        private string _requiredErrMessage = "This field is required.";
        private bool _requiredValidate;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// 构造。
        /// </summary>
        protected ValidatorBase()
        {
            IsValid = true;
            ManagerName = "";
            ValidationType = ValidationType.Validator;
            if ( Load != null )
                Load( this, new RoutedEventArgs() );
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// 验证状态改变事件。
        /// </summary>
        public event RoutedEventHandler IsValidChanged;

        /// <summary>
        /// 装载事件。
        /// </summary>
        public event RoutedEventHandler Load;

        #endregion Events

        #region Properties

        /// <summary>
        /// 获取或设置被验证的控件。
        /// </summary>
        public FrameworkElement ElementToValidate { get; set; }

        /// <summary>
        /// 获取或设置错误信息。
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 获取或设置提示控件。
        /// </summary>
        public IIndicator Indicator { get; set; }

        /// <summary>
        /// 获取或设置无效状态的背景色。
        /// </summary>
        public Brush InvalidBackground { get; set; }

        /// <summary>
        /// 获取或设置无效状态的边框。
        /// </summary>
        public Brush InvalidBorder { get; set; }

        /// <summary>
        /// 获取或设置无效状态的边框厚度。
        /// </summary>
        public Thickness? InvalidBorderThickness { get; set; }

        /// <summary>
        /// 获取或设置空验证的错误信息，默认为"This field is required."。
        /// </summary>
        public string RequiredErrMessage
        {
            get { return _requiredErrMessage; }
            set { _requiredErrMessage = value; }
        }

        /// <summary>
        /// 获取或设置是否可以为空。
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// 获取或设置验证时是否去除两端的空格。
        /// </summary>
        public bool IsTrim { get; set; }

        /// <summary>
        /// 获取或设置是否有效。
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// 获取或设置验证分组。
        /// </summary>
        public string ManagerName { get; set; }

        /// <summary>
        /// ValidatorManager通过该属性获取实例。
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 获取或设置被验证控件的父UserControl Or ChildWindow。
        /// </summary>
        public Control ParentUserControlOrChildWindow { get; set; }

        /// <summary>
        /// 获取或设置验证类型。
        /// </summary>
        public ValidationType ValidationType { get; set; }

        /// <summary>
        /// 获取或设置验证分组。
        /// </summary>
        protected ValidatorManager Manager { get; set; }

        /// <summary>
        /// 获取或设置是否为异步验证。
        /// </summary>
        public bool IsAsynchronous { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 执行验证路由。
        /// </summary>
        public virtual void ActivateValidationRoutine()
        {
            ElementToValidate.LostFocus += ElementToValidate_LostFocus;
            if ( _validataWhenKeyUp )
                ElementToValidate.KeyUp += ElementToValidate_KeyUp;
        }

        /// <summary>
        /// 初始化。
        /// </summary>
        /// <param name="element">被验证的控件。</param>
        public void Initialize( FrameworkElement element )
        {
            ElementToValidate = element;
            element.Loaded += element_Loaded;
        }

        /// <summary>
        /// 设置无效状态的样式。
        /// </summary>
        public virtual void SetInvalidStyle()
        {
            if ( !string.IsNullOrEmpty( ErrorMessage ) )
            {
                //causing a onownermouseleave error currently...
                ElementToValidate.ClearValue( ToolTipService.ToolTipProperty );
                if ( !_requiredValidate )
                    Indicator.SetErrMessage( RequiredErrMessage );
                else
                    Indicator.SetErrMessage( ErrorMessage );
                SetToolTip( ToolTipService.GetToolTip( (DependencyObject)Indicator ) );
            }

            Helper.SetSytle( ElementToValidate, InvalidBorder, InvalidBorderThickness, InvalidBackground );
        }

        /// <summary>
        /// 设置验证分组和被验证的控件。
        /// </summary>
        /// <param name="manager">验证分组。</param>
        /// <param name="element">被验证的控件。</param>
        public void SetManagerAndControl( ValidatorManager manager, FrameworkElement element )
        {
            Manager = manager;
            ElementToValidate = element;
        }

        /// <summary>
        /// 设置有效状态的样式。
        /// </summary>
        public virtual void SetValidStyle()
        {
            if ( _origTooltip != null )
                SetToolTip( _origTooltip );
            else
                SetToolTip( null );

            Helper.SetSytle( ElementToValidate, _origBorder, _origBorderThickness, _origBackground );
        }

        /// <summary>
        /// 验证。
        /// </summary>
        /// <returns>验证通过返回true，否则返回false。</returns>
        public bool Validate()
        {
            bool newIsValid;
            HandleTrim();
            bool validateControl = ValidateControl( Helper.GetText( ElementToValidate ) );
            _requiredValidate = RequiredValidate();
            if ( validateControl && _requiredValidate )
                newIsValid = true;
            else
                newIsValid = false;

            bool isValidChanged = ( IsValid != newIsValid );
            IsValid = newIsValid;

            if ( isValidChanged )
            {
                if ( IsValidChanged != null )
                    IsValidChanged( this, null );
            }

            return IsValid;
        }

        /// <summary>
        /// 丢失焦点事件。
        /// </summary>
        protected virtual void ElementToValidate_LostFocus( object sender, RoutedEventArgs e )
        {
            if ( IsAsynchronous )
            {
                Dispatcher.BeginInvoke( () => AsynValidateControl( Helper.GetText( ElementToValidate ) ) );
                return;
            }
            Dispatcher.BeginInvoke( () => Validate() );
        }

        /// <summary>
        /// 按键弹起事件。
        /// </summary>
        protected void ElementToValidate_KeyUp( object sender, KeyEventArgs e )
        {
            if ( IsAsynchronous )
            {
                Dispatcher.BeginInvoke(() => AsynValidateControl(Helper.GetText(ElementToValidate)));
                return;
            }
            Dispatcher.BeginInvoke( () => Validate() );
        }

        /// <summary>
        /// 执行验证。
        /// </summary>
        /// <param name="text">待验证的文本。</param>
        /// <returns>成功返回true，失败返回false。</returns>
        protected abstract bool ValidateControl( string text );

        /// <summary>
        /// 执行异步验证。
        /// </summary>
        /// <param name="text">待验证的文本。</param>
        /// <returns>返回验证结果。</returns>
        public abstract void AsynValidateControl(string text);
        /// <summary>
        /// 根据IsRequired属性验证是否为空。
        /// </summary>
        /// <returns>通过验证返回true，否则返回false。</returns>
        private bool RequiredValidate()
        {
            if ( !IsRequired )
                return true;

            string text = Helper.GetText( ElementToValidate );
            return !string.IsNullOrEmpty( text );
        }

        /// <summary>
        /// 如果IsTrim为true，把两端的空格去除。
        /// </summary>
        private void HandleTrim()
        {
            if ( IsTrim )
            {
                string text = Helper.GetText( ElementToValidate );
                text = text.Trim();
                Helper.SetText( ElementToValidate, text );
            }
        }

        /// <summary>
        /// 查找分组。
        /// </summary>
        /// <param name="userControl">被验证控件的父用户控件。</param>
        /// <param name="groupName">分组名。</param>
        /// <returns>返回查找到的分组。</returns>
        private static ValidatorManager FindManager( Control userControl, string groupName )
        {
            const string defaultName = "_DefaultValidatorManager";
            var mgr = userControl.FindName( groupName );
            if ( mgr == null )
                mgr = userControl.FindName( defaultName );
            if ( mgr == null )
            {
                mgr = new ValidatorManager
                          {
                              Name = defaultName
                          };
                Panel g = userControl.FindName( "LayoutRoot" ) as Panel;
                if ( g == null )
                    return null;
                g.Children.Add( mgr as ValidatorManager );
            }
            return mgr as ValidatorManager;
        }

        /// <summary>
        /// 查找上一级最近的父UserControl或ChildWindow。
        /// </summary>
        /// <param name="element">Control to validate</param>
        private static Control FindUserControl( FrameworkElement element )
        {
            if ( element == null )
                return null;
            if ( element.Parent != null )
            {
                UserControl userControl = element.Parent as UserControl;
                if ( userControl != null )
                    return userControl;

                ChildWindow childWindow = element.Parent as ChildWindow;
                if ( childWindow != null )
                    return childWindow;

                return FindUserControl( element.Parent as FrameworkElement );
            }
            return null;
        }

        /// <summary>
        /// 加载事件。
        /// </summary>
        private void element_Loaded( object sender, RoutedEventArgs e )
        {
            if ( !_loaded )
            {
                ParentUserControlOrChildWindow = FindUserControl( ElementToValidate );

                //no usercontrol.  throw error?
                if ( ParentUserControlOrChildWindow == null )
                    return;

                Manager = FindManager( ParentUserControlOrChildWindow, ManagerName );
                if ( Manager == null )
                {
                    Debug.WriteLine( String.Format( "No ValidatorManager found named '{0}'", ManagerName ) );
                    throw new Exception( String.Format( "No ValidatorManager found named '{0}'", ManagerName ) );
                }
                Manager.Register( this );

                //_validataWhenKeyUp
                _validataWhenKeyUp = Manager.ValidataWhenKeyUp;

                if ( ValidationType == ValidationType.Validator )
                    ActivateValidationRoutine();

                //Use the properties from the manager if they are not set at the control level
                if ( InvalidBackground == null )
                    InvalidBackground = Manager.InvalidBackground;

                if ( InvalidBorder == null )
                {
                    InvalidBorder = Manager.InvalidBorder;

                    if ( InvalidBorderThickness != null )
                        if ( InvalidBorderThickness.Value.Bottom == 0 )
                            InvalidBorderThickness = Manager.InvalidBorderThickness;
                }

                Helper.GetStyle( ElementToValidate, out _origBorder, out _origBorderThickness, out _origBackground, out _origTooltip );

                if ( Indicator == null )
                {
                    Type x = Manager.Indicator.GetType();
                    Indicator = x.GetConstructor( Type.EmptyTypes ).Invoke( null ) as IIndicator;
                    foreach ( var param in x.GetProperties() )
                    {
                        var val = param.GetValue( Manager.Indicator, null );
                        if ( param.CanWrite && val != null && val.GetType().IsPrimitive )
                            param.SetValue( Indicator, val, null );
                    }
                }

                _loaded = true;
            }
            ElementToValidate.Loaded -= element_Loaded;
        }

        /// <summary>
        /// Dispatcher方式设置ToolTip。
        /// </summary>
        private void SetToolTip( object toolTip )
        {
            //Dispatcher.BeginInvoke(() =>
            //                           {
            ToolTipService.SetPlacement( ElementToValidate, PlacementMode.Right );
            ToolTipService.SetToolTip( ElementToValidate, toolTip );
            //}
            //);
        }

        #endregion Methods

        #region IDisposable Members

        public void Dispose()
        {
            _origBorderThickness = null;
            Load = null;
            IsValidChanged = null;
            InvalidBackground = null;
            ElementToValidate.Loaded -= element_Loaded;
            ElementToValidate.LostFocus -= ElementToValidate_LostFocus;
            ElementToValidate.KeyUp -= ElementToValidate_KeyUp;
            ElementToValidate = null;
            InvalidBorder = null;
            InvalidBorderThickness = null;
            ParentUserControlOrChildWindow = null;
            Manager.Dispose();
            Manager = null;
        }

        #endregion
    }
}