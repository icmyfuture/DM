using System.Windows.Controls;

namespace DM.Client.WPF.Controls.NotifyMessage.Extension.View
{
    /// <summary>
    /// Interaction logic for NotifyWindow.xaml
    /// </summary>
    public partial class NotifyWindow : UserControl
    {
        public NotifyWindow()
        {
            InitializeComponent();
        }

        private void ButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            NotifyMessageManager.NotifyTaskBar.CloseBalloon();
        }
    }
}
