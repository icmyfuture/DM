using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.SqlCommand;
using System.Collections;
using NHibernate.Engine;
using System.Data;
using NHibernate.SqlTypes;
using NHibernate.Type;

namespace DM.Common.Data.DA
{
    /// <summary>
    /// 封装数据库操作，此控制类只支持一个数据库连接；每次New这个类都是一个全新的session；
    /// 资源释放，建议使用using 或 调用Dispose接口；
    /// 数据库事务操作请调用：BeginTransaction、CommitTransaction、RollbackTransaction 方法。
    /// 如果要同时连接多个数据库，那么可以New多个类；多个类的事务操作，建议使用 MS 的 TransactionScope；
    /// 使用TransactionScope 需要开启 分布式数据库事务【MSDTC】服务
    /// 如果是Oracle数据库，还需要 Oracle 分布式数据库事务服务 【OracleMTSRecoveryService】
    /// </summary>
    public class DAControl : IDisposable
    {
        #region 字段

        private readonly ISession _session;
        private ITransaction _tranaction;
        private SessionMgr _sessionManager;

        #endregion

        #region 内部构造函数

        /// <summary>
        /// 不对外公布，只能通过DAService构造
        /// </summary>
        /// <param name="hbmCfgPath">Nhibernate配置文件</param>
        /// <param name="connStr">连接字符串</param>
        internal DAControl(string hbmCfgPath, string connStr)
        {
            _sessionManager = new SessionMgr(hbmCfgPath, connStr);
            _session = _sessionManager.Data();
        }

        /// <summary>
        /// 不对外公布，只能通过DAProxy构造
        /// </summary>
        /// <param name="hbmCfgPath">Nhibernate配置文件</param>
        /// <param name="mainDirectory"> </param>
        /// <param name="connStr">连接字符串</param>
        internal DAControl(string hbmCfgPath, string mainDirectory, string connStr)
        {
            _sessionManager = new SessionMgr(null, hbmCfgPath, mainDirectory, connStr);
            _session = _sessionManager.Data();
        }

        #endregion

        #region【方法】[公共] >>>> BeginTransaction >>>> 开始事务
        /// <summary>
        /// 开始事务
        /// </summary>
        public void BeginTransaction()
        {
            _tranaction = _session.BeginTransaction();
        }
        #endregion
        #region【方法】[公共] >>>> CommitTransaction >>>> 提交事务
        /// <summary>
        /// 提交事务
        /// </summary>
        public void CommitTransaction()
        {
            _tranaction.Commit();
        }
        #endregion
        #region【方法】[公共] >>>> RollbackTransaction >>>> 回滚事务
        /// <summary>
        /// 
        /// </summary>
        public void RollbackTransaction()
        {
            _tranaction.Rollback();
        }
        #endregion

        #region【方法】[公共] >>>> Save<T> >>>> 添加一个对象

        ///<summary>
        ///  插入数据
        ///</summary>
        ///<param name = "item">实体</param>
        ///<typeparam name = "T">实体类型</typeparam>
        ///<returns>增加成功后的实体</returns>
        public T Save<T>(T item) where T : class
        {
            object obj = _session.Merge(typeof(T).Name, item);
            _session.Flush();
            return (T)obj;
        }

        #endregion
        #region【方法】[公共] >>>> Save >>>> 添加一个对象
        /// <summary>
        /// 添加一个对象
        /// </summary>
        /// <param name="obj">对象</param>
        public void Save(object obj)
        {
            _session.Save(obj);
            _session.Flush();
        }
        #endregion
        #region【方法】[公共] >>>> Save >>>> 批量添加一组对象
        /// <summary>
        /// 批量添加一组对象
        /// </summary>
        /// <param name="objs">对象数组</param>
        public void Save(List<object> objs)
        {
            foreach (object obj in objs)
            {
                _session.Save(obj);
            }
            _session.Flush();
        }
        #endregion

