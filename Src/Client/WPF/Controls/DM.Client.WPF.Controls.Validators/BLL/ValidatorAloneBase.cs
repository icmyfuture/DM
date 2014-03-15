using System.Linq;
using DM.Client.WPF.Controls.Validators.DefaultIndicator;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Windows.Media;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;

namespace DM.Client.WPF.Controls.Validators.BLL
{
    /// <summary>
    /// 表示一个验证器的基类。
    /// </summary>
    public abstract class ValidatorAloneBase : DependencyObject, IDisposable
    {
        #region Constructors

        /// <summary>
        /// 构造。
        /// </summary>
        protected ValidatorAloneBase()
        {
            IsRequired = false;
            IsValid = true;
            ValidationType = ValidationType.Validator;
        }

        #endregion Constructors

        #region Fields

        private static readonly DependencyProperty ElementToValidateProperty = DependencyProperty.Register("ElementToValidate", typeof(FrameworkElement), typeof(ValidatorAloneBase));
        private static readonly DependencyProperty ErrorMessageProperty = DependencyProperty.Register("ErrorMessage", typeof(string), typeof(ValidatorAloneBase));
        private static readonly DependencyProperty IndicatorProperty = DependencyProperty.Register("Indicator", typeof(IIndicator), typeof(ValidatorAloneBase));
        private static readonly DependencyProperty InvalidBorderProperty = DependencyProperty.Register("InvalidBorder", typeof(Brush), typeof(ValidatorAloneBase));
        private static readonly DependencyProperty IsRequiredProperty = DependencyProperty.Register("IsRequired", typeof(bool), typeof(ValidatorAloneBase));
        private static readonly DependencyProperty IsTrimProperty = DependencyProperty.Register("IsTrim", typeof(bool), typeof(ValidatorAloneBase));
        private static readonly DependencyProperty IsValidProperty = DependencyProperty.Register("IsValid", typeof(bool), typeof(ValidatorAloneBase));
        private static readonly DependencyProperty KeyProperty = DependencyProperty.Register("Key", typeof(string), typeof(ValidatorAloneBase));
        private static readonly DependencyProperty RequiredErrMessageProperty = DependencyProperty.Register("RequiredErrMessage", typeof(string), typeof(ValidatorAloneBase), new PropertyMetadata("This field is required."));
        private static readonly DependencyProperty ShadowTextProperty = DependencyProperty.Register("ShadowText", typeof(string), typeof(ValidatorAloneBase));
        private static readonly DependencyProperty ValidataWhenKeyUpProperty = DependencyProperty.Register("ValidataWhenKeyUp", typeof(bool), typeof(ValidatorAloneBase));
        private static readonly DependencyProperty ValidationTypeProperty = DependencyProperty.Register("ValidationType", typeof(ValidationType), typeof(ValidatorAloneBase));

        private bool _loaded;
        private Brush _origBorder;
        private object _origTooltip;
        private bool _requiredValidate;

        #endregion Fields

        #region Properties Public

        /// <summary>
        /// 获取值
        /// </summary>
        public Func<string> GetTextValue;

        /// <summary>
        /// 获取或设置被验证的控件。
        /// </summary>
        public FrameworkElement ElementToValidate
        {
            set { SetValue(ElementToValidateProperty, value); }
            get { return (FrameworkElement)GetValue(ElementToValidateProperty); }
        }

        /// <summary>
        /// 获取或设置错误信息。
        /// </summary>
        public string ErrorMessage
        {
            set { SetValue(ErrorMessageProperty, value); }
            get { return (string)GetValue(ErrorMessageProperty); }
        }

        /// <summary>
        /// 获取或设置提示控件。
        /// </summary>
        public IIndicator Indicator
        {
            set { SetValue(IndicatorProperty, value); }
            get { return (IIndicator)GetValue(IndicatorProperty); }
        }

