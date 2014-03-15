namespace DM.Client.WPF.Controls.MessageBox
{
    /// <summary>
    /// 对话框
    /// </summary>
    public interface IMessageBox
    {
        /// <summary>
        /// 关闭窗体
        /// </summary>
        void CloseMessageBox();

        /// <summary>
        /// 返回窗体内容
        /// </summary>
        object MsgContent { get; }
    }
}
