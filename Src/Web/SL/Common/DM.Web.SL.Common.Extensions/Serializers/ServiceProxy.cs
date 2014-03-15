namespace DM.Web.SL.Common.Extensions.Serializers
{
    /// <summary>
    ///   序列化服务代理
    /// </summary>
    internal class SerializerProxy
    {
        #region Fields

        private static readonly ISerializer InstanceBinarySerialize;
        private static readonly ISerializer InstanceJsonSerialize;
        private static readonly ISerializer InstanceXmlSerialize;

        #endregion Fields

        #region Constructors

        static SerializerProxy()
        {
            InstanceJsonSerialize = new JsonSerializer(); //new JsonSerializer();//需要3.0以上版本
            InstanceBinarySerialize = new BinarySerializer();
            InstanceXmlSerialize = new XmlSerializer();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        ///   获取Binary序列化服务
        /// </summary>
        public static ISerializer BinarySerialize
        {
            get { return InstanceBinarySerialize; }
        }

        /// <summary>
        ///   获取JSON序列化服务
        /// </summary>
        public static ISerializer JsonSerialize
        {
            get { return InstanceJsonSerialize; }
        }

        /// <summary>
        ///   获取Xml序列化服务
        /// </summary>
        public static ISerializer XmlSerialize
        {
            get { return InstanceXmlSerialize; }
        }

        #endregion Properties
    }
}