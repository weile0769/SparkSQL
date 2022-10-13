using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Spark.DatabaseAccessor.SqlSugar.XUnit.Builders;
using Spark.DatabaseAccessor.SqlSugar.XUnit.Extensions;

namespace Spark.DatabaseAccessor.SqlSugar.XUnit
{
    /// <summary>
    ///   容器服务启动配置
    /// </summary>
    public class Startup
    {
        /// <summary>
        ///   应用程序初始化
        /// </summary>
        /// <param name="hostBuilder"></param>
        public void ConfigureHost(IHostBuilder hostBuilder) =>
            hostBuilder.ConfigureWebHost(webHostBuilder => webHostBuilder
                .UseTestServer()
                .Configure(ConfigureApplication)
                .ConfigureServices(ConfigureServices));


        /// <summary>
        ///   容器服务注册
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            // 注册SqlSugar数据库驱动容器服务
            services.AddSqlSugarDatabaseAccessor(() => TestsDatabaseAccessorOptionsBuilder.Build());
        }

        /// <summary>
        ///   应用程序配置
        /// </summary>
        /// <param name="builder"></param>
        private void ConfigureApplication(IApplicationBuilder builder)
        {
            builder.UseSqlSugarDatabaseAccessor();
            builder.UseApplicationInitTables();
        }
    }
}
