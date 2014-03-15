using System;
using System.Collections.Generic;
using System.Linq;

namespace DM.Common.Utility.Log
{
    /// <summary>
    /// 字节转换帮助类
    /// </summary>
    internal class ByteConverter
    {
        #region 方法

        #region 覆盖字节
        /// <summary>
        /// 覆盖字节(从左边索引0开始覆盖)
        /// </summary>
        /// <param name="source">被覆盖字节</param>
        /// <param name="direct">指定的字节来源</param>
        public static void ReplaceByteArray(byte[] source, byte[] direct)
        {
            Array.Copy(direct, 0, source, 0, direct.Length);
        }


        /// <summary>
        /// 覆盖字节(从指定起始位置覆盖)
        /// </summary>
        /// <param name="source">被覆盖字节</param>
        /// <param name="direct">指定的字节来源</param>
        /// <param name="sourceOffset">被覆盖字节开始位置</param>
        public static void ReplaceByteArray(byte[] source, byte[] direct, int sourceOffset)
        {
            Array.Copy(direct, 0, source, sourceOffset, direct.Length);
        }

        /// <summary>
        /// 覆盖字节(从指定起始位置覆盖指定长度)
        /// </summary>
        /// <param name="source">被覆盖字节</param>
        /// <param name="direct">指定的字节来源</param>
        /// <param name="sourceOffset">被覆盖字节开始位置</param>
        /// <param name="directOffset">指定的字节来源开始位置</param>
        /// <param name="replaceLength">置覆长度</param>
        public static void ReplaceByteArray(byte[] source, byte[] direct, int sourceOffset, int directOffset, int replaceLength)
        {
            Array.Copy(direct, directOffset, source, sourceOffset, replaceLength);
        }
        #endregion

        #region 截取字节数组
        /// <summary>
        /// 截取字节数组
        /// </summary>
        /// <param name="input">待截取字节数组</param>
        /// <param name="offset">起始位置</param>
        /// <param name="length">截取长度</param>
        /// <returns></returns>
        public static byte[] CutByteArray(byte[] input, int offset, int length)
        {
            if (input != null && input.Length > 0 && offset >= 0 && length > 0)
            {
                var returnByteArray = new byte[length];

                Array.Copy(input, offset, returnByteArray, 0, length);

                return returnByteArray;
            }
            return new byte[0];
        }

        #endregion

        #region 反转字节数组
        /// <summary>
        /// 反转字节数组
        /// </summary>
        /// <param name="input">待反转字节数组</param>
        public static byte[] ReverseByteArray(byte[] input)
        {
            if (input != null && input.Length > 0)
            {
                Array.Reverse(input);

                return input;
            }
            return new byte[0];
        }

        #endregion

        #region 统计几个字节数组总长度
        /// <summary>
        /// 统计几个字节数组总长度
        /// </summary>
        /// <param name="byteArray">字节数组集合</param>
        /// <returns></returns>
        public static long GetLenthOfByteArray(List<byte[]> byteArray)
        {
            int len = 0;

            if (byteArray != null && byteArray.Count > 0)
            {
                len += byteArray.Sum(t => t.Length);
            }

            return len;
        }
        #endregion

        #region 合并几个字节数组

        /// <summary>
        /// 合并两个字节数组
        /// </summary>
        /// <param name="souce1">字节数组1</param>
        /// <param name="souce2">字节数组2</param>
        /// <returns></returns>
        public static byte[] MergeByteArray(byte[] souce1, byte[] souce2)
        {
            if (souce1 != null && souce2 != null)
            {
                var byteArray = new List<byte[]> {souce1, souce2};

                return MergeByteArray(byteArray);
            }
            return new byte[0];
        }

        /// <summary>
        /// 合并几个字节数组
        /// </summary>
        /// <param name="byteArray">字节数组集合</param>
        /// <returns></returns>
        public static byte[] MergeByteArray(List<byte[]> byteArray)
        {
            if (byteArray != null && byteArray.Count > 0)
            {
                int mergedLength = 0;

                var direct = new byte[GetLenthOfByteArray(byteArray)];

                foreach (byte[] t in byteArray)
                {
                    Array.Copy(t, 0, direct, mergedLength, t.Length);

                    mergedLength += t.Length;
                }

                return direct;
            }
            return new byte[0];
        }

        #endregion

        #region 转换为带单位大小的字符串(例如：42M、2.3G)
        /// <summary>
        /// 转换为带单位大小的字符串(例如：42M、2.3G)
        /// </summary>
        /// <param name="byteCount">字节数</param>
        /// <returns></returns>
        public static string ConvertToSizeUnitStringFromByteCount(long byteCount)
        {
            SizeWithUnitInfo swui = ConvertToSizeWithUnitInfoFromByteCount(byteCount);

            return swui.Size + " " + GetUnitName(swui.Unit);
        }
        #endregion

