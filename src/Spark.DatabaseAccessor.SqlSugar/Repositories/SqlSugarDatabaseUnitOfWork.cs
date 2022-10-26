using Microsoft.Extensions.Logging;
using Spark.DatabaseAccessor.Repositories;
using Spark.DatabaseAccessor.SqlSugar.Utils;
using SqlSugar;
using System;
using System.Data;

namespace Spark.DatabaseAccessor.SqlSugar.Repositories
{
    /// <summary>
    ///     数据库事务工作单元提供器
    /// </summary>
    internal class SqlSugarDatabaseUnitOfWork : IDatabaseUnitOfWork
    {
        /// <summary>
        ///     日志提供器
        /// </summary>
        private readonly ILogger<SqlSugarDatabaseUnitOfWork> _logger;

        /// <summary>
        ///     数据库连接上下文
        /// </summary>
        private readonly ISqlSugarClient _databaseContext;

        /// <summary>
        ///     构造函数
        /// </summary>
        public SqlSugarDatabaseUnitOfWork(ILogger<SqlSugarDatabaseUnitOfWork> logger,
            IDatabaseContext databaseContextProvider)
        {
            _logger = logger;
            _databaseContext = (SqlSugarProvider)SparkCheck.NotNull(databaseContextProvider.GetDatabaseContext(), nameof(databaseContextProvider));
        }

        /// <summary>
        ///     数据库连接上下文
        /// </summary>
        public object DatabaseContext
        {
            get { return _databaseContext; }
        }

        /// <summary>
        ///     开启事务
        /// </summary>
        public void BeginTran()
        {
            _databaseContext?.Ado.BeginTran();
        }

        /// <summary>
        ///     开启事务
        /// </summary>
        /// <param name="IsolationLevel">事务隔离等级</param>
        public void BeginTran(IsolationLevel IsolationLevel)
        {
            _databaseContext?.Ado.BeginTran(IsolationLevel);
        }

        /// <summary>
        ///     提交事务
        /// </summary>
        public void CommitTran()
        {
            try
            {
                _databaseContext?.Ado.CommitTran();
            }
            catch (Exception ex)
            {
                _databaseContext?.Ado.RollbackTran();
                _logger.LogError($"{ex.Message}\r\n{ex.InnerException}");
            }
            finally
            {
                _databaseContext?.Ado.Dispose();
            }
        }

        /// <summary>
        ///     回滚事务
        /// </summary>
        public void Dispose()
        {
            _databaseContext?.Dispose();
        }

        /// <summary>
        ///     销毁
        /// </summary>
        public void RollbackTran()
        {
            _databaseContext?.Ado.RollbackTran();
        }
    }
}
