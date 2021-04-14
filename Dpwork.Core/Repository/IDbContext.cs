using Dpwork.Core.Domain;
using System.Data;

namespace Dpwork.Core.Repository
{
    public interface IDbContext
    {
        IRepository<TPrimaryKey, TEntity> GetRepository<TPrimaryKey, TEntity>() where TEntity : class, IEntity<TPrimaryKey>;

        TRepository GetRepository<TRepository>() where TRepository : IRepository;

        void BeginTransaction(IsolationLevel level);
        void Commit();
        void Rollback();
    }
}
