using Dpwork.Core.Message.Args;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Dpwork.Core.Message
{
    public interface IMessagePublisher
    {
        event EventHandler<MessagePublishedEventArgs> MessagePublished;

        void Publish<TMessage>(TMessage obj)
            where TMessage : IMessage;
        Task PublishAsync<TMessage>(TMessage obj, CancellationToken cancellationToken = default)
            where TMessage : IMessage;
    }
}
