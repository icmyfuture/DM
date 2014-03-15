using System;
using System.Windows;

namespace DM.Client.WPF.Controls.Styles
{
    public static class StyleManager
    {
        /// <summary>
        /// 添加样式
        /// </summary>
        public static void AddStyles()
        {
            var dic = Application.Current.Resources.MergedDictionaries;
            dic.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/DM.Client.WPF.Controls.Styles;component/Style/Common/BaseColor.xaml") });
            //公共颜色
            dic.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/DM.Client.WPF.Controls.Styles;component/Style/Common/BaseColor.xaml") });
            //公共滚动条-->
            dic.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/DM.Client.WPF.Controls.Styles;component/Style/Common/BaseScrollBarStyle.xaml") });
            //公共TabControl-->
            dic.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/DM.Client.WPF.Controls.Styles;component/Style/Common/BaseTabControlStyle.xaml") });
            //公共TextBox-->
            dic.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/DM.Client.WPF.Controls.Styles;component/Style/Common/BaseTextBoxStyle.xaml") });
            //公共PasswordBox-->
            dic.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/DM.Client.WPF.Controls.Styles;component/Style/Common/BasePasswordBoxStyle.xaml") });
            //公共CheckBox-->
            dic.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/DM.Client.WPF.Controls.Styles;component/Style/Common/BaseCheckBoxStyle.xaml") });
            //公共ComboBox-->
            dic.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/DM.Client.WPF.Controls.Styles;component/Style/Common/BaseComboBoxStyle.xaml") });
            //公共Button-->
            dic.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/DM.Client.WPF.Controls.Styles;component/Style/Common/BaseButtonStyle.xaml") });
            //公共ProgressBar-->
            dic.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/DM.Client.WPF.Controls.Styles;component/Style/Common/BaseProgressBarStyle.xaml") });
            //公共DataGrid-->
            dic.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/DM.Client.WPF.Controls.Styles;component/Style/Common/BaseDatagridStyle.xaml") });
            //公共ListView-->
            dic.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/DM.Client.WPF.Controls.Styles;component/Style/Common/BaseListViewStyle.xaml") });
            //公共TextBlock-->
            dic.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/DM.Client.WPF.Controls.Styles;component/Style/Common/BaseTextBlock.xaml") });
            //公共ListBox-->
            dic.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/DM.Client.WPF.Controls.Styles;component/Style/Common/BaseListBoxStyle.xaml") });
            //公共GroupBox-->
            dic.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/DM.Client.WPF.Controls.Styles;component/Style/Common/BaseGroupBoxStyle.xaml") });
            //公共DatePicker-->
            dic.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/DM.Client.WPF.Controls.Styles;component/Style/Common/BaseDatePicker.xaml") });
            //公共Expander-->
            dic.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/DM.Client.WPF.Controls.Styles;component/Style/Common/BaseExpander.xaml") });
            //公共打印样式-->
            dic.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/DM.Client.WPF.Controls.Styles;component/Style/Common/BasePrint.xaml") });
            //公共ToolTip-->
            dic.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/DM.Client.WPF.Controls.Styles;component/Style/Common/BaseToolTipStyle.xaml") });
            //此样式能去除按钮点击后的虚线框-->
            dic.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/DM.Client.WPF.Controls.Styles;component/Style/Common/MyFocusVisual.xaml") });
            //RadioButton-->
            dic.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/DM.Client.WPF.Controls.Styles;component/Style/Common/BaseRadioButtonStyle.xaml") });
            //ValidateTextBoxStyle-->
            dic.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/DM.Client.WPF.Controls.Styles;component/Style/Common/ValidateTextBoxStyle.xaml") });
            //TreeView-->
            dic.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/DM.Client.WPF.Controls.Styles;component/Style/NavControlStyle.xaml") });
            //主窗体样式-->
            dic.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/DM.Client.WPF.Controls.Styles;component/Style/MainView/Window_max_button.xaml") });
            dic.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/DM.Client.WPF.Controls.Styles;component/Style/MainView/Window_min_button.xaml") });
            dic.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/DM.Client.WPF.Controls.Styles;component/Style/MainView/Window_close_button.xaml") });
            dic.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/DM.Client.WPF.Controls.Styles;component/Style/MainView/GridSplitter.xaml") });
            dic.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/DM.Client.WPF.Controls.Styles;component/Style/Common/BaseProgressBarStyle.xaml") });
            dic.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/DM.Client.WPF.Controls.Styles;component/Style/MainView/Setting_button.xaml") });
            dic.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/DM.Client.WPF.Controls.Styles;component/Style/Common/BasicListBox.xaml") });
            dic.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/DM.Client.WPF.Controls.Styles;component/Style/Common/BaseTextBoxPoint.xaml") });
            //其他
            dic.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/DM.Client.WPF.Controls.Styles;component/Style/Common/contextmenu.xaml") });
            dic.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/DM.Client.WPF.Controls.Styles;component/Style/MainView/window_help_button.xaml") });
            dic.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/DM.Client.WPF.Controls.Styles;component/Style/MainView/window_config_button.xaml") });
            dic.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/DM.Client.WPF.Controls.Styles;component/Style/Common/Simple_Styles.xaml") });
            dic.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/DM.Client.WPF.Controls.Styles;component/Style/MessageBox/BaseMessageBoxStyle.xaml") });
        }
    }
}