using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;
using DM.Common.Extensions;
using DM.Common.Service.Entities;
using DM.Common.Service.Helpers;
using DM.Common.Service.Interfaces;
using DM.Common.Utility.Log;

namespace DM.Common.Service.Services
{
    /// <summary>
    /// 服务提供者工厂
    /// </summary>
    /// <typeparam name="TDispatcher"></typeparam>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false, Name = "Command_Service")]
    public class CommandService<TDispatcher> : ICommandService where TDispatcher : IRequestDispatcher
    {
        /// <summary>
        /// 由于ServiceProviderFactory会为每次调用都实例化一次,这里必须用静态对象,以保证_dispatcher只有一个实例
        /// </summary>
        private static readonly TDispatcher Dispatcher;

        static CommandService()
        {
            Dispatcher = Activator.CreateInstance<TDispatcher>();
        }

        public string Invoke(string request)
        {
            try
            {
                string requestGuid = Guid.NewGuid().ToString();

                //0.解压缩请求报文,反序列化请求对象
                var rm = request.ToDeCompressedObject<RequestModel>();

                //完善参数
                #region 获取WCF调用客户端IP
                //提供方法执行的上下文环境
                OperationContext context = OperationContext.Current;

                //获取传进的消息属性
                MessageProperties properties = context.IncomingMessageProperties;

                //获取消息发送的远程终结点IP和端口
                var endpoint = properties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;

                if (endpoint != null) rm.UserIp = endpoint.Address;

                #endregion

                //打印交互数据
                if (ConfigHelper.SaveInteractivData)
                {
                    LogHelper.Info(ConfigHelper.ModuleName, string.Format("[#{3}:{2}:{1}] Request => \r\n{0}\r\n", rm.ToJson(), requestGuid, rm.UserIp, Thread.CurrentThread.ManagedThreadId));
                }

                //1.调用命令处理程序,压缩响应报文,返回结果
                ResponseModel responseM = Dispatcher.Dispatch(rm);

                //打印交互数据
                if (ConfigHelper.SaveInteractivData)
                {
                    LogHelper.Info(ConfigHelper.ModuleName, string.Format("[#{3}:{2}:{1}] Response => \r\n{0}\r\n", responseM.ToJson(), requestGuid, rm.UserIp, Thread.CurrentThread.ManagedThreadId));
                }

                return responseM.ToCompressedStr();
            }
            catch (Exception ex)
            {
                LogHelper.Fatal(ConfigHelper.ModuleName, ex);

                return new ResponseModel { State = ResponseStateDefine.Failed }.ToCompressedStr();
            }
        }
    }
}