        #region【方法】[公共] >>>> Update<T> >>>> 修改一个对象
        ///<summary>
        ///  更新数据
        ///</summary>
        ///<param name = "item">实体</param>
        ///<param name = "key">实体标识</param>
        ///<typeparam name = "T">实体类型</typeparam>
        ///<typeparam name = "TK">标识类型</typeparam>
        ///<returns>修改完成后的实体</returns>
        public T Update<T, TK>(T item, TK key)
        {
            _session.Update(item, key);
            _session.Flush();
            return item;
        }
        #endregion
        #region【方法】[公共] >>>> Update >>>> 批量修改一组对象
        /// <summary>
        /// 批量修改一组对象
        /// </summary>
        /// <param name="objs">对象数组</param>
        public void Update(object[] objs)
        {
            foreach (object obj in objs)
            {
                _session.Update(obj);
            }
            _session.Flush();
        }
        #endregion
        #region【方法】[公共] >>>> Update >>>> 修改一个对象
        /// <summary>
        /// 修改一个对象
        /// </summary>
        /// <param name="obj">对象</param>
        public void Update(object obj)
        {
            _session.Update(obj);
            _session.Flush();
        }

        #endregion
        #region 【方法】[公共] >>>> Update >>>> 修改一个执行HQL语句
        public int Update(string hql, object[] values = null)
        {
            var query = _session.CreateQuery(hql);
            if (values != null && values.Length > 0)
            {
                IList listParamName = GetHqlParamName(hql);

                if (listParamName.Count != values.Length)
                {
                    string sError = string.Format("Parametric variable is wrong. Param count required:{0}  Param count recieved:{1}", listParamName.Count, values.Length);
                    throw new Exception(sError);
                }
                for (int i = 0; i < values.Length; i++)
                {
                    var strParm = (string)listParamName[i];
                    strParm = strParm.TrimStart(':');
                    query.SetParameter(strParm, values[i]);
                }
            }
            int res = query.ExecuteUpdate();
            _session.Flush();
            return res;
        }

        #endregion

        #region【方法】[公共] >>>> Delete >>>> 批量删除一组对象
        /// <summary>
        /// 批量删除一组对象
        /// </summary>
        /// <param name="objs">对象数组</param>
        public void Delete(object[] objs)
        {
            foreach (object obj in objs)
            {
                _session.Delete(obj);
            }
            _session.Flush();
        }
        #endregion
        #region【方法】[公共] >>>> Delete >>>> 删除一个对象
        /// <summary>
        /// 删除一个对象
        /// </summary>
        /// <param name="obj">对象</param>
        public void Delete(object obj)
        {
            _session.Delete(obj);
            _session.Flush();
        }
        #endregion
        #region【方法】[公共] >>>> Delete >>>> 删除
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="hql">hql语句</param>
        public void Delete(string hql)
        {
            _session.Delete(hql);
            _session.Flush();
        }
        #endregion

        #region【方法】[公共] >>>> Find<T> >>>> 获取一个列表

        ///<summary>
        ///  获取一个列表
        ///</summary>
        ///<typeparam name = "T">实体类型</typeparam>
        ///<returns></returns>
        public IList<T> Find<T>()
        {
            return _session.CreateQuery("from " + typeof(T)).List<T>();
        }
        #endregion
        #region【方法】[公共] >>>> Find<T> >>>> 获取一个列表

        /// <summary>
        ///   分页获取实体数据。
        /// </summary>
        /// <typeparam name = "T">要获取的实体类型。</typeparam>
        /// <param name = "pageIndex">第几页，从1开始。</param>
        /// <param name = "pageSize">页大小。</param>
        /// <param name = "whereClause">where子句。</param>
        /// <param name = "orderClause">order条件。</param>
        /// <returns>返回获取的数据。</returns>
        public IList<T> Find<T>(int pageIndex, int pageSize, string whereClause, string orderClause)
        {
            //生成hql: from <Entity> [where <WhereClause>] [order by <SortClause>]
            var sb = new StringBuilder();
            sb.Append("from ");
            sb.Append(typeof(T));

            if (!string.IsNullOrEmpty(whereClause))
            {
                sb.Append(" where ");
                sb.Append(whereClause);
            }

            if (!string.IsNullOrEmpty(orderClause))
            {
                sb.Append(" order by ");
                sb.Append(orderClause);
            }

            IList<T> ret =
                _session.CreateQuery(sb.ToString()).SetFirstResult((pageIndex - 1) *
                                                                  pageSize).
                    SetMaxResults(pageSize).List<T>();

            return ret;
        }
        #endregion
        #region【方法】[公共] >>>> Find<T> >>>> 获取一个列表

