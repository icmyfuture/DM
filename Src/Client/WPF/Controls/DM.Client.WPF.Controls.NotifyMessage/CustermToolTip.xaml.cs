using System.Windows.Controls;

namespace DM.Client.WPF.Controls.NotifyMessage
{
    /// <summary>
    /// Interaction logic for CustermToolTip.xaml
    /// </summary>
    public partial class CustermToolTip : Border
    {
        /// <summary>
        /// 
        /// </summary>
        public CustermToolTip(string toolTipText)
        {
            InitializeComponent();
            tblContent.Text = toolTipText;
        }
    }
}
