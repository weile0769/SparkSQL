using Spark.DatabaseAccessor.Attributes;
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
        internal static void Build(Type type, EntityInfo entity)
        {
            var sqltable = type.GetCustomAttribute<SqlTableAttribute>();
            if (sqltable == null) return;
            entity.DbTableName = sqltable.TableName.IsNullOrEmpty() ? type.Name : sqltable.TableName;
            entity.TableDescription = sqltable.TableDescription;
            entity.IsDisabledUpdateAll = sqltable.IsDisabledUpdateAll;
            entity.IsDisabledDelete = sqltable.IsDisabledDelete;
        }
    }
}
