using DM.Web.SL.Controls.Validator.BLL;
using System.Windows;

namespace DM.Web.SL.Controls.Validator
{
    /// <summary>
    /// 长度验证器。
    /// </summary>
    public class LengthValidator : ValidatorBase
    {
        #region Fields

        /// <summary>
        /// MaxLengthProperty。
        /// </summary>
        public static readonly DependencyProperty MaxLengthProperty =
            DependencyProperty.Register("MaxLength", typeof(double), typeof(LengthValidator),
                                        new PropertyMetadata(double.MaxValue));

        /// <summary>
        /// MinLengthProperty。
        /// </summary>
        public static readonly DependencyProperty MinLengthProperty =
            DependencyProperty.Register("MinLength", typeof(double), typeof(LengthValidator),
                                        new PropertyMetadata(double.MinValue));

        #endregion Fields

        #region Properties

        /// <summary>
        /// 获取或设置最大长度。
        /// </summary>
        public double MaxLength
        {
            get { return (double)GetValue(MaxLengthProperty); }
            set { SetValue(MaxLengthProperty, value); }
        }

        /// <summary>
        /// 获取或设置最小长度。
        /// </summary>
        public double MinLength
        {
            get { return (double)GetValue(MinLengthProperty); }
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

        #region 只在为异步时才调用

        public override void AsynValidateControl(string text)
        {
        }

        #endregion
    }
}