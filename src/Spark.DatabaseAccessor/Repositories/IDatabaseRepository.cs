using Spark.DatabaseAccessor.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Spark.DatabaseAccessor.Repositories
{
    #region 异步数据仓储

    /// <summary>
    ///     非泛型数据仓储
    /// </summary>
    public interface IDatabaseRepository
    {
        /// <summary>
        ///     数据库上下文
        /// </summary>
        object DbContext { get; }
    }


    /// <summary>
    ///     泛型数据仓储
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public partial interface IDatabaseRepository<TEntity> where TEntity : class, new()
    {
        /// <summary>
        ///     数据库上下文
        /// </summary>
        object DbContext { get; }

        #region QueryAsync


        /// <summary>
        ///     查询所有数据
        /// </summary>
        /// <returns>所有数据</returns>
        Task<List<TEntity>> QueryAllAsync();

        /// <summary>
        ///     查询数据列表
        /// <remarks>
        ///    根据SQL条件查询数据列表
        /// </remarks>
        /// </summary>
        /// <param name="whereSql">查询条件</param>
        /// <returns>数据列表</returns>
        Task<List<TEntity>> QueryAsync(string whereSql);

        /// <summary>
        ///     查询数据列表
        /// <remarks>
        ///    Lambda条件表达式查询数据列表
        /// </remarks>
        /// </summary>
        /// <param name="whereExpression">查询表达式</param>
        /// <returns>数据列表</returns>
        Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> whereExpression);

        /// <summary>
        ///     查询单条数据
        /// </summary>
        /// <remarks>
        ///    Lambda条件表达式查询单条数据,如果查询到多条数据会抛出异常
        /// </remarks>
        /// <param name="whereExpression">查询表达式</param>
        /// <returns>单条数据</returns>
        Task<TEntity> QuerySingleAsync(Expression<Func<TEntity, bool>> whereExpression);

        /// <summary>
        ///     查询单条数据
        /// </summary>
        /// <remarks>
        ///    Lambda条件表达式查询单条数据，支持部分列查询,如果查询到多条数据会抛出异常
        /// </remarks>
        /// <param name="whereExpression">查询表达式</param>
        /// <param name="fieldColumns">查询列</param>
        /// <returns>单条数据</returns>
        Task<TEntity> QuerySingleAsync(Expression<Func<TEntity, bool>> whereExpression, string fieldColumns);

        /// <summary>
        ///     查询单条数据
        /// </summary>
        /// <remarks>
        ///   Lambda条件表达式查询单条数据，如果查询到多条数据只取其中一条
        /// </remarks>
        /// <param name="whereExpression">查询表达式</param>
        /// <returns>单条数据</returns>

        Task<TEntity> QueryFirstOrDefaultAsync(Expression<Func<TEntity, bool>> whereExpression);

        /// <summary>
        ///     查询单条数据
        /// </summary>
        /// <remarks>
        ///   Lambda条件表达式查询单条数据，支持部分列查询,如果查询到多条数据只取其中一条
        /// </remarks>
        /// <param name="whereExpression">查询表达式</param>
        /// <param name="fieldColumns">查询列</param>
        /// <returns>单条数据</returns>
        Task<TEntity> QueryFirstOrDefaultAsync(Expression<Func<TEntity, bool>> whereExpression, string fieldColumns);


        /// <summary>
        ///     查询数据列表
        /// </summary>
        /// <param name="whereExpression">查询表达式</param>
        /// <param name="orderByFileds">排序条件</param>
        /// <returns>数据列表</returns>
        Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> whereExpression, string orderByFileds);

        /// <summary>
        ///     查询数据列表
        /// </summary>
        /// <param name="whereExpression">查询表达式</param>
        /// <param name="orderByExpression">排序表达式</param>
        /// <param name="isAsc">是否升序</param>
        /// <returns>数据列表</returns>
        Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool isAsc = true);

        /// <summary>
        ///     查询数据列表
        /// </summary>
        /// <param name="whereSql">查询表达式</param>
        /// <param name="orderByFileds">排序条件</param>
        /// <returns>数据列表</returns>
        Task<List<TEntity>> QueryAsync(string whereSql, string orderByFileds);


        /// <summary>
        ///     查询前N条数据
        /// </summary>
        /// <param name="whereExpression">查询表达式</param>
        /// <param name="intTop">前N条</param>
        /// <param name="orderByFileds">排序条件</param>
        /// <returns>前N条数据</returns>
        Task<List<TEntity>> QueryTopAsync(Expression<Func<TEntity, bool>> whereExpression, int intTop, string orderByFileds);

        /// <summary>
        ///     查询前N条数据
        /// </summary>
        /// <param name="whereSql">查询条件</param>
        /// <param name="intTop">前N条</param>
        /// <param name="orderByFileds">排序条件</param>
        /// <returns>前N条数据</returns>
        Task<List<TEntity>> QueryTopAsync(string whereSql, int intTop, string orderByFileds);

        /// <summary>
        ///     查询数据列表
        /// </summary>
        /// <param name="sql">完整的SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>数据列表</returns>
        Task<List<TEntity>> QuerySqlAsync(string sql, object? parameters = default);

        /// <summary>
        ///     查询数据列表
        /// </summary>
        /// <param name="sql">完整的SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>DataTable</returns>
        Task<DataTable> QueryTableAsync(string sql, object? parameters = default);


        /// <summary>
        ///     查询数据列表
        /// </summary>
        /// <param name="whereExpression">查询表达式</param>
        /// <param name="pageIndex">页码（下标0）</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="orderByFileds">排序条件</param>
        /// <returns>数据列表</returns>
        Task<PageModel<TEntity>> QueryPageAsync(Expression<Func<TEntity, bool>> whereExpression, int pageIndex = 1, int pageSize = 20, string? orderByFileds = default);


        /// <summary>
        ///     获取总记录数
        /// </summary>
        /// <param name="whereExpression">查询表达式</param>
        /// <returns>总记录数</returns>
        Task<int> CountAsync(Expression<Func<TEntity, bool>> whereExpression);

        /// <summary>
        ///     检查是否存在
        /// </summary>
        /// <param name="whereExpression">查询表达式</param>
        /// <returns>是否存在</returns>
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> whereExpression);

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
        Task<List<TEntity>> QueryForEachAsync(Expression<Func<TEntity, bool>> whereExpression);

        #endregion

        #region InsertAsync

        /// <summary>
        ///     单条插入
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns>影响行数</returns>
        Task<int> InsertAsync(TEntity entity);

        /// <summary>
        ///     单条插入
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="insertColumns">指定只插入列</param>
        /// <returns>影响行数</returns>
        Task<int> InsertAsync(TEntity entity, Expression<Func<TEntity, object>>? insertColumns = default);

        /// <summary>
        ///     批量插入
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <returns>影响行数</returns>
        Task<int> InsertAsync(List<TEntity> entities);

        /// <summary>
        ///     批量插入
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <param name="insertColumns">指定只插入列</param>
        /// <returns>影响行数</returns>
        Task<int> InsertAsync(List<TEntity> entities, Expression<Func<TEntity, object>>? insertColumns = default);

        #endregion

        #region UpdateAsync

        /// <summary>
        ///     单条更新
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns>影响行数</returns>
        Task<int> UpdateAsync(TEntity entity);

        /// <summary>
        ///     单条更新
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="updateColumns">更新列</param>
        /// <returns></returns>
        Task<int> UpdateAsync(TEntity entity, Expression<Func<TEntity, object>> updateColumns);

        /// <summary>
        ///     单条更新
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="updateColumns">更新列</param>
        /// <param name="whereExpression">查询条件表达式</param>
        /// <returns></returns>
        Task<int> UpdateAsync(TEntity entity, Expression<Func<TEntity, object>> updateColumns, Expression<Func<TEntity, bool>> whereExpression);

        /// <summary>
        ///     单条更新
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="whereSql">查询条件</param>
        /// <returns>影响行数</returns>
        Task<int> UpdateAsync(TEntity entity, string whereSql);

        /// <summary>
        ///     单条更新
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="whereExpression">查询条件表达式</param>
        /// <returns>影响行数</returns>
        Task<int> UpdateAsync(TEntity entity, Expression<Func<TEntity, bool>> whereExpression);

        /// <summary>
        ///     单条更新
        /// </summary>
        /// <param name="setColumns">设置列</param>
        /// <param name="whereExpression">查询条件表达式</param>
        /// <returns>影响行数</returns>
        Task<int> UpdateAsync(Expression<Func<TEntity, TEntity>> setColumns, Expression<Func<TEntity, bool>> whereExpression);


        /// <summary>
        ///     单条更新
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="updateColumns">更新列</param>
        /// <returns>影响行数</returns>
        Task<int> UpdateAsync(TEntity entity, string[] updateColumns);

        /// <summary>
        ///     单条更新
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="updateColumns">更新列</param>
        /// <param name="whereExpression">查询条件表达式</param>
        /// <returns>影响行数</returns>
        Task<int> UpdateAsync(TEntity entity, string[] updateColumns, Expression<Func<TEntity, bool>> whereExpression);

        /// <summary>
        ///     批量更新
        /// </summary>
        /// <param name="entities">实体类</param>
        /// <returns></returns>
        Task<int> UpdateAsync(List<TEntity> entities);


        /// <summary>
        ///     批量更新
        /// </summary>
        /// <param name="entities">实体类</param>
        /// <param name="updateColumns">更新列</param>
        /// <returns></returns>
        Task<int> UpdateAsync(List<TEntity> entities, Expression<Func<TEntity, object>> updateColumns);

        #endregion

        #region DeleteAsync

        /// <summary>
        ///     单条删除
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns>影响行数</returns>
        Task<int> DeleteAsync(TEntity entity);

        /// <summary>
        ///     条件删除
        /// </summary>
        /// <param name="deleteExpression">查询条件表达式</param>
        /// <returns>影响行数</returns>
        Task<int> DeleteAsync(Expression<Func<TEntity, bool>> deleteExpression);

        /// <summary>
        ///     批量删除
        /// </summary>
        /// <param name="entities">实体集</param>
        /// <returns>影响行数</returns>
        Task<int> DeleteAsync(List<TEntity> entities);

        #endregion

        #region TruncateTable

        /// <summary>
        ///   初始化表
        /// <remarks>
        ///  表中数据全部清空，清除，自增初始化
        /// </remarks>
        /// </summary>
        /// <returns></returns>
        bool TruncateTable();

        #endregion

        #region SaveableAsync

        /// <summary>
        ///     单条保存
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        Task SaveableAsync(TEntity entity);

        /// <summary>
        ///     单条保存
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="updateColumns">更新列</param>
        /// <returns></returns>
        Task SaveableAsync(TEntity entity, Expression<Func<TEntity, object>> updateColumns);

        /// <summary>
        ///     批量保存
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <returns></returns>
        Task SaveableAsync(List<TEntity> entities);

        /// <summary>
        ///     批量保存
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <param name="updateColumns">更新列</param>
        /// <returns></returns>
        Task SaveableAsync(List<TEntity> entities, Expression<Func<TEntity, object>> updateColumns);


        #endregion

        #region BulkCopyAsync

        /// <summary>
        ///     大数据插入
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <returns>影响行数</returns>
        Task<int> BulkCopyAsync(List<TEntity> entities);

        /// <summary>
        ///     大数据插入
        /// </summary>
        /// <param name="tableName">表名字</param>
        /// <param name="dataTable">数据集</param>
        /// <returns></returns>
        Task<int> BulkCopyAsync(string tableName, DataTable dataTable);

        #endregion

        #region BulkUpdateAsync

        /// <summary>
        ///     大数据更新
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <returns>影响行数</returns>
        Task<int> BulkUpdateAsync(List<TEntity> entities);

        #endregion

        #region ExecuteCommandAsync

        /// <summary>
        ///     执行SQL
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数集</param>
        /// <returns></returns>
        Task<int> ExecuteCommandAsync(string sql, object parameters);

        #endregion

        #region InsertQueue

        /// <summary>
        ///     单条插入
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns>影响行数</returns>
        void InsertQueue(TEntity entity);

        /// <summary>
        ///     单条插入
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="insertColumns">指定只插入列</param>
        /// <returns>影响行数</returns>
        void InsertQueue(TEntity entity, Expression<Func<TEntity, object>>? insertColumns = default);

        /// <summary>
        ///     批量插入
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <returns>影响行数</returns>
        void InsertQueue(List<TEntity> entities);

        /// <summary>
        ///     批量插入
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <param name="insertColumns">指定只插入列</param>
        /// <returns>影响行数</returns>
        void InsertQueue(List<TEntity> entities, Expression<Func<TEntity, object>>? insertColumns = default);

        #endregion

        #region UpdateQueue

        /// <summary>
        ///     单条更新
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns>影响行数</returns>
        void UpdateQueue(TEntity entity);

        /// <summary>
        ///     单条更新
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="updateColumns">更新列</param>
        /// <returns></returns>
        void UpdateQueue(TEntity entity, Expression<Func<TEntity, object>> updateColumns);

        /// <summary>
        ///     单条更新
        /// </summary>
        /// <param name="setColumns">设置列</param>
        /// <param name="whereExpression">查询条件表达式</param>
        /// <returns>影响行数</returns>
        void UpdateQueue(Expression<Func<TEntity, TEntity>> setColumns, Expression<Func<TEntity, bool>> whereExpression);

        /// <summary>
        ///     批量更新
        /// </summary>
        /// <param name="entities">实体类</param>
        /// <param name="updateColumns">更新列</param>
        /// <returns></returns>
        void UpdateQueue(List<TEntity> entities, Expression<Func<TEntity, object>> updateColumns);

        #endregion

        #region SaveQueuesAsync

        /// <summary>
        ///     批量提交
        /// </summary>
        /// <param name="isTran">是否自动事务  默认：false</param>
        /// <returns>影响行数</returns>
        Task<int> SaveQueuesAsync(bool isTran = false);

        /// <summary>
        ///     批量提交
        /// </summary>
        /// <param name="isTran">是否自动事务  默认：false</param>
        /// <returns>影响行数</returns>
        Task<List<T>> SaveQueuesAsync<T>(bool isTran = false);

        #endregion
    }

    #endregion

    #region 同步数据仓储
    /// <summary>
    ///   泛型数据仓储
    /// </summary>
    public partial interface IDatabaseRepository<TEntity> where TEntity : class, new()
    {

        #region Query


        /// <summary>
        ///     查询所有数据
        /// </summary>
        /// <returns>所有数据</returns>
        List<TEntity> QueryAll();

        /// <summary>
        ///     查询数据列表
        /// <remarks>
        ///    根据SQL条件查询数据列表
        /// </remarks>
        /// </summary>
        /// <param name="whereSql">查询条件</param>
        /// <returns>数据列表</returns>
        List<TEntity> Query(string whereSql);

        /// <summary>
        ///     查询数据列表
        /// <remarks>
        ///    Lambda条件表达式查询数据列表
        /// </remarks>
        /// </summary>
        /// <param name="whereExpression">查询表达式</param>
        /// <returns>数据列表</returns>
        List<TEntity> Query(Expression<Func<TEntity, bool>> whereExpression);

        /// <summary>
        ///     查询单条数据
        /// </summary>
        /// <remarks>
        ///    Lambda条件表达式查询单条数据,如果查询到多条数据会抛出异常
        /// </remarks>
        /// <param name="whereExpression">查询表达式</param>
        /// <returns>单条数据</returns>
        TEntity QuerySingle(Expression<Func<TEntity, bool>> whereExpression);

        /// <summary>
        ///     查询单条数据
        /// </summary>
        /// <remarks>
        ///    Lambda条件表达式查询单条数据，支持部分列查询,如果查询到多条数据会抛出异常
        /// </remarks>
        /// <param name="whereExpression">查询表达式</param>
        /// <param name="fieldColumns">查询列</param>
        /// <returns>单条数据</returns>
        TEntity QuerySingle(Expression<Func<TEntity, bool>> whereExpression, string fieldColumns);

        /// <summary>
        ///     查询单条数据
        /// </summary>
        /// <remarks>
        ///   Lambda条件表达式查询单条数据，如果查询到多条数据只取其中一条
        /// </remarks>
        /// <param name="whereExpression">查询表达式</param>
        /// <returns>单条数据</returns>

        TEntity QueryFirstOrDefault(Expression<Func<TEntity, bool>> whereExpression);

        /// <summary>
        ///     查询单条数据
        /// </summary>
        /// <remarks>
        ///   Lambda条件表达式查询单条数据，支持部分列查询,如果查询到多条数据只取其中一条
        /// </remarks>
        /// <param name="whereExpression">查询表达式</param>
        /// <param name="fieldColumns">查询列</param>
        /// <returns>单条数据</returns>
        TEntity QueryFirstOrDefault(Expression<Func<TEntity, bool>> whereExpression, string fieldColumns);


        /// <summary>
        ///     查询数据列表
        /// </summary>
        /// <param name="whereExpression">查询表达式</param>
        /// <param name="orderByFileds">排序条件</param>
        /// <returns>数据列表</returns>
        List<TEntity> Query(Expression<Func<TEntity, bool>> whereExpression, string orderByFileds);

        /// <summary>
        ///     查询数据列表
        /// </summary>
        /// <param name="whereExpression">查询表达式</param>
        /// <param name="orderByExpression">排序表达式</param>
        /// <param name="isAsc">是否升序</param>
        /// <returns>数据列表</returns>
        List<TEntity> Query(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool isAsc = true);

        /// <summary>
        ///     查询数据列表
        /// </summary>
        /// <param name="whereSql">查询表达式</param>
        /// <param name="orderByFileds">排序条件</param>
        /// <returns>数据列表</returns>
        List<TEntity> Query(string whereSql, string orderByFileds);


        /// <summary>
        ///     查询前N条数据
        /// </summary>
        /// <param name="whereExpression">查询表达式</param>
        /// <param name="intTop">前N条</param>
        /// <param name="orderByFileds">排序条件</param>
        /// <returns>前N条数据</returns>
        List<TEntity> QueryTop(Expression<Func<TEntity, bool>> whereExpression, int intTop, string orderByFileds);

        /// <summary>
        ///     查询前N条数据
        /// </summary>
        /// <param name="whereSql">查询条件</param>
        /// <param name="intTop">前N条</param>
        /// <param name="orderByFileds">排序条件</param>
        /// <returns>前N条数据</returns>
        List<TEntity> QueryTop(string whereSql, int intTop, string orderByFileds);

        /// <summary>
        ///     查询数据列表
        /// </summary>
        /// <param name="sql">完整的SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>数据列表</returns>
        List<TEntity> QuerySql(string sql, object? parameters = default);

        /// <summary>
        ///     查询数据列表
        /// </summary>
        /// <param name="sql">完整的SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>DataTable</returns>
        DataTable QueryTable(string sql, object? parameters = default);


        /// <summary>
        ///     查询数据列表
        /// </summary>
        /// <param name="whereExpression">查询表达式</param>
        /// <param name="pageIndex">页码（下标0）</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="orderByFileds">排序条件</param>
        /// <returns>数据列表</returns>
        PageModel<TEntity> QueryPage(Expression<Func<TEntity, bool>> whereExpression, int pageIndex = 1, int pageSize = 20, string? orderByFileds = default);


        /// <summary>
        ///     获取总记录数
        /// </summary>
        /// <param name="whereExpression">查询表达式</param>
        /// <returns>总记录数</returns>
        int Count(Expression<Func<TEntity, bool>> whereExpression);

        /// <summary>
        ///     检查是否存在
        /// </summary>
        /// <param name="whereExpression">查询表达式</param>
        /// <returns>是否存在</returns>
        bool Any(Expression<Func<TEntity, bool>> whereExpression);

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
        List<TEntity> QueryForEach(Expression<Func<TEntity, bool>> whereExpression);

        #endregion

        #region Insert

        /// <summary>
        ///     单条插入
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns>影响行数</returns>
        int Insert(TEntity entity);

        /// <summary>
        ///     单条插入
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="insertColumns">指定只插入列</param>
        /// <returns>影响行数</returns>
        int Insert(TEntity entity, Expression<Func<TEntity, object>>? insertColumns = default);

        /// <summary>
        ///     批量插入
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <returns>影响行数</returns>
        int Insert(List<TEntity> entities);

        /// <summary>
        ///     批量插入
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <param name="insertColumns">指定只插入列</param>
        /// <returns>影响行数</returns>
        int Insert(List<TEntity> entities, Expression<Func<TEntity, object>>? insertColumns = default);

        #endregion

        #region Update

        /// <summary>
        ///     单条更新
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns>影响行数</returns>
        int Update(TEntity entity);

        /// <summary>
        ///     单条更新
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="updateColumns">更新列</param>
        /// <returns></returns>
        int Update(TEntity entity, Expression<Func<TEntity, object>> updateColumns);

        /// <summary>
        ///     单条更新
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="updateColumns">更新列</param>
        /// <param name="whereExpression">查询条件表达式</param>
        /// <returns></returns>
        int Update(TEntity entity, Expression<Func<TEntity, object>> updateColumns, Expression<Func<TEntity, bool>> whereExpression);

        /// <summary>
        ///     单条更新
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="whereSql">查询条件</param>
        /// <returns>影响行数</returns>
        int Update(TEntity entity, string whereSql);

        /// <summary>
        ///     单条更新
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="whereExpression">查询条件表达式</param>
        /// <returns>影响行数</returns>
        int Update(TEntity entity, Expression<Func<TEntity, bool>> whereExpression);

        /// <summary>
        ///     单条更新
        /// </summary>
        /// <param name="setColumns">设置列</param>
        /// <param name="whereExpression">查询条件表达式</param>
        /// <returns>影响行数</returns>
        int Update(Expression<Func<TEntity, TEntity>> setColumns, Expression<Func<TEntity, bool>> whereExpression);


        /// <summary>
        ///     单条更新
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="updateColumns">更新列</param>
        /// <returns>影响行数</returns>
        int Update(TEntity entity, string[] updateColumns);

        /// <summary>
        ///     单条更新
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="updateColumns">更新列</param>
        /// <param name="whereExpression">查询条件表达式</param>
        /// <returns>影响行数</returns>
        int Update(TEntity entity, string[] updateColumns, Expression<Func<TEntity, bool>> whereExpression);

        /// <summary>
        ///     批量更新
        /// </summary>
        /// <param name="entities">实体类</param>
        /// <returns></returns>
        int Update(List<TEntity> entities);


        /// <summary>
        ///     批量更新
        /// </summary>
        /// <param name="entities">实体类</param>
        /// <param name="updateColumns">更新列</param>
        /// <returns></returns>
        int Update(List<TEntity> entities, Expression<Func<TEntity, object>> updateColumns);

        #endregion

        #region Delete

        /// <summary>
        ///     单条删除
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns>影响行数</returns>
        int Delete(TEntity entity);

        /// <summary>
        ///     条件删除
        /// </summary>
        /// <param name="deleteExpression">查询条件表达式</param>
        /// <returns>影响行数</returns>
        int Delete(Expression<Func<TEntity, bool>> deleteExpression);

        /// <summary>
        ///     批量删除
        /// </summary>
        /// <param name="entities">实体集</param>
        /// <returns>影响行数</returns>
        int Delete(List<TEntity> entities);

        #endregion

        #region Saveable

        /// <summary>
        ///     单条保存
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        void Saveable(TEntity entity);

        /// <summary>
        ///     单条保存
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="updateColumns">更新列</param>
        /// <returns></returns>
        void Saveable(TEntity entity, Expression<Func<TEntity, object>> updateColumns);

        /// <summary>
        ///     批量保存
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <returns></returns>
        void Saveable(List<TEntity> entities);

        /// <summary>
        ///     批量保存
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <param name="updateColumns">更新列</param>
        /// <returns></returns>
        void Saveable(List<TEntity> entities, Expression<Func<TEntity, object>> updateColumns);


        #endregion

        #region BulkCopy

        /// <summary>
        ///     大数据插入
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <returns>影响行数</returns>
        int BulkCopy(List<TEntity> entities);

        /// <summary>
        ///     大数据插入
        /// </summary>
        /// <param name="tableName">表名字</param>
        /// <param name="dataTable">数据集</param>
        /// <returns></returns>
        int BulkCopy(string tableName, DataTable dataTable);

        #endregion

        #region BulkUpdate

        /// <summary>
        ///     大数据更新
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <returns>影响行数</returns>
        int BulkUpdate(List<TEntity> entities);

        #endregion

        #region ExecuteCommand

        /// <summary>
        ///     执行SQL
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数集</param>
        /// <returns></returns>
        int ExecuteCommand(string sql, object? parameters = default);

        #endregion

        #region SaveQueues

        /// <summary>
        ///     批量提交
        /// </summary>
        /// <param name="isTran">是否自动事务  默认：false</param>
        /// <returns>影响行数</returns>
        int SaveQueues(bool isTran = false);

        /// <summary>
        ///     批量提交
        /// </summary>
        /// <param name="isTran">是否自动事务  默认：false</param>
        /// <returns>影响行数</returns>
        List<T> SaveQueues<T>(bool isTran = false);

        #endregion
    } 
    
    #endregion
}