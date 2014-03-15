using System;
using System.Windows;
using System.Windows.Browser;
using DM.Web.SL.Common.Utility;

namespace DM.Web.SL.Controls.MessageBox
{

    #region Enumerations

    /// <summary>
    /// 对话框类型枚举值
    /// </summary>
    public enum BoxType
    {
        /// <summary>
        /// 提示
        /// </summary>
        Tip,
        
        /// <summary>
        /// 错误
        /// </summary>
        Error,

        /// <summary>
        /// 确认
        /// </summary>
        Confirm,
    }

    #endregion Enumerations

    ///<summary>
    /// MessageBox控件窗体
    ///</summary>
    public partial class Ai3MessageBox
    {
        #region Constructors

        /// <summary>
        /// 无参构造函数
        /// </summary>
        private Ai3MessageBox()
        {
            InitializeComponent();
            LanguageHelper.Globalization( "DM.Common.Controls", Globalization );
            GotFocus += Ai3MessageBox_GotFocus;
        }

        void Ai3MessageBox_GotFocus(object sender, RoutedEventArgs e)
        {
            HtmlPage.Plugin.Focus();
            OKButton.Focus();
            GotFocus -= Ai3MessageBox_GotFocus;
        }

        /// <summary>
        /// 构造函数 msg
        /// </summary>
        /// <param name="msg">提示信息</param>
        private Ai3MessageBox( String msg )
        {
            InitializeComponent();
            LanguageHelper.Globalization( "DM.Common.Controls", Globalization );
            LanguageHelper.Globalization( "DM.Common.Controls", ( obj, arg ) =>
               {
                   this.msg.Text = msg;
                   OKButton.Margin = new Thickness( 0 );
                   container.Children.Remove( NoButton );
                   container.Children.Remove( CancelButton );
                   OKButton.Content = LanguageHelper.GetDictionary("DM.Common.Controls", "txtOKButton", "OK");
                   Title = LanguageHelper.GetDictionary("DM.Common.Controls", "txtsTips", "Tip");
               } );
            GotFocus += Ai3MessageBox_GotFocus;
        }

        /// <summary>
        /// 构造函数 msg title button
        /// </summary>
        /// <param name="msg">提示信息</param>
        /// <param name="title">标题信息</param> 
        /// <param name="button">按钮类型</param>
        private Ai3MessageBox( String msg, String title, MessageBoxButtonType button )
        {
            InitializeComponent();
            LanguageHelper.Globalization( "DM.Common.Controls", Globalization );
            LanguageHelper.Globalization( "DM.Common.Controls", ( obj, arg ) =>
                {
                    if ( title == "Tip" )
                        title = LanguageHelper.GetDictionary( "DM.Common.Controls", "txtsTips", "Tip" );
                    else if ( title == "Error" )
                        title = LanguageHelper.GetDictionary( "DM.Common.Controls", "Error", "Error" );
                    else if ( title == "Confirm" )
                        title = LanguageHelper.GetDictionary( "DM.Common.Controls", "Confirm", "Confirm" );
                    Title = title;
                    this.msg.Text = msg;
                    switch ( button )
                    {
                        case MessageBoxButtonType.OK:
                            OKButton.Margin = new Thickness( 0 );
                            OKButton.Content = LanguageHelper.GetDictionary( "DM.Common.Controls", "txtOKButton", "OK" );
                            container.Children.Remove( NoButton );
                            container.Children.Remove( CancelButton );
                            break;
                        case MessageBoxButtonType.OKCancel:
                            OKButton.Content = LanguageHelper.GetDictionary( "DM.Common.Controls", "txtOKButton", "OK" );
                            NoButton.Content = LanguageHelper.GetDictionary( "DM.Common.Controls", "txtCancelButton", "Cancel" );
                            container.Children.Remove( CancelButton );
                            break;
                        case MessageBoxButtonType.YesNo:
                            OKButton.Content = LanguageHelper.GetDictionary( "DM.Common.Controls", "txtYesButton", "Yes" );
                            NoButton.Content = LanguageHelper.GetDictionary( "DM.Common.Controls", "txtNoButton", "No" );
                            container.Children.Remove( CancelButton );
                            break;
                        case MessageBoxButtonType.YesNoCancel:
                            OKButton.Content = LanguageHelper.GetDictionary( "DM.Common.Controls", "txtYesButton", "Yes" );
                            NoButton.Content = LanguageHelper.GetDictionary( "DM.Common.Controls", "txtNoButton", "No" );
                            CancelButton.Content = LanguageHelper.GetDictionary( "DM.Common.Controls", "txtCancelButton", "Cancel" );
                            break;
                    }
                } );
            GotFocus += Ai3MessageBox_GotFocus;
        }

