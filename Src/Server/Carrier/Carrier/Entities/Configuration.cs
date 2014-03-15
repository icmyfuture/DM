using ViewModelBase = Carrier.ViewModel.ViewModelBase;

namespace Carrier.Entities
{
    ///<summary>
    ///  系统配置
    ///</summary>
    public sealed class Configuration : ViewModelBase
    {
        private bool _autoLaunch;
        private int _maxMemory;

        /// <summary>
        ///   最大内存占用
        /// </summary>
        public int MaxMemory
        {
            get { return _maxMemory; }
            set
            {
                _maxMemory = value;
                OnPropertyChanged("MaxMemory");
            }
        }

        ///<summary>
        ///  自动启动
        ///</summary>
        public bool Autolaunch
        {
            get { return _autoLaunch; }
            set
            {
                _autoLaunch = value;
                OnPropertyChanged("AutoLaunch");
            }
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