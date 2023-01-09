using Spark.DatabaseAccessor.Attributes;
using Spark.DatabaseAccessor.Options;
using SqlSugar;
using System;
using System.Reflection;

namespace Spark.DatabaseAccessor.SqlSugar.Builders
{
    /// <summary>
    ///     实体特性SqlTable配置构造器
    /// </summary>
    internal class SqlSugarDatabaseEntitiesTypesBuilder
    {
        /// <summary>
        ///     构造
        /// </summary>
        /// <param name="type">实体类型</param>
        /// <param name="entity">实体属性</param>
        /// <param name="databaseOptions">数据库驱动全局配置</param>
        internal static void Build(Type type, EntityInfo entity, DatabaseConnections databaseOptions)
        {
            var sqltable = type.GetCustomAttribute<SqlTableAttribute>();
            if (sqltable == null)
            {
                if (databaseOptions.Schema.IsNotNullOrEmpty())
                {
                    entity.DbTableName = $"{databaseOptions.Schema}.{entity.DbTableName}";
                }
            }
            else
            {
                var tableName = sqltable.TableName.IsNullOrEmpty() ? entity.DbTableName : sqltable.TableName;
                if (databaseOptions.Schema.IsNotNullOrEmpty())
                {
                    tableName = $"{databaseOptions.Schema}.{tableName}";
                }
                entity.DbTableName = tableName;
                entity.TableDescription = sqltable.TableDescription;
                entity.IsDisabledUpdateAll = sqltable.IsDisabledUpdateAll;
                entity.IsDisabledDelete = sqltable.IsDisabledDelete;
            }
        }
    }
}
