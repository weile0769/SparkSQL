using System.Diagnostics.CodeAnalysis;

namespace Spark.DatabaseAccessor.SqlSugar.Utils
{
    /// <summary>
    ///     参数校验帮助类
    /// </summary>
    internal static class SparkCheck
    {
        /// <summary>
        ///     判断泛型参数是否为空
        /// </summary>
        /// <typeparam name="T">泛型参数类型</typeparam>
        /// <param name="this">参数值</param>
        /// <returns>泛型参数</returns>
        /// <exception cref="ArgumentNullException">参数非空判断</exception>
        public static T NotNull<T>(T? @this)
        {
            if (@this == null)
            {
                throw new ArgumentNullException(nameof(T));
            }
            return @this;
        }


        /// <summary>
        ///     判断泛型参数是否为空
        /// </summary>
        /// <typeparam name="T">泛型参数类型</typeparam>
        /// <param name="this">参数值</param>
        /// <param name="parameterName">参数名称</param>
        /// <returns>泛型参数</returns>
        /// <exception cref="ArgumentNullException">参数非空判断</exception>
        public static T NotNull<T>(T? @this, [NotNull] string parameterName)
        {
            if (@this == null)
            {
                throw new ArgumentNullException(parameterName);
            }
            return @this;
        }

        /// <summary>
        ///     判断泛型参数是否为空
        /// </summary>
        /// <typeparam name="T">泛型参数类型</typeparam>
        /// <param name="this">参数值</param>
        /// <param name="parameterName">参数名称</param>
        /// <param name="message">自定义错误描述</param>
        /// <returns>泛型参数</returns>
        /// <exception cref="ArgumentNullException">参数非空判断</exception>
        public static T NotNull<T>(T? @this, [NotNull] string parameterName, string message)
        {
            if (@this == null)
            {
                throw new ArgumentNullException(parameterName, message);
            }

            return @this;
        }

        /// <summary>
        ///     判断字符串参数是否为空
        /// </summary>
        /// <param name="this">参数值</param>
        /// <param name="parameterName">参数名称</param>
        /// <param name="maxLength">最大长度</param>
        /// <param name="minLength">最小长度</param>
        /// <returns>字符串</returns>
        /// <exception cref="ArgumentException">参数非空判断或上下限超范围判断</exception>
        public static string NotNullOrEmpty(string @this, [NotNull] string parameterName, int maxLength = int.MaxValue, int minLength = 0)
        {
            if (@this.IsNullOrWhiteSpace())
            {
                throw new ArgumentException($"参数[{parameterName}]不能为空", parameterName);
            }

            if (@this.Length > maxLength)
            {
                throw new ArgumentException($"参数[{parameterName}]长度必须小于等于{maxLength}", parameterName);
            }

            if (minLength > 0 && @this.Length < minLength)
            {
                throw new ArgumentException($"参数[{parameterName}]长度必须大于等于{minLength}!", parameterName);
            }

            return @this;
        }

        /// <summary>
        ///     判断字符串参数是否为空
        /// </summary>
        /// <param name="this">参数值</param>
        /// <param name="parameterName">参数名称</param>
        /// <param name="maxLength">最大长度</param>
        /// <param name="minLength">最小长度</param>
        /// <returns>字符串</returns>
        /// <exception cref="ArgumentException">参数非空判断或上下限超范围判断</exception>
        public static string NotNullOrWhiteSpace(string? @this, [NotNull] string parameterName, int maxLength = int.MaxValue, int minLength = 0)
        {
            if (string.IsNullOrWhiteSpace(@this))
            {
                throw new ArgumentException($"参数[{parameterName}]不能为空", parameterName);
            }

            if (@this.Length > maxLength)
            {
                throw new ArgumentException($"参数[{parameterName}]长度必须小于等于{maxLength}", parameterName);
            }

            if (minLength > 0 && @this.Length < minLength)
            {
                throw new ArgumentException($"参数[{parameterName}]长度必须大于等于{minLength}!", parameterName);
            }

            return @this;
        }

        /// <summary>
        ///     判断集合参数是否为空
        /// </summary>
        /// <typeparam name="T">集合元素类型</typeparam>
        /// <param name="this">参数值</param>
        /// <param name="parameterName">参数名称</param>
        /// <returns>集合参数</returns>
        /// <exception cref="ArgumentException">参数非空判断</exception>
        public static ICollection<T> NotNullOrEmpty<T>(ICollection<T> @this, [NotNull] string parameterName)
        {
            if (@this.IsNullOrEmpty())
            {
                throw new ArgumentException(parameterName + " can not be null or empty!", parameterName);
            }

            return @this;
        }
    }
}
