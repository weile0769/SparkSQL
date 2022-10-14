namespace Spark.DatabaseAccessor.Contexts
{
    /// <summary>
    ///     数据库上下文提供器
    /// </summary>
    public interface IDatabaseContextProvider
    {
        /// <summary>
        ///   获取数据库连接上下文
        /// </summary>
        /// <param name="connectionName">数据库连接标识</param>
        /// <returns>数据库连接上下文</returns>
        object GetDatabaseContext(string? connectionName = default);
    }
}
