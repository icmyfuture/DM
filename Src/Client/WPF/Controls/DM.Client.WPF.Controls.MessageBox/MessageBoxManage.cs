using System.Windows;
using MvvmLight.Messaging;

namespace DM.Client.WPF.Controls.MessageBox
{
    /// <summary>
    ///   弹出对话框
    /// </summary>
    public sealed class MessageBoxManage
    {
        #region Fields

        /// <summary>
        ///   唯一实例
        /// </summary>
        private static MessageBoxManage _instance;

        /// <summary>
        ///   返回结果
        /// </summary>
        private MessageBoxResult _result = MessageBoxResult.None;

        #endregion

        #region Methods

        /// <summary>
        ///   注册对话框监听事件
        /// </summary>
        public void Register()
        {
            //注册对话框消息
            Messenger.Default.Register<DialogMessage>(this, m =>
                                                                {
                                                                    //弹出对话框
                                                                    new MessageBoxView(m.Content, m.Caption, m.Icon, m.Button, m.OKCaption, m.CancelCaption, r => { _result = r; })
                                                                        {ShowActivated = true}.ShowDialog();
                                                                    //返回对话框结果
                                                                    m.ProcessCallback(_result);
                                                                });
        }

        #endregion

        #region Properties

        /// <summary>
        ///   实例
        /// </summary>
        public static MessageBoxManage Instance
        {
            get { return _instance ?? (_instance = new MessageBoxManage()); }
        }

        #endregion
    }
}