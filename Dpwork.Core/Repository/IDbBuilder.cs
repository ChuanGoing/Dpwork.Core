using System.Data;

namespace Dpwork.Core.Repository
{
    public interface IDbBuilder 
    {
        IDbConnection CreateConnection(string connStr);
    }
}
