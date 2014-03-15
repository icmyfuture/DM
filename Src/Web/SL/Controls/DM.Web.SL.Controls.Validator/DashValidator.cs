using DM.Web.SL.Controls.Validator.BLL;

namespace DM.Web.SL.Controls.Validator
{
    public class DashValidator : ValidatorBase
    {
        /// <summary>
        /// 执行验证。
        /// </summary>
        /// <param name="text">待验证的文本。</param>
        /// <returns>成功返回true，失败返回false。</returns>
        protected override bool ValidateControl( string text )
        {
            return !text.Contains("-");
        }

        public override void AsynValidateControl( string text )
        {
            
        }
    }
}