        #region 转换为带单位大小实体
        /// <summary>
        /// 转换为带单位大小实体
        /// </summary>
        /// <param name="byteCount">字节数</param>
        /// <returns></returns>
        public static SizeWithUnitInfo ConvertToSizeWithUnitInfoFromByteCount(long byteCount)
        {
            var swui = new SizeWithUnitInfo();

            if (byteCount >= 1073741824)
            {
                swui.Size = double.Parse(String.Format("{0:##.##}", byteCount / 1073741824));
                swui.Unit = ByteUnit.GB;
            }
            else if (byteCount >= 1048576)
            {
                swui.Size = double.Parse(String.Format("{0:##.##}", byteCount / 1048576));
                swui.Unit = ByteUnit.MB;
            }
            else if (byteCount >= 1024)
            {
                swui.Size = double.Parse(String.Format("{0:##.##}", byteCount / 1024));
                swui.Unit = ByteUnit.KB;
            }
            else if (byteCount > 0 && byteCount < 1024)
            {
                swui.Size = byteCount;
                swui.Unit = ByteUnit.B;
            }

            return swui;
        }
        #endregion

        #region 将带单位的大小转换为字节大小
        /// <summary>
        /// 将带单位的大小转换为字节大小
        /// </summary>
        /// <param name="swui">带单位的大小信息</param>
        /// <returns></returns>
        public static long ConvertToByteCountFromSizeAndUnit(SizeWithUnitInfo swui)
        {
            if (swui != null)
            {
                switch (swui.Unit)
                {
                    case ByteUnit.B:
                        {
                            return (long)swui.Size;
                        }
                    case ByteUnit.KB:
                        {
                            return (long)swui.Size * 1024;
                        }
                    case ByteUnit.MB:
                        {
                            return (long)swui.Size * 1048576;
                        }
                    case ByteUnit.GB:
                        {
                            return (long)swui.Size * 1073741824;
                        }
                    default:
                        {
                            throw new Exception("不支持的单位换算!");
                        }
                }
            }
            return 0;
        }

        #endregion

        #region 将字节大小、单位转换为带单位的大小
        /// <summary>
        /// 换算单位
        /// </summary>
        /// <param name="byteCount">大小</param>
        /// <param name="by">单位</param>
        /// <returns></returns>
        public static SizeWithUnitInfo ConvertToSizeWithUnitInfoFromByteCountAndUnit(long byteCount, ByteUnit by)
        {
            var swui = new SizeWithUnitInfo();

            switch (by)
            {
                case ByteUnit.B:
                    {
                        swui.Size = byteCount;
                        swui.Unit = ByteUnit.B;
                        break;
                    }
                case ByteUnit.KB:
                    {
                        swui.Size = double.Parse(String.Format("{0:##.##}", byteCount / 1024));
                        swui.Unit = ByteUnit.KB;
                        break;
                    }
                case ByteUnit.MB:
                    {
                        swui.Size = double.Parse(String.Format("{0:##.##}", byteCount / 1048576));
                        swui.Unit = ByteUnit.MB;
                        break;
                    }
                case ByteUnit.GB:
                    {
                        swui.Size = double.Parse(String.Format("{0:##.##}", byteCount / 1073741824));
                        swui.Unit = ByteUnit.GB;
                        break;
                    }
                default:
                    {
                        throw new Exception("不支持的单位换算!");
                    }
            }


            return swui;
        }
        #endregion

        #region 根据字节数获取字节单位
        /// <summary>
        /// 根据字节数获取字节单位
        /// </summary>
        /// <param name="byteCount">字节数</param>
        /// <returns></returns>
        public static ByteUnit ConvertToUnitFromByteCount(long byteCount)
        {
            var size = ByteUnit.B;

            if (byteCount >= 1073741824)
                size = ByteUnit.GB;
            else if (byteCount >= 1048576)
                size = ByteUnit.MB;
            else if (byteCount >= 1024)
                size = ByteUnit.KB;
            else if (byteCount > 0 && byteCount < 1024)
                size = ByteUnit.B;

            return size;
        }
        #endregion

        #region 根据字节单位获取对应名称
        /// <summary>
        /// 根据字节单位获取对应名称
        /// </summary>
        /// <param name="unit">单位</param>
        /// <returns></returns>
        public static string GetUnitName(ByteUnit unit)
        {
            switch (unit)
            {
                case ByteUnit.B:
                    {
                        return "B";
                    }

                case ByteUnit.GB:
                    {
                        return "GB";
                    }

                case ByteUnit.KB:
                    {
                        return "KB";
                    }

                case ByteUnit.MB:
                    {
                        return "MB";
                    }
                default:
                    {
                        throw new Exception("未知单位!");
                    }
            }
        }
        #endregion

        #endregion
    }
}