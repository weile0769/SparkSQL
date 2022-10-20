using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Spark.DatabaseAccessor.SqlSugar.Utils;
using Spark.DatabaseAccessor.SqlSugar.XUnit.Entities;
using SqlSugar;

namespace Spark.DatabaseAccessor.SqlSugar.XUnit.Extensions
{
    /// <summary>
    ///   应用数据结构扩展类
    /// </summary>
    public static class SparkApplicationInitTablesBuilderExtensions
    {
        /// <summary>
        ///   表初始化服务
        /// </summary>
        /// <param name="builder"></param>
        public static void UseApplicationInitTables(this IApplicationBuilder builder)
        {
            SparkScope.Create((serviceScope) =>
            {
                //构建数据库上下文
                var context = serviceScope.ServiceProvider.GetRequiredService<ISqlSugarClient>();
                context.CodeFirst.InitTables<User>();
            });
        }
    }
}
