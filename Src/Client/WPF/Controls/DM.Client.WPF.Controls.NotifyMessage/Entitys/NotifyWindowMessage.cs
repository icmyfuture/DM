namespace DM.Client.WPF.Controls.NotifyMessage.Entitys
{
    public class NotifyWindowMessage : NotifyWindowBase
    {
        public NotifyWindowMessage(string message)
        {
            Message = message;
        }

        public string Message { get; set; }
    }
}
