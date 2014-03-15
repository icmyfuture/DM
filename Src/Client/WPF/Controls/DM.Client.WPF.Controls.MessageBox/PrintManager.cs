using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DM.Client.WPF.Controls.MessageBox
{
    /// <summary>
    /// 打印管理
    /// </summary>
    public static class PrintManager
    {

        /// <summary>
        /// 控件Xaml
        /// </summary>
        private static string _xamlStr;

        static PrintManager()
        {
            XmlFormat.Add("&", "&amp;");
            XmlFormat.Add("<", "&lt;");
            XmlFormat.Add(">", "&gt;");
            XmlFormat.Add("'", "&apos;");
            XmlFormat.Add("\"", "&quot;");
        }

        /// <summary>
        ///XML 不支持的数据转义 
        /// </summary>
        private static readonly Dictionary<string, string> XmlFormat = new Dictionary<string, string>();

        /// <summary>
        /// 打印(表格)
        /// </summary>
        /// <param name="list">要打印的对象列表</param>
        private static void Print<T>(List<T> list)
        {
            if (list == null || list.Count <= 0)
                return;
            object objinfo = list[0];
            var classType = objinfo.GetType();
            var stringBuilder = new StringBuilder();


            //开头
            stringBuilder.Append("<Grid xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"");
            stringBuilder.Append(" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" Style=\"{StaticResource PrintGrid}\">");

            //添加行模板(要多一行填写标头)
            var rowsCount = list.Count;
            var rowsBuilder = new StringBuilder();
            rowsBuilder.Append("<Grid.RowDefinitions>");
            for (var row = 0; row < rowsCount + 1; row++)
            {
                rowsBuilder.Append(row == 0 ? "<RowDefinition Height=\"50\" />" : "<RowDefinition Height=\"30\" />");
            }
            rowsBuilder.Append("</Grid.RowDefinitions>");
            stringBuilder.Append(rowsBuilder);


            //添加列模板
            var columnsCount = classType.GetProperties().Length;
            var columnsBuilder = new StringBuilder();
            columnsBuilder.Append("<Grid.ColumnDefinitions>");
            for (var column = 0; column < columnsCount; column++)
            {
                columnsBuilder.Append("<ColumnDefinition />");
            }
            columnsBuilder.Append("</Grid.ColumnDefinitions>");
            stringBuilder.Append(columnsBuilder);


            //添加表头
            var columnCount = 0;
            foreach (var s in classType.GetProperties())
            {
                stringBuilder.Append("<Border Grid.Row=\"0\" Grid.Column=\"" + columnCount + "\" Style=\"{StaticResource PrintFirstBorder}\">");
                stringBuilder.Append("<TextBlock Style=\"{StaticResource PrintFirstCellTextBlock}\" Text=\"" + s.Name + "\" />");
                stringBuilder.Append("</Border>");
                columnCount++;
            }


            //添加表格内容
            var rowCount = 1;
            foreach (var obj in list)
            {
                var one = obj.GetType();
                var contentCount = 0;
                foreach (var t in one.GetProperties())
                {
                    stringBuilder.Append("<Border Grid.Row=\"" + rowCount + "\" Grid.Column=\"" + contentCount + "\" Style=\"{StaticResource PrintBorder}\">");
                    stringBuilder.Append("<TextBlock Style=\"{StaticResource PrintCellTextBlock}\" Text=\"" + FormatXML(t.GetValue(obj, null)) + "\" />");
                    stringBuilder.Append("</Border>");
                    contentCount++;
                }
                rowCount++;
            }

            //结尾
            stringBuilder.Append("</Grid>");

            _xamlStr = stringBuilder.ToString();

        }

        /// <summary>
        /// 显示打印预览窗口
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="title"></param>
        /// <param name="list"></param>
        public static void ShowPrintMessageBox<T>(string title, List<T> list)
        {
            Print(list);
            var printMsg = new PrintControl(title, _xamlStr);
            printMsg.Show();
        }

        /// <summary>
        /// 格式化成XML支持的格式
        /// </summary>
        /// <param name="content">需要转换的对象</param>
        private static string FormatXML(object content)
        {
            if (content == null)
                content = "";
            string result = content.ToString();

            return XmlFormat.Aggregate(result, (current, keyValuePair) => current.Replace(keyValuePair.Key, keyValuePair.Value));
        }
    }
}
