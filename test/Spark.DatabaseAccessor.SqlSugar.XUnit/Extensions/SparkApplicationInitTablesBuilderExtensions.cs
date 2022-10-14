﻿using Microsoft.AspNetCore.Builder;
using Spark.DatabaseAccessor.SqlSugar.XUnit.Entities;

namespace Spark.DatabaseAccessor.SqlSugar.XUnit.Extensions
{
    /// <summary>
    ///   应用数据结构扩展类
    /// </summary>
    public static class SparkApplicationInitTablesBuilderExtensions
    {
        /// <summary>
        ///   表初始化服务
        /// </summary>
        /// <param name="builder"></param>
        public static void UseApplicationInitTables(this IApplicationBuilder builder)
        {
            Database.Context.CodeFirst.InitTables<User>();
        }
    }
}
