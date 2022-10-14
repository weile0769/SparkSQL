using Spark.DatabaseAccessor.Options;

namespace Spark.DatabaseAccessor.SqlSugar.WebAPI.Sample.Extensions
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
        /// <param name="configuration">配置服务提供器</param>
        public static void AddSparkDatabaseAccessor(this IServiceCollection services, IConfiguration configuration)
        {
            var options = new DatabaseAccessorOptions();
            configuration.GetSection(DatabaseAccessorOptions.OptionName).Bind(options);
            services.AddSqlSugarDatabaseAccessor(option =>
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
