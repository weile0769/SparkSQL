using Spark.DatabaseAccessor.Repositories;
using Spark.DatabaseAccessor.SqlSugar.XUnit.Entities;
using Spark.DatabaseAccessor.Utils;
using Xunit;

namespace Spark.DatabaseAccessor.SqlSugar.XUnit.Tests
{
    /// <summary>
    ///   数据事务工作单元测试案例
    /// </summary>
    [Collection("数据库测试案例顺序执行组别")]
    public class SqlSugarDatabaseUnitOfWork_Test
    {
        /// <summary>
        ///     用户仓储
        /// </summary>
        private readonly IBaseRepository<User> _userRepository;

        /// <summary>
        ///     数据库事务工作单元提供器
        /// </summary>
        private readonly IDatabaseUnitOfWork _databaseUnitOfWork;

        /// <summary>
        ///     构造函数
        /// </summary>
        public SqlSugarDatabaseUnitOfWork_Test(IBaseRepository<User> userRepository,
            IDatabaseUnitOfWork databaseUnitOfWork)
        {
            _userRepository = userRepository;
            _databaseUnitOfWork = databaseUnitOfWork;
        }

        /// <summary>
        ///   UnitOfWorkCommitTranSuccessed_Test事务提交成功测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "事务提交成功测试案例")]
        public async Task UnitOfWorkCommitTranSuccessed_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //创建ID标识
            var id1 = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            var userModel = new User
            {
                Id = id,
                UserName = "小明",
                Password = "123456"
            };
            var userModel1 = new User
            {
                Id = id1,
                UserName = "小花",
                Password = "123456"
            };
            try
            {
                _databaseUnitOfWork.BeginTran();
                await _userRepository.InsertAsync(userModel);
                await _userRepository.InsertAsync(userModel1);
                _databaseUnitOfWork.CommitTran();
            }
            catch
            {
                _databaseUnitOfWork.RollbackTran();
            }

            //查询全部
            var userModels = await _userRepository.QueryAllAsync();
            Assert.Contains(userModel.Id, userModels.Select(s => s.Id));
            Assert.Contains(userModel1.Id, userModels.Select(s => s.Id));
        }

        /// <summary>
        ///   UnitOfWorkCommitTranFail_Test事务提交失败测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "事务提交失败测试案例")]
        public async Task UnitOfWorkCommitTranFail_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            var userModel = new User
            {
                Id = id,
                UserName = "小明",
                Password = "123456"
            };
            try
            {
                _databaseUnitOfWork.BeginTran();
                await _userRepository.InsertAsync(userModel);
                //重复键插入会跳出回滚事务
                await _userRepository.InsertAsync(userModel);
                _databaseUnitOfWork.CommitTran();
            }
            catch
            {
                _databaseUnitOfWork.RollbackTran();
            }

            //查询全部
            var userModels = await _userRepository.QueryAllAsync();
            Assert.Empty(userModels);
            Assert.DoesNotContain(id, userModels.Select(s => s.Id));
        }
    }
}
