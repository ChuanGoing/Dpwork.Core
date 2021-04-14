using System;

namespace Dpwork.Core.Message
{
    public interface IMessage
    {
        string Id { get; set; }
        DateTime Timestamp { get; set; }
        MessageMetadata Metadata { get; }
    }
}
