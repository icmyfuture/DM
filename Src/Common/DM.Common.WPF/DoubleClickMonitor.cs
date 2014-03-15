using System;

namespace DM.Common.WPF
{
    ///<summary>
    /// 双击操作检测,针对不支持双击事件的控件
    ///</summary>
    public class DoubleClickMonitor
    {
        private object _target;

        private int _clickCount;

        public static DoubleClickMonitor Instance = new DoubleClickMonitor();

        /// <summary>
        /// 鼠标左键监控
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="e"></param>
        /// <param name="doubleClickHandler">双击事件</param>
        /// <param name="clickHandler">单击事件</param>
        public void AddDoubleMonitor(object obj, EventArgs e, EventHandler doubleClickHandler, EventHandler clickHandler)
        {
            if (_target != obj)
            {
                CleanMonitor();
                _target = obj;
            }

            _clickCount += 1;
            var timer = new System.Windows.Threading.DispatcherTimer {Interval = new TimeSpan(0, 0, 0, 0, 300)};
            timer.Tick += (s, ee) =>
            {
                _clickCount = 0;
                timer.Stop();
                timer = null;
            };

            timer.Start();

            if (_clickCount % 2 == 0)
            {
                _clickCount = 0;
                if (doubleClickHandler != null)
                {
                    doubleClickHandler(obj, e);
                }
            }
            else
            {
                if (clickHandler != null)
                {
                    clickHandler(obj, e);
                }
            }
        }

        private void CleanMonitor()
        {
            _target = null;
            _clickCount = 0;
        }
    }
}
