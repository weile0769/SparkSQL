using Spark.DatabaseAccessor.Attributes;
using Spark.DatabaseAccessor.Models;

namespace Spark.DatabaseAccessor.SqlSugar.WebAPI.Sample.Entities
{
    [SqlTable(TableName = "user")]
    public class User : RootEntity
    {
        [SqlColumn(ColumnName = "user_name")]
        public string UserName { get; set; } = string.Empty;

        [SqlColumn(ColumnName = "password", IsNullable = true)]
        public string Password { get; set; } = string.Empty;
    }
}
