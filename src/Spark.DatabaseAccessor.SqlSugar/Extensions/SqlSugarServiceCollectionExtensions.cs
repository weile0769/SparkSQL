using Microsoft.Extensions.DependencyInjection.Extensions;
using Spark.DatabaseAccessor.Contexts;
using Spark.DatabaseAccessor.Options;
using Spark.DatabaseAccessor.SqlSugar.Builders;
using Spark.DatabaseAccessor.SqlSugar.Contexts;
using Spark.DatabaseAccessor.SqlSugar.Extensions;
using Spark.DatabaseAccessor.SqlSugar.Utils;
using SqlSugar;
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
        public static IServiceCollection AddSparkDatabaseAccessor(this IServiceCollection services, Action<DatabaseAccessorOptions> options)
        {
            //加载并检查配置项
            var accessorOptions = new DatabaseAccessorOptions();
            SparkCheck.NotNull(options)(accessorOptions);
            SparkContext.ConfigureOptions(accessorOptions);
            //配置数据库配置项
            services.Configure(options);
            //配置Sqlsugar数据库访问器
            services.AddSqlSugarDatabaseAccessor(accessorOptions);
            //配置数据库上下文提供器
            services.TryAddSingleton<IDatabaseContextProvider, SqlSugarDatabaseContextProvider>();

            return services;
        }


        /// <summary>
        ///     添加数据库操作提供器容器服务
        /// </summary>
        /// <param name="services">容器服务提供器</param>
        /// <param name="options">自定义配置项</param>
        public static IServiceCollection AddSqlSugarDatabaseAccessor(this IServiceCollection services, DatabaseAccessorOptions options)
        {
            services.AddSingleton(option =>
            {
                //SqlSugarScope是线程安全，可使用单例注入
                var connectionOptions = SqlSugarDatabaseConnectionBuilder.Build();
                return new SqlSugarScope(connectionOptions).ConfigureAopEvent(options);
            });
            return services;
        }
    }
}
