using System;

namespace Dpwork.Core.Message.Args
{
    public class MessagePublishedEventArgs : MessageEventArgs
    {
        public MessagePublishedEventArgs(IMessage message) : base(message)
        { }
    }
}
