namespace DM.Web.SL.Controls.Window.Entities
{
    /// <summary>
    ///   应用程序实体
    /// </summary>
    public class ApplicationInfo
    {
        #region Constructors

        /// <summary>
        ///   构造函数
        /// </summary>
        public ApplicationInfo()
        {
            OpenNumber = 1;
            OrderNo = 100;
        }

        /// <summary>
        ///   构造函数
        /// </summary>
        /// <param name = "applicationID">应用程序标示</param>
        /// <param name = "typeName">类型名称</param>
        /// <param name = "xapName">Xap文件名</param>
        /// <param name = "applicationName">标题</param>
        /// <param name = "appIconPath">图标</param>
        /// <param name = "isResizable">是否可改变大小</param>
        public ApplicationInfo( string applicationID, string typeName, string xapName, string applicationName, string appIconPath, bool isResizable )
            : this()
        {
            ApplicationID = applicationID;
            TypeName = typeName;
            XapName = xapName;
            ApplicationName = applicationName;
            AppIconPath = appIconPath;
            IsResizable = isResizable;
        }

        /// <summary>
        ///   构造函数
        /// </summary>
        /// <param name = "applicationID">应用程序标示</param>
        /// <param name = "typeName">类型名称</param>
        /// <param name = "xapName">Xap文件名</param>
        /// <param name = "applicationName">标题</param>
        /// <param name = "appIconPath">图标</param>
        /// <param name = "isResizable">是否可改变大小</param>
        /// <param name = "openNumber">可启动个数</param>
        public ApplicationInfo( string applicationID, string typeName, string xapName, string applicationName, string appIconPath, bool isResizable, int openNumber )
        {
            ApplicationID = applicationID;
            TypeName = typeName;
            XapName = xapName;
            ApplicationName = applicationName;
            AppIconPath = appIconPath;
            IsResizable = isResizable;
            OpenNumber = openNumber;
            OrderNo = 100;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        ///   应用列表图标
        /// </summary>
        public string AppIconPath { get; set; }

        /// <summary>
        ///   应用程序标示(注意要全程序集唯一，安装时请判断是否重复)
        /// </summary>
        public string ApplicationID { get; set; }

        /// <summary>
        ///   应用名称
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        ///   类型
        /// </summary>
        public object Instance { get; set; }

        /// <summary>
        ///   是否是打开IE窗口
        /// </summary>
        public bool IsNewIeWindow { get; set; }

        /// <summary>
        ///   是否可变
        /// </summary>
        public bool IsResizable { get; set; }

        /// <summary>
        ///   是否可最大化
        /// </summary>
        public bool CanMaxWindow { get; set; }

        /// <summary>
        ///   是否在列表中显示
        /// </summary>
        public bool IsShowAppList { get; set; }

        /// <summary>
        ///   启动此程序的个数
        /// </summary>
        public int OpenNumber { get; set; }

        /// <summary>
        ///   程序参数
        /// </summary>
        public string Param { get; set; }

        /// <summary>
        ///   服务地址(默认取DCMP服务，只有特定的服务自己调用WS服务)
        /// </summary>
        public string ServiceAddress { get; set; }

        /// <summary>
        ///   类型名称
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        ///   程序窗口标题
        /// </summary>
        public string WindowCaption { get; set; }

        /// <summary>
        ///   窗口图标
        /// </summary>
        public string WindowIconPath { get; set; }

        /// <summary>
        ///   XAP名称
        /// </summary>
        public string XapName { get; set; }

        /// <summary>
        /// 窗口的宽
        /// </summary>
        public long WindowWidth { get; set; }

        /// <summary>
        /// 窗口的高
        /// </summary>
        public long WindowHeight { get; set; }
        
        /// <summary>
        /// 是否替换原窗口
        /// </summary>
        public bool IsReplaceOrgWindow { get; set; }

        ///<summary>
        /// 父级应用名称
        ///</summary>
        public string ParentID { get; set; }


        ///<summary>
        /// 排序字段序列号,默认为100,在后面 根据不同的产品自己配置
        ///</summary>
        public int OrderNo { get; set; }

        #endregion Properties
    }
}