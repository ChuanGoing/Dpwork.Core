using Dpwork.Core.Message;

namespace Dpwork.Core.Command
{
    public interface ICommandHandle : IMessageHandle
    {
    }

    public interface ICommandHandle<in TCommand> : ICommandHandle, IMessageHandle<TCommand>
        where TCommand : ICommand
    {
    }
}
