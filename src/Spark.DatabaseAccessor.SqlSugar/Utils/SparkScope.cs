using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Spark.DatabaseAccessor.SqlSugar.Utils
{
    /// <summary>
    ///     创建作用域静态类
    /// </summary>
    public static partial class SparkScope
    {
        /// <summary>
        /// 创建一个作用域范围
        /// </summary>
        /// <param name="handler"></param>
        public static void Create(Action<IServiceScope> handler)
        {
            // 创建作用域
            using (var serviceScope = SparkCheck.NotNull(SparkContext.RootServices).CreateScope())
            {
                // 执行方法
                SparkCheck.NotNull(handler)(serviceScope);
            }
        }

        /// <summary>
        /// 创建一个作用域范围（异步）
        /// </summary>
        /// <param name="handler"></param>
        public static async Task CreateAsync(Func<IServiceScope, Task> handler)
        {
            // 创建作用域
            using (var serviceScope = SparkCheck.NotNull(SparkContext.RootServices).CreateAsyncScope())
            {
                // 执行方法
                await SparkCheck.NotNull(handler)(serviceScope);
            }
        }
    }
}
