﻿using System.Collections.Concurrent;

namespace System.Threading
{
    /// <summary>
    ///   异步线程共享上下文
    /// </summary>
    /// <typeparam name="T">共享对象类型</typeparam>
    public class AsyncContext<T>
    {
        /// <summary>
        ///   共享对象字典集
        /// </summary>
        /// <returns></returns>
        static ConcurrentDictionary<string, AsyncLocal<T>> state = new ConcurrentDictionary<string, AsyncLocal<T>>();

        /// <summary>
        ///   设置共享对象
        /// </summary>
        /// <param name="name">共享对象名称</param>
        /// <param name="data">共享对象</param>
        public static void SetData(string name, T data) => state.GetOrAdd(name, _ => new AsyncLocal<T>()).Value = data;

        /// <summary>
        ///   获取共享对象
        /// </summary>
        /// <param name="name">共享对象名称</param>
        /// <returns>共享对象</returns>
        public static T? GetData(string name) => state.TryGetValue(name, out AsyncLocal<T>? data) ? data.Value : default;
    }
}