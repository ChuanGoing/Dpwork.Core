using System;
using System.Threading;
using System.Threading.Tasks;

namespace Dpwork.Core.Message
{
    /// <summary>
    /// 消息处理上下文
    /// </summary>
    public interface IMessageHandleContext
    {
        bool HandleRegistered(Type message, Type messageHandle);

        bool HandleRegistered<TMessage, TMessageHandle>()
            where TMessage : IMessage
            where TMessageHandle : IMessageHandle;

        void RegisterHandle(Type message, Type messageHandle);

        void RegisterHandle<TMessage, TMessageHandle>()
            where TMessage : IMessage
            where TMessageHandle : IMessageHandle;

        Task HandleAsync(IMessage message, CancellationToken cancellationToken = default);
    }
}
