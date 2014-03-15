using System.Windows;

namespace DM.Web.SL.Controls.MessageBox
{
    public abstract class MessageBoxIcon
    {
        #region Fields

        private static readonly ImageIcon m_Caution = new ImageIcon( "/Resources/Image/MsgBox_Caution.png" );
        private static readonly ImageIcon m_Error = new ImageIcon( "/Resources/Image/MsgBox_Error.png" );
        private static readonly ImageIcon m_Information = new ImageIcon( "/Resources/Image/MsgBox_Information.png" );
        private static readonly ImageIcon m_Question = new ImageIcon( "/Resources/Image/MsgBox_Question.png" );
        private static readonly ImageIcon m_Warn = new ImageIcon( "/Resources/Image/MsgBox_Warn.png" );

        #endregion Fields

        #region Properties

        public static MessageBoxIcon Caution
        {
            get
            {
                return m_Caution;
            }
        }

        public static MessageBoxIcon Error
        {
            get
            {
                return m_Error;
            }
        }

        public static MessageBoxIcon Information
        {
            get
            {
                return m_Information;
            }
        }

        public static MessageBoxIcon Question
        {
            get
            {
                return m_Question;
            }
        }

        public static MessageBoxIcon Warn
        {
            get
            {
                return m_Warn;
            }
        }

        #endregion Properties

        #region Methods

        public abstract FrameworkElement GetContentIcon();

        public abstract FrameworkElement GetTitleIcon();

        #endregion Methods
    }
}