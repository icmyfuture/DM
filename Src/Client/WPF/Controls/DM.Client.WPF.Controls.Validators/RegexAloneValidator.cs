using DM.Client.WPF.Controls.Validators.BLL;
using System;
using System.Text.RegularExpressions;
using System.Windows;

namespace DM.Client.WPF.Controls.Validators
{
    /// <summary>
    /// 正则表达式验证器。
    /// </summary>
    public class RegexAloneValidator : ValidatorAloneBase
    {
        #region Fields

        /// <summary>
        /// ExpressionProperty。
        /// </summary>
        public static readonly DependencyProperty ExpressionProperty =
            DependencyProperty.Register("Expression", typeof(string), typeof(RegexAloneValidator),
                                        new PropertyMetadata(ExpressionPropertyChangedCallback));

        /// <summary>
        /// RegExpressionProperty。
        /// </summary>
        public static readonly DependencyProperty RegExpressionProperty =
            DependencyProperty.Register("RegExpression", typeof(Regex), typeof(RegexAloneValidator),
                                        new PropertyMetadata(null));

        #endregion Fields

        #region Properties

        /// <summary>
        /// 获取或设置正则表达式。
        /// </summary>
        public string Expression
        {
            get { return (string) GetValue(ExpressionProperty); }
            set { SetValue(ExpressionProperty, value); }
        }

        /// <summary>
        /// 获取或设置正则表达式。
        /// </summary>
        public Regex RegExpression
        {
            get { return (Regex) GetValue(RegExpressionProperty); }
            set { SetValue(RegExpressionProperty, value); }
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
            if (String.IsNullOrEmpty(text)) return true;
            return RegExpression.Match(text).Success;
        }

        #endregion Methods
    }
}