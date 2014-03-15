using DM.Client.WPF.Controls.Validators.BLL;

namespace DM.Client.WPF.Controls.Validators
{
    /// <summary>
    /// 是否为空验证器。
    /// </summary>
    public class RequiredValidator : ValidatorBase
    {
        #region Constructors

        /// <summary>
        /// 构造。
        /// </summary>
        public RequiredValidator()
        {
            IsRequired = true;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// 执行验证。
        /// </summary>
        /// <param name="text">待验证的文本。</param>
        /// <returns>成功返回true，失败返回false。</returns>
        protected override bool ValidateControl(string text)
        {
            return true;
        }

        #endregion Methods
    }
}