using System.Data;

namespace Dpwork.Core.Repository
{
    public interface ITransactionRepository
    {
        /// <summary>
        /// 打开事务
        /// </summary>
        /// <param name="level"></param>
        void BeginTransaction(IsolationLevel level = IsolationLevel.Unspecified);
        /// <summary>
        /// 提交事务
        /// </summary>
        void Commit();
        /// <summary>
        /// 事务回滚
        /// </summary>
        void Rollback();
    }
}
