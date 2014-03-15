using System;
using System.Windows;
using System.Windows.Threading;
using DM.Web.SL.Common.Core.App;

namespace DM.Web.SL.Common.Utility
{
    /// <summary>
    /// ApplicationDispatcher
    /// </summary>
    public class ApplicationDispatcher
    {
        private static IApplicationDispatcher m_current;

        ///<summary>
        /// 当前应用Dispatcher
        ///</summary>
        public static IApplicationDispatcher Current
        {
            get
            {
                if (m_current == null)
                {
                    m_current = GetDefaultApplicationDispatcher();
                }
                return m_current;
            }
            internal set { m_current = value; }
        }

        private static IApplicationDispatcher GetDefaultApplicationDispatcher()
        {
            var dispatcher = Deployment.Current.Dispatcher;
            if (dispatcher != null)
            {
                return new DispatcherAdapter(dispatcher);
            }
            return null;
        }

        #region Nested type: DispatcherAdapter

        /// <summary>
        ///   Adapts a <see cref = "Dispatcher" /> to an IApplicationDispatcher
        /// </summary>
        private class DispatcherAdapter : IApplicationDispatcher
        {
            private readonly Dispatcher m_dispatcher;

            public DispatcherAdapter(Dispatcher dispatcher)
            {
                m_dispatcher = dispatcher;
            }

            #region IApplicationDispatcher Members

            public DispatcherOperation BeginInvoke(Action a)
            {
                return m_dispatcher.BeginInvoke(a);
            }

            public DispatcherOperation BeginInvoke(Delegate d, params object[] args)
            {
                return m_dispatcher.BeginInvoke(d, args);
            }

            public bool CheckAccess()
            {
                return m_dispatcher.CheckAccess();
            }

            #endregion
        }

        #endregion
    }
}