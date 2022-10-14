using Microsoft.AspNetCore.Mvc;
using Spark.DatabaseAccessor.SqlSugar.WebAPI.Sample.Entities;

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

            //��ʼ����
            Database.Context.DbMaintenance.TruncateTable<User>();
            //��������
            var userModel = new User
            {
                Id = 1,
                UserName = "С��",
                Password = "123456"
            };
            Database.Context.Insertable(userModel).ExecuteCommand();

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
            //��ѯȫ��
            var userModels = await Database.Context.Queryable<User>().ToListAsync();
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
            //��ѯȫ��
            var userModels = Database.Context.Queryable<User>().ToList();
            return Ok(userModels);
        }
    }
}