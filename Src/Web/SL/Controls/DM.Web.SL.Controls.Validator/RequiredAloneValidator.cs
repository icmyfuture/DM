using DM.Web.SL.Controls.Validator.BLL;

namespace DM.Web.SL.Controls.Validator
{
    /// <summary>
    /// 是否为空验证器。
    /// </summary>
    public class RequiredAloneValidator : ValidatorAloneBase
    {
        #region Constructors

        /// <summary>
        /// 构造。
        /// </summary>
        public RequiredAloneValidator()
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