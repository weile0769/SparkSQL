using Spark.DatabaseAccessor.Options;
using Spark.DatabaseAccessor.Repositories;
using Spark.DatabaseAccessor.SqlSugar.Builders;
using Spark.DatabaseAccessor.SqlSugar.Extensions;
using Spark.DatabaseAccessor.SqlSugar.Repositories;
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
            services.AddScoped<IDatabaseContext, SqlSugarDatabaseContext>();
            //配置非泛型数据仓储服务
            services.AddTransient<IDatabaseRepository, SqlSugarDatabaseRepository>();
            //配置数据库事务工作单元服务
            services.AddScoped<IDatabaseUnitOfWork, SqlSugarDatabaseUnitOfWork>();
            //配置泛型数据仓储服务
            services.AddTransient(typeof(IDatabaseRepository<>), typeof(SqlSugarDatabaseRepository<>));
            return services;
        }


        /// <summary>
        ///     添加数据库操作提供器容器服务
        /// </summary>
        /// <param name="services">容器服务提供器</param>
        /// <param name="options">自定义配置项</param>
        public static IServiceCollection AddSqlSugarDatabaseAccessor(this IServiceCollection services, DatabaseAccessorOptions options)
        {
            services.AddScoped(option =>
            {
                var connectionOptions = SqlSugarDatabaseConnectionBuilder.Build();
                return new SqlSugarClient(connectionOptions).ConfigureAopEvent(options);
            });
            return services;
        }
    }
}
