namespace AI3.Common.Controls.Window.Entities
{
    /// <summary>
    ///   ExportRequestWrapperInfo实体类
    /// </summary>
    public class ExportRequestWrapperInfo
    {
        #region  构造函数

        #endregion  构造函数

        #region 实体

        #region 私有变量

        #endregion

        #region 属性

        /// <summary>
        ///   TargetSystemID
        /// </summary>
        public string TargetSystemID { get; set; }

        /// <summary>
        ///   FlowName
        /// </summary>
        public string FlowName { get; set; }

        /// <summary>
        ///   Port
        /// </summary>
        public string Port { get; set; }

        /// <summary>
        ///   出库请求协议 DCMExportRequest 的XML
        /// </summary>
        public string exportRequest { get; set; }

        #endregion

        #endregion
    }
}