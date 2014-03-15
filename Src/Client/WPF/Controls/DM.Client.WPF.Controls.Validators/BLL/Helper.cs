using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DM.Client.WPF.Controls.Validators.BLL
{
    /// <summary>
    /// 通用功能的帮助类。
    /// </summary>
    public static class Helper
    {
        /// <summary>
        /// 设置待验证控件的样式。
        /// </summary>
        /// <param name="elementToValidate">待验证控件。</param>
        /// <param name="borderBrush">borderBrush。</param>
        /// <param name="borderThickness">borderThickness。</param>
        /// <param name="backgroundBrush">backgroundBrush。</param>
        public static void SetSytle(FrameworkElement elementToValidate, Brush borderBrush, Thickness? borderThickness, Brush backgroundBrush)
        {
            Control control = elementToValidate as Control;
            if (control != null)
            {
                if (backgroundBrush != null)
                    control.Background = backgroundBrush;

                if (borderBrush != null)
                    control.BorderBrush = borderBrush;

                if (borderThickness.HasValue)
                    control.BorderThickness = borderThickness.Value;
            }
        }

        /// <summary>
        /// 获取待验证的文本。
        /// </summary>
        /// <returns>返回获取待的文本。</returns>
        public static string GetText(FrameworkElement elementToValidate)
        {
            var tb = elementToValidate as TextBox;
            var pb = elementToValidate as PasswordBox;
            var cb = elementToValidate as ComboBox;

            if (tb != null)
                return tb.Text;
            if (pb != null)
                return pb.Password;
            if (cb != null)
            {
                string str = string.Empty;
                if (cb.SelectedItem != null)
                    str = cb.SelectedItem.ToString();
                return str;
            }
            return "";
        }

        /// <summary>
        /// 设置待验证控件的文本。
        /// </summary>
        /// <param name="elementToValidate">待验证的控件。</param>
        /// <param name="text">设置的文本。</param>
        public static void SetText(FrameworkElement elementToValidate, string text)
        {
            var tb = elementToValidate as TextBox;
            var pb = elementToValidate as PasswordBox;
            var cb = elementToValidate as ComboBox;

            if (tb != null)
            {
                var selectionStart = tb.SelectionStart;
                tb.Text = text;
                if (selectionStart > 1)
                    tb.SelectionStart = selectionStart;
            }
            else if (pb != null)
                pb.Password = text;
            else if (cb != null)
                cb.SelectedItem = text;
        }

        /// <summary>
        /// 获取待验证控件的样式。
        /// </summary>
        /// <param name="elementToValidate">待验证的控件。</param>
        /// <param name="borderBrush">borderBrush。</param>
        /// <param name="borderThickness">borderThickness。</param>
        /// <param name="backgroundBrush">backgroundBrush。</param>
        /// <param name="toolTip">toolTip。</param>
        public static void GetStyle(FrameworkElement elementToValidate, out Brush borderBrush, out Thickness? borderThickness,
                                    out Brush backgroundBrush, out object toolTip)
        {
            backgroundBrush = null;
            borderBrush = null;
            borderThickness = null;
            toolTip = null;
            Control control = elementToValidate as Control;
            if (control != null)
            {
                backgroundBrush = control.Background;
                borderBrush = control.BorderBrush;
                borderThickness = control.BorderThickness;
                toolTip = ToolTipService.GetToolTip(elementToValidate);
            }
        }

        /// <summary>
        /// 获取待验证控件的ToolTip。
        /// </summary>
        /// <param name="elementToValidate">待验证的控件。</param>
        /// <returns>返回获取的ToolTip。</returns>
        public static object GetToolTip(FrameworkElement elementToValidate)
        {
            object toolTip;
            Brush borderBrush;
            Thickness? borderThickness;
            Brush backgroundBrush;
            GetStyle(elementToValidate, out borderBrush, out borderThickness, out backgroundBrush, out toolTip);
            return toolTip;
        }
    }
}