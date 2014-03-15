using System.Windows;

namespace DM.Client.WPF.Controls.Validators.BLL
{
    /// <summary>
    /// 验证器容器，可以支持多个验证器。
    /// </summary>
    public abstract class ValidatorAloneService : DependencyObject
    {
        #region Fields

        /// <summary>
        /// ValidatorProperty。
        /// </summary>
        public static readonly DependencyProperty ValidatorProperty =
            DependencyProperty.RegisterAttached("Validator", typeof(ValidatorAloneBase), typeof(ValidatorAloneService),
                                                new PropertyMetadata(ValidatorsPropertyChangedCallback));

        #endregion Fields

        #region Methods

        /// <summary>
        /// 获取验证器。
        /// </summary>
        public static ValidatorAloneBase GetValidator(DependencyObject obj)
        {
            return (ValidatorAloneBase)obj.GetValue(ValidatorProperty);
        }

        /// <summary>
        /// 设置验证器。
        /// </summary>
        public static void SetValidator(DependencyObject obj, ValidatorAloneBase value)
        {
            obj.SetValue(ValidatorProperty, value);
        }

        /// <summary>
        /// 验证器属性改变回调事件。
        /// </summary>
        private static void ValidatorsPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ValidatorAloneBase v = e.NewValue as ValidatorAloneBase;
            v.Initialize(d as FrameworkElement);
        }

        #endregion Methods
    }
}