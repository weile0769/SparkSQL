using Spark.DatabaseAccessor.Options;
using System;

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
        ///     根服务
        /// </summary>
        internal static IServiceProvider? RootServices;

        /// <summary>
        ///     静态设置数据库访问器全局配置
        /// </summary>
        /// <param name="accessorOptions">数据库访问器全局配置</param>
        public static DatabaseAccessorOptions ConfigureOptions(DatabaseAccessorOptions accessorOptions)
        {
            var options = DatabaseAccessorOptions = accessorOptions;
            if (options == null)
            {
                throw new InvalidOperationException($"数据库访问器全局配置不能为空：{typeof(DatabaseAccessorOptions).AssemblyQualifiedName}");
            }
            return options;
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
