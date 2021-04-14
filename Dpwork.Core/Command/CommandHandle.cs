using Dpwork.Core.Message;

namespace Dpwork.Core.Command
{
    public abstract class CommandHandle<TCommand> : MessageHandle<TCommand>, ICommandHandle<TCommand>
        where TCommand : ICommand
    {
    }
}
