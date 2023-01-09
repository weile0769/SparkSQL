using Spark.DatabaseAccessor.Models;
using SqlSugar;
using System;
using System.Threading.Tasks;

namespace Spark.DatabaseAccessor.SqlSugar.Extensions
{
    /// <summary>
    ///     分页拓展类
    /// </summary>
    public static class ISugarQueryableExtensions
    {
        /// <summary>
        ///     分页拓展
        /// </summary>
        /// <param name="entity">实体类型</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <returns></returns>
        public static PageModel<TEntity> ToPagedList<TEntity>(this ISugarQueryable<TEntity> entity, int page, int pageSize)
            where TEntity : new()
        {
            var totalCount = 0;
            var items = entity.ToPageList(page, pageSize, ref totalCount);
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            return new PageModel<TEntity>
            {
                Page = page,
                PageSize = pageSize,
                Data = items,
                TotalCount = totalCount,
                PageCount = totalPages,
                HasNextPages = page < totalPages,
                HasPrevPages = page - 1 > 0
            };
        }

        /// <summary>
        ///     分页拓展
        /// </summary>
        /// <param name="entity">实体类型</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <returns></returns>
        public static async Task<PageModel<TEntity>> ToPagedListAsync<TEntity>(this ISugarQueryable<TEntity> entity, int page, int pageSize)
            where TEntity : new()
        {
            RefAsync<int> totalCount = 0;
            var items = await entity.ToPageListAsync(page, pageSize, totalCount);
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            return new PageModel<TEntity>
            {
                Page = page,
                PageSize = pageSize,
                Data = items,
                TotalCount = (int)totalCount,
                PageCount = totalPages,
                HasNextPages = page < totalPages,
                HasPrevPages = page - 1 > 0
            };
        }
    }
}
