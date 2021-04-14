using System;

namespace Dpwork.Core.Repository.Dapper
{
    public class CommandDbContext : DapperDbContext
    {
        public CommandDbContext(IServiceProvider serviceProvider, string conStr)
            : base(serviceProvider, conStr)
        {

        }
    }
}
