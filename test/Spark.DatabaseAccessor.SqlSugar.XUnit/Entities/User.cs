using Spark.DatabaseAccessor.Attributes;
using Spark.DatabaseAccessor.Models;

namespace Spark.DatabaseAccessor.SqlSugar.XUnit.Entities
{
    [SqlTable(TableName = "user")]
    public class User : RootEntity
    {
        /// <summary>
        ///   用户名
        /// </summary>
        [SqlColumn(ColumnName = "user_name", ColumnDescription = "用户名")]
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        ///   密码
        /// </summary>
        [SqlColumn(ColumnName = "password", ColumnDescription = "密码", IsNullable = true)]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        ///   性别
        /// </summary>
        [SqlColumn(ColumnName = "gender", ColumnDescription = "性别")]
        public GenderEnum Gender { get; set; } = GenderEnum.Boy;

        /// <summary>
        ///   生日
        /// </summary>
        [SqlColumn(ColumnName = "birthday", ColumnDescription = "生日日期", IsNullable = true)]
        public DateTime? Birthday { get; set; }
    }
}
