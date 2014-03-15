using System;
using System.Text.RegularExpressions;
using System.Windows;
using DM.Web.SL.Controls.Validator.BLL;

namespace DM.Web.SL.Controls.Validator
{
    /// <summary>
    /// 正则表达式 + 长度验证
    /// </summary>
    public class RegexLengthAloneValidator : ValidatorAloneBase
    {
        #region Fields

        /// <summary>
        /// ExpressionProperty。
        /// </summary>
        public static readonly DependencyProperty ExpressionProperty =
            DependencyProperty.Register( "Expression", typeof( string ), typeof( RegexLengthAloneValidator ),
                                        new PropertyMetadata( ExpressionPropertyChangedCallback ) );

        /// <summary>
        /// RegExpressionProperty。
        /// </summary>
        public static readonly DependencyProperty RegExpressionProperty =
            DependencyProperty.Register( "RegExpression", typeof( Regex ), typeof( RegexLengthAloneValidator ),
                                        new PropertyMetadata( null ) );

        /// <summary>
        /// MaxLengthProperty。
        /// </summary>
        public static readonly DependencyProperty MaxLengthProperty =
            DependencyProperty.Register( "MaxLength", typeof( double ), typeof( RegexLengthAloneValidator ),
                                        new PropertyMetadata( double.MaxValue ) );

        /// <summary>
        /// MinLengthProperty。
        /// </summary>
        public static readonly DependencyProperty MinLengthProperty =
            DependencyProperty.Register( "MinLength", typeof( double ), typeof( RegexLengthAloneValidator ),
                                        new PropertyMetadata( double.MinValue ) );

        /// <summary>
        /// Regex ErrorMessage
        /// </summary>
        public static readonly DependencyProperty RegexErrorMessageProperty =
          DependencyProperty.Register( "RegexErrorMessage", typeof( string ), typeof( RegexLengthAloneValidator ),
                                      new PropertyMetadata( string.Empty ) );

        /// <summary>
        /// Length ErrorMessage
        /// </summary>
        public static readonly DependencyProperty LengthErrorMessageProperty =
          DependencyProperty.Register( "LengthErrorMessage", typeof( string ), typeof( RegexLengthAloneValidator ),
                                      new PropertyMetadata( string.Empty ) );
        #endregion Fields

        #region Properties

        /// <summary>
        /// 获取或设置正则表达式。
        /// </summary>
        public string Expression
        {
            get { return (string)GetValue( ExpressionProperty ); }
            set { SetValue( ExpressionProperty, value ); }
        }

        /// <summary>
        /// 获取或设置正则表达式。
        /// </summary>
        public Regex RegExpression
        {
            get { return (Regex)GetValue( RegExpressionProperty ); }
            set { SetValue( RegExpressionProperty, value ); }
        }

        /// <summary>
        /// 获取或设置最大长度。
        /// </summary>
        public double MaxLength
        {
            get { return (double)GetValue( MaxLengthProperty ); }
            set { SetValue( MaxLengthProperty, value ); }
        }

        /// <summary>
        /// 获取或设置最小长度。
        /// </summary>
        public double MinLength
        {
            get { return (double)GetValue( MinLengthProperty ); }
            set { SetValue( MinLengthProperty, value ); }
        }

        /// <summary>
        /// 获取或设置错误信息。
        /// </summary>
        public string RegexErrorMessage
        {
            get
            {
                return GetValue( RegexErrorMessageProperty ).ToString();
            }
            set
            {
                SetValue( RegexErrorMessageProperty, value );
            }
        }

        /// <summary>
        /// 获取或设置错误信息。
        /// </summary>
        public string LengthErrorMessage
        {
            get
            {
                return GetValue( LengthErrorMessageProperty ).ToString();
            }
            set
            {
                SetValue( LengthErrorMessageProperty, value );
            }
        } 

        #endregion Properties

        #region Methods

        /// <summary>
        /// 验证器属性改变回调事件。
        /// </summary>
        public static void ExpressionPropertyChangedCallback( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            try
            {
                d.SetValue( RegExpressionProperty,
                           new Regex( e.NewValue.ToString() ) );
            }
            catch
            {
                d.SetValue( RegExpressionProperty, null );
            }
        }

        /// <summary>
        /// 执行验证。
        /// </summary>
        /// <param name="text">待验证的文本。</param>
        /// <returns>成功返回true，失败返回false。</returns>
        protected override bool ValidateControl( string text )
        {
            if ( String.IsNullOrEmpty( text ) ) return true;
            bool isMatch = RegExpression.Match( text ).Success;
            if(!isMatch)
            {
                ErrorMessage = RegexErrorMessage;
                return false;
            }
            bool isLength = text.Length >= MinLength && text.Length <= MaxLength;
            if ( !isLength )
            {
                ErrorMessage = LengthErrorMessage;
                return false;
            }
            return true;
        }

        #endregion Methods
    }
}
