using System;

namespace DM.Common.Service.Entities
{
    /// <summary>
    /// 请求模型
    /// </summary>
    [Serializable]
    public class RequestModel
    {       
        /// <summary>
        /// 命令名称
        /// </summary>
        public string CommandName { get; set; }

        /// <summary>
        /// 命令参数
        /// </summary>
        public string Parameters { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 用户IP
        /// </summary>
        public string UserIp { get; set; }

        /// <summary>
        /// 项目编号
        /// </summary>
        public long ProjectId { get; set; }

        /// <summary>
        /// 软件类型
        /// </summary>
        public string SoftType { get; set; }

        /// <summary>
        /// 会话编号
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// 服务IP
        /// </summary>
        public string ServerIp { get; set; }
    }
}