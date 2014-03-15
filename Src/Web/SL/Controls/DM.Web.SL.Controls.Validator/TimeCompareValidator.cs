using System;
using System.Windows;
using System.Text.RegularExpressions;
using DM.Web.SL.Controls.Validator.BLL;

namespace DM.Web.SL.Controls.Validator
{
    /// <summary>
    /// 时间格式及起始时间对比
    /// </summary>
    public class TimeCompareValidator : ValidatorBase
    {
        #region Fields

        /// <summary>
        /// 正则表达式属性
        /// </summary>
        public static readonly DependencyProperty ExpressionProperty =
            DependencyProperty.Register("Expression", typeof(string), typeof(TimeCompareValidator),
                                        new PropertyMetadata(ExpressionPropertyChangedCallback));

        /// <summary>
        /// 正则表达式属性
        /// </summary>
        public static readonly DependencyProperty RegExpressionProperty =
            DependencyProperty.Register("RegExpression", typeof(Regex), typeof(TimeCompareValidator),
                                        new PropertyMetadata(null));

        /// <summary>
        /// 时间格式错误提示
        /// </summary>
        public static readonly DependencyProperty RegexErrorMessageProperty =
          DependencyProperty.Register("RegexErrorMessage", typeof(string), typeof(TimeCompareValidator),
                                      new PropertyMetadata(string.Empty));

        #endregion Fields

        #region Properties

        /// <summary>
        /// 获取或设置正则表达式。
        /// </summary>
        public string Expression
        {
            get { return (string)GetValue(ExpressionProperty); }
            set { SetValue(ExpressionProperty, value); }
        }

        /// <summary>
        /// 获取或设置正则表达式。
        /// </summary>
        public Regex RegExpression
        {
            get { return (Regex)GetValue(RegExpressionProperty); }
            set { SetValue(RegExpressionProperty, value); }
        }

        /// <summary>
        /// 获取或设置错误信息。
        /// </summary>
        public string RegexErrorMessage
        {
            get
            {
                return GetValue(RegexErrorMessageProperty).ToString();
            }
            set
            {
                SetValue(RegexErrorMessageProperty, value);
            }
        }


        #endregion Properties

        #region Methods

        /// <summary>
        /// 验证器属性改变回调事件。
        /// </summary>
        public static void ExpressionPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                d.SetValue(RegExpressionProperty,
                           new Regex(e.NewValue.ToString()));
            }
            catch
            {
                d.SetValue(RegExpressionProperty, null);
            }
        }

        /// <summary>
        /// 执行验证。
        /// </summary>
        /// <param name="text">待验证的文本。</param>
        /// <returns>成功返回true，失败返回false。</returns>
        protected override bool ValidateControl(string text)
        {
            if (String.IsNullOrEmpty(text)||text.ToUpper()=="YYYY-MM-DD")
                return true;
            var isMatch = RegExpression.Match(text).Success;
            if (!isMatch)
            {
                ErrorMessage = RegexErrorMessage;
                return false;
            }
            var strs = text.Split('-');
            var year = Convert.ToInt32(strs[0])%4;
            var month = strs[1];
            var day = strs[2];
            if(0!=year&&(month=="2"||month=="02")&&day=="29")
            {
                ErrorMessage = null;
                ErrorMessage = RegexErrorMessage;
                return false;
            }
            return true;
        }
        #endregion

        #region 只在为异步时才调用

        public override void AsynValidateControl(string text)
        {
        }

        #endregion
    }
}
