namespace Dpwork.Core.Message.Args
{
    public class MessageReceivedEventArgs : MessageEventArgs
    {
        public MessageReceivedEventArgs(IMessage message) : base(message)
        { }
    }
}
