using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DM.Web.SL.Controls.Validator.BLL
{
    /// <summary>
    /// 通用功能的帮助类。
    /// </summary>
    internal static class Helper
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
            var ab = elementToValidate as AutoCompleteBox;
            var db = elementToValidate as DatePicker;

            if (tb != null)
                return tb.Text;
            if (pb != null)
                return pb.Password;
            if (cb != null)
            {
                var item = (ComboBoxItem) cb.SelectedItem;
                if ( item == null )
                    return string.Empty;
                return item.Content.ToString();
            }
            if (ab != null)
                return ab.Text;
            if ( db != null )
                return db.SelectedDate.HasValue ? db.Text : string.Empty;
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
            var ab = elementToValidate as AutoCompleteBox;
            var db = elementToValidate as DatePicker;

            if (tb != null)
                tb.Text = text;
            else if (pb != null)
                pb.Password = text;
            else if (cb != null)
                cb.SelectedItem = text;
            else if (ab != null)
                ab.Text = text;
            else if ( db != null )
                db.Text = text;
        }

        /// <summary>
        /// 获取待验证控件的样式。
        /// </summary>
        /// <param name="elementToValidate">待验证的控件。</param>
        /// <param name="borderBrush">borderBrush。</param>
        /// <param name="borderThickness">borderThickness。</param>
        /// <param name="backgroundBrush">backgroundBrush。</param>
        /// <param name="toolTip">toolTip。</param>
        public static void GetStyle( FrameworkElement elementToValidate, out SolidColorBrush borderBrush, out Thickness? borderThickness,
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
                borderBrush = (SolidColorBrush) control.BorderBrush;
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
            SolidColorBrush borderBrush;
            Thickness? borderThickness;
            Brush backgroundBrush;
            GetStyle(elementToValidate, out borderBrush, out borderThickness, out backgroundBrush, out toolTip);
            return toolTip;
        }
    }
}