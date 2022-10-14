using Spark.DatabaseAccessor.Attributes;
using System;
using System.ComponentModel;

namespace Spark.DatabaseAccessor.Models
{
    /// <summary>
    ///     实体模型公共类型
    /// </summary>
    public class RootEntity
    {
        /// <summary>
        ///     主键
        /// </summary>
        [SqlColumn(IsPrimaryKey = true, ColumnName = "id", ColumnDescription = "主键")]
        public long Id { get; set; }

        /// <summary>
        ///     创建时间
        /// </summary>
        [SqlColumn(ColumnName = "create_time", ColumnDescription = "创建时间")]
        public DateTime CreationDate { get; set; } = DateTime.Now;

        /// <summary>
        ///     最后修改时间
        /// </summary>
        [SqlColumn(ColumnName = "modify_time", ColumnDescription = "最后修改时间")]
        public DateTime LastModified { get; set; } = DateTime.Now;

        /// <summary>
        ///     记录状态
        /// </summary>
        [SqlColumn(ColumnName = "record_state", ColumnDescription = "记录状态")]
        public RecordStateEnum RecordState { get; set; } = RecordStateEnum.Normal;
    }


    /// <summary>
    ///     实体模型公共泛型
    /// </summary>
    /// <typeparam name="Tkey"></typeparam>
    public class RootEntity<Tkey> where Tkey : IEquatable<Tkey>
    {
        /// <summary>
        ///     主键
        /// </summary>
        [SqlColumn(IsPrimaryKey = true, ColumnName = "id", ColumnDescription = "主键")]
        public Tkey? Id { get; set; }

        /// <summary>
        ///     创建时间
        /// </summary>
        [SqlColumn(ColumnName = "create_time", ColumnDescription = "创建时间")]
        public DateTime CreationDate { get; set; } = DateTime.Now;

        /// <summary>
        ///     最后修改时间
        /// </summary>
        [SqlColumn(ColumnName = "modify_time", ColumnDescription = "最后修改时间")]
        public DateTime LastModified { get; set; } = DateTime.Now;

        /// <summary>
        ///     记录状态
        /// </summary>
        [SqlColumn(ColumnName = "record_state", ColumnDescription = "记录状态")]
        public RecordStateEnum RecordState { get; set; } = RecordStateEnum.Normal;
    }

    /// <summary>
    ///     记录状态枚举
    /// </summary>
    public enum RecordStateEnum
    {
        /// <summary>
        ///     作废
        /// </summary>
        [Description("作废")]
        Delete = 0,
        /// <summary>
        ///     隐藏
        /// </summary>
        [Description("隐藏")]
        Hide = 1,
        /// <summary>
        ///     正常
        /// </summary>
        [Description("正常")]
        Normal = 2
    }
}
