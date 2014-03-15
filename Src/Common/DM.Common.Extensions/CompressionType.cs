namespace DM.Common.Extensions
{
    /// <summary>
    /// 压缩方式。
    /// </summary>
    public enum CompressionType
    {
        /// <summary>
        /// GZip 压缩格式
        /// </summary>
        GZip,
        /// <summary>
        /// BZip2 压缩格式
        /// </summary>
        BZip2,
        /// <summary>
        /// Zip 压缩格式
        /// </summary>
        Zip,
        /// <summary>
        /// ZLib 压缩格式
        /// </summary>
        ZLib,
    }
}