        /// <summary>
        /// 获取或设置无效状态的边框。
        /// </summary>
        public Brush InvalidBorder
        {
            set { SetValue(InvalidBorderProperty, value); }
            get { return (Brush)GetValue(InvalidBorderProperty); }
        }

        /// <summary>
        /// 获取或设置是否可以为空。
        /// </summary>
        public bool IsRequired
        {
            set { SetValue(IsRequiredProperty, value); }
            get { return (bool)GetValue(IsRequiredProperty); }
        }

        /// <summary>
        /// 获取或设置验证时是否去除两端的空格。
        /// </summary>
        public bool IsTrim
        {
            set { SetValue(IsTrimProperty, value); }
            get { return (bool)GetValue(IsTrimProperty); }
        }

        /// <summary>
        /// 获取或设置是否有效。
        /// </summary>
        public bool IsValid
        {
            set { SetValue(IsValidProperty, value); }
            get { return (bool)GetValue(IsValidProperty); }
        }

        /// <summary>
        /// ValidatorManager通过该属性获取实例。
        /// </summary>
        public string Key
        {
            set { SetValue(KeyProperty, value); }
            get { return (string)GetValue(KeyProperty); }
        }

        /// <summary>
        /// 获取或设置空验证的错误信息，默认为"This field is required."。
        /// </summary>
        public string RequiredErrMessage
        {
            set
            {
                SetValue(RequiredErrMessageProperty, value);
            }
            get { return (string)GetValue(RequiredErrMessageProperty); }
        }

        /// <summary>
        /// 阴影文字
        /// </summary>
        public string ShadowText
        {
            set { SetValue(ShadowTextProperty, value); }
            get { return (string)GetValue(ShadowTextProperty); }
        }

        /// <summary>
        /// 是否实时验证。
        /// </summary>
        public bool ValidataWhenKeyUp
        {
            set { SetValue(ValidataWhenKeyUpProperty, value); }
            get { return (bool)GetValue(ValidataWhenKeyUpProperty); }
        }

        /// <summary>
        /// 获取或设置验证类型。
        /// </summary>
        public ValidationType ValidationType
        {
            set { SetValue(ValidationTypeProperty, value); }
            get { return (ValidationType)GetValue(ValidationTypeProperty); }
        }

        #endregion Properties Public

        #region Methods Public

        /// <summary>
        /// 执行验证路由。
        /// </summary>
        public virtual void ActivateValidationRoutine()
        {
            ElementToValidate.LostFocus += ElementToValidate_LostFocus;
        }

        /// <summary>
        /// 设置无效状态的样式。
        /// </summary>
        public virtual void SetInvalidStyle()
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                //causing a onownermouseleave error currently...
                ElementToValidate.ClearValue(ToolTipService.ToolTipProperty);
                if (!_requiredValidate)
                    Indicator.SetErrMessage(RequiredErrMessage);
                else
                    Indicator.SetErrMessage(ErrorMessage);
                SetToolTip(ToolTipService.GetToolTip((DependencyObject)Indicator));
            }

            Thickness? borderThickness;
            Brush backgroundBrush;
            Brush borderBrush;
            object toolTip;
            Helper.GetStyle(ElementToValidate, out borderBrush, out borderThickness, out backgroundBrush, out toolTip);

            if (borderBrush != InvalidBorder)
                _origBorder = borderBrush;

