using System;
using System.Collections.Generic;
using System.Linq;

namespace Spark.DatabaseAccessor.Options
{
    /// <summary>
    ///   数据库访问器全局配置
    /// </summary>
    public class DatabaseAccessorOptions
    {
        /// <summary>
        ///     配置项节点名称
        /// </summary>
        public const string OptionName = "DatabaseAccessorOptions";

        /// <summary>
        ///   数据库默认连接配置
        /////// </summary>
        public DatabaseConnections? Default
        {
            get
            {
                return DatabaseConnections.FirstOrDefault(s => s.ConnectionName.IsNotNullOrWhiteSpace() && s.ConnectionName.Equals(DefaultConnectionName) && s.Enabled);
            }
        }

        /// <summary>
        ///   数据库默认连接配置
        /// </summary>
        public IEnumerable<DatabaseConnections> Defaults
        {
            get
            {
                return DatabaseConnections.Where(s => s.ConnectionName.IsNotNullOrWhiteSpace() && s.ConnectionName.Equals(DefaultConnectionName) && s.Enabled);
            }
        }


        /// <summary>
        ///   构造函数
        /// </summary>
        public DatabaseAccessorOptions()
        {
            DefaultConnectionName = "Default";
            DatabaseConnections = new List<DatabaseConnections>();
        }

        /// <summary>
        ///   当前主库连接标识
        /// </summary>
        public string DefaultConnectionName { get; set; } = "Default";

        /// <summary>
        ///   是否支持读写分离
        /// </summary>
        public bool CQRSEnabled { get; set; }

        /// <summary>
        ///   是否开启打印SQL日志
        /// </summary>
        public bool PrintSqlLogEnabled { get; set; }

        /// <summary>
        ///   是否开启打印SQL日志过程携带连接字符串
        /// </summary>
        public bool PrintSqlLogWithConnectionEnabled { get; set; }

        /// <summary>
        ///   SQL日志监控耗时上限值
        ///   说明：
        ///   1.SQL耗时到达上限值时才触发日志监控事件
        ///   2.需配合PrintSqlLogEnabled设置true才触发
        /// </summary>
        public int PrintSqlLogExecutionTimeLimit { get; set; }


        /// <summary>
        ///   获取启用的数据库连接配置[主库]
        /// </summary>
        /// <returns>启用的数据库连接配置[主库]</returns>
        public IEnumerable<DatabaseConnections> EnabledDatabaseConnections
        {
            get => DatabaseConnections.Where(s => s.Enabled);
        }

        /// <summary>
        ///   数据库连接配置项
        /// </summary>
        public List<DatabaseConnections> DatabaseConnections { get; set; }

    }

    /// <summary>
    ///   数据库连接配置[主库]
    /// </summary>
    public class DatabaseConnections
    {
        /// <summary>
        ///   构造函数
        /// </summary>
        public DatabaseConnections()
        {
            IsAutoCloseConnection = true;
            SlaveDatabaseConnections = new List<SlaveDatabaseConnections>();
        }

        /// <summary>
        ///   获取启用的数据库连接配置[从库]
        /// </summary>
        /// <returns>启用的数据库连接配置[从库]</returns>
        public IEnumerable<SlaveDatabaseConnections> EnabledSlaveDatabaseConnections
        {
            get => SlaveDatabaseConnections.Where(s => s.Enabled);
        }


        /// <summary>
        ///   配置项开关
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        ///   数据库连接标识
        /// </summary>
        public string ConnectionName { get; set; } = string.Empty;

        /// <summary>
        ///   数据库架构
        /// </summary>
        public string? Schema { get; set; }


        /// <summary>
        ///   数据库连接字符串
        /// </summary>
        public string ConnectionString { get; set; } = string.Empty;

        /// <summary>
        ///   数据库连接配置[从库]
        /// </summary>
        public List<SlaveDatabaseConnections> SlaveDatabaseConnections { get; set; }


        /// <summary>
        ///   是否自动关闭数据库连接
        ///   说明：默认开启自动关闭
        /// </summary>
        public bool IsAutoCloseConnection { get; set; }

        /// <summary>
        ///   数据库类型
        /// </summary>
        public DatabaseType DatabaseType { get; set; }
    }

    /// <summary>
    ///   数据库连接配置
    /// </summary>
    public class SlaveDatabaseConnections
    {

        /// <summary>
        ///   配置项开关
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        ///   数据库连接标识
        /// </summary>
        public string? ConnectionName { get; set; }

        /// <summary>
        ///   从库执行级别
        ///   说明：值越大越优先匹配执行  
        /// </summary>
        public int HitRate { get; set; }

        /// <summary>
        ///   数据库连接字符串
        /// </summary>
        public string? ConnectionString { get; set; }
    }


    /// <summary>
    ///   支持的数据库类型
    /// </summary>
    public enum DatabaseType
    {
        /// <summary>
        ///   MySql
        /// </summary>
        MySql = 0,
        /// <summary>
        ///   SqlServer
        /// </summary>
        SqlServer = 1,
        /// <summary>
        ///   Sqlite
        /// </summary>
        Sqlite = 2,
        /// <summary>
        ///   Oracle
        /// </summary>
        Oracle = 3,
        /// <summary>
        ///   PostgreSQL
        /// </summary>
        PostgreSQL = 4,
        /// <summary>
        ///   Dm
        /// </summary>
        Dm = 5,
        /// <summary>
        ///   Kdbndp
        /// </summary>
        Kdbndp = 6,
        /// <summary>
        ///   Oscar
        /// </summary>
        Oscar = 7,
        /// <summary>
        ///   MySqlConnector
        /// </summary>
        MySqlConnector = 8,
        /// <summary>
        ///   Access
        /// </summary>
        Access = 9,
        /// <summary>
        ///   Custom
        /// </summary>
        Custom = 900
    }
}
