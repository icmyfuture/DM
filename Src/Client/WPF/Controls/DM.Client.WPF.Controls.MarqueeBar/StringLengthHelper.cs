using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DM.Client.WPF.Controls.MarqueeBar
{
    /// <summary>
    /// 获取字符串的显示长度.
    /// </summary>
    public static class StringLengthHelper
    {
        /// <summary>
        /// 根据文字字体和字号计算所需的显示空间
        /// </summary>
        /// <param name="measureText">文字内容</param>
        /// <param name="fontSize">字号 默认12</param>
        /// <param name="fontFamliy">字体名字 默认:楷体</param>
        /// <returns></returns>
        /// <exception cref="Exception">字体名字可能在目标计算机中不存在. 那么查找字体时会报错</exception>
        internal static double MeasureTextWidth(string measureText, double fontSize = 12, string fontFamliy = "kaiti")
        {
            if (fontFamliy == null) throw new ArgumentNullException("fontFamliy");
            var formattedText = new FormattedText(
               measureText,
               System.Globalization.CultureInfo.InvariantCulture,
               FlowDirection.LeftToRight,
               new Typeface(fontFamliy),
               fontSize,
               Brushes.Black
               );
            return formattedText.WidthIncludingTrailingWhitespace;
        }

        /// <summary>
        /// 计算TextBlock标签所需要的显示宽度
        /// </summary>
        /// <param name="textBlock">TextBlock 实例</param>
        /// <returns></returns>
        /// <exception cref="Exception">字体名字可能在目标计算机中不存在. 那么查找字体时会报错</exception>
        public static double MeasureTextWidth(TextBlock textBlock)
        {
            return MeasureTextWidth(textBlock.Text, textBlock.FontSize, textBlock.FontFamily.Source);
        }

        /// <summary>
        /// 根据文字字体和字号计算所需的显示空间
        /// </summary>
        /// <param name="measureText">文字内容</param>
        /// <param name="fontSize">字号 默认12</param>
        /// <returns></returns>
        /// <exception cref="Exception">字体名字可能在目标计算机中不存在. 那么查找字体时会报错</exception>
        public static double MeasureTextWidth(string measureText, double fontSize = 12)
        {
            var formattedText = new FormattedText(
               measureText,
               System.Globalization.CultureInfo.InvariantCulture,
               FlowDirection.LeftToRight,
               new Typeface("kaiti"),
               fontSize,
               Brushes.Black
               );
            return formattedText.WidthIncludingTrailingWhitespace;
        }

    }
}