            if (InvalidBorder != null)
                Helper.SetSytle(ElementToValidate, InvalidBorder, null, null);
        }

        /// <summary>
        /// 设置有效状态的样式。
        /// </summary>
        public virtual void SetValidStyle()
        {
            if (_origTooltip != null)
                SetToolTip(_origTooltip);
            else
                SetToolTip(null);

            Helper.SetSytle(ElementToValidate, _origBorder, null, null);
        }

        public void Dispose()
        {
            _origBorder = null;
            _origTooltip = null;
            ElementToValidate.LostFocus -= ElementToValidate_LostFocus;
            ElementToValidate.KeyUp -= ElementToValidate_KeyUp;
            ElementToValidate = null;
            Indicator = null;
            InvalidBorder = null;
        }

        /// <summary>
        /// 初始化。
        /// </summary>
        /// <param name="element">被验证的控件。</param>
        public void Initialize(FrameworkElement element)
        {
            ElementToValidate = element;
            element_Loaded();
            if (ValidataWhenKeyUp)
                ElementToValidate.KeyUp += ElementToValidate_KeyUp;
        }

        /// <summary>
        /// 验证。
        /// </summary>
        /// <returns>验证通过返回true，否则返回false。</returns>
        public bool Validate()
        {
            bool newIsValid;
            HandleTrim();
            //jiyi 2011-11-7 17:18:01 添加外部支持获取值方法
            string value = GetTextValue != null ? GetTextValue() : Helper.GetText(ElementToValidate);
            bool validateControl = ValidateControl(value);
            _requiredValidate = RequiredValidate();
            if (validateControl && _requiredValidate)
            {
                newIsValid = true;
            }
            else
            {
                newIsValid = false;
            }

            IsValid = newIsValid;
            if (IsValid)
            {
                SetValidStyle();
            }
            else
            {
                SetInvalidStyle();
            }
            return IsValid;
        }

        #endregion Methods Public

        #region Methods Protected

        /// <summary>
        /// 丢失焦点事件。
        /// </summary>
        protected virtual void ElementToValidate_LostFocus(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke((MethodInvoker)(() => Validate()));
        }

        /// <summary>
        /// 执行验证。
        /// </summary>
        /// <param name="text">待验证的文本。</param>
        /// <returns>成功返回true，失败返回false。</returns>
        protected abstract bool ValidateControl(string text);

        /// <summary>
        /// 按键弹起事件。
        /// </summary>
        protected void ElementToValidate_KeyUp(object sender, KeyEventArgs e)
        {
            Dispatcher.BeginInvoke((MethodInvoker)(() => Validate()));
        }

        #endregion Methods Protected

        #region Methods Private

        /// <summary>
        /// 加载事件。
        /// </summary>
        private void element_Loaded()
        {
            if (!_loaded)
            {
                if (ValidationType == ValidationType.Validator)
                {
                    ActivateValidationRoutine();
                }

                InvalidBorder = new SolidColorBrush(Colors.Red);
                _origTooltip = Helper.GetToolTip(ElementToValidate);
                Indicator = new DefaultIndicator.DefaultIndicator();
                _loaded = true;
            }
        }

        /// <summary>
        /// 如果IsTrim为true，把两端的空格去除。
        /// </summary>
        private void HandleTrim()
        {
            if (IsTrim)
            {
                //jiyi 2011-11-7 17:18:01 添加外部支持获取值方法
                string text = GetTextValue != null ? GetTextValue() : Helper.GetText(ElementToValidate);
                text = text.Trim();
                Helper.SetText(ElementToValidate, text);
            }
        }

        /// <summary>
        /// 根据IsRequired属性验证是否为空。
        /// </summary>
        /// <returns>通过验证返回true，否则返回false。</returns>
        private bool RequiredValidate()
        {
            if (!IsRequired)
                return true;

            //jiyi 2011-11-7 17:18:01 添加外部支持获取值方法
            string text = GetTextValue != null ? GetTextValue() : Helper.GetText(ElementToValidate);
            return !string.IsNullOrEmpty( text ) && !text.All(c => c.Equals(' ')) && !text.Equals( ShadowText );
        }

        /// <summary>
        /// Dispatcher方式设置ToolTip。
        /// </summary>
        private void SetToolTip(object toolTip)
        {
            ToolTipService.SetPlacement(ElementToValidate, PlacementMode.Right);
            ToolTipService.SetToolTip(ElementToValidate, toolTip);
        }

        #endregion Methods Private
    }
}