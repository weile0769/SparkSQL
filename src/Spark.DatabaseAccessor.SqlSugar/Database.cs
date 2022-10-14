using Microsoft.Extensions.DependencyInjection;
using Spark.DatabaseAccessor.SqlSugar.Utils;
using SqlSugar;

namespace Spark.DatabaseAccessor.SqlSugar
{
    /// <summary>
    ///     数据库上下文全局静态类
    /// </summary>
    public class Database
    {
        /// <summary>
        ///     共享线程数据库上下文
        /// </summary>
        public static ISqlSugarClient Context
        {
            get
            {
                return SparkCheck.NotNull(SparkContext.RootServices).GetRequiredService<ISqlSugarClient>();
            }
        }

        /// <summary>
        ///   创建数据库上下文
        /// </summary>
        public static ISqlSugarClient NewContext
        {
            get
            {
                var databaseContextProvider = SparkCheck.NotNull(SparkContext.RootServices).GetRequiredService<ISqlSugarClient>();
                return databaseContextProvider.CopyNew();
            }
        }
    }
}