using System.Threading;
using System.Threading.Tasks;

namespace Dpwork.Core.Message
{
    /// <summary>
    /// 消息处理接口
    /// </summary>
    public interface IMessageHandle 
    {
        bool CanHandle(IMessage message);

        Task<bool> HandleAsync(IMessage message, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// 消息处理接口
    /// (注意：内部通过依赖注入方式拿到消息处理现实类的实例)
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public interface IMessageHandle<in TMessage> : IMessageHandle
        where TMessage : IMessage
    {
        Task<bool> HandleAsync(TMessage message, CancellationToken cancellationToken = default);
    }
}
