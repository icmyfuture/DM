using System.Configuration;
using System.Net;
using System.Net.Sockets;

namespace DM.Common.Service.Helpers
{
    public class ConfigHelper
    {
        /// <summary>
        /// 模块名称
        /// </summary>
        public static string ModuleName = "DM.Service";

        /// <summary>
        /// 获取的是服务器端的IP
        /// </summary>
        /// <returns></returns>
        public static string GetServerIp()
        {
            IPAddress[] arrIPAddresses = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress ip in arrIPAddresses)
            {
                if (ip.AddressFamily.Equals(AddressFamily.InterNetwork))
                {
                    return ip.ToString();
                }
            }
            return "127.0.0.1";
        }

        /// <summary>
        /// 获取AppSettings节点的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetAppSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        /// <summary>
        /// 是否保存详细服务交互数据到日志文件
        /// </summary>
        public static bool SaveInteractivData { get; set; }
    }
}