        /// <summary>
        ///   分页获取实体数据。
        /// </summary>
        /// <typeparam name = "T">要获取的实体类型。</typeparam>
        /// <param name = "pageIndex">第几页，从1开始</param>
        /// <param name = "pageSize">页大小。</param>
        /// <param name = "whereClause">where子句。</param>
        /// <param name = "orderClause">order条件。</param>
        /// <param name = "totalCount">返回实体的总条数。</param>
        /// <returns>返回获取的数据。</returns>
        public IList<T> Find<T>(int pageIndex, int pageSize, string whereClause, string orderClause, out int totalCount)
        {
            var sb = new StringBuilder();
            sb.Append("select count(*) from ");
            sb.Append(typeof(T));

            if (!string.IsNullOrEmpty(whereClause))
            {
                sb.Append(" where ");
                sb.Append(whereClause);
            }

            if (!string.IsNullOrEmpty(orderClause))
            {
                sb.Append(" order by ");
                sb.Append(orderClause);
            }

            object o = _session.CreateQuery(sb.ToString()).UniqueResult();
            totalCount = Convert.ToInt32(o);

            return Find<T>(pageIndex, pageSize, whereClause, orderClause);
        }
        #endregion
        #region【方法】[公共] >>>> Find<T> >>>> 查找对象
        /// <summary>
        /// 查找对象
        /// </summary>
        /// <param name="hql">hql语句</param>
        /// <param name="values">对象值数组</param>
        /// <param name="lLow">开始索引</param>
        /// <param name="lCount">个数</param>
        /// <returns>对象列表</returns>
        public IList<T> Find<T>(string hql, Object[] values, long lLow, long lCount)
        {
            IQuery query = _session.CreateQuery(hql);
            if (lLow != -1)
            {
                query.SetFirstResult((int)lLow);
            }
            if (lCount != -1)
            {
                query.SetMaxResults((int)lCount);
            }
            if (values != null && values.Length > 0)
            {
                IList listParamName = GetHqlParamName(hql);

                if (listParamName.Count != values.Length)
                {
                    string sError = string.Format("Parametric variable is wrong. Param count required:{0}  Param count recieved:{1}", listParamName.Count, values.Length);
                    throw new Exception(sError);
                }
                for (int i = 0; i < values.Length; i++)
                {
                    //query.SetParameter( "TemplateID", values[i]);
                    var strParm = (string)listParamName[i];
                    strParm = strParm.TrimStart(':');
                    query.SetParameter(strParm, values[i]);
                }
            }

            return query.List<T>();
        }
        #endregion
        #region【方法】[公共] >>>> Find<T> >>>> 查找对象
        /// <summary>
        /// 查找对象
        /// </summary>
        /// <param name="hql">hql语句</param>
        /// <param name="lLow">开始索引</param>
        /// <param name="lCount">个数</param>
        /// <returns>对象列表</returns>
        public IList<T> Find<T>(string hql, long lLow, long lCount)
        {
            return Find<T>(hql, new object[] { }, lLow, lCount);
        }
        #endregion
        #region【方法】[公共] >>>> Find >>>> 查找对象
        /// <summary>
        /// 查找对象
        /// </summary>
        /// <param name="hql">hql语句</param>
        /// <param name="values">对象值数组</param>
        /// <param name="lLow">开始索引</param>
        /// <param name="lCount">个数</param>
        /// <returns>对象列表</returns>
        public IList Find(string hql, Object[] values, long lLow, long lCount)
        {
            IQuery query = _session.CreateQuery(hql);
            if (lLow != -1)
            {
                query.SetFirstResult((int)lLow);
            }
            if (lCount != -1)
            {
                query.SetMaxResults((int)lCount);
            }
            if (values != null && values.Length > 0)
            {
                IList listParamName = GetHqlParamName(hql);

                if (listParamName.Count != values.Length)
                {
                    string sError = string.Format("Parametric variable is wrong. Param count required:{0}  Param count recieved:{1}", listParamName.Count, values.Length);
                    throw new Exception(sError);
                }
                for (int i = 0; i < values.Length; i++)
                {
                    //query.SetParameter( "TemplateID", values[i]);
                    var strParm = (string)listParamName[i];
                    strParm = strParm.TrimStart(':');
                    query.SetParameter(strParm, values[i]);
                }
            }

            return query.List();
        }
        #endregion
        #region【方法】[公共] >>>> Find >>>> 查找对象
        /// <summary>
        /// 查找对象
        /// </summary>
        /// <param name="hql">hql语句</param>
        /// <param name="lLow">开始索引</param>
        /// <param name="lCount">个数</param>
        /// <returns>对象列表</returns>
        public IList Find(string hql, long lLow, long lCount)
        {
            return Find(hql, new object[] { }, lLow, lCount);
        }
        #endregion
        #region【方法】[公共] >>>> Find >>>> 查找对象
        /// <summary>
        /// 获取部分字段列表
        /// </summary>
        /// <param name="hql"></param>
        /// <returns></returns>
        public List<object[]> FindObjs(string hql)
        {
            return _session.CreateQuery(hql).List<object[]>().ToList();
        }
        #endregion
        #region【方法】[公共] >>>> Find >>>> 查找对象
        /// <summary>
        /// 获取部分字段列表
        /// </summary>
        /// <param name="hql"></param>
        /// <returns></returns>
        public List<object> FindObj(string hql)
        {
            return _session.CreateQuery(hql).List<object>().ToList();
        }
        #endregion
        #region【方法】[公共] >>>> Find >>>> 查找对象sql

