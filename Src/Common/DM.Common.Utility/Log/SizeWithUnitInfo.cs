using System;

namespace DM.Common.Utility.Log
{
    /// <summary>
    /// 带单位的大小信息
    /// </summary>
    [Serializable]
    public class SizeWithUnitInfo
    {
        /// <summary>
        /// 换算后大小(如果没有指定Bytes的值，则按Size和Unit进行换算，此换算会丢失精度)
        /// </summary>
        public double Size;

        /// <summary>
        /// 字节数(用于计算，防止换算过程中数据失真)
        /// </summary>
        public long Bytes
        {
            get { return ByteConverter.ConvertToByteCountFromSizeAndUnit(this); }
        }

        /// <summary>
        /// 单位
        /// </summary>
        public ByteUnit Unit;
    }
}