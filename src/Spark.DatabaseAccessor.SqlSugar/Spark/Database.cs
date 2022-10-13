using Spark.DatabaseAccessor.Options;
using Spark.DatabaseAccessor.SqlSugar.Builders;
using Spark.DatabaseAccessor.SqlSugar.Utils;
using SqlSugar;
using System.Diagnostics;

namespace Spark.DatabaseAccessor.SqlSugar
{
    /// <summary>
    ///     数据库上下文全局静态类
    /// </summary>
    public class Database
    {
        /// <summary>
        ///   数据库连接配置
        /// </summary>
        private static readonly List<ConnectionConfig> ConnectionOptions = SqlSugarDatabaseConnectionBuilder.Build();

        /// <summary>
        ///   数据库配置
        /// </summary>
        private static readonly DatabaseAccessorOptions databaseAccessorOptions = SparkCheck.NotNull(SparkContext.DatabaseAccessorOptions);

        /// <summary>
        ///     共享线程数据库上下文
        /// </summary>
        public static SqlSugarClient Context => GetContext();

        /// <summary>
        ///   创建数据库上下文
        /// </summary>
        public static SqlSugarClient CreateContext
        {
            get
            {
                var sqlClient = new SqlSugarClient(ConnectionOptions);
                SqlSugarDatabaseSqlLoggingBuilder.Build(databaseAccessorOptions, sqlClient);
                return sqlClient;
            }
        }





        /// <summary>
        ///     获取数据库上下文
        /// </summary>
        private static SqlSugarClient GetContext()
        {
            var key = ConnectionOptions.GetHashCode().ToString();
            StackTrace st = new StackTrace(true);
            var methods = st.GetFrames();
            var isAsync = UtilMethods.IsAnyAsyncMethod(methods);
            SqlSugarClient sqlClient;
            if (isAsync)
            {
                sqlClient = GetAsyncContext(key);
            }
            else
            {
                sqlClient = GetThreadContext(key);
            }
            return sqlClient;
        }


        /// <summary>
        ///   获取数据库上下文
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static SqlSugarClient GetAsyncContext(string key)
        {
            var sqlClient = AsyncContext<SqlSugarClient>.GetData(key);
            if (sqlClient == null)
            {
                AsyncContext<SqlSugarClient>.SetData(key, new SqlSugarClient(ConnectionOptions));
                sqlClient = AsyncContext<SqlSugarClient>.GetData(key);
                if (sqlClient == null) throw new InvalidOperationException($"数据库上下文对象线程异步设置失效：{typeof(SqlSugarClient).AssemblyQualifiedName}");
                SqlSugarDatabaseSqlLoggingBuilder.Build(databaseAccessorOptions, sqlClient);
            }
            return sqlClient;
        }

        /// <summary>
        ///   获取数据库上下文
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static SqlSugarClient GetThreadContext(string key)
        {
            var sqlClient = ThreadContext<SqlSugarClient>.GetData(key);
            if (sqlClient == null)
            {
                ThreadContext<SqlSugarClient>.SetData(key, new SqlSugarClient(ConnectionOptions));
                sqlClient = ThreadContext<SqlSugarClient>.GetData(key);
                if (sqlClient == null) throw new InvalidOperationException($"数据库上下文对象线程同步设置失效：{typeof(SqlSugarClient).AssemblyQualifiedName}");
                SqlSugarDatabaseSqlLoggingBuilder.Build(databaseAccessorOptions, sqlClient);
            }
            return sqlClient;
        }
    }
}