using DM.Web.SL.Controls.Validator.BLL;
using System;
using System.Text.RegularExpressions;
using System.Windows;

namespace DM.Web.SL.Controls.Validator
{
    /// <summary>
    /// 正则表达式验证器。
    /// </summary>
    public class RegexValidator : ValidatorBase
    {
        #region Fields

        /// <summary>
        /// ExpressionProperty。
        /// </summary>
        public static readonly DependencyProperty ExpressionProperty = 
            DependencyProperty.Register("Expression", typeof (string), typeof (RegexValidator),
                                        new PropertyMetadata(ExpressionPropertyChangedCallback));

        /// <summary>
        /// RegExpressionProperty。
        /// </summary>
        public static readonly DependencyProperty RegExpressionProperty = 
            DependencyProperty.Register("RegExpression", typeof (Regex), typeof (RegexValidator),
                                        new PropertyMetadata(null));
        /// <summary>
        /// ExpressionOther
        /// </summary>
        public static readonly DependencyProperty ExpressionOtherProperty =
            DependencyProperty.Register("ExpressionOther",typeof(string),typeof(RegexValidator),new PropertyMetadata(ExpressionOtherPropertyChangedCallback));
        /// <summary>
        /// RegExpressionOther
        /// </summary>
        public static readonly DependencyProperty RegExpressionOtherProperty =
            DependencyProperty.Register("RegExpressionOther", typeof(Regex), typeof(RegexValidator),
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
        /// 获取或设置正则表达式。（多个验证）
        /// </summary>
        public string ExpressionOther
        {
            get { return GetValue(ExpressionOtherProperty).ToString(); }
            set { SetValue(ExpressionOtherProperty,value); }
        }

        /// <summary>
        /// 获取或设置正则表达式。
        /// </summary>
        public Regex RegExpression
        {
            get { return (Regex) GetValue(RegExpressionProperty); }
            set { SetValue(RegExpressionProperty, value); }
        }

        /// <summary>
        /// 获取或设置正则表达式。(多个验证)
        /// </summary>
        public Regex RegExpressionOther
        {
            get { return (Regex)GetValue(RegExpressionOtherProperty); }
            set { SetValue(RegExpressionOtherProperty, value); }
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
        /// 验证器属性改变回调事件。
        /// </summary>
        public static void ExpressionOtherPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                d.SetValue(RegExpressionOtherProperty,
                           new Regex(e.NewValue.ToString()));
            }
            catch
            {
                d.SetValue(RegExpressionOtherProperty, null);
            }
        }

        /// <summary>
        /// 执行验证。
        /// </summary>
        /// <returns>成功返回true，失败返回false。</returns>
        protected override bool ValidateControl(string text)
        {
            bool result = true;
            if (RegExpression != null)
            {
                if (String.IsNullOrEmpty(text)) return result;
                result= RegExpression.Match(text).Success;
                if (!result && RegExpressionOther!=null) result = RegExpressionOther.Match(text).Success;
            }

            return result;
        }

        #endregion Methods

        #region 只在为异步时才调用

        public override void AsynValidateControl(string text)
        {
        }

        #endregion
    }
}