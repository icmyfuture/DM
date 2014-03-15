using System.ComponentModel;

namespace DM.Web.SL.Controls.Window.Entities
{
    /// <summary>
    ///   窗口关闭
    /// </summary>
    public class WindowClosedEventArgs : CancelEventArgs
    {
        #region Fields

        private readonly CloseReason close_reason;

        #endregion Fields

        #region Constructors

        /// <summary>
        ///   Initializes a new instance of the System.Windows.Forms.FormClosingEventArgs class.
        /// </summary>
        /// <param name = "closeReason">A System.Windows.Forms.CloseReason value that represents the reason why the form is being closed.</param>
        /// <param name = "cancel">true to cancel the event; otherwise, false.</param>
        public WindowClosedEventArgs(CloseReason closeReason, bool cancel)
            : base(cancel)
        {
            close_reason = closeReason;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        ///   Gets a value that indicates why the form is being closed.
        /// </summary>
        public CloseReason CloseReason
        {
            get { return close_reason; }
        }

        #endregion Properties
    }
}