using System.Windows;
using DM.Web.SL.Controls.Validator.BLL;

namespace DM.Web.SL.Controls.Validator
{

    /// <summary>
    /// 通过自定义事件 执行验证
    /// </summary>
    public class ActionValidator : ValidatorBase
    {
        readonly AsynValidatorResult result = new AsynValidatorResult();
        #region Fields

        /// <summary>
        /// 错误提示
        /// </summary>
        public static readonly DependencyProperty ValidatorErrorMessageProperty =
          DependencyProperty.Register( "ValidatorErrorMessage", typeof( string ), typeof( ActionValidator ),
                                      new PropertyMetadata( string.Empty ) );
        #endregion

        #region Property

        /// <summary>
        /// 验证事件。
        /// </summary>
        public event RoutedEventHandler ImportValidator;

        /// <summary>
        /// 获取或设置错误信息。
        /// </summary>
        public string ValidatorErrorMessage
        {
            get
            {
                return GetValue( ValidatorErrorMessageProperty ).ToString();
            }
            set
            {
                SetValue( ValidatorErrorMessageProperty, value );
            }
        }
        #endregion

        protected override bool ValidateControl( string text )
        {
            if ( string.IsNullOrEmpty( text ) )
                return true;
            if ( null != result && !result.Status )
            {
                ErrorMessage = ValidatorErrorMessage;
                return false;
            }
            return true;
        }

        public override void AsynValidateControl(string text)
        {
            if ( string.IsNullOrEmpty( text ) )
            {
                Validate();
                return;
            }
            result.ValidatorText = text;
            result.GetResult = (sender, args) =>
                               {
                                   var backResult = (bool) sender;
                                   //result = new AsynValidatorResult();
                                   result.Status = backResult == true ? true : false;
                                   Validate();
                               };
            if ( null != ImportValidator )
                ImportValidator( this, result);
            
        }
    }
}
