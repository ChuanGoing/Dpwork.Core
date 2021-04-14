using System;

namespace Dpwork.Core.Message
{
    public interface IMessageBus : IMessagePublisher, IMessageSubscriber
    {
    }
}
