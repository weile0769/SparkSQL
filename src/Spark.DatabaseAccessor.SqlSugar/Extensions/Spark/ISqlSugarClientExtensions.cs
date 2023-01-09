using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Spark.DatabaseAccessor.Options;
using Spark.DatabaseAccessor.SqlSugar.Utils;
using SqlSugar;
using System.Text;

namespace Spark.DatabaseAccessor.SqlSugar.Extensions
{
    /// <summary>
    ///     SqlSugarClient扩展类
    /// </summary>
    internal static class ISqlSugarClientExtensions
    {
        /// <summary>
        ///     配置SqlSugarClient数据库对象Aop事件
        /// </summary>
        /// <param name="databaseClient">SqlSugarClient数据库对象</param>
        /// <param name="databaseAccessorOptions">数据库访问器全局配置</param>
        /// <returns>SqlSugarClient数据库对象</returns>
        internal static ISqlSugarClient ConfigureAopEvent(this ISqlSugarClient databaseClient, DatabaseAccessorOptions databaseAccessorOptions)
        {
            if (databaseAccessorOptions.PrintSqlLogEnabled)
            {
                var loggerFactory = SparkCheck.NotNull(SparkContext.RootServices).GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger("DatabaseAccessorAopEvent");
                databaseClient.Aop.OnLogExecuted = (sql, p) =>
                {
                    var stringBuilder = new StringBuilder();
                    if (databaseAccessorOptions.PrintSqlLogWithConnectionEnabled)
                    {
                        stringBuilder.AppendLine($"【连接字符串】：{databaseClient.CurrentConnectionConfig.ConnectionString}");
                    }
                    stringBuilder.AppendLine($"【SQL语句】：{sql.ParameterFormat(p)}");
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
            return databaseClient;
        }
    }
}
