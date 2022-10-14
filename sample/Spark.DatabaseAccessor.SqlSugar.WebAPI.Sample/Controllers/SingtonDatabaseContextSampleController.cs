using Microsoft.AspNetCore.Mvc;
using Spark.DatabaseAccessor.SqlSugar.WebAPI.Sample.Entities;

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

            //初始化表
            Database.Context.DbMaintenance.TruncateTable<User>();
            //插入数据
            var userModel = new User
            {
                Id = 1,
                UserName = "小明",
                Password = "123456"
            };
            Database.Context.Insertable(userModel).ExecuteCommand();

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
            //查询全部
            var userModels = await Database.Context.Queryable<User>().ToListAsync();
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
            //查询全部
            var userModels = Database.Context.Queryable<User>().ToList();
            return Ok(userModels);
        }
    }
}