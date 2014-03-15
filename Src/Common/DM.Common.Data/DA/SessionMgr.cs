using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Mapping.Attributes;

namespace DM.Common.Data.DA
{
    ///<summary>
    ///  session管理器，通过传入NHIBERNATE配置文件及映射文件来管理SESSION
    ///</summary>
    internal sealed class SessionMgr : IDisposable
    {
        private readonly ISessionFactory _sessionFactory;
        private volatile ISession _session;
        private volatile IStatelessSession _statelessSession;
        private static readonly Dictionary<string, ISessionFactory> HbmcfgKeyFactory = new Dictionary<string, ISessionFactory>();

        ///<summary>
        ///</summary>
        ///<param name = "config">配置文件路径，【注：使用此构造的话映射文件和配置要放同一路径】</param>
        public SessionMgr(string config)
            : this(null, config, string.Empty)
        { }

        /// <summary>
        /// </summary>
        /// <param name = "config">配置文件路径</param>
        /// <param name = "connstr">连接字符串</param>
        public SessionMgr(string config, string connstr)
            : this(null, config, connstr)
        { }

        ///<summary>
        ///</summary>
        ///<param name = "assembly">包含实体及非配置映射文件的程序集名</param>
        ///<param name = "config">配置文件路径</param>
        public SessionMgr(Assembly assembly, string config)
            : this(assembly, config, string.Empty)
        { }

        /// <summary>
        /// </summary>
        /// <param name = "assembly">包含实体及非配置映射文件的程序集名</param>
        /// <param name = "config">配置文件路径</param>
        /// <param name = "connstr">连接字符串</param>
        public SessionMgr(Assembly assembly, string config, string connstr)
            : this(assembly, config, "", connstr)
        { }

        /// <summary>
        /// </summary>
        /// <param name = "assembly">包含实体及非配置映射文件的程序集名</param>
        /// <param name = "config">配置文件路径</param>
        /// <param name="mainDirectory"> </param>
        /// <param name = "connstr">连接字符串</param>
        public SessionMgr(Assembly assembly, string config, string mainDirectory, string connstr)
        {
            _sessionFactory = GetSessionFactory(assembly, config, mainDirectory, connstr);
            _session = _sessionFactory.OpenSession();
        }

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

        public ISessionFactory SessionFactory()
        {
            return _sessionFactory;
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

        private static ISessionFactory GetSessionFactory(Assembly assembly, string config, string mainDirectory, string connstr)
        {
            string key = config;
            if (!string.IsNullOrEmpty(connstr))
            {
                key += "#";
            }
            else
            {
                key += "#" + connstr;
            }
            if (!HbmcfgKeyFactory.ContainsKey(key))
                HbmcfgKeyFactory[key] = CreateFactory(assembly, config, mainDirectory, connstr);

            return HbmcfgKeyFactory[key];
        }

        private static ISessionFactory CreateFactory(Assembly assembly, string config, string mainDirectory, string connstr)
        {
            var configuration = new Configuration().Configure(config);
            if (!string.IsNullOrEmpty(connstr))
            {
                configuration.Properties[NHibernate.Cfg.Environment.ConnectionString] = connstr;
            }

            if (assembly == null)
            {
                configuration.AddDirectory(string.IsNullOrEmpty(mainDirectory)
                                               ? new DirectoryInfo(Path.GetDirectoryName(config))
                                               : new DirectoryInfo(Path.GetDirectoryName(mainDirectory)));
            }
            else
            {
                //使用mapping.attributes时用这个
                configuration.AddInputStream(HbmSerializer.Default.Serialize(assembly));
            }
            return configuration.BuildSessionFactory();
        }
    }
}