using System.Diagnostics;
using System.IO;
using System.Windows.Media.Imaging;
using ViewModelBase = Carrier.ViewModel.ViewModelBase;

namespace Carrier.Entities
{
    ///<summary>
    ///  应用实体视图模型
    ///</summary>
    public sealed class ExecuteFile : ViewModelBase
    {
        private int _deatCount;
        private bool _isAlive;
        private bool _needlive;
        private string _port;
        private AppProcess _process;
        private string _startTime;
        private Process _sysProcess;
        private string _url;

        /// <summary>
        ///   应用ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        ///   文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        ///   运行文件名
        /// </summary>
        public string RunFileFullName { get; set; }


        /// <summary>
        ///   显示名字
        /// </summary>
        public string ShowName
        {
            get
            {
                var file = new FileInfo(FilePath);
                if (file.Directory != null) return file.Directory.Name;
                return string.Empty;
            }
        }

        /// <summary>
        ///   文件路径
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        ///   缩略图
        /// </summary>
        public BitmapSource Icon { get; set; }

        /// <summary>
        ///   是否启用
        /// </summary>
        public bool IsAlive
        {
            get { return _isAlive; }
            set
            {
                _isAlive = value;
                OnPropertyChanged("IsAlive");
            }
        }

        /// <summary>
        ///   服务路径
        /// </summary>
        public string Url
        {
            get { return _url; }
            set
            {
                _url = value;
                OnPropertyChanged("Url");
            }
        }

        /// <summary>
        ///   服务端口
        /// </summary>
        public string Port
        {
            get { return _port; }
            set
            {
                _port = value;
                OnPropertyChanged("Port");
            }
        }

        /// <summary>
        ///   启动时间
        /// </summary>
        public string StartTime
        {
            get { return _startTime; }
            set
            {
                _startTime = value;
                OnPropertyChanged("StartTime");
            }
        }

        /// <summary>
        ///   死亡次数
        /// </summary>
        public int DeadCount
        {
            get { return _deatCount; }
            set
            {
                _deatCount = value;
                OnPropertyChanged("DeadCount");
            }
        }

        /// <summary>
        ///   进程实例
        /// </summary>
        public AppProcess Process
        {
            get { return _process; }
            set
            {
                _process = value;
                OnPropertyChanged("Process");
            }
        }

        /// <summary>
        ///   系统进程
        /// </summary>
        public Process SysProcess
        {
            get { return _sysProcess; }
            set
            {
                _sysProcess = value;
                OnPropertyChanged("SysProcess");
            }
        }

        /// <summary>
        ///   是否存活
        /// </summary>
        public bool NeedLive
        {
            get { return _needlive; }
            set
            {
                _needlive = value;
                OnPropertyChanged("NeedLive");
            }
        }

        /// <summary>
        /// 启动顺序
        /// </summary>
        public int StartIndex { get; set; }

        /// <summary>
        ///   释放方法
        /// </summary>
        protected override void OnDispose()
        {
            Dispose();
        }
    }
}