namespace Dpwork.Core.Message.Args
{
    public class MessageHandledEventArgs : MessageEventArgs
    {
        public MessageHandledEventArgs(IMessage message) : base(message)
        { }
    }
}
