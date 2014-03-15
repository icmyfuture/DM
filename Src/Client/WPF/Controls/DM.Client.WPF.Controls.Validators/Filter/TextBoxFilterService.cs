using System.Windows;
using System.Windows.Input;
using DM.Client.WPF.Controls.Validators.BLL;

namespace DM.Client.WPF.Controls.Validators.Filter
{
    /// <summary>
    /// 提供文本框过滤功能。
    /// </summary>
    public static class TextBoxFilterService
    {
        #region Fields

        /// <summary>
        /// FilterProperty
        /// </summary>
        public static readonly DependencyProperty FilterProperty = 
            DependencyProperty.RegisterAttached("Filter", typeof (TextBoxFilterType), typeof (TextBoxFilterService),
                                                new PropertyMetadata(OnFilterChanged));

        #endregion Fields

        #region Methods Public

        /// <summary>
        /// 获取过滤的类型。
        /// </summary>
        public static TextBoxFilterType GetFilter(DependencyObject d)
        {
            return (TextBoxFilterType) d.GetValue(FilterProperty);
        }

        /// <summary>
        /// 设置过滤的类型。
        /// </summary>
        public static void SetFilter(DependencyObject d, TextBoxFilterType value)
        {
            d.SetValue(FilterProperty, value);
        }

        #endregion Methods Public

        #region Methods Private

        /// <summary>
        /// 在指定的文本框里键入的是否是有效的AlphaKey。
        /// </summary>
        /// <returns>是返回true，否则返回false。</returns>
        private static bool IsValidAlphaKey(Key key)
        {
            if (Key.A <= key && key <= Key.Z)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 在指定的文本框里键入的是否是有效的Decmial键。
        /// </summary>
        /// <param name="text">指定的内容。</param>
        /// <param name="key">被检测的键。</param>
        /// <param name="platformKeyCode">platform key code。</param>
        /// <param name="negativeAllowed">是否允许负数。</param>
        /// <returns>是返回true，否则返回false。</returns>
        private static bool IsValidDecmialKey(string text, Key key, int platformKeyCode, bool negativeAllowed)
        {
            if (IsValidIntegerKey(text, key, platformKeyCode, negativeAllowed))
            {
                return true;
            }
            if (key == Key.Decimal || (platformKeyCode == 190))
            {
                return !text.Contains(".");
            }
            return false;
        }

        /// <summary>
        /// 在指定的文本框里键入的是否是有效的整数键。
        /// </summary>
        /// <param name="text">内容。</param>
        /// <param name="key">被检测的键。</param>
        /// <param name="platformKeyCode">platform key code。</param>
        /// <param name="negativeAllowed">是否允许负数。</param>
        /// <returns>是返回true，否则返回false。</returns>
        private static bool IsValidIntegerKey(string text, Key key, int platformKeyCode, bool negativeAllowed)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Shift) != 0)
            {
                return false;
            }
            if (Key.D0 <= key && key <= Key.D9)
            {
                return true;
            }
            if (Key.NumPad0 <= key && key <= Key.NumPad9)
            {
                return true;
            }
            if (negativeAllowed && (key == Key.Subtract || (platformKeyCode == 189)))
            {
                return 0 == text.Length;
            }
            return false;
        }

        /// <summary>
        /// 过滤的类型改变事件。
        /// </summary>
        private static void OnFilterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement elementToValidate = d as FrameworkElement;
            if (elementToValidate != null)
            {
                if (TextBoxFilterType.None != (TextBoxFilterType) e.OldValue)
                    elementToValidate.KeyDown -= textBox_KeyDown;
                if (TextBoxFilterType.None != (TextBoxFilterType) e.NewValue)
                    elementToValidate.KeyDown += textBox_KeyDown;
            }
        }

        /// <summary>
        /// 文本框按键按下事件。
        /// </summary>
        private static void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            // bypass other keys!
            if (FilterHelper.IsValidOtherKey(e.Key))
                return;
            //
            TextBoxFilterType filterType = GetFilter((DependencyObject) sender);

            FrameworkElement textBox = sender as FrameworkElement;
            if (textBox == null)
                textBox = e.OriginalSource as FrameworkElement;

            string text = Helper.GetText(textBox);
            switch (filterType)
            {
                case TextBoxFilterType.PositiveInteger:
                    e.Handled = !IsValidIntegerKey(text, e.Key, 0, false);
                    break;
                case TextBoxFilterType.Integer:
                    e.Handled = !IsValidIntegerKey(text, e.Key, 0, true);
                    break;
                case TextBoxFilterType.PositiveDecimal:
                    e.Handled = !IsValidDecmialKey(text, e.Key, 0, false);
                    break;
                case TextBoxFilterType.Decimal:
                    e.Handled = !IsValidDecmialKey(text, e.Key, 0, true);
                    break;
                case TextBoxFilterType.Alpha:
                    e.Handled = !IsValidAlphaKey(e.Key);
                    break;
            }
        }

        #endregion Methods Private
    }
}