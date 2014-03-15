using System;

namespace DM.Web.SL.Controls.Guide
{
    /// <summary>
    /// 步骤空异常
    /// </summary>
    public class StepIndexInvalidException : Exception
    {
        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public StepIndexInvalidException()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public StepIndexInvalidException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public StepIndexInvalidException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        #endregion Constructors
    }
}