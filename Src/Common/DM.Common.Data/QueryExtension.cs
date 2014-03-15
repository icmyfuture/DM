using System;
using System.Collections.Generic;
using System.Text;
using NHibernate;

namespace DM.Common.Data
{
    ///<summary>
    ///  查询辅助方法
    ///</summary>
    public static class QueryExtension
    {
        ///<summary>
        ///  查询
        ///</summary>
        ///<param name = "session">session上下文</param>
        ///<param name = "querystring">查询字符串</param>
        ///<typeparam name = "T">返回类型</typeparam>
        ///<returns></returns>
        public static T Query<T>(this ISession session, string querystring)
        {
            return (T)session.CreateQuery(querystring).Enumerable();
        }

        ///<summary>
        ///  插入数据
        ///</summary>
        ///<param name = "session">session上下文</param>
        ///<param name = "item">实体</param>
        ///<typeparam name = "T">实体类型</typeparam>
        ///<returns>增加成功后的实体</returns>
        public static T Add<T>(this ISession session, T item) where T : class
        {
            return session.Merge(typeof(T).Name, item);
        }

        ///<summary>
        ///  删除数据
        ///</summary>
        ///<param name = "session">session上下文</param>
        ///<param name = "item">实体</param>
        ///<typeparam name = "T">实体类型</typeparam>
        public static void Delete<T>(this ISession session, T item)
        {
            session.Delete(item);
        }

        ///<summary>
        ///  更新数据
        ///</summary>
        ///<param name = "session">session上下文</param>
        ///<param name = "item">实体</param>
        ///<param name = "key">实体标识</param>
        ///<typeparam name = "T">实体类型</typeparam>
        ///<typeparam name = "TK">标识类型</typeparam>
        ///<returns>修改完成后的实体</returns>
        public static T Update<T, TK>(this ISession session, T item, TK key)
        {
            session.Update(item, key);
            return item;
        }

        ///<summary>
        ///  获取一条数据
        ///</summary>
        ///<param name = "session">session上下文</param>
        ///<param name = "key">实体标识</param>
        ///<typeparam name = "T">实体类型</typeparam>
        ///<typeparam name = "TK">表示类型</typeparam>
        ///<returns></returns>
        public static T Get<T, TK>(this ISession session, TK key)
        {
            return session.Get<T>(key);
        }

        ///<summary>
        ///  获取一个列表
        ///</summary>
        ///<param name = "session">session上下文</param>
        ///<typeparam name = "T">实体类型</typeparam>
        ///<returns></returns>
        public static IList<T> GetList<T>(this ISession session)
        {
            return session.CreateQuery("from " + typeof(T)).List<T>();
        }

        /// <summary>
        ///   分页获取实体数据。
        /// </summary>
        /// <typeparam name = "T">要获取的实体类型。</typeparam>
        /// <param name = "session">session上下文。</param>
        /// <param name = "pageIndex">第几页，从1开始。</param>
        /// <param name = "pageSize">页大小。</param>
        /// <param name = "whereClause">where子句。</param>
        /// <param name = "orderClause">order条件。</param>
        /// <returns>返回获取的数据。</returns>
        public static IList<T> GetList<T>(this ISession session, int pageIndex, int pageSize, string whereClause, string orderClause)
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

            IList<T> ret = session.CreateQuery(sb.ToString()).SetFirstResult((pageIndex - 1) * pageSize).SetMaxResults(pageSize).List<T>();

            return ret;
        }

        /// <summary>
        ///   分页获取实体数据。
        /// </summary>
        /// <typeparam name = "T">要获取的实体类型。</typeparam>
        /// <param name = "session">session上下文。</param>
        /// <param name = "pageIndex">第几页，从1开始</param>
        /// <param name = "pageSize">页大小。</param>
        /// <param name = "whereClause">where子句。</param>
        /// <param name = "orderClause">order条件。</param>
        /// <param name = "totalCount">返回实体的总条数。</param>
        /// <returns>返回获取的数据。</returns>
        public static IList<T> GetList<T>(this ISession session, int pageIndex, int pageSize, string whereClause, string orderClause, out int totalCount)
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

            object o = session.CreateQuery(sb.ToString()).UniqueResult();
            totalCount = Convert.ToInt32(o);

            return GetList<T>(session, pageIndex, pageSize, whereClause, orderClause);
        }
    }
}