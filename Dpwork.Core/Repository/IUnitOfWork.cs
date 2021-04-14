using System.Data;

namespace Dpwork.Core.Repository
{
    public interface IUnitOfWork
    {
        void Begin(IsolationLevel level = IsolationLevel.Unspecified);
        void SaveChanges();
        void Failed();
    }
}
