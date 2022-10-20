using Microsoft.Extensions.DependencyInjection;
using Spark.DatabaseAccessor.SqlSugar.Utils;
using Spark.DatabaseAccessor.SqlSugar.XUnit.Entities;
using Spark.DatabaseAccessor.Utils;
using SqlSugar;
using Xunit;

namespace Spark.DatabaseAccessor.SqlSugar.XUnit.Tests
{
    /// <summary>
    ///     单例数据库上下文测试
    /// </summary>
    [Collection("数据仓储测试案例顺序执行组别")]
    public class SingtonDatabaseContext_Test
    {
        /// <summary>
        ///   SingtonDatabase_Test单例数据库上下文测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "单例数据库上下文测试案例")]
        public async Task SingtonDatabase_Test()
        {
            await SparkScope.CreateAsync(async (serviceScope) =>
            {
                //创建ID标识
                var id = IDGen.SequentialInt64();
                //构建数据库上下文
                var context = serviceScope.ServiceProvider.GetRequiredService<ISqlSugarClient>();
                //初始化表
                context.DbMaintenance.TruncateTable<User>();
                //插入数据
                var userModel = new User
                {
                    Id = id,
                    UserName = "小明",
                    Password = "123456"
                };
                var insertSuccessedCount = await context.Insertable(userModel).ExecuteCommandAsync();
                //查询全部
                var userModels = await context.Queryable<User>().ToListAsync();
                Assert.Equal(userModels.Count, insertSuccessedCount);
            });
        }


        /// <summary>
        ///   ParallelTaskExecuteCommandAsync_Tests 单例数据库上下文多线程测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "单例数据库上下文多线程测试案例")]
        public async Task ParallelTaskExecuteCommandAsync_Tests()
        {
            await SparkScope.CreateAsync(async (serviceScope) =>
            {
                //创建ID标识
                var id = IDGen.SequentialInt64();
                //构建数据库上下文
                var context = serviceScope.ServiceProvider.GetRequiredService<ISqlSugarClient>();
                //初始化表
                context.DbMaintenance.TruncateTable<User>();
                //插入数据
                var userModel = new User
                {
                    Id = id,
                    UserName = "小明",
                    Password = "123456"
                };
                await context.Insertable(userModel).ExecuteCommandAsync();
                var dbContext1 = context;
                var supplierModel1 = await dbContext1.Queryable<User>().FirstAsync(s => s.UserName == "小明");
                var contextID1 = dbContext1.ContextID;
                var tasks = new Task[]
                {
                    Task.Run(async () =>
                    {
                            await SparkScope.CreateAsync(async (serviceScope) =>
                            {
                                //构建数据库上下文
                                var context = serviceScope.ServiceProvider.GetRequiredService<ISqlSugarClient>();
                                var dbContext2=context;
                                var supplierModel2 = await dbContext2.Queryable<User>().Where(s=>s.UserName=="小明").FirstAsync();
                                var contextID2 = dbContext2.ContextID;
                                Assert.NotEqual(contextID1,contextID2);
                            });
                    }),
                    Task.Run(async () =>
                    {
                            await SparkScope.CreateAsync(async (serviceScope) =>
                            {
                                //构建数据库上下文
                                var context = serviceScope.ServiceProvider.GetRequiredService<ISqlSugarClient>();
                                var dbContext3=context;
                                var supplierModel2 = await dbContext3.Queryable<User>().Where(s=>s.UserName=="小明").FirstAsync();
                                var contextID3 = dbContext3.ContextID;
                                Assert.NotEqual(contextID1,contextID3);
                            });
                    }),
                };
                Task.WaitAll(tasks);
                var context4 = serviceScope.ServiceProvider.GetRequiredService<ISqlSugarClient>();
                var supplierModel4 = await context4.Queryable<User>().FirstAsync(s => s.UserName == "小明");
                var contextID4 = context4.ContextID;
                Assert.Equal(contextID1, contextID4);
            });
        }
    }
}
