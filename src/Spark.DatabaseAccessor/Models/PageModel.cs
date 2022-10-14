using System.Collections.Generic;

namespace Spark.DatabaseAccessor.Models
{
    /// <summary>
    ///     通用分页信息类
    /// </summary>
    public class PageModel<T>
    {
        /// <summary>
        ///     页码
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        ///     页容量
        /// </summary>
        public int PageSize { set; get; }

        /// <summary>
        ///     总页数
        /// </summary>
        public int PageCount { get; set; } = 0;

        /// <summary>
        ///     总条数
        /// </summary>
        public int TotalCount { get; set; } = 0;

        /// <summary>
        ///     返回数据
        /// </summary>
        public List<T>? Data { get; set; }

        /// <summary>
        /// 是否有上一页
        /// </summary>
        public bool HasPrevPages { get; set; }

        /// <summary>
        /// 是否有下一页
        /// </summary>
        public bool HasNextPages { get; set; }
    }
}
