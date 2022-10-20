using Microsoft.AspNetCore.Mvc;
using Spark.DatabaseAccessor.SqlSugar.Utils;
using Spark.DatabaseAccessor.SqlSugar.WebAPI.Sample.Entities;
using Spark.DatabaseAccessor.Utils;
using SqlSugar;

namespace Spark.DatabaseAccessor.SqlSugar.WebAPI.Sample.Controllers
{
    /// <summary>
    ///     �������ݿ������İ���
    /// </summary>
    [ApiController]
    [Route("samples")]
    public class SingtonDatabaseContextSampleController : ControllerBase
    {
        /// <summary>
        ///     ���캯��
        /// </summary>
        public SingtonDatabaseContextSampleController()
        {
            #region ��ʼ��
            SparkScope.Create((serviceScope) =>
            {
                //�������ݿ�������
                var context = serviceScope.ServiceProvider.GetRequiredService<ISqlSugarClient>();
                //��ʼ����
                context.DbMaintenance.TruncateTable<User>();
                //��������
                var userModel = new User
                {
                    Id = IDGen.SequentialInt64(),
                    UserName = "С��",
                    Password = "123456"
                };
                var insertSuccessedCount = context.Insertable(userModel).ExecuteCommand();
            });
            #endregion
        }

        /// <summary>
        ///     �û������첽��ѯ��������
        /// </summary>
        /// <returns>�û�����</returns>
        [HttpGet]
        [Route("users/query-async")]
        public async Task<IActionResult> GetAllAsync()
        {
            List<User>? userModels = null;
            await SparkScope.CreateAsync(async (serviceScope) =>
            {
                //�������ݿ�������
                var context = serviceScope.ServiceProvider.GetRequiredService<ISqlSugarClient>();
                //��ѯȫ��
                userModels = await context.Queryable<User>().ToListAsync();
            });
            return Ok(userModels);
        }

        /// <summary>
        ///     �û�����ͬ����ѯ��������
        /// </summary>
        /// <returns>�û�����</returns>
        [HttpGet]
        [Route("users/query")]
        public IActionResult GetAll()
        {
            List<User>? userModels = null;
            SparkScope.Create((serviceScope) =>
            {
                //�������ݿ�������
                var context = serviceScope.ServiceProvider.GetRequiredService<ISqlSugarClient>();
                //��ѯȫ��
                userModels = context.Queryable<User>().ToList();
            });
            return Ok(userModels);
        }
    }
}