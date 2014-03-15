namespace DM.Client.WPF.Controls.MessageBox
{
    /// <summary>
    /// UserControl1.xaml 的交互逻辑
    /// </summary>
    public partial class Loading
    {
        public Loading()
        {
            InitializeComponent();
        }
        public string ShowMsg
        {
            set
            {
                Msg.Text = value;
            }
        }
    }
}
