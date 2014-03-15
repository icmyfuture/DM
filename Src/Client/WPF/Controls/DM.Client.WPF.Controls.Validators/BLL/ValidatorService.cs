using System;
using System.Windows;

namespace DM.Client.WPF.Controls.Validators.BLL
{
    /// <summary>
    /// 验证器容器，可以支持多个验证器。
    /// </summary>
    public abstract class ValidatorService : DependencyObject
    {
        #region Fields

        /// <summary>
        /// ValidatorProperty。
        /// </summary>
        public static readonly DependencyProperty ValidatorProperty = 
            DependencyProperty.RegisterAttached("Validator", typeof(ValidatorBase), typeof(ValidatorService),
                                                new PropertyMetadata(ValidatorsPropertyChangedCallback));

        #endregion Fields

        #region Methods

        /// <summary>
        /// 获取验证器。
        /// </summary>
        public static ValidatorBase GetValidator(DependencyObject obj)
        {
            return (ValidatorBase)obj.GetValue(ValidatorProperty);
        }

        /// <summary>
        /// 设置验证器。
        /// </summary>
        public static void SetValidator(DependencyObject obj, ValidatorBase value)
        {
            obj.SetValue(ValidatorProperty, value);
        }

        /// <summary>
        /// 验证器属性改变回调事件。
        /// </summary>
        private static void ValidatorsPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ValidatorBase v = e.NewValue as ValidatorBase;
            if (String.IsNullOrEmpty(v.ManagerName))
            {
                //ManagerName是必须有值的。
                throw new Exception();
            }
            v.Initialize(d as FrameworkElement);
        }

        #endregion Methods
    }
}