using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Spark.DatabaseAccessor.Options;
using Spark.DatabaseAccessor.SqlSugar.Utils;
using Spark.Extensions;
using SqlSugar;
using System.Text;

namespace Spark.DatabaseAccessor.SqlSugar.Builders
{
    /// <summary>
    ///   数据库日志配置构造器
    /// </summary>
    public class SqlSugarDatabaseSqlLoggingBuilder
    {
        /// <summary>
        ///     构造
        /// </summary>
        /// <returns></returns>
        public static void Build(DatabaseAccessorOptions databaseAccessorOptions, SqlSugarClient databaseClient)
        {
            if (databaseAccessorOptions.PrintSqlLogEnabled)
            {
                var logger = SparkCheck.NotNull(SparkContext.RootServices).GetRequiredService<ILogger<SqlSugarDatabaseSqlLoggingBuilder>>();
                databaseClient.Aop.OnLogExecuted = (sql, p) =>
                {
                    var stringBuilder = new StringBuilder();
                    if (databaseAccessorOptions.PrintSqlLogWithConnectionEnabled)
                    {
                        stringBuilder.AppendLine($"【连接字符串】：{databaseClient.CurrentConnectionConfig.ConnectionString}");
                    }
                    stringBuilder.AppendLine($"【SQL语句】：{SqlSugarSqlProfilerExtensions.ParameterFormat(sql, p)}");
                    stringBuilder.Append($"【耗时】：{databaseClient.Ado.SqlExecutionTime.TotalMilliseconds} ms");
                    if (databaseAccessorOptions.PrintSqlLogExecutionTimeLimit > 0)
                    {
                        if (databaseClient.Ado.SqlExecutionTime.TotalSeconds > databaseAccessorOptions.PrintSqlLogExecutionTimeLimit)
                        {
                            logger.LogDebug(stringBuilder.ToString());
                        }
                    }
                    else
                    {
                        logger.LogDebug(stringBuilder.ToString());
                    }
                };
            }
        }
    }
}
