namespace DM.Client.WPF.Controls.SpliderBar.EventArgs
{
    /// <summary>
    /// 
    /// </summary>
    public class UpdateInfoArgs : System.EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.EventArgs"/> class.
        /// </summary>
        public UpdateInfoArgs(bool isExpande)
        {
            IsExpande = isExpande;
        }

        public bool IsExpande
        {
            get;
            private set;
        }
    }
}