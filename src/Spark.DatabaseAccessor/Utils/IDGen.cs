using System;

namespace Spark.DatabaseAccessor.Utils
{
    /// <summary>
    ///    ID 生成器静态类
    /// </summary>
    public static class IDGen
    {
        /// <summary>
        ///     生成顺序Int64逻辑
        /// </summary>
        /// <returns></returns>
        public static long SequentialInt64()
        {
            byte[] bytes = Guid.NewGuid().ToByteArray();
            return BitConverter.ToInt64(bytes, 0);
        }
    }
}
