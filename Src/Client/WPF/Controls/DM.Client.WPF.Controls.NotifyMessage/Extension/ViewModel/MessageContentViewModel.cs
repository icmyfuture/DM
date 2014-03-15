using MvvmLight;

namespace DM.Client.WPF.Controls.NotifyMessage.Extension.ViewModel
{
    public class MessageContentViewModel : ViewModelBase
    {
        private string m_messageContent;

        /// <summary>
        /// 消息内容
        /// </summary>
        public string MessageContent
        {
            get { return m_messageContent; }
            set
            {
                m_messageContent = value;
                RaisePropertyChanged("MessageContent");
            }
        }
    }
}