        /// <summary>
        /// 获取部分字段列表,根据SQL
        /// </summary>
        /// <returns></returns>
        public List<object[]> FindFieldsBySql(string sql)
        {
            if (sql == null) throw new ArgumentNullException("sql");
            return _session.CreateSQLQuery(sql).List<object[]>().ToList();
        }

        #endregion

        #region【方法】[公共] >>>> Get<T, TK> >>>> 获取一条数据
        ///<summary>
        ///  获取一条数据
        ///</summary>
        ///<param name = "key">实体标识</param>
        ///<typeparam name = "T">实体类型</typeparam>
        ///<typeparam name = "TK">表示类型</typeparam>
        ///<returns></returns>
        public T Get<T, TK>(TK key)
        {
            return _session.Get<T>(key);
        }
        #endregion
        #region【方法】[公共] >>>> Get<T, TK> >>>> 获取一条数据
        /// <summary>
        /// 获取一条数据，如果获取出现多条将取第一条
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="hql"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public T Get<T>(string hql, object[] values = null)
        {
            var query = _session.CreateQuery(hql);
            if (values != null && values.Length > 0)
            {
                IList listParamName = GetHqlParamName(hql);

                if (listParamName.Count != values.Length)
                {
                    string sError = string.Format("Parametric variable is wrong. Param count required:{0}  Param count recieved:{1}", listParamName.Count, values.Length);
                    throw new Exception(sError);
                }
                for (int i = 0; i < values.Length; i++)
                {
                    var strParm = (string)listParamName[i];
                    strParm = strParm.TrimStart(':');
                    query.SetParameter(strParm, values[i]);
                }
            }
            return query.List<T>().ToList().FirstOrDefault();
        }

        #endregion

        #region 【方法】[公共] >>>> GetBySql<T> >>>> 获取一条数据
        /// <summary>
        /// 获取一条数据，如果获取出现多条将取第一条
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public T GetBySql<T>(string sql, object[] values = null)
        {
            var query = _session.CreateSQLQuery(sql);
            if (values != null && values.Length > 0)
            {
                IList listParamName = GetHqlParamName(sql);

                if (listParamName.Count != values.Length)
                {
                    string sError = string.Format("Parametric variable is wrong. Param count required:{0}  Param count recieved:{1}", listParamName.Count, values.Length);
                    throw new Exception(sError);
                }
                for (int i = 0; i < values.Length; i++)
                {
                    var strParm = (string)listParamName[i];
                    strParm = strParm.TrimStart(':');
                    query.SetParameter(strParm, values[i]);
                }
            }
            return query.List<T>().ToList().FirstOrDefault();
        }
        #endregion

        #region【方法】[公共] >>>> GetRowCount >>>> 获取Count
        /// <summary>
        /// 获取Count
        /// </summary>
        /// <param name="hql">hql语句</param>
        /// <param name="values">对象值数组</param>
        /// <returns>Count</returns>
        public long GetRowCount(string hql, Object[] values)
        {
            string s = "SELECT COUNT(*) " + hql;
            if (s.ToLower().IndexOf("order by", StringComparison.Ordinal) != -1)
            {
                s = s.Substring(0, s.ToLower().IndexOf("order by", StringComparison.Ordinal));
            }

            IList il = Find(s, values, -1, -1);
            if (il.Count == 0)
            {
                return 0;
            }
            return int.Parse(il[0].ToString());
        }
        #endregion
        #region【方法】[公共] >>>> GetRowCount >>>> 获取Count
        /// <summary>
        /// 获取Count
        /// </summary>
        /// <param name="hql">hql语句</param>
        /// <returns>Count</returns>
        public long GetRowCount(string hql)
        {
            return GetRowCount(hql, new object[] { });
        }
        #endregion

