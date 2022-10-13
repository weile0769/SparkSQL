using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    ///     配置服务扩展类
    /// </summary>
    internal static class ServiceCollectionConfigurationExtensions
    {
        /// <summary>
        ///     获取配置服务提供器，如果找不到，则抛出异常
        /// </summary>
        /// <param name="services">IOC容器</param>
        /// <returns>配置服务</returns>
        public static IConfiguration TryGetConfiguration(this IServiceCollection services)
        {
            var hostBuilderContext = services.GetSingletonInstance<HostBuilderContext>();
            if (hostBuilderContext?.Configuration != null)
            {
                return (IConfigurationRoot)hostBuilderContext.Configuration;
            }
            return services.TryGetSingletonInstance<IConfiguration>();
        }
    }
}
