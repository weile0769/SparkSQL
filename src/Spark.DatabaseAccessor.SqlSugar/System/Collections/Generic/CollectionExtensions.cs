using System.Diagnostics.CodeAnalysis;

namespace System.Collections.Generic
{
    /// <summary>
    ///     ICollection集合容器扩展类
    /// </summary>
    internal static class CollectionExtensions
    {
        /// <summary>
        ///     判断容器集合是否为NULL或者集合数量小于等于0
        /// </summary>
        /// <typeparam name="TSource">泛型类型</typeparam>
        /// <param name="source">源集合</param>
        /// <returns>true/false</returns>
        internal static bool IsNullOrEmpty<TSource>([NotNullWhen(false)] this ICollection<TSource> source)
        {
            return source == null || source.Count <= 0;
        }

        /// <summary>
        ///     判断容器集合是否非NULL且集合数量大于0
        /// </summary>
        /// <typeparam name="TSource">泛型类型</typeparam>
        /// <param name="source">源集合</param>
        /// <returns>true/false</returns>
        internal static bool IsNotNullOrEmpty<TSource>([NotNullWhen(true)] this ICollection<TSource> source)
        {
            return source != null && source.Count > 0;
        }
    }
}
