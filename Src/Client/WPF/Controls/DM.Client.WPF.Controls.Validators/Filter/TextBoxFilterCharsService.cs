using System.Windows.Controls;
using System.Windows;

namespace DM.Client.WPF.Controls.Validators.Filter
{
    /// <summary>
    /// 提供文本框过滤功能。
    /// </summary>
    public static class TextBoxFilterCharsService
    {
        #region Fields

        public static readonly DependencyProperty FilterCharsProperty =
            DependencyProperty.RegisterAttached("FilterChars", typeof(char[]), typeof(TextBoxFilterCharsService),
                                                new PropertyMetadata(OnFilterCharsChanged));

        #endregion Fields

        #region Methods Public

        public static char[] GetFilterChars(DependencyObject d)
        {
            return (char[])d.GetValue(FilterCharsProperty);
        }

        public static void SetFilterChars(DependencyObject d, char[] value)
        {
            d.SetValue(FilterCharsProperty, value);
        }

        #endregion Methods Public

        #region Methods Private

        private static void OnFilterCharsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TextBox textBox = d as TextBox;
            if (textBox != null)
            {
                if (e.OldValue != null)
                    textBox.TextChanged -= elementToValidate_TextChanged;
                if (e.NewValue != null)
                    textBox.TextChanged += elementToValidate_TextChanged;
            }
        }

        private static void elementToValidate_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            char[] filterChars = GetFilterChars((DependencyObject) sender);

            if (IsValidKey(textBox.Text, filterChars))
                return;

            TextChange textChange = null;
            foreach (TextChange tc in e.Changes)
            {
                textChange = tc;
                break;
            }

            if (textChange == null)
                return;

            if (textChange.AddedLength <= 0)
                return;

            textBox.Text = textBox.Text.Remove(textChange.Offset, textChange.AddedLength);
            textBox.Select(textChange.Offset, 0);
        }

        private static bool IsValidKey(string text, char[] filterChars)
        {
            foreach (var filterChar in filterChars)
            {
                if (text.IndexOf(filterChar) != -1)
                    return false;
            }
            return true;
        }

        #endregion Methods Private
    }
}