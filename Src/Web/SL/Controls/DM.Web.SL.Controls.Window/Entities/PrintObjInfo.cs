namespace DM.Web.SL.Controls.Window.Entities
{
    /// <summary>
    ///   打印的内容实体类
    /// </summary>
    public class PrintObjInfo
    {
        #region  构造函数

        #endregion  构造函数

        #region 实体

        #region 私有变量

        #endregion

        #region 属性

        /// <summary>
        ///   打印标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///   打印内容类型 text、html
        /// </summary>
        public string Type { set; get; }

        /// <summary>
        ///   打印的内容
        /// </summary>
        public string Content { set; get; }

        ///<summary>
        ///  打印字样显示
        ///</summary>
        public string ViewPrint { set; get; }

        ///<summary>
        ///  导出字样显示
        ///</summary>
        public string ViewExport { set; get; }

        #endregion

        #endregion
    }
}