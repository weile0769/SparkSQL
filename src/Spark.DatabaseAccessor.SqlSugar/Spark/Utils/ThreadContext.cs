using System.Collections.Concurrent;

namespace Spark.DatabaseAccessor.SqlSugar.Utils
{

    /// <summary>
    ///   线程共享上下文
    /// </summary>
    /// <typeparam name="T">共享对象类型</typeparam>
    internal class ThreadContext<T>
    {
        /// <summary>
        ///   共享对象字典集
        /// </summary>
        /// <returns></returns>
        static ConcurrentDictionary<string, ThreadLocal<T>> state = new ConcurrentDictionary<string, ThreadLocal<T>>();

        /// <summary>
        ///   设置共享对象
        /// </summary>
        /// <param name="name">共享对象名称</param>
        /// <param name="data">共享对象</param>
        public static void SetData(string name, T data) => state.GetOrAdd(name, _ => new ThreadLocal<T>()).Value = data;

        /// <summary>
        ///   获取共享对象
        /// </summary>
        /// <param name="name">共享对象名称</param>
        /// <returns>共享对象</returns>
        public static T? GetData(string name) => state.TryGetValue(name, out ThreadLocal<T>? data) ? data.Value : default;
    }
}