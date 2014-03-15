using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using DM.Web.SL.Controls.Validator.BLL;

namespace DM.Web.SL.Controls.Validator
{
    ///<summary>
    /// 重复验证
    ///</summary>
    public class RepeatValidator : ValidatorBase
    {
        /// <summary>
        /// ItemsSource
        /// </summary>
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(List<string>), typeof(RepeatValidator),
                                        new PropertyMetadata(null));

        /// <summary>
        /// IgnoreCase
        /// </summary>
        public static readonly DependencyProperty IgnoreCaseProperty =
            DependencyProperty.Register("IgnoreCase", typeof(bool), typeof(RepeatValidator),
                                        new PropertyMetadata(false));

        /// <summary>
        /// 获取或设置需要判定重复字符串列表
        /// </summary>
        public List<string> ItemsSource
        {
            get { return (List<string>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        /// <summary>
        /// 是否区分大小写
        /// </summary>
        public bool IgnoreCase
        {
            get { return (bool)GetValue(IgnoreCaseProperty); }
            set { SetValue(IgnoreCaseProperty, value); }
        }

        protected override bool ValidateControl(string text)
        {
            if (String.IsNullOrEmpty(text)) return true;
            if (ItemsSource==null || ItemsSource.Count == 0) return true;
            if (IgnoreCase)
            {
                var count = ItemsSource.Where(x => x.ToLower() == text.ToLower()).ToList().Count;
                return count == 0;
            }
            else
            {
                var count = ItemsSource.Where(x => x == text).ToList().Count;
                return count == 0;
            }
            
        }

        #region 只在为异步时才调用

        public override void AsynValidateControl(string text)
        {
        }

        #endregion
    }
}
