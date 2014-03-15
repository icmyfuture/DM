using System;
using System.Diagnostics;
using DM.Common.Config;

namespace Carrier.Utility
{
    internal static class Moniter
    {
        private static readonly long MaxMemory = Convert.ToInt64(Configuration.CommonSettings("MaxMemory")) * 1024 * 1024;

        /// <summary>
        ///   加入监控
        /// </summary>
        /// <param name = "process"></param>
        public static void AddWatch(Process process)
        {
            if (MaxMemory > 0 && process.WorkingSet64 > MaxMemory)
            {
                InvokeOverMemory(null);
            }

        }

        public static event EventHandler<EventArgs> OverMemory;

        private static void InvokeOverMemory(EventArgs e)
        {
            var handler = OverMemory;
            if (handler != null)
            {
                handler(null, e);
            }
        }
    }
}