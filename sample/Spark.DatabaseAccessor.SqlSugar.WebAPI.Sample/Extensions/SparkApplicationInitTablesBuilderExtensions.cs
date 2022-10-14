using Spark.DatabaseAccessor.SqlSugar;
using Spark.DatabaseAccessor.SqlSugar.WebAPI.Sample.Entities;

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
            Database.Context.CodeFirst.InitTables<User>();
        }
    }
}
