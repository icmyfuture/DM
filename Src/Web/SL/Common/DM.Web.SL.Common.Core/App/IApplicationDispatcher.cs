using System;
using System.Windows.Threading;

namespace DM.Web.SL.Common.Core.App
{
    #region Imports

    

    #endregion

    ///<summary>
    /// IApplicationDispatcher
    ///</summary>
    public interface IApplicationDispatcher
    {
        ///<summary>
        /// BeginInvoke
        ///</summary>
        ///<param name="a"></param>
        ///<returns></returns>
        DispatcherOperation BeginInvoke(Action a);

        ///<summary>
        /// BeginInvoke
        ///</summary>
        ///<param name="d"></param>
        ///<param name="args"></param>
        ///<returns></returns>
        DispatcherOperation BeginInvoke(Delegate d, params object[] args);

        ///<summary>
        /// CheckAccess
        ///</summary>
        ///<returns></returns>
        bool CheckAccess();
    }
}