        #region【方法】[公共] >>>> ExecuteQuery >>>> 执行SQL语句

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sqlStr"> </param>
        /// <param name="paramsValue">参数值</param>
        /// <param name="paramsType">参数类型</param>
        /// <param name="lLow">开始索引值</param>
        /// <param name="lCount">个数</param>
        /// <returns>DataTable</returns>
        public DataTable ExecuteQuery(string sqlStr, object[] paramsValue, Type[] paramsType, long lLow, long lCount)
        {
            var sql = new SqlString(sqlStr);
            IDataReader reader = null;
            IDbCommand cmd = null;

            var imp = (ISessionImplementor)_session;

            try
            {
                cmd = GetDBCommand(paramsValue, paramsType, imp, sql);
                reader = imp.Batcher.ExecuteReader(cmd);
                DataTable ret = GetDataTableFromIDataReader(reader);
                return ret;
            }
            catch (Exception e)
            {
                string sError = "Execute Sql failed";
                sError += string.Format("[{0}]", sql);
                if (paramsValue != null)
                {
                    for (int i = 0; i < paramsValue.Length; i++)
                    {
                        object o = paramsValue[i];
                        sError += string.Format("[p{0}:{1} ({2})]", i, o, (o == null) ? null : o.GetType());
                    }
                }

                throw new Exception(sError, e);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Dispose();
                }
                imp.Batcher.CloseCommand(cmd, null);
                if (cmd != null)
                {
                    cmd.Dispose();
                }
            }
        }
        #endregion
        #region【方法】[公共] >>>> ExecuteQuery >>>> 执行SQL语句

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sqlStr"> </param>
        /// <param name="lLow">开始索引值</param>
        /// <param name="lCount">个数</param>
        /// <returns>DataTable</returns>
        public DataTable ExecuteQuery(string sqlStr, long lLow, long lCount)
        {
            return ExecuteQuery(sqlStr, new object[] { }, new Type[] { }, lLow, lCount);
        }
        #endregion

        #region【方法】[公共] >>>> ExecuteNonQuery >>>> 执行非查询SQL语句

        /// <summary>
        /// 执行非查询SQL语句
        /// </summary>
        /// <param name="sqlStr"> </param>
        /// <param name="paramsValue">参数值</param>
        /// <param name="paramsType">参数类型</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sqlStr, object[] paramsValue, Type[] paramsType)
        {
            var sql = new SqlString(sqlStr);
            string ss = string.Empty;
            string aa = string.Empty;
            var imp = (ISessionImplementor)_session;
            try
            {
                //if ( imp.Connection.State == ConnectionState.Closed )
                //    imp.Connection.Open();
                using (IDbCommand cmd = GetDBCommand(paramsValue, paramsType, imp, sql))
                {
                    int num = imp.Batcher.ExecuteNonQuery(cmd);
                    //imp.Connection.Close();
                    return num;
                }
            }
            catch (Exception e)
            {
                string sError = "ExecuteNonQuery Sql failed";
                sError += string.Format("[{0}]", sql);

                if (paramsValue != null)
                {
                    for (int i = 0; i < paramsValue.Length; i++)
                    {
                        object o = paramsValue[i];
                        sError += string.Format("[p{0}:{1} ({2})]", i, o, (o == null) ? null : o.GetType());
                        sError += ss + " " + aa;
                    }
                }
                throw new Exception(sError, e);
            }
        }
        #endregion
        #region【方法】[公共] >>>> ExecuteNonQuery >>>> 执行非查询SQL语句

        /// <summary>
        /// 执行非查询SQL语句
        /// </summary>
        /// <returns></returns>
        public int ExecuteNonQuery(string sqlStr)
        {
            return ExecuteNonQuery(sqlStr, new object[] { }, new Type[] { });
        }
        #endregion

