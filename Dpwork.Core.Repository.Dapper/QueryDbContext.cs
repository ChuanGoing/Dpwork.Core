using System;

namespace Dpwork.Core.Repository.Dapper
{
    public class QueryDbContext : DapperDbContext
    {
        public QueryDbContext(IServiceProvider serviceProvider, string conStr)
            : base(serviceProvider, conStr)
        {

        }
    }
}
