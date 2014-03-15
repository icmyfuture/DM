namespace Carrier.Entities
{
    /// <summary>
    ///   地址缓存
    /// </summary>
    public struct ServiceUrl
    {
        /// <summary>
        /// 应用标识
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        ///   程序路径
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        ///   端口
        /// </summary>
        public string Port { get; set; }

        /// <summary>
        ///   路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 启动顺序(避免随机启动，造成服务依赖关系混乱 ）
        /// </summary>
        public int StartIndex { get; set; }
    }
}