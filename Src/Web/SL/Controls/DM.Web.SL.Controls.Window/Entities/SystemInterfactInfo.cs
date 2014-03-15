#region Import

using System.Collections.Generic;

#endregion

namespace AI3.Common.Controls.Window.Entities
{
    /// <summary>
    ///   SystemInterfactInfo实体类
    /// </summary>
    public class SystemInterfactInfo
    {
        #region  构造函数

        #endregion  构造函数

        #region 实体

        #region 私有变量

        #endregion

        #region 属性

        /// <summary>
        ///   系统ID
        /// </summary>
        public string SystemID { get; set; }

        /// <summary>
        ///   ExportConfigModel
        /// </summary>
        public string ExportConfigModel { get; set; }


        /// <summary>
        ///   ExportDefine
        /// </summary>
        public List<ExportDefineInfo> ExportDefine { get; set; }

        #endregion

        #endregion
    }
}