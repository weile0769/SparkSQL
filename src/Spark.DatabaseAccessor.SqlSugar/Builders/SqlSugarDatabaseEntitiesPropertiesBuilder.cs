using Spark.DatabaseAccessor.Attributes;
using SqlSugar;
using System;
using System.Reflection;

namespace Spark.DatabaseAccessor.SqlSugar.Builders
{
    /// <summary>
    ///     实体特性SqlColumn配置构造器
    /// </summary>
    internal class SqlSugarDatabaseEntitiesPropertiesBuilder
    {
        /// <summary>
        ///     构造
        /// </summary>
        /// <param name="property">实体属性</param>
        /// <param name="column">数据列</param>
        internal static void Build(PropertyInfo property, EntityColumnInfo column)
        {
            var sqlColumn = property.GetCustomAttribute<SqlColumnAttribute>();
            if (sqlColumn == null) return;
            if (sqlColumn.IsIgnore == false)
            {
                column.DbColumnName = sqlColumn.ColumnName.IsNullOrEmpty() ? property.Name : sqlColumn.ColumnName;
                column.IsPrimarykey = sqlColumn.IsPrimaryKey;
                column.IsIdentity = sqlColumn.IsIdentity;
                column.ColumnDescription = sqlColumn.ColumnDescription;
                column.IsNullable = sqlColumn.IsNullable;
                column.Length = sqlColumn.Length;
                column.OldDbColumnName = sqlColumn.OldColumnName;
                column.DataType = sqlColumn.ColumnDataType;
                column.DecimalDigits = sqlColumn.DecimalDigits;
                column.OracleSequenceName = sqlColumn.OracleSequenceName;
                column.IsOnlyIgnoreInsert = sqlColumn.IsOnlyIgnoreInsert;
                column.IsEnableUpdateVersionValidation = sqlColumn.IsEnableUpdateVersionValidation;
                column.IsTranscoding = sqlColumn.IsTranscoding;
                column.SerializeDateTimeFormat = sqlColumn.SerializeDateTimeFormat;
                column.IsJson = sqlColumn.IsJson;
                column.NoSerialize = sqlColumn.NoSerialize;
                column.DefaultValue = sqlColumn.DefaultValue;
                column.IndexGroupNameList = sqlColumn.IndexGroupNameList;
                column.UIndexGroupNameList = sqlColumn.UniqueGroupNameList;
                column.IsOnlyIgnoreUpdate = sqlColumn.IsOnlyIgnoreUpdate;
                column.IsArray = sqlColumn.IsArray;
            }
            else
            {
                column.IsIgnore = true;
                column.NoSerialize = sqlColumn.NoSerialize;
                column.ColumnDescription = sqlColumn.ColumnDescription;
            }
        }
    }
}
