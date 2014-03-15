namespace DM.Client.WPF.Controls.NotifyMessage.Enum
{
    public enum ErrorType
    {
        /// <summary>
        /// DCMP 流程执行失败
        /// </summary>
        DCMPFlowExecutingFailed,

        /// <summary>
        /// 数据超长
        /// </summary>
        DataLengthOuterDesignLength,

        /// <summary>
        /// 素材类型不被支持
        /// </summary>
        IngestTypeNotSurported,

        /// <summary>
        /// 必填字段缺省
        /// </summary>
        DataRequirdFiledisNull,

        /// <summary>
        /// 数据类型错误
        /// </summary>
        DataTypeIsError,

        /// <summary>
        /// 所填字段不被当前的数据格式支持
        /// </summary>
        FiledValueIsNotDataType,

        /// <summary>
        /// 错误的数据格式
        /// </summary>
        IllegalValue,
        /// <summary>
        /// 没有错误
        /// </summary>
        Null,
        /// <summary>
        /// 网络异常
        /// </summary>
        NetworkException



    }
}