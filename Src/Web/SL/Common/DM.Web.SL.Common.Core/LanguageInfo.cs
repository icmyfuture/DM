using System.Xml.Serialization;

namespace DM.Web.SL.Common.Core
{
    #region Imports

    

    #endregion

    /// <summary>
    ///   语言信息类
    /// </summary>
    public class LanguageInfo
    {
        /// <summary>
        ///   显示语言名称
        /// </summary>
        [XmlAttribute(AttributeName = "ShowName")]
        public string ShowName { get; set; }

        /// <summary>
        ///   语言标示
        /// </summary>
        [XmlAttribute(AttributeName = "LanguageKey")]
        public string LanguageKey { get; set; }

        /// <summary>
        ///   当前语言使用的字体
        /// </summary>
        [XmlAttribute(AttributeName = "FontFamily")]
        public string FontFamily { get; set; }
    }
}