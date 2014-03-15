using System.Windows;
using DM.Web.SL.Controls.Validator.BLL;

namespace DM.Web.SL.Controls.Validator
{
    public class SpeCharactersValidator : ValidatorBase
    {
        /// <summary>
        /// 时间格式错误提示
        /// </summary>
        public static readonly DependencyProperty SpeCharactersErrorMessageProperty =
          DependencyProperty.Register( "SpeCharactersErrorMessage", typeof( string ), typeof( SpeCharactersValidator ),
                                      new PropertyMetadata( string.Empty ) );

        /// <summary>
        /// 获取或设置错误信息。
        /// </summary>
        public string SpeCharactersErrorMessage
        {
            get
            {
                return GetValue( SpeCharactersErrorMessageProperty ).ToString();
            }
            set
            {
                SetValue( SpeCharactersErrorMessageProperty, value );
            }
        }

        #region Methods

        /// <summary>
        /// 执行验证。
        /// </summary>
        /// <param name="text">待验证的文本。</param>
        /// <returns>成功返回true，失败返回false。</returns>
        protected override bool ValidateControl( string text )
        {
            return !text.Contains( "-" );
        }
        #endregion

        public override void AsynValidateControl( string text )
        {
            //异步验证时才调用
        }
    }
}
