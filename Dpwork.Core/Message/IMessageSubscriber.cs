using Dpwork.Core.Message.Args;
using System;

namespace Dpwork.Core.Message
{
    public interface IMessageSubscriber
    {
        event EventHandler<MessageReceivedEventArgs> MessageReceived;

        void Subscribe<TMessage, TMessageHandle>()
            where TMessage : IMessage
            where TMessageHandle : IMessageHandle;
    }
}
