using System;
using System.Windows;

namespace DM.Web.SL.Controls.Styles
{
    public class StyleManager
    {
        /// <summary>
        /// 添加样式
        /// </summary>
        public static void AddStyles()
        {
            var dic = Application.Current.Resources.MergedDictionaries;
            //dic.Add(LoadResourceDictionary("Button"));
            //dic.Add(LoadResourceDictionary("CheckBox"));
            //dic.Add(LoadResourceDictionary("ChildWindow"));
            //dic.Add(LoadResourceDictionary("ComboBox"));
            //dic.Add(LoadResourceDictionary("Datagrid"));
            //dic.Add(LoadResourceDictionary("DatePicker"));
            //dic.Add(LoadResourceDictionary("HyperlinkButton"));
            //dic.Add(LoadResourceDictionary("ListBox"));
            //dic.Add(LoadResourceDictionary("NumericUpDown"));
            //dic.Add(LoadResourceDictionary("PasswordBox"));
            //dic.Add(LoadResourceDictionary("ProgressBar"));
            //dic.Add(LoadResourceDictionary("RadioButton"));
            //dic.Add(LoadResourceDictionary("RepeatButton"));
            //dic.Add(LoadResourceDictionary("ScrollViewer"));
            //dic.Add(LoadResourceDictionary("Search_OneContent"));
            //dic.Add(LoadResourceDictionary("StackPanel"));
            //dic.Add(LoadResourceDictionary("TabContorl"));
            //dic.Add(LoadResourceDictionary("TextBlock"));
            //dic.Add(LoadResourceDictionary("ToggleButton"));
            //dic.Add(LoadResourceDictionary("ToolTip"));
            //dic.Add(LoadResourceDictionary("UserControl"));
            dic.Add(LoadResourceDictionary("Window"));
        }

        private static ResourceDictionary LoadResourceDictionary(string styleFileName)
        {
            var resourceDictionary = new ResourceDictionary();
            Application.LoadComponent(resourceDictionary, new Uri(string.Format("/DM.Web.SL.Controls.Styles;component/Style/{0}.xaml", styleFileName), UriKind.Relative));
            return resourceDictionary;
        }
    }
}