using System.Threading;
using System.Threading.Tasks;

namespace Dpwork.Core.Message
{
    public abstract class MessageHandle : IMessageHandle
    {
        public abstract bool CanHandle(IMessage message);

        public abstract Task<bool> HandleAsync(IMessage message, CancellationToken cancellationToken = default);
    }

    public abstract class MessageHandle<TMessage> : MessageHandle, IMessageHandle<TMessage>
        where TMessage : IMessage
    {
        public sealed override bool CanHandle(IMessage message)
        {
            return typeof(TMessage).Equals(message.GetType());
        }

        public sealed override Task<bool> HandleAsync(IMessage message, CancellationToken cancellationToken = default)
        => CanHandle(message) ? HandleAsync((TMessage)message) : Task.FromResult(false);

        public abstract Task<bool> HandleAsync(TMessage message, CancellationToken cancellationToken = default);
    }
}
