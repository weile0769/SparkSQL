using Microsoft.AspNetCore.Mvc;
using Spark.DatabaseAccessor.SqlSugar.Utils;
using Spark.DatabaseAccessor.SqlSugar.WebAPI.Sample.Entities;
using Spark.DatabaseAccessor.Utils;
using SqlSugar;

namespace Spark.DatabaseAccessor.SqlSugar.WebAPI.Sample.Controllers
{
    /// <summary>
    ///     单例数据库上下文案例
    /// </summary>
    [ApiController]
    [Route("samples")]
    public class SingtonDatabaseContextSampleController : ControllerBase
    {
        /// <summary>
        ///     构造函数
        /// </summary>
        public SingtonDatabaseContextSampleController()
        {
            #region 初始化
            SparkScope.Create((serviceScope) =>
            {
                //构建数据库上下文
                var context = serviceScope.ServiceProvider.GetRequiredService<ISqlSugarClient>();
                //初始化表
                context.DbMaintenance.TruncateTable<User>();
                //插入数据
                var userModel = new User
                {
                    Id = IDGen.SequentialInt64(),
                    UserName = "小明",
                    Password = "123456"
                };
                var insertSuccessedCount = context.Insertable(userModel).ExecuteCommand();
            });
            #endregion
        }

        /// <summary>
        ///     用户数据异步查询案例测试
        /// </summary>
        /// <returns>用户数据</returns>
        [HttpGet]
        [Route("users/query-async")]
        public async Task<IActionResult> GetAllAsync()
        {
            List<User>? userModels = null;
            await SparkScope.CreateAsync(async (serviceScope) =>
            {
                //构建数据库上下文
                var context = serviceScope.ServiceProvider.GetRequiredService<ISqlSugarClient>();
                //查询全部
                userModels = await context.Queryable<User>().ToListAsync();
            });
            return Ok(userModels);
        }

        /// <summary>
        ///     用户数据同步查询案例测试
        /// </summary>
        /// <returns>用户数据</returns>
        [HttpGet]
        [Route("users/query")]
        public IActionResult GetAll()
        {
            List<User>? userModels = null;
            SparkScope.Create((serviceScope) =>
            {
                //构建数据库上下文
                var context = serviceScope.ServiceProvider.GetRequiredService<ISqlSugarClient>();
                //查询全部
                userModels = context.Queryable<User>().ToList();
            });
            return Ok(userModels);
        }
    }
}