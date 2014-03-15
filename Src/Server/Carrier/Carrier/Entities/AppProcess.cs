using ViewModelBase = Carrier.ViewModel.ViewModelBase;

namespace Carrier.Entities
{
    ///<summary>
    ///  应用进程信息
    ///</summary>
    public sealed class AppProcess : ViewModelBase
    {
        private long _memory;
        private int _pid;

        /// <summary>
        ///   进程ID
        /// </summary>
        public int Pid
        {
            get { return _pid; }
            set
            {
                _pid = value;
                OnPropertyChanged("Pid");
            }
        }

        /// <summary>
        ///   进程占用内存
        /// </summary>
        public long Memory
        {
            get { return _memory; }
            set
            {
                _memory = value;
                OnPropertyChanged("Memory");
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