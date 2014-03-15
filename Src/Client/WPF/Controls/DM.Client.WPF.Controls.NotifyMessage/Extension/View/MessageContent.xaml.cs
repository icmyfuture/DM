using DM.Client.WPF.Controls.NotifyMessage.Entitys;
using System.Windows.Controls;
using DM.Client.WPF.Controls.NotifyMessage.Extension.ViewModel;

namespace DM.Client.WPF.Controls.NotifyMessage.Extension.View
{
    /// <summary>
    /// Interaction logic for MessageContent.xaml
    /// </summary>
    public partial class MessageContent : UserControl
    {
        private MessageContentViewModel _model = new MessageContentViewModel();
        public MessageContent()
        {
            InitializeComponent();
            DataContext = _model;
        }

        public MessageContent(NotifyWindowEntity notifyWindowEntity)
            : this()
        {
            switch (notifyWindowEntity.NotifyWindowType)
            {
                case NotifyWindowType.Message:
                    _model.MessageContent = ((NotifyWindowMessage)notifyWindowEntity.NotifyMessage).Message;
                    break;
            }
        }
    }
}
