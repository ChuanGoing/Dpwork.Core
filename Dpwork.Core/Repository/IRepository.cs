using Dpwork.Core.Domain;

namespace Dpwork.Core.Repository
{
    public interface IRepository
    {
        void SetDbContext<TDbContext>(TDbContext dbContext) where TDbContext : IDbContext;
    }

    public interface IRepository<TPrimaryKey, TEntity>: IRepository
        where TEntity : class, IEntity<TPrimaryKey>
    {
    }
}
