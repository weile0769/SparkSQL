using Spark.DatabaseAccessor.Options;
using Spark.DatabaseAccessor.SqlSugar.Utils;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    ///     数据库操作提供器容器服务扩展类
    /// </summary>
    public static class SqlSugarServiceCollectionExtensions
    {
        /// <summary>
        ///     添加数据库操作提供器容器服务
        /// </summary>
        /// <param name="services">容器服务提供器</param>
        /// <param name="options">自定义配置项</param>
        public static IServiceCollection AddSqlSugarDatabaseAccessor(this IServiceCollection services, Action<DatabaseAccessorOptions> options)
        {
            //加载并检查配置项
            var accessorOptions = new DatabaseAccessorOptions();
            SparkCheck.NotNull(options)(accessorOptions);
            SparkContext.ConfigureOptions(accessorOptions);
            //配置数据库配置项
            services.Configure(options);
            return services;
        }
    }
}
