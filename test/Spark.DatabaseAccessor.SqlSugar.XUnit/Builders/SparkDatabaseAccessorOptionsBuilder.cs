using Spark.DatabaseAccessor.Options;

namespace Spark.DatabaseAccessor.SqlSugar.XUnit.Builders
{
    /// <summary>
    ///   数据库配置生成器
    /// </summary>
    public static class SparkDatabaseAccessorOptionsBuilder
    {
        public static string GetCurrentProjectPath
        {

            get
            {
                var baseDirectoryPath = Environment.CurrentDirectory;
                //向上一回退三级，得到需要目录
                string? rootPath = baseDirectoryPath.Substring(0, baseDirectoryPath.LastIndexOf(@"\"));
                //rootPath = rootPath.Substring(0, rootPath.LastIndexOf(@"\"));
                rootPath = rootPath.Substring(0, rootPath.LastIndexOf(@"\"));
                rootPath = rootPath.Substring(0, rootPath.LastIndexOf(@"\"));
                rootPath = rootPath.Substring(0, rootPath.LastIndexOf(@"\"));
                rootPath = rootPath.Substring(0, rootPath.LastIndexOf(@"\"));
                return @$"{rootPath}\database";
            }
        }


        /// <summary>
        ///   构建配置
        /// </summary>
        public static DatabaseAccessorOptions Build()
        {
            return new DatabaseAccessorOptions
            {
                DefaultConnectionName = "spark",
                CQRSEnabled = true,
                PrintSqlLogEnabled = true,
                PrintSqlLogWithConnectionEnabled = true,
                PrintSqlLogExecutionTimeLimit = 0,
                DatabaseConnections = new List<DatabaseConnections>
            {
                new DatabaseConnections
                {
                    Enabled = true,
                    ConnectionName = "spark",
                    DatabaseType= DatabaseType.Sqlite,
                    ConnectionString=@$"DataSource={GetCurrentProjectPath}\spark.db",
                    IsAutoCloseConnection = true
                },
                new DatabaseConnections
                {
                    Enabled = true,
                    ConnectionName = "spark1",
                    DatabaseType= DatabaseType.Sqlite,
                    ConnectionString=@$"DataSource={GetCurrentProjectPath}\spark.db",
                    IsAutoCloseConnection = true
                }
            }
            };
        }
    }
}
