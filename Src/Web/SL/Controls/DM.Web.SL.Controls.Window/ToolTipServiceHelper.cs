using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DM.Web.SL.Controls.Window
{
    /// <summary>
    ///   操作类
    /// </summary>
    public class ToolTipServiceHelper
    {
        #region 单一实例

        /// <summary>
        ///   单一实例
        /// </summary>
        public static readonly ToolTipServiceHelper Instance = new ToolTipServiceHelper();

        #endregion

        #region  构造函数

        #endregion  构造函数

        #region 私有变量

        #endregion

        #region 属性

        #endregion

        #region 公共方法

        /// <summary>
        ///   设置ToolTip样式
        /// </summary>
        /// <param name = "element">元素</param>
        /// <param name = "value">值</param>
        public static void SetToolTip( DependencyObject element, object value )
        {
            //if ( Application.Current.Resources.Contains( "ToolTipStyle" ) )
            //{
            //    Style toolTipStyle = Application.Current.Resources["ToolTipStyle"] as Style;
            //    ToolTipService.SetToolTip( element, new ToolTip { Style = toolTipStyle, Content = value } );
            //}
            //else
            //{
            //    ToolTipService.SetToolTip( element, value );
            //}

            if (element != null && value != null)
            {
                BitmapCache bitmapCache = new BitmapCache
                                          {
                                              RenderAtScale = 1
                                          };
                if (Application.Current.Resources.Contains("ToolTipTextBlock"))
                {
                    Style toolTipStyle = Application.Current.Resources["ToolTipTextBlock"] as Style;
                    TextBlock toolTipTxt = new TextBlock
                                           {
                                               Style = toolTipStyle,
                                               Text = value.ToString(),
                                               CacheMode = bitmapCache
                                           };
                    ToolTipService.SetToolTip(element, new ToolTip
                                                       {
                                                           Content = toolTipTxt,
                                                           CacheMode = bitmapCache
                                                       });
                }
                else if (Application.Current.Resources.Contains("ToolTipStyle"))
                {
                    Style toolTipStyle = Application.Current.Resources["ToolTipStyle"] as Style;
                    ToolTipService.SetToolTip(element, new ToolTip
                                                       {
                                                           Style = toolTipStyle,
                                                           Content = value,
                                                           CacheMode = bitmapCache
                                                       });
                }
                else
                {
                    ToolTipService.SetToolTip(element, value);
                }
            }
        }

        #endregion

        #region 私有方法

        #endregion
    }
}