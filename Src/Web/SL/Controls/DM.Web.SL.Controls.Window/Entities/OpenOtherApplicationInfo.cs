namespace DM.Web.SL.Controls.Window.Entities
{
    /// <summary>
    ///   启动其他程序接口参数
    /// </summary>
    public class OpenOtherApplicationInfo
    {
        #region Constructors

        #endregion Constructors

        #region 私有字段
        private int m_width = 0;
        private int m_height = 0;
        #endregion

        #region Properties

        /// <summary>
        ///   应用程序标示(注意要唯一)
        /// </summary>
        public string ApplicationID { get; set; }

        /// <summary>
        ///   程序参数
        /// </summary>
        public string Param { get; set; }

        /// <summary>
        ///   窗口宽度程序参数
        /// </summary>
        public int Width
        {
            get { return m_width; }
            set { m_width = value; }
        }


        /// <summary>
        ///   窗口高度程序参数
        /// </summary>
        public int Height
        {
            get { return m_height; }
            set { m_height = value; }
        }

        #endregion Properties
    }
}