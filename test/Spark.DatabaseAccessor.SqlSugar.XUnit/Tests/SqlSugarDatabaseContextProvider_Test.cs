using Spark.DatabaseAccessor.Contexts;
using Spark.DatabaseAccessor.SqlSugar.XUnit.Entities;
using Spark.DatabaseAccessor.Utils;
using SqlSugar;
using Xunit;

namespace Spark.DatabaseAccessor.SqlSugar.XUnit.Tests
{
    /// <summary>
    ///     数据库上下文提供器测试案例
    /// </summary>
    [Collection("数据仓储测试案例顺序执行组别")]
    public class SqlSugarDatabaseContextProvider_Test
    {
        /// <summary>
        ///     数据库上下文提供器
        /// </summary>
        private readonly IDatabaseContextProvider _databaseContextProvider;

        /// <summary>
        ///     构造函数
        /// </summary>
        public SqlSugarDatabaseContextProvider_Test(IDatabaseContextProvider databaseContextProvider)
        {
            _databaseContextProvider = databaseContextProvider;
        }

        /// <summary>
        ///     数据库上下文
        /// </summary>
        private ISqlSugarClient _context => (ISqlSugarClient)_databaseContextProvider.GetDatabaseContext();

        /// <summary>
        ///   DatabaseContextProvider_Test数据库上下文提供器测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "数据库上下文提供器测试案例")]
        public async Task DatabaseContextProvider_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _context.DbMaintenance.TruncateTable<User>();
            //插入数据
            var userModel = new User
            {
                Id = id,
                UserName = "小明",
                Password = "123456"
            };
            var insertSuccessedCount = await _context.Insertable(userModel).ExecuteCommandAsync();
            //查询全部
            var userModels = await _context.Queryable<User>().ToListAsync();
            Assert.Equal(userModels.Count, insertSuccessedCount);
        }
    }
}
