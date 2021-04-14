namespace Dpwork.Core.Repository
{
    public interface IUnitOfWorkService
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