        #region【方法】[公共] >>>> QueryResultCount >>>> 获取个数
        /// <summary>
        /// 获取个数
        /// </summary>
        /// <param name="sql">SqlString对象</param>
        /// <param name="paramsValue">参数值</param>
        /// <param name="paramsType">参数类型</param>
        /// <returns></returns>
        public long QueryResultCount(string sql, object[] paramsValue, Type[] paramsType)
        {
            DataTable odt = ExecuteQuery(sql, paramsValue, paramsType, -1, -1);
            long count = Convert.ToInt64(odt.Rows[0][0]);

            if (odt != null)
            {
                odt.Dispose();
            }

            return count;
        }
        #endregion
        #region【方法】[公共] >>>> QueryResultCount >>>> 获取个数

        /// <summary>
        /// 获取个数
        /// </summary>
        /// <param name="sql">SqlString对象</param>
        /// <returns></returns>
        public long QueryResultCount(string sql)
        {
            return QueryResultCount(sql, new object[] { }, new Type[] { });
        }
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (_tranaction != null)
            {
                _tranaction.Dispose();
                _tranaction = null;
            }

            // 在sessionManager 中会释放
            //if ( session != null )
            //{
            //    session.Dispose();
            //    session = null;
            //}

            if (_sessionManager != null)
            {
                _sessionManager.Dispose();
                _sessionManager = null;
            }
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 获取HQL中的参数列表
        /// </summary>
        /// <param name="sHQL">hql语句</param>
        /// <returns>参数列表</returns>
        private IList GetHqlParamName(string sHQL)
        {
            var listParamName = new ArrayList();
            //先过滤掉['...']中的内容
            string[] arrStr = sHQL.Split('\'');
            sHQL = string.Empty;
            for (int i = 0; i < arrStr.Length; i += 2)
            {
                sHQL += arrStr[i];
            }

            //提取:中的内容
            for (int iFind = sHQL.IndexOf(':'); iFind != -1; iFind = sHQL.IndexOf(':', iFind + 1))
            {
                int iSpace = sHQL.IndexOf(' ', iFind);
                if (iSpace != -1)
                {
                    string sTmp = sHQL.Substring(iFind, iSpace - iFind);
                    listParamName.Add(sTmp);
                }
                else
                {
                    string sTmp = sHQL.Substring(iFind);
                    listParamName.Add(sTmp);
                }
            }
            return listParamName;
        }

        private IDbCommand GetDBCommand(object[] paramsValue, Type[] paramsType, ISessionImplementor se, SqlString sql)
        {
            SqlType[] arrSqlType = ObjectToSqlType(paramsType);
            IDbCommand cmd = se.Batcher.PrepareCommand(CommandType.Text, sql, arrSqlType);
            for (int i = 0; i < paramsValue.Length; i++)
            {
                object oValue = paramsValue[i];
                IType type = ObjectToType(paramsType[i]);
                if (Equals(type, NHibernateUtil.Object))
                {
                    type = ObjectToType(oValue);
                }
                type.NullSafeSet(cmd, oValue, i, se);
            }
            return cmd;
        }

        private DataTable GetDataTableFromIDataReader(IDataReader reader)
        {
            var dt = new DataTable();

            bool init = false;

            dt.BeginLoadData();
            var vals = new object[0];
            while (reader.Read())
            {
                if (!init)
                {
                    init = true;
                    int fieldCount = reader.FieldCount;
                    for (int i = 0; i < fieldCount; ++i)
                    {
                        dt.Columns.Add(reader.GetName(i), reader.GetFieldType(i));
                    }
                    vals = new object[fieldCount];
                }
                reader.GetValues(vals);
                dt.LoadDataRow(vals, true);
            }
            if (!init)
            {
                int fieldCount = reader.FieldCount;
                for (int i = 0; i < fieldCount; ++i)
                {
                    dt.Columns.Add(reader.GetName(i), reader.GetFieldType(i));
                }
            }
            reader.Close();
            dt.EndLoadData();
            return dt;
        }

