using Dpwork.Core.Domain;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Dpwork.Core.Repository
{
    public interface IQueryRepository<TPrimaryKey, TEntity>: IRepository<TPrimaryKey, TEntity>
        where TEntity : class, IEntity<TPrimaryKey>
    {       
        TEntity Find(TPrimaryKey key);
        Task<TEntity> FindAsync(TPrimaryKey key, CancellationToken cancellationToken = default);
        IEnumerable<TEntity> FindAll();
        Task<IEnumerable<TEntity>> FindAllAsync(CancellationToken cancellationToken = default);
        //bool Exist(Expression<Func<TEntity, bool>> predicate);
        //Task<bool> ExistAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
        //IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate = null, Expression<Func<TEntity, bool>> orderPredicate = null);
        //Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate = null, Expression<Func<TEntity, bool>> orderPredicate = null, CancellationToken cancellationToken = default);
    }
}
