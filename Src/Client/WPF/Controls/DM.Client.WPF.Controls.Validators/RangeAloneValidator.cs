using DM.Client.WPF.Controls.Validators.BLL;
using DM.Client.WPF.Controls.Validators.Filter;
using System;
using System.Windows;

namespace DM.Client.WPF.Controls.Validators
{
    /// <summary>
    /// 数值范围验证器。
    /// </summary>
    public class RangeAloneValidator : ValidatorAloneBase
    {
        #region Fields

        /// <summary>
        /// FilterProperty。
        /// </summary>
        public static readonly DependencyProperty FilterProperty =
            DependencyProperty.Register("Filter", typeof(TextBoxFilterType), typeof(RangeAloneValidator),
                                        new PropertyMetadata(TextBoxFilterType.Decimal));

        /// <summary>
        /// MaxProperty。
        /// </summary>
        public static readonly DependencyProperty MaxProperty = 
            DependencyProperty.Register("Max", typeof (double), typeof (RangeAloneValidator),
                                        new PropertyMetadata(double.MaxValue));

        /// <summary>
        /// MinProperty。
        /// </summary>
        public static readonly DependencyProperty MinProperty = 
            DependencyProperty.Register("Min", typeof (double), typeof (RangeAloneValidator),
                                        new PropertyMetadata(double.MinValue));

        ///// <summary>
        ///// ShadowText。
        ///// </summary>
        //public static readonly DependencyProperty ShadowTextProperty =
        //    DependencyProperty.Register( "ShadowText", typeof( string ), typeof( RangeAloneValidator ),
        //                                new PropertyMetadata( string.Empty ) );

        #endregion Fields

        #region Properties

        /// <summary>
        /// 获取或设置过滤的类型。
        /// </summary>
        public TextBoxFilterType Filter
        {
            get { return (TextBoxFilterType) GetValue(FilterProperty); }
            set { SetValue(FilterProperty, value); }
        }

        /// <summary>
        /// 获取或设置最大值。
        /// </summary>
        public double Max
        {
            get { return (double) GetValue(MaxProperty); }
            set { SetValue(MaxProperty, value); }
        }

        /// <summary>
        /// 获取或设置最小值。
        /// </summary>
        public double Min
        {
            get { return (double) GetValue(MinProperty); }
            set { SetValue(MinProperty, value); }
        }

        //public string ShadowText
        //{
        //    get { return (string) GetValue(ShadowTextProperty); }
        //    set{SetValue(ShadowTextProperty,value);}
        //}
        #endregion Properties

        #region Methods

        /// <summary>
        /// 执行验证路由。
        /// </summary>
        public override void ActivateValidationRoutine()
        {
            base.ActivateValidationRoutine();
            TextBoxFilterService.SetFilter(ElementToValidate, Filter);
        }

        /// <summary>
        /// 执行验证。
        /// </summary>
        /// <param name="text">待验证的文本。</param>
        /// <returns>成功返回true，失败返回false。</returns>
        protected override bool ValidateControl(string text)
        {
            if (String.IsNullOrEmpty(text)||text.Equals(this.ShadowText)) 
                return true;

            double d;
            if (!double.TryParse(text, out d))
                return false;

            if (text.IndexOf('.') != -1)
                return false;
            return d >= Min && d <= Max;
        }

        #endregion Methods
    }
}