        /// <summary>
        /// 构造函数 msg type button
        /// </summary>
        /// <param name="msg">提示信息</param>
        /// <param name="type">标题类型</param>
        /// <param name="button">按钮类型</param>
        private Ai3MessageBox( string msg, BoxType type, MessageBoxButtonType button )
            : this( msg, type.ToString(), button )
        {
        }

        /// <summary>
        /// 构造函数 msg type button
        /// </summary>
        /// <param name="msg">提示信息</param>
        /// <param name="type">标题类型</param>
        /// <param name="button">按钮类型</param>
        /// <param name="width">弹框宽度</param>
        /// <param name="height">弹框高度</param>
        private Ai3MessageBox( string msg, BoxType type, MessageBoxButtonType button, Double width, Double height )
            : this( msg, type.ToString(), button )
        {
            //Width = width;
            //Height = height;
        }

        /// <summary>
        /// 构造函数 msg type button
        /// </summary>
        /// <param name="msg">提示信息</param>
        /// <param name="type">标题类型</param>
        /// <param name="button">按钮类型</param>
        /// <param name="width">弹框宽度</param>
        /// <param name="height">弹框高度</param>
        /// <param name="callBack_Closed">回调事件</param>
        private Ai3MessageBox( string msg, BoxType type, MessageBoxButtonType button, Double width, Double height, EventHandler<ClosedEventArgs> callBack_Closed )
            : this( msg, type.ToString(), button )
        {
            //Width = width;
            //Height = height;
        }

        /// <summary>
        /// 构造函数 msg type button
        /// </summary>
        /// <param name="msg">提示信息</param>
        /// <param name="title">标题信息</param>
        /// <param name="button">按钮类型</param>
        /// <param name="width">弹框宽度</param>
        /// <param name="height">弹框高度</param>
        private Ai3MessageBox( string msg, String title, MessageBoxButtonType button, Double width, Double height )
            : this( msg, title, button )
        {
            //Width = width;
            //Height = height;
        }

        /// <summary>
        /// 构造函数 msg title button width height
        /// </summary>
        /// <param name="msg">提示信息</param>
        /// <param name="title">标题信息</param>
        /// <param name="button">按钮类型</param>
        /// <param name="width">弹框宽度</param>
        /// <param name="height">弹框高度</param>
        /// <param name="callBack_Closed">回调事件</param>
        private Ai3MessageBox( string msg, String title, MessageBoxButtonType button, Double width, Double height, EventHandler<ClosedEventArgs> callBack_Closed )
            : this( msg, title, button )
        {
            //Width = width;
            //Height = height;
        }

        #endregion Constructors

        #region Properties

