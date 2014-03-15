using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Mapping.Attributes;
using Environment = NHibernate.Cfg.Environment;

namespace DM.Common.Data
{
    ///<summary>
    /// 多数据库session管理器，通过传入NHIBERNATE配置文件及映射文件来管理SESSION
    ///</summary>
    public sealed class MultiDbSessionManager : IDisposable
    {
        #region 字段

        private static readonly Dictionary<string, ISessionFactory> SessionFactorys = new Dictionary<string, ISessionFactory>();
        private static readonly object Obj = new object();
        private readonly string _dbKey;
        private volatile ISession _session;
        private volatile IStatelessSession _statelessSession;

        #endregion

        #region 构造函数

        ///<summary>
        /// 使用此构造,映射文件和配置要放同一路径
        ///</summary>
        ///<param name="dbKey">系统标识</param>
        ///<param name = "config">配置文件路径</param>
        public MultiDbSessionManager(string dbKey, string config)
            : this(dbKey, null, config, string.Empty, null)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="dbKey">系统标识 </param>
        /// <param name = "config">配置文件路径</param>
        /// <param name = "connstr">连接字符串</param>
        public MultiDbSessionManager(string dbKey, string config, string connstr)
            : this(dbKey, null, config, connstr, null)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="dbKey">系统标识 </param>
        /// <param name = "config">配置文件路径</param>
        /// <param name = "connstr">连接字符串</param>
        /// <param name="myInterceptor"> </param>
        public MultiDbSessionManager(string dbKey, string config, string connstr, IInterceptor myInterceptor)
            : this(dbKey, null, config, connstr, myInterceptor)
        {
        }

        ///<summary>
        ///</summary>
        ///<param name="dbKey">系统标识 </param>
        ///<param name = "assembly">包含实体及非配置映射文件的程序集名</param>
        ///<param name = "config">配置文件路径</param>
        public MultiDbSessionManager(string dbKey, Assembly assembly, string config)
            : this(dbKey, assembly, config, string.Empty, null)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="dbKey">系统标识 </param>
        /// <param name = "assembly">包含实体及非配置映射文件的程序集名</param>
        /// <param name = "config">配置文件路径</param>
        /// <param name = "connstr">连接字符串</param>
        public MultiDbSessionManager(string dbKey, Assembly assembly, string config, string connstr)
            : this(dbKey, assembly, config, connstr, null)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="dbKey">系统标识 </param>
        /// <param name = "assembly">包含实体及非配置映射文件的程序集名</param>
        /// <param name = "config">配置文件路径</param>
        /// <param name = "connstr">连接字符串</param>
        /// <param name = "myInterceptor">自定义拦截器</param>
        public MultiDbSessionManager(string dbKey, Assembly assembly, string config, string connstr, IInterceptor myInterceptor)
        {
            _dbKey = dbKey;
            var sessionFactory = GetSessionFactory(dbKey, assembly, config, connstr);
            _session = myInterceptor != null ? sessionFactory.OpenSession(myInterceptor) : sessionFactory.OpenSession();
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 重置会话工厂（用于切换数据库连接）
        /// </summary>
        public static void ResetSessionFactory(string dbKey)
        {
            lock (Obj)
            {
                if (!SessionFactorys.ContainsKey(dbKey)) return;
                SessionFactorys[dbKey].Close();
                SessionFactorys[dbKey].Dispose();
                SessionFactorys[dbKey] = null;
                SessionFactorys.Remove(dbKey);
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
            return !SessionFactorys.ContainsKey(_dbKey) ? null : (_statelessSession ?? (_statelessSession = SessionFactorys[_dbKey].OpenStatelessSession()));
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

        private static ISessionFactory CreateFactory(string dbKey, Assembly assembly, string config, string connstr)
        {
            lock (Obj)
            {
                var configuration = new Configuration().Configure(config);
                if (!string.IsNullOrEmpty(connstr))
                {
                    configuration.Properties[Environment.ConnectionString] = connstr;
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

                SessionFactorys.Add(dbKey, configuration.BuildSessionFactory());

                return SessionFactorys[dbKey];
            }
        }

        private static ISessionFactory GetSessionFactory(string dbKey, Assembly assembly, string config, string connstr)
        {
            lock (Obj)
            {
                return !SessionFactorys.ContainsKey(dbKey) ? CreateFactory(dbKey, assembly, config, connstr) : SessionFactorys[dbKey];
            }
        }

        #endregion
    }
}