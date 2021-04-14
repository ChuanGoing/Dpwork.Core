using Dapper;
using System;

namespace Dpwork.Core.Repository.Dapper.Commands
{
    public class SqlCommand
    {
        public virtual string SqlString { get; private set; }
        public virtual DynamicParameters Parameters { get; private set; }
        public SqlCommand(string sqlString, DynamicParameters parameters)
        {
            if (string.IsNullOrWhiteSpace(sqlString))
            {
                throw new ArgumentNullException(nameof(sqlString), "无效的Sql语句");
            }
            SqlString = sqlString;
            Parameters = parameters;
        }
    }
}
