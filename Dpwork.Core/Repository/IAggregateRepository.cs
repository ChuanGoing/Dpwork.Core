using Dpwork.Core.Domain;

namespace Dpwork.Core.Repository
{
    public interface IAggregateRepository<TPrimaryKey, TAggregateRoot> : ICommandRepository<TPrimaryKey, TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot<TPrimaryKey>
    {
        TAggregateRoot Find(TPrimaryKey key);
    }
}
