using Microsoft.Extensions.Configuration;
using Spark.DatabaseAccessor.Options;
using Spark.DatabaseAccessor.SqlSugar.Utils;

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
        /// <param name="optionsAction">自定义配置项</param>
        public static IServiceCollection AddSqlSugarDatabaseAccessor(this IServiceCollection services, Func<DatabaseAccessorOptions>? optionsAction = default)
        {
            var accessorOptions = SparkContext.ConfigureAccessorOptions(SparkCheck.NotNull(optionsAction?.Invoke(), nameof(Func<DatabaseAccessorOptions>)));
            services.AddOptions<DatabaseAccessorOptions>(DatabaseAccessorOptions.OptionName).Configure(options => options = accessorOptions);
            return SparkContext.ConfigureServiceCollection(services);
        }

        /// <summary>
        ///     添加数据库操作提供器容器服务
        /// </summary>
        /// <param name="services">容器服务提供器</param>
        public static IServiceCollection AddSqlSugarDatabaseAccessor(this IServiceCollection services)
        {
            var section = services.TryGetConfiguration().GetSection(DatabaseAccessorOptions.OptionName);
            SparkContext.ConfigureAccessorOptions(section.Get<DatabaseAccessorOptions>());
            services.Configure<DatabaseAccessorOptions>(section);
            return SparkContext.ConfigureServiceCollection(services);
        }
    }
}