        private IType ObjectToType(object oValue)
        {
            var tp = oValue as Type;
            if (tp != null)
            {

                switch (oValue.ToString())
                {
                    case "System.String":
                        return NHibernateUtil.String;
                    case "System.Int32":
                        return NHibernateUtil.Int32;
                    case "System.Int64":
                        return NHibernateUtil.Int64;
                    case "System.DateTime":
                        return NHibernateUtil.DateTime;
                    case "System.Boolean":
                        return NHibernateUtil.Boolean;
                    case "System.Byte":
                        return NHibernateUtil.Byte;
                    case "System.Char":
                        return NHibernateUtil.Character;
                    case "System.Decimal":
                        return NHibernateUtil.Decimal;
                    case "System.Double":
                        return NHibernateUtil.Double;
                    case "System.Guid":
                        return NHibernateUtil.Guid;
                    case "System.Int16":
                        return NHibernateUtil.Int16;
                    case "System.SByte":
                        return NHibernateUtil.SByte;
                    case "System.UInt16":
                        return NHibernateUtil.UInt16;
                    case "System.UInt32":
                        return NHibernateUtil.UInt32;
                    case "System.UInt64":
                        return NHibernateUtil.UInt64;
                    case "System.Single":
                        return NHibernateUtil.Single;
                    case "System.TimeSpan":
                        return NHibernateUtil.TimeSpan;
                }

                if (tp.IsEnum)
                {
                    try
                    {
                        return NHibernateUtil.Enum(tp);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format("enum = [{0}]Type =[{1}] is not enum", oValue, oValue.GetType()), ex);
                    }

                }

            }
            if (oValue is string)
            {
                return NHibernateUtil.String;
            }
            if (oValue is int)
            {
                return NHibernateUtil.Int32;
            }
            if (oValue is long)
            {
                return NHibernateUtil.Int64;
            }
            if (oValue is DateTime)
            {
                return NHibernateUtil.DateTime;
            }
            if (oValue is Boolean)
            {
                return NHibernateUtil.Boolean;
            }
            if (oValue is Byte)
            {
                return NHibernateUtil.Byte;
            }
            if (oValue is Char)
            {
                return NHibernateUtil.Character;
            }
            if (oValue is Decimal)
            {
                return NHibernateUtil.Decimal;
            }
            if (oValue is Double)
            {
                return NHibernateUtil.Double;
            }
            if (oValue is Guid)
            {
                return NHibernateUtil.Guid;
            }
            if (oValue is Int16)
            {
                return NHibernateUtil.Int16;
            }
            if (oValue is SByte)
            {
                return NHibernateUtil.SByte;
            }
            if (oValue is UInt16)
            {
                return NHibernateUtil.UInt16;
            }
            if (oValue is UInt32)
            {
                return NHibernateUtil.UInt32;
            }
            if (oValue is UInt64)
            {
                return NHibernateUtil.UInt64;
            }
            if (oValue is Single)
            {
                return NHibernateUtil.Single;
            }
            if (oValue is TimeSpan)
            {
                return NHibernateUtil.TimeSpan;
            }
            if (oValue is Enum)
            {
                return NHibernateUtil.Enum(oValue.GetType());
            }

            return NHibernateUtil.Object;
        }

        private SqlType[] ObjectToSqlType(IEnumerable<Type> arrType)
        {
            var result = new ArrayList();
            foreach (Type tp in arrType)
            {
                SqlType st;
                switch (tp.ToString())
                {
                    case "System.String":
                        st = SqlTypeFactory.GetString(512);
                        break;
                    case "System.Int32":
                        st = SqlTypeFactory.Int32;
                        break;
                    case "System.Int64":
                        st = SqlTypeFactory.Int64;
                        break;
                    case "System.DateTime":
                        st = SqlTypeFactory.DateTime;
                        break;
                    case "System.Boolean":
                        st = SqlTypeFactory.Boolean;
                        break;
                    case "System.Byte":
                        st = SqlTypeFactory.Byte;
                        break;
                    case "System.Char":
                        st = SqlTypeFactory.Byte;
                        break;
                    case "System.Decimal":
                        st = SqlTypeFactory.Decimal;
                        break;
                    case "System.Double":
                        st = SqlTypeFactory.Double;
                        break;
                    case "System.Guid":
                        st = SqlTypeFactory.Guid;
                        break;
                    case "System.Int16":
                        st = SqlTypeFactory.Int16;
                        break;
                    case "System.SByte":
                        st = SqlTypeFactory.SByte;
                        break;
                    case "System.UInt16":
                        st = SqlTypeFactory.UInt16;
                        break;
                    case "System.UInt32":
                        st = SqlTypeFactory.UInt32;
                        break;
                    case "System.UInt64":
                        st = SqlTypeFactory.UInt64;
                        break;
                    case "System.Single":
                        st = SqlTypeFactory.Single;
                        break;
                    default:
                        st = SqlTypeFactory.GetString(512);
                        break;
                }
                result.Add(st);

            }
            return (SqlType[])result.ToArray(typeof(SqlType));
        }

        #endregion
    }
}