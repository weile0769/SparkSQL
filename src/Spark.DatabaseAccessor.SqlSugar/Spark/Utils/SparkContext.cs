using Microsoft.Extensions.DependencyInjection;
using Spark.DatabaseAccessor.Options;

namespace Spark.DatabaseAccessor.SqlSugar.Utils
{
    /// <summary>
    ///     程序上下文
    /// </summary>
    internal class SparkContext
    {
        /// <summary>
        ///     数据库访问器全局配置
        /// </summary>
        internal static DatabaseAccessorOptions? DatabaseAccessorOptions;

        /// <summary>
        ///     IOC容器访问器
        /// </summary>
        internal static IServiceCollection? ServiceCollection;

        /// <summary>
        ///     根服务
        /// </summary>
        internal static IServiceProvider? RootServices;

        /// <summary>
        ///     静态设置数据库访问器全局配置
        /// </summary>
        /// <param name="accessorOptions">数据库访问器全局配置</param>
        public static DatabaseAccessorOptions ConfigureAccessorOptions(DatabaseAccessorOptions accessorOptions)
        {
            var options = DatabaseAccessorOptions = accessorOptions;
            if (options == null)
            {
                throw new InvalidOperationException($"数据库访问器全局配置不能为空：{typeof(DatabaseAccessorOptions).AssemblyQualifiedName}");
            }
            return options;
        }

        /// <summary>
        ///     静态设置IOC容器访问器全局配置
        /// </summary>
        /// <param name="serviceCollection">IOC容器访问器</param>
        public static IServiceCollection ConfigureServiceCollection(IServiceCollection serviceCollection)
        {
            var services = ServiceCollection = serviceCollection;
            if (services == null)
            {
                throw new InvalidOperationException($"IOC容器访问器全局配置不能为空：{typeof(IServiceCollection).AssemblyQualifiedName}");
            }
            return services;
        }

        /// <summary>
        ///     静态设置IOC容器根服务全局配置
        /// </summary>
        /// <param name="rootServices">根服务</param>
        /// <returns>根服务</returns>
        public static IServiceProvider ConfigureRootServices(IServiceProvider rootServices)
        {
            var serviceProvider = RootServices = rootServices;
            if (serviceProvider == null)
            {
                throw new InvalidOperationException($"IOC容器根服务不能为空：{typeof(IServiceProvider).AssemblyQualifiedName}");
            }
            return serviceProvider;
        }
    }
}
