using SqlSugar;
using System;

namespace Spark.DatabaseAccessor.SqlSugar.Extensions
{
    /// <summary>
    ///     SQL语句参数格式化扩展类
    /// </summary>
    internal static class SqlProfilerExtensions
    {
        /// <summary>
        ///     格式化参数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public static string ParameterFormat(this string sql, SugarParameter[] parameters)
        {
            for (int i = parameters.Length - 1; i >= 0; i--)
            {
                if (parameters[i].DbType == System.Data.DbType.String
                    || parameters[i].DbType == System.Data.DbType.DateTime
                    || parameters[i].DbType == System.Data.DbType.Date
                    || parameters[i].DbType == System.Data.DbType.Time
                    || parameters[i].DbType == System.Data.DbType.DateTime2
                    || parameters[i].DbType == System.Data.DbType.DateTimeOffset
                    || parameters[i].DbType == System.Data.DbType.Guid
                    || parameters[i].DbType == System.Data.DbType.VarNumeric
                    || parameters[i].DbType == System.Data.DbType.AnsiStringFixedLength
                    || parameters[i].DbType == System.Data.DbType.AnsiString
                    || parameters[i].DbType == System.Data.DbType.StringFixedLength)
                {
                    sql = sql.Replace(parameters[i].ParameterName, "'" + parameters[i].Value?.ToString() + "'");
                }
                else if (parameters[i].DbType == System.Data.DbType.Boolean)
                {
                    sql = sql.Replace(parameters[i].ParameterName, Convert.ToBoolean(parameters[i].Value) ? "1" : "0");
                }
                else
                {
                    sql = sql.Replace(parameters[i].ParameterName, parameters[i].Value?.ToString());
                }
            }

            return sql;
        }

        /// <summary>
        ///     格式化参数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public static string ParameterFormat(this string sql, object parameters)
        {
            return ParameterFormat(sql, (SugarParameter[])parameters);
        }
    }
}
