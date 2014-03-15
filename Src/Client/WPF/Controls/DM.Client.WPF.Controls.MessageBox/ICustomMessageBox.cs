using System;

namespace DM.Client.WPF.Controls.MessageBox
{
    public interface ICustomMessageBox : IDisposable
    {
        /// <summary>
        /// 关闭通知事件
        /// </summary>
        event EventHandler Close;
    }
}
