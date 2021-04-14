using Dpwork.Core.Message;

namespace Dpwork.Core.Command
{
    public interface ICommandBus : IMessageBus, ICommandPublisher, ICommandSubscriber
    {
    }
}