        public EventHandler<ClosedEventArgs> ClosedHandler { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 弹出对话框 msg
        /// </summary>
        /// <param name="msg">提示信息</param>
        public static void Show( string msg )
        {
            Ai3MessageBox msgbox = new Ai3MessageBox( msg );
            LanguageHelper.Globalization( "DM.Common.Controls", ( obj, arg ) =>
            {
                msgbox.Show();
            } );
        }

        /// <summary>
        /// 弹出对话框 msg type button
        /// </summary>
        /// <param name="msg">提示信息</param>
        /// <param name="type">标题类型</param>
        /// <param name="button">按钮类型</param>
        public static void Show( string msg, BoxType type, MessageBoxButtonType button )
        {
            Ai3MessageBox msgbox = new Ai3MessageBox( msg, type, button );
            LanguageHelper.Globalization( "DM.Common.Controls", ( obj, arg ) =>
            {
                msgbox.Show();
            } );
        }

        /// <summary>
        /// 弹出对话框 msg type button callBack_Closed
        /// </summary>
        /// <param name="msg">提示信息</param>
        /// <param name="type">标题类型</param>
        /// <param name="button">按钮类型</param>
        /// <param name="callBack_Closed">回调事件</param>
        public static void Show( string msg, BoxType type, MessageBoxButtonType button, EventHandler<ClosedEventArgs> callBack_Closed )
        {
            Ai3MessageBox msgbox = new Ai3MessageBox( msg, type, button );
            msgbox.ClosedHandler = callBack_Closed;
            LanguageHelper.Globalization( "DM.Common.Controls", ( obj, arg ) =>
            {
                msgbox.Show();
            } );
        }

        /// <summary>
        /// 弹出对话框 msg title button width height
        /// </summary>
        /// <param name="msg">提示信息</param>
        /// <param name="type">标题类型</param>
        /// <param name="button">按钮类型</param>
        /// <param name="width">弹框宽度</param>
        /// <param name="height">弹框高度</param>
        public static void Show( string msg, BoxType type, MessageBoxButtonType button, Double width, Double height )
        {
            Ai3MessageBox msgbox = new Ai3MessageBox( msg, type, button, width, height );
            LanguageHelper.Globalization( "DM.Common.Controls", ( obj, arg ) =>
               {
                   msgbox.Show();
               } );
        }

        /// <summary>
        /// 弹出对话框 msg title button width height callBack_Closed
        /// </summary>
        /// <param name="msg">提示信息</param>
        /// <param name="type">标题类型</param>
        /// <param name="button">按钮类型</param>
        /// <param name="width">弹框宽度</param>
        /// <param name="height">弹框高度</param>
        /// <param name="callBack_Closed">回调事件</param>
        public static void Show( string msg, BoxType type, MessageBoxButtonType button, Double width, Double height, EventHandler<ClosedEventArgs> callBack_Closed )
        {
            Ai3MessageBox msgbox = new Ai3MessageBox( msg, type, button, width, height );
            msgbox.ClosedHandler = callBack_Closed;
            LanguageHelper.Globalization( "DM.Common.Controls", ( obj, arg ) =>
               {
                   msgbox.Show();
               } );
        }

        /// <summary>
        /// 弹出对话框 msg title button
        /// </summary>
        /// <param name="msg">提示信息</param>
        /// <param name="title">标题信息</param>
        /// <param name="button">按钮类型</param>
        public static void Show( string msg, String title, MessageBoxButtonType button )
        {
            Ai3MessageBox msgbox = new Ai3MessageBox( msg, title, button );
            LanguageHelper.Globalization( "DM.Common.Controls", ( obj, arg ) =>
               {
                   msgbox.Show();
               } );
        }
        /// <summary>
        /// 弹出对话框  
        /// Author:cj
        /// </summary>
        /// <param name="msg">提示信息</param>
        /// <param name="button">按钮类型</param>
        public static void Show( string msg, MessageBoxButtonType button )
        {
            string title = LanguageHelper.GetDictionary("DM.Common.Controls", "txtsTips", "Tip");
            Show(msg, title, button);
        }

        /// <summary>
        /// 弹出对话框 msg title button callBack_Closed
        /// </summary>
        /// <param name="msg">提示信息</param>
        /// <param name="title">标题信息</param>
        /// <param name="button">按钮类型</param>
        /// <param name="callBack_Closed">回调事件</param>
        public static void Show( string msg, String title, MessageBoxButtonType button, EventHandler<ClosedEventArgs> callBack_Closed )
        {
            Ai3MessageBox msgbox = new Ai3MessageBox( msg, title, button );
            msgbox.ClosedHandler = callBack_Closed;
            LanguageHelper.Globalization( "DM.Common.Controls", ( obj, arg ) =>
               {
                   msgbox.Show();
               } );
        }

        /// <summary>
        /// 弹出对话框 msg title button callBack_Closed
        /// Author:cj
        /// </summary>
        /// <param name="msg">提示信息</param>
        /// <param name="button">按钮类型</param>
        /// <param name="callBack_Closed">回调事件</param>
        public static void Show( string msg,  MessageBoxButtonType button, EventHandler<ClosedEventArgs> callBack_Closed )
        {
            string title = LanguageHelper.GetDictionary( "DM.Common.Controls", "txtsTips", "Tip" );
            Show(msg, title, button, callBack_Closed);
        }

        /// <summary>
        /// 弹出对话框 msg title button width height
        /// </summary>
        /// <param name="msg">提示信息</param>
        /// <param name="title">标题信息</param>
        /// <param name="button">按钮类型</param>
        /// <param name="width">弹框宽度</param>
        /// <param name="height">弹框高度</param>
        public static void Show( string msg, String title, MessageBoxButtonType button, Double width, Double height )
        {
            Ai3MessageBox msgbox = new Ai3MessageBox( msg, title, button, width, height );
            LanguageHelper.Globalization( "DM.Common.Controls", ( obj, arg ) =>
               {
                   msgbox.Show();
               } );
        }

        /// <summary>
        /// 弹出对话框 msg title button width height
        /// Author:cj
        /// </summary>
        /// <param name="msg">提示信息</param>
       /// <param name="button">按钮类型</param>
        /// <param name="width">弹框宽度</param>
        /// <param name="height">弹框高度</param>
        public static void Show( string msg,   MessageBoxButtonType button, Double width, Double height )
        {
            string title = LanguageHelper.GetDictionary( "DM.Common.Controls", "txtsTips", "Tip" );
            Show(msg, title, button, width, height);
        }

        /// <summary>
        /// 弹出对话框 msg title button width height callBack_Closed
        /// </summary>
        /// <param name="msg">提示信息</param>
        /// <param name="title">标题信息</param>
        /// <param name="button">按钮类型</param>
        /// <param name="width">弹框宽度</param>
        /// <param name="height">弹框高度</param>
        /// <param name="callBack_Closed">回调事件</param>
        public static void Show( string msg, String title, MessageBoxButtonType button, Double width, Double height, EventHandler<ClosedEventArgs> callBack_Closed )
        {
            Ai3MessageBox msgbox = new Ai3MessageBox( msg, title, button, width, height );
            msgbox.ClosedHandler = callBack_Closed;
            LanguageHelper.Globalization( "DM.Common.Controls", ( obj, arg ) =>
               {
                   msgbox.Show();
               } );
        }

        /// <summary>
        /// 弹出对话框 msg title button width height callBack_Closed
        /// </summary>
        /// Author:cj
        /// <param name="msg">提示信息</param>
       /// <param name="button">按钮类型</param>
        /// <param name="width">弹框宽度</param>
        /// <param name="height">弹框高度</param>
        /// <param name="callBack_Closed">回调事件</param>
        public static void Show( string msg,   MessageBoxButtonType button, Double width, Double height, EventHandler<ClosedEventArgs> callBack_Closed )
        {
            string title = LanguageHelper.GetDictionary( "DM.Common.Controls", "txtsTips", "Tip" );
            Show(msg, title, button, width, height, callBack_Closed);
        }

        /// <summary>
        /// 弹出对话框 msg type button callBack_Closed callBack_windowClose 
        /// 添加一个参数，暴露窗口关闭事件
        /// </summary>
        /// <param name="msg">提示信息</param>
        /// <param name="type">标题类型</param>
        /// <param name="button">按钮类型</param>
        /// <param name="callBack_Closed">回调事件</param>
        public static void Show(string msg, BoxType type, MessageBoxButtonType button, EventHandler<ClosedEventArgs> callBack_Closed, EventHandler windowClosed)
        {
            Ai3MessageBox msgbox = new Ai3MessageBox(msg, type, button);
            msgbox.ClosedHandler = callBack_Closed;
            msgbox.Closed += windowClosed;
            LanguageHelper.Globalization("DM.Common.Controls", (obj, arg) =>
            {
                msgbox.Show();
            });
        }

        /// <summary>
        /// No点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NoButton_Click( object sender, RoutedEventArgs e )
        {
            DialogResult = false;
            msgbox_Closed( this, e );
        }

        /// <summary>
        /// Cancel点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButton_Click( object sender, RoutedEventArgs e )
        {
            DialogResult = null;
            msgbox_Closed( this, e );
            DialogResult = false;
        }

        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void msgbox_Closed( object sender, EventArgs e )
        {
            Ai3MessageBox box = (Ai3MessageBox)sender;
            if ( box.ClosedHandler != null )
                box.ClosedHandler( box, new ClosedEventArgs( box.DialogResult ) );
        }

        private void YesButton_Click( object sender, RoutedEventArgs e )
        {
            DialogResult = true;
            msgbox_Closed( this, e );
        }


        private void Globalization( object sender, EventArgs e )
        {
            OKButton.Content = LanguageHelper.GetDictionary( "DM.Common.Controls", "txtYesButton", "Yes" );
            NoButton.Content = LanguageHelper.GetDictionary( "DM.Common.Controls", "txtNoButton", "No" );
            CancelButton.Content = LanguageHelper.GetDictionary( "DM.Common.Controls", "txtCancelButton", "Cancel" );

        }
        #endregion Methods

        #region Nested Types

        /// <summary>
        /// ClosedEventArgs
        /// </summary>
        public class ClosedEventArgs : EventArgs
        {
            #region Fields

            private bool? _dialogResult;

            #endregion Fields

            #region Constructors

            public ClosedEventArgs( bool? dialogResult )
            {
                _dialogResult = dialogResult;
            }

            #endregion Constructors

            #region Properties

            public bool? DialogResult
            {
                get { return _dialogResult; }
            }

            #endregion Properties
        }

        #endregion Nested Types
    }
}