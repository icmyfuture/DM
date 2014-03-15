using System;
using DM.Web.SL.Common.Core.App;
using DM.Web.SL.Common.Extensions;

namespace DM.Web.SL.Sample.App.Demo
{
    public partial class MainPage : IApplicationInterface
    {
        /// <summary>
        /// 应用程序交互数据
        /// </summary>
        public AppInterfaceData AppData = new AppInterfaceData();

        public MainPage()
        {
            InitializeComponent();
        }

        #region IApplicationInterface Members

        public void Globalization(object sender, EventArgs e)
        {

        }

        public string Param
        {
            get
            {
                return string.Empty;
            }
            set
            {
                AppData = value.ToObject<AppInterfaceData>();
            }
        }

        public string WindowID { get; set; }

        #endregion

        #region IDisposable 成员

        public void Dispose()
        {
        }

        #endregion
    }
}
