namespace Spark.DatabaseAccessor.Attributes
{
    /// <summary>
    ///     数据实体特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class SqlTableAttribute : Attribute
    {
        /// <summary>
        ///     表名
        /// </summary>
        public string? TableName { get; set; }

        /// <summary>
        ///     表描述
        /// </summary>
        public string? TableDescription { get; set; }

        /// <summary>
        ///   数据库连接标识
        /// <remarks>
        ///   如果不指定，则使用默认连接
        /// </remarks>
        /// </summary>
        public string? ConnectionName { get; set; }

        /// <summary>
        ///     禁止删除列
        /// </summary>
        public bool IsDisabledDelete { get; set; }

        /// <summary>
        ///     禁止更新数据库
        /// </summary>
        public bool IsDisabledUpdateAll { get; set; }

        /// <summary>
        ///     构造函数
        /// </summary>
        public SqlTableAttribute() { }

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="tableName">表名</param>
        public SqlTableAttribute(string tableName)
        {
            TableName = tableName;
        }

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="connectionName">数据库连接标识</param>
        public SqlTableAttribute(string tableName, string connectionName)
        {
            TableName = tableName;
            ConnectionName = connectionName;
        }
    }
}
