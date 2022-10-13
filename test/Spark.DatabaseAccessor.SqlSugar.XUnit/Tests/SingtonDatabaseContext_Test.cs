﻿using Spark.DatabaseAccessor.SqlSugar.XUnit.Entities;
using Xunit;

namespace Spark.DatabaseAccessor.SqlSugar.XUnit.Tests
{
    /// <summary>
    ///     单例数据库上下文测试
    /// </summary>
    public class SingtonDatabaseContext_Test
    {
        /// <summary>
        ///   SingtonDatabase_Test单例数据库上下文测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "单例数据库上下文测试案例")]
        public async Task SingtonDatabase_Test()
        {
            //初始化表
            Database.Context.DbMaintenance.TruncateTable<User>();
            //插入数据
            var userModel = new User
            {
                Id = 1,
                UserName = "小明",
                Password = "123456"
            };
            var insertSuccessedCount = await Database.Context.Insertable(userModel).ExecuteCommandAsync();
            //查询全部
            var userModels = await Database.Context.Queryable<User>().ToListAsync();
            Assert.Equal(userModels.Count, insertSuccessedCount);
        }
    }
}