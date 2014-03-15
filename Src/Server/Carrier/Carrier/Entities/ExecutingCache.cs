using System;
using System.Diagnostics;

namespace Carrier.Entities
{
    ///<summary>
    ///  运行时状态
    ///</summary>
    public struct ExecutingCache
    {
        /// <summary>
        ///   应用标识
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        ///   应用名称
        /// </summary>
        public string AppName { get; set; }

        /// <summary>
        ///   进程实例
        /// </summary>
        public Process Process { get; set; }

        /// <summary>
        ///   句柄指针
        /// </summary>
        public IntPtr Handle { get; set; }
    }
}