// -------------------------
// .Author : jiyi
// .Email: memory-jy@hotmail.com
// -------------------------
namespace AI3.Common.Core.Utility.DataTransform
{
    /// <summary>
    /// 文件大小单位转换
    /// </summary>
    public class FileSizeTransferHelper
    {
        public static readonly FileSizeTransferHelper Instance = new FileSizeTransferHelper();
        private FileSizeTransferHelper()
        {}

        /// <summary>
        /// 字节转换为K
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public double TransferBToKb(double size)
        {
            return double.Parse((1.0 * size / 1024).ToString("F2"));
        }

        /// <summary>
        /// K转换为字节
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public double TransferKbToB(double size)
        {
            return size*1024;
        }

        /// <summary>
        /// K转换为M
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public double TransferKbToM(double size)
        {
            return double.Parse((1.0*size/1024).ToString("F2"));
        }

        /// <summary>
        /// M转换为K
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public double TranferMtoKb(double size)
        {
            return size*1024;
        }

        /// <summary>
        /// M转换为G
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public double TransferMToG(double size)
        {
            return double.Parse((1.0 * size / 1024).ToString("F2"));
        }

        /// <summary>
        /// G转换为M
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public double TransferGToM(double size)
        {
            return size*1024;
        }

        /// <summary>
        /// 字节转换为G
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public double TransferBToG(double size)
        {
            double k = TransferBToKb(size);
            double m = TransferKbToM(k);
            string val = TransferMToG(m).ToString("F2");
            return double.Parse(val);
        }

        /// <summary>
        /// 转换文件大小为最接近的单位
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public string TransferSize(double size)
        {
            double val = TransferBToKb(size);
            if (val > 1024)
            {
                val = TransferKbToM(val);
                if (val > 1024)
                {
                    val = TransferBToG(size);
                    return val + " GB";
                }
                return val + " MB";
            }
            return val + " KB";
        }
    }
}