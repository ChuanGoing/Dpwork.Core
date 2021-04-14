using Dpwork.Core.Dependence;
using MySqlConnector;
using System.Data;

namespace Dpwork.Core.Repository.Dapper
{
    public class MySqlDbBuilder : IDbBuilder, ISingleton
    {
        public MySqlDbBuilder()
        {
        }

        public IDbConnection CreateConnection(string connStr)
        {
            return new MySqlConnection(connStr);
        }
    }
}
