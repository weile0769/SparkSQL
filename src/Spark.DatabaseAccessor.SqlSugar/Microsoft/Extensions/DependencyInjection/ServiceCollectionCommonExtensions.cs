namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    ///     IOC容器公共扩展类
    /// </summary>
    internal static class ServiceCollectionCommonExtensions
    {
        /// <summary>
        ///     获取单例服务实例，可能为Null
        /// </summary>
        /// <typeparam name="T">服务类型</typeparam>
        /// <param name="services">IOC容器</param>
        /// <returns>单例服务实例或Null</returns>
        internal static T? GetSingletonInstance<T>(this IServiceCollection services)
        {
            return (T?)services.FirstOrDefault(d => d.ServiceType == typeof(T))?.ImplementationInstance;
        }

        /// <summary>
        ///     获取单例服务实例，如果找不到，则抛出异常
        /// </summary>
        /// <typeparam name="T">服务类型</typeparam>
        /// <param name="services">IOC容器</param>
        /// <returns>单例服务实例</returns>
        internal static T TryGetSingletonInstance<T>(this IServiceCollection services)
        {
            var service = services.GetSingletonInstance<T>();
            if (service == null)
            {
                throw new InvalidOperationException("无法找到单例服务实例：" + typeof(T).AssemblyQualifiedName);
            }
            return service;
        }
    }
}
