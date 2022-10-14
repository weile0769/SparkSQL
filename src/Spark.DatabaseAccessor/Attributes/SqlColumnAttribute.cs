using System;

namespace Spark.DatabaseAccessor.Attributes
{
    /// <summary>
    ///     数据列特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class SqlColumnAttribute : Attribute
    {
        /// <summary>
        ///     列名
        /// </summary>
        public string? ColumnName { get; set; }

        /// <summary>
        ///     列描述
        /// </summary>
        public string? ColumnDescription { get; set; }

        /// <summary>
        ///     自增列
        ///     如果是Oracle请设置OracleSequenceName 设置后和自增一样使用
        /// </summary>
        public bool IsIdentity { get; set; }

        /// <summary>
        ///     主键列
        /// </summary>
        public bool IsPrimaryKey { get; set; }

        /// <summary>
        ///     ORM忽略该列
        /// </summary>
        public bool IsIgnore { get; set; }

        /// <summary>
        ///     插入操作忽略该列
        /// </summary>
        public bool IsOnlyIgnoreInsert { get; set; }

        /// <summary>
        ///     更新操作忽略该列
        /// </summary>
        public bool IsOnlyIgnoreUpdate { get; set; }

        /// <summary>
        ///     设置Oracle序列，设置后该列等同于自增列
        /// </summary>
        public string? OracleSequenceName { get; set; }

        /// <summary>
        ///     数据列长度
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        ///     可空列
        /// </summary>
        public bool IsNullable { get; set; }

        /// <summary>
        ///     数据列类型
        /// </summary>
        public string? ColumnDataType { get; set; }

        /// <summary>
        ///     精度 如 decimal(18,2) length=18,DecimalDigits=2
        /// </summary>
        public int DecimalDigits { get; set; }

        /// <summary>
        ///     Json字符串数据列
        /// </summary>
        public bool IsJson { get; set; }

        /// <summary>
        ///     数组数据列
        /// </summary>
        public bool IsArray { get; set; }

        /// <summary>
        ///     Json字符串数据列序列化时时间格式
        /// </summary>
        public string? SerializeDateTimeFormat { get; set; }

        /// <summary>
        ///     修改列名用，这样不会新增或者删除列
        /// </summary>
        public string? OldColumnName { get; set; }

        /// <summary>
        ///     数据版本列
        /// </summary>
        public bool IsEnableUpdateVersionValidation { get; set; }

        /// <summary>
        ///     是否Base64编码
        /// </summary>
        public bool IsTranscoding { get; set; }

        /// <summary>
        ///     无序列化
        /// </summary>
        public bool NoSerialize { get; set; }

        /// <summary>
        ///     默认值
        /// </summary>
        public string? DefaultValue { get; set; }

        /// <summary>
        ///     创建索引用
        /// </summary>
        public string[]? IndexGroupNameList { get; set; }

        /// <summary>
        ///     创建唯一索引
        /// </summary>
        public string[]? UniqueGroupNameList { get; set; }
    }
}
