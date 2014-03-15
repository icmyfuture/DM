using System;

namespace DM.Web.SL.Common.Core.App
{
    /// <summary>
    ///   应用参数接口
    /// </summary>
    public interface IApplicationInterface : IDisposable
    {
        #region Properties

        /// <summary>
        ///   程序参数
        /// </summary>
        string Param { get; set; }

        /// <summary>
        ///   窗口标识
        /// </summary>
        string WindowID { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        ///   全球化
        /// </summary>
        void Globalization(object sender, EventArgs e);

        #endregion Methods
    }
}