namespace DM.Web.SL.Common.Service.Entities
{
    /// <summary>
    /// 相应模型
    /// </summary>
    public class ResponseModel
    {
        /// <summary>
        ///   请求返回字符数据
        /// </summary>
        public string ResultData { get; set; }

        /// <summary>
        ///   请求返回状态
        /// </summary>
        public ResponseStateDefine State { get; set; }
    }
}