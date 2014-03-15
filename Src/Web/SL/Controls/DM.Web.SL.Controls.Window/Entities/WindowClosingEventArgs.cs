using System.ComponentModel;

namespace DM.Web.SL.Controls.Window.Entities
{

    #region Enumerations

    /// <summary>
    ///   Specifies the reason that a form was closed.
    /// </summary>
    public enum CloseReason
    {
        /// <summary>
        ///   The cause of the closure was not defined or could not be determined.
        /// </summary>
        None = 0,

        /// <summary>
        ///   The operating system is closing all applications before shutting down.
        /// </summary>
        WindowsShutDown = 1,

        /// <summary>
        ///   The parent form of this multiple document interface (MDI) form is closing.
        /// </summary>
        MdiFormClosing = 2,

        /// <summary>
        ///   The user is closing the form through the user interface (UI), for example
        ///   by clicking the Close button on the form window, selecting Close from the
        ///   window's control menu, or pressing ALT+F4.
        /// </summary>
        UserClosing = 3,

        /// <summary>
        ///   The Microsoft Windows Task Manager is closing the application.
        /// </summary>
        TaskManagerClosing = 4,

        /// <summary>
        ///   The owner form is closing.
        /// </summary>
        FormOwnerClosing = 5,

        /// <summary>
        ///   The System.Windows.Forms.Application.Exit() method of the System.Windows.Forms.Application
        ///   class was invoked.
        /// </summary>
        ApplicationExitCall = 6,
    }

    #endregion Enumerations

    /// <summary>
    ///   窗口关闭
    /// </summary>
    public class WindowClosingEventArgs : CancelEventArgs
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
        public WindowClosingEventArgs(CloseReason closeReason, bool cancel)
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