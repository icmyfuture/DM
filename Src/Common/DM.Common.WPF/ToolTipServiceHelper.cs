using System.Windows;
using System.Windows.Controls;

namespace DM.Common.WPF
{
    /// <summary>
    /// 操作类
    /// </summary>
    public class ToolTipServiceHelper
    {
        /// <summary>
        /// 单一实例
        /// </summary>
        public static readonly ToolTipServiceHelper Instance = new ToolTipServiceHelper();

        /// <summary>
        /// 构造函数
        /// </summary>
        private ToolTipServiceHelper()
        {
        }

        #region 属性

        #endregion

        #region 公共方法
        /// <summary>
        /// 设置ToolTip样式
        /// </summary>
        /// <param name="element">元素</param>
        /// <param name="value">值</param>
        public static void SetToolTip( DependencyObject element, object value )
        {
            ToolTipService.SetToolTip(element, null);


            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return;
            }

            if (Application.Current.Resources.Contains("BaseToolTip"))
            {
                var model = new ToolTipBind { ToolTipText = value.ToString() };
                var toolTipStyle = Application.Current.Resources["BaseToolTip"] as Style;
                ToolTipService.SetToolTip(element, new ToolTip { Style = toolTipStyle, DataContext = model });
            }
            else
            {
                ToolTipService.SetToolTip(element, value);
            }

        }
        #endregion

        #region 私有方法

        #endregion
    }

    /// <summary>
    /// 绑定用的类
    /// </summary>
    public class ToolTipBind
    {
        /// <summary>
        /// 
        /// </summary>
        public string ToolTipText { get; set; }
    }
}
