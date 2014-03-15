using System;
using System.Timers;
using DM.Common.Config;

namespace Carrier.Utility
{
    internal static class CarrierTimer
    {
        private static readonly Timer Timer;
        private static readonly int Interval = Convert.ToInt32(Configuration.CommonSettings("Interval"));

        static CarrierTimer()
        {
            var interval = Interval < 1000
                               ? 1000
                               : Interval;
            Timer = new Timer(interval);
        }

        public static void SetMethod(Action act)
        {
            Timer.Elapsed += ((s, e) => act());
        }

        public static void Start()
        {
            Timer.Start();
        }

        public static void Stop()
        {
            Timer.Stop();
        }

        public static void End()
        {
            Timer.Close();
        }
    }
}