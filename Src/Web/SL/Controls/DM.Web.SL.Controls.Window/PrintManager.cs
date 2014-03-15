using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Browser;
using DM.Web.SL.Common.Extensions;
using DM.Web.SL.Common.Utility;
using DM.Web.SL.Controls.Window.Entities;

namespace DM.Web.SL.Controls.Window
{
    /// <summary>
    ///   打印类
    /// </summary>
    public class PrintManager
    {
        ///<summary>
        ///  获取当前实例
        ///</summary>
        public static readonly PrintManager Instance = new PrintManager();
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public PrintManager()
        {
            ViewExportText = LanguageHelper.GetDictionary( "DM.Common.Controls", "txtbtnExport", "Export" );
            LanguageHelper.Globalization("DM.Common.Controls", (obj, arg) =>
                                                                {
                                                                    ViewExportText = LanguageHelper.GetDictionary("DM.Common.Controls", "txtbtnExport", "Export");
                                                                });
        }

        /// <summary>
        /// 导出按钮的文本
        /// </summary>
        public static string ViewExportText { get; set; }

        /// <summary>
        ///   打印(自定义格式)
        /// </summary>
        /// <param name = "title">打印标题</param>
        /// <param name = "html">打印对象的html字符串</param>
        public void Print(string title, string html)
        {
            PrintObjInfo printInfo = new PrintObjInfo
                                         {
                                             Title = title,
                                             Type = "html",
                                             Content = html
                                         };
            Print(printInfo);
        }

        /// <summary>
        ///   打印(自定义格式)
        /// </summary>
        /// <param name = "viewPrint">打印字样显示</param>
        /// <param name = "title">打印标题</param>
        /// <param name = "html">打印对象的html字符串</param>
        public void Print( string viewPrint,  string title, string html )
        {
            PrintObjInfo printInfo = new PrintObjInfo
                                     {
                                         ViewPrint = viewPrint,
                                         Title = title,
                                         Type = "html",
                                         Content = html
                                     };
            Print(printInfo);
        }

        /// <summary>
        ///   打印
        /// </summary>
        /// <param name = "printInfo">打印信息</param>
        public void Print( PrintObjInfo printInfo )
        {
            printInfo.ViewExport = ViewExportText;
            if (string.IsNullOrEmpty(printInfo.ViewPrint))
            {
                string viewPrint = LanguageHelper.GetDictionary("DM.Common.Controls", "viewPrint", "Print");
                printInfo.ViewPrint = viewPrint;
            }
            string printInfoJsonstr = printInfo.ToJson();
            HtmlWindow win = HtmlPage.Window;
            win.Eval("PrintObj=" + printInfoJsonstr + ";");
            win.Eval("OpenPrint();");
        }


        /// <summary>
        ///   打印（表格）
        /// </summary>
        /// <param name = "list">要打印的对象列表</param>
        public void Print<T>(List<T> list)
        {
            Print("", list);
        }

        /// <summary>
        ///   打印（表格）
        /// </summary>
        /// <param name = "viewPrint">打印字样显示</param>
        /// <param name = "tableHeade">表头</param>
        /// <param name = "list">要打印的对象列表</param>
        public void Print<T>(string viewPrint, string[] tableHeade, List<T> list)
        {
            Print(viewPrint, tableHeade, "", list);
        }

        /// <summary>
        ///   打印（表格）
        /// </summary>
        /// <param name = "viewPrint">打印字样显示</param>
        /// <param name = "tableHeade">表头</param>
        /// <param name = "title">打印表标题</param>
        /// <param name = "list">要打印的对象列表</param>
        public void Print<T>(string viewPrint, string[] tableHeade, string title, List<T> list)
        {
            if (list == null || list.Count <= 0)
                return;
            //string className = assemblies + list[0];
            //object assembly = Assembly.GetExecutingAssembly().CreateInstance(className);
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("<table>");

            //添加表头
            stringBuilder.Append("<tr>");

            foreach (var s in tableHeade)
            {
                stringBuilder.Append("<th>");
                stringBuilder.Append(s);
                stringBuilder.Append("</th>");
            }
            stringBuilder.Append("</tr>");

            //添加表格内容
            foreach (var obj in list)
            {
                //string name = assemblies + s;
                //object obj = Assembly.GetExecutingAssembly().CreateInstance(name);
                Type one = obj.GetType();
                stringBuilder.Append("<tr>");

                foreach (var t in one.GetProperties())
                {
                    stringBuilder.Append("<td>");
                    stringBuilder.Append(t.GetValue(obj, null));
                    stringBuilder.Append("</td>");
                }
                stringBuilder.Append("</tr>");
            }
            stringBuilder.Append("</table>");

            Print(viewPrint, title, stringBuilder.ToString());
        }

        /// <summary>
        ///   打印（表格）
        /// </summary>
        /// <param name = "title">打印表标题</param>
        /// <param name = "list">要打印的对象列表</param>
        public void Print<T>(string title, List<T> list)
        {
            if (list == null || list.Count <= 0)
                return;
            object objinfo = list[0];
            //string className = assemblies + list[0];
            //object assembly = Assembly.GetExecutingAssembly().CreateInstance(className);
            Type classType = objinfo.GetType();

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("<table>");

            //添加表头
            stringBuilder.Append("<tr>");

            foreach (var s in classType.GetProperties())
            {
                stringBuilder.Append("<th>");
                stringBuilder.Append(s.Name);
                stringBuilder.Append("</th>");
            }
            stringBuilder.Append("</tr>");

            //添加表格内容
            foreach (var obj in list)
            {
                //string name = assemblies + s;
                //object obj = Assembly.GetExecutingAssembly().CreateInstance(name);
                Type one = obj.GetType();
                stringBuilder.Append("<tr>");

                foreach (var t in one.GetProperties())
                {
                    stringBuilder.Append("<td>");
                    stringBuilder.Append(t.GetValue(obj, null));
                    stringBuilder.Append("</td>");
                }
                stringBuilder.Append("</tr>");
            }
            stringBuilder.Append("</table>");

            Print(title, stringBuilder.ToString());
        }
    }
}