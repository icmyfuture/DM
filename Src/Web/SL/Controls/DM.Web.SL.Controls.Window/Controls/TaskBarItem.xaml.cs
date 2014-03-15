using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace DM.Web.SL.Controls.Window.Controls
{
    /// <summary>
    ///   任务栏项
    /// </summary>
    public partial class TaskBarItem : IDisposable
    {
        #region Fields

        private string _ApplicationId = string.Empty;
        private string _IcoPath = string.Empty;
        private bool _isActive;
        private string id;

        #endregion Fields

        #region Constructors

        /// <summary>
        ///   构造函数
        /// </summary>
        public TaskBarItem()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Events

        /// <summary>
        ///   Clicked
        /// </summary>
        public event EventHandler Clicked;

        #endregion Events

        #region Properties

        /// <summary>
        ///   应用ID用于区分所属应用
        /// </summary>
        public string ApplicationId
        {
            get { return _ApplicationId; }
            set { _ApplicationId = value; }
        }

        /// <summary>
        ///   标题设置
        /// </summary>
        public string Caption
        {
            get { return txtTitle.ToString(); }
            set
            {
                if (value.Length > 16)
                {
                    value = value.Substring(0, 14) + "..";
                }
                txtTitle.Text = value;
            }
        }

        /// <summary>
        ///   图标设置
        /// </summary>
        public string IcoPath
        {
            get { return _IcoPath; }
            set
            {
                _IcoPath = value;
                if (!string.IsNullOrEmpty(_IcoPath))
                {
                    BitmapImage image = new BitmapImage();
                    image.UriSource = new Uri(_IcoPath);
                    imgIcoPath.Source = image;
                }
            }
        }

        /// <summary>
        ///   唯一标识
        /// </summary>
        public string ID
        {
            get
            {
                if (string.IsNullOrEmpty(id))
                {
                    id = Guid.NewGuid().ToString();
                }
                return id;
            }
            set { id = value; }
        }

        /// <summary>
        ///   是否是活动任务
        /// </summary>
        public bool IsActived
        {
            get { return _isActive; }
            set { _isActive = value; }
        }

        #endregion Properties

        #region Methods

        private void btnTitle_Click(object sender, RoutedEventArgs e)
        {
            if (Clicked != null)
            {
                Clicked(this, EventArgs.Empty);
            }
        }

        private void btnTitle_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!_isActive)
            {
                VisualStateManager.GoToState(btnTitle, "Normal", false);
            }
            else
            {
                VisualStateManager.GoToState(btnTitle, "MouseOver", false);
            }
        }

        #endregion Methods

        #region IDisposable Members

        /// <summary>
        ///   释放内存
        /// </summary>
        public void Dispose()
        {
            imgIcoPath = null;
        }

        #endregion
    }
}