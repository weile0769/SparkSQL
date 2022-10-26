using Microsoft.Extensions.Options;
using Spark.DatabaseAccessor.Attributes;
using Spark.DatabaseAccessor.Models;
using Spark.DatabaseAccessor.Options;
using Spark.DatabaseAccessor.Repositories;
using Spark.DatabaseAccessor.SqlSugar.Extensions;
using Spark.DatabaseAccessor.SqlSugar.Utils;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Spark.DatabaseAccessor.SqlSugar.Repositories
{
    #region 异步数据仓储

    /// <summary>
    ///     非泛型数据仓储
    /// </summary>
    internal class SqlSugarDatabaseRepository : IDatabaseRepository
    {
        /// <summary>
        ///     SqlSugar数据库连接对象
        /// </summary>
        private readonly ISqlSugarClient _sqlSugarClient;

        /// <summary>
        ///   构造函数
        /// </summary>
        public SqlSugarDatabaseRepository(ISqlSugarClient sqlSugarClient)
        {
            _sqlSugarClient = SparkCheck.NotNull(sqlSugarClient, nameof(sqlSugarClient));
        }

        /// <summary>
        ///     数据库上下文
        /// </summary>
        public object DbContext => _sqlSugarClient;
    }


    /// <summary>
    ///     泛型数据仓储
    /// </summary>
    internal partial class SqlSugarDatabaseRepository<TEntity> : IDatabaseRepository<TEntity> where TEntity : class, new()
    {
        /// <summary>
        ///     SqlSugar数据库连接对象
        /// </summary>
        private readonly ISqlSugarClient _sqlSugarClient;

        /// <summary>
        ///     数据库访问器全局配置
        /// </summary>
        private readonly DatabaseAccessorOptions _databaseAccessorOptions;

        /// <summary>
        ///   构造函数
        /// </summary>
        public SqlSugarDatabaseRepository(IOptions<DatabaseAccessorOptions> options,
            ISqlSugarClient sqlSugarClient)
        {
            _sqlSugarClient = SparkCheck.NotNull(sqlSugarClient, nameof(sqlSugarClient));
            _databaseAccessorOptions = SparkCheck.NotNull(options.Value, nameof(options));
        }

        #region 数据库上下文

        /// <summary>
        ///     数据库上下文
        /// </summary>
        private ISqlSugarClient _dbContext => (ISqlSugarClient)DbContext;

        /// <summary>
        ///     数据库上下文
        /// </summary>
        public object DbContext
        {
            get
            {
                var connectionName = _databaseAccessorOptions.DefaultConnectionName;
                if (_databaseAccessorOptions.DatabaseConnections.Count > 1)
                {
                    //ToDo:实体特性缓存
                    if (typeof(TEntity).GetTypeInfo().GetCustomAttributes(typeof(SqlTableAttribute), true)
                    .FirstOrDefault(x => x.GetType() == typeof(SqlTableAttribute)) is SqlTableAttribute sqlTable
                    && sqlTable.ConnectionName.IsNotNullOrWhiteSpace())
                    {
                        connectionName = sqlTable.ConnectionName;
                    }
                    _sqlSugarClient.AsTenant().ChangeDatabase(connectionName);
                }
                return _sqlSugarClient;
            }
        }


        #endregion

        #region QueryAsync


        /// <summary>
        ///     查询所有数据
        /// </summary>
        /// <returns>所有数据</returns>
        public Task<List<TEntity>> QueryAllAsync()
        {
            return _dbContext.Queryable<TEntity>().ToListAsync();
        }

        /// <summary>
        ///     查询数据列表
        /// <remarks>
        ///    根据SQL条件查询数据列表
        /// </remarks>
        /// </summary>
        /// <param name="whereSql">查询条件</param>
        /// <returns>数据列表</returns>
        public Task<List<TEntity>> QueryAsync(string whereSql)
        {
            return _dbContext.Queryable<TEntity>().WhereIF(whereSql.IsNotNullOrWhiteSpace(), whereSql).ToListAsync();
        }

        /// <summary>
        ///     查询数据列表
        /// <remarks>
        ///    Lambda条件表达式查询数据列表
        /// </remarks>
        /// </summary>
        /// <param name="whereExpression">查询表达式</param>
        /// <returns>数据列表</returns>
        public Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> whereExpression)
        {
            return _dbContext.Queryable<TEntity>().WhereIF(whereExpression != null, whereExpression).ToListAsync();
        }

        /// <summary>
        ///     查询单条数据
        /// </summary>
        /// <remarks>
        ///    Lambda条件表达式查询单条数据,如果查询到多条数据会抛出异常
        /// </remarks>
        /// <param name="whereExpression">查询表达式</param>
        /// <returns>单条数据</returns>
        public Task<TEntity> QuerySingleAsync(Expression<Func<TEntity, bool>> whereExpression)
        {
            return _dbContext.Queryable<TEntity>().SingleAsync(whereExpression);
        }

        /// <summary>
        ///     查询单条数据
        /// </summary>
        /// <remarks>
        ///    Lambda条件表达式查询单条数据，支持部分列查询,如果查询到多条数据会抛出异常
        /// </remarks>
        /// <param name="whereExpression">查询表达式</param>
        /// <param name="fieldColumns">查询列</param>
        /// <returns>单条数据</returns>
        public Task<TEntity> QuerySingleAsync(Expression<Func<TEntity, bool>> whereExpression, string fieldColumns)
        {
            return _dbContext.Queryable<TEntity>().Select(fieldColumns).SingleAsync(whereExpression);
        }

        /// <summary>
        ///     查询单条数据
        /// </summary>
        /// <remarks>
        ///   Lambda条件表达式查询单条数据，如果查询到多条数据只取其中一条
        /// </remarks>
        /// <param name="whereExpression">查询表达式</param>
        /// <returns>单条数据</returns>
        public Task<TEntity> QueryFirstOrDefaultAsync(Expression<Func<TEntity, bool>> whereExpression)
        {
            return _dbContext.Queryable<TEntity>().FirstAsync(whereExpression);
        }

        /// <summary>
        ///     查询单条数据
        /// </summary>
        /// <remarks>
        ///   Lambda条件表达式查询单条数据，支持部分列查询,如果查询到多条数据只取其中一条
        /// </remarks>
        /// <param name="whereExpression">查询表达式</param>
        /// <param name="fieldColumns">查询列</param>
        /// <returns>单条数据</returns>
        public Task<TEntity> QueryFirstOrDefaultAsync(Expression<Func<TEntity, bool>> whereExpression, string fieldColumns)
        {
            return _dbContext.Queryable<TEntity>().Select(fieldColumns).FirstAsync(whereExpression);
        }

        /// <summary>
        ///     查询数据列表
        /// </summary>
        /// <param name="whereExpression">查询表达式</param>
        /// <param name="orderByFileds">排序条件</param>
        /// <returns>数据列表</returns>
        public Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> whereExpression, string orderByFileds)
        {
            return _dbContext.Queryable<TEntity>().WhereIF(whereExpression != null, whereExpression).OrderByIF(orderByFileds.IsNotNullOrWhiteSpace(), orderByFileds).ToListAsync();
        }

        /// <summary>
        ///     查询数据列表
        /// </summary>
        /// <param name="whereExpression">查询表达式</param>
        /// <param name="orderByExpression">排序表达式</param>
        /// <param name="isAsc">是否升序</param>
        /// <returns>数据列表</returns>
        public Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool isAsc = true)
        {
            return _dbContext.Queryable<TEntity>().OrderByIF(orderByExpression != null, orderByExpression, isAsc ? OrderByType.Asc : OrderByType.Desc).WhereIF(whereExpression != null, whereExpression).ToListAsync();
        }

        /// <summary>
        ///     查询数据列表
        /// </summary>
        /// <param name="whereSql">查询表达式</param>
        /// <param name="orderByFileds">排序条件</param>
        /// <returns>数据列表</returns>
        public Task<List<TEntity>> QueryAsync(string whereSql, string orderByFileds)
        {
            return _dbContext.Queryable<TEntity>().OrderByIF(orderByFileds.IsNotNullOrWhiteSpace(), orderByFileds).WhereIF(orderByFileds.IsNotNullOrWhiteSpace(), whereSql).ToListAsync();
        }


        /// <summary>
        ///     查询前N条数据
        /// </summary>
        /// <param name="whereExpression">查询表达式</param>
        /// <param name="intTop">前N条</param>
        /// <param name="orderByFileds">排序条件</param>
        /// <returns>前N条数据</returns>
        public Task<List<TEntity>> QueryTopAsync(Expression<Func<TEntity, bool>> whereExpression, int intTop, string orderByFileds)
        {
            return _dbContext.Queryable<TEntity>().OrderByIF(orderByFileds.IsNotNullOrWhiteSpace(), orderByFileds).WhereIF(whereExpression != null, whereExpression).Take(intTop).ToListAsync();
        }

        /// <summary>
        ///     查询前N条数据
        /// </summary>
        /// <param name="whereSql">查询条件</param>
        /// <param name="intTop">前N条</param>
        /// <param name="orderByFileds">排序条件</param>
        /// <returns>前N条数据</returns>
        public Task<List<TEntity>> QueryTopAsync(string whereSql, int intTop, string orderByFileds)
        {
            return _dbContext.Queryable<TEntity>().OrderByIF(orderByFileds.IsNotNullOrWhiteSpace(), orderByFileds).WhereIF(whereSql.IsNotNullOrWhiteSpace(), whereSql).Take(intTop).ToListAsync();
        }

        /// <summary>
        ///     查询数据列表
        /// </summary>
        /// <param name="sql">完整的SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>数据列表</returns>
        public Task<List<TEntity>> QuerySqlAsync(string sql, object? parameters = default)
        {
            return _dbContext.Ado.SqlQueryAsync<TEntity>(sql, parameters);
        }

        /// <summary>
        ///     查询数据列表
        /// </summary>
        /// <param name="sql">完整的SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>DataTable</returns>
        public Task<DataTable> QueryTableAsync(string sql, object? parameters = default)
        {
            return _dbContext.Ado.GetDataTableAsync(sql, parameters);
        }


        /// <summary>
        ///     查询数据列表
        /// </summary>
        /// <param name="whereExpression">查询表达式</param>
        /// <param name="pageIndex">页码（下标0）</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="orderByFileds">排序条件</param>
        /// <returns>数据列表</returns>
        public Task<PageModel<TEntity>> QueryPageAsync(Expression<Func<TEntity, bool>> whereExpression, int pageIndex = 1, int pageSize = 20, string? orderByFileds = default)
        {
            return _dbContext.Queryable<TEntity>().OrderByIF(orderByFileds.IsNotNullOrWhiteSpace(), orderByFileds).WhereIF(whereExpression != null, whereExpression).ToPagedListAsync(pageIndex, pageSize);
        }


        /// <summary>
        ///     获取总记录数
        /// </summary>
        /// <param name="whereExpression">查询表达式</param>
        /// <returns>总记录数</returns>
        public Task<int> CountAsync(Expression<Func<TEntity, bool>> whereExpression)
        {
            return _dbContext.Queryable<TEntity>().CountAsync(whereExpression);
        }

        /// <summary>
        ///     检查是否存在
        /// </summary>
        /// <param name="whereExpression">查询表达式</param>
        /// <returns>是否存在</returns>
        public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> whereExpression)
        {
            return _dbContext.Queryable<TEntity>().AnyAsync(whereExpression);
        }

        #endregion

        #region QueryForEachAsync

        /// <summary>
        ///     查询数据列表[分批]
        /// <remarks>
        ///    Lambda条件表达式查询数据列表
        /// </remarks>
        /// </summary>
        /// <param name="whereExpression">查询表达式</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> QueryForEachAsync(Expression<Func<TEntity, bool>> whereExpression)
        {
            var list = new List<TEntity>();
            await _dbContext.Queryable<TEntity>().WhereIF(whereExpression != null, whereExpression).ForEachAsync(p =>
            {
                list.Add(p);
            }, 200);
            return list;
        }

        #endregion

        #region InsertAsync

        /// <summary>
        ///     单条插入
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns>影响行数</returns>
        public Task<int> InsertAsync(TEntity entity)
        {
            return _dbContext.Insertable(entity).ExecuteCommandAsync();
        }

        /// <summary>
        ///     单条插入
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="insertColumns">指定只插入列</param>
        /// <returns>影响行数</returns>
        public Task<int> InsertAsync(TEntity entity, Expression<Func<TEntity, object>>? insertColumns = default)
        {
            var insert = _dbContext.Insertable(entity);
            if (insertColumns == null)
            {
                return insert.ExecuteCommandAsync();
            }
            return insert.InsertColumns(insertColumns).ExecuteCommandAsync();
        }

        /// <summary>
        ///     批量插入
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <returns>影响行数</returns>
        public Task<int> InsertAsync(List<TEntity> entities)
        {
            return _dbContext.Insertable(entities).UseParameter().ExecuteCommandAsync();
        }

        /// <summary>
        ///     批量插入
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <param name="insertColumns">指定只插入列</param>
        /// <returns>影响行数</returns>
        public Task<int> InsertAsync(List<TEntity> entities, Expression<Func<TEntity, object>>? insertColumns = default)
        {
            var insert = _dbContext.Insertable(entities);
            if (insertColumns == null)
            {
                return insert.UseParameter().ExecuteCommandAsync();
            }
            return insert.InsertColumns(insertColumns).UseParameter().ExecuteCommandAsync();
        }

        #endregion

        #region UpdateAsync

        /// <summary>
        ///     单条更新
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns>影响行数</returns>
        public Task<int> UpdateAsync(TEntity entity)
        {
            return _dbContext.Updateable(entity).ExecuteCommandAsync();
        }

        /// <summary>
        ///     单条更新
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="updateColumns">更新列</param>
        /// <returns></returns>
        public Task<int> UpdateAsync(TEntity entity, Expression<Func<TEntity, object>> updateColumns)
        {
            return _dbContext.Updateable(entity).UpdateColumns(updateColumns).ExecuteCommandAsync();
        }

        /// <summary>
        ///     单条更新
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="updateColumns">更新列</param>
        /// <param name="whereExpression">查询条件表达式</param>
        /// <returns></returns>
        public Task<int> UpdateAsync(TEntity entity, Expression<Func<TEntity, object>> updateColumns, Expression<Func<TEntity, bool>> whereExpression)
        {
            return _dbContext.Updateable(entity).UpdateColumns(updateColumns).Where(whereExpression).ExecuteCommandAsync();
        }

        /// <summary>
        ///     单条更新
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="whereSql">查询条件</param>
        /// <returns>影响行数</returns>
        public Task<int> UpdateAsync(TEntity entity, string whereSql)
        {
            return _dbContext.Updateable(entity).Where(whereSql).ExecuteCommandAsync();
        }

        /// <summary>
        ///     单条更新
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="whereExpression">查询条件表达式</param>
        /// <returns>影响行数</returns>
        public Task<int> UpdateAsync(TEntity entity, Expression<Func<TEntity, bool>> whereExpression)
        {
            return _dbContext.Updateable(entity).Where(whereExpression).ExecuteCommandAsync();
        }

        /// <summary>
        ///     单条更新
        /// </summary>
        /// <param name="setColumns">设置列</param>
        /// <param name="whereExpression">查询条件表达式</param>
        /// <returns>影响行数</returns>
        public Task<int> UpdateAsync(Expression<Func<TEntity, TEntity>> setColumns, Expression<Func<TEntity, bool>> whereExpression)
        {
            return _dbContext.Updateable<TEntity>().SetColumns(setColumns).Where(whereExpression).ExecuteCommandAsync();
        }


        /// <summary>
        ///     单条更新
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="updateColumns">更新列</param>
        /// <returns>影响行数</returns>
        public Task<int> UpdateAsync(TEntity entity, string[] updateColumns)
        {
            return _dbContext.Updateable(entity).UpdateColumns(updateColumns).ExecuteCommandAsync();
        }

        /// <summary>
        ///     单条更新
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="updateColumns">更新列</param>
        /// <param name="whereExpression">查询条件表达式</param>
        /// <returns>影响行数</returns>
        public Task<int> UpdateAsync(TEntity entity, string[] updateColumns, Expression<Func<TEntity, bool>> whereExpression)
        {
            return _dbContext.Updateable(entity).UpdateColumns(updateColumns).Where(whereExpression).ExecuteCommandAsync();
        }

        /// <summary>
        ///     批量更新
        /// </summary>
        /// <param name="entities">实体类</param>
        /// <returns></returns>
        public Task<int> UpdateAsync(List<TEntity> entities)
        {
            return _dbContext.Updateable(entities).ExecuteCommandAsync();
        }


        /// <summary>
        ///     批量更新
        /// </summary>
        /// <param name="entities">实体类</param>
        /// <param name="updateColumns">更新列</param>
        /// <returns></returns>
        public Task<int> UpdateAsync(List<TEntity> entities, Expression<Func<TEntity, object>> updateColumns)
        {
            return _dbContext.Updateable(entities).UpdateColumns(updateColumns).ExecuteCommandAsync();
        }

        #endregion

        #region DeleteAsync

        /// <summary>
        ///     单条删除
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns>影响行数</returns>
        public Task<int> DeleteAsync(TEntity entity)
        {
            return _dbContext.Deleteable(entity).ExecuteCommandAsync();
        }

        /// <summary>
        ///     条件删除
        /// </summary>
        /// <param name="deleteExpression">查询条件表达式</param>
        /// <returns>影响行数</returns>
        public Task<int> DeleteAsync(Expression<Func<TEntity, bool>> deleteExpression)
        {
            return _dbContext.Deleteable<TEntity>().Where(deleteExpression).ExecuteCommandAsync();
        }


        /// <summary>
        ///     批量删除
        /// </summary>
        /// <param name="entities">实体集</param>
        /// <returns>影响行数</returns>
        public Task<int> DeleteAsync(List<TEntity> entities)
        {
            return _dbContext.Deleteable(entities).ExecuteCommandAsync();
        }

        #endregion

        #region TruncateTable

        /// <summary>
        ///   初始化表
        /// <remarks>
        ///  表中数据全部清空，清除，自增初始化
        /// </remarks>
        /// </summary>
        /// <returns></returns>
        public bool TruncateTable()
        {
            return _dbContext.DbMaintenance.TruncateTable<TEntity>();
        }

        #endregion

        #region SaveableAsync

        /// <summary>
        ///     单条保存
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        public async Task SaveableAsync(TEntity entity)
        {
            var storage = _dbContext.Storageable(entity).Saveable().ToStorage();
            await storage.AsInsertable.ExecuteCommandAsync();
            await storage.AsUpdateable.ExecuteCommandAsync();
        }

        /// <summary>
        ///     单条保存
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="updateColumns">更新列</param>
        /// <returns></returns>
        public async Task SaveableAsync(TEntity entity, Expression<Func<TEntity, object>> updateColumns)
        {
            var storage = _dbContext.Storageable(entity).Saveable().ToStorage();
            await storage.AsInsertable.ExecuteCommandAsync();
            await storage.AsUpdateable.UpdateColumns(updateColumns).ExecuteCommandAsync();
        }

        /// <summary>
        ///     批量保存
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <returns></returns>
        public async Task SaveableAsync(List<TEntity> entities)
        {
            var storage = _dbContext.Storageable(entities).Saveable().ToStorage();
            await storage.AsInsertable.ExecuteCommandAsync();
            await storage.AsUpdateable.ExecuteCommandAsync();
        }

        /// <summary>
        ///     批量保存
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <param name="updateColumns">更新列</param>
        /// <returns></returns>
        public async Task SaveableAsync(List<TEntity> entities, Expression<Func<TEntity, object>> updateColumns)
        {
            var storage = _dbContext.Storageable(entities).Saveable().ToStorage();
            await storage.AsInsertable.ExecuteCommandAsync();
            await storage.AsUpdateable.UpdateColumns(updateColumns).ExecuteCommandAsync();
        }


        #endregion

        #region BulkCopyAsync

        /// <summary>
        ///     大数据插入
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <returns>影响行数</returns>
        public Task<int> BulkCopyAsync(List<TEntity> entities)
        {
            return _dbContext.Fastest<TEntity>().BulkCopyAsync(entities);
        }

        /// <summary>
        ///     大数据插入
        /// </summary>
        /// <param name="tableName">表名字</param>
        /// <param name="dataTable">数据集</param>
        /// <returns></returns>
        public Task<int> BulkCopyAsync(string tableName, DataTable dataTable)
        {
            return _dbContext.Fastest<TEntity>().BulkCopyAsync(tableName, dataTable);
        }

        #endregion

        #region BulkUpdateAsync

        /// <summary>
        ///     大数据更新
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <returns>影响行数</returns>
        public Task<int> BulkUpdateAsync(List<TEntity> entities)
        {
            return _dbContext.Fastest<TEntity>().BulkUpdateAsync(entities);
        }

        #endregion

        #region ExecuteCommandAsync

        /// <summary>
        ///     执行SQL
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数集</param>
        /// <returns></returns>
        public Task<int> ExecuteCommandAsync(string sql, object parameters)
        {
            return _dbContext.Ado.ExecuteCommandAsync(sql, parameters);
        }

        #endregion

        #region InsertQueue

        /// <summary>
        ///     单条插入
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns>影响行数</returns>
        public void InsertQueue(TEntity entity)
        {
            _dbContext.Insertable(entity).AddQueue();
        }

        /// <summary>
        ///     单条插入
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="insertColumns">指定只插入列</param>
        /// <returns>影响行数</returns>
        public void InsertQueue(TEntity entity, Expression<Func<TEntity, object>>? insertColumns = default)
        {
            var insert = _dbContext.Insertable(entity);
            if (insertColumns != null)
            {
                insert.InsertColumns(insertColumns).AddQueue();
                return;
            }
            insert.AddQueue();
        }

        /// <summary>
        ///     批量插入
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <returns>影响行数</returns>
        public void InsertQueue(List<TEntity> entities)
        {
            _dbContext.Insertable(entities).AddQueue();
        }

        /// <summary>
        ///     批量插入
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <param name="insertColumns">指定只插入列</param>
        /// <returns>影响行数</returns>
        public void InsertQueue(List<TEntity> entities, Expression<Func<TEntity, object>>? insertColumns = default)
        {
            var insert = _dbContext.Insertable(entities);
            if (insertColumns != null)
            {
                insert.InsertColumns(insertColumns).AddQueue();
                return;
            }
            insert.AddQueue();
        }

        #endregion

        #region UpdateQueue

        /// <summary>
        ///     单条更新
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns>影响行数</returns>
        public void UpdateQueue(TEntity entity)
        {
            _dbContext.Updateable(entity).AddQueue();
        }

        /// <summary>
        ///     单条更新
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="updateColumns">更新列</param>
        /// <returns>影响行数</returns>
        public void UpdateQueue(TEntity entity, Expression<Func<TEntity, object>> updateColumns)
        {
            _dbContext.Updateable(entity).UpdateColumns(updateColumns).AddQueue();
        }

        /// <summary>
        ///     单条更新
        /// </summary>
        /// <param name="setColumns">设置列</param>
        /// <param name="whereExpression">查询条件表达式</param>
        /// <returns>影响行数</returns>
        public void UpdateQueue(Expression<Func<TEntity, TEntity>> setColumns, Expression<Func<TEntity, bool>> whereExpression)
        {
            _dbContext.Updateable<TEntity>().SetColumns(setColumns).Where(whereExpression).AddQueue();
        }

        /// <summary>
        ///     批量更新
        /// </summary>
        /// <param name="entities">实体类</param>
        /// <param name="updateColumns">更新列</param>
        /// <returns></returns>
        public void UpdateQueue(List<TEntity> entities, Expression<Func<TEntity, object>> updateColumns)
        {
            _dbContext.Updateable(entities).UpdateColumns(updateColumns).AddQueue();
        }

        #endregion

        #region SaveQueuesAsync

        /// <summary>
        ///     批量提交
        /// </summary>
        /// <param name="isTran">是否自动事务  默认：false</param>
        /// <returns>影响行数</returns>
        public Task<int> SaveQueuesAsync(bool isTran = false)
        {
            return _dbContext.SaveQueuesAsync(isTran);
        }

        /// <summary>
        ///     批量提交
        /// </summary>
        /// <param name="isTran">是否自动事务  默认：false</param>
        /// <returns>影响行数</returns>
        public Task<List<T>> SaveQueuesAsync<T>(bool isTran = false)
        {
            return _dbContext.SaveQueuesAsync<T>(isTran);
        }

        #endregion
    }

    #endregion

    #region 同步数据仓储

    /// <summary>
    ///     泛型数据仓储
    /// </summary>
    internal partial class SqlSugarDatabaseRepository<TEntity> : IDatabaseRepository<TEntity> where TEntity : class, new()
    {
        #region Query


        /// <summary>
        ///     查询所有数据
        /// </summary>
        /// <returns>所有数据</returns>
        public List<TEntity> QueryAll()
        {
            return _dbContext.Queryable<TEntity>().ToList();
        }

        /// <summary>
        ///     查询数据列表
        /// <remarks>
        ///    根据SQL条件查询数据列表
        /// </remarks>
        /// </summary>
        /// <param name="whereSql">查询条件</param>
        /// <returns>数据列表</returns>
        public List<TEntity> Query(string whereSql)
        {
            return _dbContext.Queryable<TEntity>().WhereIF(whereSql.IsNotNullOrWhiteSpace(), whereSql).ToList();
        }

        /// <summary>
        ///     查询数据列表
        /// <remarks>
        ///    Lambda条件表达式查询数据列表
        /// </remarks>
        /// </summary>
        /// <param name="whereExpression">查询表达式</param>
        /// <returns>数据列表</returns>
        public List<TEntity> Query(Expression<Func<TEntity, bool>> whereExpression)
        {
            return _dbContext.Queryable<TEntity>().WhereIF(whereExpression != null, whereExpression).ToList();
        }

        /// <summary>
        ///     查询单条数据
        /// </summary>
        /// <remarks>
        ///    Lambda条件表达式查询单条数据,如果查询到多条数据会抛出异常
        /// </remarks>
        /// <param name="whereExpression">查询表达式</param>
        /// <returns>单条数据</returns>
        public TEntity QuerySingle(Expression<Func<TEntity, bool>> whereExpression)
        {
            return _dbContext.Queryable<TEntity>().Single(whereExpression);
        }

        /// <summary>
        ///     查询单条数据
        /// </summary>
        /// <remarks>
        ///    Lambda条件表达式查询单条数据，支持部分列查询,如果查询到多条数据会抛出异常
        /// </remarks>
        /// <param name="whereExpression">查询表达式</param>
        /// <param name="fieldColumns">查询列</param>
        /// <returns>单条数据</returns>
        public TEntity QuerySingle(Expression<Func<TEntity, bool>> whereExpression, string fieldColumns)
        {
            return _dbContext.Queryable<TEntity>().Select(fieldColumns).Single(whereExpression);
        }

        /// <summary>
        ///     查询单条数据
        /// </summary>
        /// <remarks>
        ///   Lambda条件表达式查询单条数据，如果查询到多条数据只取其中一条
        /// </remarks>
        /// <param name="whereExpression">查询表达式</param>
        /// <returns>单条数据</returns>
        public TEntity QueryFirstOrDefault(Expression<Func<TEntity, bool>> whereExpression)
        {
            return _dbContext.Queryable<TEntity>().First(whereExpression);
        }

        /// <summary>
        ///     查询单条数据
        /// </summary>
        /// <remarks>
        ///   Lambda条件表达式查询单条数据，支持部分列查询,如果查询到多条数据只取其中一条
        /// </remarks>
        /// <param name="whereExpression">查询表达式</param>
        /// <param name="fieldColumns">查询列</param>
        /// <returns>单条数据</returns>
        public TEntity QueryFirstOrDefault(Expression<Func<TEntity, bool>> whereExpression, string fieldColumns)
        {
            return _dbContext.Queryable<TEntity>().Select(fieldColumns).First(whereExpression);
        }

        /// <summary>
        ///     查询数据列表
        /// </summary>
        /// <param name="whereExpression">查询表达式</param>
        /// <param name="orderByFileds">排序条件</param>
        /// <returns>数据列表</returns>
        public List<TEntity> Query(Expression<Func<TEntity, bool>> whereExpression, string orderByFileds)
        {
            return _dbContext.Queryable<TEntity>().WhereIF(whereExpression != null, whereExpression).OrderByIF(orderByFileds.IsNotNullOrWhiteSpace(), orderByFileds).ToList();
        }

        /// <summary>
        ///     查询数据列表
        /// </summary>
        /// <param name="whereExpression">查询表达式</param>
        /// <param name="orderByExpression">排序表达式</param>
        /// <param name="isAsc">是否升序</param>
        /// <returns>数据列表</returns>
        public List<TEntity> Query(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool isAsc = true)
        {
            return _dbContext.Queryable<TEntity>().OrderByIF(orderByExpression != null, orderByExpression, isAsc ? OrderByType.Asc : OrderByType.Desc).WhereIF(whereExpression != null, whereExpression).ToList();
        }

        /// <summary>
        ///     查询数据列表
        /// </summary>
        /// <param name="whereSql">查询表达式</param>
        /// <param name="orderByFileds">排序条件</param>
        /// <returns>数据列表</returns>
        public List<TEntity> Query(string whereSql, string orderByFileds)
        {
            return _dbContext.Queryable<TEntity>().OrderByIF(orderByFileds.IsNotNullOrWhiteSpace(), orderByFileds).WhereIF(orderByFileds.IsNotNullOrWhiteSpace(), whereSql).ToList();
        }


        /// <summary>
        ///     查询前N条数据
        /// </summary>
        /// <param name="whereExpression">查询表达式</param>
        /// <param name="intTop">前N条</param>
        /// <param name="orderByFileds">排序条件</param>
        /// <returns>前N条数据</returns>
        public List<TEntity> QueryTop(Expression<Func<TEntity, bool>> whereExpression, int intTop, string orderByFileds)
        {
            return _dbContext.Queryable<TEntity>().OrderByIF(orderByFileds.IsNotNullOrWhiteSpace(), orderByFileds).WhereIF(whereExpression != null, whereExpression).Take(intTop).ToList();
        }

        /// <summary>
        ///     查询前N条数据
        /// </summary>
        /// <param name="whereSql">查询条件</param>
        /// <param name="intTop">前N条</param>
        /// <param name="orderByFileds">排序条件</param>
        /// <returns>前N条数据</returns>
        public List<TEntity> QueryTop(string whereSql, int intTop, string orderByFileds)
        {
            return _dbContext.Queryable<TEntity>().OrderByIF(orderByFileds.IsNotNullOrWhiteSpace(), orderByFileds).WhereIF(whereSql.IsNotNullOrWhiteSpace(), whereSql).Take(intTop).ToList();
        }

        /// <summary>
        ///     查询数据列表
        /// </summary>
        /// <param name="sql">完整的SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>数据列表</returns>
        public List<TEntity> QuerySql(string sql, object? parameters = default)
        {
            return _dbContext.Ado.SqlQuery<TEntity>(sql, parameters);
        }

        /// <summary>
        ///     查询数据列表
        /// </summary>
        /// <param name="sql">完整的SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>DataTable</returns>
        public DataTable QueryTable(string sql, object? parameters = default)
        {
            return _dbContext.Ado.GetDataTable(sql, parameters);
        }


        /// <summary>
        ///     查询数据列表
        /// </summary>
        /// <param name="whereExpression">查询表达式</param>
        /// <param name="pageIndex">页码（下标0）</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="orderByFileds">排序条件</param>
        /// <returns>数据列表</returns>
        public PageModel<TEntity> QueryPage(Expression<Func<TEntity, bool>> whereExpression, int pageIndex = 1, int pageSize = 20, string? orderByFileds = default)
        {
            return _dbContext.Queryable<TEntity>().OrderByIF(orderByFileds.IsNotNullOrWhiteSpace(), orderByFileds).WhereIF(whereExpression != null, whereExpression).ToPagedList(pageIndex, pageSize);
        }


        /// <summary>
        ///     获取总记录数
        /// </summary>
        /// <param name="whereExpression">查询表达式</param>
        /// <returns>总记录数</returns>
        public int Count(Expression<Func<TEntity, bool>> whereExpression)
        {
            return _dbContext.Queryable<TEntity>().Count(whereExpression);
        }

        /// <summary>
        ///     检查是否存在
        /// </summary>
        /// <param name="whereExpression">查询表达式</param>
        /// <returns>是否存在</returns>
        public bool Any(Expression<Func<TEntity, bool>> whereExpression)
        {
            return _dbContext.Queryable<TEntity>().Any(whereExpression);
        }

        #endregion

        #region QueryForEach

        /// <summary>
        ///     查询数据列表[分批]
        /// <remarks>
        ///    Lambda条件表达式查询数据列表
        /// </remarks>
        /// </summary>
        /// <param name="whereExpression">查询表达式</param>
        /// <returns>数据列表</returns>
        public List<TEntity> QueryForEach(Expression<Func<TEntity, bool>> whereExpression)
        {
            var list = new List<TEntity>();
            _dbContext.Queryable<TEntity>().WhereIF(whereExpression != null, whereExpression).ForEach(p =>
            {
                list.Add(p);
            }, 200);
            return list;
        }

        #endregion

        #region Insert

        /// <summary>
        ///     单条插入
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns>影响行数</returns>
        public int Insert(TEntity entity)
        {
            return _dbContext.Insertable(entity).ExecuteCommand();
        }

        /// <summary>
        ///     单条插入
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="insertColumns">指定只插入列</param>
        /// <returns>影响行数</returns>
        public int Insert(TEntity entity, Expression<Func<TEntity, object>>? insertColumns = default)
        {
            var insert = _dbContext.Insertable(entity);
            if (insertColumns == null)
            {
                return insert.ExecuteCommand();
            }
            return insert.InsertColumns(insertColumns).ExecuteCommand();
        }

        /// <summary>
        ///     批量插入
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <returns>影响行数</returns>
        public int Insert(List<TEntity> entities)
        {
            return _dbContext.Insertable(entities).UseParameter().ExecuteCommand();
        }

        /// <summary>
        ///     批量插入
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <param name="insertColumns">指定只插入列</param>
        /// <returns>影响行数</returns>
        public int Insert(List<TEntity> entities, Expression<Func<TEntity, object>>? insertColumns = default)
        {
            var insert = _dbContext.Insertable(entities);
            if (insertColumns == null)
            {
                return insert.UseParameter().ExecuteCommand();
            }
            return insert.InsertColumns(insertColumns).UseParameter().ExecuteCommand();
        }

        #endregion

        #region Update

        /// <summary>
        ///     单条更新
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns>影响行数</returns>
        public int Update(TEntity entity)
        {
            return _dbContext.Updateable(entity).ExecuteCommand();
        }

        /// <summary>
        ///     单条更新
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="updateColumns">更新列</param>
        /// <returns></returns>
        public int Update(TEntity entity, Expression<Func<TEntity, object>> updateColumns)
        {
            return _dbContext.Updateable(entity).UpdateColumns(updateColumns).ExecuteCommand();
        }

        /// <summary>
        ///     单条更新
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="updateColumns">更新列</param>
        /// <param name="whereExpression">查询条件表达式</param>
        /// <returns></returns>
        public int Update(TEntity entity, Expression<Func<TEntity, object>> updateColumns, Expression<Func<TEntity, bool>> whereExpression)
        {
            return _dbContext.Updateable(entity).UpdateColumns(updateColumns).Where(whereExpression).ExecuteCommand();
        }

        /// <summary>
        ///     单条更新
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="whereSql">查询条件</param>
        /// <returns>影响行数</returns>
        public int Update(TEntity entity, string whereSql)
        {
            return _dbContext.Updateable(entity).Where(whereSql).ExecuteCommand();
        }

        /// <summary>
        ///     单条更新
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="whereExpression">查询条件表达式</param>
        /// <returns>影响行数</returns>
        public int Update(TEntity entity, Expression<Func<TEntity, bool>> whereExpression)
        {
            return _dbContext.Updateable(entity).Where(whereExpression).ExecuteCommand();
        }

        /// <summary>
        ///     单条更新
        /// </summary>
        /// <param name="setColumns">设置列</param>
        /// <param name="whereExpression">查询条件表达式</param>
        /// <returns>影响行数</returns>
        public int Update(Expression<Func<TEntity, TEntity>> setColumns, Expression<Func<TEntity, bool>> whereExpression)
        {
            return _dbContext.Updateable<TEntity>().SetColumns(setColumns).Where(whereExpression).ExecuteCommand();
        }


        /// <summary>
        ///     单条更新
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="updateColumns">更新列</param>
        /// <returns>影响行数</returns>
        public int Update(TEntity entity, string[] updateColumns)
        {
            return _dbContext.Updateable(entity).UpdateColumns(updateColumns).ExecuteCommand();
        }

        /// <summary>
        ///     单条更新
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="updateColumns">更新列</param>
        /// <param name="whereExpression">查询条件表达式</param>
        /// <returns>影响行数</returns>
        public int Update(TEntity entity, string[] updateColumns, Expression<Func<TEntity, bool>> whereExpression)
        {
            return _dbContext.Updateable(entity).UpdateColumns(updateColumns).Where(whereExpression).ExecuteCommand();
        }

        /// <summary>
        ///     批量更新
        /// </summary>
        /// <param name="entities">实体类</param>
        /// <returns></returns>
        public int Update(List<TEntity> entities)
        {
            return _dbContext.Updateable(entities).ExecuteCommand();
        }


        /// <summary>
        ///     批量更新
        /// </summary>
        /// <param name="entities">实体类</param>
        /// <param name="updateColumns">更新列</param>
        /// <returns></returns>
        public int Update(List<TEntity> entities, Expression<Func<TEntity, object>> updateColumns)
        {
            return _dbContext.Updateable(entities).UpdateColumns(updateColumns).ExecuteCommand();
        }

        #endregion

        #region Delete

        /// <summary>
        ///     单条删除
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns>影响行数</returns>
        public int Delete(TEntity entity)
        {
            return _dbContext.Deleteable(entity).ExecuteCommand();
        }

        /// <summary>
        ///     条件删除
        /// </summary>
        /// <param name="deleteExpression">查询条件表达式</param>
        /// <returns>影响行数</returns>
        public int Delete(Expression<Func<TEntity, bool>> deleteExpression)
        {
            return _dbContext.Deleteable<TEntity>().Where(deleteExpression).ExecuteCommand();
        }


        /// <summary>
        ///     批量删除
        /// </summary>
        /// <param name="entities">实体集</param>
        /// <returns>影响行数</returns>
        public int Delete(List<TEntity> entities)
        {
            return _dbContext.Deleteable(entities).ExecuteCommand();
        }

        #endregion

        #region Saveable

        /// <summary>
        ///     单条保存
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        public void Saveable(TEntity entity)
        {
            var storage = _dbContext.Storageable(entity).Saveable().ToStorage();
            storage.AsInsertable.ExecuteCommand();
            storage.AsUpdateable.ExecuteCommand();
        }

        /// <summary>
        ///     单条保存
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="updateColumns">更新列</param>
        /// <returns></returns>
        public void Saveable(TEntity entity, Expression<Func<TEntity, object>> updateColumns)
        {
            var storage = _dbContext.Storageable(entity).Saveable().ToStorage();
            storage.AsInsertable.ExecuteCommand();
            storage.AsUpdateable.UpdateColumns(updateColumns).ExecuteCommand();
        }

        /// <summary>
        ///     批量保存
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <returns></returns>
        public void Saveable(List<TEntity> entities)
        {
            var storage = _dbContext.Storageable(entities).Saveable().ToStorage();
            storage.AsInsertable.ExecuteCommand();
            storage.AsUpdateable.ExecuteCommand();
        }

        /// <summary>
        ///     批量保存
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <param name="updateColumns">更新列</param>
        /// <returns></returns>
        public void Saveable(List<TEntity> entities, Expression<Func<TEntity, object>> updateColumns)
        {
            var storage = _dbContext.Storageable(entities).Saveable().ToStorage();
            storage.AsInsertable.ExecuteCommand();
            storage.AsUpdateable.UpdateColumns(updateColumns).ExecuteCommand();
        }


        #endregion

        #region BulkCopy

        /// <summary>
        ///     大数据插入
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <returns>影响行数</returns>
        public int BulkCopy(List<TEntity> entities)
        {
            return _dbContext.Fastest<TEntity>().BulkCopy(entities);
        }

        /// <summary>
        ///     大数据插入
        /// </summary>
        /// <param name="tableName">表名字</param>
        /// <param name="dataTable">数据集</param>
        /// <returns></returns>
        public int BulkCopy(string tableName, DataTable dataTable)
        {
            return _dbContext.Fastest<TEntity>().BulkCopy(tableName, dataTable);
        }

        #endregion

        #region BulkUpdate

        /// <summary>
        ///     大数据更新
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <returns>影响行数</returns>
        public int BulkUpdate(List<TEntity> entities)
        {
            return _dbContext.Fastest<TEntity>().BulkUpdate(entities);
        }

        #endregion

        #region ExecuteCommand

        /// <summary>
        ///     执行SQL
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数集</param>
        /// <returns></returns>
        public int ExecuteCommand(string sql, object? parameters = default)
        {
            return _dbContext.Ado.ExecuteCommand(sql, parameters);
        }

        #endregion

        #region SaveQueues

        /// <summary>
        ///     批量提交
        /// </summary>
        /// <param name="isTran">是否自动事务  默认：false</param>
        /// <returns>影响行数</returns>
        public int SaveQueues(bool isTran = false)
        {
            return _dbContext.SaveQueues(isTran);
        }

        /// <summary>
        ///     批量提交
        /// </summary>
        /// <param name="isTran">是否自动事务  默认：false</param>
        /// <returns>影响行数</returns>
        public List<T> SaveQueues<T>(bool isTran = false)
        {
            return _dbContext.SaveQueues<T>(isTran);
        }

        #endregion
    }

    #endregion
}