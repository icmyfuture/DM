namespace DM.Client.WPF.Controls.NotifyMessage.Entitys
{
    public class NotifyWindowEntity
    {
        public NotifyWindowEntity(NotifyWindowBase notifyMessage)
            : this(notifyMessage, NotifyWindowType.Message)
        {

        }

        public NotifyWindowEntity(NotifyWindowBase notifyMessage, NotifyWindowType notifyWindowType)
        {
            NotifyMessage = notifyMessage;
            NotifyWindowType = notifyWindowType;
        }
        public NotifyWindowBase NotifyMessage { get; private set; }

        public NotifyWindowType NotifyWindowType { get; private set; }
    }

}