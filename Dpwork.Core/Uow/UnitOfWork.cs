using Dpwork.Core.Repository;
using System.Data;

namespace Dpwork.Core.Uow
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbContext _dbContext;
        public UnitOfWork(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual void Begin(IsolationLevel level = IsolationLevel.Unspecified)
        {
            _dbContext.BeginTransaction(level);
        }

        public virtual void SaveChanges()
        {
            _dbContext.Commit();
        }

        public virtual void Failed()
        {
            _dbContext.Rollback();
        }
    }
}
