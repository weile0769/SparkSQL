using Microsoft.Extensions.DependencyInjection;
using Spark.DatabaseAccessor.SqlSugar.XUnit.Builders;

namespace Spark.DatabaseAccessor.SqlSugar.XUnit.Extensions
{
    /// <summary>
    ///     数据库操作提供器容器服务扩展类
    /// </summary>
    public static class SparkDatabaseAccessorServiceCollectionExtensions
    {
        /// <summary>
        ///     添加数据库操作提供器容器服务
        /// </summary>
        /// <param name="services">容器服务提供器</param>
        public static void AddSparkDatabaseAccessor(this IServiceCollection services)
        {
            var options = SparkDatabaseAccessorOptionsBuilder.Build();
            services.AddSparkDatabaseAccessor(option =>
            {
                option.DefaultConnectionName = options.DefaultConnectionName;
                option.CQRSEnabled = options.CQRSEnabled;
                option.PrintSqlLogEnabled = options.PrintSqlLogEnabled;
                option.PrintSqlLogWithConnectionEnabled = options.PrintSqlLogWithConnectionEnabled;
                option.PrintSqlLogExecutionTimeLimit = options.PrintSqlLogExecutionTimeLimit;
                option.DatabaseConnections = options.DatabaseConnections;
            });
        }
    }
}
