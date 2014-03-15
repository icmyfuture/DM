using DM.Client.WPF.Controls.Validators.BLL;
using System;
using System.Windows;

namespace DM.Client.WPF.Controls.Validators
{
    /// <summary>
    /// 长度验证器。
    /// </summary>
    public class LengthAloneValidator : ValidatorAloneBase
    {
        #region Fields

        /// <summary>
        /// MaxLengthProperty。
        /// </summary>
        public static readonly DependencyProperty MaxLengthProperty =
            DependencyProperty.Register("MaxLength", typeof(int), typeof(LengthAloneValidator),
                                        new PropertyMetadata(Int32.MaxValue));

        /// <summary>
        /// MinLengthProperty。
        /// </summary>
        public static readonly DependencyProperty MinLengthProperty =
            DependencyProperty.Register("MinLength", typeof(int), typeof(LengthAloneValidator),
                                        new PropertyMetadata(Int32.MinValue));

        #endregion Fields

        #region Properties

        /// <summary>
        /// 获取或设置最大长度。
        /// </summary>
        public int MaxLength
        {
            get { return (int) GetValue(MaxLengthProperty); }
            set { SetValue(MaxLengthProperty, value); }
        }

        /// <summary>
        /// 获取或设置最小长度。
        /// </summary>
        public int MinLength
        {
            get { return (int) GetValue(MinLengthProperty); }
            set { SetValue(MinLengthProperty, value); }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 执行验证。
        /// </summary>
        /// <param name="text">待验证的文本。</param>
        /// <returns>成功返回true，失败返回false。</returns>
        protected override bool ValidateControl(string text)
        {
            return text.Length >= MinLength && text.Length <= MaxLength;
        }

        #endregion Methods
    }
}