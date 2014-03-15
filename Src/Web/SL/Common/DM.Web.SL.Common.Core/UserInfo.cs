using System;

namespace DM.Web.SL.Common.Core
{
    /// <summary>
    ///   实体类
    /// </summary>
    public class UserInfo
    {
        #region Constructors

        /// <summary>
        ///   构造函数
        /// </summary>
        public UserInfo()
        {
            UserID = "-1";
            Language = new LanguageInfo();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        ///   用户语言
        /// </summary>
        public LanguageInfo Language { get; set; }

        /// <summary>
        ///   最后一次验证有效时间
        /// </summary>
        public DateTime LastVerifyEffectiveTime { get; set; }

        /// <summary>
        ///   用户ID
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        ///   用户名
        /// </summary>
        public string NickName { get; set; }

        #endregion Properties
    }
}