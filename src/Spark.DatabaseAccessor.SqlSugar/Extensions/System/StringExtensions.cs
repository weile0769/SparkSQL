using System.Diagnostics.CodeAnalysis;

namespace System
{
    /// <summary>
    ///     字符串扩展
    /// </summary>
    internal static class StringExtensions
    {
        /// <summary>
        ///     判断字符串是否为空或空字符串
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        internal static bool IsNullOrEmpty([NotNullWhen(false)] this string? @this)
        {
            return string.IsNullOrEmpty(@this);
        }

        /// <summary>
        ///     判断字符串是否不为空或不为空字符串
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        internal static bool IsNotNullOrEmpty([NotNullWhen(true)] this string? @this)
        {
            return !string.IsNullOrEmpty(@this);
        }

        /// <summary>
        ///     判断字符串是否为空或空字符串
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        internal static bool IsNullOrWhiteSpace([NotNullWhen(false)] this string? @this)
        {
            return string.IsNullOrWhiteSpace(@this);
        }

        /// <summary>
        ///     判断字符串是否不为空或不为空字符串
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        internal static bool IsNotNullOrWhiteSpace([NotNullWhen(true)] this string? @this)
        {
            return !string.IsNullOrWhiteSpace(@this);
        }
    }
}
