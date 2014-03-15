#region Import

using System;

#endregion

namespace DM.Client.WPF.Controls.SimpleControls.DataPager
{
    /// <summary>
    /// 页索引超出范围没有找到
    /// </summary>
    public class PageIndexNotFoundException : Exception
    {

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public PageIndexNotFoundException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public PageIndexNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        #endregion

    }
}
