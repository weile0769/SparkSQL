using Microsoft.Extensions.Options;
using Spark.DatabaseAccessor.Contexts;
using Spark.DatabaseAccessor.Options;
using Spark.DatabaseAccessor.SqlSugar.Utils;
using SqlSugar;

namespace Spark.DatabaseAccessor.SqlSugar.Contexts
{
    /// <summary>
    ///     数据库上下文提供器默认实现
    /// </summary>
    internal class SqlSugarDatabaseContextProvider : IDatabaseContextProvider
    {
        /// <summary>
        ///     SqlSugar数据库连接对象
        /// </summary>
        private readonly ISqlSugarClient _sqlSugarClient;

        /// <summary>
        ///     数据库访问器全局配置
        /// </summary>
        private readonly DatabaseAccessorOptions _databaseAccessorOptions;

        /// <summary>
        ///   构造函数
        /// </summary>
        public SqlSugarDatabaseContextProvider(IOptions<DatabaseAccessorOptions> options,
            ISqlSugarClient sqlSugarClient)
        {
            _sqlSugarClient = SparkCheck.NotNull(sqlSugarClient, nameof(sqlSugarClient));
            _databaseAccessorOptions = SparkCheck.NotNull(options.Value, nameof(options));
        }


        /// <summary>
        ///   获取数据库连接上下文
        /// </summary>
        /// <param name="connectionName">数据库连接标识</param>
        /// <returns>数据库连接上下文</returns>
        public object GetDatabaseContext(string? connectionName = default)
        {
            connectionName = connectionName ?? _databaseAccessorOptions.DefaultConnectionName;
            var databaseClient = (SqlSugarClient)_sqlSugarClient;
            return databaseClient.GetConnection(connectionName);
        }
    }
}
