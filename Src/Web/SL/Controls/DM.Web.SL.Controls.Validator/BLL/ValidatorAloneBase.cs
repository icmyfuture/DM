using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using DM.Web.SL.Common.Utility;
using DM.Web.SL.Controls.Validator.DefaultIndicator;

namespace DM.Web.SL.Controls.Validator.BLL
{
    /// <summary>
    /// 表示一个验证器的基类。
    /// </summary>
    public abstract class ValidatorAloneBase : DependencyObject, IDisposable
    {
        #region 构造

        /// <summary>
        /// 构造。
        /// </summary>
        protected ValidatorAloneBase()
        {
            IsRequired = false;
            IsValid = true;
            ValidationType = ValidationType.Validator;
        }

        #endregion 构造

        #region 字段

        private bool _loaded;
        private Brush _origBorder;
        private object _origTooltip;
        private string _requiredErrMessage = LanguageHelper.GetDictionary("DM.Common.Controls", "fieldRequired", "This field is required."); // "This field is required.";
        private bool _requiredValidate;

        /// <summary>
        /// 获取值
        /// </summary>
        public Func<string> GetTextValue;

        #endregion 字段

        #region 公共属性

        /// <summary>
        /// 获取或设置被验证的控件。
        /// </summary>
        public FrameworkElement ElementToValidate { get; set; }


        #region ErrorMessage

        public static readonly DependencyProperty ErrorMessageProperty =
            DependencyProperty.Register("ErrorMessage", typeof (string), typeof (ValidatorAloneBase),
                                        new PropertyMetadata(string.Empty));

        /// <summary>
        /// 获取或设置错误信息。
        /// </summary>
        public string ErrorMessage
        {
            get { return GetValue(ErrorMessageProperty).ToString(); }
            set { SetValue(ErrorMessageProperty, value); }
        }

        #endregion

        /// <summary>
        /// 获取或设置提示控件。
        /// </summary>
        public IIndicator Indicator { get; set; }

        /// <summary>
        /// 获取或设置无效状态的边框。
        /// </summary>
        public SolidColorBrush InvalidBorder { get; set; }

        /// <summary>
        /// 获取或设置是否可以为空。
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// 获取或设置验证时是否去除两端的空格。
        /// </summary>
        public bool IsTrim { get; set; }

        #region IsValid

        /// <summary>
        /// 获取或设置是否有效。
        /// </summary>
        public static readonly DependencyProperty IsValidProperty =
            DependencyProperty.Register("IsValid", typeof (bool), typeof (ValidatorAloneBase),
                                        new PropertyMetadata(true));

        /// <summary>
        /// 获取或设置是否有效。
        /// </summary>
        public bool IsValid
        {
            get { return (bool) GetValue(IsValidProperty); }
            set { SetValue(IsValidProperty, value); }
        }

        #endregion

        /// <summary>
        /// ValidatorManager通过该属性获取实例。
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 获取或设置空验证的错误信息，默认为"This field is required."。
        /// </summary>
        public string RequiredErrMessage
        {
            get { return _requiredErrMessage; }
            set { _requiredErrMessage = value; }
        }

        /// <summary>
        /// 是否实时验证。
        /// </summary>
        public bool ValidataWhenKeyUp { get; set; }

        /// <summary>
        /// 获取或设置验证类型。
        /// </summary>
        public ValidationType ValidationType { get; set; }

        /// <summary>
        /// 阴影文字
        /// </summary>
        public string ShadowText { get; set; }

        #endregion 公共属性

        #region 公共方法

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
                SetToolTip(ToolTipService.GetToolTip((DependencyObject) Indicator));
            }

            Thickness? borderThickness;
            Brush backgroundBrush;
            SolidColorBrush borderBrush;
            object toolTip;
            Helper.GetStyle(ElementToValidate, out borderBrush, out borderThickness, out backgroundBrush, out toolTip);

            if (ColorToHexString(borderBrush.Color) != ColorToHexString(InvalidBorder.Color))
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
                newIsValid = true;
            else
                newIsValid = false;

            IsValid = newIsValid;
            if (IsValid)
                SetValidStyle();
            else
                SetInvalidStyle();
            return IsValid;
        }

        #endregion 公共方法

        #region 保护方法

        /// <summary>
        /// 丢失焦点事件。
        /// </summary>
        protected virtual void ElementToValidate_LostFocus(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(() => Validate());
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
            Dispatcher.BeginInvoke(() => Validate());
        }

        #endregion 保护方法

        #region 私有方法

        /// <summary>
        /// 加载事件。
        /// </summary>
        private void element_Loaded()
        {
            if (!_loaded)
            {
                if (ValidationType == ValidationType.Validator)
                    ActivateValidationRoutine();

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
                //string text = Helper.GetText(ElementToValidate);
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

            string text = GetTextValue != null ? GetTextValue() : Helper.GetText(ElementToValidate);
            return !string.IsNullOrEmpty(text) && !text.Equals(this.ShadowText);
        }

        /// <summary>
        /// Dispatcher方式设置ToolTip。
        /// </summary>
        private void SetToolTip(object toolTip)
        {
            ToolTipService.SetPlacement(ElementToValidate, PlacementMode.Right);
            ToolTipService.SetToolTip(ElementToValidate, toolTip);
        }

        /// <summary>
        /// 转换颜色为Hex字符串
        /// </summary>
        /// <param name="color"></param>    
        /// <returns></returns>
        private string ColorToHexString( Color color )
        {
            return String.Format("#{0}{1}{2}",
                                 color.R.ToString("x2"),
                                 color.G.ToString("x2"),
                                 color.B.ToString("x2"));
        }

        #endregion 私有方法
    }
}