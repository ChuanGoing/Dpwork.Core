using Dpwork.Core.Domain;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Dpwork.Core.Repository
{
    public interface ICommandRepository<TPrimaryKey, TEntity> : IRepository<TPrimaryKey, TEntity>, ITransactionRepository
        where TEntity : class, IEntity<TPrimaryKey>
    {
        TPrimaryKey Create(TEntity entity);
        Task<TPrimaryKey> CreateAsync(TEntity entity, CancellationToken cancellationToken = default);
        void Update(TEntity entity);
        Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);       
        void DeleteByKey(TPrimaryKey key);
        Task DeleteByKeyAsync(TPrimaryKey key, CancellationToken cancellationToken = default);
        void DeleteByKeys(IEnumerable<TPrimaryKey> keys);
        Task DeleteByKeysAsync(IEnumerable<TPrimaryKey> keys, CancellationToken cancellationToken = default);
        void Delete(TEntity entity);
        Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
        void Delete(IEnumerable<TEntity> keys);
        Task DeleteAsync(IEnumerable<TEntity> keys, CancellationToken cancellationToken = default);
    }
}
