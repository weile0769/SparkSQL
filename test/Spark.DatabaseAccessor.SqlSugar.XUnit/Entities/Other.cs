using Spark.DatabaseAccessor.Attributes;
using Spark.DatabaseAccessor.Models;
using System.ComponentModel;

namespace Spark.DatabaseAccessor.SqlSugar.XUnit.Entities
{
    [SqlTable("user", "用户信息", ConnectionName = "spark1")]
    public class Other : RootEntity
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

    /// <summary>
    ///   性别
    /// </summary>
    public enum GenderEnum
    {
        /// <summary>
        ///     男
        /// </summary>
        [Description("男")] Boy = 1,

        /// <summary>
        ///     女
        /// </summary>
        [Description("女")] Girl = 2
    }
}