using Spark.DatabaseAccessor.SqlSugar.Utils;
using Spark.DatabaseAccessor.SqlSugar.WebAPI.Sample.Entities;
using SqlSugar;

namespace Microsoft.AspNetCore.Builder
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
        public static void UseSparkApplicationInitTables(this IApplicationBuilder builder)
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
