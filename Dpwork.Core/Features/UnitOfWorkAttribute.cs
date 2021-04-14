using AspectCore.DynamicProxy;
using Dpwork.Core.Repository;
using System.Threading.Tasks;

namespace Dpwork.Core.Features
{
    /// <summary>
    /// Uow 切面实现事务
    /// </summary>
    public class UnitOfWorkAttribute : AbstractInterceptorAttribute
    {
        public override Task Invoke(AspectContext context, AspectDelegate next)
        {
            if (context.Implementation is IUnitOfWorkService uowService)
            {
                var uow = uowService.UnitOfWork;
                uow.Begin();
                var aspectDelegate = next(context);
                if (aspectDelegate.Exception != null)
                {
                    uow.Failed();
                    throw aspectDelegate.Exception;
                }
                else
                {
                    uow.SaveChanges();
                    return aspectDelegate;
                }
            }
            else
            {
                return next(context);
            }
        }
    }
}
