using System;

namespace DM.Client.WPF.Controls.NotifyMessage.Enum
{
    /// <summary>
    /// 映射实体
    /// </summary>
    [Serializable]
    public class ErrorTypeMapping
    {
        /// <summary>
        /// 错误类型
        /// </summary>
        public ErrorType ErrorType { get; set; }
        /// <summary>
        /// 解决方案
        /// </summary>
        public string Soluntion { get; set; }
        /// <summary>
        /// 错误原因
        /// </summary>
        public string Reason { get; set; }
    }
}