using Spark.DatabaseAccessor.SqlSugar.Utils;
using SqlSugar;
using System.Collections.Generic;
using System.Linq;

namespace Spark.DatabaseAccessor.SqlSugar.Builders
{
    /// <summary>
    ///     数据库连接配置构造器
    /// </summary>
    public class SqlSugarDatabaseConnectionBuilder
    {
        /// <summary>
        ///   创建Sqlsugar数据库连接配置
        /// </summary>
        /// <returns>Sqlsugar数据库连接配置</returns>
        public static List<ConnectionConfig> Build()
        {
            return SparkCheck.NotNull(SparkContext.DatabaseAccessorOptions).DatabaseConnections.Select(option => new ConnectionConfig
            {
                ConfigId = option.ConnectionName,
                ConnectionString = option.ConnectionString,
                DbType = (DbType)option.DatabaseType,
                IsAutoCloseConnection = option.IsAutoCloseConnection,
                ConfigureExternalServices = new ConfigureExternalServices()
                {
                    EntityService = SqlSugarDatabaseEntitiesPropertiesBuilder.Build,
                    EntityNameService = SqlSugarDatabaseEntitiesTypesBuilder.Build
                },
                SlaveConnectionConfigs = option.EnabledSlaveDatabaseConnections.Select(s => new SlaveConnectionConfig
                {
                    HitRate = s.HitRate,
                    ConnectionString = s.ConnectionString
                }).ToList()
            }).ToList();
        }
    }
}
