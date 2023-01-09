using Spark.DatabaseAccessor.SqlSugar.Utils;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    ///     数据库操作提供器应用构建扩展类
    /// </summary>
    public static class IApplicationBuilderExtensions
    {
        /// <summary>
        ///     配置数据库操作提供器应用构建
        /// </summary>
        /// <param name="builder">应用构造器</param>
        /// <returns></returns>
        public static void UseSparkDatabaseAccessor(this IApplicationBuilder builder)
        {
            builder.UseSqlSugarDatabaseAccessor();
        }

        /// <summary>
        ///     配置数据库操作提供器应用构建
        /// </summary>
        /// <param name="builder">应用构造器</param>
        /// <returns></returns>
        private static void UseSqlSugarDatabaseAccessor(this IApplicationBuilder builder)
        {
            SparkContext.ConfigureRootServices(builder.ApplicationServices);
        }
    }
}
