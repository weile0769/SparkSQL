using Spark.DatabaseAccessor.SqlSugar.Builders;
using Xunit;

namespace Spark.DatabaseAccessor.SqlSugar.XUnit.Tests
{
    /// <summary>
    ///     数据库连接配置构造器测试案例
    /// </summary>
    public class SqlSugarDatabaseConnectionBuilder_Test
    {
        /// <summary>
        ///   DatabaseConnectionBuild_Test创建数据库连接配置测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "创建数据库连接配置测试案例")]
        public void DatabaseConnectionBuild_Test()
        {
            var options = SqlSugarDatabaseConnectionBuilder.Build();
            Assert.Equal(2, options.Count);
        }
    }
}
