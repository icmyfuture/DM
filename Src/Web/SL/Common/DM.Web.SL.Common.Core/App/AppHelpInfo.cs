using System.Xml.Serialization;

namespace DM.Web.SL.Common.Core.App
{
    /// <summary>
    /// 实体类
    /// </summary>
    public class AppHelpeInfo
    {
        #region  构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public AppHelpeInfo()
        {
        }
        #endregion  构造函数

        #region 实体

        #region 私有变量

        #endregion


        #region 属性

        #region  应用id

        /// <summary>
        /// 应用id
        /// </summary>
        private string m_applicationId = string.Empty;

        /// <summary>
        /// 应用id
        /// </summary>
        [XmlAttribute(AttributeName = "ApplicationId")]
        public string ApplicationId
        {
            get
            {
                return m_applicationId;
            }
            set
            {
                m_applicationId = value;
            }
        }

        #endregion

        #region  应用对应的帮助地址

        /// <summary>
        /// 应用对应的帮助地址
        /// </summary>
        private string m_appHelpUrl = string.Empty;

        /// <summary>
        /// 应用对应的帮助地址 /help/source/WebHelp_cn/index.htm 支持语言通配符和应用通配符
        /// </summary>
        [XmlAttribute(AttributeName = "AppHelpUrl")]
        public string AppHelpUrl
        {
            get
            {
                return m_appHelpUrl;
            }
            set
            {
                m_appHelpUrl = value;
            }
        }

        #endregion

        #endregion

        #endregion
    }
}
