using System;
using System.Windows.Input;

namespace Carrier.ViewModel
{
    /// <summary>
    ///   设置窗口视图
    /// </summary>
    public sealed class OptionViewModel : ViewModelBase
    {
        /// <summary>
        ///   视图单件实例
        /// </summary>
        public static readonly OptionViewModel Instance = new OptionViewModel();

        private ICommand _exit;
        private ICommand _save;
        private ICommand _saveexit;

        private OptionViewModel()
        {
        }

        /// <summary>
        ///   退出设置
        /// </summary>
        public ICommand Exit
        {
            get { return _exit ?? (_exit = new RelayCommand(act => ExitOption(), allow => true)); }
        }

        /// <summary>
        ///   保存设置
        /// </summary>
        public ICommand Save
        {
            get { return _save ?? (_save = new RelayCommand(act => SaveOption(), allow => true)); }
        }

        /// <summary>
        ///   保存设置并退出
        /// </summary>
        public ICommand SaveExit
        {
            get { return _saveexit ?? (_saveexit = new RelayCommand(act => SaveExitOption(), allow => true)); }
        }

        ///<summary>
        ///  关闭事件
        ///</summary>
        public event EventHandler RequestExit;

        private void ExitOption()
        {
            EventHandler handler = RequestExit;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private void SaveOption()
        {
        }

        private void SaveExitOption()
        {
            SaveOption();
            ExitOption();
        }

        /// <summary>
        ///   释放方法
        /// </summary>
        protected override void OnDispose()
        {
            Dispose();
        }
    }
}