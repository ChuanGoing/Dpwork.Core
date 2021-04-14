using System;

namespace Dpwork.Core.Message.Args
{
    public class MessageEventArgs : EventArgs
    {
        public IMessage Message { get; }
        public MessageEventArgs(IMessage message)
        {
            Message = message;
        }
    }
}
