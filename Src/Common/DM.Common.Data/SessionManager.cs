using System;
using System.IO;
using System.Reflection;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Mapping.Attributes;

namespace DM.Common.Data
{
    ///<summary>
    ///  session管理器，通过传入NHIBERNATE配置文件及映射文件来管理SESSION
    ///</summary>
    public sealed class SessionManager : IDisposable
    {
        #region 字段

        private static ISessionFactory _sessionFactory;
        private volatile ISession _session;
        private volatile IStatelessSession _statelessSession;
        private static readonly object Obj = new object();

        #endregion

        #region 构造函数

        ///<summary>
        /// 使用此构造,映射文件和配置要放同一路径
        ///</summary>
        ///<param name = "config">配置文件路径</param>
        public SessionManager(string config)
            : this(null, config, string.Empty)
        { }

        /// <summary>
        /// </summary>
        /// <param name = "config">配置文件路径</param>
        /// <param name = "connstr">连接字符串</param>
        public SessionManager(string config, string connstr)
            : this(null, config, connstr, null)
        { }

        /// <summary>
        /// </summary>
        /// <param name = "config">配置文件路径</param>
        /// <param name = "connstr">连接字符串</param>
        /// <param name="myInterceptor"> </param>
        public SessionManager(string config, string connstr, IInterceptor myInterceptor)
            : this(null, config, connstr, myInterceptor)
        { }

        ///<summary>
        ///</summary>
        ///<param name = "assembly">包含实体及非配置映射文件的程序集名</param>
        ///<param name = "config">配置文件路径</param>
        public SessionManager(Assembly assembly, string config)
            : this(assembly, config, string.Empty, null)
        { }

        /// <summary>
        /// </summary>
        /// <param name = "assembly">包含实体及非配置映射文件的程序集名</param>
        /// <param name = "config">配置文件路径</param>
        /// <param name = "connstr">连接字符串</param>
        public SessionManager(Assembly assembly, string config, string connstr)
            : this(assembly, config, connstr, null)
        { }

        /// <summary>
        /// </summary>
        /// <param name = "assembly">包含实体及非配置映射文件的程序集名</param>
        /// <param name = "config">配置文件路径</param>
        /// <param name = "connstr">连接字符串</param>
        /// <param name = "myInterceptor">自定义拦截器</param>
        public SessionManager(Assembly assembly, string config, string connstr, IInterceptor myInterceptor)
        {
            _sessionFactory = GetSessionFactory(assembly, config, connstr);
            _session = myInterceptor != null ? _sessionFactory.OpenSession(myInterceptor) : _sessionFactory.OpenSession();
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 重置会话工厂（用于切换数据库连接）
        /// </summary>
        public static void ResetSessionFactory()
        {
            lock (Obj)
            {
                _sessionFactory.Close();
                _sessionFactory.Dispose();

                _sessionFactory = null;
            }
        }

        ///<summary>
        ///  ISession
        ///</summary>
        ///<returns></returns>
        public ISession Data()
        {
            return _session;
        }

        /// <summary>
        /// IStatelessSession
        /// </summary>
        /// <returns></returns>
        public IStatelessSession DataStateless()
        {
            return _statelessSession ?? (_statelessSession = _sessionFactory.OpenStatelessSession());
        }

        #endregion

        #region IDisposable Members

        ///<summary>
        ///  关闭session
        ///</summary>
        public void Dispose()
        {
            if (_session == null)
            {
                return;
            }
            _session.Close();
        }

        #endregion

        #region 私有方法

        private static ISessionFactory CreateFactory(Assembly assembly, string config, string connstr)
        {
            lock (Obj)
            {
                var configuration = new Configuration().Configure(config);
                if (!string.IsNullOrEmpty(connstr))
                {
                    configuration.Properties[NHibernate.Cfg.Environment.ConnectionString] = connstr;
                }

                if (assembly == null)
                {
                    configuration.AddDirectory(new DirectoryInfo(Path.GetDirectoryName(config)));
                }
                else
                {
                    //使用mapping.attributes时用这个
                    configuration.AddInputStream(HbmSerializer.Default.Serialize(assembly));
                }
                return configuration.BuildSessionFactory();
            }
        }

        private static ISessionFactory GetSessionFactory(Assembly assembly, string config, string connstr)
        {
            lock (Obj)
            {
                return _sessionFactory ?? CreateFactory(assembly, config, connstr);
            }
        }

        #endregion
    }
}