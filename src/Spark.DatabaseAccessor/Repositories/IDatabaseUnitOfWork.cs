using System.Data;

namespace Spark.DatabaseAccessor.Repositories
{
    /// <summary>
    ///     数据库事务工作单元提供器
    /// </summary>
    public interface IDatabaseUnitOfWork
    {
        /// <summary>
        ///     数据库上下文
        /// </summary>
        object DatabaseContext { get; }

        /// <summary>
        ///     开启事务
        /// </summary>
        void BeginTran();

        /// <summary>
        ///     开启事务
        /// </summary>
        /// <param name="IsolationLevel">事务隔离等级</param>
        void BeginTran(IsolationLevel IsolationLevel);

        /// <summary>
        ///     提交事务
        /// </summary>
        void CommitTran();

        /// <summary>
        ///     回滚事务
        /// </summary>
        void RollbackTran();

        /// <summary>
        ///     销毁
        /// </summary>
        void Dispose();
    }
}
